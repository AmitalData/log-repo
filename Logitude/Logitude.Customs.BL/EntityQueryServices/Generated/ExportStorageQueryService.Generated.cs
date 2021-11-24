 
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
   public partial class ExportStorageQueryService: EntityQueryService<ExportStorage,ExportStorageKeys,ExportStoragePM,object,ExportStorageKeys>
   {
   
        ExportStorageRepository repository;
		ICustomContext  context;
        public ExportStorageQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExportStorageRepository(context);
            Repository = repository;
            mapping = new ExportStorageDataMapping();
        }

        public ExportStorageQueryService(ExportStorageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExportStorageDataMapping();
        }

        public ExportStorageQueryService(ICustomContext context)
        {
            this.repository = new ExportStorageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExportStorageDataMapping();
        }
		 
		public  ExportStoragePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExportStorageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExportStorage entityPOCO)
        {
            ExportStorageKeys entityKeys = new ExportStorageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 