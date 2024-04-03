 
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
   public partial class CustomerRoleTypeRepository:IRepository<CustomerRoleType>
   {
   
        private ICustomContext currentContext;
        public CustomerRoleTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerRoleTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerRoleType GetSingle(string code)
        {
            return (from a in context.CustomerRoleTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerRoleType> GetAll()
        {
            return from a in context.CustomerRoleTypes  
                   select a;
        }
				 
        public CustomerRoleType GetSingle(EntityKeyFields entityKeys)
        {
            CustomerRoleTypeKeys keys = entityKeys as CustomerRoleTypeKeys;
            return (from a in context.CustomerRoleTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerRoleType entity)
        {
            onAdd();
            context.CustomerRoleTypes.Add(entity);
        }

        public void Remove(CustomerRoleType entity)
        {
            context.CustomerRoleTypes.Attach(entity);
            context.CustomerRoleTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerRoleType entity)
        {
            onUpdate();
            context.CustomerRoleTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerRoleType> All()
        {
            return context.CustomerRoleTypes.ToList();
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
	 