 
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
   public partial class ExportStorgeCargoQueryService: EntityQueryService<ExportStorgeCargo,ExportStorgeCargoKeys,ExportStorgeCargoPM,object,ExportStorgeCargoKeys>
   {
   
        ExportStorgeCargoRepository repository;
		ICustomContext  context;
        public ExportStorgeCargoQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExportStorgeCargoRepository(context);
            Repository = repository;
            mapping = new ExportStorgeCargoDataMapping();
        }

        public ExportStorgeCargoQueryService(ExportStorgeCargoRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExportStorgeCargoDataMapping();
        }

        public ExportStorgeCargoQueryService(ICustomContext context)
        {
            this.repository = new ExportStorgeCargoRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExportStorgeCargoDataMapping();
        }
		 
		public  ExportStorgeCargoPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExportStorgeCargoKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExportStorgeCargo entityPOCO)
        {
            ExportStorgeCargoKeys entityKeys = new ExportStorgeCargoKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 