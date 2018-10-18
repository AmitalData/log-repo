 
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
   public partial class SLALineQueryService: EntityQueryService<SLALine,SLALineKeys,SLALinePM,SLAHeaderPM,SLAHeaderKeys>
   {
   
        SLALineRepository repository;
		ICRMContext  context;
        public SLALineQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new SLALineRepository(context);
            Repository = repository;
            mapping = new SLALineDataMapping();
        }

        public SLALineQueryService(SLALineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SLALineDataMapping();
        }

        public SLALineQueryService(ICRMContext context)
        {
            this.repository = new SLALineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SLALineDataMapping();
        }
		 
		public  SLALinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SLALineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SLALine entityPOCO)
        {
            SLALineKeys entityKeys = new SLALineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 