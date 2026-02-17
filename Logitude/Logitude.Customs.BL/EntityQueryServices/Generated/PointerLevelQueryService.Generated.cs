 
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
   public partial class PointerLevelQueryService: EntityQueryService<PointerLevel,PointerLevelKeys,PointerLevelPM,object,PointerLevelKeys>
   {
   
        PointerLevelRepository repository;
		ICustomContext  context;
        public PointerLevelQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PointerLevelRepository(context);
            Repository = repository;
            mapping = new PointerLevelDataMapping();
        }

        public PointerLevelQueryService(PointerLevelRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PointerLevelDataMapping();
        }

        public PointerLevelQueryService(ICustomContext context)
        {
            this.repository = new PointerLevelRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PointerLevelDataMapping();
        }
		 
		public  PointerLevelPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PointerLevelKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PointerLevel entityPOCO)
        {
            PointerLevelKeys entityKeys = new PointerLevelKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 