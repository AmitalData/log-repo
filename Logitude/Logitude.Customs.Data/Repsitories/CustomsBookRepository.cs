 
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
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsBookRepository:IRepository<CustomsBook>
   {
        
		public List<CustomsBook> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public DateTime? GetLastUpdateDate(int tenant)
        {
            DateTime? lastUpdateDate = null;
            var q = (from a in context.CustomsBooks
                     where a.Tenant == tenant
                     select a.LastUpdateDate);
            lastUpdateDate = q.FirstOrDefault();
            return lastUpdateDate;

        }
        public List<CustomsBook> GetCustomsBookByTenant(int tenant)
        {
            if (tenant == null) return null;

            return (from a in context.CustomsBooks
                    where a.Tenant == tenant
                    select a).ToList();
        }

        public List<CustomsBook> GetCustomsBookData()
        {
            return (from a in context.CustomsBooks
                    select a).ToList();
        }
   }
}
   