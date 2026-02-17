 
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
   public partial class ClaimRepository:IRepository<Claim>
   {
   
        private ICustomContext currentContext;
        public ClaimRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Claim GetSingle(string id, int tenant)
        {
            return (from a in context.Claims
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Claim> GetAll(int tenant)
        {
            return from a in context.Claims  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Claim GetSingle(EntityKeyFields entityKeys)
        {
            ClaimKeys keys = entityKeys as ClaimKeys;
            return (from a in context.Claims
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Claim entity)
        {
            onAdd();
            context.Claims.Add(entity);
        }

        public void Remove(Claim entity)
        {
            context.Claims.Attach(entity);
            context.Claims.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Claim entity)
        {
            onUpdate();
            context.Claims.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Claim> All()
        {
            return context.Claims.ToList();
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
	 