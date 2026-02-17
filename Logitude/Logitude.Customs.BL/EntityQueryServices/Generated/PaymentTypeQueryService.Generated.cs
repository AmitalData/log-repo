 
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
   public partial class PaymentTypeQueryService: EntityQueryService<PaymentType,PaymentTypeKeys,PaymentTypePM,object,PaymentTypeKeys>
   {
   
        PaymentTypeRepository repository;
		ICustomContext  context;
        public PaymentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PaymentTypeRepository(context);
            Repository = repository;
            mapping = new PaymentTypeDataMapping();
        }

        public PaymentTypeQueryService(PaymentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PaymentTypeDataMapping();
        }

        public PaymentTypeQueryService(ICustomContext context)
        {
            this.repository = new PaymentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PaymentTypeDataMapping();
        }
		 
		public  PaymentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PaymentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PaymentType entityPOCO)
        {
            PaymentTypeKeys entityKeys = new PaymentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 