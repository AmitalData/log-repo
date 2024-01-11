 
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
   public partial class CustomsAirlineRepository:IRepository<CustomsAirline>
   {
   
        private ICustomContext currentContext;
        public CustomsAirlineRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsAirlineRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsAirline GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsAirlines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsAirline> GetAll(int tenant)
        {
            return from a in context.CustomsAirlines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsAirline GetSingle(EntityKeyFields entityKeys)
        {
            CustomsAirlineKeys keys = entityKeys as CustomsAirlineKeys;
            return (from a in context.CustomsAirlines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsAirline entity)
        {
            onAdd();
            context.CustomsAirlines.Add(entity);
        }

        public void Remove(CustomsAirline entity)
        {
            context.CustomsAirlines.Attach(entity);
            context.CustomsAirlines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsAirline entity)
        {
            onUpdate();
            context.CustomsAirlines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsAirline> All()
        {
            return context.CustomsAirlines.ToList();
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
	 