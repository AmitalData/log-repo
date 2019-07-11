 
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
   public partial class TariffVersionAllInChargeRepository:IRepository<TariffVersionAllInCharge>
   {
   
        private ITariffModuleContext currentContext;
        public TariffVersionAllInChargeRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffVersionAllInChargeRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffVersionAllInCharge GetSingle(string id, int tenant)
        {
            return (from a in context.TariffVersionAllInCharges
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffVersionAllInCharge> GetAll(int tenant)
        {
            return from a in context.TariffVersionAllInCharges  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffVersionAllInCharge GetSingle(EntityKeyFields entityKeys)
        {
            TariffVersionAllInChargeKeys keys = entityKeys as TariffVersionAllInChargeKeys;
            return (from a in context.TariffVersionAllInCharges
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffVersionAllInCharge entity)
        {
            onAdd();
            context.TariffVersionAllInCharges.Add(entity);
        }

        public void Remove(TariffVersionAllInCharge entity)
        {
            context.TariffVersionAllInCharges.Attach(entity);
            context.TariffVersionAllInCharges.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffVersionAllInCharge entity)
        {
            onUpdate();
            context.TariffVersionAllInCharges.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffVersionAllInCharge> All()
        {
            return context.TariffVersionAllInCharges.ToList();
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
	 