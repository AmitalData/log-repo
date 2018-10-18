 
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
   public partial class CheckTypeLookupRepository:IRepository<CheckTypeLookup>
   {
   
        private ICustomContext currentContext;
        public CheckTypeLookupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CheckTypeLookupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CheckTypeLookup GetSingle(string code)
        {
            return (from a in context.CheckTypeLookups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CheckTypeLookup> GetAll()
        {
            return from a in context.CheckTypeLookups  
                   select a;
        }
				 
        public CheckTypeLookup GetSingle(EntityKeyFields entityKeys)
        {
            CheckTypeLookupKeys keys = entityKeys as CheckTypeLookupKeys;
            return (from a in context.CheckTypeLookups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CheckTypeLookup entity)
        {
            onAdd();
            context.CheckTypeLookups.Add(entity);
        }

        public void Remove(CheckTypeLookup entity)
        {
            context.CheckTypeLookups.Attach(entity);
            context.CheckTypeLookups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CheckTypeLookup entity)
        {
            onUpdate();
            context.CheckTypeLookups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CheckTypeLookup> All()
        {
            return context.CheckTypeLookups.ToList();
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
	 