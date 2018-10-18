 
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
   public partial class StorageStatusQueryService: EntityQueryService<StorageStatus,StorageStatusKeys,StorageStatusPM,object,StorageStatusKeys>
   {
   
        StorageStatusRepository repository;
		ICustomContext  context;
        public StorageStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new StorageStatusRepository(context);
            Repository = repository;
            mapping = new StorageStatusDataMapping();
        }

        public StorageStatusQueryService(StorageStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new StorageStatusDataMapping();
        }

        public StorageStatusQueryService(ICustomContext context)
        {
            this.repository = new StorageStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new StorageStatusDataMapping();
        }
		 
		public  StorageStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new StorageStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(StorageStatus entityPOCO)
        {
            StorageStatusKeys entityKeys = new StorageStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 