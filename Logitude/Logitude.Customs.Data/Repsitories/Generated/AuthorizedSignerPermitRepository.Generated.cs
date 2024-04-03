 
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
   public partial class AuthorizedSignerPermitRepository:IRepository<AuthorizedSignerPermit>
   {
   
        private ICustomContext currentContext;
        public AuthorizedSignerPermitRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AuthorizedSignerPermitRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AuthorizedSignerPermit GetSingle(string code)
        {
            return (from a in context.AuthorizedSignerPermits
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AuthorizedSignerPermit> GetAll()
        {
            return from a in context.AuthorizedSignerPermits  
                   select a;
        }
				 
        public AuthorizedSignerPermit GetSingle(EntityKeyFields entityKeys)
        {
            AuthorizedSignerPermitKeys keys = entityKeys as AuthorizedSignerPermitKeys;
            return (from a in context.AuthorizedSignerPermits
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AuthorizedSignerPermit entity)
        {
            onAdd();
            context.AuthorizedSignerPermits.Add(entity);
        }

        public void Remove(AuthorizedSignerPermit entity)
        {
            context.AuthorizedSignerPermits.Attach(entity);
            context.AuthorizedSignerPermits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AuthorizedSignerPermit entity)
        {
            onUpdate();
            context.AuthorizedSignerPermits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AuthorizedSignerPermit> All()
        {
            return context.AuthorizedSignerPermits.ToList();
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
	 