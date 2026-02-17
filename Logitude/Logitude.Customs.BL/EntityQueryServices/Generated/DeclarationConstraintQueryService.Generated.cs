 
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
   public partial class DeclarationConstraintQueryService: EntityQueryService<DeclarationConstraint,DeclarationConstraintKeys,DeclarationConstraintPM,DeclarationPM,DeclarationKeys>
   {
   
        DeclarationConstraintRepository repository;
		ICustomContext  context;
        public DeclarationConstraintQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationConstraintRepository(context);
            Repository = repository;
            mapping = new DeclarationConstraintDataMapping();
        }

        public DeclarationConstraintQueryService(DeclarationConstraintRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationConstraintDataMapping();
        }

        public DeclarationConstraintQueryService(ICustomContext context)
        {
            this.repository = new DeclarationConstraintRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationConstraintDataMapping();
        }
		 
		public  DeclarationConstraintPM GetSingle(string declarationid, string constraintnumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationConstraintKeys(){ DeclarationID = declarationid, ConstraintNumber = constraintnumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationConstraint entityPOCO)
        {
            DeclarationConstraintKeys entityKeys = new DeclarationConstraintKeys() { DeclarationID = entityPOCO.DeclarationID, ConstraintNumber = entityPOCO.ConstraintNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 