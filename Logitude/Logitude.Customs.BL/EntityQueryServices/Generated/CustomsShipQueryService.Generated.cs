 
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
   public partial class CustomsShipQueryService: EntityQueryService<CustomsShip,CustomsShipKeys,CustomsShipPM,object,CustomsShipKeys>
   {
   
        CustomsShipRepository repository;
		ICustomContext  context;
        public CustomsShipQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsShipRepository(context);
            Repository = repository;
            mapping = new CustomsShipDataMapping();
        }

        public CustomsShipQueryService(CustomsShipRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsShipDataMapping();
        }

        public CustomsShipQueryService(ICustomContext context)
        {
            this.repository = new CustomsShipRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsShipDataMapping();
        }
		 
		public  CustomsShipPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsShipKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsShip entityPOCO)
        {
            CustomsShipKeys entityKeys = new CustomsShipKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 