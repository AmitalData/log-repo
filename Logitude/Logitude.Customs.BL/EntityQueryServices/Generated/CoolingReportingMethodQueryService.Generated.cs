 
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
   public partial class CoolingReportingMethodQueryService: EntityQueryService<CoolingReportingMethod,CoolingReportingMethodKeys,CoolingReportingMethodPM,object,CoolingReportingMethodKeys>
   {
   
        CoolingReportingMethodRepository repository;
		ICustomContext  context;
        public CoolingReportingMethodQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CoolingReportingMethodRepository(context);
            Repository = repository;
            mapping = new CoolingReportingMethodDataMapping();
        }

        public CoolingReportingMethodQueryService(CoolingReportingMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CoolingReportingMethodDataMapping();
        }

        public CoolingReportingMethodQueryService(ICustomContext context)
        {
            this.repository = new CoolingReportingMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CoolingReportingMethodDataMapping();
        }
		 
		public  CoolingReportingMethodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CoolingReportingMethodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CoolingReportingMethod entityPOCO)
        {
            CoolingReportingMethodKeys entityKeys = new CoolingReportingMethodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 