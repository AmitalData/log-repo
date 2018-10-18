 
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
   public partial class RequestStatusQueryService: EntityQueryService<RequestStatus,RequestStatusKeys,RequestStatusPM,object,RequestStatusKeys>
   {
   
        RequestStatusRepository repository;
		ICustomContext  context;
        public RequestStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RequestStatusRepository(context);
            Repository = repository;
            mapping = new RequestStatusDataMapping();
        }

        public RequestStatusQueryService(RequestStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RequestStatusDataMapping();
        }

        public RequestStatusQueryService(ICustomContext context)
        {
            this.repository = new RequestStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RequestStatusDataMapping();
        }
		 
		public  RequestStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RequestStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RequestStatus entityPOCO)
        {
            RequestStatusKeys entityKeys = new RequestStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 