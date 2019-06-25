 
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
   public partial class OccasionStatusQueryService: EntityQueryService<OccasionStatus,OccasionStatusKeys,OccasionStatusPM,object,OccasionStatusKeys>
   {
   
        OccasionStatusRepository repository;
		ICRMContext  context;
        public OccasionStatusQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OccasionStatusRepository(context);
            Repository = repository;
            mapping = new OccasionStatusDataMapping();
        }

        public OccasionStatusQueryService(OccasionStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OccasionStatusDataMapping();
        }

        public OccasionStatusQueryService(ICRMContext context)
        {
            this.repository = new OccasionStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OccasionStatusDataMapping();
        }
		 
		public  OccasionStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OccasionStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OccasionStatus entityPOCO)
        {
            OccasionStatusKeys entityKeys = new OccasionStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 