 
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
   public partial class CourierHawbFromExcelRepository:IRepository<CourierHawbFromExcel>
   {
        
		public List<CourierHawbFromExcel> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<CourierHawbFromExcel> GetAllByUser(int tenant,string userId)
        {
            return from a in context.CourierHawbFromExcels
                   where a.Tenant == tenant  && a.CreatedByUserId == userId && a.NotFound != true
                   select a;
        }
        public void DeleteByUserAndTenant(int tenant, string userId)
        {
            (context as DbContextBase)
               .DeleteWhere<CourierHawbFromExcel>(rec => rec.Tenant == tenant && rec.CreatedByUserId == userId);

        }
    }

}
   