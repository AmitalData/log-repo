 
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
   public partial class CustomsAirlineRepository:IRepository<CustomsAirline>
   {
        
		public List<CustomsAirline> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CustomsAirline GetByAirlineAndPrefix(string code, string prefix, string entityId)
        {

            var Airline = (from a in context.CustomsAirlines
                       where a.AirlineCode == code && a.AirlinePrefix == prefix
                          && a.Id != entityId
                       select a).FirstOrDefault();

            return Airline;
        }

        public CustomsAirline GetByPrefix(string prefix, string entityId)
        {

            var Airline = (from a in context.CustomsAirlines
                           where a.AirlinePrefix == prefix
                              && a.Id != entityId
                           select a).FirstOrDefault();

            return Airline;
        }

    }

}
   