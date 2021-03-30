 
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
   public partial class CustomerIdentificationTypeRepository:IRepository<CustomerIdentificationType>
   {
   
        private ICustomContext currentContext;
        public CustomerIdentificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerIdentificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerIdentificationType GetSingle(string code)
        {
            return (from a in context.CustomerIdentificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerIdentificationType> GetAll()
        {
            return from a in context.CustomerIdentificationTypes  
                   select a;
        }
				 
        public CustomerIdentificationType GetSingle(EntityKeyFields entityKeys)
        {
            CustomerIdentificationTypeKeys keys = entityKeys as CustomerIdentificationTypeKeys;
            return (from a in context.CustomerIdentificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerIdentificationType entity)
        {
            onAdd();
            context.CustomerIdentificationTypes.Add(entity);
        }

        public void Remove(CustomerIdentificationType entity)
        {
            context.CustomerIdentificationTypes.Attach(entity);
            context.CustomerIdentificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerIdentificationType entity)
        {
            onUpdate();
            context.CustomerIdentificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerIdentificationType> All()
        {
            return context.CustomerIdentificationTypes.ToList();
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
	 