using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;


namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffSurchargesUpdateUpdateService
    {
        protected override void OnCreating(TariffSurchargesUpdatePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TariffSurchargesUpdate", entityPM.Tenant);
            }
        }

        protected override void OnUpdating(TariffSurchargesUpdatePM entityPM, TariffSurchargesUpdate entityPOCO)
        {
            var myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.CreateDate = myDate;
                var myLoggedUser = GentUserInfo(entityPM);
                if(myLoggedUser != null)
                {
                    if (entityPM.CreatedByUserId == null)
                    {
                        entityPM.CreatedByUserId = myLoggedUser.Id;
                    }
                }
            }
        }

        private Contact GentUserInfo(TariffSurchargesUpdatePM entityPM)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }
            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            return contact;
        }
    }
}
