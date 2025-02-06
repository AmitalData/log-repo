using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.BL.EntityPMs.UGenerated;
using System.Transactions;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.BL.Security;
using System.Data.Entity;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Logitude.Customs.Data.EntityMapping;
/*using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;*/

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceItemUpdateService : EntityUpdateService<SupplierInvoiceItem, SupplierInvoiceItemPM, SupplierInvoicePM>
    {

        protected override void OnCreating(SupplierInvoiceItemPM entityPM, SupplierInvoicePM entityParentPM)
        {
            if (entityPM.CurrentContextTag != null && entityPM.CurrentContextTag.ToString() == "ACCUMULATION" && entityPM.LineNumber > 0) return;
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.CounterKey = entityParentPM.InvoiceCounterKey;

            if (!entityPM.IsCopy && entityPM.LastCopyFromOrderNo != "10000" && entityPM.LineNumber ==0)
            {
                entityParentPM.InvoiceItemLastLineNumber += 1;
                entityPM.LineNumber = entityParentPM.InvoiceItemLastLineNumber;
            }

            if (!entityPM.IsCopy)
            {
                entityPM.OrderByLineNo = entityPM.LineNumber.ToString();
                if (entityPM.SequenceNumeric == null)// bug number35869
                {
                    entityPM.SequenceNumeric = entityPM.LineNumber;
                }
            }
            if (entityPM.Tenant < 1)
            {
                throw new BusinessErrorException("Tenant '" + entityPM.Tenant + "' Can't be less than 1 (OnCreating)");
            }
        }

        public void DoOnCreating(SupplierInvoiceItemPM entityPM, SupplierInvoicePM entityParentPM)
        {
            OnCreating(entityPM, entityParentPM);
        }

        protected override void UpdateComposition(SupplierInvoiceItemPM entityPM)
        {

            //SupplierInvoiceItemsQuantityUpdateService supplierInvoiceItemsQuantityUpdateService = new SupplierInvoiceItemsQuantityUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            //supplierInvoiceItemsQuantityUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsQuantities, entityPM.DeletedSupplierInvoiceItemsQuantities, entityPM, false);

            SupplierInvoiceItemsTaxUpdateService supplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsTaxUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemTaxes, entityPM.DeletedSupplierInvoiceItemTaxes, entityPM, false);

            SupplierInvoiceItemsConDeclarUpdateService supplierInvoiceItemsConnectedDeclarationUpdateService = new SupplierInvoiceItemsConDeclarUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsConnectedDeclarationUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsConDeclars, entityPM.DeletedSupplierInvoiceItemsConDeclars, entityPM, false);

            SupplierInvioceItemCertificatUpdateService supplierInvioceItemsCertificateUpdateService = new SupplierInvioceItemCertificatUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvioceItemsCertificateUpdateService.UpdateMulti(entityPM.SupplierInvioceItemCertificats, entityPM.DeletedSupplierInvioceItemCertificats, entityPM, false);

            SupplierInvoiceItemsModUpdateService supplierInvoiceItemsModificationUpdateService = new SupplierInvoiceItemsModUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsModificationUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsMods, entityPM.DeletedSupplierInvoiceItemsMods, entityPM, false);


            SupplierInvoiceItemsSerialNumUpdateService supplierInvoiceItemsSerialNumberUpdateService = new SupplierInvoiceItemsSerialNumUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsSerialNumberUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsSerialNums, entityPM.DeletedSupplierInvoiceItemsSerialNums, entityPM, false);

            SupplierInvoiceItemsProdIdentUpdateService supplierInvoiceItemsProductIdentificationUpdateService = new SupplierInvoiceItemsProdIdentUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsProductIdentificationUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsProdIdents, entityPM.DeletedSupplierInvoiceItemsProdIdents, entityPM, false);


            SupplierInvoiceItemsDescriptUpdateService supplierInvoiceItemsDescriptionUpdateService = new SupplierInvoiceItemsDescriptUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsDescriptionUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsDescripts, entityPM.DeletedSupplierInvoiceItemsDescripts, entityPM, false);

            SupplierInvoiceItemProcesTypeUpdateService supplierInvoiceItemsProcessTypeUpdateService = new SupplierInvoiceItemProcesTypeUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsProcessTypeUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemProcesTypes, entityPM.DeletedSupplierInvoiceItemProcesTypes, entityPM, false);

            SupplierInvoiceItemsLevyUpdateService supplierInvoiceItemsLevyUpdateService = new SupplierInvoiceItemsLevyUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsLevyUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemLevies, entityPM.DeletedSupplierInvoiceItemLevies, entityPM, false);

            SupplierInvoiceItemVehicleUpdateService supplierInvoiceItemVehicleUpdateService = new SupplierInvoiceItemVehicleUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemVehicleUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemVehicles, entityPM.DeletedSupplierInvoiceItemVehicles, entityPM, false);

            SupplierInvoiceItemModVehicleUpdateService supplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemModVehicleUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemModVehicles, entityPM.DeletedSupplierInvoiceItemModVehicles, entityPM, false);

            SuppInvoiceItemsAbachStatementUpdateService suppInvoiceItemsAbachStatementUpdateService = new SuppInvoiceItemsAbachStatementUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            suppInvoiceItemsAbachStatementUpdateService.UpdateMulti(entityPM.SuppInvoiceItemsAbachStatements, entityPM.DeletedSuppInvoiceItemsAbachStatements, entityPM, false);

            SupplierInvoiceItemsPriceUpdateService supplierInvoiceItemsPriceUpdateService = new SupplierInvoiceItemsPriceUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            supplierInvoiceItemsPriceUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsPrices, entityPM.DeletedSupplierInvoiceItemsPrices, entityPM, false);



            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(SupplierInvoiceItemPM entityPM, SupplierInvoicePM entityParentPM)
        {

            //if ((entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Delete)&&entityParentPM.ChangeSetOp!=ChangeSetOperation.Delete)
            //{
            //    SubmitChanges();
            //    ICustomContext context = MainContext as CustomContext;
            //    SupplierInvoiceItemRepository invoiceItemRepository = new SupplierInvoiceItemRepository(context);
            //    List<SupplierInvoiceItem> supplierInvoices = invoiceItemRepository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.CounterKey });


            //    bool dirty = false;
            //    int index = 0;
            //    foreach (SupplierInvoiceItem item in supplierInvoices)
            //    {
            //        index += 1;
            //        if (item.SequenceNumeric == index) continue; //itzik 
            //        dirty = true; //itzik 
            //        item.SequenceNumeric = index;
            //        invoiceItemRepository.Update(item);
            //        if (item.DeclarationId == entityPM.DeclarationId && item.CounterKey == entityPM.CounterKey && item.LineNumber==entityPM.LineNumber)
            //        {
            //            entityPM.SequenceNumeric = item.SequenceNumeric;
            //        }
            //    }
            //    if (dirty)
            //    {
            //        invoiceItemRepository.SubmitChanges();
            //    }
            //}
        }

        protected override void OnUpdating(SupplierInvoiceItemPM entityPM, SupplierInvoiceItem entityPOCO)
        {
            DateTime stopLogAt = new DateTime(2020, 06, 01);
            string logData = "";
            DeclarationPM declarationPM = null;
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);

            if (entityPM.ClassificationCode != entityPOCO.ClassificationCode)
            {
                var loggedUser = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                logData = $"entityPM.ClassificationCode(New value)={entityPM.ClassificationCode},entityPOCO.ClassificationCode(Old value)={entityPOCO.ClassificationCode}, User name={loggedUser}"; 
                LogitudeSettings.HandleLogMe("ClassificationCode changed " + logData, false, "SupplierInvoiceItemUpdate.ClassificationCode", stopLogAt);                
            }

            if (SecurityUtility.CheckFeature("Customs.Declaration", "OCR", entityPM.Tenant) && !string.IsNullOrEmpty(entityPM.ItemCode) || !string.IsNullOrEmpty(entityPM.ItemDescription)) 
            {
                if(string.IsNullOrEmpty(entityPM.ClassificationCode))
                {
                    ClientItemQueryService clientItemQueryService = new ClientItemQueryService(entityPM.Tenant);
                    declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, true);
                    
                    if (declarationPM != null && declarationPM?.Direction == "E" && !string.IsNullOrEmpty(declarationPM.ExporterImporterCode))
                    {
                        ClientItemPM clientItem = clientItemQueryService.GetSingleWithTenant(entityPM.ItemCode, declarationPM.ExporterImporterCode, entityPM.Tenant);
                        if(clientItem != null)
                        {
                            entityPM.ClassificationCode = clientItem?.ClassificationCode;
                            if(string.IsNullOrEmpty(entityPM.ItemDescription))
                                entityPM.ItemDescription = clientItem?.ItemDescription;
                            if (string.IsNullOrEmpty(entityPM.OriginCountryCode))
                                entityPM.OriginCountryCode = clientItem?.OriginCountryCode;

                            if (!string.IsNullOrEmpty(entityPM.ClassificationCode))
                            {

                                CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(entityPM.Tenant);
                                string quantityType = customsItemQueryService.GetQuantityTypeByClassificationWithMultiCustomItems(entityPM.ClassificationCode, entityPM.Tenant, true);
                                if (!string.IsNullOrEmpty(quantityType))
                                {
                                    //entityPM.StatisticQuantityType = quantityType;
                                    //entityPM.StatisticQuantity = entityPM?.InvoiceQuantity;
                                    //entityPM.ItemAdditionalStatus = true;
                                    //if (string.IsNullOrEmpty(entityPM.InvoiceQuantityType))
                                        entityPM.InvoiceQuantityType = quantityType;

                                }
                            }
                        }
                    }

                }
                else 
                {
                    if (entityPM.ClassificationCode != entityPOCO.ClassificationCode || entityPM.ItemCode != entityPOCO.ItemCode || entityPM.OriginCountryCode != entityPOCO.OriginCountryCode || entityPM.ItemDescription != entityPOCO.ItemDescription)
                    {
                        declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, true);
                        if (declarationPM != null)
                        {
                            if (declarationPM.Direction == "E")
                                this.UpdateOrInsertInClientItems(entityPM, declarationPM?.ExporterImporterCode, declarationPM.ImporterId);
                        }

                    }
                   
                }



            }

            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void OnUpdating(SupplierInvoiceItemPM entityPM)
        {
            bool isValid = true;
            bool hasRequest = false;
             if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                ICustomContext context = this.MainContext as CustomContext;
                CustomsDocumentsTicketQueryService ticketsQueryService = new CustomsDocumentsTicketQueryService(context);
                CustomsDocumentsTicketUpdateService ticketsUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                List<CustomsDocumentsTicketPM> tickets = ticketsQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityPM.DeclarationId, entityPM.CounterKey.ToString(), entityPM.LineNumber.ToString(), null, entityPM.Tenant, "Declaration");
                foreach (CustomsDocumentsTicketPM ticket in tickets)
                {

                    List<CustomsDocumentPointerPM> deletedPointers = (from a in ticket.CustomsDocumentPointers
                                                                      where a.Child1EntityId == entityPM.CounterKey.ToString() && a.Child2EntityId == entityPM.LineNumber.ToString()
                                                                      select a).ToList();



                    foreach (CustomsDocumentPointerPM pointer in deletedPointers)
                    {
                        CustomsDocumentPointerPM deletedPointer = new CustomsDocumentPointerPM() { Id = pointer.Id, ChangeSetOp = ChangeSetOperation.Delete };
                        ticket.DeletedCustomsDocumentPointers.Add(deletedPointer);
                    }

                    if (ticket.CustomsDocumentPointers.Count == ticket.DeletedCustomsDocumentPointers.Count)
                    {
                        ticket.ChangeSetOp = ChangeSetOperation.Delete;
                    }
                    else
                    {
                        ticket.ChangeSetOp = ChangeSetOperation.Update;
                    }

                    if (deletedPointers.Count != 0)
                    {
                        ticketsUpdateService.Update(ticket, true);
                    }
                }
            }
            else
            {
                ICommonDataContext myContext = CommonDataContext.GetContext(entityPM.Tenant);
                FeatureRepository myFeatureRepository = new FeatureRepository(myContext);
                FeatureQuery featureQuery = new FeatureQuery(myFeatureRepository);
                var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(entityPM.Tenant), entityPM.Tenant);
                var feature = features.Features.FirstOrDefault(x => x.Code == "REFERANTWORKSPACE");

                if (!string.IsNullOrWhiteSpace(entityPM.ClasifiedRemarks)  && feature!=null)
                {
                    DeclarationReferantDataUpdate(entityPM);
                }

               
                if (entityPM.IsItemChanged && !string.IsNullOrWhiteSpace(entityPM.ItemCode))
                {
                    var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
                    if (setting != null && setting.IsConnectedToUniFreight)
                    {


                        TransactionScope scope = null;
                        if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                        {
                            scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                        }
                        try
                        {
                            using (var myAmitalContext = AmitalContext.GetContext(entityPM.Tenant))
                            {
                                UpsertCustomsPartnersItems(myAmitalContext, entityPM);
                                myAmitalContext.SaveChanges();
                            }
                            if (scope != null)
                            {
                                scope.Complete();
                            }
                        }
                        finally
                        {
                            if (scope != null)
                            {
                                scope.Dispose();
                            }
                        }
                    }

                  
                }
            }
            bool notdeleted = entityPM.SupplierInvioceItemCertificats.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Any();
            if (entityPM.SupplierInvioceItemCertificats.Count == 0 || !notdeleted)
            {
                  entityPM.CertificatesStatusCode = null;
            }

            else
            {
                foreach (SupplierInvioceItemCertificatPM item in entityPM.SupplierInvioceItemCertificats.Where(x => x.ChangeSetOp != ChangeSetOperation.Delete))
                {
                    if (!string.IsNullOrWhiteSpace(item.ApprovalRequestNumber))
                    {
                        hasRequest = true;
                    }
                    if (item.AttachmentTypeCode == null)
                    {
                        isValid = false;
                        break;
                    }

                    else
                    {
                        if (item.AttachmentTypeCode == "1" || item.AttachmentTypeCode == "2")
                        {
                            if (entityPM.Direction == "E")
                            {
                                if (string.IsNullOrWhiteSpace(item.CertificateNumber) || string.IsNullOrWhiteSpace(item.ReqConfirmationTypeCode) || string.IsNullOrWhiteSpace(item.ResConfirmationTypeCode) || !string.IsNullOrWhiteSpace(item.CertificateExemptionTypeCode)  )
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(item.CertificateNumber) || string.IsNullOrWhiteSpace(item.ReqConfirmationTypeCode) || string.IsNullOrWhiteSpace(item.ResConfirmationTypeCode) || !string.IsNullOrWhiteSpace(item.CertificateExemptionTypeCode) || !string.IsNullOrWhiteSpace(item.CustomsAttachmentID))
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                          

                        }

                        else
                        {
                            if (item.AttachmentTypeCode == "4")
                            {
                                if (string.IsNullOrWhiteSpace(item.CertificateExemptionTypeCode) || string.IsNullOrWhiteSpace(item.ReqConfirmationTypeCode) || !string.IsNullOrWhiteSpace(item.CertificateNumber) || !string.IsNullOrWhiteSpace(item.ResConfirmationTypeCode) || !string.IsNullOrWhiteSpace(item.CustomsAttachmentID))
                                {
                                    isValid = false;
                                    break;
                                }
                            }

                        }
                    }


                }

                if (!isValid)
                {
                    if (hasRequest)
                    {
                        entityPM.CertificatesStatusCode = "4";
                    }
                    else
                    {
                        entityPM.CertificatesStatusCode = "2";
                    }
                }
                else
                {
                    if (hasRequest)
                    {
                        entityPM.CertificatesStatusCode = "3";
                    }
                    else
                    {
                        entityPM.CertificatesStatusCode = "1";
                    }
                }

            }
            //var mySIAccumulationUtil = new SIAccumulationUtil();
            //string itemHash = 
            if (entityPM.ItemHash != null && (entityPM.CurrentContextTag == null || entityPM.CurrentContextTag.ToString() != "ACCUMULATION"))
            {
                object entityPOCO; object entityPM1; object entityParentPM;
                this.GetAncestor(out entityPOCO, out entityPM1, out entityParentPM);
                var mySI = (entityPM1 as SupplierInvoicePM);
                if (mySI == null)
                {
                    var myDec = (entityPM1 as DeclarationPM);
                    if (myDec != null) mySI = myDec.SupplierInvoices.Where(rec => rec.InvoiceCounterKey == entityPM.CounterKey).FirstOrDefault();
                }
                if (mySI == null || mySI.AccumalationStateCode != "3")
                {
                    SupplierInvoiceItemDataMapping supplierInvoiceItemDataMapping = new SupplierInvoiceItemDataMapping();// removed the calchash from pm because i moved the pm to the .def project
                    string ItemHash = supplierInvoiceItemDataMapping.CalcHash(entityPM);// circular refrence problem.
                    if (ItemHash != entityPM.ItemHash) entityPM.ItemHash = null;
                }
            }
            if (entityPM.Tenant < 1)
            {
                throw new BusinessErrorException("Tenant '" + entityPM.Tenant + "' Can't be less than 1 (OnUpdating)");
            }
        }

        private void DeclarationReferantDataUpdate(SupplierInvoiceItemPM entityPM)
        {
            if (string.IsNullOrWhiteSpace(entityPM.ClasifiedRemarks)) return;

            ICustomContext _context = this.MainContext as CustomContext;
            var myDeclarationReferantDataQueryService = new DeclarationReferantDataQueryService(_context);
            DeclarationReferantDataPM declarationReferantDataPM = myDeclarationReferantDataQueryService.GetSingle(entityPM.DeclarationId, true, false);
            
            if (declarationReferantDataPM == null)return;

            if(declarationReferantDataPM.IsClassificationRemarks) return;

            var myDeclarationReferantDataUpdateService = new DeclarationReferantDataUpdateService(_context, new Dictionary<string, IContext>(), entityPM.Tenant);
            declarationReferantDataPM.IsClassificationRemarks = true;
            declarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationReferantDataUpdateService.Update(declarationReferantDataPM, true);

        }

        private void UpsertCustomsPartnersItems(AmitalContext myAmitalContext, SupplierInvoiceItemPM supplierInvoiceItem)
        {
            var queryService = new GTBITEMQueryService(myAmitalContext);
            var updateService = new GTBITEMUpdateService(myAmitalContext);

            ICustomContext context = this.MainContext as CustomContext;
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);

            string partnerId = "";
            DeclarationKeys entityKeys = new DeclarationKeys() { Id = supplierInvoiceItem.DeclarationId, };
            DeclarationPM myDeclarationPM;
            //object entityPOCO; object entityPM; object entityParentPM;
            //this.GetAncestor(out entityPOCO, out entityPM, out entityParentPM);
            //myDeclarationPM = (entityPM as DeclarationPM);
            var myQueryService = new DeclarationQueryService(context);
            myDeclarationPM = myQueryService.GetSingle(supplierInvoiceItem.DeclarationId, false, true);
            if (!myDeclarationPM.IsConnectedToUnifreight) return;
            if (myDeclarationPM != null)
            {
                partnerId = myDeclarationPM.CustomerCode;
            }
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(supplierInvoiceItem.Tenant);

            string partnerDefault = defaultValueQueryService.GetDefault("ISRAEL", "CIM_SIVUG_103", "NON", partnerId, supplierInvoiceItem.Tenant); // S=Supplier I=Client
            if (partnerDefault == "S") // If Supplier get Unifreight card
            {
                partnerId = defaultValueQueryService.GetDefaultAccountNumber("ISRAEL", "CEX_CUS_SUP", "NON", partnerId, supplierInvoiceItem.Tenant);
            }

            if (string.IsNullOrWhiteSpace(partnerId) || string.IsNullOrWhiteSpace(supplierInvoiceItem.ItemCode))
            {
                return;
            }

            Unifreight.BL.EntityPMs.GTBITEMPM myGTBITEMPM = queryService.GetSingle(partnerId, supplierInvoiceItem.ItemCode, false); // Not From Cache        
            if (myGTBITEMPM != null)
            {
                myGTBITEMPM.ChangeSetOp = ChangeSetOperation.Update;
            }
            else
            {
                myGTBITEMPM = new Unifreight.BL.EntityPMs.GTBITEMPM();
                myGTBITEMPM.ChangeSetOp = ChangeSetOperation.Insert;
                myGTBITEMPM.PARTNERID = partnerId;
                myGTBITEMPM.ITEMID = supplierInvoiceItem.ItemCode;
            }

            //<--- Yuval Chalup 04.12.2016 TASK-24655
            var myTempGTBITEMPM = new Unifreight.BL.EntityPMs.GTBITEMPM()
            {
                ChangeSetOp = myGTBITEMPM.ChangeSetOp,
                PARTNERID = myGTBITEMPM.PARTNERID,
                ITEMID = myGTBITEMPM.ITEMID,
                PRATID = myGTBITEMPM.PRATID,
                DESCRIPTION = myGTBITEMPM.DESCRIPTION,
                SEARCHENG = myGTBITEMPM.SEARCHENG,
            };
            SupplierInvoicePM mySupplierInvoicePM = this.EntityParentPM as SupplierInvoicePM;
            if (mySupplierInvoicePM.GTBITEMsToUpdate == null)
            {
                mySupplierInvoicePM.GTBITEMsToUpdate = new List<Unifreight.BL.EntityPMs.GTBITEMPM>();
            }
            mySupplierInvoicePM.GTBITEMsToUpdate.Add(myTempGTBITEMPM);
            //Yuval Chalup 04.12.2016 TASK-24655  

            myGTBITEMPM.PRATID = supplierInvoiceItem.ClassificationCode;
            myGTBITEMPM.DESCRIPTION = supplierInvoiceItem.ItemDescription;
            if (!string.IsNullOrEmpty(supplierInvoiceItem.ItemDescription))
            {
                myGTBITEMPM.SEARCHENG = supplierInvoiceItem.ItemDescription.ToUpper();
            }

            updateService.Update(myGTBITEMPM, true);
        }


       

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemRepository).FastDeleteMulti(entityKeyFields);
        }
   
        public void DeclarationSupplierInvoiceItemsParentsFastDeleteComposition(Logitude.Customs.Data.EntityKeys.SupplierInvoiceKeys entityKeyFields, ICustomContext dbContext, int tenant, bool isParent = true)
        {

            List<int> supplierInvoiceItemsParentsLines = GetSupplierInvoiceItemsParents(entityKeyFields.DeclarationId, entityKeyFields.InvoiceCounterKey, dbContext, isParent);
            if (supplierInvoiceItemsParentsLines == null || supplierInvoiceItemsParentsLines.Count() < 1) return;
            var mySupplierInvoiceItemsConDeclarUpdateService = new SupplierInvoiceItemsConDeclarUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceItemsConDeclarUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsConDeclarRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemsDescriptUpdateService = new SupplierInvoiceItemsDescriptUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemsDescriptUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsDescriptRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceItemsModUpdateService = new SupplierInvoiceItemsModUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceItemsModUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsModRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemProcesTypeUpdateService = new SupplierInvoiceItemProcesTypeUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemProcesTypeUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemProcesTypeRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemsProdIdentUpdateService = new SupplierInvoiceItemsProdIdentUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemsProdIdentUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsProdIdentRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemsSerialNumUpdateService = new SupplierInvoiceItemsSerialNumUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemsSerialNumUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsSerialNumRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvioceItemCertificatUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvioceItemCertificatRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemsTaxUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsTaxRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemsLevyUpdateService = new SupplierInvoiceItemsLevyUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemsLevyUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsLevyRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemVehicleModUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleModRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceItemVehicleAddUpdateService = new SupplierInvoiceItemVehicleAddUpdateService(dbContext, new Dictionary<string, IContext>(), tenant); // moran 14.3.16 - AMI-55746
            mySupplierInvoiceItemVehicleAddUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleAddRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemVehicleUpdateService = new SupplierInvoiceItemVehicleUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemVehicleUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var mySupplierInvoiceInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceInvoiceItemModVehicleUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemModVehicleRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);

            var mySupplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            mySupplierInvoiceItemUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            //(Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
            var supplierInvoiceItemsPriceUpdateService = new SupplierInvoiceItemsPriceUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
            supplierInvoiceItemsPriceUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);

            var suppInvoiceItemsAbachStatementUpdateService = new  SuppInvoiceItemsAbachStatementUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
             suppInvoiceItemsAbachStatementUpdateService.FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);

        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }

        public List<int> GetSupplierInvoiceItemsParents(string declarationId, int invoiceCounterKey, ICustomContext dbContext, bool isParent)
        {
            return (from a in dbContext.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey && a.IsParent == isParent
                    select a.LineNumber).ToList();
        }

        private void UpdateOrInsertInClientItems(SupplierInvoiceItemPM entityPM, string exporterCode, string exporterId)
        {
            ICustomContext context = this.MainContext as CustomContext;
            ClientItemQueryService clientItemQueryService = new ClientItemQueryService(entityPM.Tenant);
            ClientItemUpdateService clientItemUpdateServicev = new ClientItemUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

            if (entityPM != null)
            {
                if (!string.IsNullOrEmpty(entityPM.ItemCode) || !string.IsNullOrEmpty(entityPM.ItemDescription))
                {
                    string ItemKey = entityPM.ItemCode + "_" + entityPM.ItemDescription;

                    if (entityPM.ChangeSetOp != ChangeSetOperation.None && !string.IsNullOrEmpty(ItemKey))
                    {
                        ClientItemPM clientItem = clientItemQueryService.GetSingleWithTenantByItemKey(ItemKey, exporterCode, entityPM.Tenant);
                        if (clientItem == null)
                        {
                            clientItem = new ClientItemPM()
                            {
                                ItemCode = entityPM.ItemCode,
                                ItemKey = ItemKey,
                                Tenant = entityPM.Tenant,
                                ItemDescription = entityPM.ItemDescription,
                                ClassificationCode = entityPM.ClassificationCode,
                                OriginCountryCode = entityPM.OriginCountryCode,
                                ClientCode = exporterCode,
                                ChangeSetOp = ChangeSetOperation.Insert

                            };

                        }
                        else
                        {
                            clientItem.ItemDescription = entityPM.ItemDescription;
                            clientItem.ClassificationCode = entityPM.ClassificationCode;
                            clientItem.OriginCountryCode = entityPM.OriginCountryCode;
                            clientItem.ChangeSetOp = ChangeSetOperation.Update;
                        }

                        clientItemUpdateServicev.Update(clientItem, true);


                    }
                }
            }
        }


    }
}

