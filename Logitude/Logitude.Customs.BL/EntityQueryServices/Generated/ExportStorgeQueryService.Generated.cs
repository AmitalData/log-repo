 
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
   public partial class ExportStorgeQueryService: EntityQueryService<ExportStorge,ExportStorgeKeys,ExportStorgePM,object,ExportStorgeKeys>
   {
   
        ExportStorgeRepository repository;
		ICustomContext  context;
        public ExportStorgeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExportStorgeRepository(context);
            Repository = repository;
            mapping = new ExportStorgeDataMapping();
        }

        public ExportStorgeQueryService(ExportStorgeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExportStorgeDataMapping();
        }

        public ExportStorgeQueryService(ICustomContext context)
        {
            this.repository = new ExportStorgeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExportStorgeDataMapping();
        }
		 
		public  ExportStorgePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExportStorgeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExportStorge entityPOCO)
        {
            ExportStorgeKeys entityKeys = new ExportStorgeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 