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
    public partial class ConsignmentPackageQueryService : EntityQueryService<ConsignmentPackage, ConsignmentPackageKeys, ConsignmentPackagePM, ConsignmentPM, ConsignmentKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, ConsignmentPackagePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ConsignmentPackageKeys consignmentPackageKeys = entityKeys as ConsignmentPackageKeys;
            ConsignmentPackDangerQueryService consignmentPackDangerQueryService = new ConsignmentPackDangerQueryService(context);

            entityPM.ConsignmentPackDangers = consignmentPackDangerQueryService.GetMulti(consignmentPackageKeys, false);

        }
    } }
