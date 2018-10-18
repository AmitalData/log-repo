using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomBankPM GetSingleCustomBankPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customBankQuery = new CustomBankQueryService(customContext);
            CustomBankPM CustomBank = customBankQuery.GetSingle(id, true, false);
            return CustomBank;
        }

        public CustomBankList GetSingleCustomBankList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           SecurityUtility.CheckContactFeature("Customs.CustomBank", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomBankListQueryService listService = new CustomBankListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomBankList> GetCustomBanksForCard(string cardId, int tenant)
        {

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customBankQuery = new CustomBankQueryService(customContext);
            List<CustomBankList> customBanks = customBankQuery.GetCustomBanksByCard(cardId, tenant);



            return customBanks;

        }

        public CustomBanksCardPM GetSingleCustomBankCard(string bankId, string cardId, int tenant)
        {


            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            CustomBanksCardQueryService customBanksCardQuery = new CustomBanksCardQueryService(customContext);
            CustomBanksCardPM customBanksCard = customBanksCardQuery.GetSingleCustomBanksCard(bankId, cardId, tenant);
            return customBanksCard;
        }

        public List<CustomBankList> GetCustomBankLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomBank", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomBankListQueryService listService = new CustomBankListQueryService(customContext);
            return listService.GetList(tenant);
        }

        public List<CustomBankList> GetCustomBankFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           SecurityUtility.CheckContactFeature("Customs.CustomBank", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomBankListQueryService listService = new CustomBankListQueryService(customContext);
    
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomBankFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomBank", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomBankListQueryService queryService = new CustomBankListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public bool DoesCustomBankCodeExist(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            CustomBankListQueryService listService = new CustomBankListQueryService(customContext);
            return (listService.GetList(tenant).Where(d => d.InternalCode == code && d.Tenant == tenant)).Any();

        }

        public void InsertCustomBank(CustomBankPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomBank", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomBankUpdateService service = new CustomBankUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (CustomBanksCardPM CustomBankCard in entityPm.CustomBanksCards)
            {
                CustomBankCard.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPm, true);



        }

        public void UpdateCustomBank(CustomBankPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomBank", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomBankUpdateService service = new CustomBankUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetCustomBankCardChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }


        private void SetCustomBankCardChangeSet(CustomBankPM currententityPm)
        {
            List<CustomBanksCardPM> customBanksCardsChangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.CustomBanksCards).Cast<CustomBanksCardPM>().ToList();
            foreach (CustomBanksCardPM itemPM in customBanksCardsChangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CustomBanksCardPM currentItemPM = currententityPm.CustomBanksCards.Where(d => d.CustomBankId == itemPM.CustomBankId && d.CardId == itemPM.CardId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            CustomBanksCardPM currentItemPM = currententityPm.CustomBanksCards.Where(d => d.CustomBankId == itemPM.CustomBankId && d.CardId == itemPM.CardId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            CustomBanksCardPM currentItemPM = new CustomBanksCardPM() { ChangeSetOp = ChangeSetOperation.Delete,Id = itemPM.Id, CustomBankId = itemPM.CustomBankId , CardId = itemPM.CardId };
                            currententityPm.DeletedCustomBanksCards.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;


                            break;
                        }
                    default:
                        {
                            CustomBanksCardPM currentItemPM = currententityPm.CustomBanksCards.Where(d => d.CustomBankId == itemPM.CustomBankId && d.CardId == itemPM.CardId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdateCustomBankList(CustomBankList entity)
        {
 
        }


        [Invoke]
        public string GetCustomBankDefaultForCard(string customerCode, int tenant)
        {
            if (string.IsNullOrWhiteSpace(customerCode) || tenant == null)
            {
                return null;
            }

            SecurityUtility.AuthenticationOnTenant(tenant);

            string customBankDefault = GetDefault("ISRAEL", "CIM_AGENT_BANK", "NON", customerCode, tenant);
            return customBankDefault;
        }
    }
}