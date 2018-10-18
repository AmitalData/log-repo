using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public SupplierInvoicePM GetSingleSupplierInvoicePM(string declarationId, int counterkey, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            SupplierInvoicePM SupplierInvoice = supplierInvoiceQuery.GetSingle(declarationId, counterkey, true, false);
            return SupplierInvoice;
        }

      

        public SupplierInvoiceList GetSingleSupplierInvoiceList(string declarationid, int counterkey, int linenumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceListQueryService listService = new SupplierInvoiceListQueryService(customContext);
            return listService.GetSingle(declarationid, counterkey);
        }

        public List<SupplierInvoiceList> GetSupplierInvoiceLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceListQueryService listService = new SupplierInvoiceListQueryService(customContext);
            return listService.GetList(tenant);
        }

        public List<SupplierInvoiceList> GetSupplierInvoiceFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceListQueryService listService = new SupplierInvoiceListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public List<SupplierInvoicePM> GetSupplierInvoicesForDeclaration(string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            List<SupplierInvoicePM> supplierInvoices = supplierInvoiceQuery.GetSupplierInvoicesForDeclaration(declarationId, tenant);
            return supplierInvoices;
        }

        public int GetSupplierInvoiceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceListQueryService queryService = new SupplierInvoiceListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertSupplierInvoice(SupplierInvoicePM entityPm)
        {

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            SupplierInvoiceUpdateService service = new SupplierInvoiceUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            SetSupplierInvoiceItemChangeSet(entityPm);
            SetSupplierInvoiceModificationsChangeSet(entityPm);
            SetSupplierInvoiceFreightAmounts(entityPm);
            service.Update(entityPm, true);

        }

        public void UpdateSupplierInvoice(SupplierInvoicePM currententityPm)
        {

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            SupplierInvoiceUpdateService service = new SupplierInvoiceUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetSupplierInvoiceItemChangeSet(currententityPm);
            SetSupplierInvoiceModificationsChangeSet(currententityPm);
            SetSupplierInvoiceFreightAmounts(currententityPm); 
            service.Update(currententityPm, true);

        }

        public void DeleteSupplierInvoice(SupplierInvoicePM entityPM)
        {
            entityPM.SupplierInvoiceItems.Clear();
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPM.Tenant);
            }
            SupplierInvoiceUpdateService service = new SupplierInvoiceUpdateService(customContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            SupplierInvoiceItemQueryService supplierInvoiceItemQueryService=new SupplierInvoiceItemQueryService(customContext);

            #region composition handling
            //invoice items
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
                SupplierInvoiceItemsConDeclarQueryService supplierInvoiceItemsConDeclarQueryService = new SupplierInvoiceItemsConDeclarQueryService(customContext);
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
                supplierInvoiceItemsTaxQuery = new SupplierInvoiceItemsTaxQueryService(customContext);
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
                supplierInvioceItemCertificatQuery = new SupplierInvioceItemCertificatQueryService(customContext);
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
                SupplierInvoiceItemsModQueryService supplierInvoiceItemsModQuery = new SupplierInvoiceItemsModQueryService(customContext);
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
                SupplierInvoiceItemsSerialNumQueryService supplierInvoiceItemsSerialNumQuery = new SupplierInvoiceItemsSerialNumQueryService(customContext);
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
                SupplierInvoiceItemsProdIdentQueryService supplierInvoiceItemsProdIdentQuery = new SupplierInvoiceItemsProdIdentQueryService(customContext);
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
                SupplierInvoiceItemsDescriptQueryService supplierInvoiceItemsDescriptQuery = new SupplierInvoiceItemsDescriptQueryService(customContext);
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
                SupplierInvoiceItemProcesTypeQueryService supplierInvoiceItemProcesTypeQuery = new SupplierInvoiceItemProcesTypeQueryService(customContext);
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
                SupplierInvoiceItemsLevyQueryService supplierInvoiceItemsLevyQuery = new SupplierInvoiceItemsLevyQueryService(customContext);
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

                 SupplierInvoiceItemModVehicleQueryService supplierInvoiceItemModVehicleQueryService = new SupplierInvoiceItemModVehicleQueryService(customContext);
                 List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicleChangeSet = supplierInvoiceItemModVehicleQueryService.GetSupplierInvoiceItemModVehiclesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, item.LineNumber, item.Tenant);
                 foreach (SupplierInvoiceItemModVehiclePM itemModVehicle in supplierInvoiceItemModVehicleChangeSet)
                 {
                     SupplierInvoiceItemModVehiclePM deletedItemModVehicle= new SupplierInvoiceItemModVehiclePM()
                     {
                         InvoiceCounterKey = itemModVehicle.InvoiceCounterKey,
                         DeclarationId = itemModVehicle.DeclarationId,
                         AdjustmentTypeCode=itemModVehicle.AdjustmentTypeCode,
                         InvoiceItemLineNumber = itemModVehicle.InvoiceItemLineNumber,
                         Tenant = itemModVehicle.Tenant,
                         ChangeSetOp = ChangeSetOperation.Delete,
                     };

                     deletedItem.DeletedSupplierInvoiceItemModVehicles.Add(deletedItemModVehicle);
                 }
                // Vehicles

                SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQuery = new SupplierInvoiceItemVehicleQueryService(customContext);
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

                        ChangeSetOp = ChangeSetOperation.Delete,
                    };

                    deletedItem.DeletedSupplierInvoiceItemVehicles.Add(deletedItemVehicle);

                    SupplierInvoiceItemVehicleAddQueryService supplierInvoiceItemVehicleAddQueryService = new SupplierInvoiceItemVehicleAddQueryService(customContext);
                    List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAddChangeSet = supplierInvoiceItemVehicleAddQueryService.GetSupplierInvoiceItemVehicleAddsForSupplierInvoiceItemVehicle(vehicle.DeclarationId, vehicle.InvoiceCounterKey, vehicle.InvoiceItemLineNumber, vehicle.LineNumber, vehicle.Tenant);

                    foreach (SupplierInvoiceItemVehicleAddPM itemVehicleAdd in supplierInvoiceItemVehicleAddChangeSet)
                    {
                        SupplierInvoiceItemVehicleAddPM itemPM = new SupplierInvoiceItemVehicleAddPM()
                        {
                            DeclarationId = itemVehicleAdd.DeclarationId,
                            InvoiceCounterKey = itemVehicleAdd.InvoiceCounterKey,
                            InvoiceItemLineNumber = itemVehicleAdd.InvoiceItemLineNumber,
                            LineNumber = itemVehicleAdd.LineNumber,
                            ChangeSetOp=ChangeSetOperation.Delete,
                        };
                        deletedItemVehicle.DeletedSupplierInvoiceItemVehicleAdds.Add(itemPM);
                    }

                    SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModQueryService = new SupplierInvoiceItemVehicleModQueryService(customContext);
                    List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleModChangeSet = supplierInvoiceItemVehicleModQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoiceForVehicle(vehicle.DeclarationId, vehicle.InvoiceCounterKey, vehicle.InvoiceItemLineNumber, vehicle.LineNumber, vehicle.Tenant);

                    foreach (SupplierInvoiceItemVehicleModPM itemVehicleMod in supplierInvoiceItemVehicleModChangeSet)
                    {
                        SupplierInvoiceItemVehicleModPM itemPM = new SupplierInvoiceItemVehicleModPM()
                        {
                            DeclarationId = itemVehicleMod.DeclarationId,
                            InvoiceCounterKey = itemVehicleMod.InvoiceCounterKey,
                            InvoiceItemLineNumber = itemVehicleMod.InvoiceItemLineNumber,
                            LineNumber = itemVehicleMod.LineNumber,
                            VehicleLineNumber=itemVehicleMod.VehicleLineNumber,
                            ChangeSetOp = ChangeSetOperation.Delete,
                        };
                        deletedItemVehicle.DeletedSupplierInvoiceItemVehicleMods.Add(itemPM);
                    }

                   

                }

            }
            //FreightAmounts
            SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(customContext);
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
            SupplierInvoiceModificationQueryService supplierInvoiceModificationQuery = new SupplierInvoiceModificationQueryService(customContext);

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
            #endregion
            service.Update(entityPM, true);
        }

        public void UpdateSupplierInvoiceList(SupplierInvoiceList list)
        {

        }

       
        public SupplierInvoicePM GetSingleSupplierInvoicePMWithLimitedItems(string declarationId, int counterkey, int tenant,int skip,int take)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            SupplierInvoicePM SupplierInvoice = supplierInvoiceQuery.GetSingleSupplierInvoiceWithLimitedItems(declarationId, counterkey,tenant, skip, take, null);
            return SupplierInvoice;
        }

        public List<SupplierInvoiceItemList> GetSupplierInvoiceItemFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemListQueryService listService = new SupplierInvoiceItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetSupplierInvoiceItemFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.SupplierInvoice", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemListQueryService queryService = new SupplierInvoiceItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public decimal? GetTotalForeignCurrencyForInvoice(string declarationId, int counterKey, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemQueryService queryService = new SupplierInvoiceItemQueryService(customContext);
            decimal? value = queryService.GetTotalForeignCurrencyForInvoice(declarationId, counterKey, tenant);
            return value;
        }
       
        public List<SupplierInvoiceList> GetSupplierInvoiceListsForDeclaration(string declarationId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceListQueryService listService = new SupplierInvoiceListQueryService(customContext);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.SetFilter("DeclarationId", declarationId, false, "Equals", null, false);
            queryOperations.PageIndex = 0;
            queryOperations.PageSize = 100;

            return listService.GetList(queryOperations, tenant);

        }

        public SupplierInvoicePM GetSupplierInvoiceWithSpecificItemBySequenceNumber(string declarationId, int invoiceSequence, int itemSequence, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            SupplierInvoicePM supplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceWithSpecificItemBySequenceNumber(declarationId, invoiceSequence, itemSequence);
            return supplierInvoice;
        }

        public SupplierInvoicePM GetSupplierInvoiceWithSpecificItemByCounterKey(string declarationId, int counterKey, int sequence, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            SupplierInvoicePM supplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceWithSpecificItemByCounterKey(declarationId, counterKey, sequence);
            return supplierInvoice;
        }

        public SupplierInvoicePM GetSupplierInvoiceBySequenceNumber(string declarationId, int invoiceSequence, int skip, int take,int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            SupplierInvoicePM supplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceBySequenceNumber(declarationId, invoiceSequence, skip, take);
            return supplierInvoice;
        }

        private void SetSupplierInvoiceModificationsChangeSet(SupplierInvoicePM currententityPm)
        {
            List<SupplierInvoiceModificationPM> supplierInvoiceModificationchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceModifications).Cast<SupplierInvoiceModificationPM>().ToList();
            foreach (SupplierInvoiceModificationPM itemPM in supplierInvoiceModificationchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceModificationPM currentItemPM = currententityPm.SupplierInvoiceModifications.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.ModificationCounterKey == itemPM.ModificationCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceModificationPM currentItemPM = currententityPm.SupplierInvoiceModifications.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.ModificationCounterKey == itemPM.ModificationCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceModificationPM currentItemPM = new SupplierInvoiceModificationPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, InvoiceCounterKey = itemPM.InvoiceCounterKey, ModificationCounterKey = itemPM.ModificationCounterKey };
                            currententityPm.DeletedSupplierInvoiceModifications.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            SupplierInvoiceModificationPM currentItemPM = currententityPm.SupplierInvoiceModifications.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.ModificationCounterKey == itemPM.ModificationCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetSupplierInvoiceItemChangeSet(SupplierInvoicePM currententityPm)
        {
            List<SupplierInvoiceItemPM> supplierInvoiceItemchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItems).Cast<SupplierInvoiceItemPM>().ToList();
            foreach (SupplierInvoiceItemPM itemPM in supplierInvoiceItemchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemPM currentItemPM = currententityPm.SupplierInvoiceItems.Where(d => d.DeclarationId == itemPM.DeclarationId && d.CounterKey == itemPM.CounterKey && d.LineNumber == itemPM.LineNumber && d.SequenceNumeric == itemPM.SequenceNumeric).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            //SetSupplierInvoiceItemQuantityChangeSet(currentItemPM);
                            SetSupplierInvoiceItemConnectedToDeclarationChangeSet(currentItemPM);
                            SetSupplierInvoiceItemModificationsChangeSet(currentItemPM);
                            SetSupplierInvoiceItemTaxChangeSet(currentItemPM);
                            SetSupplierInvoiceItemCertificatesChangeSet(currentItemPM);
                            SetSupplierInvoiceItemDescriptionChangeSet(currentItemPM);
                            SetSupplierInvoiceItemSerialNumberChangeSet(currentItemPM);
                            SetSupplierInvoiceItemProductIdentificationChangeSet(currentItemPM);
                            SetSupplierInvoiceItemProcessTypesChangeSet(currentItemPM);
                            SetSupplierInvoiceItemLeviesChangeSet(currentItemPM);
                            SetSupplierInvoiceItemVehicleChangeSet(currentItemPM);
                            SetSupplierInvoiceItemModVehicleChangeSet(currentItemPM);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemPM currentItemPM = currententityPm.SupplierInvoiceItems.Where(d => d.DeclarationId == itemPM.DeclarationId && d.CounterKey == itemPM.CounterKey && d.LineNumber == itemPM.LineNumber && d.SequenceNumeric == itemPM.SequenceNumeric).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            //SetSupplierInvoiceItemQuantityChangeSet(currentItemPM);
                            SetSupplierInvoiceItemConnectedToDeclarationChangeSet(currentItemPM);
                            SetSupplierInvoiceItemModificationsChangeSet(currentItemPM);
                            SetSupplierInvoiceItemTaxChangeSet(currentItemPM);
                            SetSupplierInvoiceItemCertificatesChangeSet(currentItemPM);
                            SetSupplierInvoiceItemDescriptionChangeSet(currentItemPM);
                            SetSupplierInvoiceItemSerialNumberChangeSet(currentItemPM);
                            SetSupplierInvoiceItemProductIdentificationChangeSet(currentItemPM);
                            SetSupplierInvoiceItemProcessTypesChangeSet(currentItemPM);
                            SetSupplierInvoiceItemLeviesChangeSet(currentItemPM);
                            SetSupplierInvoiceItemVehicleChangeSet(currentItemPM);
                            SetSupplierInvoiceItemModVehicleChangeSet(currentItemPM);

                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemPM currentItemPM = new SupplierInvoiceItemPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, CounterKey = itemPM.CounterKey, SequenceNumeric = itemPM.SequenceNumeric };

                            SupplierInvoiceItemModVehicleQueryService supplierInvoiceItemModVehicleQueryService = new SupplierInvoiceItemModVehicleQueryService(customContext);
                            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicleChangeSet = supplierInvoiceItemModVehicleQueryService.GetSupplierInvoiceItemModVehiclesForSupplierInvoiceItem(currentItemPM.DeclarationId, currentItemPM.CounterKey, currentItemPM.LineNumber, currentItemPM.Tenant);
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

                                currentItemPM.DeletedSupplierInvoiceItemModVehicles.Add(deletedItemModVehicle);
                            }

                            ////quantities
                            //List<SupplierInvoiceItemsQuantityPM> supplierInvoiceItemsQuantityChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsQuantities).Cast<SupplierInvoiceItemsQuantityPM>().ToList();

                            //foreach (SupplierInvoiceItemsQuantityPM item in supplierInvoiceItemsQuantityChangeset)
                            //{
                            //    SupplierInvoiceItemsQuantityPM deletedItem = new SupplierInvoiceItemsQuantityPM()
                            //    {
                            //        InvoiceCounterKey = item.InvoiceCounterKey,
                            //        DeclarationId = item.DeclarationId,
                            //        LineNumber = item.LineNumber,
                            //        InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                            //        ChangeSetOp = ChangeSetOperation.Delete,
                            //    };

                            //    currentItemPM.DeletedSupplierInvoiceItemsQuantities.Add(deletedItem);
                            //}
                            //connected to declarations
                            List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemsConnectedDeclarationChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsConDeclars).Cast<SupplierInvoiceItemsConDeclarPM>().ToList();

                            foreach (SupplierInvoiceItemsConDeclarPM item in supplierInvoiceItemsConnectedDeclarationChangeset)
                            {
                                SupplierInvoiceItemsConDeclarPM deletedItem = new SupplierInvoiceItemsConDeclarPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemsConDeclars.Add(deletedItem);
                            }
                            //taxes
                            supplierInvoiceItemsTaxQuery = new SupplierInvoiceItemsTaxQueryService(customContext);
                            List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemsTaxChangeset = supplierInvoiceItemsTaxQuery.GetSupplierInvoiceItemsTaxForInvoiceItem(itemPM.DeclarationId, itemPM.CounterKey, itemPM.LineNumber, itemPM.Tenant);  //ChangeSet.GetAssociatedChanges(item, d => d.SupplierInvoiceItemTaxes).Cast<SupplierInvoiceItemsTaxPM>().ToList();

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

                                currentItemPM.DeletedSupplierInvoiceItemTaxes.Add(deletedItemTax);
                            }
                            //certificates
                            List<SupplierInvioceItemCertificatPM> supplierInvoiceItemsCertificateChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvioceItemCertificats).Cast<SupplierInvioceItemCertificatPM>().ToList();

                            foreach (SupplierInvioceItemCertificatPM item in supplierInvoiceItemsCertificateChangeset)
                            {
                                SupplierInvioceItemCertificatPM deletedItem = new SupplierInvioceItemCertificatPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    ItemCertificateCounterKey = item.ItemCertificateCounterKey,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvioceItemCertificats.Add(deletedItem);
                            }

                            //modifications
                            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsModificationChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsMods).Cast<SupplierInvoiceItemsModPM>().ToList();

                            foreach (SupplierInvoiceItemsModPM item in supplierInvoiceItemsModificationChangeset)
                            {
                                SupplierInvoiceItemsModPM deletedItem = new SupplierInvoiceItemsModPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    TypeCode = item.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    ModificationCounterKey = item.ModificationCounterKey,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemsMods.Add(deletedItem);
                            }

                            //serialnumbers
                            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumberChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsSerialNums).Cast<SupplierInvoiceItemsSerialNumPM>().ToList();

                            foreach (SupplierInvoiceItemsSerialNumPM item in supplierInvoiceItemsSerialNumberChangeset)
                            {
                                SupplierInvoiceItemsSerialNumPM deletedItem = new SupplierInvoiceItemsSerialNumPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    TypeCode = item.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    SerialNumber = item.SerialNumber,
                                    Tenant = item.Tenant,

                                };

                                currentItemPM.DeletedSupplierInvoiceItemsSerialNums.Add(deletedItem);
                            }

                            //product identification
                            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProductIdentificationChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsProdIdents).Cast<SupplierInvoiceItemsProdIdentPM>().ToList();

                            foreach (SupplierInvoiceItemsProdIdentPM item in supplierInvoiceItemsProductIdentificationChangeset)
                            {
                                SupplierInvoiceItemsProdIdentPM deletedItem = new SupplierInvoiceItemsProdIdentPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    TypeCode = item.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    Identification = item.Identification,
                                    Tenant = item.Tenant,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemsProdIdents.Add(deletedItem);
                            }

                            //Descriptions
                            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptionChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsDescripts).Cast<SupplierInvoiceItemsDescriptPM>().ToList();

                            foreach (SupplierInvoiceItemsDescriptPM item in supplierInvoiceItemsDescriptionChangeset)
                            {
                                SupplierInvoiceItemsDescriptPM deletedItem = new SupplierInvoiceItemsDescriptPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    TypeCode = item.TypeCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    Description = item.Description,
                                    Tenant = item.Tenant,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemsDescripts.Add(deletedItem);
                            }
                            // process types

                            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemsProcessTypeChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemProcesTypes).Cast<SupplierInvoiceItemProcesTypePM>().ToList();

                            foreach (SupplierInvoiceItemProcesTypePM item in supplierInvoiceItemsProcessTypeChangeset)
                            {
                                SupplierInvoiceItemProcesTypePM deletedItem = new SupplierInvoiceItemProcesTypePM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    ProcessTypeCode = item.ProcessTypeCode,
                                    Tenant = item.Tenant,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemProcesTypes.Add(deletedItem);
                            }

                            currententityPm.DeletedSupplierInvoiceItems.Add(currentItemPM);


                            //Levies
                            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemLevies).Cast<SupplierInvoiceItemsLevyPM>().ToList();

                            foreach (SupplierInvoiceItemsLevyPM item in supplierInvoiceItemsLevyChangeset)
                            {
                                SupplierInvoiceItemsLevyPM deletedItem = new SupplierInvoiceItemsLevyPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    Tenant = item.Tenant,
                                    TradeLevyExamptCode = item.TradeLevyExamptCode,
                                    TradeLevyNumber = item.TradeLevyNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemLevies.Add(deletedItem);
                            }

                            //Vehicles
                            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehicleChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemVehicles).Cast<SupplierInvoiceItemVehiclePM>().ToList();

                            foreach (SupplierInvoiceItemVehiclePM item in supplierInvoiceItemVehicleChangeset)
                            {
                                SupplierInvoiceItemVehiclePM deletedItem = new SupplierInvoiceItemVehiclePM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    Tenant = item.Tenant,
                                    RichbitFileNumber = item.RichbitFileNumber,
                                    VehicleChassisNumber = item.VehicleChassisNumber,
                                    SequenceNumeric = item.SequenceNumeric,
                                    VehicleId = item.VehicleId,
                                    VehicleTypeCode = item.VehicleTypeCode,
                                    ExcludeFromInterface = item.ExcludeFromInterface,

                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemVehicles.Add(deletedItem);
                            }

                            //Mod Vehicles
                            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicleChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemModVehicles).Cast<SupplierInvoiceItemModVehiclePM>().ToList();

                            foreach (SupplierInvoiceItemModVehiclePM item in supplierInvoiceItemModVehicleChangeset)
                            {
                                SupplierInvoiceItemModVehiclePM deletedItem = new SupplierInvoiceItemModVehiclePM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    Tenant = item.Tenant,

                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemModVehicles.Add(deletedItem);
                            }

                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemPM currentItemPM = currententityPm.SupplierInvoiceItems.Where(d => d.DeclarationId == itemPM.DeclarationId && d.CounterKey == itemPM.CounterKey && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetSupplierInvoiceItemLeviesChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemLevieschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemLevies).Cast<SupplierInvoiceItemsLevyPM>().ToList();
            foreach (SupplierInvoiceItemsLevyPM itemPM in supplierInvoiceItemLevieschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsLevyPM currentItemPM = currententityPm.SupplierInvoiceItemLevies.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsLevyPM currentItemPM = currententityPm.SupplierInvoiceItemLevies.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsLevyPM currentItemPM = new SupplierInvoiceItemsLevyPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemLevies.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsLevyPM currentItemPM = currententityPm.SupplierInvoiceItemLevies.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }
        private void SetSupplierInvoiceItemTaxChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            return;
            List<SupplierInvoiceItemsTaxPM> supplierInvoiceItemTaxchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemTaxes).Cast<SupplierInvoiceItemsTaxPM>().ToList();
            foreach (SupplierInvoiceItemsTaxPM itemPM in supplierInvoiceItemTaxchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsTaxPM currentItemPM = currententityPm.SupplierInvoiceItemTaxes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.TaxTypeCode == itemPM.TaxTypeCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            //SetSupplierInvoiceItemTaxModificationChangeSet(currentItemPM);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsTaxPM currentItemPM = currententityPm.SupplierInvoiceItemTaxes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.TaxTypeCode == itemPM.TaxTypeCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            //SetSupplierInvoiceItemTaxModificationChangeSet(currentItemPM);

                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsTaxPM currentItemPM = new SupplierInvoiceItemsTaxPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, TaxTypeCode = itemPM.TaxTypeCode };
                            currententityPm.DeletedSupplierInvoiceItemTaxes.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;

                            //List<SupplierInvoiceItemsTaxesModPM> supplierInvoiceItemsTaxesModificationQuantityChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemsTaxesMods).Cast<SupplierInvoiceItemsTaxesModPM>().ToList();

                            //foreach (SupplierInvoiceItemsTaxesModPM item in supplierInvoiceItemsTaxesModificationQuantityChangeset)
                            //{
                            //    SupplierInvoiceItemsTaxesModPM deletedItem = new SupplierInvoiceItemsTaxesModPM()
                            //    {
                            //        InvoiceCounterKey = item.InvoiceCounterKey,
                            //        DeclarationId = item.DeclarationId,
                            //        LineNumber = item.LineNumber,
                            //        TaxTypeCode = item.TaxTypeCode,
                            //        TypeCode = item.TypeCode,
                            //        ChangeSetOp = ChangeSetOperation.Delete,
                            //    };

                            //    currentItemPM.DeletedSupplierInvoiceItemsTaxesMods.Add(deletedItem);
                            //}
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsTaxPM currentItemPM = currententityPm.SupplierInvoiceItemTaxes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.TaxTypeCode == itemPM.TaxTypeCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemConnectedToDeclarationChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemsConDeclarPM> supplierInvoiceItemConnectedToDeclarationschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemsConDeclars).Cast<SupplierInvoiceItemsConDeclarPM>().ToList();
            foreach (SupplierInvoiceItemsConDeclarPM itemPM in supplierInvoiceItemConnectedToDeclarationschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsConDeclarPM currentItemPM = currententityPm.SupplierInvoiceItemsConDeclars.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsConDeclarPM currentItemPM = currententityPm.SupplierInvoiceItemsConDeclars.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsConDeclarPM currentItemPM = new SupplierInvoiceItemsConDeclarPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemsConDeclars.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsConDeclarPM currentItemPM = currententityPm.SupplierInvoiceItemsConDeclars.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemCertificatesChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvioceItemCertificatPM> supplierInvoiceItemCertificatesschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvioceItemCertificats).Cast<SupplierInvioceItemCertificatPM>().ToList();
            foreach (SupplierInvioceItemCertificatPM itemPM in supplierInvoiceItemCertificatesschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvioceItemCertificatPM currentItemPM = currententityPm.SupplierInvioceItemCertificats.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.ItemCertificateCounterKey == itemPM.ItemCertificateCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvioceItemCertificatPM currentItemPM = currententityPm.SupplierInvioceItemCertificats.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.ItemCertificateCounterKey == itemPM.ItemCertificateCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvioceItemCertificatPM currentItemPM = new SupplierInvioceItemCertificatPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, ItemCertificateCounterKey = itemPM.ItemCertificateCounterKey };
                            currententityPm.DeletedSupplierInvioceItemCertificats.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvioceItemCertificatPM currentItemPM = currententityPm.SupplierInvioceItemCertificats.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.ItemCertificateCounterKey == itemPM.ItemCertificateCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemVehicleChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclechangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemVehicles).Cast<SupplierInvoiceItemVehiclePM>().ToList();
            foreach (SupplierInvoiceItemVehiclePM itemPM in supplierInvoiceItemVehiclechangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemVehiclePM currentItemPM = currententityPm.SupplierInvoiceItemVehicles.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetSupplierInvoiceItemVehicleModChangeSet(itemPM);
                            SetSupplierInvoiceItemVehicleAddChangeSet(itemPM); // moran 15.3.16 - AMI-55746
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemVehiclePM currentItemPM = currententityPm.SupplierInvoiceItemVehicles.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetSupplierInvoiceItemVehicleModChangeSet(itemPM);
                            SetSupplierInvoiceItemVehicleAddChangeSet(itemPM); // moran 15.3.16 - AMI-55746
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemVehiclePM currentItemPM = new SupplierInvoiceItemVehiclePM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber, ExcludeFromInterface = itemPM.ExcludeFromInterface };
                            currententityPm.DeletedSupplierInvoiceItemVehicles.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;

                            List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleModChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemVehicleMods).Cast<SupplierInvoiceItemVehicleModPM>().ToList();

                            foreach (SupplierInvoiceItemVehicleModPM item in supplierInvoiceItemVehicleModChangeset)
                            {
                                SupplierInvoiceItemVehicleModPM deletedItem = new SupplierInvoiceItemVehicleModPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    VehicleLineNumber = item.VehicleLineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemVehicleMods.Add(deletedItem);
                            }
                            // moran 15.3.16 - AMI-55746 -->
                            List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAddChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.SupplierInvoiceItemVehicleAdds).Cast<SupplierInvoiceItemVehicleAddPM>().ToList();

                            foreach (SupplierInvoiceItemVehicleAddPM item in supplierInvoiceItemVehicleAddChangeset)
                            {
                                SupplierInvoiceItemVehicleAddPM deletedItem = new SupplierInvoiceItemVehicleAddPM()
                                {
                                    InvoiceCounterKey = item.InvoiceCounterKey,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    InvoiceItemLineNumber = item.InvoiceItemLineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };

                                currentItemPM.DeletedSupplierInvoiceItemVehicleAdds.Add(deletedItem);
                            }
                            // moran 15.3.16 - AMI-55746 <--

                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemVehiclePM currentItemPM = currententityPm.SupplierInvoiceItemVehicles.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemModVehicleChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehiclechangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemModVehicles).Cast<SupplierInvoiceItemModVehiclePM>().ToList();
            foreach (SupplierInvoiceItemModVehiclePM itemPM in supplierInvoiceItemModVehiclechangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemModVehiclePM currentItemPM = currententityPm.SupplierInvoiceItemModVehicles.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.AdjustmentTypeCode == itemPM.AdjustmentTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemModVehiclePM currentItemPM = currententityPm.SupplierInvoiceItemModVehicles.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.AdjustmentTypeCode == itemPM.AdjustmentTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemModVehiclePM currentItemPM = new SupplierInvoiceItemModVehiclePM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber, AdjustmentTypeCode = itemPM.AdjustmentTypeCode };
                            currententityPm.DeletedSupplierInvoiceItemModVehicles.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;




                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemModVehiclePM currentItemPM = currententityPm.SupplierInvoiceItemModVehicles.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.AdjustmentTypeCode == itemPM.AdjustmentTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemModificationsChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemsModPM> supplierInvoiceItemCertificatesschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemsMods).Cast<SupplierInvoiceItemsModPM>().ToList();
            foreach (SupplierInvoiceItemsModPM itemPM in supplierInvoiceItemCertificatesschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsModPM currentItemPM = currententityPm.SupplierInvoiceItemsMods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.ModificationCounterKey == itemPM.ModificationCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsModPM currentItemPM = currententityPm.SupplierInvoiceItemsMods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.ModificationCounterKey == itemPM.ModificationCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsModPM currentItemPM = new SupplierInvoiceItemsModPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, ModificationCounterKey = itemPM.ModificationCounterKey };
                            currententityPm.DeletedSupplierInvoiceItemsMods.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsModPM currentItemPM = currententityPm.SupplierInvoiceItemsMods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.ModificationCounterKey == itemPM.ModificationCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemProcessTypesChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemProcesTypePM> supplierInvoiceItemCertificatesschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemProcesTypes).Cast<SupplierInvoiceItemProcesTypePM>().ToList();
            foreach (SupplierInvoiceItemProcesTypePM itemPM in supplierInvoiceItemCertificatesschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemProcesTypePM currentItemPM = currententityPm.SupplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemProcesTypePM currentItemPM = currententityPm.SupplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemProcesTypePM currentItemPM = new SupplierInvoiceItemProcesTypePM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemProcesTypes.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemProcesTypePM currentItemPM = currententityPm.SupplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemSerialNumberChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemSerialNumberchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemsSerialNums).Cast<SupplierInvoiceItemsSerialNumPM>().ToList();
            foreach (SupplierInvoiceItemsSerialNumPM itemPM in supplierInvoiceItemSerialNumberchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsSerialNumPM currentItemPM = currententityPm.SupplierInvoiceItemsSerialNums.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsSerialNumPM currentItemPM = currententityPm.SupplierInvoiceItemsSerialNums.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsSerialNumPM currentItemPM = new SupplierInvoiceItemsSerialNumPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber, };
                            currententityPm.DeletedSupplierInvoiceItemsSerialNums.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsSerialNumPM currentItemPM = currententityPm.SupplierInvoiceItemsSerialNums.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemProductIdentificationChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemProductIdentificationchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemsProdIdents).Cast<SupplierInvoiceItemsProdIdentPM>().ToList();
            foreach (SupplierInvoiceItemsProdIdentPM itemPM in supplierInvoiceItemProductIdentificationchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsProdIdentPM currentItemPM = currententityPm.SupplierInvoiceItemsProdIdents.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsProdIdentPM currentItemPM = currententityPm.SupplierInvoiceItemsProdIdents.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsProdIdentPM currentItemPM = new SupplierInvoiceItemsProdIdentPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemsProdIdents.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsProdIdentPM currentItemPM = currententityPm.SupplierInvoiceItemsProdIdents.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemDescriptionChangeSet(SupplierInvoiceItemPM currententityPm)
        {
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemDescriptchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemsDescripts).Cast<SupplierInvoiceItemsDescriptPM>().ToList();
            foreach (SupplierInvoiceItemsDescriptPM itemPM in supplierInvoiceItemDescriptchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemsDescriptPM currentItemPM = currententityPm.SupplierInvoiceItemsDescripts.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemsDescriptPM currentItemPM = currententityPm.SupplierInvoiceItemsDescripts.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemsDescriptPM currentItemPM = new SupplierInvoiceItemsDescriptPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemsDescripts.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemsDescriptPM currentItemPM = currententityPm.SupplierInvoiceItemsDescripts.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceFreightAmounts(SupplierInvoicePM currententityPm)
        {
            List<SupplierInvoiceFreightAmountPM> supplierInvoiceFreightAmountschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceFreightAmounts).Cast<SupplierInvoiceFreightAmountPM>().ToList();
            foreach (SupplierInvoiceFreightAmountPM itemPM in supplierInvoiceFreightAmountschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceFreightAmountPM currentItemPM = currententityPm.SupplierInvoiceFreightAmounts.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.CurrencyTypeCode == itemPM.CurrencyTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceFreightAmountPM currentItemPM = currententityPm.SupplierInvoiceFreightAmounts.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.CurrencyTypeCode == itemPM.CurrencyTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceFreightAmountPM currentItemPM = new SupplierInvoiceFreightAmountPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, InvoiceCounterKey = itemPM.InvoiceCounterKey, CurrencyTypeCode = itemPM.CurrencyTypeCode };
                            currententityPm.DeletedSupplierInvoiceFreightAmounts.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceFreightAmountPM currentItemPM = currententityPm.SupplierInvoiceFreightAmounts.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.CurrencyTypeCode == itemPM.CurrencyTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }

        private void SetSupplierInvoiceItemVehicleModChangeSet(SupplierInvoiceItemVehiclePM currententityPm)
        {
            List<SupplierInvoiceItemVehicleModPM> supplierInvoiceItemVehicleModchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemVehicleMods).Cast<SupplierInvoiceItemVehicleModPM>().ToList();
            foreach (SupplierInvoiceItemVehicleModPM itemPM in supplierInvoiceItemVehicleModchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemVehicleModPM currentItemPM = currententityPm.SupplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.VehicleLineNumber == itemPM.VehicleLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemVehicleModPM currentItemPM = currententityPm.SupplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.VehicleLineNumber == itemPM.VehicleLineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemVehicleModPM currentItemPM = new SupplierInvoiceItemVehicleModPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber, VehicleLineNumber = itemPM.VehicleLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemVehicleMods.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemVehicleModPM currentItemPM = currententityPm.SupplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.VehicleLineNumber == itemPM.VehicleLineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }


        private void SetSupplierInvoiceItemVehicleAddChangeSet(SupplierInvoiceItemVehiclePM currententityPm) // moran 15.3.16 - AMI-55746
        {
            List<SupplierInvoiceItemVehicleAddPM> supplierInvoiceItemVehicleAddchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.SupplierInvoiceItemVehicleAdds).Cast<SupplierInvoiceItemVehicleAddPM>().ToList();
            foreach (SupplierInvoiceItemVehicleAddPM itemPM in supplierInvoiceItemVehicleAddchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            SupplierInvoiceItemVehicleAddPM currentItemPM = currententityPm.SupplierInvoiceItemVehicleAdds.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            SupplierInvoiceItemVehicleAddPM currentItemPM = currententityPm.SupplierInvoiceItemVehicleAdds.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            SupplierInvoiceItemVehicleAddPM currentItemPM = new SupplierInvoiceItemVehicleAddPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, LineNumber = itemPM.LineNumber, InvoiceCounterKey = itemPM.InvoiceCounterKey, InvoiceItemLineNumber = itemPM.InvoiceItemLineNumber };
                            currententityPm.DeletedSupplierInvoiceItemVehicleAdds.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            SupplierInvoiceItemVehicleAddPM currentItemPM = currententityPm.SupplierInvoiceItemVehicleAdds.Where(d => d.DeclarationId == itemPM.DeclarationId && d.InvoiceCounterKey == itemPM.InvoiceCounterKey && d.LineNumber == itemPM.LineNumber && d.InvoiceItemLineNumber == itemPM.InvoiceItemLineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }

        }


        public SupplierInvoicePM GetSingleSupplierInvoiceByInvoiceNumber(string invoiceNumber, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            supplierInvoiceQuery = new SupplierInvoiceQueryService(customContext);
            SupplierInvoicePM SupplierInvoice = supplierInvoiceQuery.GetSupplierInvoiceByNumber(invoiceNumber, tenant);
            return SupplierInvoice;
        }

        [Invoke]
        public int GetInvoiceItemsWithTradeAgreementCount(string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationQuery = new DeclarationQueryService(customContext);
            return declarationQuery.GetInvoiceItemsWithTradeAgreementCount(declarationId, tenant);
        }

        public List<SupplierInvoiceFreightAmountList> GetSupplierInvoiceFreightAmountList(string declarationId, int invoiceCounterKey, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceFreightAmountListQueryService listService = new SupplierInvoiceFreightAmountListQueryService(customContext);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.SetFilter("DeclarationId", declarationId, false, "Equals", null, false);
            queryOperations.SetFilter("InvoiceCounterKey", invoiceCounterKey, false, "Equals", null, false);
            queryOperations.PageIndex = 0;
            queryOperations.PageSize = 100;

            return listService.GetList(queryOperations, tenant);
        }

        public List<SupplierInvoiceItemList> GetSupplierInvoiceItemsForInvoices(string declarationId, string supplierInvoiceCounterKeys, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemListQueryService queryService = new SupplierInvoiceItemListQueryService(customContext);
            return queryService.GetSupplierInvoiceItemsForInvoices(declarationId, supplierInvoiceCounterKeys, tenant);
        }

        

        [Invoke]
        public string GetInsurancePercentDefault(string customerCode, int tenant)
        {
            if (string.IsNullOrWhiteSpace(customerCode) || tenant == null)
            {
                return null;
            }

            SecurityUtility.AuthenticationOnTenant(tenant);
       
            string insurancePercent = GetDefault("ISRAEL", "CIM_INSUR_PERC", "NON", customerCode, tenant);
            return insurancePercent;
        }
    }
}