 
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
   public partial class PhysicalCheckSearchResultTypeQueryService: EntityQueryService<PhysicalCheckSearchResultType,PhysicalCheckSearchResultTypeKeys,PhysicalCheckSearchResultTypePM,object,PhysicalCheckSearchResultTypeKeys>
   {
   
        PhysicalCheckSearchResultTypeRepository repository;
		ICustomContext  context;
        public PhysicalCheckSearchResultTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PhysicalCheckSearchResultTypeRepository(context);
            Repository = repository;
            mapping = new PhysicalCheckSearchResultTypeDataMapping();
        }

        public PhysicalCheckSearchResultTypeQueryService(PhysicalCheckSearchResultTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PhysicalCheckSearchResultTypeDataMapping();
        }

        public PhysicalCheckSearchResultTypeQueryService(ICustomContext context)
        {
            this.repository = new PhysicalCheckSearchResultTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PhysicalCheckSearchResultTypeDataMapping();
        }
		 
		public  PhysicalCheckSearchResultTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PhysicalCheckSearchResultTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PhysicalCheckSearchResultType entityPOCO)
        {
            PhysicalCheckSearchResultTypeKeys entityKeys = new PhysicalCheckSearchResultTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 