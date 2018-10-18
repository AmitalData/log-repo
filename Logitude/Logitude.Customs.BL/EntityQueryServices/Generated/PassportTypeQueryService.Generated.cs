 
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
   public partial class PassportTypeQueryService: EntityQueryService<PassportType,PassportTypeKeys,PassportTypePM,object,PassportTypeKeys>
   {
   
        PassportTypeRepository repository;
		ICustomContext  context;
        public PassportTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PassportTypeRepository(context);
            Repository = repository;
            mapping = new PassportTypeDataMapping();
        }

        public PassportTypeQueryService(PassportTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PassportTypeDataMapping();
        }

        public PassportTypeQueryService(ICustomContext context)
        {
            this.repository = new PassportTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PassportTypeDataMapping();
        }
		 
		public  PassportTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PassportTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PassportType entityPOCO)
        {
            PassportTypeKeys entityKeys = new PassportTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 