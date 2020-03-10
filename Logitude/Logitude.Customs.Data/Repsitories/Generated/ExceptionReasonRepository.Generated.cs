 
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
   public partial class ExceptionReasonRepository:IRepository<ExceptionReason>
   {
   
        private ICustomContext currentContext;
        public ExceptionReasonRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExceptionReasonRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExceptionReason GetSingle(string code, int tenant)
        {
            return (from a in context.ExceptionReasons
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExceptionReason> GetAll(int tenant)
        {
            return from a in context.ExceptionReasons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExceptionReason GetSingle(EntityKeyFields entityKeys)
        {
            ExceptionReasonKeys keys = entityKeys as ExceptionReasonKeys;
            return (from a in context.ExceptionReasons
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExceptionReason entity)
        {
            onAdd();
            context.ExceptionReasons.Add(entity);
        }

        public void Remove(ExceptionReason entity)
        {
            context.ExceptionReasons.Attach(entity);
            context.ExceptionReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExceptionReason entity)
        {
            onUpdate();
            context.ExceptionReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExceptionReason> All()
        {
            return context.ExceptionReasons.ToList();
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
	 