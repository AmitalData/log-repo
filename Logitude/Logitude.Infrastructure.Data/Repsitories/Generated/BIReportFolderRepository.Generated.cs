 
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
   public partial class BIReportFolderRepository:IRepository<BIReportFolder>
   {
   
        private IInfrastructureContext currentContext;
        public BIReportFolderRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BIReportFolderRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BIReportFolder GetSingle(string id, int tenant)
        {
            return (from a in context.BIReportFolders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BIReportFolder> GetAll(int tenant)
        {
            return from a in context.BIReportFolders  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BIReportFolder GetSingle(EntityKeyFields entityKeys)
        {
            BIReportFolderKeys keys = entityKeys as BIReportFolderKeys;
            return (from a in context.BIReportFolders
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BIReportFolder entity)
        {
            onAdd();
            context.BIReportFolders.Add(entity);
        }

        public void Remove(BIReportFolder entity)
        {
            context.BIReportFolders.Attach(entity);
            context.BIReportFolders.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BIReportFolder entity)
        {
            onUpdate();
            context.BIReportFolders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BIReportFolder> All()
        {
            return context.BIReportFolders.ToList();
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
	 