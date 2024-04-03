 
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
   public partial class CustomsEnvironmentSettingRepository:IRepository<CustomsEnvironmentSetting>
   {
   
        private ICustomContext currentContext;
        public CustomsEnvironmentSettingRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsEnvironmentSettingRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsEnvironmentSetting GetSingle(string id, string environmentcode)
        {
            return (from a in context.CustomsEnvironmentSettings
                    where a.Id == id && a.EnvironmentCode == environmentcode 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsEnvironmentSetting> GetAll()
        {
            return from a in context.CustomsEnvironmentSettings  
                   select a;
        }
				 
        public CustomsEnvironmentSetting GetSingle(EntityKeyFields entityKeys)
        {
            CustomsEnvironmentSettingKeys keys = entityKeys as CustomsEnvironmentSettingKeys;
            return (from a in context.CustomsEnvironmentSettings
                    where a.Id == keys.Id && a.EnvironmentCode == keys.EnvironmentCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsEnvironmentSetting entity)
        {
            onAdd();
            context.CustomsEnvironmentSettings.Add(entity);
        }

        public void Remove(CustomsEnvironmentSetting entity)
        {
            context.CustomsEnvironmentSettings.Attach(entity);
            context.CustomsEnvironmentSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsEnvironmentSetting entity)
        {
            onUpdate();
            context.CustomsEnvironmentSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsEnvironmentSetting> All()
        {
            return context.CustomsEnvironmentSettings.ToList();
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
	 