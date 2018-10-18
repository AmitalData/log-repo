 
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
   public partial class EscalationPreDefinitionQueryService: EntityQueryService<EscalationPreDefinition,EscalationPreDefinitionKeys,EscalationPreDefinitionPM,object,EscalationPreDefinitionKeys>
   {
   
        EscalationPreDefinitionRepository repository;
		ICRMContext  context;
        public EscalationPreDefinitionQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new EscalationPreDefinitionRepository(context);
            Repository = repository;
            mapping = new EscalationPreDefinitionDataMapping();
        }

        public EscalationPreDefinitionQueryService(EscalationPreDefinitionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new EscalationPreDefinitionDataMapping();
        }

        public EscalationPreDefinitionQueryService(ICRMContext context)
        {
            this.repository = new EscalationPreDefinitionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new EscalationPreDefinitionDataMapping();
        }
		 
		public  EscalationPreDefinitionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new EscalationPreDefinitionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(EscalationPreDefinition entityPOCO)
        {
            EscalationPreDefinitionKeys entityKeys = new EscalationPreDefinitionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 