 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class EmployeeGroupLineRepository:IRepository<EmployeeGroupLine>
   {
   
        private ICRMContext currentContext;
        public EmployeeGroupLineRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public EmployeeGroupLineRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  EmployeeGroupLine GetSingle(string id, int tenant)
        {
            return (from a in context.EmployeeGroupLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<EmployeeGroupLine> GetAll(int tenant)
        {
            return from a in context.EmployeeGroupLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public EmployeeGroupLine GetSingle(EntityKeyFields entityKeys)
        {
            EmployeeGroupLineKeys keys = entityKeys as EmployeeGroupLineKeys;
            return (from a in context.EmployeeGroupLines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EmployeeGroupLine entity)
        {
            onAdd();
            context.EmployeeGroupLines.Add(entity);
        }

        public void Remove(EmployeeGroupLine entity)
        {
            context.EmployeeGroupLines.Attach(entity);
            context.EmployeeGroupLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EmployeeGroupLine entity)
        {
            onUpdate();
            context.EmployeeGroupLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EmployeeGroupLine> All()
        {
            return context.EmployeeGroupLines.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 