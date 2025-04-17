 
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
   public partial class RequestHandlingAuthorityRepository:IRepository<RequestHandlingAuthority>
   {
   
        private ICustomContext currentContext;
        public RequestHandlingAuthorityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RequestHandlingAuthorityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RequestHandlingAuthority GetSingle(string code)
        {
            return (from a in context.RequestHandlingAuthorities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RequestHandlingAuthority> GetAll()
        {
            return from a in context.RequestHandlingAuthorities  
                   select a;
        }
				 
        public RequestHandlingAuthority GetSingle(EntityKeyFields entityKeys)
        {
            RequestHandlingAuthorityKeys keys = entityKeys as RequestHandlingAuthorityKeys;
            return (from a in context.RequestHandlingAuthorities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RequestHandlingAuthority entity)
        {
            onAdd();
            context.RequestHandlingAuthorities.Add(entity);
        }

        public void Remove(RequestHandlingAuthority entity)
        {
            context.RequestHandlingAuthorities.Attach(entity);
            context.RequestHandlingAuthorities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RequestHandlingAuthority entity)
        {
            onUpdate();
            context.RequestHandlingAuthorities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RequestHandlingAuthority> All()
        {
            return context.RequestHandlingAuthorities.ToList();
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
	 