 
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
   public partial class LogisticActionResponseRequestSRepository:IRepository<LogisticActionResponseRequestS>
   {
   
        private ICustomContext currentContext;
        public LogisticActionResponseRequestSRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LogisticActionResponseRequestSRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LogisticActionResponseRequestS GetSingle(string code)
        {
            return (from a in context.LogisticActionResponseRequestSes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LogisticActionResponseRequestS> GetAll()
        {
            return from a in context.LogisticActionResponseRequestSes  
                   select a;
        }
				 
        public LogisticActionResponseRequestS GetSingle(EntityKeyFields entityKeys)
        {
            LogisticActionResponseRequestSKeys keys = entityKeys as LogisticActionResponseRequestSKeys;
            return (from a in context.LogisticActionResponseRequestSes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LogisticActionResponseRequestS entity)
        {
            onAdd();
            context.LogisticActionResponseRequestSes.Add(entity);
        }

        public void Remove(LogisticActionResponseRequestS entity)
        {
            context.LogisticActionResponseRequestSes.Attach(entity);
            context.LogisticActionResponseRequestSes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LogisticActionResponseRequestS entity)
        {
            onUpdate();
            context.LogisticActionResponseRequestSes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LogisticActionResponseRequestS> All()
        {
            return context.LogisticActionResponseRequestSes.ToList();
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
	 