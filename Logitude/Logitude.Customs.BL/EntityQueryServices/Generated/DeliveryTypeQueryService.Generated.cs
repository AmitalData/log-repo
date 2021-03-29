 
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
   public partial class DeliveryTypeQueryService: EntityQueryService<DeliveryType,DeliveryTypeKeys,DeliveryTypePM,object,DeliveryTypeKeys>
   {
   
        DeliveryTypeRepository repository;
		ICustomContext  context;
        public DeliveryTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeliveryTypeRepository(context);
            Repository = repository;
            mapping = new DeliveryTypeDataMapping();
        }

        public DeliveryTypeQueryService(DeliveryTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeliveryTypeDataMapping();
        }

        public DeliveryTypeQueryService(ICustomContext context)
        {
            this.repository = new DeliveryTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeliveryTypeDataMapping();
        }
		 
		public  DeliveryTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeliveryTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeliveryType entityPOCO)
        {
            DeliveryTypeKeys entityKeys = new DeliveryTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 