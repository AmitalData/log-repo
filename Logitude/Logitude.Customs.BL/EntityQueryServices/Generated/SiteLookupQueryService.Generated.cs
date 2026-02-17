 
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
   public partial class SiteLookupQueryService: EntityQueryService<SiteLookup,SiteLookupKeys,SiteLookupPM,object,SiteLookupKeys>
   {
   
        SiteLookupRepository repository;
		ICustomContext  context;
        public SiteLookupQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SiteLookupRepository(context);
            Repository = repository;
            mapping = new SiteLookupDataMapping();
        }

        public SiteLookupQueryService(SiteLookupRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SiteLookupDataMapping();
        }

        public SiteLookupQueryService(ICustomContext context)
        {
            this.repository = new SiteLookupRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SiteLookupDataMapping();
        }
		 
		public  SiteLookupPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SiteLookupKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SiteLookup entityPOCO)
        {
            SiteLookupKeys entityKeys = new SiteLookupKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 