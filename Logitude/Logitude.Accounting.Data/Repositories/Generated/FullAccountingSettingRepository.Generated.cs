 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class FullAccountingSettingRepository:IRepository<FullAccountingSetting>
   {
   
        private IAccountingContext currentContext;
        public FullAccountingSettingRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public FullAccountingSettingRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  FullAccountingSetting GetSingle(string id, int tenant)
        {
            return (from a in context.FullAccountingSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FullAccountingSetting> GetAll(int tenant)
        {
            return from a in context.FullAccountingSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public FullAccountingSetting GetSingle(EntityKeyFields entityKeys)
        {
            FullAccountingSettingKeys keys = entityKeys as FullAccountingSettingKeys;
            return (from a in context.FullAccountingSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FullAccountingSetting entity)
        {
            onAdd();
            context.FullAccountingSettings.Add(entity);
        }

        public void Remove(FullAccountingSetting entity)
        {
            context.FullAccountingSettings.Attach(entity);
            context.FullAccountingSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FullAccountingSetting entity)
        {
            onUpdate();
            context.FullAccountingSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FullAccountingSetting> All()
        {
            return context.FullAccountingSettings.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 