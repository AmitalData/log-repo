 
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
   public partial class Aur_PaymentQueryService: EntityQueryService<Aur_Payment,Aur_PaymentKeys,Aur_PaymentPM,object,Aur_PaymentKeys>
   {
   
        Aur_PaymentRepository repository;
		IAccountingContext  context;
        public Aur_PaymentQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Aur_PaymentRepository(context);
            Repository = repository;
            mapping = new Aur_PaymentDataMapping();
        }

        public Aur_PaymentQueryService(Aur_PaymentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Aur_PaymentDataMapping();
        }

        public Aur_PaymentQueryService(IAccountingContext context)
        {
            this.repository = new Aur_PaymentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Aur_PaymentDataMapping();
        }
		 
		public  Aur_PaymentPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Aur_PaymentKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Aur_Payment entityPOCO)
        {
            Aur_PaymentKeys entityKeys = new Aur_PaymentKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 