 
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
   public partial class CustomsItemDetailsHistoryRepository:IRepository<CustomsItemDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CustomsItemDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsItemDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsItemDetailsHistory GetSingle(string id)
        {
            return (from a in context.CustomsItemDetailsHistorys
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsItemDetailsHistory> GetAll()
        {
            return from a in context.CustomsItemDetailsHistorys  
                   select a;
        }
				 
        public CustomsItemDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CustomsItemDetailsHistoryKeys keys = entityKeys as CustomsItemDetailsHistoryKeys;
            return (from a in context.CustomsItemDetailsHistorys
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsItemDetailsHistory entity)
        {
            onAdd();
            context.CustomsItemDetailsHistorys.Add(entity);
        }

        public void Remove(CustomsItemDetailsHistory entity)
        {
            context.CustomsItemDetailsHistorys.Attach(entity);
            context.CustomsItemDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsItemDetailsHistory entity)
        {
            onUpdate();
            context.CustomsItemDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsItemDetailsHistory> All()
        {
            return context.CustomsItemDetailsHistorys.ToList();
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
	 