 
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
   public partial class CB_RuleDetailsHistoryRepository:IRepository<CB_RuleDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_RuleDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RuleDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_RuleDetailsHistory GetSingle(string cb_id)
        {
            return (from a in context.CB_RuleDetailsHistorys
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_RuleDetailsHistory> GetAll()
        {
            return from a in context.CB_RuleDetailsHistorys  
                   select a;
        }
				 
        public CB_RuleDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_RuleDetailsHistoryKeys keys = entityKeys as CB_RuleDetailsHistoryKeys;
            return (from a in context.CB_RuleDetailsHistorys
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_RuleDetailsHistory entity)
        {
            onAdd();
            context.CB_RuleDetailsHistorys.Add(entity);
        }

        public void Remove(CB_RuleDetailsHistory entity)
        {
            context.CB_RuleDetailsHistorys.Attach(entity);
            context.CB_RuleDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_RuleDetailsHistory entity)
        {
            onUpdate();
            context.CB_RuleDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_RuleDetailsHistory> All()
        {
            return context.CB_RuleDetailsHistorys.ToList();
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
	 