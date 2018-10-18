 
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
   public partial class FreightPaymentMethodQueryService: EntityQueryService<FreightPaymentMethod,FreightPaymentMethodKeys,FreightPaymentMethodPM,object,FreightPaymentMethodKeys>
   {
   
        FreightPaymentMethodRepository repository;
		ICustomContext  context;
        public FreightPaymentMethodQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new FreightPaymentMethodRepository(context);
            Repository = repository;
            mapping = new FreightPaymentMethodDataMapping();
        }

        public FreightPaymentMethodQueryService(FreightPaymentMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FreightPaymentMethodDataMapping();
        }

        public FreightPaymentMethodQueryService(ICustomContext context)
        {
            this.repository = new FreightPaymentMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FreightPaymentMethodDataMapping();
        }
		 
		public  FreightPaymentMethodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FreightPaymentMethodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FreightPaymentMethod entityPOCO)
        {
            FreightPaymentMethodKeys entityKeys = new FreightPaymentMethodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 