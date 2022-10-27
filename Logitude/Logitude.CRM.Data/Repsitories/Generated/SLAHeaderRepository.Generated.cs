 
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
   public partial class SLAHeaderRepository:IRepository<SLAHeader>
   {
   
        private ICRMContext currentContext;
        public SLAHeaderRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public SLAHeaderRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  SLAHeader GetSingle(string id, int tenant)
        {
            return (from a in context.SLAHeaders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SLAHeader> GetAll(int tenant)
        {
            return from a in context.SLAHeaders  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SLAHeader GetSingle(EntityKeyFields entityKeys)
        {
            SLAHeaderKeys keys = entityKeys as SLAHeaderKeys;
            return (from a in context.SLAHeaders
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SLAHeader entity)
        {
            onAdd();
            context.SLAHeaders.Add(entity);
        }

        public void Remove(SLAHeader entity)
        {
            context.SLAHeaders.Attach(entity);
            context.SLAHeaders.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SLAHeader entity)
        {
            onUpdate();
            context.SLAHeaders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SLAHeader> All()
        {
            return context.SLAHeaders.ToList();
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
	 