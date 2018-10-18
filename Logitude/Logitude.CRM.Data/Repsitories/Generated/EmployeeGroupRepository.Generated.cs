 
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
   public partial class EmployeeGroupRepository:IRepository<EmployeeGroup>
   {
   
        private ICRMContext currentContext;
        public EmployeeGroupRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public EmployeeGroupRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  EmployeeGroup GetSingle(string id, int tenant)
        {
            return (from a in context.EmployeeGroups
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<EmployeeGroup> GetAll(int tenant)
        {
            return from a in context.EmployeeGroups  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public EmployeeGroup GetSingle(EntityKeyFields entityKeys)
        {
            EmployeeGroupKeys keys = entityKeys as EmployeeGroupKeys;
            return (from a in context.EmployeeGroups
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EmployeeGroup entity)
        {
            onAdd();
            context.EmployeeGroups.Add(entity);
        }

        public void Remove(EmployeeGroup entity)
        {
            context.EmployeeGroups.Attach(entity);
            context.EmployeeGroups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EmployeeGroup entity)
        {
            onUpdate();
            context.EmployeeGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EmployeeGroup> All()
        {
            return context.EmployeeGroups.ToList();
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
	 