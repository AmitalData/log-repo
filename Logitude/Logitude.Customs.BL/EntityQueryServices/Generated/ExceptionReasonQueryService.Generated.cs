 
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
   public partial class ExceptionReasonQueryService: EntityQueryService<ExceptionReason,ExceptionReasonKeys,ExceptionReasonPM,object,ExceptionReasonKeys>
   {
   
        ExceptionReasonRepository repository;
		ICustomContext  context;
        public ExceptionReasonQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExceptionReasonRepository(context);
            Repository = repository;
            mapping = new ExceptionReasonDataMapping();
        }

        public ExceptionReasonQueryService(ExceptionReasonRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExceptionReasonDataMapping();
        }

        public ExceptionReasonQueryService(ICustomContext context)
        {
            this.repository = new ExceptionReasonRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExceptionReasonDataMapping();
        }
		 
		public  ExceptionReasonPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExceptionReasonKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExceptionReason entityPOCO)
        {
            ExceptionReasonKeys entityKeys = new ExceptionReasonKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 