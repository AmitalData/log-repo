 
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
   public partial class CustomsTransportModeQueryService: EntityQueryService<CustomsTransportMode,CustomsTransportModeKeys,CustomsTransportModePM,object,CustomsTransportModeKeys>
   {
   
        CustomsTransportModeRepository repository;
		ICustomContext  context;
        public CustomsTransportModeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsTransportModeRepository(context);
            Repository = repository;
            mapping = new CustomsTransportModeDataMapping();
        }

        public CustomsTransportModeQueryService(CustomsTransportModeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsTransportModeDataMapping();
        }

        public CustomsTransportModeQueryService(ICustomContext context)
        {
            this.repository = new CustomsTransportModeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsTransportModeDataMapping();
        }
		 
		public  CustomsTransportModePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsTransportModeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsTransportMode entityPOCO)
        {
            CustomsTransportModeKeys entityKeys = new CustomsTransportModeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 