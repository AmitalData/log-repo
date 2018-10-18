using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public DepositPM GetSingleDepositPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            depositQuery = new DepositQueryService(customContext);
            DepositPM Deposit = depositQuery.GetSingle(id, true, false);
            return Deposit;
        }

        public DepositList GetSingleDepositList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.Deposit", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DepositListQueryService listService = new DepositListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public DepositPM GetDepositPMByPaymentOrderNumberOrTapagId(string paymentNumber, string tapagId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            depositQuery = new DepositQueryService(customContext);
            DepositPM Deposit = depositQuery.GetDepositByPaymentOrderNumberOrTapagId(paymentNumber, tapagId, tenant);
            return Deposit;
        }

        public List<DepositList> GetDepositLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.Deposit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DepositListQueryService listService = new DepositListQueryService(customContext);
            return listService.GetList(tenant);
            return new List<DepositList>();
        }


        public List<DepositList> GetDepositFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
       //     SecurityUtility.CheckContactFeature("Customs.Deposit", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            DepositListQueryService listService = new DepositListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDepositFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.Deposit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DepositListQueryService queryService = new DepositListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertDeposit(DepositPM entityPm)
        {
          //  SecurityUtility.CheckContactFeature("Customs.Deposit", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            DepositUpdateService service = new DepositUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
           
            service.Update(entityPm, true);

        }

        public void UpdateDeposit(DepositPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Deposit", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            DepositUpdateService service = new DepositUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetDepositConditionChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }


        private void SetDepositConditionChangeSet(DepositPM currententityPm)
        {
            List<DepositConditionPM> depositConditionchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.DepositConditions).Cast<DepositConditionPM>().ToList();
            foreach (DepositConditionPM itemPM in depositConditionchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            DepositConditionPM currentItemPM = currententityPm.DepositConditions.Where(d => d.DepositId == itemPM.DepositId && d.DepositConditionCode == itemPM.DepositConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert; 
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            DepositConditionPM currentItemPM = currententityPm.DepositConditions.Where(d => d.DepositId == itemPM.DepositId && d.DepositConditionCode == itemPM.DepositConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                          

                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            DepositConditionPM currentItemPM = new DepositConditionPM() { ChangeSetOp = ChangeSetOperation.Delete, DepositId = itemPM.DepositId, DepositConditionCode = itemPM.DepositConditionCode };
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;

                            break;
                        }
                    default:
                        {
                            DepositConditionPM currentItemPM = currententityPm.DepositConditions.Where(d => d.DepositId == itemPM.DepositId && d.DepositConditionCode == itemPM.DepositConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }


        public void UpdateDepositList(DepositList list)
        {


        }
    }
}