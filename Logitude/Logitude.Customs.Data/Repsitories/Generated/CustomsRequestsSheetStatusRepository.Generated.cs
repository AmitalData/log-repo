 
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
   public partial class CustomsRequestsSheetStatusRepository:IRepository<CustomsRequestsSheetStatus>
   {
   
        private ICustomContext currentContext;
        public CustomsRequestsSheetStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsRequestsSheetStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsRequestsSheetStatus GetSingle(string code)
        {
            return (from a in context.CustomsRequestsSheetStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsRequestsSheetStatus> GetAll()
        {
            return from a in context.CustomsRequestsSheetStatuses  
                   select a;
        }
				 
        public CustomsRequestsSheetStatus GetSingle(EntityKeyFields entityKeys)
        {
            CustomsRequestsSheetStatusKeys keys = entityKeys as CustomsRequestsSheetStatusKeys;
            return (from a in context.CustomsRequestsSheetStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsRequestsSheetStatus entity)
        {
            onAdd();
            context.CustomsRequestsSheetStatuses.Add(entity);
        }

        public void Remove(CustomsRequestsSheetStatus entity)
        {
            context.CustomsRequestsSheetStatuses.Attach(entity);
            context.CustomsRequestsSheetStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsRequestsSheetStatus entity)
        {
            onUpdate();
            context.CustomsRequestsSheetStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsRequestsSheetStatus> All()
        {
            return context.CustomsRequestsSheetStatuses.ToList();
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
	 