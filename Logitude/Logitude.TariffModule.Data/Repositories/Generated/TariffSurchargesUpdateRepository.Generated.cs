 
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
   public partial class TariffSurchargesUpdateRepository:IRepository<TariffSurchargesUpdate>
   {
   
        private ITariffModuleContext currentContext;
        public TariffSurchargesUpdateRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffSurchargesUpdateRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffSurchargesUpdate GetSingle(string id, int tenant)
        {
            return (from a in context.TariffSurchargesUpdates
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffSurchargesUpdate> GetAll(int tenant)
        {
            return from a in context.TariffSurchargesUpdates  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffSurchargesUpdate GetSingle(EntityKeyFields entityKeys)
        {
            TariffSurchargesUpdateKeys keys = entityKeys as TariffSurchargesUpdateKeys;
            return (from a in context.TariffSurchargesUpdates
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffSurchargesUpdate entity)
        {
            onAdd();
            context.TariffSurchargesUpdates.Add(entity);
        }

        public void Remove(TariffSurchargesUpdate entity)
        {
            context.TariffSurchargesUpdates.Attach(entity);
            context.TariffSurchargesUpdates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffSurchargesUpdate entity)
        {
            onUpdate();
            context.TariffSurchargesUpdates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffSurchargesUpdate> All()
        {
            return context.TariffSurchargesUpdates.ToList();
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
	 