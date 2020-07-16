 
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
   public partial class InterestLastBatchServiceQueryService 
   {
        public InterestLastBatchServicePM CheckInterestLastBatchServicesByTenant(int tenant)
        {
            InterestLastBatchServicePM InterestLastBatchService = (from a in context.InterestLastBatchServices
                    where a.Tenant == tenant  
                    select new InterestLastBatchServicePM
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateInvoicesBatchId = a.CreateInvoicesBatchId,
                        CreateReportsBatchId = a.CreateReportsBatchId,
  
                    }).FirstOrDefault();

            return InterestLastBatchService;

        }

   
    }
   
}
	 