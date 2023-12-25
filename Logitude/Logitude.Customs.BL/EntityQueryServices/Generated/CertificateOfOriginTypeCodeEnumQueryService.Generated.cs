 
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
   public partial class CertificateOfOriginTypeCodeEnumQueryService: EntityQueryService<CertificateOfOriginTypeCodeEnum,CertificateOfOriginTypeCodeEnumKeys,CertificateOfOriginTypeCodeEnumPM,object,CertificateOfOriginTypeCodeEnumKeys>
   {
   
        CertificateOfOriginTypeCodeEnumRepository repository;
		ICustomContext  context;
        public CertificateOfOriginTypeCodeEnumQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CertificateOfOriginTypeCodeEnumRepository(context);
            Repository = repository;
            mapping = new CertificateOfOriginTypeCodeEnumDataMapping();
        }

        public CertificateOfOriginTypeCodeEnumQueryService(CertificateOfOriginTypeCodeEnumRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CertificateOfOriginTypeCodeEnumDataMapping();
        }

        public CertificateOfOriginTypeCodeEnumQueryService(ICustomContext context)
        {
            this.repository = new CertificateOfOriginTypeCodeEnumRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CertificateOfOriginTypeCodeEnumDataMapping();
        }
		 
		public  CertificateOfOriginTypeCodeEnumPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CertificateOfOriginTypeCodeEnumKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CertificateOfOriginTypeCodeEnum entityPOCO)
        {
            CertificateOfOriginTypeCodeEnumKeys entityKeys = new CertificateOfOriginTypeCodeEnumKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 