 
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
   public partial class DeclarationCasualDetailsQueryService: EntityQueryService<DeclarationCasualDetails,DeclarationCasualDetailsKeys,DeclarationCasualDetailsPM,object,DeclarationCasualDetailsKeys>
   {
   
        DeclarationCasualDetailsRepository repository;
		ICustomContext  context;
        public DeclarationCasualDetailsQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationCasualDetailsRepository(context);
            Repository = repository;
            mapping = new DeclarationCasualDetailsDataMapping();
        }

        public DeclarationCasualDetailsQueryService(DeclarationCasualDetailsRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationCasualDetailsDataMapping();
        }

        public DeclarationCasualDetailsQueryService(ICustomContext context)
        {
            this.repository = new DeclarationCasualDetailsRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationCasualDetailsDataMapping();
        }
		 
		public  DeclarationCasualDetailsPM GetSingle(string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationCasualDetailsKeys(){ DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationCasualDetails entityPOCO)
        {
            DeclarationCasualDetailsKeys entityKeys = new DeclarationCasualDetailsKeys() { DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 