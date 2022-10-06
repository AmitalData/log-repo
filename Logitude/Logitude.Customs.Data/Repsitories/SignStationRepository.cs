 
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
   public partial class SignStationRepository:IRepository<SignStation>
   {
        
		public List<SignStation> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public SignStation GetAvailableSignServerByCustomsAgentId(string customsAgentId,int LastAccessedInMin)
        {
            DateTime LastAccessedAt = DateTime.Now.AddMinutes(-1 * LastAccessedInMin);
            return (from a in context.SignStations
                    where a.CustomsAgentId == customsAgentId && a.IsCompanySignOn && a.LastAccessedAt > LastAccessedAt
                    select a).FirstOrDefault();
        }
        public List<SignStation> GetAllAvailable(string customsAgentId, int LastAccessedInMin)
        {
            DateTime LastAccessedAt = DateTime.Now.AddMinutes(-1 * LastAccessedInMin);
            return (from a in context.SignStations
                    where a.CustomsAgentId == customsAgentId && a.LastAccessedAt > LastAccessedAt
                    select a).ToList();
        }

    }

}
   