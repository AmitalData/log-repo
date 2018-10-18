 
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
   public partial class FuelTypeQueryService: EntityQueryService<FuelType,FuelTypeKeys,FuelTypePM,object,FuelTypeKeys>
   {
   
        FuelTypeRepository repository;
		ICustomContext  context;
        public FuelTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new FuelTypeRepository(context);
            Repository = repository;
            mapping = new FuelTypeDataMapping();
        }

        public FuelTypeQueryService(FuelTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FuelTypeDataMapping();
        }

        public FuelTypeQueryService(ICustomContext context)
        {
            this.repository = new FuelTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FuelTypeDataMapping();
        }
		 
		public  FuelTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FuelTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FuelType entityPOCO)
        {
            FuelTypeKeys entityKeys = new FuelTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 