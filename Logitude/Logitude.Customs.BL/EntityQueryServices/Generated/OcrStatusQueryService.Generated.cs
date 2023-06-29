 
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
   public partial class OcrStatusQueryService: EntityQueryService<OcrStatus,OcrStatusKeys,OcrStatusPM,object,OcrStatusKeys>
   {
   
        OcrStatusRepository repository;
		ICustomContext  context;
        public OcrStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new OcrStatusRepository(context);
            Repository = repository;
            mapping = new OcrStatusDataMapping();
        }

        public OcrStatusQueryService(OcrStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OcrStatusDataMapping();
        }

        public OcrStatusQueryService(ICustomContext context)
        {
            this.repository = new OcrStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OcrStatusDataMapping();
        }
		 
		public  OcrStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OcrStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OcrStatus entityPOCO)
        {
            OcrStatusKeys entityKeys = new OcrStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 