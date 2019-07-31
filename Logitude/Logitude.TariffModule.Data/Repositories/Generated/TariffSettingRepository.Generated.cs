 
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
   public partial class TariffSettingRepository:IRepository<TariffSetting>
   {
   
        private ITariffModuleContext currentContext;
        public TariffSettingRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffSettingRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffSetting GetSingle(string id, int tenant)
        {
            return (from a in context.TariffSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffSetting> GetAll(int tenant)
        {
            return from a in context.TariffSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffSetting GetSingle(EntityKeyFields entityKeys)
        {
            TariffSettingKeys keys = entityKeys as TariffSettingKeys;
            return (from a in context.TariffSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffSetting entity)
        {
            onAdd();
            context.TariffSettings.Add(entity);
        }

        public void Remove(TariffSetting entity)
        {
            context.TariffSettings.Attach(entity);
            context.TariffSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffSetting entity)
        {
            onUpdate();
            context.TariffSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffSetting> All()
        {
            return context.TariffSettings.ToList();
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
	 