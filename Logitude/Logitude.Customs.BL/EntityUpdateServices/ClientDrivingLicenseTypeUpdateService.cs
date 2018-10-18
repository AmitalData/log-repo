using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientDrivingLicenseTypeUpdateService : EntityUpdateService<ClientDrivingLicenseType, ClientDrivingLicenseTypePM, ClientDrivingLicensePM>
    {

        protected override void OnCreating(ClientDrivingLicenseTypePM entityPM, ClientDrivingLicensePM entityParentPM)
        {
            entityPM.ClientId = entityParentPM.ClientId;
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.ClientDrivingLicenseLine = entityParentPM.Line;
        }

    }
}
