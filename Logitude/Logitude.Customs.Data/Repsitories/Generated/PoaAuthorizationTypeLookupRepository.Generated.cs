 
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
   public partial class PoaAuthorizationTypeLookupRepository:IRepository<PoaAuthorizationTypeLookup>
   {
   
        private ICustomContext currentContext;
        public PoaAuthorizationTypeLookupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PoaAuthorizationTypeLookupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PoaAuthorizationTypeLookup GetSingle(string code)
        {
            return (from a in context.PoaAuthorizationTypeLookups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PoaAuthorizationTypeLookup> GetAll()
        {
            return from a in context.PoaAuthorizationTypeLookups  
                   select a;
        }
				 
        public PoaAuthorizationTypeLookup GetSingle(EntityKeyFields entityKeys)
        {
            PoaAuthorizationTypeLookupKeys keys = entityKeys as PoaAuthorizationTypeLookupKeys;
            return (from a in context.PoaAuthorizationTypeLookups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PoaAuthorizationTypeLookup entity)
        {
            onAdd();
            context.PoaAuthorizationTypeLookups.Add(entity);
        }

        public void Remove(PoaAuthorizationTypeLookup entity)
        {
            context.PoaAuthorizationTypeLookups.Attach(entity);
            context.PoaAuthorizationTypeLookups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PoaAuthorizationTypeLookup entity)
        {
            onUpdate();
            context.PoaAuthorizationTypeLookups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PoaAuthorizationTypeLookup> All()
        {
            return context.PoaAuthorizationTypeLookups.ToList();
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
	 