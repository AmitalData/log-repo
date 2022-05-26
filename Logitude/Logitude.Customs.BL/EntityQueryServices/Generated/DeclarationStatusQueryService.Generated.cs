 
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
   public partial class DeclarationStatusQueryService: EntityQueryService<DeclarationStatus,DeclarationStatusKeys,DeclarationStatusPM,object,DeclarationStatusKeys>
   {
   
        DeclarationStatusRepository repository;
		ICustomContext  context;
        public DeclarationStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationStatusRepository(context);
            Repository = repository;
            mapping = new DeclarationStatusDataMapping();
        }

        public DeclarationStatusQueryService(DeclarationStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationStatusDataMapping();
        }

        public DeclarationStatusQueryService(ICustomContext context)
        {
            this.repository = new DeclarationStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationStatusDataMapping();
        }
		 
		public  DeclarationStatusPM GetSingle(string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationStatusKeys(){ DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationStatus entityPOCO)
        {
            DeclarationStatusKeys entityKeys = new DeclarationStatusKeys() { DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 