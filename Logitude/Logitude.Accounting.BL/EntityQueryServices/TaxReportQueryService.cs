
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
namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class TaxReportQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TaxReportPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            TaxReportKeys keys = entityKeys as TaxReportKeys;
            TaxReportLineQueryService lineQueryService = new TaxReportLineQueryService(context);
            entityPM.TaxReportLines = lineQueryService.GetMulti(keys, true);

            base.GetComposition(entityKeys, entityPM);
        }




    }
}
