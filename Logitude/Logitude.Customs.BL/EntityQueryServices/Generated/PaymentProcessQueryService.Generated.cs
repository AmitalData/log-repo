 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class PaymentProcessQueryService: EntityQueryService<PaymentProcess,PaymentProcessKeys,PaymentProcessPM,object,PaymentProcessKeys>
   {
   
        PaymentProcessRepository repository;
		ICustomContext  context;
        public PaymentProcessQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentProcessRepository(context);
            Repository = repository;
            mapping = new PaymentProcessDataMapping();
        }

        public PaymentProcessQueryService(PaymentProcessRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentProcessDataMapping();
        }

        public PaymentProcessQueryService(ICustomContext context)
        {
            this.repository = new PaymentProcessRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentProcessDataMapping();
        }
		 
		public  PaymentProcessPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentProcessKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentProcess entityPOCO)
        {
            PaymentProcessKeys entityKeys = new PaymentProcessKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 