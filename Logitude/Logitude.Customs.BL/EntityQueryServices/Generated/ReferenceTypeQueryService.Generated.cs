 
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
   public partial class ReferenceTypeQueryService: EntityQueryService<ReferenceType,ReferenceTypeKeys,ReferenceTypePM,object,ReferenceTypeKeys>
   {
   
        ReferenceTypeRepository repository;
		ICustomContext  context;
        public ReferenceTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ReferenceTypeRepository(context);
            Repository = repository;
            mapping = new ReferenceTypeDataMapping();
        }

        public ReferenceTypeQueryService(ReferenceTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReferenceTypeDataMapping();
        }

        public ReferenceTypeQueryService(ICustomContext context)
        {
            this.repository = new ReferenceTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReferenceTypeDataMapping();
        }
		 
		public  ReferenceTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReferenceTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReferenceType entityPOCO)
        {
            ReferenceTypeKeys entityKeys = new ReferenceTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 