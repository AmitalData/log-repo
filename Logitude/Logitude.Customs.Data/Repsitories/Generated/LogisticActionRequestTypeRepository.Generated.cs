 
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
   public partial class LogisticActionRequestTypeRepository:IRepository<LogisticActionRequestType>
   {
   
        private ICustomContext currentContext;
        public LogisticActionRequestTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LogisticActionRequestTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LogisticActionRequestType GetSingle(string code)
        {
            return (from a in context.LogisticActionRequestTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LogisticActionRequestType> GetAll()
        {
            return from a in context.LogisticActionRequestTypes  
                   select a;
        }
				 
        public LogisticActionRequestType GetSingle(EntityKeyFields entityKeys)
        {
            LogisticActionRequestTypeKeys keys = entityKeys as LogisticActionRequestTypeKeys;
            return (from a in context.LogisticActionRequestTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LogisticActionRequestType entity)
        {
            onAdd();
            context.LogisticActionRequestTypes.Add(entity);
        }

        public void Remove(LogisticActionRequestType entity)
        {
            context.LogisticActionRequestTypes.Attach(entity);
            context.LogisticActionRequestTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LogisticActionRequestType entity)
        {
            onUpdate();
            context.LogisticActionRequestTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogisticActionRequestType> All()
        {
            return context.LogisticActionRequestTypes.ToList();
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
	 