 
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
   public partial class SLALineRepository:IRepository<SLALine>
   {
   
        private ICRMContext currentContext;
        public SLALineRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public SLALineRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  SLALine GetSingle(string id, int tenant)
        {
            return (from a in context.SLALines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SLALine> GetAll(int tenant)
        {
            return from a in context.SLALines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SLALine GetSingle(EntityKeyFields entityKeys)
        {
            SLALineKeys keys = entityKeys as SLALineKeys;
            return (from a in context.SLALines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SLALine entity)
        {
            onAdd();
            context.SLALines.Add(entity);
        }

        public void Remove(SLALine entity)
        {
            context.SLALines.Attach(entity);
            context.SLALines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SLALine entity)
        {
            onUpdate();
            context.SLALines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SLALine> All()
        {
            return context.SLALines.ToList();
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
	 