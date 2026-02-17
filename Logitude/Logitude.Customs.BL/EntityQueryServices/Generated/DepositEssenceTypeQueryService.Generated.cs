 
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
   public partial class DepositEssenceTypeQueryService: EntityQueryService<DepositEssenceType,DepositEssenceTypeKeys,DepositEssenceTypePM,object,DepositEssenceTypeKeys>
   {
   
        DepositEssenceTypeRepository repository;
		ICustomContext  context;
        public DepositEssenceTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DepositEssenceTypeRepository(context);
            Repository = repository;
            mapping = new DepositEssenceTypeDataMapping();
        }

        public DepositEssenceTypeQueryService(DepositEssenceTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DepositEssenceTypeDataMapping();
        }

        public DepositEssenceTypeQueryService(ICustomContext context)
        {
            this.repository = new DepositEssenceTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DepositEssenceTypeDataMapping();
        }
		 
		public  DepositEssenceTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DepositEssenceTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DepositEssenceType entityPOCO)
        {
            DepositEssenceTypeKeys entityKeys = new DepositEssenceTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 