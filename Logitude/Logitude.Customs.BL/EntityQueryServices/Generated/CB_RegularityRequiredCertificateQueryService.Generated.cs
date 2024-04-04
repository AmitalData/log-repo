 
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
   public partial class CB_RegularityRequiredCertificateQueryService: EntityQueryService<CB_RegularityRequiredCertificate,CB_RegularityRequiredCertificateKeys,CB_RegularityRequiredCertificatePM,object,CB_RegularityRequiredCertificateKeys>
   {
   
        CB_RegularityRequiredCertificateRepository repository;
		ICustomContext  context;
        public CB_RegularityRequiredCertificateQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RegularityRequiredCertificateRepository(context);
            Repository = repository;
            mapping = new CB_RegularityRequiredCertificateDataMapping();
        }

        public CB_RegularityRequiredCertificateQueryService(CB_RegularityRequiredCertificateRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RegularityRequiredCertificateDataMapping();
        }

        public CB_RegularityRequiredCertificateQueryService(ICustomContext context)
        {
            this.repository = new CB_RegularityRequiredCertificateRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RegularityRequiredCertificateDataMapping();
        }
		 
		public  CB_RegularityRequiredCertificatePM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RegularityRequiredCertificateKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_RegularityRequiredCertificate entityPOCO)
        {
            CB_RegularityRequiredCertificateKeys entityKeys = new CB_RegularityRequiredCertificateKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 