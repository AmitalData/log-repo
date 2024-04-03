 
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
   public partial class CustomerTypeGeneralRepository:IRepository<CustomerTypeGeneral>
   {
   
        private ICustomContext currentContext;
        public CustomerTypeGeneralRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomerTypeGeneralRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomerTypeGeneral GetSingle(string code)
        {
            return (from a in context.CustomerTypeGenerals
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomerTypeGeneral> GetAll()
        {
            return from a in context.CustomerTypeGenerals  
                   select a;
        }
				 
        public CustomerTypeGeneral GetSingle(EntityKeyFields entityKeys)
        {
            CustomerTypeGeneralKeys keys = entityKeys as CustomerTypeGeneralKeys;
            return (from a in context.CustomerTypeGenerals
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomerTypeGeneral entity)
        {
            onAdd();
            context.CustomerTypeGenerals.Add(entity);
        }

        public void Remove(CustomerTypeGeneral entity)
        {
            context.CustomerTypeGenerals.Attach(entity);
            context.CustomerTypeGenerals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomerTypeGeneral entity)
        {
            onUpdate();
            context.CustomerTypeGenerals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomerTypeGeneral> All()
        {
            return context.CustomerTypeGenerals.ToList();
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
	 