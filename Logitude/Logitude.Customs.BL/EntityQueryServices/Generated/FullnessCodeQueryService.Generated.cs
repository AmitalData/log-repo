 
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
   public partial class FullnessCodeQueryService: EntityQueryService<FullnessCode,FullnessCodeKeys,FullnessCodePM,object,FullnessCodeKeys>
   {
   
        FullnessCodeRepository repository;
		ICustomContext  context;
        public FullnessCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new FullnessCodeRepository(context);
            Repository = repository;
            mapping = new FullnessCodeDataMapping();
        }

        public FullnessCodeQueryService(FullnessCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FullnessCodeDataMapping();
        }

        public FullnessCodeQueryService(ICustomContext context)
        {
            this.repository = new FullnessCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FullnessCodeDataMapping();
        }
		 
		public  FullnessCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FullnessCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FullnessCode entityPOCO)
        {
            FullnessCodeKeys entityKeys = new FullnessCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 