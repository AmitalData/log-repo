 
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
   public partial class LogisticActionResponseReqSRepository:IRepository<LogisticActionResponseReqS>
   {
   
        private ICustomContext currentContext;
        public LogisticActionResponseReqSRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LogisticActionResponseReqSRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LogisticActionResponseReqS GetSingle(string code)
        {
            return (from a in context.LogisticActionResponseReqSes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LogisticActionResponseReqS> GetAll()
        {
            return from a in context.LogisticActionResponseReqSes  
                   select a;
        }
				 
        public LogisticActionResponseReqS GetSingle(EntityKeyFields entityKeys)
        {
            LogisticActionResponseReqSKeys keys = entityKeys as LogisticActionResponseReqSKeys;
            return (from a in context.LogisticActionResponseReqSes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LogisticActionResponseReqS entity)
        {
            onAdd();
            context.LogisticActionResponseReqSes.Add(entity);
        }

        public void Remove(LogisticActionResponseReqS entity)
        {
            context.LogisticActionResponseReqSes.Attach(entity);
            context.LogisticActionResponseReqSes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LogisticActionResponseReqS entity)
        {
            onUpdate();
            context.LogisticActionResponseReqSes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogisticActionResponseReqS> All()
        {
            return context.LogisticActionResponseReqSes.ToList();
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
	 