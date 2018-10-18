 
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
   public partial class CollateralTypeQueryService: EntityQueryService<CollateralType,CollateralTypeKeys,CollateralTypePM,object,CollateralTypeKeys>
   {
   
        CollateralTypeRepository repository;
		ICustomContext  context;
        public CollateralTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CollateralTypeRepository(context);
            Repository = repository;
            mapping = new CollateralTypeDataMapping();
        }

        public CollateralTypeQueryService(CollateralTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CollateralTypeDataMapping();
        }

        public CollateralTypeQueryService(ICustomContext context)
        {
            this.repository = new CollateralTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CollateralTypeDataMapping();
        }
		 
		public  CollateralTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CollateralTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CollateralType entityPOCO)
        {
            CollateralTypeKeys entityKeys = new CollateralTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 