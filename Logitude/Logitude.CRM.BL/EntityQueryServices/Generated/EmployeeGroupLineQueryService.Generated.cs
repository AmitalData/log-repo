 
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
   public partial class EmployeeGroupLineQueryService: EntityQueryService<EmployeeGroupLine,EmployeeGroupLineKeys,EmployeeGroupLinePM,EmployeeGroupPM,EmployeeGroupKeys>
   {
   
        EmployeeGroupLineRepository repository;
		ICRMContext  context;
        public EmployeeGroupLineQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new EmployeeGroupLineRepository(context);
            Repository = repository;
            mapping = new EmployeeGroupLineDataMapping();
        }

        public EmployeeGroupLineQueryService(EmployeeGroupLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new EmployeeGroupLineDataMapping();
        }

        public EmployeeGroupLineQueryService(ICRMContext context)
        {
            this.repository = new EmployeeGroupLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new EmployeeGroupLineDataMapping();
        }
		 
		public  EmployeeGroupLinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new EmployeeGroupLineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(EmployeeGroupLine entityPOCO)
        {
            EmployeeGroupLineKeys entityKeys = new EmployeeGroupLineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 