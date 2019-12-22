 
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
   public partial class InterestReportLinesByDateQueryService: EntityQueryService<InterestReportLinesByDate,InterestReportLinesByDateKeys,InterestReportLinesByDatePM,object,InterestReportLinesByDateKeys>
   {
   
        InterestReportLinesByDateRepository repository;
		IAccountingContext  context;
        public InterestReportLinesByDateQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestReportLinesByDateRepository(context);
            Repository = repository;
            mapping = new InterestReportLinesByDateDataMapping();
        }

        public InterestReportLinesByDateQueryService(InterestReportLinesByDateRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestReportLinesByDateDataMapping();
        }

        public InterestReportLinesByDateQueryService(IAccountingContext context)
        {
            this.repository = new InterestReportLinesByDateRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestReportLinesByDateDataMapping();
        }
		 
		public  InterestReportLinesByDatePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestReportLinesByDateKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestReportLinesByDate entityPOCO)
        {
            InterestReportLinesByDateKeys entityKeys = new InterestReportLinesByDateKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 