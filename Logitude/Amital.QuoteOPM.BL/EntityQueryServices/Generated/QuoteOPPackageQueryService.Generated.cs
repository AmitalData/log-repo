 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Amital.QuoteOPM.BL.EntityDataMappings;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Data;
using Simplog.Server.Infrastructure;
namespace Amital.QuoteOPM.BL.EntityQueryServices
{ 
   public partial class QuoteopPackageQueryService: EntityQueryService<QuoteopPackage,QuoteopPackageKeys,QuoteopPackagePM,QuoteOPPM,QuoteOPKeys>
   {
   
        QuoteopPackageRepository repository;
		IQuoteOPMContext  context;
        public QuoteopPackageQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteopPackageRepository(context);
            Repository = repository;
            mapping = new QuoteopPackageDataMapping();
        }

        public QuoteopPackageQueryService(QuoteopPackageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteopPackageDataMapping();
        }

        public QuoteopPackageQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteopPackageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteopPackageDataMapping();
        }
		 
		public  QuoteopPackagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteopPackageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteopPackage entityPOCO)
        {
            QuoteopPackageKeys entityKeys = new QuoteopPackageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 