 
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
   public partial class CustomsAutonomyKeywordRepository:IRepository<CustomsAutonomyKeyword>
   {
   
        private ICustomContext currentContext;
        public CustomsAutonomyKeywordRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsAutonomyKeywordRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsAutonomyKeyword GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsAutonomyKeywords
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsAutonomyKeyword> GetAll(int tenant)
        {
            return from a in context.CustomsAutonomyKeywords  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsAutonomyKeyword GetSingle(EntityKeyFields entityKeys)
        {
            CustomsAutonomyKeywordKeys keys = entityKeys as CustomsAutonomyKeywordKeys;
            return (from a in context.CustomsAutonomyKeywords
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsAutonomyKeyword entity)
        {
            onAdd();
            context.CustomsAutonomyKeywords.Add(entity);
        }

        public void Remove(CustomsAutonomyKeyword entity)
        {
            context.CustomsAutonomyKeywords.Attach(entity);
            context.CustomsAutonomyKeywords.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsAutonomyKeyword entity)
        {
            onUpdate();
            context.CustomsAutonomyKeywords.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsAutonomyKeyword> All()
        {
            return context.CustomsAutonomyKeywords.ToList();
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
	 