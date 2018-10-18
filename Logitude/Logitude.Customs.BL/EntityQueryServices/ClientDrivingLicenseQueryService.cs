using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ClientDrivingLicenseQueryService : EntityQueryService<ClientDrivingLicense, ClientDrivingLicenseKeys, ClientDrivingLicensePM, ClientPM, ClientKeys>
    {

        public override void GetComposition(EntityKeyFields entityKeys, ClientDrivingLicensePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ClientDrivingLicenseKeys ClientDrivingLicenseKeys = entityKeys as ClientDrivingLicenseKeys;
            ClientDrivingLicenseTypeQueryService clientDrivingLicenseTypeQueryService = new ClientDrivingLicenseTypeQueryService(context);
            entityPM.ClientDrivingLicenseTypes = clientDrivingLicenseTypeQueryService.GetMulti(ClientDrivingLicenseKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}

