 
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
   public partial class CollateralAnswerStatusRepository:IRepository<CollateralAnswerStatus>
   {
   
        private ICustomContext currentContext;
        public CollateralAnswerStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CollateralAnswerStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CollateralAnswerStatus GetSingle(string code)
        {
            return (from a in context.CollateralAnswerStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CollateralAnswerStatus> GetAll()
        {
            return from a in context.CollateralAnswerStatus  
                   select a;
        }
				 
        public CollateralAnswerStatus GetSingle(EntityKeyFields entityKeys)
        {
            CollateralAnswerStatusKeys keys = entityKeys as CollateralAnswerStatusKeys;
            return (from a in context.CollateralAnswerStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CollateralAnswerStatus entity)
        {
            onAdd();
            context.CollateralAnswerStatus.Add(entity);
        }

        public void Remove(CollateralAnswerStatus entity)
        {
            context.CollateralAnswerStatus.Attach(entity);
            context.CollateralAnswerStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CollateralAnswerStatus entity)
        {
            onUpdate();
            context.CollateralAnswerStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CollateralAnswerStatus> All()
        {
            return context.CollateralAnswerStatus.ToList();
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
	 