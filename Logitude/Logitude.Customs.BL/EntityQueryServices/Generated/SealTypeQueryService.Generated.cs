 
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
   public partial class SealTypeQueryService: EntityQueryService<SealType,SealTypeKeys,SealTypePM,object,SealTypeKeys>
   {
   
        SealTypeRepository repository;
		ICustomContext  context;
        public SealTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SealTypeRepository(context);
            Repository = repository;
            mapping = new SealTypeDataMapping();
        }

        public SealTypeQueryService(SealTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SealTypeDataMapping();
        }

        public SealTypeQueryService(ICustomContext context)
        {
            this.repository = new SealTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SealTypeDataMapping();
        }
		 
		public  SealTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SealTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SealType entityPOCO)
        {
            SealTypeKeys entityKeys = new SealTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 