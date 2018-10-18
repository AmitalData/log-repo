 
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
   public partial class FlightsSchedulesRequestRepository:IRepository<FlightsSchedulesRequest>
   {        
		public List<FlightsSchedulesRequest> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public FlightsSchedulesRequest GetFlightsSchedulesRequestByDetails(string myRequestDetails, int myTenant)
        {
            FlightsSchedulesRequest myRequest = (from a in context.FlightsSchedulesRequests
                                                 where a.ResponseDate == null
                                                 && a.Tenant == myTenant
                                                 && a.RequestDetails == myRequestDetails
                                                 orderby a.CreateDate descending
                                                 select a).FirstOrDefault();

            return myRequest;
        }
   }

}
   