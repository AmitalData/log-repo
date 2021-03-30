 
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
   public partial class CoolingReportingMethodRepository:IRepository<CoolingReportingMethod>
   {
   
        private ICustomContext currentContext;
        public CoolingReportingMethodRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CoolingReportingMethodRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CoolingReportingMethod GetSingle(string code)
        {
            return (from a in context.CoolingReportingMethods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CoolingReportingMethod> GetAll()
        {
            return from a in context.CoolingReportingMethods  
                   select a;
        }
				 
        public CoolingReportingMethod GetSingle(EntityKeyFields entityKeys)
        {
            CoolingReportingMethodKeys keys = entityKeys as CoolingReportingMethodKeys;
            return (from a in context.CoolingReportingMethods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CoolingReportingMethod entity)
        {
            onAdd();
            context.CoolingReportingMethods.Add(entity);
        }

        public void Remove(CoolingReportingMethod entity)
        {
            context.CoolingReportingMethods.Attach(entity);
            context.CoolingReportingMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CoolingReportingMethod entity)
        {
            onUpdate();
            context.CoolingReportingMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CoolingReportingMethod> All()
        {
            return context.CoolingReportingMethods.ToList();
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
	 