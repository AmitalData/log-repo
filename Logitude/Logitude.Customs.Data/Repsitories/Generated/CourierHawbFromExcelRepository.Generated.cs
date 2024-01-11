 
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
   public partial class CourierHawbFromExcelRepository:IRepository<CourierHawbFromExcel>
   {
   
        private ICustomContext currentContext;
        public CourierHawbFromExcelRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierHawbFromExcelRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierHawbFromExcel GetSingle(string declarationid, int tenant)
        {
            return (from a in context.CourierHawbFromExcels
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierHawbFromExcel> GetAll(int tenant)
        {
            return from a in context.CourierHawbFromExcels  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CourierHawbFromExcel GetSingle(EntityKeyFields entityKeys)
        {
            CourierHawbFromExcelKeys keys = entityKeys as CourierHawbFromExcelKeys;
            return (from a in context.CourierHawbFromExcels
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierHawbFromExcel entity)
        {
            onAdd();
            context.CourierHawbFromExcels.Add(entity);
        }

        public void Remove(CourierHawbFromExcel entity)
        {
            context.CourierHawbFromExcels.Attach(entity);
            context.CourierHawbFromExcels.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierHawbFromExcel entity)
        {
            onUpdate();
            context.CourierHawbFromExcels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierHawbFromExcel> All()
        {
            return context.CourierHawbFromExcels.ToList();
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
	 