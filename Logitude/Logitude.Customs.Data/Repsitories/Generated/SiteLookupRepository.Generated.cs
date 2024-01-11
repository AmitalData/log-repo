 
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
   public partial class SiteLookupRepository:IRepository<SiteLookup>
   {
   
        private ICustomContext currentContext;
        public SiteLookupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SiteLookupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SiteLookup GetSingle(string code)
        {
            return (from a in context.SiteLookups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SiteLookup> GetAll()
        {
            return from a in context.SiteLookups  
                   select a;
        }
				 
        public SiteLookup GetSingle(EntityKeyFields entityKeys)
        {
            SiteLookupKeys keys = entityKeys as SiteLookupKeys;
            return (from a in context.SiteLookups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SiteLookup entity)
        {
            onAdd();
            context.SiteLookups.Add(entity);
        }

        public void Remove(SiteLookup entity)
        {
            context.SiteLookups.Attach(entity);
            context.SiteLookups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SiteLookup entity)
        {
            onUpdate();
            context.SiteLookups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SiteLookup> All()
        {
            return context.SiteLookups.ToList();
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
	 