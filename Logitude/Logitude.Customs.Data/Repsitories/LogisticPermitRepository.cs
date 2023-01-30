
 
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
   public partial class LogisticPermitRepository:IRepository<LogisticPermit>
   {
        
		public List<LogisticPermit> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public LogisticPermit GetSingleByIdentifierKeys(int CargoIdentifierType, string CargoIdentifierKey1, string CargoIdentifierKey2, string CargoIdentifierKey3, int tenant)
        {
            return (from a in context.LogisticPermits
             where a.CargoIdentifierType == CargoIdentifierType.ToString() && a.CargoIdentifierKey1 == CargoIdentifierKey1 && a.CargoIdentifierKey2 == CargoIdentifierKey2 && a.CargoIdentifierKey3 == CargoIdentifierKey3 && a.Tenant == tenant
             select a).FirstOrDefault();
        }
        
    }

}
   