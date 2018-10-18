 
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
   public partial class AuthorityRepository:IRepository<Authority>
   {
   
        private ICustomContext currentContext;
        public AuthorityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AuthorityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Authority GetSingle(string code)
        {
            return (from a in context.Authorities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Authority> GetAll()
        {
            return from a in context.Authorities  
                   select a;
        }
				 
        public Authority GetSingle(EntityKeyFields entityKeys)
        {
            AuthorityKeys keys = entityKeys as AuthorityKeys;
            return (from a in context.Authorities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Authority entity)
        {
            onAdd();
            context.Authorities.Add(entity);
        }

        public void Remove(Authority entity)
        {
            context.Authorities.Attach(entity);
            context.Authorities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Authority entity)
        {
            onUpdate();
            context.Authorities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Authority> All()
        {
            return context.Authorities.ToList();
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
	 