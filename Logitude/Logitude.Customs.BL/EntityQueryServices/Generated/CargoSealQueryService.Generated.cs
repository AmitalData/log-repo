 
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
   public partial class CargoSealQueryService: EntityQueryService<CargoSeal,CargoSealKeys,CargoSealPM,CargoSealIdentifierPM,CargoSealIdentifierKeys>
   {
   
        CargoSealRepository repository;
		ICustomContext  context;
        public CargoSealQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoSealRepository(context);
            Repository = repository;
            mapping = new CargoSealDataMapping();
        }

        public CargoSealQueryService(CargoSealRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoSealDataMapping();
        }

        public CargoSealQueryService(ICustomContext context)
        {
            this.repository = new CargoSealRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoSealDataMapping();
        }
		 
		public  CargoSealPM GetSingle(string cargosealidentifierid, string sealnumber, string sealcompletenessstatecode, string sealtypecode, string updatereasoncode, string updatetypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoSealKeys(){ CargoSealIdentifierId = cargosealidentifierid, SealNumber = sealnumber, SealCompletenessStateCode = sealcompletenessstatecode, SealTypeCode = sealtypecode, UpdateReasonCode = updatereasoncode, UpdateTypeCode = updatetypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoSeal entityPOCO)
        {
            CargoSealKeys entityKeys = new CargoSealKeys() { CargoSealIdentifierId = entityPOCO.CargoSealIdentifierId, SealNumber = entityPOCO.SealNumber, SealCompletenessStateCode = entityPOCO.SealCompletenessStateCode, SealTypeCode = entityPOCO.SealTypeCode, UpdateReasonCode = entityPOCO.UpdateReasonCode, UpdateTypeCode = entityPOCO.UpdateTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 