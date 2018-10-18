using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientDrivingLicenseUpdateService : EntityUpdateService<ClientDrivingLicense, ClientDrivingLicensePM, ClientPM>
    {
        protected override void OnCreating(ClientDrivingLicensePM entityPM, ClientPM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.ClientId = entityParentPM.Id;
                entityPM.Tenant = entityParentPM.Tenant;
            }
            int line = 0;
            if (entityParentPM.ClientDrivingLicenses.Count > 0)
            {
                line = entityParentPM.ClientDrivingLicenses.Max(d => d.Line);
            }
            entityPM.Line = line + 1;
        }

        protected override void UpdateComposition(ClientDrivingLicensePM entityPM)
        {
            ClientDrivingLicenseTypeUpdateService clientDrivingLicenseTypeUpdateService = new ClientDrivingLicenseTypeUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            clientDrivingLicenseTypeUpdateService.UpdateMulti(entityPM.ClientDrivingLicenseTypes, entityPM.DeletedClientDrivingLicenseTypes, entityPM, false);
            base.UpdateComposition(entityPM);
        }

    }
}
