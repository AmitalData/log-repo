 
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
   public partial class TariffLinesContainersPriceRepository:IRepository<TariffLinesContainersPrice>
   {
   
        private ITariffModuleContext currentContext;
        public TariffLinesContainersPriceRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffLinesContainersPriceRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffLinesContainersPrice GetSingle(string id, int tenant)
        {
            return (from a in context.TariffLinesContainersPrices
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffLinesContainersPrice> GetAll(int tenant)
        {
            return from a in context.TariffLinesContainersPrices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffLinesContainersPrice GetSingle(EntityKeyFields entityKeys)
        {
            TariffLinesContainersPriceKeys keys = entityKeys as TariffLinesContainersPriceKeys;
            return (from a in context.TariffLinesContainersPrices
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffLinesContainersPrice entity)
        {
            onAdd();
            context.TariffLinesContainersPrices.Add(entity);
        }

        public void Remove(TariffLinesContainersPrice entity)
        {
            context.TariffLinesContainersPrices.Attach(entity);
            context.TariffLinesContainersPrices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffLinesContainersPrice entity)
        {
            onUpdate();
            context.TariffLinesContainersPrices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffLinesContainersPrice> All()
        {
            return context.TariffLinesContainersPrices.ToList();
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
	 