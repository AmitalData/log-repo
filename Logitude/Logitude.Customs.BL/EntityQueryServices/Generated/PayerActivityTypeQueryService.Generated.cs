 
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
   public partial class PayerActivityTypeQueryService: EntityQueryService<PayerActivityType,PayerActivityTypeKeys,PayerActivityTypePM,object,PayerActivityTypeKeys>
   {
   
        PayerActivityTypeRepository repository;
		ICustomContext  context;
        public PayerActivityTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PayerActivityTypeRepository(context);
            Repository = repository;
            mapping = new PayerActivityTypeDataMapping();
        }

        public PayerActivityTypeQueryService(PayerActivityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PayerActivityTypeDataMapping();
        }

        public PayerActivityTypeQueryService(ICustomContext context)
        {
            this.repository = new PayerActivityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PayerActivityTypeDataMapping();
        }
		 
		public  PayerActivityTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PayerActivityTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PayerActivityType entityPOCO)
        {
            PayerActivityTypeKeys entityKeys = new PayerActivityTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 