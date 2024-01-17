 
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
   public partial class CB_CountriesExclusionRepository:IRepository<CB_CountriesExclusion>
   {
   
        private ICustomContext currentContext;
        public CB_CountriesExclusionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CountriesExclusionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CountriesExclusion GetSingle(string id)
        {
            return (from a in context.CB_CountriesExclusions
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CountriesExclusion> GetAll()
        {
            return from a in context.CB_CountriesExclusions  
                   select a;
        }
				 
        public CB_CountriesExclusion GetSingle(EntityKeyFields entityKeys)
        {
            CB_CountriesExclusionKeys keys = entityKeys as CB_CountriesExclusionKeys;
            return (from a in context.CB_CountriesExclusions
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CountriesExclusion entity)
        {
            onAdd();
            context.CB_CountriesExclusions.Add(entity);
        }

        public void Remove(CB_CountriesExclusion entity)
        {
            context.CB_CountriesExclusions.Attach(entity);
            context.CB_CountriesExclusions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CountriesExclusion entity)
        {
            onUpdate();
            context.CB_CountriesExclusions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CountriesExclusion> All()
        {
            return context.CB_CountriesExclusions.ToList();
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
	 