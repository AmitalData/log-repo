 
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
   public partial class RequestReasonCodeEnumQueryService: EntityQueryService<RequestReasonCodeEnum,RequestReasonCodeEnumKeys,RequestReasonCodeEnumPM,object,RequestReasonCodeEnumKeys>
   {
   
        RequestReasonCodeEnumRepository repository;
		ICustomContext  context;
        public RequestReasonCodeEnumQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RequestReasonCodeEnumRepository(context);
            Repository = repository;
            mapping = new RequestReasonCodeEnumDataMapping();
        }

        public RequestReasonCodeEnumQueryService(RequestReasonCodeEnumRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RequestReasonCodeEnumDataMapping();
        }

        public RequestReasonCodeEnumQueryService(ICustomContext context)
        {
            this.repository = new RequestReasonCodeEnumRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RequestReasonCodeEnumDataMapping();
        }
		 
		public  RequestReasonCodeEnumPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RequestReasonCodeEnumKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RequestReasonCodeEnum entityPOCO)
        {
            RequestReasonCodeEnumKeys entityKeys = new RequestReasonCodeEnumKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 