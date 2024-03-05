 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class SatisfactionSurveyQueryService: EntityQueryService<SatisfactionSurvey,SatisfactionSurveyKeys,SatisfactionSurveyPM,object,SatisfactionSurveyKeys>
   {
   
        SatisfactionSurveyRepository repository;
		IInfrastructureContext  context;
        public SatisfactionSurveyQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new SatisfactionSurveyRepository(context);
            Repository = repository;
            mapping = new SatisfactionSurveyDataMapping();
        }

        public SatisfactionSurveyQueryService(SatisfactionSurveyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SatisfactionSurveyDataMapping();
        }

        public SatisfactionSurveyQueryService(IInfrastructureContext context)
        {
            this.repository = new SatisfactionSurveyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SatisfactionSurveyDataMapping();
        }
		 
		public  SatisfactionSurveyPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SatisfactionSurveyKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SatisfactionSurvey entityPOCO)
        {
            SatisfactionSurveyKeys entityKeys = new SatisfactionSurveyKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 