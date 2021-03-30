 
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
   public partial class NDMessageActionCodeQueryService: EntityQueryService<NDMessageActionCode,NDMessageActionCodeKeys,NDMessageActionCodePM,object,NDMessageActionCodeKeys>
   {
   
        NDMessageActionCodeRepository repository;
		ICustomContext  context;
        public NDMessageActionCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NDMessageActionCodeRepository(context);
            Repository = repository;
            mapping = new NDMessageActionCodeDataMapping();
        }

        public NDMessageActionCodeQueryService(NDMessageActionCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NDMessageActionCodeDataMapping();
        }

        public NDMessageActionCodeQueryService(ICustomContext context)
        {
            this.repository = new NDMessageActionCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NDMessageActionCodeDataMapping();
        }
		 
		public  NDMessageActionCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NDMessageActionCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NDMessageActionCode entityPOCO)
        {
            NDMessageActionCodeKeys entityKeys = new NDMessageActionCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 