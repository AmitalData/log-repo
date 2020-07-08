
namespace WebFreight.Web.CustomModel.DomainServices
{

    using Logitude.BL.CommonDataModel.EntityPMs;
    using Logitude.BL.CommonDataModel.EntityQueries;
    using Logitude.Customs.Def.EntityPMs;
    using Logitude.Customs.BL.EntityQueryServices;
    using Logitude.Customs.BL.EntityUpdateServices;
    using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection;
    using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
    using Logitude.Customs.Data;
    using Logitude.Customs.Data.DataContracts;
    using Logitude.Customs.Data.EntityListQueryServices;
    using Logitude.Customs.Data.EntityLists;
    using Logitude.Customs.Data.Repsitories;
    using Logitude.Server.Tools.Helpers;
    using Simplog.Server.Infrastructure;
    using Simplog.Server.Infrastructure.DataContracts;
    using Simplog.Server.Infrastructure.Helpers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using WebFreight.Web.Security;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public partial class DeclarationDomainService : DomainService
    {
        ICustomContext customContext;
        DeclarationQueryService declarationQuery;
        ConsignmentQueryService consignmentQuery;

        [Query(HasSideEffects = true)]
        public List<DeclarationList> GetDeclarationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);


        }

        public int GetDeclarationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationListQueryService queryService = new DeclarationListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public List<DeclarationReferantDataList> GetDeclarationReferantDataFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationReferantDataListQueryService listService = new DeclarationReferantDataListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);


        }

        public int GetDeclarationReferantDataFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationReferantDataListQueryService queryService = new DeclarationReferantDataListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }


        public List<CourierMasterList> GetCourierMasterFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            var listService = new CourierMasterListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);


        }

        public int GetCourierMasterFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            var queryService = new CourierMasterListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }


        public DeclarationPM GetSingleDeclarationPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationQuery = new DeclarationQueryService(customContext);
            // declarationQuery.LoadSupplierInvoices = true;
            declarationQuery.LoadSupplierInvoicesWithItems = false;
            DeclarationPM Declaration = declarationQuery.GetSingle(id, true, false);
            return Declaration;
        }

        //<--- Yuval Chalup 22.10.2014 TASK-6711
        [Query]
        public List<ConsignmentPM> GetConsignmentListPMByCustomFileNo(string customFileNo, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }

            customContext = CustomContext.GetContext(tenant);
            DeclarationRepository declarationRep = new DeclarationRepository(customContext);

            string DeclarationId = declarationRep.GetIdByCustomFileNo(customFileNo, tenant);
            if (string.IsNullOrWhiteSpace(DeclarationId))
            {
                return null;
            }
            declarationQuery = new DeclarationQueryService(customContext);
            List<ConsignmentPM> myConsignmentPM = declarationQuery.GetConsignmentListPMByDeclarationId(DeclarationId, tenant);

            return myConsignmentPM;
        }

        [Query]
        public List<DeclarationPendingPM> GetDeclarationPendingListPMByDeclarationId(string declarationId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }

            customContext = CustomContext.GetContext(tenant);
            DeclarationRepository declarationRep = new DeclarationRepository(customContext);

            if (string.IsNullOrWhiteSpace(declarationId))
            {
                return null;
            }
            declarationQuery = new DeclarationQueryService(customContext);
            List<DeclarationPendingPM> myDeclarationPendingPM = declarationQuery.GetDeclarationPendingListPMByDeclarationId(declarationId, tenant);

            return myDeclarationPendingPM;
        }

        [Query]
        public List<DeclarationPM> GetSingleDeclarationPMByCargoIdentifiers(string cargoTypeCode, string manifestNumber, string secondCargoID, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }

            declarationQuery = new DeclarationQueryService(customContext);
            var declarationPM = declarationQuery.GetDeclarationPMByCargoIdentifiers(cargoTypeCode, manifestNumber, secondCargoID, tenant);
            if (declarationPM == null)
            {
                return null;
            }
            var myList = new List<DeclarationPM>();
            myList.Add(declarationPM);
            return myList;
        }
        //Yuval Chalup 22.10.2014 TASK-6711 --->

        public DeclarationList GetSingleDeclarationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }

            customContext = CustomContext.GetContext(tenant);
            DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public DeclarationList GetSingleDeclarationByCustomFileNo(string customFileNo, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationRepository declarationRep = new DeclarationRepository(customContext);
            string id = declarationRep.GetIdByCustomFileNo(customFileNo, tenant);

            declarationQuery = new DeclarationQueryService(customContext);
            DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public DeclarationList GetSingleDeclarationByNumber(string declarationByNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationRepository declarationRep = new DeclarationRepository(customContext);
            string id = declarationRep.GetIdByDeclarationNumber(declarationByNumber, tenant);

            declarationQuery = new DeclarationQueryService(customContext);
            DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DeclarationList> GetDeclarationLists(int tenant)
        {
            
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("Customs.Declaration", "READ", tenant);
                customContext = CustomContext.GetContext(tenant);
                DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
                return listService.GetList(tenant);

            }
        }

        public DeclarationCorrectionView GetDeclarationCorrection(string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationQuery = new DeclarationQueryService(customContext);
            return declarationQuery.GetDeclarationCorrection(declarationId, tenant);

        }


        public List<DeclarationList> GetDeclarationByTapagConnectionConnection(string tapagId, int tenant)
        {

            customContext = CustomContext.GetContext(tenant);

            declarationQuery = new DeclarationQueryService(customContext);
            return declarationQuery.GetTapagDeclarations(tapagId, tenant);
        }


        public void InsertDeclaration(DeclarationPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            string useremail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(entityPm.Tenant);
            ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(useremail, entityPm.Tenant, true);

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(useremail, entityPm.Tenant);
            }
            entityPm.CreatedByUserId = loggedContact.Id;
            entityPm.ReferentUserId = entityPm.CreatedByUserId;
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateDeclaration(DeclarationPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Declaration", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            currententityPm.MarkAsChanged = true;
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            
            SetConsignmentChangeSet(currententityPm);
            SetDeclarationTaxChangeSet(currententityPm);
            

            service.Update(currententityPm, true);
            var cacheKey = "DeclarationPM.RequiredVldAfterUpdate" + currententityPm.Id;
            CacheManager.CacheWrapper.Insert(cacheKey, currententityPm, null, DateTime.UtcNow.AddSeconds(10), TimeSpan.Zero);

        }


        public List<SupplierInvoiceCurrency> GetCurrenciesCodesForDeclaration(string declarationId, int tenant)
        {
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }

            declarationQuery = new DeclarationQueryService(customContext);
            List<SupplierInvoicePM> invoices = declarationQuery.GetInvoicesByDeclaration(declarationId, tenant);
            List<SupplierInvoiceCurrency> result = (from a in invoices
                                                    group a by new { a.ExchangeRate, a.InvoiceCurrencyTypeCode } into gr
                                                    select new SupplierInvoiceCurrency()
                                                    {
                                                        Id = Guid.NewGuid().ToString(),
                                                        ExchangeRate = gr.Key.ExchangeRate,
                                                        InvoiceCurrencyId = gr.Key.InvoiceCurrencyTypeCode,

                                                    }).ToList();
            return result;
        }




        private void SetDeclarationConstraintChangeSet(DeclarationPM currententityPm)
        {
            List<DeclarationConstraintPM> declarationConstraintchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.DeclarationConstraints).Cast<DeclarationConstraintPM>().ToList();
            foreach (DeclarationConstraintPM itemPM in declarationConstraintchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            DeclarationConstraintPM currentItemPM = currententityPm.DeclarationConstraints.Where(d => d.DeclarationID == itemPM.DeclarationID && d.ConstraintNumber == itemPM.ConstraintNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            DeclarationConstraintPM currentItemPM = currententityPm.DeclarationConstraints.Where(d => d.DeclarationID == itemPM.DeclarationID && d.ConstraintNumber == itemPM.ConstraintNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;

                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            DeclarationConstraintPM currentItemPM = new DeclarationConstraintPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationID = itemPM.DeclarationID, ConstraintNumber = itemPM.ConstraintNumber };
                            currententityPm.DeletedDeclarationConstraints.Add(currentItemPM);
                            break;
                        }
                    default:
                        {
                            DeclarationConstraintPM currentItemPM = currententityPm.DeclarationConstraints.Where(d => d.DeclarationID == itemPM.DeclarationID && d.ConstraintNumber == itemPM.ConstraintNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

      
        private void SetDeclarationTaxChangeSet(DeclarationPM currententityPm)
        {
            List<DeclarationTaxPM> declarationTaxchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.DeclarationTaxes).Cast<DeclarationTaxPM>().ToList();
            foreach (DeclarationTaxPM itemPM in declarationTaxchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            DeclarationTaxPM currentItemPM = currententityPm.DeclarationTaxes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.TaxTypeCode == itemPM.TaxTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            DeclarationTaxPM currentItemPM = currententityPm.DeclarationTaxes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.TaxTypeCode == itemPM.TaxTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;

                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            DeclarationTaxPM currentItemPM = new DeclarationTaxPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, TaxTypeCode = itemPM.TaxTypeCode };



                            currententityPm.DeletedDeclarationTaxes.Add(currentItemPM);
                            break;
                        }
                    default:
                        {
                            DeclarationTaxPM currentItemPM = currententityPm.DeclarationTaxes.Where(d => d.DeclarationId == itemPM.DeclarationId && d.TaxTypeCode == itemPM.TaxTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetConsignmentChangeSet(DeclarationPM currententityPm)
        {
            List<ConsignmentPM> Consignmentchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.Consignments).Cast<ConsignmentPM>().ToList();
            foreach (ConsignmentPM itemPM in Consignmentchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ConsignmentPM currentItemPM = currententityPm.Consignments.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetConsignmentPackagesChangeSet(currentItemPM);
                            SetConsignmentInternalTransitionChangeSet(currentItemPM);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ConsignmentPM currentItemPM = currententityPm.Consignments.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetConsignmentPackagesChangeSet(currentItemPM);
                            SetConsignmentInternalTransitionChangeSet(currentItemPM);
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ConsignmentPM currentItemPM = new ConsignmentPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, ConsignmentNumber = itemPM.ConsignmentNumber };

                            List<ConsignmentPackagePM> ConsignmentPackageschangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.ConsignmentPackages).Cast<ConsignmentPackagePM>().ToList();
                            foreach (ConsignmentPackagePM item in ConsignmentPackageschangeset)
                            {
                                ConsignmentPackagePM deletedItem = new ConsignmentPackagePM()
                                {
                                    ConsignmentNumber = item.ConsignmentNumber,
                                    DeclarationId = item.DeclarationId,
                                    LineNumber = item.LineNumber,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                };
                                currentItemPM.DeletedConsignmentPackages.Add(deletedItem);
                            }


                            List<ConsignmentInternalTransitionPM> transitionsChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ConsignmentInternalTransitions).Cast<ConsignmentInternalTransitionPM>().ToList();
                            foreach (ConsignmentInternalTransitionPM item in transitionsChangeSet)
                            {
                                ConsignmentInternalTransitionPM deletedItem = new ConsignmentInternalTransitionPM()
                                {
                                    ConsignmentNumber = item.ConsignmentNumber,
                                    DeclarationId = item.DeclarationId,
                                    SiteCode = item.SiteCode,
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    LineNumber = item.LineNumber
                                };
                                currentItemPM.DeletedConsignmentInternalTransitions.Add(deletedItem);
                            }

                            currententityPm.DeletedConsignments.Add(currentItemPM);
                            break;
                        }
                    default:
                        {
                            ConsignmentPM currentItemPM = currententityPm.Consignments.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetConsignmentPackagesChangeSet(ConsignmentPM currententityPm)
        {
            List<ConsignmentPackagePM> ConsignmentPackageschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.ConsignmentPackages).Cast<ConsignmentPackagePM>().ToList();
            foreach (ConsignmentPackagePM itemPM in ConsignmentPackageschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ConsignmentPackagePM currentItemPM = currententityPm.ConsignmentPackages.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ConsignmentPackagePM currentItemPM = currententityPm.ConsignmentPackages.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ConsignmentPackagePM currentItemPM = new ConsignmentPackagePM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, ConsignmentNumber = itemPM.ConsignmentNumber, LineNumber = itemPM.LineNumber };
                            currententityPm.DeletedConsignmentPackages.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            ConsignmentPackagePM currentItemPM = currententityPm.ConsignmentPackages.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetConsignmentInternalTransitionChangeSet(ConsignmentPM currententityPm)
        {
            List<ConsignmentInternalTransitionPM> ConsignmentInternalTransitionchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.ConsignmentInternalTransitions).Cast<ConsignmentInternalTransitionPM>().ToList();
            foreach (ConsignmentInternalTransitionPM itemPM in ConsignmentInternalTransitionchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ConsignmentInternalTransitionPM currentItemPM = currententityPm.ConsignmentInternalTransitions.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber && d.SiteCode == itemPM.SiteCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ConsignmentInternalTransitionPM currentItemPM = currententityPm.ConsignmentInternalTransitions.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber && d.SiteCode == itemPM.SiteCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ConsignmentInternalTransitionPM currentItemPM = new ConsignmentInternalTransitionPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, ConsignmentNumber = itemPM.ConsignmentNumber, SiteCode = itemPM.SiteCode, LineNumber = itemPM.LineNumber, Tenant = itemPM.Tenant };
                            currententityPm.DeletedConsignmentInternalTransitions.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            ConsignmentInternalTransitionPM currentItemPM = currententityPm.ConsignmentInternalTransitions.Where(d => d.DeclarationId == itemPM.DeclarationId && d.ConsignmentNumber == itemPM.ConsignmentNumber && d.SiteCode == itemPM.SiteCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdateDeclarationList(DeclarationList list)
        {

        }

        public void UpdateDeclarationConsignmentPM(DeclarationConsignmentPM entityPM)
        {

        }

        public ConsignmentPM GetSingleConsignmentPM(string declarationId, int? consignmentNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            consignmentQuery = new ConsignmentQueryService(customContext);
            ConsignmentPM Consignment = consignmentQuery.GetSingle(declarationId, consignmentNumber, true, false);
            return Consignment;
        }

        public void InsertDeclarationConsignment(DeclarationConsignmentPM entityPM)
        {

        }


        public List<DeclarationErrorView> GetDeclarationErrors(string declarationId, int tenant, string listVersionId)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationQuery = new DeclarationQueryService(customContext);
            return declarationQuery.GetDeclarationErrors(declarationId, tenant, listVersionId);
        }

        public List<DeclarationConstraintPM> GetDeclarationConstraints(string declarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            DeclarationConstraintQueryService declarationConstraintQueryService = new DeclarationConstraintQueryService(customContext);
            List<DeclarationConstraintPM> constraints = declarationConstraintQueryService.GetDeclarationConstraintsByDeclrationId(declarationId, tenant);
            return constraints;
        }
       
        public bool CheckIfCustomFileNoExists(string customFileNo, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationQuery = new DeclarationQueryService(customContext);
            string declarationId = declarationQuery.GetIdByCustomFileNo(customFileNo, tenant);
            bool exists = false;
            if (!string.IsNullOrEmpty(declarationId))
            {
                exists = true;
            }
            return exists;
        }

        //<--- Yuval Chalup 17.12.2015 TASK-18939
        [Invoke]
        public string ResetDeclarationNumber(string declarationId, int tenant)
        {
            return DeclarationUpdateService.ResetDeclarationNumber(declarationId, tenant);
            try
            {
                customContext = CustomContext.GetContext(tenant);
                declarationQuery = new DeclarationQueryService(customContext);
                DeclarationPM myDeclarationPM = declarationQuery.GetSingle(declarationId, false, false);

                //If the Declaration exists
                if (myDeclarationPM == null) return null;

                //Update ExternalDeclarationNumber
                string externalDeclarationNumber = null;
                int pos = myDeclarationPM.ExternalDeclarationNumber.IndexOf("-");
                if (pos == -1)
                {
                    externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber + "-1";
                }
                else
                {
                    if (pos + 1 >= myDeclarationPM.ExternalDeclarationNumber.Length)
                    {
                        externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber.Substring(0, myDeclarationPM.ExternalDeclarationNumber.Length - 1) + "-1";
                    }
                    else
                    {
                        int after;
                        if (int.TryParse(myDeclarationPM.ExternalDeclarationNumber.Substring(pos + 1), out after))
                        {
                            after++;
                            externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber.Substring(0, pos) + "-" + after.ToString();
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(externalDeclarationNumber))
                {
                    myDeclarationPM.ExternalDeclarationNumber = externalDeclarationNumber;
                }

                var traceEventParams = new EventTracerArgs()
                {
                    EntityId = myDeclarationPM.Id,
                    ObjectTableName = "Customs.Declaration",
                    Tenant = myDeclarationPM.Tenant,
                    UserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                    EventTypeCode = "DNR",
                    Notes = "Reset Declaration Number." + Environment.NewLine + "Old DeclarationNumber: " + myDeclarationPM.DeclarationNumber + Environment.NewLine + "Old VersionId: " + myDeclarationPM.VersionId,
                };
                EventTracer.CreateTraceEvent(traceEventParams);

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode= " + "DNR" + "CustomFileNo= " + myDeclarationPM.CustomFileNo + "  ");

                myDeclarationPM.DeclarationNumber = null;
                myDeclarationPM.VersionId = null;
                myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                myDeclarationPM.MarkAsChanged = true;
                myDeclarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
                service.Update(myDeclarationPM, true);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
     
        //Mohammad moved copy declaration to server because of the supplier invoice and items 
        [Invoke]
        public bool CopyDeclaration(string fromDeclarationId, string toDeclarationId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            return service.CopyDeclaration(fromDeclarationId, toDeclarationId, tenant);


        }

        public List<DeclarationVehicleModification> GetDeclarationVehicleModifications(string declarationId, string chassisNumber, string adjustmentTypeCode, int tenant)
        {
            SupplierInvoiceItemVehicleRepository rep = new SupplierInvoiceItemVehicleRepository(tenant);
            List<DeclarationVehicleModification> mods = rep.GetDeclarationVehicleModifications(declarationId, chassisNumber, adjustmentTypeCode, tenant);
            return mods;
        }

        public void UpdateSupplierInvoicePM(SupplierInvoicePM entityPM)
        {
        }

        public void DeleteSupplierInvoicepM(SupplierInvoicePM entityPM)
        {
 
        }

    }
}


