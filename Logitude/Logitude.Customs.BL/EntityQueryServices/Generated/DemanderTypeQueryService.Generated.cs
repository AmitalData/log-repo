 
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
   public partial class DemanderTypeQueryService: EntityQueryService<DemanderType,DemanderTypeKeys,DemanderTypePM,object,DemanderTypeKeys>
   {
   
        DemanderTypeRepository repository;
		ICustomContext  context;
        public DemanderTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DemanderTypeRepository(context);
            Repository = repository;
            mapping = new DemanderTypeDataMapping();
        }

        public DemanderTypeQueryService(DemanderTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DemanderTypeDataMapping();
        }

        public DemanderTypeQueryService(ICustomContext context)
        {
            this.repository = new DemanderTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DemanderTypeDataMapping();
        }
		 
		public  DemanderTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DemanderTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DemanderType entityPOCO)
        {
            DemanderTypeKeys entityKeys = new DemanderTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 