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
    public class Update1416ModificationAndDiscountType : ClosedTableGenericService<ModificationAndDiscountTypePM>
    {
        public Update1416ModificationAndDiscountType(ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<ModificationAndDiscountTypePM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<ModificationAndDiscountTypePM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ModificationAndDiscountTypePM curDbPM)
        {
            var gov = mehesTableRow.MyModificationAndDiscountType ?? new ModificationAndDiscountType();
            return base.IsEqual(mehesTableRow, curDbPM) && curDbPM.IsRelevantGoodsItem == gov.IsRelevantGoodsItem && gov.IsRelevantInvoice == curDbPM.IsRelevantInvoice && curDbPM.IsRelevantGoodsItemExport == gov.IsRelevantGoodsItemExport && gov.IsRelevantInvoiceExport == curDbPM.IsRelevantInvoiceExport;
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ModificationAndDiscountTypePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyModificationAndDiscountType ?? new ModificationAndDiscountType();
            curDbPM.IsRelevantGoodsItem = gov.IsRelevantGoodsItem;
            curDbPM.IsRelevantInvoice = gov.IsRelevantInvoice;
            curDbPM.IsRelevantInvoiceExport = gov.IsRelevantInvoiceExport;
            curDbPM.IsRelevantGoodsItemExport = gov.IsRelevantGoodsItemExport;


        }
    }
}

