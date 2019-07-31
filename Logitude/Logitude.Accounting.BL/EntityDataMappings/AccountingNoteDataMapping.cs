
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
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AccountingNoteDataMapping: IMapping<AccountingNotePM, AccountingNote>
   {

        public void CustomPMToPOCO(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);


            // GET logged contact, RTL
            ContactPM loggedContact = GetLoggedContact(entityPOCO.Tenant);
            bool showLocals = !loggedContact.DontShowLocal;


            // Get user
            ContactQuery query = new ContactQuery(entityPOCO.Tenant);

            if (entityPOCO.CreatedByUserId != null)
            {
                ContactPM createdByContact = query.GetSinglePM(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (createdByContact != null)
                {
                    entityPM.CreatedByUserName = showLocals ? (createdByContact.LocalName ?? createdByContact.EnglishName) : createdByContact.EnglishName;
                }
            }
            if (entityPOCO.UpdatedByUserId != null)
            {
                ContactPM updatedByContact = query.GetSinglePM(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (updatedByContact != null)
                {
                    entityPM.UpdatedByUserName = showLocals ? (updatedByContact.LocalName ?? updatedByContact.EnglishName) : updatedByContact.EnglishName;
                }
            }
        }


        private ContactPM GetLoggedContact(int tenant)
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }


}
   