 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class AuditLogQueryService: EntityQueryService<AuditLog,AuditLogKeys,AuditLogPM,object,AuditLogKeys>
   {
   
        AuditLogRepository repository;
		IInfrastructureContext  context;
        public AuditLogQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new AuditLogRepository(context);
            Repository = repository;
            mapping = new AuditLogDataMapping();
        }

        public AuditLogQueryService(AuditLogRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AuditLogDataMapping();
        }

        public AuditLogQueryService(IInfrastructureContext context)
        {
            this.repository = new AuditLogRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AuditLogDataMapping();
        }
		 
		public  AuditLogPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AuditLogKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AuditLog entityPOCO)
        {
            AuditLogKeys entityKeys = new AuditLogKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 