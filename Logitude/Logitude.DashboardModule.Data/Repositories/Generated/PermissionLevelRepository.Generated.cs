 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.DashboardModule.Data.Repositories
{
   public partial class PermissionLevelRepository:IRepository<PermissionLevel>
   {
   
        private IDashboardContext currentContext;
        public PermissionLevelRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public PermissionLevelRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  PermissionLevel GetSingle(string code)
        {
            return (from a in context.PermissionLevels
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PermissionLevel> GetAll()
        {
            return from a in context.PermissionLevels  
                   select a;
        }
				 
        public PermissionLevel GetSingle(EntityKeyFields entityKeys)
        {
            PermissionLevelKeys keys = entityKeys as PermissionLevelKeys;
            return (from a in context.PermissionLevels
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PermissionLevel entity)
        {
            onAdd();
            context.PermissionLevels.Add(entity);
        }

        public void Remove(PermissionLevel entity)
        {
            context.PermissionLevels.Attach(entity);
            context.PermissionLevels.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PermissionLevel entity)
        {
            onUpdate();
            context.PermissionLevels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PermissionLevel> All()
        {
            return context.PermissionLevels.ToList();
        }

        private IDashboardContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 