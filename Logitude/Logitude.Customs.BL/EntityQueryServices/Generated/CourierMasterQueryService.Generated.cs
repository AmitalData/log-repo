 
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
   public partial class CourierMasterQueryService: EntityQueryService<CourierMaster,CourierMasterKeys,CourierMasterPM,object,CourierMasterKeys>
   {
   
        CourierMasterRepository repository;
		ICustomContext  context;
        public CourierMasterQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CourierMasterRepository(context);
            Repository = repository;
            mapping = new CourierMasterDataMapping();
        }

        public CourierMasterQueryService(CourierMasterRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CourierMasterDataMapping();
        }

        public CourierMasterQueryService(ICustomContext context)
        {
            this.repository = new CourierMasterRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CourierMasterDataMapping();
        }
		 
		public  CourierMasterPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CourierMasterKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CourierMaster entityPOCO)
        {
            CourierMasterKeys entityKeys = new CourierMasterKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 