 
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
   public partial class CustomsSettingRepository:IRepository<CustomsSetting>
   {
   
        private ICustomContext currentContext;
        public CustomsSettingRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsSettingRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsSetting GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsSetting> GetAll(int tenant)
        {
            return from a in context.CustomsSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsSetting GetSingle(EntityKeyFields entityKeys)
        {
            CustomsSettingKeys keys = entityKeys as CustomsSettingKeys;
            return (from a in context.CustomsSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsSetting entity)
        {
            onAdd();
            context.CustomsSettings.Add(entity);
        }

        public void Remove(CustomsSetting entity)
        {
            context.CustomsSettings.Attach(entity);
            context.CustomsSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsSetting entity)
        {
            onUpdate();
            context.CustomsSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsSetting> All()
        {
            return context.CustomsSettings.ToList();
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
	 