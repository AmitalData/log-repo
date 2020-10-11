 
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
   public partial class CourierPendingReasonQueryService: EntityQueryService<CourierPendingReason,CourierPendingReasonKeys,CourierPendingReasonPM,object,CourierPendingReasonKeys>
   {
   
        CourierPendingReasonRepository repository;
		ICustomContext  context;
        public CourierPendingReasonQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CourierPendingReasonRepository(context);
            Repository = repository;
            mapping = new CourierPendingReasonDataMapping();
        }

        public CourierPendingReasonQueryService(CourierPendingReasonRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CourierPendingReasonDataMapping();
        }

        public CourierPendingReasonQueryService(ICustomContext context)
        {
            this.repository = new CourierPendingReasonRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CourierPendingReasonDataMapping();
        }
		 
		public  CourierPendingReasonPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CourierPendingReasonKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CourierPendingReason entityPOCO)
        {
            CourierPendingReasonKeys entityKeys = new CourierPendingReasonKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 