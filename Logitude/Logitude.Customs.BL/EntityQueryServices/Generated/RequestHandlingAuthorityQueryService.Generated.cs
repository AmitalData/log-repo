 
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
   public partial class RequestHandlingAuthorityQueryService: EntityQueryService<RequestHandlingAuthority,RequestHandlingAuthorityKeys,RequestHandlingAuthorityPM,object,RequestHandlingAuthorityKeys>
   {
   
        RequestHandlingAuthorityRepository repository;
		ICustomContext  context;
        public RequestHandlingAuthorityQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RequestHandlingAuthorityRepository(context);
            Repository = repository;
            mapping = new RequestHandlingAuthorityDataMapping();
        }

        public RequestHandlingAuthorityQueryService(RequestHandlingAuthorityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RequestHandlingAuthorityDataMapping();
        }

        public RequestHandlingAuthorityQueryService(ICustomContext context)
        {
            this.repository = new RequestHandlingAuthorityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RequestHandlingAuthorityDataMapping();
        }
		 
		public  RequestHandlingAuthorityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RequestHandlingAuthorityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RequestHandlingAuthority entityPOCO)
        {
            RequestHandlingAuthorityKeys entityKeys = new RequestHandlingAuthorityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 