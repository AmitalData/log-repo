 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class SharedLogisticsSettingRepository:IRepository<SharedLogisticsSetting>
   {
   
        private IInfrastructureContext currentContext;
        public SharedLogisticsSettingRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public SharedLogisticsSettingRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  SharedLogisticsSetting GetSingle(string id, int tenant)
        {
            return (from a in context.SharedLogisticsSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SharedLogisticsSetting> GetAll(int tenant)
        {
            return from a in context.SharedLogisticsSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SharedLogisticsSetting GetSingle(EntityKeyFields entityKeys)
        {
            SharedLogisticsSettingKeys keys = entityKeys as SharedLogisticsSettingKeys;
            return (from a in context.SharedLogisticsSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SharedLogisticsSetting entity)
        {
            onAdd();
            context.SharedLogisticsSettings.Add(entity);
        }

        public void Remove(SharedLogisticsSetting entity)
        {
            context.SharedLogisticsSettings.Attach(entity);
            context.SharedLogisticsSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SharedLogisticsSetting entity)
        {
            onUpdate();
            context.SharedLogisticsSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedLogisticsSetting> All()
        {
            return context.SharedLogisticsSettings.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 