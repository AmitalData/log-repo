
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class RevaluationDataMapping: IMapping<RevaluationPM, Revaluation>
   {

        public void CustomPMToPOCO(RevaluationPM entityPM, Revaluation entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(RevaluationPM entityPM, Revaluation entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.GLAccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.GLAccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ChartOfAccountsName);


            if (entityPOCO.GLAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM account = gLAccountQueryService.GetSingle(entityPOCO.GLAccountId, false, true);
                if (account != null)
                {
                    entityPM.GLAccountName = account.EnglishName;
                    entityPM.GLAccountNumber = account.DisplayNumber;
                  
                }
            }

            if (entityPOCO.CreatedByUserId != null)
            {
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
                if (userContact != null)
                {
                    entityPM.CreatedByUserName = userContact.LocalName;
                }
            }
            if (entityPOCO.Status != null)
            {
                RevaluationStatusQueryService revaluationStatusQueryService = new RevaluationStatusQueryService(entityPOCO.Tenant);
                RevaluationStatusPM revaluationStatusPM = revaluationStatusQueryService.GetSingle(entityPOCO.Status, false, false);
                if (revaluationStatusPM != null)
                {
                    ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.StatusName = revaluationStatusPM.Name;
                    }
                    else { entityPM.StatusName = revaluationStatusPM.LocalName; }

                }

            }

            if (entityPOCO.ChartOfAccountsId != null)
            {
                ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(entityPOCO.Tenant);
                ChartOfAccountPM chartOfAccounts = chartOfAccountQueryService.GetSingle(entityPOCO.ChartOfAccountsId, false, true);
                if (chartOfAccounts != null) entityPM.ChartOfAccountsName = chartOfAccounts.EnglishName;
            }

        }

        private ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }


}
   