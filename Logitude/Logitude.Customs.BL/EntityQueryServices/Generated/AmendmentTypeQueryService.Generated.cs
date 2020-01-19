 
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
   public partial class AmendmentTypeQueryService: EntityQueryService<AmendmentType,AmendmentTypeKeys,AmendmentTypePM,object,AmendmentTypeKeys>
   {
   
        AmendmentTypeRepository repository;
		ICustomContext  context;
        public AmendmentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AmendmentTypeRepository(context);
            Repository = repository;
            mapping = new AmendmentTypeDataMapping();
        }

        public AmendmentTypeQueryService(AmendmentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AmendmentTypeDataMapping();
        }

        public AmendmentTypeQueryService(ICustomContext context)
        {
            this.repository = new AmendmentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AmendmentTypeDataMapping();
        }
		 
		public  AmendmentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AmendmentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AmendmentType entityPOCO)
        {
            AmendmentTypeKeys entityKeys = new AmendmentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 