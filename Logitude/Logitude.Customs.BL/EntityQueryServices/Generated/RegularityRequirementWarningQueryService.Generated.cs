 
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
   public partial class RegularityRequirementWarningQueryService: EntityQueryService<RegularityRequirementWarning,RegularityRequirementWarningKeys,RegularityRequirementWarningPM,object,RegularityRequirementWarningKeys>
   {
   
        RegularityRequirementWarningRepository repository;
		ICustomContext  context;
        public RegularityRequirementWarningQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RegularityRequirementWarningRepository(context);
            Repository = repository;
            mapping = new RegularityRequirementWarningDataMapping();
        }

        public RegularityRequirementWarningQueryService(RegularityRequirementWarningRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RegularityRequirementWarningDataMapping();
        }

        public RegularityRequirementWarningQueryService(ICustomContext context)
        {
            this.repository = new RegularityRequirementWarningRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RegularityRequirementWarningDataMapping();
        }
		 
		public  RegularityRequirementWarningPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RegularityRequirementWarningKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RegularityRequirementWarning entityPOCO)
        {
            RegularityRequirementWarningKeys entityKeys = new RegularityRequirementWarningKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 