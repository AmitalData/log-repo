 
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
   public partial class IncotemrsFileValidationQueryService: EntityQueryService<IncotemrsFileValidation,IncotemrsFileValidationKeys,IncotemrsFileValidationPM,object,IncotemrsFileValidationKeys>
   {
   
        IncotemrsFileValidationRepository repository;
		ICustomContext  context;
        public IncotemrsFileValidationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new IncotemrsFileValidationRepository(context);
            Repository = repository;
            mapping = new IncotemrsFileValidationDataMapping();
        }

        public IncotemrsFileValidationQueryService(IncotemrsFileValidationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new IncotemrsFileValidationDataMapping();
        }

        public IncotemrsFileValidationQueryService(ICustomContext context)
        {
            this.repository = new IncotemrsFileValidationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new IncotemrsFileValidationDataMapping();
        }
		 
		public  IncotemrsFileValidationPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new IncotemrsFileValidationKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(IncotemrsFileValidation entityPOCO)
        {
            IncotemrsFileValidationKeys entityKeys = new IncotemrsFileValidationKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 