 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class Aur_PaymentItemQueryService: EntityQueryService<Aur_PaymentItem,Aur_PaymentItemKeys,Aur_PaymentItemPM,Aur_PaymentPM,Aur_PaymentKeys>
   {
   
        Aur_PaymentItemRepository repository;
		IAccountingContext  context;
        public Aur_PaymentItemQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Aur_PaymentItemRepository(context);
            Repository = repository;
            mapping = new Aur_PaymentItemDataMapping();
        }

        public Aur_PaymentItemQueryService(Aur_PaymentItemRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Aur_PaymentItemDataMapping();
        }

        public Aur_PaymentItemQueryService(IAccountingContext context)
        {
            this.repository = new Aur_PaymentItemRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Aur_PaymentItemDataMapping();
        }
		 
		public  Aur_PaymentItemPM GetSingle(string paymentid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Aur_PaymentItemKeys(){ PaymentId = paymentid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Aur_PaymentItem entityPOCO)
        {
            Aur_PaymentItemKeys entityKeys = new Aur_PaymentItemKeys() { PaymentId = entityPOCO.PaymentId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 