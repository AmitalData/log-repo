
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ExternalReconciliationDataMapping: IMapping<ExternalReconciliationPM, ExternalReconciliation>
   {

        public void CustomPMToPOCO(ExternalReconciliationPM entityPM, ExternalReconciliation entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            //entityPOCO.SearchFields = entityPM.ReconciliationNumber!= null ? entityPM.ReconciliationNumber.ToString() : entityPM.SearchFields;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);

        }

        public void CustomPOCOToPM(ExternalReconciliationPM entityPM, ExternalReconciliation entityPOCO)
        {

            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;

            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.AccountName);
            CustomMappedPMProperties.Add(PMPropertyNames.AccountNumber);

            if (entityPOCO.CreatedByUserId != null)
            {
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
                if (userContact != null)
                {
                    entityPM.CreatedByUserName = (showLocals ? userContact.LocalName : userContact.EnglishName);
                    

                }
            }

            if (entityPOCO.GLAccountId != null)
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM account = query.GetSingle(entityPOCO.GLAccountId, false, false);

                if (account != null)
                {
                    entityPM.AccountName = account.LocalName;
                    entityPM.AccountNumber = account.DisplayNumber;
                    entityPM.AccountCurrencyId = (account.IsMultiCurrency == true ? "multi" : account.CurrencyId);
                }
            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
        }

        public static void BuildSearchFields(ExternalReconciliationPM entityPM, ExternalReconciliation poco, bool isNewEntity)
        {
            string result = "";

            // header
            if (entityPM.ReconciliationNumber != null)
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ReconciliationNumber.ToString() : result + "," + entityPM.ReconciliationNumber.ToString();
            }

            if (entityPM.AccountNumber != null)
            {
                result = string.IsNullOrEmpty(result) ? entityPM.AccountNumber.ToString() : result + "," + entityPM.AccountNumber.ToString();
            }


            //lines
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
            ReconcileExternalPageLineQueryService pageLineQuery = new ReconcileExternalPageLineQueryService(entityPM.Tenant);

            List<string> LedgerTransactionIds = entityPM.ExternalReconciliationLines.Where(d => d.LedgerTransactionId != null).Select(d => d.LedgerTransactionId).ToList();
            List<string> PageLineIds = entityPM.ExternalReconciliationLines.Where(d => d.ExternalPageLineId != null).Select(d => d.ExternalPageLineId).ToList();

            List<LedgerTransactionPM> LedgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(LedgerTransactionIds, entityPM.Tenant);
            List<ReconcileExternalPageLinePM> PageLines = pageLineQuery.GetPageLinesPMsByIdList(PageLineIds, entityPM.Tenant);

            foreach (LedgerTransactionPM item in LedgerTransactions)
            {
                if (!string.IsNullOrEmpty(item.Reference1))
                {
                    if (!(result.Split(',').Contains(item.Reference1)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference1 : result + "," + item.Reference1;
                    }
                }

                if (!string.IsNullOrEmpty(item.Reference2))
                {
                    if (!(result.Split(',').Contains(item.Reference2)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference2 : result + "," + item.Reference2;
                    }
                }

                if (!string.IsNullOrEmpty(item.Reference3))
                {
                    if (!(result.Split(',').Contains(item.Reference3)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference3 : result + "," + item.Reference3;
                    }
                }
            }

            foreach (ReconcileExternalPageLinePM item in PageLines)
            {
                if (!string.IsNullOrEmpty(item.Reference))
                {
                    if (!(result.Split(',').Contains(item.Reference)))
                    {
                        result = string.IsNullOrEmpty(result) ? item.Reference : result + "," + item.Reference;
                    }
                }
            }

            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }




        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }


}
   