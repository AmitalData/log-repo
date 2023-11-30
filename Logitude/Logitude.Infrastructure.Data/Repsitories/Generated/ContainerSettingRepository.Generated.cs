 
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
   public partial class ContainerSettingRepository:IRepository<ContainerSetting>
   {
   
        private IInfrastructureContext currentContext;
        public ContainerSettingRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public ContainerSettingRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContainerSetting GetSingle(string id, int tenant)
        {
            return (from a in context.ContainerSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ContainerSetting> GetAll(int tenant)
        {
            return from a in context.ContainerSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ContainerSetting GetSingle(EntityKeyFields entityKeys)
        {
            ContainerSettingKeys keys = entityKeys as ContainerSettingKeys;
            return (from a in context.ContainerSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContainerSetting entity)
        {
            onAdd();
            context.ContainerSettings.Add(entity);
        }

        public void Remove(ContainerSetting entity)
        {
            context.ContainerSettings.Attach(entity);
            context.ContainerSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContainerSetting entity)
        {
            onUpdate();
            context.ContainerSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainerSetting> All()
        {
            return context.ContainerSettings.ToList();
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
	 