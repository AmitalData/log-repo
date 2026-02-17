 
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
   public partial class TradeAgreementRepository:IRepository<TradeAgreement>
   {
   
        private ICustomContext currentContext;
        public TradeAgreementRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TradeAgreementRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TradeAgreement GetSingle(string code)
        {
            return (from a in context.TradeAgreements
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TradeAgreement> GetAll()
        {
            return from a in context.TradeAgreements  
                   select a;
        }
				 
        public TradeAgreement GetSingle(EntityKeyFields entityKeys)
        {
            TradeAgreementKeys keys = entityKeys as TradeAgreementKeys;
            return (from a in context.TradeAgreements
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TradeAgreement entity)
        {
            onAdd();
            context.TradeAgreements.Add(entity);
        }

        public void Remove(TradeAgreement entity)
        {
            context.TradeAgreements.Attach(entity);
            context.TradeAgreements.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TradeAgreement entity)
        {
            onUpdate();
            context.TradeAgreements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TradeAgreement> All()
        {
            return context.TradeAgreements.ToList();
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
	 