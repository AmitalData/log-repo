 
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
   public partial class TradeLevyExamptTypeQueryService: EntityQueryService<TradeLevyExamptType,TradeLevyExamptTypeKeys,TradeLevyExamptTypePM,object,TradeLevyExamptTypeKeys>
   {
   
        TradeLevyExamptTypeRepository repository;
		ICustomContext  context;
        public TradeLevyExamptTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TradeLevyExamptTypeRepository(context);
            Repository = repository;
            mapping = new TradeLevyExamptTypeDataMapping();
        }

        public TradeLevyExamptTypeQueryService(TradeLevyExamptTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TradeLevyExamptTypeDataMapping();
        }

        public TradeLevyExamptTypeQueryService(ICustomContext context)
        {
            this.repository = new TradeLevyExamptTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TradeLevyExamptTypeDataMapping();
        }
		 
		public  TradeLevyExamptTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TradeLevyExamptTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TradeLevyExamptType entityPOCO)
        {
            TradeLevyExamptTypeKeys entityKeys = new TradeLevyExamptTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 