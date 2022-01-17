 
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
   public partial class ExportStorageRepository:IRepository<ExportStorage>
   {
        
		public List<ExportStorage> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetIDByStorageNo(string storageNo, int tenant)
        {
            var q= from a in context.ExportStorages
                   where 
                   a.Tenant == tenant &&
                   a.StorageNo == storageNo
                   select a.Id;
            return q.FirstOrDefault();

        }
    }

}
   