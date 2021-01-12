 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffVersionUploadedExcelRepository:IRepository<TariffVersionUploadedExcel>
   {
        
		public List<TariffVersionUploadedExcel> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<TariffVersionUploadedExcel> GetAllVersionUploadedExcels(string tariffId, int version, int tenant)
        {
            return (from a in context.TariffVersionUploadedExcels
                    where a.TariffId == tariffId && a.Tenant == tenant && a.Version == version
                    select a);
        }
    }

}
   