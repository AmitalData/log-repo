 
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
   public partial class InterConditionsRelationshipRepository:IRepository<InterConditionsRelationship>
   {
   
        private ICustomContext currentContext;
        public InterConditionsRelationshipRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InterConditionsRelationshipRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterConditionsRelationship GetSingle(string code)
        {
            return (from a in context.InterConditionsRelationships
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InterConditionsRelationship> GetAll()
        {
            return from a in context.InterConditionsRelationships  
                   select a;
        }
				 
        public InterConditionsRelationship GetSingle(EntityKeyFields entityKeys)
        {
            InterConditionsRelationshipKeys keys = entityKeys as InterConditionsRelationshipKeys;
            return (from a in context.InterConditionsRelationships
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterConditionsRelationship entity)
        {
            onAdd();
            context.InterConditionsRelationships.Add(entity);
        }

        public void Remove(InterConditionsRelationship entity)
        {
            context.InterConditionsRelationships.Attach(entity);
            context.InterConditionsRelationships.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterConditionsRelationship entity)
        {
            onUpdate();
            context.InterConditionsRelationships.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterConditionsRelationship> All()
        {
            return context.InterConditionsRelationships.ToList();
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
	 