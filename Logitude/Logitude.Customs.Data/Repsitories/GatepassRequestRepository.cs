 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class GatepassRequestRepository:IRepository<GatepassRequest>
   {
        
		public List<GatepassRequest> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public GatepassRequest GetGatepassRequestByGatepassNumber(int gatepassNumber, int tenant)
        {
            GatepassRequest gatepassRequest = (from a in context.GatepassRequests
                                               where a.GatepassNumber == gatepassNumber && a.Tenant == tenant
                                               select a).FirstOrDefault();

            return gatepassRequest;
        }

    }

}
   