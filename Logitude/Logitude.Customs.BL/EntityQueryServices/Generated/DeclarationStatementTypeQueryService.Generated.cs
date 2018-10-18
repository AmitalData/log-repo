 
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
   public partial class DeclarationStatementTypeQueryService: EntityQueryService<DeclarationStatementType,DeclarationStatementTypeKeys,DeclarationStatementTypePM,object,DeclarationStatementTypeKeys>
   {
   
        DeclarationStatementTypeRepository repository;
		ICustomContext  context;
        public DeclarationStatementTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationStatementTypeRepository(context);
            Repository = repository;
            mapping = new DeclarationStatementTypeDataMapping();
        }

        public DeclarationStatementTypeQueryService(DeclarationStatementTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationStatementTypeDataMapping();
        }

        public DeclarationStatementTypeQueryService(ICustomContext context)
        {
            this.repository = new DeclarationStatementTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationStatementTypeDataMapping();
        }
		 
		public  DeclarationStatementTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationStatementTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationStatementType entityPOCO)
        {
            DeclarationStatementTypeKeys entityKeys = new DeclarationStatementTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 