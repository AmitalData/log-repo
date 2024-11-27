 
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
   public partial class CB_RuleClassificationRepository:IRepository<CB_RuleClassification>
   {
   
        private ICustomContext currentContext;
        public CB_RuleClassificationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RuleClassificationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_RuleClassification GetSingle(string cb_id)
        {
            return (from a in context.CB_RuleClassifications
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_RuleClassification> GetAll()
        {
            return from a in context.CB_RuleClassifications  
                   select a;
        }
				 
        public CB_RuleClassification GetSingle(EntityKeyFields entityKeys)
        {
            CB_RuleClassificationKeys keys = entityKeys as CB_RuleClassificationKeys;
            return (from a in context.CB_RuleClassifications
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_RuleClassification entity)
        {
            onAdd();
            context.CB_RuleClassifications.Add(entity);
        }

        public void Remove(CB_RuleClassification entity)
        {
            context.CB_RuleClassifications.Attach(entity);
            context.CB_RuleClassifications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_RuleClassification entity)
        {
            onUpdate();
            context.CB_RuleClassifications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_RuleClassification> All()
        {
            return context.CB_RuleClassifications.ToList();
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
	 