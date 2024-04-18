 
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
   public partial class CB_TradeAgreementHistoryRepository:IRepository<CB_TradeAgreementHistory>
   {
   
        private ICustomContext currentContext;
        public CB_TradeAgreementHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_TradeAgreementHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_TradeAgreementHistory GetSingle(string cb_id)
        {
            return (from a in context.CB_TradeAgreementHistories
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_TradeAgreementHistory> GetAll()
        {
            return from a in context.CB_TradeAgreementHistories  
                   select a;
        }
				 
        public CB_TradeAgreementHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_TradeAgreementHistoryKeys keys = entityKeys as CB_TradeAgreementHistoryKeys;
            return (from a in context.CB_TradeAgreementHistories
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_TradeAgreementHistory entity)
        {
            onAdd();
            context.CB_TradeAgreementHistories.Add(entity);
        }

        public void Remove(CB_TradeAgreementHistory entity)
        {
            context.CB_TradeAgreementHistories.Attach(entity);
            context.CB_TradeAgreementHistories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_TradeAgreementHistory entity)
        {
            onUpdate();
            context.CB_TradeAgreementHistories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_TradeAgreementHistory> All()
        {
            return context.CB_TradeAgreementHistories.ToList();
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
	 