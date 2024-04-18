 
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
   public partial class OriginCriterionRepository:IRepository<OriginCriterion>
   {
   
        private ICustomContext currentContext;
        public OriginCriterionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public OriginCriterionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  OriginCriterion GetSingle(string code)
        {
            return (from a in context.OriginCriterions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OriginCriterion> GetAll()
        {
            return from a in context.OriginCriterions  
                   select a;
        }
				 
        public OriginCriterion GetSingle(EntityKeyFields entityKeys)
        {
            OriginCriterionKeys keys = entityKeys as OriginCriterionKeys;
            return (from a in context.OriginCriterions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OriginCriterion entity)
        {
            onAdd();
            context.OriginCriterions.Add(entity);
        }

        public void Remove(OriginCriterion entity)
        {
            context.OriginCriterions.Attach(entity);
            context.OriginCriterions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OriginCriterion entity)
        {
            onUpdate();
            context.OriginCriterions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OriginCriterion> All()
        {
            return context.OriginCriterions.ToList();
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
	 