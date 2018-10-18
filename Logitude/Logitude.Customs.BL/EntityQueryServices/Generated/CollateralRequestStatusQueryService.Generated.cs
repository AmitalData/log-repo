 
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
   public partial class CollateralRequestStatusQueryService: EntityQueryService<CollateralRequestStatus,CollateralRequestStatusKeys,CollateralRequestStatusPM,object,CollateralRequestStatusKeys>
   {
   
        CollateralRequestStatusRepository repository;
		ICustomContext  context;
        public CollateralRequestStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CollateralRequestStatusRepository(context);
            Repository = repository;
            mapping = new CollateralRequestStatusDataMapping();
        }

        public CollateralRequestStatusQueryService(CollateralRequestStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CollateralRequestStatusDataMapping();
        }

        public CollateralRequestStatusQueryService(ICustomContext context)
        {
            this.repository = new CollateralRequestStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CollateralRequestStatusDataMapping();
        }
		 
		public  CollateralRequestStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CollateralRequestStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CollateralRequestStatus entityPOCO)
        {
            CollateralRequestStatusKeys entityKeys = new CollateralRequestStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 