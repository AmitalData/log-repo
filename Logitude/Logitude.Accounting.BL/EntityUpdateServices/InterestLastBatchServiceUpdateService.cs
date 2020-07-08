using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class InterestLastBatchServiceUpdateService
    {

        protected override void OnCreating(InterestLastBatchServicePM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("InterestLastBatchService", entityPM.Tenant);

        }



        protected override void Trace(InterestLastBatchServicePM entityPM, InterestLastBatchService entityPOCO, string changesXml)
        {
            
 
        }

    }
}
