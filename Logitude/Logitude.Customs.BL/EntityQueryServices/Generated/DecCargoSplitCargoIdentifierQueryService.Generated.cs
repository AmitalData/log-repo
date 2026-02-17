 
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
   public partial class DecCargoSplitCargoIdentifierQueryService: EntityQueryService<DecCargoSplitCargoIdentifier,DecCargoSplitCargoIdentifierKeys,DecCargoSplitCargoIdentifierPM,DeclarationCargoSplitPM,DeclarationCargoSplitKeys>
   {
   
        DecCargoSplitCargoIdentifierRepository repository;
		ICustomContext  context;
        public DecCargoSplitCargoIdentifierQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DecCargoSplitCargoIdentifierRepository(context);
            Repository = repository;
            mapping = new DecCargoSplitCargoIdentifierDataMapping();
        }

        public DecCargoSplitCargoIdentifierQueryService(DecCargoSplitCargoIdentifierRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DecCargoSplitCargoIdentifierDataMapping();
        }

        public DecCargoSplitCargoIdentifierQueryService(ICustomContext context)
        {
            this.repository = new DecCargoSplitCargoIdentifierRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DecCargoSplitCargoIdentifierDataMapping();
        }
		 
		public  DecCargoSplitCargoIdentifierPM GetSingle(string declarationcargosplitid, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DecCargoSplitCargoIdentifierKeys(){ DeclarationCargoSplitId = declarationcargosplitid, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DecCargoSplitCargoIdentifier entityPOCO)
        {
            DecCargoSplitCargoIdentifierKeys entityKeys = new DecCargoSplitCargoIdentifierKeys() { DeclarationCargoSplitId = entityPOCO.DeclarationCargoSplitId, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 