using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure;
using System.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        [Invoke]
        public string GetDefBankForCustomer(string CustomerCode, int tenant) // moran 9.3.17 - task 26276
        {
            string bank = "";
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (!string.IsNullOrWhiteSpace(CustomerCode))
            {
                if (customContext == null)
                {
                    customContext = CustomContext.GetContext(tenant);
                }

                var declarationQS = new DeclarationQueryService(customContext);
                bank = declarationQS.GetDefault("ISRAEL", "CIM_AGENT_BANK", "NON", CustomerCode, tenant);
            }
            return bank;
        }

        public DeclarationPaymentPM GetSingleDeclarationPaymentPMandDefaultExplain(string id, string CustomerCode, int tenant) // moran 3.1.17 - AMI-58876
        {
            var declarationPaymentPM  = GetSingleDeclarationPaymentPM(id, tenant);
            if (!string.IsNullOrWhiteSpace(CustomerCode))
            {
                if (declarationPaymentPM == null || declarationPaymentPM.DeclarationPaymentProtests == null || declarationPaymentPM.DeclarationPaymentProtests.Count == 0 || string.IsNullOrWhiteSpace(declarationPaymentPM.DeclarationPaymentProtests.FirstOrDefault().CustomsAgentExplanation))
                {
                    var declarationQS = new DeclarationQueryService(customContext);

                    string customsAgentExplanationDefault = declarationQS.GetDefault("ISRAEL", "CIM_PROTEST_PAY", "NON", CustomerCode, tenant);

                    if (!string.IsNullOrWhiteSpace(customsAgentExplanationDefault))
                    {
                        if (declarationPaymentPM == null)
                        {
                            declarationPaymentPM = new DeclarationPaymentPM()
                            {
                                DeclarationId = id,
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,
                                CustomsAgentExplanationDefault = customsAgentExplanationDefault,
                            };
                        }

                        else if (declarationPaymentPM.DeclarationPaymentProtests == null || declarationPaymentPM.DeclarationPaymentProtests.Count() == 0 || string.IsNullOrWhiteSpace(declarationPaymentPM.DeclarationPaymentProtests.FirstOrDefault().CustomsAgentExplanation))
                        {
                            declarationPaymentPM.CustomsAgentExplanationDefault = customsAgentExplanationDefault;
                        }
                    }
                }
            }
            return declarationPaymentPM;
        }

        public DeclarationPaymentPM GetSingleDeclarationPaymentPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            declarationPaymentQuery = new DeclarationPaymentQueryService(customContext);
            DeclarationPaymentPM declarationPayment = declarationPaymentQuery.GetSingle(id, true, false);
            return declarationPayment;
        }

        public DeclarationPaymentList GetSingleDeclarationPaymentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DeclarationPayment", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DeclarationPaymentListQueryService listService = new DeclarationPaymentListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<DeclarationPaymentList> GetDeclarationPaymentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DeclarationPayment", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationPaymentListQueryService listService = new DeclarationPaymentListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DeclarationPaymentList> GetDeclarationPaymentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DeclarationPayment", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DeclarationPaymentListQueryService listService = new DeclarationPaymentListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDeclarationPaymentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.DeclarationPayment", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DeclarationPaymentListQueryService queryService = new DeclarationPaymentListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertDeclarationPayment(DeclarationPaymentPM entityPm)
        {
            ////SecurityUtility.CheckContactFeature("Customs.DeclarationPayment", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            DeclarationPaymentUpdateService service = new DeclarationPaymentUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (DeclarationPaymentMethodPM declarationPaymentMethod in entityPm.DeclarationPaymentMethods)
            {
                declarationPaymentMethod.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }

            foreach (DeclarationPaymentProtestPM declarationPaymentProtest in entityPm.DeclarationPaymentProtests)
            {
                declarationPaymentProtest.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPm, true);

        }

        public void UpdateDeclarationPayment(DeclarationPaymentPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.DeclarationPayment", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            DeclarationPaymentUpdateService service = new DeclarationPaymentUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetDeclarationPaymentMethodChangeSet(currententityPm);
            SetDeclarationPaymentProtestChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetDeclarationPaymentMethodChangeSet(DeclarationPaymentPM currententityPm)
        {
            List<DeclarationPaymentMethodPM> declarationPaymentMethodchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.DeclarationPaymentMethods).Cast<DeclarationPaymentMethodPM>().ToList();
            foreach (DeclarationPaymentMethodPM itemPM in declarationPaymentMethodchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            DeclarationPaymentMethodPM currentItemPM = currententityPm.DeclarationPaymentMethods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            DeclarationPaymentMethodPM currentItemPM = currententityPm.DeclarationPaymentMethods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            DeclarationPaymentMethodPM currentItemPM = new DeclarationPaymentMethodPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, Line = itemPM.Line };
                            currententityPm.DeletedDeclarationPaymentMethods.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            DeclarationPaymentMethodPM currentItemPM = currententityPm.DeclarationPaymentMethods.Where(d => d.DeclarationId == itemPM.DeclarationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetDeclarationPaymentProtestChangeSet(DeclarationPaymentPM currententityPm)
        {
            List<DeclarationPaymentProtestPM> declarationPaymentProtestchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.DeclarationPaymentProtests).Cast<DeclarationPaymentProtestPM>().ToList();
            foreach (DeclarationPaymentProtestPM itemPM in declarationPaymentProtestchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            DeclarationPaymentProtestPM currentItemPM = currententityPm.DeclarationPaymentProtests.Where(d => d.DeclarationId == itemPM.DeclarationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            DeclarationPaymentProtestPM currentItemPM = currententityPm.DeclarationPaymentProtests.Where(d => d.DeclarationId == itemPM.DeclarationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            DeclarationPaymentProtestPM currentItemPM = new DeclarationPaymentProtestPM() { ChangeSetOp = ChangeSetOperation.Delete, DeclarationId = itemPM.DeclarationId, Line = itemPM.Line };
                            currententityPm.DeletedDeclarationPaymentProtests.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            DeclarationPaymentProtestPM currentItemPM = currententityPm.DeclarationPaymentProtests.Where(d => d.DeclarationId == itemPM.DeclarationId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdateDeclarationPaymentList(DeclarationPaymentList list)
        {

        }


    }
}