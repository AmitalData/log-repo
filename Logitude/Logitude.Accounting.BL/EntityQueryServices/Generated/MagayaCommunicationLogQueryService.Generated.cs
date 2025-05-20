 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class MagayaCommunicationLogQueryService: EntityQueryService<MagayaCommunicationLog,MagayaCommunicationLogKeys,MagayaCommunicationLogPM,object,MagayaCommunicationLogKeys>
   {
   
        MagayaCommunicationLogRepository repository;
		IAccountingContext  context;
        public MagayaCommunicationLogQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new MagayaCommunicationLogRepository(context);
            Repository = repository;
            mapping = new MagayaCommunicationLogDataMapping();
        }

        public MagayaCommunicationLogQueryService(MagayaCommunicationLogRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MagayaCommunicationLogDataMapping();
        }

        public MagayaCommunicationLogQueryService(IAccountingContext context)
        {
            this.repository = new MagayaCommunicationLogRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MagayaCommunicationLogDataMapping();
        }
		 
		public  MagayaCommunicationLogPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MagayaCommunicationLogKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MagayaCommunicationLog entityPOCO)
        {
            MagayaCommunicationLogKeys entityKeys = new MagayaCommunicationLogKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 