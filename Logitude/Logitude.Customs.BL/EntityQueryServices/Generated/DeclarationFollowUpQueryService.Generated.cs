 
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
   public partial class DeclarationFollowUpQueryService: EntityQueryService<DeclarationFollowUp,DeclarationFollowUpKeys,DeclarationFollowUpPM,DeclarationPM,DeclarationKeys>
   {
   
        DeclarationFollowUpRepository repository;
		ICustomContext  context;
        public DeclarationFollowUpQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationFollowUpRepository(context);
            Repository = repository;
            mapping = new DeclarationFollowUpDataMapping();
        }

        public DeclarationFollowUpQueryService(DeclarationFollowUpRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationFollowUpDataMapping();
        }

        public DeclarationFollowUpQueryService(ICustomContext context)
        {
            this.repository = new DeclarationFollowUpRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationFollowUpDataMapping();
        }
		 
		public  DeclarationFollowUpPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationFollowUpKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationFollowUp entityPOCO)
        {
            DeclarationFollowUpKeys entityKeys = new DeclarationFollowUpKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 