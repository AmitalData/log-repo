 
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
   public partial class PerYearFrequencyQueryService: EntityQueryService<PerYearFrequency,PerYearFrequencyKeys,PerYearFrequencyPM,object,PerYearFrequencyKeys>
   {
   
        PerYearFrequencyRepository repository;
		ICustomContext  context;
        public PerYearFrequencyQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PerYearFrequencyRepository(context);
            Repository = repository;
            mapping = new PerYearFrequencyDataMapping();
        }

        public PerYearFrequencyQueryService(PerYearFrequencyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PerYearFrequencyDataMapping();
        }

        public PerYearFrequencyQueryService(ICustomContext context)
        {
            this.repository = new PerYearFrequencyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PerYearFrequencyDataMapping();
        }
		 
		public  PerYearFrequencyPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PerYearFrequencyKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PerYearFrequency entityPOCO)
        {
            PerYearFrequencyKeys entityKeys = new PerYearFrequencyKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 