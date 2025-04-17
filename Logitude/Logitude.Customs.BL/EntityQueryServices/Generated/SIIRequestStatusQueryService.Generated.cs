 
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
   public partial class SIIRequestStatusQueryService: EntityQueryService<SIIRequestStatus,SIIRequestStatusKeys,SIIRequestStatusPM,object,SIIRequestStatusKeys>
   {
   
        SIIRequestStatusRepository repository;
		ICustomContext  context;
        public SIIRequestStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SIIRequestStatusRepository(context);
            Repository = repository;
            mapping = new SIIRequestStatusDataMapping();
        }

        public SIIRequestStatusQueryService(SIIRequestStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SIIRequestStatusDataMapping();
        }

        public SIIRequestStatusQueryService(ICustomContext context)
        {
            this.repository = new SIIRequestStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SIIRequestStatusDataMapping();
        }
		 
		public  SIIRequestStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SIIRequestStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SIIRequestStatus entityPOCO)
        {
            SIIRequestStatusKeys entityKeys = new SIIRequestStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 