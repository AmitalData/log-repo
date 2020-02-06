 
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
   public partial class CargoSealIdentifierQueryService: EntityQueryService<CargoSealIdentifier,CargoSealIdentifierKeys,CargoSealIdentifierPM,object,CargoSealIdentifierKeys>
   {
   
        CargoSealIdentifierRepository repository;
		ICustomContext  context;
        public CargoSealIdentifierQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CargoSealIdentifierRepository(context);
            Repository = repository;
            mapping = new CargoSealIdentifierDataMapping();
        }

        public CargoSealIdentifierQueryService(CargoSealIdentifierRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CargoSealIdentifierDataMapping();
        }

        public CargoSealIdentifierQueryService(ICustomContext context)
        {
            this.repository = new CargoSealIdentifierRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CargoSealIdentifierDataMapping();
        }
		 
		public  CargoSealIdentifierPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CargoSealIdentifierKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CargoSealIdentifier entityPOCO)
        {
            CargoSealIdentifierKeys entityKeys = new CargoSealIdentifierKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 