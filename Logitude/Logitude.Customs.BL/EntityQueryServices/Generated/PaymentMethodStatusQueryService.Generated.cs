 
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
   public partial class PaymentMethodStatusQueryService: EntityQueryService<PaymentMethodStatus,PaymentMethodStatusKeys,PaymentMethodStatusPM,object,PaymentMethodStatusKeys>
   {
   
        PaymentMethodStatusRepository repository;
		ICustomContext  context;
        public PaymentMethodStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentMethodStatusRepository(context);
            Repository = repository;
            mapping = new PaymentMethodStatusDataMapping();
        }

        public PaymentMethodStatusQueryService(PaymentMethodStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentMethodStatusDataMapping();
        }

        public PaymentMethodStatusQueryService(ICustomContext context)
        {
            this.repository = new PaymentMethodStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentMethodStatusDataMapping();
        }
		 
		public  PaymentMethodStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentMethodStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentMethodStatus entityPOCO)
        {
            PaymentMethodStatusKeys entityKeys = new PaymentMethodStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 