 
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
   public partial class PerYearFrequencyRepository:IRepository<PerYearFrequency>
   {
   
        private ICustomContext currentContext;
        public PerYearFrequencyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PerYearFrequencyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PerYearFrequency GetSingle(string code)
        {
            return (from a in context.PerYearFrequencies
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PerYearFrequency> GetAll()
        {
            return from a in context.PerYearFrequencies  
                   select a;
        }
				 
        public PerYearFrequency GetSingle(EntityKeyFields entityKeys)
        {
            PerYearFrequencyKeys keys = entityKeys as PerYearFrequencyKeys;
            return (from a in context.PerYearFrequencies
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PerYearFrequency entity)
        {
            onAdd();
            context.PerYearFrequencies.Add(entity);
        }

        public void Remove(PerYearFrequency entity)
        {
            context.PerYearFrequencies.Attach(entity);
            context.PerYearFrequencies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PerYearFrequency entity)
        {
            onUpdate();
            context.PerYearFrequencies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PerYearFrequency> All()
        {
            return context.PerYearFrequencies.ToList();
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
	 