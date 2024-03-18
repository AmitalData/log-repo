 
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
   public partial class CB_RegularityRequirementRepository:IRepository<CB_RegularityRequirement>
   {
   
        private ICustomContext currentContext;
        public CB_RegularityRequirementRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RegularityRequirementRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_RegularityRequirement GetSingle(string id)
        {
            return (from a in context.CB_RegularityRequirements
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_RegularityRequirement> GetAll()
        {
            return from a in context.CB_RegularityRequirements  
                   select a;
        }
				 
        public CB_RegularityRequirement GetSingle(EntityKeyFields entityKeys)
        {
            CB_RegularityRequirementKeys keys = entityKeys as CB_RegularityRequirementKeys;
            return (from a in context.CB_RegularityRequirements
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_RegularityRequirement entity)
        {
            onAdd();
            context.CB_RegularityRequirements.Add(entity);
        }

        public void Remove(CB_RegularityRequirement entity)
        {
            context.CB_RegularityRequirements.Attach(entity);
            context.CB_RegularityRequirements.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_RegularityRequirement entity)
        {
            onUpdate();
            context.CB_RegularityRequirements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_RegularityRequirement> All()
        {
            return context.CB_RegularityRequirements.ToList();
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
	 