 
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
   public partial class QuotaComputationBasisQueryService: EntityQueryService<QuotaComputationBasis,QuotaComputationBasisKeys,QuotaComputationBasisPM,object,QuotaComputationBasisKeys>
   {
   
        QuotaComputationBasisRepository repository;
		ICustomContext  context;
        public QuotaComputationBasisQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new QuotaComputationBasisRepository(context);
            Repository = repository;
            mapping = new QuotaComputationBasisDataMapping();
        }

        public QuotaComputationBasisQueryService(QuotaComputationBasisRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuotaComputationBasisDataMapping();
        }

        public QuotaComputationBasisQueryService(ICustomContext context)
        {
            this.repository = new QuotaComputationBasisRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuotaComputationBasisDataMapping();
        }
		 
		public  QuotaComputationBasisPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuotaComputationBasisKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuotaComputationBasis entityPOCO)
        {
            QuotaComputationBasisKeys entityKeys = new QuotaComputationBasisKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 