 
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
   public partial class BIFoldersPermissionRepository:IRepository<BIFoldersPermission>
   {
   
        private IInfrastructureContext currentContext;
        public BIFoldersPermissionRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BIFoldersPermissionRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BIFoldersPermission GetSingle(string id, int tenant)
        {
            return (from a in context.BIFoldersPermissions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BIFoldersPermission> GetAll(int tenant)
        {
            return from a in context.BIFoldersPermissions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BIFoldersPermission GetSingle(EntityKeyFields entityKeys)
        {
            BIFoldersPermissionKeys keys = entityKeys as BIFoldersPermissionKeys;
            return (from a in context.BIFoldersPermissions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BIFoldersPermission entity)
        {
            onAdd();
            context.BIFoldersPermissions.Add(entity);
        }

        public void Remove(BIFoldersPermission entity)
        {
            context.BIFoldersPermissions.Attach(entity);
            context.BIFoldersPermissions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BIFoldersPermission entity)
        {
            onUpdate();
            context.BIFoldersPermissions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BIFoldersPermission> All()
        {
            return context.BIFoldersPermissions.ToList();
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
	 