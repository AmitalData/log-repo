 
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
   public partial class CustomsRequestsSheetRepository:IRepository<CustomsRequestsSheet>
   {
   
        private ICustomContext currentContext;
        public CustomsRequestsSheetRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsRequestsSheetRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsRequestsSheet GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsRequestsSheets
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsRequestsSheet> GetAll(int tenant)
        {
            return from a in context.CustomsRequestsSheets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsRequestsSheet GetSingle(EntityKeyFields entityKeys)
        {
            CustomsRequestsSheetKeys keys = entityKeys as CustomsRequestsSheetKeys;
            return (from a in context.CustomsRequestsSheets
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsRequestsSheet entity)
        {
            onAdd();
            context.CustomsRequestsSheets.Add(entity);
        }

        public void Remove(CustomsRequestsSheet entity)
        {
            context.CustomsRequestsSheets.Attach(entity);
            context.CustomsRequestsSheets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsRequestsSheet entity)
        {
            onUpdate();
            context.CustomsRequestsSheets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsRequestsSheet> All()
        {
            return context.CustomsRequestsSheets.ToList();
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
	 