 
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
   public partial class AmedmentTypeQueryService: EntityQueryService<AmedmentType,AmedmentTypeKeys,AmedmentTypePM,object,AmedmentTypeKeys>
   {
   
        AmedmentTypeRepository repository;
		ICustomContext  context;
        public AmedmentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AmedmentTypeRepository(context);
            Repository = repository;
            mapping = new AmedmentTypeDataMapping();
        }

        public AmedmentTypeQueryService(AmedmentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AmedmentTypeDataMapping();
        }

        public AmedmentTypeQueryService(ICustomContext context)
        {
            this.repository = new AmedmentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AmedmentTypeDataMapping();
        }
		 
		public  AmedmentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AmedmentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AmedmentType entityPOCO)
        {
            AmedmentTypeKeys entityKeys = new AmedmentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 