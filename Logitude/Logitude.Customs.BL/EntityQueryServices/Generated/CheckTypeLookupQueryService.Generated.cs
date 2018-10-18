 
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
   public partial class CheckTypeLookupQueryService: EntityQueryService<CheckTypeLookup,CheckTypeLookupKeys,CheckTypeLookupPM,object,CheckTypeLookupKeys>
   {
   
        CheckTypeLookupRepository repository;
		ICustomContext  context;
        public CheckTypeLookupQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CheckTypeLookupRepository(context);
            Repository = repository;
            mapping = new CheckTypeLookupDataMapping();
        }

        public CheckTypeLookupQueryService(CheckTypeLookupRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CheckTypeLookupDataMapping();
        }

        public CheckTypeLookupQueryService(ICustomContext context)
        {
            this.repository = new CheckTypeLookupRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CheckTypeLookupDataMapping();
        }
		 
		public  CheckTypeLookupPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CheckTypeLookupKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CheckTypeLookup entityPOCO)
        {
            CheckTypeLookupKeys entityKeys = new CheckTypeLookupKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 