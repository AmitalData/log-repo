 
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
   public partial class RansomViolationTypeQueryService: EntityQueryService<RansomViolationType,RansomViolationTypeKeys,RansomViolationTypePM,object,RansomViolationTypeKeys>
   {
   
        RansomViolationTypeRepository repository;
		ICustomContext  context;
        public RansomViolationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RansomViolationTypeRepository(context);
            Repository = repository;
            mapping = new RansomViolationTypeDataMapping();
        }

        public RansomViolationTypeQueryService(RansomViolationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RansomViolationTypeDataMapping();
        }

        public RansomViolationTypeQueryService(ICustomContext context)
        {
            this.repository = new RansomViolationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RansomViolationTypeDataMapping();
        }
		 
		public  RansomViolationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RansomViolationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RansomViolationType entityPOCO)
        {
            RansomViolationTypeKeys entityKeys = new RansomViolationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 