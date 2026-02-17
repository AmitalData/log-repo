using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffVersionAllInChargeUpdateService
    {
        protected override void OnCreating(TariffVersionAllInChargePM entityPM, TariffVersionPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                string myLoggedUserId = null;

                string email = "";
                if (AuthenticationUtil.IsAuthenticatedUserExists())
                {
                    email = AuthenticationUtil.GetAuthenticatedUser();
                }

                else
                {
                    email = "system@tenant" + entityPM.Tenant + ".com";
                }

                ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
                ContactRepository contactRep = new ContactRepository(commonContext);
                Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
                if (contact != null)
                {
                    myLoggedUserId = contact.Id;
                }

                entityPM.TariffId = entityParentPM.TariffId;
                entityPM.Id = IdCounter.GetNumber("TariffVersionAllInCharge", entityPM.Tenant);
                entityPM.AddDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.AddedByUserId = myLoggedUserId;
            }
        }
    }
}
