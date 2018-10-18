 
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
   public partial class AcceptanceStatusQueryService: EntityQueryService<AcceptanceStatus,AcceptanceStatusKeys,AcceptanceStatusPM,object,AcceptanceStatusKeys>
   {
   
        AcceptanceStatusRepository repository;
		ICustomContext  context;
        public AcceptanceStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AcceptanceStatusRepository(context);
            Repository = repository;
            mapping = new AcceptanceStatusDataMapping();
        }

        public AcceptanceStatusQueryService(AcceptanceStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AcceptanceStatusDataMapping();
        }

        public AcceptanceStatusQueryService(ICustomContext context)
        {
            this.repository = new AcceptanceStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AcceptanceStatusDataMapping();
        }
		 
		public  AcceptanceStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AcceptanceStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AcceptanceStatus entityPOCO)
        {
            AcceptanceStatusKeys entityKeys = new AcceptanceStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 