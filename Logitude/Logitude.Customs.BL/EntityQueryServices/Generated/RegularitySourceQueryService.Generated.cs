 
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
   public partial class RegularitySourceQueryService: EntityQueryService<RegularitySource,RegularitySourceKeys,RegularitySourcePM,object,RegularitySourceKeys>
   {
   
        RegularitySourceRepository repository;
		ICustomContext  context;
        public RegularitySourceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RegularitySourceRepository(context);
            Repository = repository;
            mapping = new RegularitySourceDataMapping();
        }

        public RegularitySourceQueryService(RegularitySourceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RegularitySourceDataMapping();
        }

        public RegularitySourceQueryService(ICustomContext context)
        {
            this.repository = new RegularitySourceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RegularitySourceDataMapping();
        }
		 
		public  RegularitySourcePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RegularitySourceKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RegularitySource entityPOCO)
        {
            RegularitySourceKeys entityKeys = new RegularitySourceKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 