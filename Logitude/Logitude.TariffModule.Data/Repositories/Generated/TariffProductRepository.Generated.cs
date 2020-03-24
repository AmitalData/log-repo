 
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
   public partial class TariffProductRepository:IRepository<TariffProduct>
   {
   
        private ITariffModuleContext currentContext;
        public TariffProductRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffProductRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffProduct GetSingle(string id, int tenant)
        {
            return (from a in context.TariffProducts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffProduct> GetAll(int tenant)
        {
            return from a in context.TariffProducts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffProduct GetSingle(EntityKeyFields entityKeys)
        {
            TariffProductKeys keys = entityKeys as TariffProductKeys;
            return (from a in context.TariffProducts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffProduct entity)
        {
            onAdd();
            context.TariffProducts.Add(entity);
        }

        public void Remove(TariffProduct entity)
        {
            context.TariffProducts.Attach(entity);
            context.TariffProducts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffProduct entity)
        {
            onUpdate();
            context.TariffProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffProduct> All()
        {
            return context.TariffProducts.ToList();
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
	 