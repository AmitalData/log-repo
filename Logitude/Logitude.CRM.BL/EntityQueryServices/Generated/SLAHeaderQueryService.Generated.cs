 
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
   public partial class SLAHeaderQueryService: EntityQueryService<SLAHeader,SLAHeaderKeys,SLAHeaderPM,object,SLAHeaderKeys>
   {
   
        SLAHeaderRepository repository;
		ICRMContext  context;
        public SLAHeaderQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new SLAHeaderRepository(context);
            Repository = repository;
            mapping = new SLAHeaderDataMapping();
        }

        public SLAHeaderQueryService(SLAHeaderRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SLAHeaderDataMapping();
        }

        public SLAHeaderQueryService(ICRMContext context)
        {
            this.repository = new SLAHeaderRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SLAHeaderDataMapping();
        }
		 
		public  SLAHeaderPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SLAHeaderKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SLAHeader entityPOCO)
        {
            SLAHeaderKeys entityKeys = new SLAHeaderKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 