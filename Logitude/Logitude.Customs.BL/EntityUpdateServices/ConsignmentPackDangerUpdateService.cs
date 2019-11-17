using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.ComponentModel;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ConsignmentPackDangerUpdateService : EntityUpdateService<ConsignmentPackDanger, ConsignmentPackDangerPM, ConsignmentPackagePM >
    {
        protected override void OnCreating(ConsignmentPackDangerPM entityPM, ConsignmentPackagePM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.ConsignmentNumber = entityParentPM.ConsignmentNumber;
            entityPM.LineNumber = entityParentPM.LineNumber;
            entityPM.DangerousLineNo = 1;
         }
 

    }
}
