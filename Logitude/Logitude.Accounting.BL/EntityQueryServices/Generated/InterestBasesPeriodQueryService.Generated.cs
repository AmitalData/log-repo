 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class InterestBasesPeriodQueryService: EntityQueryService<InterestBasesPeriod,InterestBasesPeriodKeys,InterestBasesPeriodPM,object,InterestBasesPeriodKeys>
   {
   
        InterestBasesPeriodRepository repository;
		IAccountingContext  context;
        public InterestBasesPeriodQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestBasesPeriodRepository(context);
            Repository = repository;
            mapping = new InterestBasesPeriodDataMapping();
        }

        public InterestBasesPeriodQueryService(InterestBasesPeriodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestBasesPeriodDataMapping();
        }

        public InterestBasesPeriodQueryService(IAccountingContext context)
        {
            this.repository = new InterestBasesPeriodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestBasesPeriodDataMapping();
        }
		 
		public  InterestBasesPeriodPM GetSingle(string interestbasetypeid, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestBasesPeriodKeys(){ InterestBaseTypeId = interestbasetypeid, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestBasesPeriod entityPOCO)
        {
            InterestBasesPeriodKeys entityKeys = new InterestBasesPeriodKeys() { InterestBaseTypeId = entityPOCO.InterestBaseTypeId, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 