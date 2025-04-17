 
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
   public partial class SIIRequestQueryService: EntityQueryService<SIIRequest,SIIRequestKeys,SIIRequestPM,object,SIIRequestKeys>
   {
   
        SIIRequestRepository repository;
		ICustomContext  context;
        public SIIRequestQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SIIRequestRepository(context);
            Repository = repository;
            mapping = new SIIRequestDataMapping();
        }

        public SIIRequestQueryService(SIIRequestRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SIIRequestDataMapping();
        }

        public SIIRequestQueryService(ICustomContext context)
        {
            this.repository = new SIIRequestRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SIIRequestDataMapping();
        }
		 
		public  SIIRequestPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SIIRequestKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SIIRequest entityPOCO)
        {
            SIIRequestKeys entityKeys = new SIIRequestKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 