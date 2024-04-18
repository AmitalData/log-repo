 
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
   public partial class CB_AdditionRulesDetailsHistoryRepository:IRepository<CB_AdditionRulesDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_AdditionRulesDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_AdditionRulesDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_AdditionRulesDetailsHistory GetSingle(string cb_id)
        {
            return (from a in context.CB_AdditionRulesDetailsHistorys
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_AdditionRulesDetailsHistory> GetAll()
        {
            return from a in context.CB_AdditionRulesDetailsHistorys  
                   select a;
        }
				 
        public CB_AdditionRulesDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_AdditionRulesDetailsHistoryKeys keys = entityKeys as CB_AdditionRulesDetailsHistoryKeys;
            return (from a in context.CB_AdditionRulesDetailsHistorys
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_AdditionRulesDetailsHistory entity)
        {
            onAdd();
            context.CB_AdditionRulesDetailsHistorys.Add(entity);
        }

        public void Remove(CB_AdditionRulesDetailsHistory entity)
        {
            context.CB_AdditionRulesDetailsHistorys.Attach(entity);
            context.CB_AdditionRulesDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_AdditionRulesDetailsHistory entity)
        {
            onUpdate();
            context.CB_AdditionRulesDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_AdditionRulesDetailsHistory> All()
        {
            return context.CB_AdditionRulesDetailsHistorys.ToList();
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
	 