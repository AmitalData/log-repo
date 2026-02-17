 
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
   public partial class BIReportRepository:IRepository<BIReport>
   {
   
        private IInfrastructureContext currentContext;
        public BIReportRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BIReportRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BIReport GetSingle(string id, int tenant)
        {
            return (from a in context.BIReports
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BIReport> GetAll(int tenant)
        {
            return from a in context.BIReports  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BIReport GetSingle(EntityKeyFields entityKeys)
        {
            BIReportKeys keys = entityKeys as BIReportKeys;
            return (from a in context.BIReports
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BIReport entity)
        {
            onAdd();
            context.BIReports.Add(entity);
        }

        public void Remove(BIReport entity)
        {
            context.BIReports.Attach(entity);
            context.BIReports.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BIReport entity)
        {
            onUpdate();
            context.BIReports.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BIReport> All()
        {
            return context.BIReports.ToList();
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
	 