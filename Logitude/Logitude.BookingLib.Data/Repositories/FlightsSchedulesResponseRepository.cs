 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.BookingLib.Data.Repositories
{
   public partial class FlightsSchedulesResponseRepository:IRepository<FlightsSchedulesResponse>
   {        
		public List<FlightsSchedulesResponse> GetMulti(EntityKeyFields entityKeys)
        {
            FlightsSchedulesRequestKeys myEntityKeys = entityKeys as FlightsSchedulesRequestKeys;
            return (from a in context.FlightsSchedulesResponses where a.RequestId == myEntityKeys.Id select a).ToList();
        }

        public List<FlightsSchedulesResponse> GetResponsesByIds(List<string> ids, int tenant)
        {
            List<FlightsSchedulesResponse> myResult = (from d in context.FlightsSchedulesResponses
                                                       where ids.Contains(d.Id) && d.Tenant == tenant
                                                       select d).ToList();

            return myResult;
        }
   }

}
   