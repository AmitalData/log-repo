 
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
   public partial class CustomerTypeGeneralQueryService: EntityQueryService<CustomerTypeGeneral,CustomerTypeGeneralKeys,CustomerTypeGeneralPM,object,CustomerTypeGeneralKeys>
   {
   
        CustomerTypeGeneralRepository repository;
		ICustomContext  context;
        public CustomerTypeGeneralQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomerTypeGeneralRepository(context);
            Repository = repository;
            mapping = new CustomerTypeGeneralDataMapping();
        }

        public CustomerTypeGeneralQueryService(CustomerTypeGeneralRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomerTypeGeneralDataMapping();
        }

        public CustomerTypeGeneralQueryService(ICustomContext context)
        {
            this.repository = new CustomerTypeGeneralRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomerTypeGeneralDataMapping();
        }
		 
		public  CustomerTypeGeneralPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomerTypeGeneralKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomerTypeGeneral entityPOCO)
        {
            CustomerTypeGeneralKeys entityKeys = new CustomerTypeGeneralKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 