 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CRMFilterSettingRepository:IRepository<CRMFilterSetting>
   {
   
        private ICRMContext currentContext;
        public CRMFilterSettingRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public CRMFilterSettingRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  CRMFilterSetting GetSingle(string id, int tenant)
        {
            return (from a in context.CRMFilterSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CRMFilterSetting> GetAll(int tenant)
        {
            return from a in context.CRMFilterSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CRMFilterSetting GetSingle(EntityKeyFields entityKeys)
        {
            CRMFilterSettingKeys keys = entityKeys as CRMFilterSettingKeys;
            return (from a in context.CRMFilterSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CRMFilterSetting entity)
        {
            onAdd();
            context.CRMFilterSettings.Add(entity);
        }

        public void Remove(CRMFilterSetting entity)
        {
            context.CRMFilterSettings.Attach(entity);
            context.CRMFilterSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CRMFilterSetting entity)
        {
            onUpdate();
            context.CRMFilterSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CRMFilterSetting> All()
        {
            return context.CRMFilterSettings.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 