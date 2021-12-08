 
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
   public partial class TradeAgreementProtocolRepository:IRepository<TradeAgreementProtocol>
   {
   
        private ICustomContext currentContext;
        public TradeAgreementProtocolRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TradeAgreementProtocolRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TradeAgreementProtocol GetSingle(string code)
        {
            return (from a in context.TradeAgreementProtocols
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TradeAgreementProtocol> GetAll()
        {
            return from a in context.TradeAgreementProtocols  
                   select a;
        }
				 
        public TradeAgreementProtocol GetSingle(EntityKeyFields entityKeys)
        {
            TradeAgreementProtocolKeys keys = entityKeys as TradeAgreementProtocolKeys;
            return (from a in context.TradeAgreementProtocols
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TradeAgreementProtocol entity)
        {
            onAdd();
            context.TradeAgreementProtocols.Add(entity);
        }

        public void Remove(TradeAgreementProtocol entity)
        {
            context.TradeAgreementProtocols.Attach(entity);
            context.TradeAgreementProtocols.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TradeAgreementProtocol entity)
        {
            onUpdate();
            context.TradeAgreementProtocols.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TradeAgreementProtocol> All()
        {
            return context.TradeAgreementProtocols.ToList();
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
	 