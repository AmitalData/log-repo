 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_TariffRepository:IRepository<CB_Tariff>
   {
   
        private ICustomContext currentContext;
        public CB_TariffRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_TariffRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_Tariff GetSingle(string id)
        {
            return (from a in context.CB_Tariffs
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_Tariff> GetAll()
        {
            return from a in context.CB_Tariffs  
                   select a;
        }
				 
        public CB_Tariff GetSingle(EntityKeyFields entityKeys)
        {
            CB_TariffKeys keys = entityKeys as CB_TariffKeys;
            return (from a in context.CB_Tariffs
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_Tariff entity)
        {
            onAdd();
            context.CB_Tariffs.Add(entity);
        }

        public void Remove(CB_Tariff entity)
        {
            context.CB_Tariffs.Attach(entity);
            context.CB_Tariffs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_Tariff entity)
        {
            onUpdate();
            context.CB_Tariffs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_Tariff> All()
        {
            return context.CB_Tariffs.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 