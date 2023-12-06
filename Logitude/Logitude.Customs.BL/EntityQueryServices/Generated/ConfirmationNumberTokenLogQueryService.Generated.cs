 
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
   public partial class ConfirmationNumberTokenLogQueryService: EntityQueryService<ConfirmationNumberTokenLog,ConfirmationNumberTokenLogKeys,ConfirmationNumberTokenLogPM,object,ConfirmationNumberTokenLogKeys>
   {
   
        ConfirmationNumberTokenLogRepository repository;
		ICustomContext  context;
        public ConfirmationNumberTokenLogQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConfirmationNumberTokenLogRepository(context);
            Repository = repository;
            mapping = new ConfirmationNumberTokenLogDataMapping();
        }

        public ConfirmationNumberTokenLogQueryService(ConfirmationNumberTokenLogRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConfirmationNumberTokenLogDataMapping();
        }

        public ConfirmationNumberTokenLogQueryService(ICustomContext context)
        {
            this.repository = new ConfirmationNumberTokenLogRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConfirmationNumberTokenLogDataMapping();
        }
		 
		public  ConfirmationNumberTokenLogPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConfirmationNumberTokenLogKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConfirmationNumberTokenLog entityPOCO)
        {
            ConfirmationNumberTokenLogKeys entityKeys = new ConfirmationNumberTokenLogKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 