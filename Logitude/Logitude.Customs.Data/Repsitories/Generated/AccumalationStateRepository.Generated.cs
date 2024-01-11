 
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
   public partial class AccumalationStateRepository:IRepository<AccumalationState>
   {
   
        private ICustomContext currentContext;
        public AccumalationStateRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AccumalationStateRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccumalationState GetSingle(string code)
        {
            return (from a in context.AccumalationStates
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AccumalationState> GetAll()
        {
            return from a in context.AccumalationStates  
                   select a;
        }
				 
        public AccumalationState GetSingle(EntityKeyFields entityKeys)
        {
            AccumalationStateKeys keys = entityKeys as AccumalationStateKeys;
            return (from a in context.AccumalationStates
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccumalationState entity)
        {
            onAdd();
            context.AccumalationStates.Add(entity);
        }

        public void Remove(AccumalationState entity)
        {
            context.AccumalationStates.Attach(entity);
            context.AccumalationStates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccumalationState entity)
        {
            onUpdate();
            context.AccumalationStates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccumalationState> All()
        {
            return context.AccumalationStates.ToList();
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
	 