 
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
   public partial class PendingByKeywordRepository:IRepository<PendingByKeyword>
   {
   
        private ICustomContext currentContext;
        public PendingByKeywordRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PendingByKeywordRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PendingByKeyword GetSingle(string id, int tenant)
        {
            return (from a in context.PendingByKeywords
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PendingByKeyword> GetAll(int tenant)
        {
            return from a in context.PendingByKeywords  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PendingByKeyword GetSingle(EntityKeyFields entityKeys)
        {
            PendingByKeywordKeys keys = entityKeys as PendingByKeywordKeys;
            return (from a in context.PendingByKeywords
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PendingByKeyword entity)
        {
            onAdd();
            context.PendingByKeywords.Add(entity);
        }

        public void Remove(PendingByKeyword entity)
        {
            context.PendingByKeywords.Attach(entity);
            context.PendingByKeywords.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PendingByKeyword entity)
        {
            onUpdate();
            context.PendingByKeywords.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PendingByKeyword> All()
        {
            return context.PendingByKeywords.ToList();
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
	 