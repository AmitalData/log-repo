 
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
   public partial class QuotaIncrementQueryService: EntityQueryService<QuotaIncrement,QuotaIncrementKeys,QuotaIncrementPM,object,QuotaIncrementKeys>
   {
   
        QuotaIncrementRepository repository;
		ICustomContext  context;
        public QuotaIncrementQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new QuotaIncrementRepository(context);
            Repository = repository;
            mapping = new QuotaIncrementDataMapping();
        }

        public QuotaIncrementQueryService(QuotaIncrementRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuotaIncrementDataMapping();
        }

        public QuotaIncrementQueryService(ICustomContext context)
        {
            this.repository = new QuotaIncrementRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuotaIncrementDataMapping();
        }
		 
		public  QuotaIncrementPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuotaIncrementKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuotaIncrement entityPOCO)
        {
            QuotaIncrementKeys entityKeys = new QuotaIncrementKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 