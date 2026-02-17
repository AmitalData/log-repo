 
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
   public partial class CheckEssenceLookupRepository:IRepository<CheckEssenceLookup>
   {
   
        private ICustomContext currentContext;
        public CheckEssenceLookupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CheckEssenceLookupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CheckEssenceLookup GetSingle(string code)
        {
            return (from a in context.CheckEssenceLookups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CheckEssenceLookup> GetAll()
        {
            return from a in context.CheckEssenceLookups  
                   select a;
        }
				 
        public CheckEssenceLookup GetSingle(EntityKeyFields entityKeys)
        {
            CheckEssenceLookupKeys keys = entityKeys as CheckEssenceLookupKeys;
            return (from a in context.CheckEssenceLookups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CheckEssenceLookup entity)
        {
            onAdd();
            context.CheckEssenceLookups.Add(entity);
        }

        public void Remove(CheckEssenceLookup entity)
        {
            context.CheckEssenceLookups.Attach(entity);
            context.CheckEssenceLookups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CheckEssenceLookup entity)
        {
            onUpdate();
            context.CheckEssenceLookups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CheckEssenceLookup> All()
        {
            return context.CheckEssenceLookups.ToList();
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
	 