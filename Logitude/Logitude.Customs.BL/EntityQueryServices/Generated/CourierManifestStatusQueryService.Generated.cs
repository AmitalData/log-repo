 
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
   public partial class CourierManifestStatusQueryService: EntityQueryService<CourierManifestStatus,CourierManifestStatusKeys,CourierManifestStatusPM,object,CourierManifestStatusKeys>
   {
   
        CourierManifestStatusRepository repository;
		ICustomContext  context;
        public CourierManifestStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CourierManifestStatusRepository(context);
            Repository = repository;
            mapping = new CourierManifestStatusDataMapping();
        }

        public CourierManifestStatusQueryService(CourierManifestStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CourierManifestStatusDataMapping();
        }

        public CourierManifestStatusQueryService(ICustomContext context)
        {
            this.repository = new CourierManifestStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CourierManifestStatusDataMapping();
        }
		 
		public  CourierManifestStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CourierManifestStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CourierManifestStatus entityPOCO)
        {
            CourierManifestStatusKeys entityKeys = new CourierManifestStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 