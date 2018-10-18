 
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
   public partial class StorageMessageTypeQueryService: EntityQueryService<StorageMessageType,StorageMessageTypeKeys,StorageMessageTypePM,object,StorageMessageTypeKeys>
   {
   
        StorageMessageTypeRepository repository;
		ICustomContext  context;
        public StorageMessageTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new StorageMessageTypeRepository(context);
            Repository = repository;
            mapping = new StorageMessageTypeDataMapping();
        }

        public StorageMessageTypeQueryService(StorageMessageTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new StorageMessageTypeDataMapping();
        }

        public StorageMessageTypeQueryService(ICustomContext context)
        {
            this.repository = new StorageMessageTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new StorageMessageTypeDataMapping();
        }
		 
		public  StorageMessageTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new StorageMessageTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(StorageMessageType entityPOCO)
        {
            StorageMessageTypeKeys entityKeys = new StorageMessageTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 