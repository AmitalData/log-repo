 
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
   public partial class ActivityNoteQueryService: EntityQueryService<ActivityNote,ActivityNoteKeys,ActivityNotePM,ActivityPM,ActivityKeys>
   {
   
        ActivityNoteRepository repository;
		ICRMContext  context;
        public ActivityNoteQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new ActivityNoteRepository(context);
            Repository = repository;
            mapping = new ActivityNoteDataMapping();
        }

        public ActivityNoteQueryService(ActivityNoteRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ActivityNoteDataMapping();
        }

        public ActivityNoteQueryService(ICRMContext context)
        {
            this.repository = new ActivityNoteRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ActivityNoteDataMapping();
        }
		 
		public  ActivityNotePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ActivityNoteKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ActivityNote entityPOCO)
        {
            ActivityNoteKeys entityKeys = new ActivityNoteKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 