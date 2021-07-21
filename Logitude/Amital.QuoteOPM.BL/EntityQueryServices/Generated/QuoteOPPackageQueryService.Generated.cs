 
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
   public partial class QuoteOPPackageQueryService: EntityQueryService<QuoteOPPackage,QuoteOPPackageKeys,QuoteOPPackagePM,QuoteOPPM,QuoteOPKeys>
   {
   
        QuoteOPPackageRepository repository;
		IQuoteOPMContext  context;
        public QuoteOPPackageQueryService(int tenant)
        {
		    context = QuoteOPMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuoteOPPackageRepository(context);
            Repository = repository;
            mapping = new QuoteOPPackageDataMapping();
        }

        public QuoteOPPackageQueryService(QuoteOPPackageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuoteOPPackageDataMapping();
        }

        public QuoteOPPackageQueryService(IQuoteOPMContext context)
        {
            this.repository = new QuoteOPPackageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuoteOPPackageDataMapping();
        }
		 
		public  QuoteOPPackagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuoteOPPackageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuoteOPPackage entityPOCO)
        {
            QuoteOPPackageKeys entityKeys = new QuoteOPPackageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 