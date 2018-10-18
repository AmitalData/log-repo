 
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
   public partial class CourierStatusQueryService: EntityQueryService<CourierStatus,CourierStatusKeys,CourierStatusPM,object,CourierStatusKeys>
   {
   
        CourierStatusRepository repository;
		ICustomContext  context;
        public CourierStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CourierStatusRepository(context);
            Repository = repository;
            mapping = new CourierStatusDataMapping();
        }

        public CourierStatusQueryService(CourierStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CourierStatusDataMapping();
        }

        public CourierStatusQueryService(ICustomContext context)
        {
            this.repository = new CourierStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CourierStatusDataMapping();
        }
		 
		public  CourierStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CourierStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CourierStatus entityPOCO)
        {
            CourierStatusKeys entityKeys = new CourierStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 