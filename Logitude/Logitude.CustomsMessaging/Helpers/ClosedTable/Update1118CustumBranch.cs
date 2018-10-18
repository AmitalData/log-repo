using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class Update1118CustumBranch : ClosedTableGenericService<CustomsBranchPM>
    {
        public Update1118CustumBranch(ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<CustomsBranchPM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<CustomsBranchPM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override string GetPMKey(CustomsBranchPM rec)
        {
            if (rec.Id == "129,31")
            {
            }
            return rec.Id;
        }
        protected override string GetPMKeyFromCustomRow(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow)
        {
            if (!mehesTableRow.extraNumericData.HasValue)
            {
                throw new System.Exception("!mehesTableRow.extraNumericData.HasValue id=" + mehesTableRow.id);
            }
            return mehesTableRow.id + "," + mehesTableRow.extraNumericData.ToString();
        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CustomsBranchPM curDbPM)
        {
            curDbPM.Inactive = TranslateMehesInactive(mehesTableRow);
            curDbPM.LocalName = mehesTableRow.name;
            curDbPM.SearchFields = GetPMKeyFromCustomRow(mehesTableRow) + "," + mehesTableRow.name;
            curDbPM.Code = mehesTableRow.id;
            curDbPM.BankCode = mehesTableRow.extraNumericData.GetValueOrDefault().ToString();
        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CustomsBranchPM curDbPM)
        {

            string bk = mehesTableRow.extraNumericData.Value.ToString();
            return curDbPM.LocalName == mehesTableRow.name &&
                            curDbPM.LocalName == mehesTableRow.name &&

                            curDbPM.Code == mehesTableRow.id &&

                            curDbPM.BankCode == bk &&

                            curDbPM.Inactive == TranslateMehesInactive(mehesTableRow)
            ;
        }

        protected override void SetPMKey(CustomsBranchPM curDbPM, SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow)
        {
            curDbPM.Id = GetPMKeyFromCustomRow(mehesTableRow);

        }
        protected override List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> MakeUniqeList(List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows)
        {
            return (new ClosedTableUniqeList1118Service()).MakeUniqeList(mehesTableRows);
        }
    }
}
