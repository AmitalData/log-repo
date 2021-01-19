 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityDataMappings;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.TariffModule.BL.EntityQueryServices
{ 
   public partial class TariffVersionUploadedExcelQueryService: EntityQueryService<TariffVersionUploadedExcel,TariffVersionUploadedExcelKeys,TariffVersionUploadedExcelPM,object,TariffVersionUploadedExcelKeys>
   {
   
        TariffVersionUploadedExcelRepository repository;
		ITariffModuleContext  context;
        public TariffVersionUploadedExcelQueryService(int tenant)
        {
		    context = TariffModuleContext.GetContext(tenant);
            MainContext = context;
            repository = new TariffVersionUploadedExcelRepository(context);
            Repository = repository;
            mapping = new TariffVersionUploadedExcelDataMapping();
        }

        public TariffVersionUploadedExcelQueryService(TariffVersionUploadedExcelRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TariffVersionUploadedExcelDataMapping();
        }

        public TariffVersionUploadedExcelQueryService(ITariffModuleContext context)
        {
            this.repository = new TariffVersionUploadedExcelRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TariffVersionUploadedExcelDataMapping();
        }
		 
		public  TariffVersionUploadedExcelPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TariffVersionUploadedExcelKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TariffVersionUploadedExcel entityPOCO)
        {
            TariffVersionUploadedExcelKeys entityKeys = new TariffVersionUploadedExcelKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 