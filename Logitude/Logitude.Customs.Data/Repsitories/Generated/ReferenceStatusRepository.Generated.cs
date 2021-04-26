 
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
   public partial class ReferenceStatusRepository:IRepository<ReferenceStatus>
   {
   
        private ICustomContext currentContext;
        public ReferenceStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReferenceStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReferenceStatus GetSingle(string code)
        {
            return (from a in context.ReferenceStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReferenceStatus> GetAll()
        {
            return from a in context.ReferenceStatuses  
                   select a;
        }
				 
        public ReferenceStatus GetSingle(EntityKeyFields entityKeys)
        {
            ReferenceStatusKeys keys = entityKeys as ReferenceStatusKeys;
            return (from a in context.ReferenceStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReferenceStatus entity)
        {
            onAdd();
            context.ReferenceStatuses.Add(entity);
        }

        public void Remove(ReferenceStatus entity)
        {
            context.ReferenceStatuses.Attach(entity);
            context.ReferenceStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReferenceStatus entity)
        {
            onUpdate();
            context.ReferenceStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferenceStatus> All()
        {
            return context.ReferenceStatuses.ToList();
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
	 