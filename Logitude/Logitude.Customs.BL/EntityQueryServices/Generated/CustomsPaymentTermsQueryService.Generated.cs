 
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
   public partial class CustomsPaymentTermQueryService: EntityQueryService<CustomsPaymentTerm,CustomsPaymentTermKeys,CustomsPaymentTermPM,object,CustomsPaymentTermKeys>
   {
   
        CustomsPaymentTermRepository repository;
		ICustomContext  context;
        public CustomsPaymentTermQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsPaymentTermRepository(context);
            Repository = repository;
            mapping = new CustomsPaymentTermDataMapping();
        }

        public CustomsPaymentTermQueryService(CustomsPaymentTermRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsPaymentTermDataMapping();
        }

        public CustomsPaymentTermQueryService(ICustomContext context)
        {
            this.repository = new CustomsPaymentTermRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsPaymentTermDataMapping();
        }
		 
		public  CustomsPaymentTermPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsPaymentTermKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsPaymentTerm entityPOCO)
        {
            CustomsPaymentTermKeys entityKeys = new CustomsPaymentTermKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 