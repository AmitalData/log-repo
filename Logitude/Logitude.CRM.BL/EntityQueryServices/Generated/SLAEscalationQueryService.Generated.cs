 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class SLAEscalationQueryService: EntityQueryService<SLAEscalation,SLAEscalationKeys,SLAEscalationPM,SLAHeaderPM,SLAHeaderKeys>
   {
   
        SLAEscalationRepository repository;
		ICRMContext  context;
        public SLAEscalationQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new SLAEscalationRepository(context);
            Repository = repository;
            mapping = new SLAEscalationDataMapping();
        }

        public SLAEscalationQueryService(SLAEscalationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SLAEscalationDataMapping();
        }

        public SLAEscalationQueryService(ICRMContext context)
        {
            this.repository = new SLAEscalationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SLAEscalationDataMapping();
        }
		 
		public  SLAEscalationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SLAEscalationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SLAEscalation entityPOCO)
        {
            SLAEscalationKeys entityKeys = new SLAEscalationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 