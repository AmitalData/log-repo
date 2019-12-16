
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class GLAccountInterestPeriodDataMapping: IMapping<GLAccountInterestPeriodPM, GLAccountInterestPeriod>
   {

        public void CustomPMToPOCO(GLAccountInterestPeriodPM entityPM, GLAccountInterestPeriod entityPOCO)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
     
            GLAccountInterestPeriodRepository PeriodRepository = new GLAccountInterestPeriodRepository(entityPM.Tenant);
            GLAccountInterestPeriod Period = PeriodRepository.GetSingleByPeriodStartDateeAndnterestGLAccountId(entityPM.PeriodStartDate, entityPM.GLAccountId, entityPM.Tenant);
            if (Period != null)
                if (entityPM.PeriodStartDate != entityPOCO.PeriodStartDate)
            {
                
                {   if(entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Update)
                    throw new Exception(TextCodesTranslator.TranslateText("Accounting.General.O.LineDateExist", entityPM.Tenant, showLocals));
                }
            }

            if( Math.Floor(Math.Log10((double)entityPM.StandardAddInterestPercent) + 1) > 2)
            {
                throw new Exception("Number Of Digit Before Comma Must Be Two Or Less In Standard Add Interest Percent");

            }

            if (Math.Floor(Math.Log10((double)entityPM.CreditAddInterestPercent) + 1) > 2)
            {
                throw new Exception("Number Of Digit Before Comma Must Be Two Or Less In Credit Add Interest Percent");

            }

            if (Math.Floor(Math.Log10((double)entityPM.ExceptionalAddInterestPercent) + 1) > 2)
            {
                throw new Exception("Number Of Digit Before Comma Must Be Two Or Less In Exceptional Add Interest Percent");

            }

            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.GLAccountId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdatedByUserId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdateDateTime);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreateDateTime);

            DateTime CurrentDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
     
                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    entityPOCO.CreatedByUserId = loggedContact.Id;
                    entityPOCO.CreateDateTime = CurrentDate;
                    entityPOCO.LineNumber = entityPM.LineNumber;
                    entityPOCO.GLAccountId = entityPM.GLAccountId;
                    entityPOCO.Tenant = entityPM.Tenant;
                    entityPOCO.UpdatedByUserId = loggedContact.Id;
                    entityPOCO.UpdateDateTime = CurrentDate;
                }

                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
                {
                    entityPOCO.UpdatedByUserId = loggedContact.Id;
                    entityPOCO.UpdateDateTime = CurrentDate;
                }
 
           
        }

        public void CustomPOCOToPM(GLAccountInterestPeriodPM entityPM, GLAccountInterestPeriod entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            // GET logged contact, RTL
            ContactQuery contactQuery = new ContactQuery(entityPOCO.Tenant);
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;

            InterestBasesTypeQueryService interestBasesTypeQueryService = new InterestBasesTypeQueryService(entityPOCO.Tenant);
            if (entityPOCO.CreditInterestRateBaseId != null)
            {
                InterestBasesTypePM CreditInterestRateBase = interestBasesTypeQueryService.GetSingle(entityPOCO.CreditInterestRateBaseId,false,true);
                if (CreditInterestRateBase != null)
                    entityPM.CreditInterestRateBaseName = CreditInterestRateBase.LocalName;
            }
            if (entityPOCO.ExceptionalInterestRateBaseId != null)
            {
                InterestBasesTypePM ExceptionalInterestRateBase = interestBasesTypeQueryService.GetSingle(entityPOCO.ExceptionalInterestRateBaseId, false, true);
                if (ExceptionalInterestRateBase != null)
                    entityPM.ExceptionalInterestRateName = ExceptionalInterestRateBase.LocalName;
            }
            if (entityPOCO.StandardInterestRateBaseId != null)
            {
                InterestBasesTypePM StandardInterestRateBase = interestBasesTypeQueryService.GetSingle(entityPOCO.StandardInterestRateBaseId, false, true);
                if (StandardInterestRateBase != null)
                    entityPM.StandardInterestRateBaseName = StandardInterestRateBase.LocalName;
            }
            if (entityPOCO.CreatedByUserId != null)
            {
                ContactPM createdByContact = contactQuery.GetSinglePMFromCache(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (createdByContact == null)
                    createdByContact = contactQuery.GetSinglePMFromCache(entityPOCO.CreatedByUserId, 0); // user is customer care, get it from tenant 0
                if (createdByContact != null)
                    entityPM.CreatedByUserName = showLocals ? createdByContact.LocalName : createdByContact.EnglishName;
            }

            if (entityPOCO.UpdatedByUserId != null)
            {
                ContactPM updatedByContact = contactQuery.GetSinglePMFromCache(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
                if (updatedByContact == null)
                    updatedByContact = contactQuery.GetSinglePMFromCache(entityPOCO.UpdatedByUserId, 0); // user is customer care, get it from tenant 0
                if (updatedByContact != null)
                    entityPM.UpdatedByUserName = showLocals ? updatedByContact.LocalName : updatedByContact.EnglishName;
            }
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public bool SuppressFetchOpenReconcilation { get; internal set; }

        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
    }


}
   