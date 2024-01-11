 
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
   public partial class CustomerIdentifyTypeRepository:IRepository<CustomerIdentifyType>
   {
   
        private ICustomContext currentContext;
        public CustomerIdentifyTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerIdentifyTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerIdentifyType GetSingle(string code)
        {
            return (from a in context.CustomerIdentifyTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerIdentifyType> GetAll()
        {
            return from a in context.CustomerIdentifyTypes  
                   select a;
        }
				 
        public CustomerIdentifyType GetSingle(EntityKeyFields entityKeys)
        {
            CustomerIdentifyTypeKeys keys = entityKeys as CustomerIdentifyTypeKeys;
            return (from a in context.CustomerIdentifyTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerIdentifyType entity)
        {
            onAdd();
            context.CustomerIdentifyTypes.Add(entity);
        }

        public void Remove(CustomerIdentifyType entity)
        {
            context.CustomerIdentifyTypes.Attach(entity);
            context.CustomerIdentifyTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerIdentifyType entity)
        {
            onUpdate();
            context.CustomerIdentifyTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerIdentifyType> All()
        {
            return context.CustomerIdentifyTypes.ToList();
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
	 