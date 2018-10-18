 
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
   public partial class DecCargoSplitConsPackDetQueryService: EntityQueryService<DecCargoSplitConsPackDet,DecCargoSplitConsPackDetKeys,DecCargoSplitConsPackDetPM,DecCargoSplitConsItemPM,DecCargoSplitConsItemKeys>
   {
   
        DecCargoSplitConsPackDetRepository repository;
		ICustomContext  context;
        public DecCargoSplitConsPackDetQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DecCargoSplitConsPackDetRepository(context);
            Repository = repository;
            mapping = new DecCargoSplitConsPackDetDataMapping();
        }

        public DecCargoSplitConsPackDetQueryService(DecCargoSplitConsPackDetRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DecCargoSplitConsPackDetDataMapping();
        }

        public DecCargoSplitConsPackDetQueryService(ICustomContext context)
        {
            this.repository = new DecCargoSplitConsPackDetRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DecCargoSplitConsPackDetDataMapping();
        }
		 
		public  DecCargoSplitConsPackDetPM GetSingle(string declarationcargosplitid, int? deccargosplitconslineno, int deccargosplitconsitemline, int packageline,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DecCargoSplitConsPackDetKeys(){ DeclarationCargoSplitId = declarationcargosplitid, DecCargoSplitConsLineNo = deccargosplitconslineno, DecCargoSplitConsItemLine = deccargosplitconsitemline, PackageLine = packageline };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DecCargoSplitConsPackDet entityPOCO)
        {
            DecCargoSplitConsPackDetKeys entityKeys = new DecCargoSplitConsPackDetKeys() { DeclarationCargoSplitId = entityPOCO.DeclarationCargoSplitId, DecCargoSplitConsLineNo = entityPOCO.DecCargoSplitConsLineNo, DecCargoSplitConsItemLine = entityPOCO.DecCargoSplitConsItemLine, PackageLine = entityPOCO.PackageLine,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 