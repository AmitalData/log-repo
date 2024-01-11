 
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
   public partial class DecisionTypeRepository:IRepository<DecisionType>
   {
   
        private ICustomContext currentContext;
        public DecisionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecisionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecisionType GetSingle(string code)
        {
            return (from a in context.DecisionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DecisionType> GetAll()
        {
            return from a in context.DecisionTypes  
                   select a;
        }
				 
        public DecisionType GetSingle(EntityKeyFields entityKeys)
        {
            DecisionTypeKeys keys = entityKeys as DecisionTypeKeys;
            return (from a in context.DecisionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecisionType entity)
        {
            onAdd();
            context.DecisionTypes.Add(entity);
        }

        public void Remove(DecisionType entity)
        {
            context.DecisionTypes.Attach(entity);
            context.DecisionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecisionType entity)
        {
            onUpdate();
            context.DecisionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecisionType> All()
        {
            return context.DecisionTypes.ToList();
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
	 