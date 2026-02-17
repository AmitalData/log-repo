 
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
   public partial class DeclarationCourierStatusQueryService: EntityQueryService<DeclarationCourierStatus,DeclarationCourierStatusKeys,DeclarationCourierStatusPM,object,DeclarationCourierStatusKeys>
   {
   
        DeclarationCourierStatusRepository repository;
		ICustomContext  context;
        public DeclarationCourierStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationCourierStatusRepository(context);
            Repository = repository;
            mapping = new DeclarationCourierStatusDataMapping();
        }

        public DeclarationCourierStatusQueryService(DeclarationCourierStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationCourierStatusDataMapping();
        }

        public DeclarationCourierStatusQueryService(ICustomContext context)
        {
            this.repository = new DeclarationCourierStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationCourierStatusDataMapping();
        }
		 
		public  DeclarationCourierStatusPM GetSingle(string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationCourierStatusKeys(){ DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationCourierStatus entityPOCO)
        {
            DeclarationCourierStatusKeys entityKeys = new DeclarationCourierStatusKeys() { DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 