 
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
   public partial class CB_CustomsItemDetailsHistoryRepository:IRepository<CB_CustomsItemDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsItemDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsItemDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsItemDetailsHistory GetSingle(string cb_id)
        {
            return (from a in context.CB_CustomsItemDetailsHistorys
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsItemDetailsHistory> GetAll()
        {
            return from a in context.CB_CustomsItemDetailsHistorys  
                   select a;
        }
				 
        public CB_CustomsItemDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsItemDetailsHistoryKeys keys = entityKeys as CB_CustomsItemDetailsHistoryKeys;
            return (from a in context.CB_CustomsItemDetailsHistorys
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsItemDetailsHistory entity)
        {
            onAdd();
            context.CB_CustomsItemDetailsHistorys.Add(entity);
        }

        public void Remove(CB_CustomsItemDetailsHistory entity)
        {
            context.CB_CustomsItemDetailsHistorys.Attach(entity);
            context.CB_CustomsItemDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsItemDetailsHistory entity)
        {
            onUpdate();
            context.CB_CustomsItemDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsItemDetailsHistory> All()
        {
            return context.CB_CustomsItemDetailsHistorys.ToList();
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
	 