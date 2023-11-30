
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
using Logitude.Server.Tools.Counters;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestBasesPeriodDataMapping: IMapping<InterestBasesPeriodPM, InterestBasesPeriod>
   {

        public void CustomPMToPOCO(InterestBasesPeriodPM entityPM, InterestBasesPeriod entityPOCO)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;

            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            entityPOCO.LineNumber = entityPM.LineNumber;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InterestBaseTypeId);
            entityPOCO.InterestBaseTypeId = entityPM.InterestBaseTypeId;
            if (entityPM.InterestBaseStartDate != entityPOCO.InterestBaseStartDate)
            {
                InterestBasesPeriodRepository PeriodRepository = new InterestBasesPeriodRepository(entityPM.Tenant);
                InterestBasesPeriod Period = PeriodRepository.GetSingleByInterestBaseStartDateAndnterestBaseTypeId(entityPM.InterestBaseStartDate, entityPM.InterestBaseTypeId, entityPM.Tenant);
                if (Period != null)
                {
                    throw new ApplicationException(TextCodesTranslator.TranslateText("Accounting.General.O.Abaseperiodwiththesamestartdateexists", entityPM.Tenant, showLocals));
                }
            }
        }

        public void CustomPOCOToPM(InterestBasesPeriodPM entityPM, InterestBasesPeriod entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);

            // GET logged contact, RTL
            ContactQuery contactQuery = new ContactQuery(entityPOCO.Tenant);
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;

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
   