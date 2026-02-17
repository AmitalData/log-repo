 
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
   public partial class OpenFormatReportStatusQueryService: EntityQueryService<OpenFormatReportStatus,OpenFormatReportStatusKeys,OpenFormatReportStatusPM,object,OpenFormatReportStatusKeys>
   {
   
        OpenFormatReportStatusRepository repository;
		IAccountingContext  context;
        public OpenFormatReportStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new OpenFormatReportStatusRepository(context);
            Repository = repository;
            mapping = new OpenFormatReportStatusDataMapping();
        }

        public OpenFormatReportStatusQueryService(OpenFormatReportStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpenFormatReportStatusDataMapping();
        }

        public OpenFormatReportStatusQueryService(IAccountingContext context)
        {
            this.repository = new OpenFormatReportStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpenFormatReportStatusDataMapping();
        }
		 
		public  OpenFormatReportStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpenFormatReportStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpenFormatReportStatus entityPOCO)
        {
            OpenFormatReportStatusKeys entityKeys = new OpenFormatReportStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 