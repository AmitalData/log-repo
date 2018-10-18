using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class ClosedTableUniqeList1118Service : ClosedTableUniqeListGenericService
    {
        protected override void ManipulateRow(UnifreightIIG.Common.SystemTableServiceReference.SYSTBL_NG_9001_MSG_SystemTablesResponseTableData rec)
        {
            base.ManipulateRow(rec);
        }
        protected override string RealKey(UnifreightIIG.Common.SystemTableServiceReference.SYSTBL_NG_9001_MSG_SystemTablesResponseTableData rec)
        {
            if (!rec.extraNumericData.HasValue)
            {
                throw new System.Exception("ClosedTableUniqeList1118Service :!rec.extraNumericData.HasValue " + rec.id); 
            }
            return rec.id.ToUpper() + "," + rec.extraNumericData.Value.ToString().ToUpper();
        }
    }
}
