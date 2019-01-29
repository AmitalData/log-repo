 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class AccountingNoteUpdateService
   {
        protected override void OnCreating(AccountingNotePM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant);


            // users:
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {
                entityPM.CreatedByUserId = user.Id;
                entityPM.CreateDate = GetCurrentDateTime(entityPM.Tenant);
            }

        }

        protected override void OnUpdating(AccountingNotePM entityPM, AccountingNote entityPOCO)
        {
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {
                entityPM.UpdatedByUserId = user.Id;
                entityPM.UpdateDate = GetCurrentDateTime(entityPM.Tenant);
            }


        }


        // methods
        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }


        public virtual Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        public virtual ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "AccountingNote", Tenant);
        }

    }
}
	 