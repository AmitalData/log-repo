 
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
   public partial class BIReportQueryService: EntityQueryService<BIReport,BIReportKeys,BIReportPM,object,BIReportKeys>
   {
   
        BIReportRepository repository;
		IInfrastructureContext  context;
        public BIReportQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BIReportRepository(context);
            Repository = repository;
            mapping = new BIReportDataMapping();
        }

        public BIReportQueryService(BIReportRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BIReportDataMapping();
        }

        public BIReportQueryService(IInfrastructureContext context)
        {
            this.repository = new BIReportRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BIReportDataMapping();
        }
		 
		public  BIReportPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BIReportKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BIReport entityPOCO)
        {
            BIReportKeys entityKeys = new BIReportKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 