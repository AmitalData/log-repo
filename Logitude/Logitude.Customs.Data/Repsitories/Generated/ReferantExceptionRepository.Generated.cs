 
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
   public partial class ReferantExceptionRepository:IRepository<ReferantException>
   {
   
        private ICustomContext currentContext;
        public ReferantExceptionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReferantExceptionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReferantException GetSingle(string declarationid, string exceptionreasonscode, int tenant)
        {
            return (from a in context.ReferantExceptions
                    where a.DeclarationId == declarationid && a.ExceptionReasonsCode == exceptionreasonscode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ReferantException> GetAll(int tenant)
        {
            return from a in context.ReferantExceptions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ReferantException GetSingle(EntityKeyFields entityKeys)
        {
            ReferantExceptionKeys keys = entityKeys as ReferantExceptionKeys;
            return (from a in context.ReferantExceptions
                    where a.DeclarationId == keys.DeclarationId && a.ExceptionReasonsCode == keys.ExceptionReasonsCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReferantException entity)
        {
            onAdd();
            context.ReferantExceptions.Add(entity);
        }

        public void Remove(ReferantException entity)
        {
            context.ReferantExceptions.Attach(entity);
            context.ReferantExceptions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReferantException entity)
        {
            onUpdate();
            context.ReferantExceptions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferantException> All()
        {
            return context.ReferantExceptions.ToList();
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
	 