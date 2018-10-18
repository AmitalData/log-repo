 
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
   public partial class CourierPaymentStatusQueryService: EntityQueryService<CourierPaymentStatus,CourierPaymentStatusKeys,CourierPaymentStatusPM,object,CourierPaymentStatusKeys>
   {
   
        CourierPaymentStatusRepository repository;
		ICustomContext  context;
        public CourierPaymentStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CourierPaymentStatusRepository(context);
            Repository = repository;
            mapping = new CourierPaymentStatusDataMapping();
        }

        public CourierPaymentStatusQueryService(CourierPaymentStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CourierPaymentStatusDataMapping();
        }

        public CourierPaymentStatusQueryService(ICustomContext context)
        {
            this.repository = new CourierPaymentStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CourierPaymentStatusDataMapping();
        }
		 
		public  CourierPaymentStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CourierPaymentStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CourierPaymentStatus entityPOCO)
        {
            CourierPaymentStatusKeys entityKeys = new CourierPaymentStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 