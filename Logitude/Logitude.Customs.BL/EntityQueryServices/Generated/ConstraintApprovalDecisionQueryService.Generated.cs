 
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
   public partial class ConstraintApprovalDecisionQueryService: EntityQueryService<ConstraintApprovalDecision,ConstraintApprovalDecisionKeys,ConstraintApprovalDecisionPM,object,ConstraintApprovalDecisionKeys>
   {
   
        ConstraintApprovalDecisionRepository repository;
		ICustomContext  context;
        public ConstraintApprovalDecisionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConstraintApprovalDecisionRepository(context);
            Repository = repository;
            mapping = new ConstraintApprovalDecisionDataMapping();
        }

        public ConstraintApprovalDecisionQueryService(ConstraintApprovalDecisionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConstraintApprovalDecisionDataMapping();
        }

        public ConstraintApprovalDecisionQueryService(ICustomContext context)
        {
            this.repository = new ConstraintApprovalDecisionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConstraintApprovalDecisionDataMapping();
        }
		 
		public  ConstraintApprovalDecisionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConstraintApprovalDecisionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConstraintApprovalDecision entityPOCO)
        {
            ConstraintApprovalDecisionKeys entityKeys = new ConstraintApprovalDecisionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 