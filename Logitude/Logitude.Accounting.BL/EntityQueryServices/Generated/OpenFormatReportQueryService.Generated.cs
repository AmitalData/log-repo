 
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
   public partial class OpenFormatReportQueryService: EntityQueryService<OpenFormatReport,OpenFormatReportKeys,OpenFormatReportPM,object,OpenFormatReportKeys>
   {
   
        OpenFormatReportRepository repository;
		IAccountingContext  context;
        public OpenFormatReportQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new OpenFormatReportRepository(context);
            Repository = repository;
            mapping = new OpenFormatReportDataMapping();
        }

        public OpenFormatReportQueryService(OpenFormatReportRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpenFormatReportDataMapping();
        }

        public OpenFormatReportQueryService(IAccountingContext context)
        {
            this.repository = new OpenFormatReportRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpenFormatReportDataMapping();
        }
		 
		public  OpenFormatReportPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpenFormatReportKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpenFormatReport entityPOCO)
        {
            OpenFormatReportKeys entityKeys = new OpenFormatReportKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 