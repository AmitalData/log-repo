 
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
   public partial class VehicleStatusQueryService: EntityQueryService<VehicleStatus,VehicleStatusKeys,VehicleStatusPM,object,VehicleStatusKeys>
   {
   
        VehicleStatusRepository repository;
		ICustomContext  context;
        public VehicleStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleStatusRepository(context);
            Repository = repository;
            mapping = new VehicleStatusDataMapping();
        }

        public VehicleStatusQueryService(VehicleStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleStatusDataMapping();
        }

        public VehicleStatusQueryService(ICustomContext context)
        {
            this.repository = new VehicleStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleStatusDataMapping();
        }
		 
		public  VehicleStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleStatus entityPOCO)
        {
            VehicleStatusKeys entityKeys = new VehicleStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 