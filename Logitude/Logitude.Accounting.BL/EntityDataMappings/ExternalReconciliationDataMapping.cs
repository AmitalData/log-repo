
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

            entityPOCO.SearchFields = entityPM.ReconciliationNumber!= null ? entityPM.ReconciliationNumber.ToString() : entityPM.SearchFields;
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
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        private static ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

    }


}
   