 
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
   public partial class LogisticActionRequestRepository:IRepository<LogisticActionRequest>
   {
        
		public List<LogisticActionRequest> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public LogisticActionRequest GetByCargoKey(string key1, string key2, string key3, int type)
        {
            string sType = type.ToString();

            var res = (from x in context.LogisticActionRequests
                     where x.CargoIdentifierKey1 == key1 && x.CargoIdentifierKey2 == key2 && x.CargoIdentifierKey3 == key3 && x.CargoIdentifierType == sType
                     select x
                     ).FirstOrDefault();
            
            return res;
        }


        public bool GetExistByCargoKey(string id, string cargoIdentifierKey1, string cargoIdentifierKey2, string cargoIdentifierKey3, string cargoIdentifierType)
        {
            return (from x in context.LogisticActionRequests
                       where x.Id != id && x.CargoIdentifierKey1 == cargoIdentifierKey1 && x.CargoIdentifierKey2 == cargoIdentifierKey2 && x.CargoIdentifierKey3 == cargoIdentifierKey3 && x.CargoIdentifierType == cargoIdentifierType
                       select x
                     ).Any();

        }


        public List<LogisticActionRequest> GetByids(string[] ids)
        {
            var res = context.LogisticActionRequests.Where(x => ids.Contains(x.Id));
            return res.ToList();
        }
   
        
    }

}
   