 
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
   public partial class RequestTypeQueryService: EntityQueryService<RequestType,RequestTypeKeys,RequestTypePM,object,RequestTypeKeys>
   {
   
        RequestTypeRepository repository;
		ICustomContext  context;
        public RequestTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RequestTypeRepository(context);
            Repository = repository;
            mapping = new RequestTypeDataMapping();
        }

        public RequestTypeQueryService(RequestTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RequestTypeDataMapping();
        }

        public RequestTypeQueryService(ICustomContext context)
        {
            this.repository = new RequestTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RequestTypeDataMapping();
        }
		 
		public  RequestTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RequestTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RequestType entityPOCO)
        {
            RequestTypeKeys entityKeys = new RequestTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 