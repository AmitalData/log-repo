 
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
   public partial class TariffSurchargesUpdateMethodRepository:IRepository<TariffSurchargesUpdateMethod>
   {
   
        private ITariffModuleContext currentContext;
        public TariffSurchargesUpdateMethodRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffSurchargesUpdateMethodRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffSurchargesUpdateMethod GetSingle(string code)
        {
            return (from a in context.TariffSurchargesUpdateMethods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffSurchargesUpdateMethod> GetAll()
        {
            return from a in context.TariffSurchargesUpdateMethods  
                   select a;
        }
				 
        public TariffSurchargesUpdateMethod GetSingle(EntityKeyFields entityKeys)
        {
            TariffSurchargesUpdateMethodKeys keys = entityKeys as TariffSurchargesUpdateMethodKeys;
            return (from a in context.TariffSurchargesUpdateMethods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffSurchargesUpdateMethod entity)
        {
            onAdd();
            context.TariffSurchargesUpdateMethods.Add(entity);
        }

        public void Remove(TariffSurchargesUpdateMethod entity)
        {
            context.TariffSurchargesUpdateMethods.Attach(entity);
            context.TariffSurchargesUpdateMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffSurchargesUpdateMethod entity)
        {
            onUpdate();
            context.TariffSurchargesUpdateMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffSurchargesUpdateMethod> All()
        {
            return context.TariffSurchargesUpdateMethods.ToList();
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
	 