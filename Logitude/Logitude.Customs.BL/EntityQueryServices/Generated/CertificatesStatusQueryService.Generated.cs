 
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
   public partial class CertificatesStatusQueryService: EntityQueryService<CertificatesStatus,CertificatesStatusKeys,CertificatesStatusPM,object,CertificatesStatusKeys>
   {
   
        CertificatesStatusRepository repository;
		ICustomContext  context;
        public CertificatesStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CertificatesStatusRepository(context);
            Repository = repository;
            mapping = new CertificatesStatusDataMapping();
        }

        public CertificatesStatusQueryService(CertificatesStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CertificatesStatusDataMapping();
        }

        public CertificatesStatusQueryService(ICustomContext context)
        {
            this.repository = new CertificatesStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CertificatesStatusDataMapping();
        }
		 
		public  CertificatesStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CertificatesStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CertificatesStatus entityPOCO)
        {
            CertificatesStatusKeys entityKeys = new CertificatesStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 