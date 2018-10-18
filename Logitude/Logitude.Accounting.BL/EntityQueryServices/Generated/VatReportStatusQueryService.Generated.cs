 
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
   public partial class VatReportStatusQueryService: EntityQueryService<VatReportStatus,VatReportStatusKeys,VatReportStatusPM,object,VatReportStatusKeys>
   {
   
        VatReportStatusRepository repository;
		IAccountingContext  context;
        public VatReportStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new VatReportStatusRepository(context);
            Repository = repository;
            mapping = new VatReportStatusDataMapping();
        }

        public VatReportStatusQueryService(VatReportStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VatReportStatusDataMapping();
        }

        public VatReportStatusQueryService(IAccountingContext context)
        {
            this.repository = new VatReportStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VatReportStatusDataMapping();
        }
		 
		public  VatReportStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VatReportStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VatReportStatus entityPOCO)
        {
            VatReportStatusKeys entityKeys = new VatReportStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 