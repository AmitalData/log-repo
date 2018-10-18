 
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
   public partial class VehicleOwnerQueryService: EntityQueryService<VehicleOwner,VehicleOwnerKeys,VehicleOwnerPM,VehiclePM,VehicleKeys>
   {
   
        VehicleOwnerRepository repository;
		ICustomContext  context;
        public VehicleOwnerQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VehicleOwnerRepository(context);
            Repository = repository;
            mapping = new VehicleOwnerDataMapping();
        }

        public VehicleOwnerQueryService(VehicleOwnerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VehicleOwnerDataMapping();
        }

        public VehicleOwnerQueryService(ICustomContext context)
        {
            this.repository = new VehicleOwnerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VehicleOwnerDataMapping();
        }
		 
		public  VehicleOwnerPM GetSingle(string vehicleid, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VehicleOwnerKeys(){ VehicleId = vehicleid, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VehicleOwner entityPOCO)
        {
            VehicleOwnerKeys entityKeys = new VehicleOwnerKeys() { VehicleId = entityPOCO.VehicleId, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 