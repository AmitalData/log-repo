 
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
   public partial class AmendmentFieldReasonTypeQueryService: EntityQueryService<AmendmentFieldReasonType,AmendmentFieldReasonTypeKeys,AmendmentFieldReasonTypePM,object,AmendmentFieldReasonTypeKeys>
   {
   
        AmendmentFieldReasonTypeRepository repository;
		ICustomContext  context;
        public AmendmentFieldReasonTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AmendmentFieldReasonTypeRepository(context);
            Repository = repository;
            mapping = new AmendmentFieldReasonTypeDataMapping();
        }

        public AmendmentFieldReasonTypeQueryService(AmendmentFieldReasonTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AmendmentFieldReasonTypeDataMapping();
        }

        public AmendmentFieldReasonTypeQueryService(ICustomContext context)
        {
            this.repository = new AmendmentFieldReasonTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AmendmentFieldReasonTypeDataMapping();
        }
		 
		public  AmendmentFieldReasonTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AmendmentFieldReasonTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AmendmentFieldReasonType entityPOCO)
        {
            AmendmentFieldReasonTypeKeys entityKeys = new AmendmentFieldReasonTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 