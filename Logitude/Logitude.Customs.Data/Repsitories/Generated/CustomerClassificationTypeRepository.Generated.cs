 
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
   public partial class CustomerClassificationTypeRepository:IRepository<CustomerClassificationType>
   {
   
        private ICustomContext currentContext;
        public CustomerClassificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerClassificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerClassificationType GetSingle(string code)
        {
            return (from a in context.CustomerClassificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerClassificationType> GetAll()
        {
            return from a in context.CustomerClassificationTypes  
                   select a;
        }
				 
        public CustomerClassificationType GetSingle(EntityKeyFields entityKeys)
        {
            CustomerClassificationTypeKeys keys = entityKeys as CustomerClassificationTypeKeys;
            return (from a in context.CustomerClassificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerClassificationType entity)
        {
            onAdd();
            context.CustomerClassificationTypes.Add(entity);
        }

        public void Remove(CustomerClassificationType entity)
        {
            context.CustomerClassificationTypes.Attach(entity);
            context.CustomerClassificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerClassificationType entity)
        {
            onUpdate();
            context.CustomerClassificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerClassificationType> All()
        {
            return context.CustomerClassificationTypes.ToList();
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
	 