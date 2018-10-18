 
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
   public partial class GuaranteeCustomerActivityQueryService: EntityQueryService<GuaranteeCustomerActivity,GuaranteeCustomerActivityKeys,GuaranteeCustomerActivityPM,object,GuaranteeCustomerActivityKeys>
   {
   
        GuaranteeCustomerActivityRepository repository;
		ICustomContext  context;
        public GuaranteeCustomerActivityQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new GuaranteeCustomerActivityRepository(context);
            Repository = repository;
            mapping = new GuaranteeCustomerActivityDataMapping();
        }

        public GuaranteeCustomerActivityQueryService(GuaranteeCustomerActivityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GuaranteeCustomerActivityDataMapping();
        }

        public GuaranteeCustomerActivityQueryService(ICustomContext context)
        {
            this.repository = new GuaranteeCustomerActivityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GuaranteeCustomerActivityDataMapping();
        }
		 
		public  GuaranteeCustomerActivityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GuaranteeCustomerActivityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GuaranteeCustomerActivity entityPOCO)
        {
            GuaranteeCustomerActivityKeys entityKeys = new GuaranteeCustomerActivityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 