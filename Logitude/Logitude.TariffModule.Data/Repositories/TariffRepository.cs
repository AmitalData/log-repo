 
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
   public partial class TariffRepository:IRepository<Tariff>
   {
        
		public List<Tariff> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public IQueryable<TariffLine> GetAllTariffLines(int tenant)
        {
            return from a in context.TariffLines
                   where a.Tenant == tenant
                   select a;
        }

        public IQueryable<Tariff> GetAllTariff(string[] Ids,int tenant)
        {
            return from a in context.Tariffs
                   where a.Tenant == tenant && Ids.Contains(a.Id)
                   select a;
        }

        public IQueryable<TariffVersion> GetAllTariffVersionsByTariffIds(string[] Ids, int tenant)
        {
            return from a in context.TariffVersions
                   where a.Tenant == tenant && Ids.Contains(a.TariffId) && a.IsDraft==false 
                   select a;
        }

    }

}
   