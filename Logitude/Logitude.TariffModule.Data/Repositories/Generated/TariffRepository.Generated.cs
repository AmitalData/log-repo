 
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
   
        private ITariffModuleContext currentContext;
        public TariffRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  Tariff GetSingle(string id, int tenant)
        {
            return (from a in context.Tariffs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Tariff> GetAll(int tenant)
        {
            return from a in context.Tariffs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Tariff GetSingle(EntityKeyFields entityKeys)
        {
            TariffKeys keys = entityKeys as TariffKeys;
            return (from a in context.Tariffs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Tariff entity)
        {
            onAdd();
            context.Tariffs.Add(entity);
        }

        public void Remove(Tariff entity)
        {
            context.Tariffs.Attach(entity);
            context.Tariffs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Tariff entity)
        {
            onUpdate();
            context.Tariffs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Tariff> All()
        {
            return context.Tariffs.ToList();
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
	 