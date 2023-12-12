using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Transactions;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using WebFreight.Web.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

using Logitude.Customs.Data.DataContracts;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using System.Text.RegularExpressions;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using WebFreight.Web.CustomWebServices.BL.XLSImport;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class DeclarationSupplierInvoicesController : ApiController
    {
        public HttpResponseMessage GetSupplierInvoicesPMsForDeclaration(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceQueryService supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
                List<SupplierInvoicePM> supplierInvoices = supplierInvoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant,true);

                return Request.CreateResponse(HttpStatusCode.OK, supplierInvoices);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Delete(string declarationId, int counterKey )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticationOnTenant(tenant);

                       

                        ICustomContext MyContext = CustomContext.GetContext(tenant);

                        SupplierInvoiceQueryService query = new SupplierInvoiceQueryService(MyContext);
                        SupplierInvoicePM entityPM = query.GetSingle(declarationId, counterKey, false, false);
                        SupplierInvoiceUpdateService service = new SupplierInvoiceUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

                        #region composition handling
                        //invoice items
                        SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(MyContext);

                        List<SupplierInvoiceItemPM> supplierInvoiceItemsChangeset = supplierInvoiceItemQueryService.GetSupplierInvoiceItemsByInvoice(entityPM.DeclarationId, entityPM.InvoiceCounterKey);//ChangeSet.GetAssociatedChanges(entityPM, d => d.SupplierInvoiceItems).Cast<SupplierInvoiceItemPM>().ToList();
                        foreach (SupplierInvoiceItemPM item in supplierInvoiceItemsChangeset)
                        {
                            SupplierInvoiceItemPM deletedItem = new SupplierInvoiceItemPM()
                            {
                                CounterKey = item.CounterKey,
                                DeclarationId = item.DeclarationId,
                                LineNumber = item.LineNumber,
                                Tenant = item.Tenant,
                                ChangeSetOp = ChangeSetOperation.Delete,
                            };
                            entityPM.DeletedSupplierInvoiceItems.Add(deletedItem);


                            //connected to declarations
                            SupplierInvoiceItemsConDeclarQueryService supplierInvoiceItemsConDeclarQueryService = new SupplierInvoiceItemsConDeclarQueryService(MyContext);
                            List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemsConnectedDeclarationChangeset = supplierInvoiceItemsConDeclarQueryService.GetSupplierInvoiceItemsConDeclarePMsForInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemsConDeclars).Cast<SupplierInvoiceItemsConDeclarPM>().ToList();

                            foreach (SupplierInvoiceItemsConDeclarPM itemConnected in supplierInvoiceItemsConnectedDeclarationChangeset)
                            {
                                SupplierInvoiceItemsConDeclarPM deletedItemConnected = new SupplierInvoiceItemsConDeclarPM()
                                {
                                    InvoiceCounterKey = itemConnected.InvoiceCounterKey,
                                    DeclarationId = itemConnected.DeclarationId,
                                    LineNumber = itemConnected.LineNumber,
                                    InvoiceItemLineNumber = itemConnected.InvoiceItemLineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                deletedItem.DeletedSupplierInvoiceItemsConDeclars.Add(deletedItemConnected);
                            }
                            //taxes
                            SupplierInvoiceItemsTaxQueryService supplierInvoiceItemsTaxQuery = new SupplierInvoiceItemsTaxQueryService(MyContext);
                            List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemsTaxChangeset = supplierInvoiceItemsTaxQuery.GetSupplierInvoiceItemsTaxForInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);  //ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemTaxes).Cast<SupplierInvoiceItemsTaxPM>().ToList();

                            foreach (SupplierInvoiceItemsTaxPM itemTax in supplierInvoiceItemsTaxChangeset)
                            {
                                SupplierInvoiceItemsTaxPM deletedItemTax = new SupplierInvoiceItemsTaxPM()
                                {
                                    InvoiceCounterKey = itemTax.InvoiceCounterKey,
                                    DeclarationId = itemTax.DeclarationId,
                                    LineNumber = itemTax.LineNumber,
                                    TaxTypeCode = itemTax.TaxTypeCode,
                                    Tenant = itemTax.Tenant,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                deletedItem.DeletedSupplierInvoiceItemTaxes.Add(deletedItemTax);
                            }
                            //certificates
                            SupplierInvioceItemCertificatQueryService supplierInvioceItemCertificatQuery = new SupplierInvioceItemCertificatQueryService(MyContext);
                            List<SupplierInvioceItemCertificatPM> supplierInvoiceItemsCertificateChangeset = supplierInvioceItemCertificatQuery.GetSupplierInvioceItemCertificatesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvioceItemCertificats).Cast<SupplierInvioceItemCertificatPM>().ToList();

                            foreach (SupplierInvioceItemCertificatPM itemCer in supplierInvoiceItemsCertificateChangeset)
                            {
                                SupplierInvioceItemCertificatPM deletedItemCer = new SupplierInvioceItemCertificatPM()
                                {
                                    InvoiceCounterKey = itemCer.InvoiceCounterKey,
                                    DeclarationId = itemCer.DeclarationId,
                                    LineNumber = itemCer.LineNumber,
                                    ItemCertificateCounterKey = itemCer.ItemCertificateCounterKey,
                                    Tenant = itemCer.Tenant,

                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                deletedItem.DeletedSupplierInvioceItemCertificats.Add(deletedItemCer);
                            }

                            //modifications
                            SupplierInvoiceItemsModQueryService supplierInvoiceItemsModQuery = new SupplierInvoiceItemsModQueryService(MyContext);
                            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsModificationChangeset = supplierInvoiceItemsModQuery.GetSupplierInvoiceItemsModsForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemsMods).Cast<SupplierInvoiceItemsModPM>().ToList();

                            foreach (SupplierInvoiceItemsModPM itemMod in supplierInvoiceItemsModificationChangeset)
                            {
                                SupplierInvoiceItemsModPM deletedItemMod = new SupplierInvoiceItemsModPM()
                                {
                                    InvoiceCounterKey = itemMod.InvoiceCounterKey,
                                    DeclarationId = itemMod.DeclarationId,
                                    LineNumber = itemMod.LineNumber,
                                    TypeCode = itemMod.TypeCode,
                                    Tenant = itemMod.Tenant,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    ModificationCounterKey = itemMod.ModificationCounterKey,
                                };

                                deletedItem.DeletedSupplierInvoiceItemsMods.Add(deletedItemMod);
                            }

                            //serialnumbers
                            SupplierInvoiceItemsSerialNumQueryService supplierInvoiceItemsSerialNumQuery = new SupplierInvoiceItemsSerialNumQueryService(MyContext);
                            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumberChangeset = supplierInvoiceItemsSerialNumQuery.GetSupplierInvoiceItemsSerialNumsForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemsSerialNums).Cast<SupplierInvoiceItemsSerialNumPM>().ToList();

                            foreach (SupplierInvoiceItemsSerialNumPM itemSerial in supplierInvoiceItemsSerialNumberChangeset)
                            {
                                SupplierInvoiceItemsSerialNumPM deletedItemSerial = new SupplierInvoiceItemsSerialNumPM()
                                {
                                    InvoiceCounterKey = itemSerial.InvoiceCounterKey,
                                    DeclarationId = itemSerial.DeclarationId,
                                    LineNumber = itemSerial.LineNumber,
                                    TypeCode = itemSerial.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = itemSerial.InvoiceItemLineNumber,
                                    SerialNumber = itemSerial.SerialNumber,
                                    Tenant = itemSerial.Tenant,

                                };

                                deletedItem.DeletedSupplierInvoiceItemsSerialNums.Add(deletedItemSerial);
                            }

                            //product identification
                            SupplierInvoiceItemsProdIdentQueryService supplierInvoiceItemsProdIdentQuery = new SupplierInvoiceItemsProdIdentQueryService(MyContext);
                            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProductIdentificationChangeset = supplierInvoiceItemsProdIdentQuery.GetSupplierInvoiceItemsProdIdentsForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemsProdIdents).Cast<SupplierInvoiceItemsProdIdentPM>().ToList();

                            foreach (SupplierInvoiceItemsProdIdentPM itemProduct in supplierInvoiceItemsProductIdentificationChangeset)
                            {
                                SupplierInvoiceItemsProdIdentPM deletedItemProduct = new SupplierInvoiceItemsProdIdentPM()
                                {
                                    InvoiceCounterKey = itemProduct.InvoiceCounterKey,
                                    DeclarationId = itemProduct.DeclarationId,
                                    LineNumber = itemProduct.LineNumber,
                                    TypeCode = itemProduct.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = itemProduct.InvoiceItemLineNumber,
                                    Identification = itemProduct.Identification,
                                    Tenant = itemProduct.Tenant,
                                };

                                deletedItem.DeletedSupplierInvoiceItemsProdIdents.Add(deletedItemProduct);
                            }

                            //Descriptions
                            SupplierInvoiceItemsDescriptQueryService supplierInvoiceItemsDescriptQuery = new SupplierInvoiceItemsDescriptQueryService(MyContext);
                            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptChangeset = supplierInvoiceItemsDescriptQuery.GetSupplierInvoiceItemsDescriptsForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemsDescripts).Cast<SupplierInvoiceItemsDescriptPM>().ToList();

                            foreach (SupplierInvoiceItemsDescriptPM itemDesc in supplierInvoiceItemsDescriptChangeset)
                            {
                                SupplierInvoiceItemsDescriptPM deletedItemDesc = new SupplierInvoiceItemsDescriptPM()
                                {
                                    InvoiceCounterKey = itemDesc.InvoiceCounterKey,
                                    DeclarationId = itemDesc.DeclarationId,
                                    LineNumber = itemDesc.LineNumber,
                                    TypeCode = itemDesc.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = itemDesc.InvoiceItemLineNumber,
                                    Description = itemDesc.Description,
                                    Tenant = itemDesc.Tenant,
                                };

                                deletedItem.DeletedSupplierInvoiceItemsDescripts.Add(deletedItemDesc);
                            }
                            // process types
                            SupplierInvoiceItemProcesTypeQueryService supplierInvoiceItemProcesTypeQuery = new SupplierInvoiceItemProcesTypeQueryService(MyContext);
                            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemsProcessTypeChangeset = supplierInvoiceItemProcesTypeQuery.GetSupplierInvoiceItemProcesTypesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemProcesTypes).Cast<SupplierInvoiceItemProcesTypePM>().ToList();

                            foreach (SupplierInvoiceItemProcesTypePM itemProccess in supplierInvoiceItemsProcessTypeChangeset)
                            {
                                SupplierInvoiceItemProcesTypePM deletedItemProcess = new SupplierInvoiceItemProcesTypePM()
                                {
                                    InvoiceCounterKey = itemProccess.InvoiceCounterKey,
                                    DeclarationId = itemProccess.DeclarationId,
                                    LineNumber = itemProccess.LineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = itemProccess.InvoiceItemLineNumber,
                                    ProcessTypeCode = itemProccess.ProcessTypeCode,
                                    Tenant = itemProccess.Tenant,
                                };

                                deletedItem.DeletedSupplierInvoiceItemProcesTypes.Add(deletedItemProcess);
                            }
                            //items levies
                            SupplierInvoiceItemsLevyQueryService supplierInvoiceItemsLevyQuery = new SupplierInvoiceItemsLevyQueryService(MyContext);
                            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyChangeset = supplierInvoiceItemsLevyQuery.GetSupplierInvoiceItemsLeviesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemLevies).Cast<SupplierInvoiceItemsLevyPM>().ToList();

                            foreach (SupplierInvoiceItemsLevyPM itemlevy in supplierInvoiceItemsLevyChangeset)
                            {
                                SupplierInvoiceItemsLevyPM deletedItemLevy = new SupplierInvoiceItemsLevyPM()
                                {
                                    InvoiceCounterKey = itemlevy.InvoiceCounterKey,
                                    DeclarationId = itemlevy.DeclarationId,
                                    LineNumber = itemlevy.LineNumber,
                                    InvoiceItemLineNumber = itemlevy.InvoiceItemLineNumber,
                                    Tenant = itemlevy.Tenant,
                                    TradeLevyExamptCode = itemlevy.TradeLevyExamptCode,
                                    TradeLevyNumber = itemlevy.TradeLevyNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                deletedItem.DeletedSupplierInvoiceItemLevies.Add(deletedItemLevy);
                            }

                            SupplierInvoiceItemModVehicleQueryService supplierInvoiceItemModVehicleQueryService = new SupplierInvoiceItemModVehicleQueryService(MyContext);
                            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicleChangeSet = supplierInvoiceItemModVehicleQueryService.GetSupplierInvoiceItemModVehiclesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);
                            foreach (SupplierInvoiceItemModVehiclePM itemModVehicle in supplierInvoiceItemModVehicleChangeSet)
                            {
                                SupplierInvoiceItemModVehiclePM deletedItemModVehicle = new SupplierInvoiceItemModVehiclePM()
                                {
                                    InvoiceCounterKey = itemModVehicle.InvoiceCounterKey,
                                    DeclarationId = itemModVehicle.DeclarationId,
                                    AdjustmentTypeCode = itemModVehicle.AdjustmentTypeCode,
                                    InvoiceItemLineNumber = itemModVehicle.InvoiceItemLineNumber,
                                    Tenant = itemModVehicle.Tenant,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                deletedItem.DeletedSupplierInvoiceItemModVehicles.Add(deletedItemModVehicle);
                            }

                            SuppInvoiceItemsAbachStatementQueryService suppInvoiceItemsAbachStatementQueryService = new SuppInvoiceItemsAbachStatementQueryService(MyContext);
                            List< SuppInvoiceItemsAbachStatementPM > suppInvoiceItemsAbachStatementChangeSet = suppInvoiceItemsAbachStatementQueryService.GetSuppInvoiceItemsAbachStatementsForSupplierInvoiceWithSpecificKeys(item.DeclarationId, item.CounterKey,new List<int>{ item.LineNumber}, item.Tenant);
                            foreach (SuppInvoiceItemsAbachStatementPM itemAbach in suppInvoiceItemsAbachStatementChangeSet)
                            {
                                SuppInvoiceItemsAbachStatementPM deletedItemAbach = new SuppInvoiceItemsAbachStatementPM()
                                {
                                    InvoiceCounterKey = itemAbach.InvoiceCounterKey,
                                    DeclarationId = itemAbach.DeclarationId,
                                     InvoiceItemLineNumber = itemAbach.InvoiceItemLineNumber,
                                    Tenant = itemAbach.Tenant,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    IsStatementInd= itemAbach.IsStatementInd,
                                    SequenceNumeric= itemAbach.SequenceNumeric,
                                    StatementTypeCode = itemAbach.StatementTypeCode
                                };

                                deletedItem.DeletedSuppInvoiceItemsAbachStatements.Add(deletedItemAbach);
                            }


                            SupplierInvoiceItemsPriceQueryService supplierInvoiceItemsPriceQueryService = new SupplierInvoiceItemsPriceQueryService(MyContext);
                            List<SupplierInvoiceItemsPricePM> supplierInvoiceItemsPriceChangeSet = supplierInvoiceItemsPriceQueryService.GetSupplierInvoiceItemsPricesForSupplierInvoiceWithSpecificKeys(item.DeclarationId, item.CounterKey, new List<int> { item.LineNumber }, item.Tenant);
                            foreach (SupplierInvoiceItemsPricePM itemPrice in supplierInvoiceItemsPriceChangeSet)
                            {
                                SupplierInvoiceItemsPricePM deletedItemPrice = new SupplierInvoiceItemsPricePM()
                                {
                                    InvoiceCounterKey = itemPrice.InvoiceCounterKey,
                                    DeclarationId = itemPrice.DeclarationId,
                                    InvoiceItemLineNumber = itemPrice.InvoiceItemLineNumber,
                                    Tenant = itemPrice.Tenant,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                   AdditionalPrice= itemPrice.AdditionalPrice,
                                   AdditionalPriceTypeCode= itemPrice.AdditionalPriceTypeCode,
                                   LineNumber= itemPrice.LineNumber
                                };

                                deletedItem.DeletedSupplierInvoiceItemsPrices.Add(deletedItemPrice);
                            }

                            // Vehicles

                            SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQuery = new SupplierInvoiceItemVehicleQueryService(MyContext);
                            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehicleChangeset = supplierInvoiceItemVehicleQuery.GetSupplierInvoiceItemVehiclesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);//ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemVehicles).Cast<SupplierInvoiceItemVehiclePM>().ToList();

                            foreach (SupplierInvoiceItemVehiclePM vehicle in supplierInvoiceItemVehicleChangeset)
                            {
                                SupplierInvoiceItemVehiclePM deletedItemVehicle = new SupplierInvoiceItemVehiclePM()
                                {
                                    InvoiceCounterKey = vehicle.InvoiceCounterKey,
                                    DeclarationId = vehicle.DeclarationId,
                                    LineNumber = vehicle.LineNumber,
                                    InvoiceItemLineNumber = vehicle.InvoiceItemLineNumber,
                                    Tenant = vehicle.Tenant,
                                    RichbitFileNumber = vehicle.RichbitFileNumber,
                                    VehicleChassisNumber = vehicle.VehicleChassisNumber,
                                    SequenceNumeric = vehicle.SequenceNumeric,
                                    VehicleId = vehicle.VehicleId,
                                    VehicleTypeCode = vehicle.VehicleTypeCode,
                                    ExcludeFromInterface = vehicle.ExcludeFromInterface,
                                    IdentifierID = vehicle.IdentifierID,                     
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                deletedItem.DeletedSupplierInvoiceItemVehicles.Add(deletedItemVehicle);

                                SupplierInvoiceItemVehicleAddQueryService supplierInvoiceItemVehicleAddQueryService = new SupplierInvoiceItemVehicleAddQueryService(MyContext);
                                List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAddChangeSet = supplierInvoiceItemVehicleAddQueryService.GetSupplierInvoiceItemVehicleAddsForSupplierInvoiceItemVehicle(vehicle.DeclarationId, vehicle.InvoiceCounterKey, vehicle.InvoiceItemLineNumber, vehicle.LineNumber, vehicle.Tenant);

                                foreach (SupplierInvoiceItemVehicleAddPM itemVehicleAdd in supplierInvoiceItemVehicleAddChangeSet)
                                {
                                    SupplierInvoiceItemVehicleAddPM itemPM = new SupplierInvoiceItemVehicleAddPM()
                                    {
                                        DeclarationId = itemVehicleAdd.DeclarationId,
                                        InvoiceCounterKey = itemVehicleAdd.InvoiceCounterKey,
                                        InvoiceItemLineNumber = itemVehicleAdd.InvoiceItemLineNumber,
                                        LineNumber = itemVehicleAdd.LineNumber,
                                        ChangeSetOp = ChangeSetOperation.Delete,
                                    };
                                    deletedItemVehicle.DeletedSupplierInvoiceItemVehicleAdds.Add(itemPM);
                                }

                                SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModQueryService = new SupplierInvoiceItemVehicleModQueryService(MyContext);
                                List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleModChangeSet = supplierInvoiceItemVehicleModQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoiceForVehicle(vehicle.DeclarationId, vehicle.InvoiceCounterKey, vehicle.InvoiceItemLineNumber, vehicle.LineNumber, vehicle.Tenant);

                                foreach (SupplierInvoiceItemVehicleModPM itemVehicleMod in supplierInvoiceItemVehicleModChangeSet)
                                {
                                    SupplierInvoiceItemVehicleModPM itemPM = new SupplierInvoiceItemVehicleModPM()
                                    {
                                        DeclarationId = itemVehicleMod.DeclarationId,
                                        InvoiceCounterKey = itemVehicleMod.InvoiceCounterKey,
                                        InvoiceItemLineNumber = itemVehicleMod.InvoiceItemLineNumber,
                                        LineNumber = itemVehicleMod.LineNumber,
                                        VehicleLineNumber = itemVehicleMod.VehicleLineNumber,
                                        ChangeSetOp = ChangeSetOperation.Delete,
                                    };
                                    deletedItemVehicle.DeletedSupplierInvoiceItemVehicleMods.Add(itemPM);
                                }



                            }

                        }
                        //FreightAmounts
                        SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(MyContext);
                        List<SupplierInvoiceFreightAmountPM> supplierInvoiceFreightAmountChangeset = supplierInvoiceFreightAmountQueryService.GetSupplierInvoiceFreightAmountsByInvoice(entityPM.DeclarationId, entityPM.InvoiceCounterKey);//ChangeSet.GetAssociatedChanges(entityPM, d => d.SupplierInvoiceFreightAmounts).Cast<SupplierInvoiceFreightAmountPM>().ToList();
                        foreach (SupplierInvoiceFreightAmountPM item in supplierInvoiceFreightAmountChangeset)
                        {
                            SupplierInvoiceFreightAmountPM deletedItem = new SupplierInvoiceFreightAmountPM()
                            {
                                InvoiceCounterKey = item.InvoiceCounterKey,
                                DeclarationId = item.DeclarationId,
                                Tenant = item.Tenant,
                                Amount = item.Amount,
                                CurrencyTypeCode = item.CurrencyTypeCode,
                                ChangeSetOp = ChangeSetOperation.Delete,
                            };
                            entityPM.DeletedSupplierInvoiceFreightAmounts.Add(deletedItem);
                        }


                        //invoice modifications
                        SupplierInvoiceModificationQueryService supplierInvoiceModificationQuery = new SupplierInvoiceModificationQueryService(MyContext);

                        List<SupplierInvoiceModificationPM> supplierInvoiceModificationsChangeset = supplierInvoiceModificationQuery.GetSupplierInvoiceModificationsForInvoice(entityPM.DeclarationId, entityPM.InvoiceCounterKey);//ChangeSet.GetAssociatedChanges(entityPM, d => d.SupplierInvoiceModifications).Cast<SupplierInvoiceModificationPM>().ToList();
                        foreach (SupplierInvoiceModificationPM item in supplierInvoiceModificationsChangeset)
                        {
                            SupplierInvoiceModificationPM deletedItem = new SupplierInvoiceModificationPM()
                            {
                                InvoiceCounterKey = item.InvoiceCounterKey,
                                DeclarationId = item.DeclarationId,
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ModificationCounterKey = item.ModificationCounterKey,
                                Tenant = item.Tenant,
                            };
                            entityPM.SupplierInvoiceModifications.Add(deletedItem);

                        }

                        SupplierInvoicePaymentQueryService supplierInvoicePaymentQueryService = new SupplierInvoicePaymentQueryService(MyContext);

                        List<SupplierInvoicePaymentPM> supplierInvoicePaymentsChangeset = supplierInvoicePaymentQueryService.GetSupplierInvoicePaymentsForInvoice(entityPM.DeclarationId, entityPM.InvoiceCounterKey);//ChangeSet.GetAssociatedChanges(entityPM, d => d.SupplierInvoiceModifications).Cast<SupplierInvoiceModificationPM>().ToList();
                        foreach (SupplierInvoicePaymentPM item in supplierInvoicePaymentsChangeset)
                        {
                            SupplierInvoicePaymentPM deletedItem = new SupplierInvoicePaymentPM()
                            {
                                InvoiceCounterKey = item.InvoiceCounterKey,
                                DeclarationId = item.DeclarationId,
                                ChangeSetOp = ChangeSetOperation.Delete,
                                PaymentAmount = item.PaymentAmount,
                                PaymentTypeCode = item.PaymentTypeCode,
                                SequenceNumeric = item.SequenceNumeric,
                                 Tenant = item.Tenant,
                            };
                            entityPM.DeletedSupplierInvoicePayments.Add(deletedItem);

                        }

                        SupplierInvoiceUCRQueryService supplierInvoiceUCRQueryService = new SupplierInvoiceUCRQueryService(MyContext);

                        List<SupplierInvoiceUCRPM> supplierInvoiceUCRsChangeset = supplierInvoiceUCRQueryService.GetSupplierInvoiceUCRsForInvoice(entityPM.DeclarationId, entityPM.InvoiceCounterKey);//ChangeSet.GetAssociatedChanges(entityPM, d => d.SupplierInvoiceModifications).Cast<SupplierInvoiceModificationPM>().ToList();
                        foreach (SupplierInvoiceUCRPM item in supplierInvoiceUCRsChangeset)
                        {
                            SupplierInvoiceUCRPM deletedItem = new SupplierInvoiceUCRPM()
                            {
                                InvoiceCounterKey = item.InvoiceCounterKey,
                                DeclarationId = item.DeclarationId,
                                ChangeSetOp = ChangeSetOperation.Delete,
                                 Tenant = item.Tenant,
                                 AgentChargeID= item.AgentChargeID,
                                 SequenceNumeric= item.SequenceNumeric,
                                 SupplierChargeID = item.SupplierChargeID
                            };
                            entityPM.DeletedSupplierInvoiceUCRs.Add(deletedItem);

                        }


                        #endregion

                        service.Update(entityPM, true);
                      //  if (entityPM.IsPrimarySupplierInvoice)
                        {
                            DeclarationRepository declarationRep = new DeclarationRepository(MyContext);
                            Declaration declaration = declarationRep.GetSingleDeclarationById(declarationId, tenant);
                            if (declaration.PrimaryInvoiceCounterKey == entityPM.InvoiceCounterKey.ToString())
                            {
                                SupplierInvoiceRepository invoiceRep = new SupplierInvoiceRepository(MyContext);
                                SupplierInvoice firstInv = invoiceRep.GetFirstInvoice(declarationId);
                                if (firstInv != null)
                                {
                                    declaration.PrimaryInvoiceCounterKey = firstInv.InvoiceCounterKey.ToString();
                                    declarationRep.Update(declaration);
                                    declarationRep.SubmitChanges();
                                }

                            }
                        }

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public HttpResponseMessage GetTotalForeignCurrencyForInvoice(string declarationId, int invoiceCounterKey)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceItemQueryService queryService = new SupplierInvoiceItemQueryService(customContext);
                decimal? value = queryService.GetTotalForeignCurrencyForInvoice(declarationId, invoiceCounterKey, tenant);
               
                return Request.CreateResponse(HttpStatusCode.OK, value);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetInvoiceItemsWithTradeAgreementCount(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                int count = declarationQuery.GetInvoiceItemsWithTradeAgreementCount(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSupplierInvoicesPMsForDeclarationWithTradeAgreementCount(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                SupplierInvoiceQueryService supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
                List<SupplierInvoicePM> supplierInvoices = supplierInvoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant);
                int count = declarationQuery.GetInvoiceItemsWithTradeAgreementCount(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, new { SupplierInvoices = supplierInvoices, Count = count });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleSupplierInvoicePMWithLimitedItems(string declarationId, int counterkey, int skip, int take, string type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceQueryService supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
                SupplierInvoicePM SupplierInvoice = supplierInvoiceQuery.GetSingleSupplierInvoiceWithLimitedItems(declarationId, counterkey, tenant, skip, take, type);
                return Request.CreateResponse(HttpStatusCode.OK, SupplierInvoice);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetImporterDepositions(string vendorId, string importerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                ImporterDespositionRepository repository = new ImporterDespositionRepository(customContext);
                IQueryable<ImporterDesposition> despositions = repository.GetImporterDespositions(vendorId, importerId, tenant);
                ImporterDespositionQueryService queryService = new ImporterDespositionQueryService(customContext);
                ImporterDesposition desposition;
                ImporterDespositionClass importer = new ImporterDespositionClass();
              
                if (despositions.Count() > 0)
                {
                    desposition = despositions.First();

                    if (desposition.EndDate >= DateTime.Today && desposition.StartDate <= DateTime.Today)
                    {
                        importer.Status = "GreenTick";
                    }
                    else if (desposition.StartDate > DateTime.Today)
                    {
                        if (despositions.Count() > 1)
                        {

                            desposition = despositions.Where(d => d.EndDate >= DateTime.Today && d.StartDate <= DateTime.Today).FirstOrDefault();
                            if (desposition != null)
                            {
                                importer.Status = "GreenTick";
                            }
                            else
                            {
                                importer.Status = "OrangeTick";
                            }
                        }
                        else
                        {
                            importer.Status = "OrangeTick";
                        }

                    }
                    else if (desposition.EndDate < DateTime.Today)
                    {
                        importer.Status = "RedX";
                    }

                    if (desposition == null)
                    {
                        desposition = despositions.First();
                    }
                        importer.EndDate = desposition.EndDate;//Value.ToShortDateString();

                        importer.ImporterDespositionNumber = desposition.DepositionNumber;
                   
                }
            

                //ServiceResponse response = new ServiceResponse();
                //response.Result = status;
                return Request.CreateResponse(HttpStatusCode.OK, importer);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDocumentFilingIdForForInvoice(string declarationId, int counterkey, bool isOcr = false)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                string resultDocumentFilingId = null;

                //
                // 1- Get Pointers
                CustomsDocumentPointerListQueryService pointersQuery = new CustomsDocumentPointerListQueryService(customContext);
                QueryOperations pointerFilters = new QueryOperations();
                pointerFilters.GetAll = true;
                pointerFilters.PageIndex = 0;
                pointerFilters.PageSize = 999;
                pointerFilters.SetFilter("ParentEntityId", declarationId, false, "Equals", null, null, false);
                pointerFilters.SetFilter("Child1EntityId", counterkey.ToString(), false, "Equals", null, null, false);
                List<CustomsDocumentPointerList> invoicePointers = pointersQuery.GetList(pointerFilters, tenant);
                string ticketIds = "";
                foreach(CustomsDocumentPointerList pointer in invoicePointers)
                {
                    ticketIds = ticketIds + pointer.CustomsDocumentsTicketId + ",";
                }
                ticketIds = ticketIds.TrimEnd(',');

                //2-get the tickets related to pointers
                //CustomsDocumentsTicketList ticket = new CustomsDocumentsTicketList();
                CustomsDocumentsTicketQueryService ticketsQuery = new CustomsDocumentsTicketQueryService(customContext);
                List<CustomsDocumentsTicketPM> tickets = ticketsQuery.GetCustomsDocumentsTicketsIds(ticketIds, tenant);
                string documentFilingIds = "";
                foreach (CustomsDocumentsTicketPM ticket in tickets)
                {
                    if (!string.IsNullOrEmpty(ticket.DocumentsFilingId))
                    {
                        documentFilingIds = documentFilingIds + ticket.DocumentsFilingId + ",";
                    }
                }
                documentFilingIds = documentFilingIds.TrimEnd(',');

                //3- get all filings related to the pointers
                if (!string.IsNullOrEmpty(documentFilingIds)) {
                    DocumentsFilingPM documentFiling = new DocumentsFilingPM();
                    DocumentsFilingQuery filingQuery = new DocumentsFilingQuery(tenant);
                    List<DocumentsFilingPM> documentsFilings = filingQuery.GetDocumentsFilingsByIds(documentFilingIds, tenant);

                    //4- check the filings
                    DocumentsFilingPM ClsiDocument = documentsFilings.Where(d => d.DocumentTypeCode == "CLSI").FirstOrDefault();
                    DocumentsFilingPM FSiDocument = documentsFilings.Where(d => d.DocumentTypeCode == "FSI").FirstOrDefault();
                    DocumentsFilingPM FirstDocument = documentsFilings.FirstOrDefault();
                    if (ClsiDocument != null)
                    {
                        resultDocumentFilingId = ClsiDocument.Id;
                    }
                    else if (FSiDocument != null)
                    {
                        resultDocumentFilingId = FSiDocument.Id;
                    }
                    else if(FirstDocument!=null)
                    {
                        resultDocumentFilingId = FirstDocument.Id;
                    }

                    if (isOcr)
                    {
                        ServiceResponse resultDocIdAndCountOfTickets = new ServiceResponse();
                        resultDocIdAndCountOfTickets.Result = resultDocumentFilingId;
                        resultDocIdAndCountOfTickets.Count = ticketsQuery.GetCountOfTicketsByDocFilingId(resultDocumentFilingId, tenant);
                        return Request.CreateResponse(HttpStatusCode.OK, resultDocIdAndCountOfTickets);

                    }

                }
                
                #region old code
                ////
                //// 2- Select a document by DocumentTypeCode (sort and select the first)
                //CustomsDocumentPointerList documentPointer = new CustomsDocumentPointerList();
                //List<CustomsDocumentPointerList> CLSI_DocumentsPointers = invoicePointers.FindAll(d => d.DocumentTypeCode == "CLSI");
                //List<CustomsDocumentPointerList> FSI_DocumentsPointers = invoicePointers.FindAll(d => d.DocumentTypeCode == "FSI");
                //if (CLSI_DocumentsPointers.Count > 0)
                //{
                //    documentPointer = CLSI_DocumentsPointers[0];
                //}
                //else if (FSI_DocumentsPointers.Count > 0)
                //{
                //    documentPointer = FSI_DocumentsPointers[0];
                //}
                //else
                //{
                //    documentPointer = invoicePointers.FirstOrDefault();
                //}


                ////
                //// 3- Get related CustomsDocumentsTickets for the pointer
                //CustomsDocumentsTicketList ticket = new CustomsDocumentsTicketList();
                //CustomsDocumentsTicketListQueryService ticketsQuery = new CustomsDocumentsTicketListQueryService(customContext);
                //if (documentPointer != null)
                //{
                //    if (documentPointer.CustomsDocumentsTicketId != null)
                //    {
                //        ticket = ticketsQuery.GetSingle(documentPointer.CustomsDocumentsTicketId);
                //    }
                //    else
                //    {
                //        // No tickets related to this pointer
                //    }
                //}


                ////
                //// 4- Get related DocumentsFilings for the ticket
                //DocumentsFilingPM documentFiling = new DocumentsFilingPM();
                //DocumentsFilingQuery filingQuery = new DocumentsFilingQuery(tenant);
                //if (ticket != null)
                //{
                //    documentFiling = filingQuery.GetSinglePM(ticket.DocumentsFilingId, tenant);
                //    if (documentFiling != null)
                //    {
                //        resultDocumentFilingId = documentFiling.Id;
                //    }
                //    else
                //    {
                //        // No document filing related to this ticket
                //    }
                //}
                #endregion   
                return Request.CreateResponse(HttpStatusCode.OK, resultDocumentFilingId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckIfInvoiceNumberExists(string declarationId, string invoiceNumber, int invoiceCounterKey)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceQueryService supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
                SupplierInvoice exists = supplierInvoiceQuery.CheckIfInvoiceNumberExists(declarationId,invoiceNumber,invoiceCounterKey, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, exists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PostSendMultiUpdate(MultiUpdateRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCBMultiUpdate_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutExportSupplierInviocesFromFileRequest(int tenant, string declarationId, ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] data = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    string decodedString = Encoding.UTF8.GetString(data);
                    var messagingService = new DCAInUCBCreateExportSupplierInvoicesFromFile_MsgMessagingService();
                    var sts = messagingService.CreateCRS(tenant, declarationId, decodedString);
                    return Request.CreateResponse(HttpStatusCode.OK, sts);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutSupplierInvioceFromFileRequest(int tenant, string clientId, string partnerId, string declarationId,bool ignoreChecks, ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] data = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    string decodedString = Encoding.UTF8.GetString(data);

                    if (!ignoreChecks)
                    {
                        List<string> classificationCodesNotValid = GetClassificationCodesNotValid(decodedString, tenant, declarationId);
                        if (classificationCodesNotValid.Count > 0)
                        {
                            var res = "לא נמצא סיווג שמתאים ללקוח בתיק עבור הדגמים: \n";
                            foreach (var item in classificationCodesNotValid)
                            {
                                res += item + "\n";
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, res);
                        }
                    }

                    var messagingService = new DCAInUCBCreateSupplierInvoiceFromFile_MsgMessagingService();
                    //partnerId = "METRO";
                    var sts = messagingService.CreateCRS(tenant, clientId, partnerId, declarationId, decodedString);

                    return Request.CreateResponse(HttpStatusCode.OK, sts);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private List<string> GetClassificationCodesNotValid(string decodedString, int tenant, string declarationId)
        {
            var classificationCodes = ReadClassificationCodesFromCsvFile(decodedString);

            List<string> classificationCodesNotValid = new List<string>();
            foreach (var item in classificationCodes)
            {
                var context = CustomContext.GetContext(tenant);
                var amitalContext = AmitalContext.GetContext(tenant);
                var declarationQueryService = new DeclarationQueryService(context);
                DeclarationPM declarationPM = declarationQueryService.GetSingle(declarationId, true, false);
                var re = new CTBCARMODRepository(amitalContext);
                var classificationCode = re.GetSingle(declarationPM.CustomerCode, item)?.PRAT;
                if (string.IsNullOrWhiteSpace(classificationCode))
                {
                    classificationCodesNotValid.Add(item);
                }
            }
            return classificationCodesNotValid;
        }

        private List<string> ReadClassificationCodesFromCsvFile(string decodedString)
        {
            var lines = decodedString.Split(new string[] { "\n" }, StringSplitOptions.None).ToList();
            List<string> classificationCodes = new List<string>();
            for (int i = 1; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] data = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                if (string.IsNullOrWhiteSpace(data[7]))
                {
                    throw new Exception("ClassificationCode cannot be null");
                }

                if (!classificationCodes.Contains(data[7]))
                    classificationCodes.Add(data[7]);
            }
            return classificationCodes;
        }



        //public HttpResponseMessage UpdateInvoiceVendorCommision(SupplierInvoicePM invoicePM)// string declarationId, int counterKey,string invoiceCurrency, decimal invoiceAmount, string vendorId, string customerId)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        string loggedUserEmail = authToken.Email;
        //        SecurityUtility.AuthenticationOnTenant(tenant);

        //        ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

        //        VendorCommissionQueryService commissionService = new VendorCommissionQueryService(tenant);
        //        DeclarationRepository declarationRep = new DeclarationRepository(customContext);
        //        Declaration declaration = declarationRep.GetSingleDeclarationById(invoicePM.DeclarationId, tenant);
        //        VendorCommissionPM commission = commissionService.GetSingleCommisionByVendorAndCustomer(invoicePM.VendorId, declaration.CustomerId, tenant);
        //        ServiceResponse response = new ServiceResponse();
        //        if (commission != null)
        //        {
        //            SupplierInvoiceQueryService invoiceService = new SupplierInvoiceQueryService(tenant);
        //            SupplierInvoicePM invoice = invoiceService.GetSupplierInvoicWithoutComposition(invoicePM.DeclarationId, invoicePM.InvoiceCounterKey, tenant);
        //            SupplierInvoiceUpdateService service = new SupplierInvoiceUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
        //            invoice.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //            invoice.VendorComissionPercentage = commission.CommisionPercentage;
        //            service.Update(invoice, true);

        //            SupplierInvoiceModificationRepository rep = new SupplierInvoiceModificationRepository(customContext);

        //            SupplierInvoiceModification mod = rep.ChekIfModWithCurrencyExist(invoicePM.DeclarationId, invoicePM.InvoiceCounterKey, invoicePM.InvoiceCurrencyTypeCode, tenant);


        //            if (mod != null)
        //            {
        //                if (mod.TypeCode == "110" && mod.CurrencyTypeCode == invoicePM.InvoiceCurrencyTypeCode && mod.Amount == (invoice.VendorComissionPercentage * invoicePM.InvoiceAmount))
        //                {
        //                    response.Result = false;
        //                }
        //                else if (mod.CurrencyTypeCode == invoicePM.InvoiceCurrencyTypeCode)
        //                {
        //                    response.Result = true;
        //                }
        //            }
        //            else
        //            {
        //                response.Result = false;
        //                SupplierInvoiceModificationPM modification = new SupplierInvoiceModificationPM() { Amount = (invoice.VendorComissionPercentage * invoicePM.InvoiceAmount), TypeCode = "110", CurrencyTypeCode = invoicePM.InvoiceCurrencyTypeCode };
        //                SupplierInvoiceModificationUpdateService modservice = new SupplierInvoiceModificationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
        //                modification.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
        //                modservice.Update(modification, true);

        //            }
        //        }


        //        response.Result = null;


        //        return Request.CreateResponse(HttpStatusCode.OK, response);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}
        public HttpResponseMessage PutUpdateSupplierInvoiceModifications([FromBody] SupplierInvoicePM[] invoicePMs)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "UPDATE", authToken.Tenant);
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                SupplierInvoiceUpdateService service = new SupplierInvoiceUpdateService(customContext, new Dictionary<string, IContext>(), authToken.Tenant);

                foreach (var invoicePM in invoicePMs)
                {
                    SecurityUtility.AuthenticationOnEntityTenant("SupplierInvoice", invoicePM.Tenant, authToken.Tenant);
                    //ICustomContext MyContext = CustomContext.GetContext(invoicePM.Tenant);
                    //service.InitializeEntityPM(invoicePM);
                    invoicePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    service.Update(invoicePM, true);
                }
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




        public HttpResponseMessage PutMultiUpdateOCR( [FromBody] PutMultiUpdateOCRRequest requestParams)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                SupplierInvoiceQueryService supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
                var ans = supplierInvoiceQuery.UpdateSupplierInvoiceByOcrDefaults(requestParams?.DeclarationId, requestParams.SupplierInvoiceList, requestParams.SupplierInvioceItemCertificats, requestParams.SupplierInvioceExportDefault,tenant);
                //   var sts = messagingService.CreateCRS(tenant, null, requestParamsData);
                return Request.CreateResponse(HttpStatusCode.OK, ans);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage DeletedSupplierInvoiceItemsConDeclars(string declarationId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticationOnTenant(tenant);



                        ICustomContext MyContext = CustomContext.GetContext(tenant);

                        var mySupplierInvoiceItemsConDeclarUpdateService = new SupplierInvoiceItemsConDeclarUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                        mySupplierInvoiceItemsConDeclarUpdateService.FastDeleteComposition(new DeclarationKeys() { Id = declarationId });

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
        public class PutMultiUpdateOCRRequest
        {
            public SupplierInvioceItemCertificatPM[] SupplierInvioceItemCertificats { get; set; }
            public SupplierInvioceExportDefaultPM SupplierInvioceExportDefault { get; set; }
            public string DeclarationId { get; set; }
            public string SupplierInvoiceList { get; set; }



        }

    }
}