 
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
   public partial class TariffVersionRepository:IRepository<TariffVersion>
   {
   
        private ITariffModuleContext currentContext;
        public TariffVersionRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffVersionRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffVersion GetSingle(string tariffid, int version, int tenant)
        {
            return (from a in context.TariffVersions
                    where a.TariffId == tariffid && a.Version == version && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffVersion> GetAll(int tenant)
        {
            return from a in context.TariffVersions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffVersion GetSingle(EntityKeyFields entityKeys)
        {
            TariffVersionKeys keys = entityKeys as TariffVersionKeys;
            return (from a in context.TariffVersions
                    where a.TariffId == keys.TariffId && a.Version == keys.Version
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffVersion entity)
        {
            onAdd();
            context.TariffVersions.Add(entity);
        }

        public void Remove(TariffVersion entity)
        {
            context.TariffVersions.Attach(entity);
            context.TariffVersions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffVersion entity)
        {
            onUpdate();
            context.TariffVersions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffVersion> All()
        {
            return context.TariffVersions.ToList();
        }

        private ITariffModuleContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 