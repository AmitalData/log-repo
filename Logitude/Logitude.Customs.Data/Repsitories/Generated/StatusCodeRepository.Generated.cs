 
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
   public partial class StatusCodeRepository:IRepository<StatusCode>
   {
   
        private ICustomContext currentContext;
        public StatusCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public StatusCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  StatusCode GetSingle(string id, int tenant)
        {
            return (from a in context.StatusCodes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<StatusCode> GetAll(int tenant)
        {
            return from a in context.StatusCodes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public StatusCode GetSingle(EntityKeyFields entityKeys)
        {
            StatusCodeKeys keys = entityKeys as StatusCodeKeys;
            return (from a in context.StatusCodes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(StatusCode entity)
        {
            onAdd();
            context.StatusCodes.Add(entity);
        }

        public void Remove(StatusCode entity)
        {
            context.StatusCodes.Attach(entity);
            context.StatusCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(StatusCode entity)
        {
            onUpdate();
            context.StatusCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<StatusCode> All()
        {
            return context.StatusCodes.ToList();
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
	 