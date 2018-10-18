 
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
   public partial class CorrespondenceQueryService: EntityQueryService<Correspondence,CorrespondenceKeys,CorrespondencePM,object,CorrespondenceKeys>
   {
   
        CorrespondenceRepository repository;
		ICRMContext  context;
        public CorrespondenceQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new CorrespondenceRepository(context);
            Repository = repository;
            mapping = new CorrespondenceDataMapping();
        }

        public CorrespondenceQueryService(CorrespondenceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CorrespondenceDataMapping();
        }

        public CorrespondenceQueryService(ICRMContext context)
        {
            this.repository = new CorrespondenceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CorrespondenceDataMapping();
        }
		 
		public  CorrespondencePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CorrespondenceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Correspondence entityPOCO)
        {
            CorrespondenceKeys entityKeys = new CorrespondenceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 