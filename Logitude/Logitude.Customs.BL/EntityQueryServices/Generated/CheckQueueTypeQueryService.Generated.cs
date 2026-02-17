 
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
   public partial class CheckQueueTypeQueryService: EntityQueryService<CheckQueueType,CheckQueueTypeKeys,CheckQueueTypePM,object,CheckQueueTypeKeys>
   {
   
        CheckQueueTypeRepository repository;
		ICustomContext  context;
        public CheckQueueTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CheckQueueTypeRepository(context);
            Repository = repository;
            mapping = new CheckQueueTypeDataMapping();
        }

        public CheckQueueTypeQueryService(CheckQueueTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CheckQueueTypeDataMapping();
        }

        public CheckQueueTypeQueryService(ICustomContext context)
        {
            this.repository = new CheckQueueTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CheckQueueTypeDataMapping();
        }
		 
		public  CheckQueueTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CheckQueueTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CheckQueueType entityPOCO)
        {
            CheckQueueTypeKeys entityKeys = new CheckQueueTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 