 
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
   public partial class CourtInstanceRepository:IRepository<CourtInstance>
   {
   
        private ICustomContext currentContext;
        public CourtInstanceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourtInstanceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourtInstance GetSingle(string code)
        {
            return (from a in context.CourtInstances
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CourtInstance> GetAll()
        {
            return from a in context.CourtInstances  
                   select a;
        }
				 
        public CourtInstance GetSingle(EntityKeyFields entityKeys)
        {
            CourtInstanceKeys keys = entityKeys as CourtInstanceKeys;
            return (from a in context.CourtInstances
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourtInstance entity)
        {
            onAdd();
            context.CourtInstances.Add(entity);
        }

        public void Remove(CourtInstance entity)
        {
            context.CourtInstances.Attach(entity);
            context.CourtInstances.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourtInstance entity)
        {
            onUpdate();
            context.CourtInstances.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourtInstance> All()
        {
            return context.CourtInstances.ToList();
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
	 