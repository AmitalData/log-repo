 
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
   public partial class FclLclCodeQueryService: EntityQueryService<FclLclCode,FclLclCodeKeys,FclLclCodePM,object,FclLclCodeKeys>
   {
   
        FclLclCodeRepository repository;
		ICustomContext  context;
        public FclLclCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new FclLclCodeRepository(context);
            Repository = repository;
            mapping = new FclLclCodeDataMapping();
        }

        public FclLclCodeQueryService(FclLclCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FclLclCodeDataMapping();
        }

        public FclLclCodeQueryService(ICustomContext context)
        {
            this.repository = new FclLclCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FclLclCodeDataMapping();
        }
		 
		public  FclLclCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FclLclCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FclLclCode entityPOCO)
        {
            FclLclCodeKeys entityKeys = new FclLclCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 