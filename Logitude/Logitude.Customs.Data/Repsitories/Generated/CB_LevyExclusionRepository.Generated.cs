 
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
   public partial class CB_LevyExclusionRepository:IRepository<CB_LevyExclusion>
   {
   
        private ICustomContext currentContext;
        public CB_LevyExclusionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_LevyExclusionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_LevyExclusion GetSingle(string cb_id)
        {
            return (from a in context.CB_LevyExclusions
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_LevyExclusion> GetAll()
        {
            return from a in context.CB_LevyExclusions  
                   select a;
        }
				 
        public CB_LevyExclusion GetSingle(EntityKeyFields entityKeys)
        {
            CB_LevyExclusionKeys keys = entityKeys as CB_LevyExclusionKeys;
            return (from a in context.CB_LevyExclusions
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_LevyExclusion entity)
        {
            onAdd();
            context.CB_LevyExclusions.Add(entity);
        }

        public void Remove(CB_LevyExclusion entity)
        {
            context.CB_LevyExclusions.Attach(entity);
            context.CB_LevyExclusions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_LevyExclusion entity)
        {
            onUpdate();
            context.CB_LevyExclusions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_LevyExclusion> All()
        {
            return context.CB_LevyExclusions.ToList();
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
	 