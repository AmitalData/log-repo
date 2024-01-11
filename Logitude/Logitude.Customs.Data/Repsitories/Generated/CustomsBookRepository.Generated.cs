 
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
   public partial class CustomsBookRepository:IRepository<CustomsBook>
   {
   
        private ICustomContext currentContext;
        public CustomsBookRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsBookRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsBook GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsBooks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsBook> GetAll(int tenant)
        {
            return from a in context.CustomsBooks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsBook GetSingle(EntityKeyFields entityKeys)
        {
            CustomsBookKeys keys = entityKeys as CustomsBookKeys;
            return (from a in context.CustomsBooks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsBook entity)
        {
            onAdd();
            context.CustomsBooks.Add(entity);
        }

        public void Remove(CustomsBook entity)
        {
            context.CustomsBooks.Attach(entity);
            context.CustomsBooks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsBook entity)
        {
            onUpdate();
            context.CustomsBooks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsBook> All()
        {
            return context.CustomsBooks.ToList();
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
	 