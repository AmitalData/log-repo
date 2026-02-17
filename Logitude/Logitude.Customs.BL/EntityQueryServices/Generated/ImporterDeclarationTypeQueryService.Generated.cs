 
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
   public partial class ImporterDeclarationTypeQueryService: EntityQueryService<ImporterDeclarationType,ImporterDeclarationTypeKeys,ImporterDeclarationTypePM,object,ImporterDeclarationTypeKeys>
   {
   
        ImporterDeclarationTypeRepository repository;
		ICustomContext  context;
        public ImporterDeclarationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ImporterDeclarationTypeRepository(context);
            Repository = repository;
            mapping = new ImporterDeclarationTypeDataMapping();
        }

        public ImporterDeclarationTypeQueryService(ImporterDeclarationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ImporterDeclarationTypeDataMapping();
        }

        public ImporterDeclarationTypeQueryService(ICustomContext context)
        {
            this.repository = new ImporterDeclarationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ImporterDeclarationTypeDataMapping();
        }
		 
		public  ImporterDeclarationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ImporterDeclarationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ImporterDeclarationType entityPOCO)
        {
            ImporterDeclarationTypeKeys entityKeys = new ImporterDeclarationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 