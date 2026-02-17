 
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
   public partial class ConfirmationTypeQueryService: EntityQueryService<ConfirmationType,ConfirmationTypeKeys,ConfirmationTypePM,object,ConfirmationTypeKeys>
   {
   
        ConfirmationTypeRepository repository;
		ICustomContext  context;
        public ConfirmationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConfirmationTypeRepository(context);
            Repository = repository;
            mapping = new ConfirmationTypeDataMapping();
        }

        public ConfirmationTypeQueryService(ConfirmationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConfirmationTypeDataMapping();
        }

        public ConfirmationTypeQueryService(ICustomContext context)
        {
            this.repository = new ConfirmationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConfirmationTypeDataMapping();
        }
		 
		public  ConfirmationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConfirmationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConfirmationType entityPOCO)
        {
            ConfirmationTypeKeys entityKeys = new ConfirmationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 