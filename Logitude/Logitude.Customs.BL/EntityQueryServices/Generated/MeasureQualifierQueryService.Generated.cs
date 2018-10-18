 
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
   public partial class MeasureQualifierQueryService: EntityQueryService<MeasureQualifier,MeasureQualifierKeys,MeasureQualifierPM,object,MeasureQualifierKeys>
   {
   
        MeasureQualifierRepository repository;
		ICustomContext  context;
        public MeasureQualifierQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new MeasureQualifierRepository(context);
            Repository = repository;
            mapping = new MeasureQualifierDataMapping();
        }

        public MeasureQualifierQueryService(MeasureQualifierRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MeasureQualifierDataMapping();
        }

        public MeasureQualifierQueryService(ICustomContext context)
        {
            this.repository = new MeasureQualifierRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MeasureQualifierDataMapping();
        }
		 
		public  MeasureQualifierPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MeasureQualifierKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MeasureQualifier entityPOCO)
        {
            MeasureQualifierKeys entityKeys = new MeasureQualifierKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 