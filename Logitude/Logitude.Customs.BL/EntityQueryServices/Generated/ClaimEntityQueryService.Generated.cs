 
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
   public partial class ClaimEntityQueryService: EntityQueryService<ClaimEntity,ClaimEntityKeys,ClaimEntityPM,object,ClaimEntityKeys>
   {
   
        ClaimEntityRepository repository;
		ICustomContext  context;
        public ClaimEntityQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimEntityRepository(context);
            Repository = repository;
            mapping = new ClaimEntityDataMapping();
        }

        public ClaimEntityQueryService(ClaimEntityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimEntityDataMapping();
        }

        public ClaimEntityQueryService(ICustomContext context)
        {
            this.repository = new ClaimEntityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimEntityDataMapping();
        }
		 
		public  ClaimEntityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimEntityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClaimEntity entityPOCO)
        {
            ClaimEntityKeys entityKeys = new ClaimEntityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 