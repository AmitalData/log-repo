 
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
   public partial class CollateralAnswerTypeRepository:IRepository<CollateralAnswerType>
   {
   
        private ICustomContext currentContext;
        public CollateralAnswerTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CollateralAnswerTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CollateralAnswerType GetSingle(string code)
        {
            return (from a in context.CollateralAnswerTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CollateralAnswerType> GetAll()
        {
            return from a in context.CollateralAnswerTypes  
                   select a;
        }
				 
        public CollateralAnswerType GetSingle(EntityKeyFields entityKeys)
        {
            CollateralAnswerTypeKeys keys = entityKeys as CollateralAnswerTypeKeys;
            return (from a in context.CollateralAnswerTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CollateralAnswerType entity)
        {
            onAdd();
            context.CollateralAnswerTypes.Add(entity);
        }

        public void Remove(CollateralAnswerType entity)
        {
            context.CollateralAnswerTypes.Attach(entity);
            context.CollateralAnswerTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CollateralAnswerType entity)
        {
            onUpdate();
            context.CollateralAnswerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CollateralAnswerType> All()
        {
            return context.CollateralAnswerTypes.ToList();
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
	 