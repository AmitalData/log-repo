 
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
   public partial class CouriersVatRepository:IRepository<CouriersVat>
   {
   
        private ICustomContext currentContext;
        public CouriersVatRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CouriersVatRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CouriersVat GetSingle(string id, int tenant)
        {
            return (from a in context.CouriersVats
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CouriersVat> GetAll(int tenant)
        {
            return from a in context.CouriersVats  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CouriersVat GetSingle(EntityKeyFields entityKeys)
        {
            CouriersVatKeys keys = entityKeys as CouriersVatKeys;
            return (from a in context.CouriersVats
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CouriersVat entity)
        {
            onAdd();
            context.CouriersVats.Add(entity);
        }

        public void Remove(CouriersVat entity)
        {
            context.CouriersVats.Attach(entity);
            context.CouriersVats.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CouriersVat entity)
        {
            onUpdate();
            context.CouriersVats.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CouriersVat> All()
        {
            return context.CouriersVats.ToList();
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
	 