 
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
   public partial class CertificateOfOriginStatusCodeEnumQueryService: EntityQueryService<CertificateOfOriginStatusCodeEnum,CertificateOfOriginStatusCodeEnumKeys,CertificateOfOriginStatusCodeEnumPM,object,CertificateOfOriginStatusCodeEnumKeys>
   {
   
        CertificateOfOriginStatusCodeEnumRepository repository;
		ICustomContext  context;
        public CertificateOfOriginStatusCodeEnumQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CertificateOfOriginStatusCodeEnumRepository(context);
            Repository = repository;
            mapping = new CertificateOfOriginStatusCodeEnumDataMapping();
        }

        public CertificateOfOriginStatusCodeEnumQueryService(CertificateOfOriginStatusCodeEnumRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CertificateOfOriginStatusCodeEnumDataMapping();
        }

        public CertificateOfOriginStatusCodeEnumQueryService(ICustomContext context)
        {
            this.repository = new CertificateOfOriginStatusCodeEnumRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CertificateOfOriginStatusCodeEnumDataMapping();
        }
		 
		public  CertificateOfOriginStatusCodeEnumPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CertificateOfOriginStatusCodeEnumKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CertificateOfOriginStatusCodeEnum entityPOCO)
        {
            CertificateOfOriginStatusCodeEnumKeys entityKeys = new CertificateOfOriginStatusCodeEnumKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 