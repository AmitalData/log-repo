 
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
   public partial class SecurityClearenceTypeCodeQueryService: EntityQueryService<SecurityClearenceTypeCode,SecurityClearenceTypeCodeKeys,SecurityClearenceTypeCodePM,object,SecurityClearenceTypeCodeKeys>
   {
   
        SecurityClearenceTypeCodeRepository repository;
		ICustomContext  context;
        public SecurityClearenceTypeCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SecurityClearenceTypeCodeRepository(context);
            Repository = repository;
            mapping = new SecurityClearenceTypeCodeDataMapping();
        }

        public SecurityClearenceTypeCodeQueryService(SecurityClearenceTypeCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SecurityClearenceTypeCodeDataMapping();
        }

        public SecurityClearenceTypeCodeQueryService(ICustomContext context)
        {
            this.repository = new SecurityClearenceTypeCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SecurityClearenceTypeCodeDataMapping();
        }
		 
		public  SecurityClearenceTypeCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SecurityClearenceTypeCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SecurityClearenceTypeCode entityPOCO)
        {
            SecurityClearenceTypeCodeKeys entityKeys = new SecurityClearenceTypeCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 