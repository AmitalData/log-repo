using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class TaxDeductionReportStatusQueryService : EntityQueryService<TaxDeductionReportStatus, TaxDeductionReportStatusKeys, TaxDeductionReportStatusPM, object, TaxDeductionReportStatusKeys>
    {


        public string GetSingleEnglishNameByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return String.Empty;

            return repository.GetSingleEnglishNameByCode(code);
        }

    } 
}
