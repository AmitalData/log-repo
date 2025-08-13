using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class GatepassRequestQueryService : EntityQueryService<GatepassRequest, GatepassRequestKeys, GatepassRequestPM, object, GatepassRequestKeys>
    {
        public GatepassRequestPM GetGatepassRequestByGatepassNumber(int gatepassNumber, int tenant)
        {
            GatepassRequestRepository gatepassRequestRepository = new GatepassRequestRepository(context);
            GatepassRequestPM gatepassRequestPM = null;

            var poco = gatepassRequestRepository.GetGatepassRequestByGatepassNumber(gatepassNumber, tenant);
            if (poco != null)
            {
                gatepassRequestPM = this.GetEntityPM(poco, false, null);
            }
            return gatepassRequestPM;
        }
        
        public GatepassRequestPM GetGatepassRequestByMasterCourierId(string masterCourierId, int tenant)
        {
            GatepassRequestRepository gatepassRequestRepository = new GatepassRequestRepository(context);
            GatepassRequestPM gatepassRequestPM = null;

            var poco = gatepassRequestRepository.GetGatepassRequestByMasterCourierId(masterCourierId, tenant);
            if (poco != null)
            {
                gatepassRequestPM = this.GetEntityPM(poco, false, null);
            }
            return gatepassRequestPM;
        }
    }
}
