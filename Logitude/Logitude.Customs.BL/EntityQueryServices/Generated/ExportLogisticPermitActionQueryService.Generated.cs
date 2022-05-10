 
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
   public partial class ExportLogisticPermitActionQueryService: EntityQueryService<ExportLogisticPermitAction,ExportLogisticPermitActionKeys,ExportLogisticPermitActionPM,object,ExportLogisticPermitActionKeys>
   {
   
        ExportLogisticPermitActionRepository repository;
		ICustomContext  context;
        public ExportLogisticPermitActionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExportLogisticPermitActionRepository(context);
            Repository = repository;
            mapping = new ExportLogisticPermitActionDataMapping();
        }

        public ExportLogisticPermitActionQueryService(ExportLogisticPermitActionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExportLogisticPermitActionDataMapping();
        }

        public ExportLogisticPermitActionQueryService(ICustomContext context)
        {
            this.repository = new ExportLogisticPermitActionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExportLogisticPermitActionDataMapping();
        }
		 
		public  ExportLogisticPermitActionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExportLogisticPermitActionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExportLogisticPermitAction entityPOCO)
        {
            ExportLogisticPermitActionKeys entityKeys = new ExportLogisticPermitActionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 