 
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
   public partial class CheckRepresentativeTypeQueryService: EntityQueryService<CheckRepresentativeType,CheckRepresentativeTypeKeys,CheckRepresentativeTypePM,object,CheckRepresentativeTypeKeys>
   {
   
        CheckRepresentativeTypeRepository repository;
		ICustomContext  context;
        public CheckRepresentativeTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CheckRepresentativeTypeRepository(context);
            Repository = repository;
            mapping = new CheckRepresentativeTypeDataMapping();
        }

        public CheckRepresentativeTypeQueryService(CheckRepresentativeTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CheckRepresentativeTypeDataMapping();
        }

        public CheckRepresentativeTypeQueryService(ICustomContext context)
        {
            this.repository = new CheckRepresentativeTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CheckRepresentativeTypeDataMapping();
        }
		 
		public  CheckRepresentativeTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CheckRepresentativeTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CheckRepresentativeType entityPOCO)
        {
            CheckRepresentativeTypeKeys entityKeys = new CheckRepresentativeTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 