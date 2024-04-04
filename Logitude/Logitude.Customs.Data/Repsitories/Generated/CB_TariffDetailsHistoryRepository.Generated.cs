 
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
   public partial class CB_TariffDetailsHistoryRepository:IRepository<CB_TariffDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_TariffDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_TariffDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_TariffDetailsHistory GetSingle(string cb_id)
        {
            return (from a in context.CB_TariffDetailsHistorys
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_TariffDetailsHistory> GetAll()
        {
            return from a in context.CB_TariffDetailsHistorys  
                   select a;
        }
				 
        public CB_TariffDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_TariffDetailsHistoryKeys keys = entityKeys as CB_TariffDetailsHistoryKeys;
            return (from a in context.CB_TariffDetailsHistorys
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_TariffDetailsHistory entity)
        {
            onAdd();
            context.CB_TariffDetailsHistorys.Add(entity);
        }

        public void Remove(CB_TariffDetailsHistory entity)
        {
            context.CB_TariffDetailsHistorys.Attach(entity);
            context.CB_TariffDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_TariffDetailsHistory entity)
        {
            onUpdate();
            context.CB_TariffDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_TariffDetailsHistory> All()
        {
            return context.CB_TariffDetailsHistorys.ToList();
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
	 