 
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
   public partial class CustomerIndicationTypeRepository:IRepository<CustomerIndicationType>
   {
   
        private ICustomContext currentContext;
        public CustomerIndicationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerIndicationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerIndicationType GetSingle(string code)
        {
            return (from a in context.CustomerIndicationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerIndicationType> GetAll()
        {
            return from a in context.CustomerIndicationTypes  
                   select a;
        }
				 
        public CustomerIndicationType GetSingle(EntityKeyFields entityKeys)
        {
            CustomerIndicationTypeKeys keys = entityKeys as CustomerIndicationTypeKeys;
            return (from a in context.CustomerIndicationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerIndicationType entity)
        {
            onAdd();
            context.CustomerIndicationTypes.Add(entity);
        }

        public void Remove(CustomerIndicationType entity)
        {
            context.CustomerIndicationTypes.Attach(entity);
            context.CustomerIndicationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerIndicationType entity)
        {
            onUpdate();
            context.CustomerIndicationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerIndicationType> All()
        {
            return context.CustomerIndicationTypes.ToList();
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
	 