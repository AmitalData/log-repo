 
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
using Simplog.Data.Helpers;
using System.Data.Entity;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffVersionRepository:IRepository<TariffVersion>
   {

        public List<TariffVersion> GetMulti(EntityKeyFields entityKeys)
        {
            TariffKeys myEntityKeys = entityKeys as TariffKeys;
            return (from a in context.TariffVersions where a.TariffId == myEntityKeys.Id select a).ToList();
        }

        public List<TariffVersion> GetActiveVersions(string tariffId, int tenant, string typeCode)
        {
            if (typeCode == "ASC")
            {
                List<TariffVersion> items = new List<TariffVersion>();

                TariffVersion item=(from a in context.TariffVersions
                        where a.TariffId == tariffId && a.Tenant == tenant && !a.IsDraft                        
                        select a).OrderByDescending(o=>o.CreateDate).FirstOrDefault();

                if (item != null)
                {
                    items.Add(item);
                }

                return items;
            }

            else
            {
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                IQueryable<TariffVersion> TariffVersions = (from a in context.TariffVersions
                                                             where a.TariffId == tariffId && a.Tenant == tenant && !a.IsDraft &&
                                                             DbFunctions.TruncateTime(a.ExpirationDate) >= DbFunctions.TruncateTime(todayDate)
                                                             select a).AsQueryable();               
                        TariffVersions = TariffVersions.Concat(from a in context.TariffVersions
                                         where a.TariffId == tariffId && a.Tenant == tenant && !a.IsDraft && a.ExpirationDate == null select a);

                return TariffVersions.ToList();
            }
        }

        public List<TariffVersion> GetAllVersions(string tariffId, int tenant)
        {
            return (from a in context.TariffVersions
                    where a.TariffId == tariffId && a.Tenant == tenant
                    select a).ToList();
        }

        public IQueryable<TariffVersion> GetActiveVersionsByTariffId(string tariffId, int tenant)
        {
            return from a in context.TariffVersions
                   where a.Tenant == tenant && a.TariffId == tariffId && !a.IsDraft
                   select a;
        }
    }
}
   