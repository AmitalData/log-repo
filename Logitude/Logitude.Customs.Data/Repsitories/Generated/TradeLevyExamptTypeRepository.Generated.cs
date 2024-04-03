 
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
   public partial class TradeLevyExamptTypeRepository:IRepository<TradeLevyExamptType>
   {
   
        private ICustomContext currentContext;
        public TradeLevyExamptTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TradeLevyExamptTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TradeLevyExamptType GetSingle(string code)
        {
            return (from a in context.TradeLevyExamptTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TradeLevyExamptType> GetAll()
        {
            return from a in context.TradeLevyExamptTypes  
                   select a;
        }
				 
        public TradeLevyExamptType GetSingle(EntityKeyFields entityKeys)
        {
            TradeLevyExamptTypeKeys keys = entityKeys as TradeLevyExamptTypeKeys;
            return (from a in context.TradeLevyExamptTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TradeLevyExamptType entity)
        {
            onAdd();
            context.TradeLevyExamptTypes.Add(entity);
        }

        public void Remove(TradeLevyExamptType entity)
        {
            context.TradeLevyExamptTypes.Attach(entity);
            context.TradeLevyExamptTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TradeLevyExamptType entity)
        {
            onUpdate();
            context.TradeLevyExamptTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TradeLevyExamptType> All()
        {
            return context.TradeLevyExamptTypes.ToList();
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
	 