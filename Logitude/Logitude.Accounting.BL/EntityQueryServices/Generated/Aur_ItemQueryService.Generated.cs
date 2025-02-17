 
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
   public partial class Aur_ItemQueryService: EntityQueryService<Aur_Item,Aur_ItemKeys,Aur_ItemPM,Aur_PaymentPM,Aur_PaymentKeys>
   {
   
        Aur_ItemRepository repository;
		IAccountingContext  context;
        public Aur_ItemQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Aur_ItemRepository(context);
            Repository = repository;
            mapping = new Aur_ItemDataMapping();
        }

        public Aur_ItemQueryService(Aur_ItemRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Aur_ItemDataMapping();
        }

        public Aur_ItemQueryService(IAccountingContext context)
        {
            this.repository = new Aur_ItemRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Aur_ItemDataMapping();
        }
		 
		public  Aur_ItemPM GetSingle(string paymentid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Aur_ItemKeys(){ PaymentId = paymentid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Aur_Item entityPOCO)
        {
            Aur_ItemKeys entityKeys = new Aur_ItemKeys() { PaymentId = entityPOCO.PaymentId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 