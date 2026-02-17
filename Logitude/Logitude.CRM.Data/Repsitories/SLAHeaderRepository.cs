
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Linq;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class SLAHeaderRepository:IRepository<SLAHeader>
   {
        
		public List<SLAHeader> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public SLAHeader GetSingleByTenant(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPoco = tenantRepository.GetSingleByTenant(tenant);
            string id = tenantPoco.DefaultSLAId;

            if (!string.IsNullOrEmpty(id))
            {
                return (from a in context.SLAHeaders
                        where a.Tenant == tenant && a.Id == id
                        select a).FirstOrDefault();
            }
            else
            {
                return (from a in context.SLAHeaders
                        where a.Tenant == tenant
                        select a).FirstOrDefault();
            }
        }

        public SLAHeader GetSingleHeaderById(int tenant, string id)
        {
            return (from a in context.SLAHeaders
                    where a.Tenant == tenant && a.Id == id
                    select a).FirstOrDefault();
        }

        public SLAHeader GetSingleHeaderByIdAndName(int tenant, string name)
        {
            return (from a in context.SLAHeaders
                    where a.Tenant == tenant && a.Name == name
                    select a).FirstOrDefault();
        }


        public string GetSLAHeaderNameById(int tenant, string id)
        {
            return (from a in context.SLAHeaders
                    where a.Tenant == tenant && a.Id == id
                    select a.Name).FirstOrDefault();
        }
    }

}
   