 
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
   public partial class CheckQueueTypeRepository:IRepository<CheckQueueType>
   {
   
        private ICustomContext currentContext;
        public CheckQueueTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CheckQueueTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CheckQueueType GetSingle(string code)
        {
            return (from a in context.CheckQueueTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CheckQueueType> GetAll()
        {
            return from a in context.CheckQueueTypes  
                   select a;
        }
				 
        public CheckQueueType GetSingle(EntityKeyFields entityKeys)
        {
            CheckQueueTypeKeys keys = entityKeys as CheckQueueTypeKeys;
            return (from a in context.CheckQueueTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CheckQueueType entity)
        {
            onAdd();
            context.CheckQueueTypes.Add(entity);
        }

        public void Remove(CheckQueueType entity)
        {
            context.CheckQueueTypes.Attach(entity);
            context.CheckQueueTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CheckQueueType entity)
        {
            onUpdate();
            context.CheckQueueTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CheckQueueType> All()
        {
            return context.CheckQueueTypes.ToList();
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
	 