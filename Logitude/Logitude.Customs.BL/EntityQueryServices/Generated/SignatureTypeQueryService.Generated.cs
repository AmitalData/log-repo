 
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
   public partial class SignatureTypeQueryService: EntityQueryService<SignatureType,SignatureTypeKeys,SignatureTypePM,object,SignatureTypeKeys>
   {
   
        SignatureTypeRepository repository;
		ICustomContext  context;
        public SignatureTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SignatureTypeRepository(context);
            Repository = repository;
            mapping = new SignatureTypeDataMapping();
        }

        public SignatureTypeQueryService(SignatureTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SignatureTypeDataMapping();
        }

        public SignatureTypeQueryService(ICustomContext context)
        {
            this.repository = new SignatureTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SignatureTypeDataMapping();
        }
		 
		public  SignatureTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SignatureTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SignatureType entityPOCO)
        {
            SignatureTypeKeys entityKeys = new SignatureTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 