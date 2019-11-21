 
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
   public partial class HazardousSubstanceQueryService: EntityQueryService<HazardousSubstance,HazardousSubstanceKeys,HazardousSubstancePM,object,HazardousSubstanceKeys>
   {
   
        HazardousSubstanceRepository repository;
		ICustomContext  context;
        public HazardousSubstanceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new HazardousSubstanceRepository(context);
            Repository = repository;
            mapping = new HazardousSubstanceDataMapping();
        }

        public HazardousSubstanceQueryService(HazardousSubstanceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new HazardousSubstanceDataMapping();
        }

        public HazardousSubstanceQueryService(ICustomContext context)
        {
            this.repository = new HazardousSubstanceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new HazardousSubstanceDataMapping();
        }
		 
		public  HazardousSubstancePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new HazardousSubstanceKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(HazardousSubstance entityPOCO)
        {
            HazardousSubstanceKeys entityKeys = new HazardousSubstanceKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 