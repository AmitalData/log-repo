using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt : SYSTBL_NG_9001_MSG_SystemTablesResponseTableData
    {
        public GovernmentProcedureType MyGovernmentProcedureType { get; set; }
        public InternationalSiteP MyInternationalSite { get; set; }
        public ModificationAndDiscountType MyModificationAndDiscountType { get; set; }

        internal static SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt CreateNew(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData item)
        {
            return new SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt()
            {
                extraNumericData = item.extraNumericData,
                extraNumericDataSpecified = item.extraNumericDataSpecified,
                extraStringData = item.extraStringData,
                id = item.id,
                malamID = item.malamID,
                name = item.name,
                state = item.state,
                stateSpecified = item.stateSpecified,
                updateDate = item.updateDate,
                updateDateSpecified = item.updateDateSpecified

            };
        }
    }
    public class GovernmentProcedureType
    {
        public bool IsImport { get; set; }
        public bool IsExport { get; set; }

    }
    public class InternationalSiteP
    {
        public string CountryTypeCode { get; set; }
    }
    public class ModificationAndDiscountType
    {
        public Boolean IsRelevantGoodsItem { get; set; }
        public Boolean IsRelevantInvoice { get; set; }
    }
}
