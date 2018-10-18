 
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
   public partial class GuaranteeCertificateTypeQueryService: EntityQueryService<GuaranteeCertificateType,GuaranteeCertificateTypeKeys,GuaranteeCertificateTypePM,object,GuaranteeCertificateTypeKeys>
   {
   
        GuaranteeCertificateTypeRepository repository;
		ICustomContext  context;
        public GuaranteeCertificateTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new GuaranteeCertificateTypeRepository(context);
            Repository = repository;
            mapping = new GuaranteeCertificateTypeDataMapping();
        }

        public GuaranteeCertificateTypeQueryService(GuaranteeCertificateTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GuaranteeCertificateTypeDataMapping();
        }

        public GuaranteeCertificateTypeQueryService(ICustomContext context)
        {
            this.repository = new GuaranteeCertificateTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GuaranteeCertificateTypeDataMapping();
        }
		 
		public  GuaranteeCertificateTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GuaranteeCertificateTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GuaranteeCertificateType entityPOCO)
        {
            GuaranteeCertificateTypeKeys entityKeys = new GuaranteeCertificateTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 