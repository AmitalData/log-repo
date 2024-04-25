 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class RemarksClassificationRepository:IRepository<RemarksClassification>
   {
   
        private ICustomContext currentContext;
        public RemarksClassificationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RemarksClassificationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RemarksClassification GetSingle(string id, int tenant)
        {
            return (from a in context.RemarksClassifications
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<RemarksClassification> GetAll(int tenant)
        {
            return from a in context.RemarksClassifications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public RemarksClassification GetSingle(EntityKeyFields entityKeys)
        {
            RemarksClassificationKeys keys = entityKeys as RemarksClassificationKeys;
            return (from a in context.RemarksClassifications
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RemarksClassification entity)
        {
            onAdd();
            context.RemarksClassifications.Add(entity);
        }

        public void Remove(RemarksClassification entity)
        {
            context.RemarksClassifications.Attach(entity);
            context.RemarksClassifications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RemarksClassification entity)
        {
            onUpdate();
            context.RemarksClassifications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RemarksClassification> All()
        {
            return context.RemarksClassifications.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 