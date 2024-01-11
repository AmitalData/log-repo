 
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
   public partial class CustomsClosedTableRepository:IRepository<CustomsClosedTable>
   {
   
        private ICustomContext currentContext;
        public CustomsClosedTableRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsClosedTableRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsClosedTable GetSingle(string id)
        {
            return (from a in context.CustomsClosedTables
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsClosedTable> GetAll()
        {
            return from a in context.CustomsClosedTables  
                   select a;
        }
				 
        public CustomsClosedTable GetSingle(EntityKeyFields entityKeys)
        {
            CustomsClosedTableKeys keys = entityKeys as CustomsClosedTableKeys;
            return (from a in context.CustomsClosedTables
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsClosedTable entity)
        {
            onAdd();
            context.CustomsClosedTables.Add(entity);
        }

        public void Remove(CustomsClosedTable entity)
        {
            context.CustomsClosedTables.Attach(entity);
            context.CustomsClosedTables.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsClosedTable entity)
        {
            onUpdate();
            context.CustomsClosedTables.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsClosedTable> All()
        {
            return context.CustomsClosedTables.ToList();
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
	 