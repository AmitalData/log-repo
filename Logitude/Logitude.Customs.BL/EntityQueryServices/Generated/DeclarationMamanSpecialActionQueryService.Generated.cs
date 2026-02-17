 
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
   public partial class DeclarationMamanSpecialActionQueryService: EntityQueryService<DeclarationMamanSpecialAction,DeclarationMamanSpecialActionKeys,DeclarationMamanSpecialActionPM,object,DeclarationMamanSpecialActionKeys>
   {
   
        DeclarationMamanSpecialActionRepository repository;
		ICustomContext  context;
        public DeclarationMamanSpecialActionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationMamanSpecialActionRepository(context);
            Repository = repository;
            mapping = new DeclarationMamanSpecialActionDataMapping();
        }

        public DeclarationMamanSpecialActionQueryService(DeclarationMamanSpecialActionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationMamanSpecialActionDataMapping();
        }

        public DeclarationMamanSpecialActionQueryService(ICustomContext context)
        {
            this.repository = new DeclarationMamanSpecialActionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationMamanSpecialActionDataMapping();
        }
		 
		public  DeclarationMamanSpecialActionPM GetSingle(string declarationid, string mamanspecialactioncode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationMamanSpecialActionKeys(){ DeclarationId = declarationid, MamanSpecialActionCode = mamanspecialactioncode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationMamanSpecialAction entityPOCO)
        {
            DeclarationMamanSpecialActionKeys entityKeys = new DeclarationMamanSpecialActionKeys() { DeclarationId = entityPOCO.DeclarationId, MamanSpecialActionCode = entityPOCO.MamanSpecialActionCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 