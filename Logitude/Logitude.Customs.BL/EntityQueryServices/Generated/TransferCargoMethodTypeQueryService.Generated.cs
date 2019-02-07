 
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
   public partial class TransferCargoMethodTypeQueryService: EntityQueryService<TransferCargoMethodType,TransferCargoMethodTypeKeys,TransferCargoMethodTypePM,object,TransferCargoMethodTypeKeys>
   {
   
        TransferCargoMethodTypeRepository repository;
		ICustomContext  context;
        public TransferCargoMethodTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TransferCargoMethodTypeRepository(context);
            Repository = repository;
            mapping = new TransferCargoMethodTypeDataMapping();
        }

        public TransferCargoMethodTypeQueryService(TransferCargoMethodTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TransferCargoMethodTypeDataMapping();
        }

        public TransferCargoMethodTypeQueryService(ICustomContext context)
        {
            this.repository = new TransferCargoMethodTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TransferCargoMethodTypeDataMapping();
        }
		 
		public  TransferCargoMethodTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TransferCargoMethodTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TransferCargoMethodType entityPOCO)
        {
            TransferCargoMethodTypeKeys entityKeys = new TransferCargoMethodTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 