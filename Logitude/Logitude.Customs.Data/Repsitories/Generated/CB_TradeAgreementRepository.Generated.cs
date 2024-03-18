 
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
   public partial class CB_TradeAgreementRepository:IRepository<CB_TradeAgreement>
   {
   
        private ICustomContext currentContext;
        public CB_TradeAgreementRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_TradeAgreementRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_TradeAgreement GetSingle(string id)
        {
            return (from a in context.CB_TradeAgreements
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_TradeAgreement> GetAll()
        {
            return from a in context.CB_TradeAgreements  
                   select a;
        }
				 
        public CB_TradeAgreement GetSingle(EntityKeyFields entityKeys)
        {
            CB_TradeAgreementKeys keys = entityKeys as CB_TradeAgreementKeys;
            return (from a in context.CB_TradeAgreements
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_TradeAgreement entity)
        {
            onAdd();
            context.CB_TradeAgreements.Add(entity);
        }

        public void Remove(CB_TradeAgreement entity)
        {
            context.CB_TradeAgreements.Attach(entity);
            context.CB_TradeAgreements.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_TradeAgreement entity)
        {
            onUpdate();
            context.CB_TradeAgreements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_TradeAgreement> All()
        {
            return context.CB_TradeAgreements.ToList();
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
	 