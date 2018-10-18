 
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
   public partial class EscalationActionTimeIndicatorQueryService: EntityQueryService<EscalationActionTimeIndicator,EscalationActionTimeIndicatorKeys,EscalationActionTimeIndicatorPM,object,EscalationActionTimeIndicatorKeys>
   {
   
        EscalationActionTimeIndicatorRepository repository;
		ICRMContext  context;
        public EscalationActionTimeIndicatorQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new EscalationActionTimeIndicatorRepository(context);
            Repository = repository;
            mapping = new EscalationActionTimeIndicatorDataMapping();
        }

        public EscalationActionTimeIndicatorQueryService(EscalationActionTimeIndicatorRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new EscalationActionTimeIndicatorDataMapping();
        }

        public EscalationActionTimeIndicatorQueryService(ICRMContext context)
        {
            this.repository = new EscalationActionTimeIndicatorRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new EscalationActionTimeIndicatorDataMapping();
        }
		 
		public  EscalationActionTimeIndicatorPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new EscalationActionTimeIndicatorKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(EscalationActionTimeIndicator entityPOCO)
        {
            EscalationActionTimeIndicatorKeys entityKeys = new EscalationActionTimeIndicatorKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 