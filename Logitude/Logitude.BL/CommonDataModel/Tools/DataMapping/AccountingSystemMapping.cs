using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AccountingSystemMapping
    {
        public static void MapEntity(AccountingSystemPM entityPM, AccountingSystem poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                
            }

            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name;
            poco.IsExternalCodesFromTable = entityPM.IsExternalCodesFromTable;
            poco.IsExternalCodesFromAPI = entityPM.IsExternalCodesFromAPI;
            poco.IsExternalCodesSyncEnabled = entityPM.IsExternalCodesSyncEnabled;
            poco.IsSingleTaxPerInvoice = entityPM.IsSingleTaxPerInvoice;
            poco.IsSingleCurrencyAccount = entityPM.IsSingleCurrencyAccount;
            poco.AllowManuallyDueDate = entityPM.AllowManuallyDueDate;
            poco.IsJournalMode = entityPM.IsJournalMode;
            poco.IsTaxItemManaged = entityPM.IsTaxItemManaged;
            poco.AllowMinusInvoiceLines = entityPM.AllowMinusInvoiceLines;
            poco.ShowDownloadScreen = entityPM.ShowDownloadScreen;
            poco.AllowARInvoicesTransfer = entityPM.AllowARInvoicesTransfer;
            poco.AllowAPInvoicesTransfer = entityPM.AllowAPInvoicesTransfer;
            poco.AllowPositiveAmountsInTheCreditNote = entityPM.AllowPositiveAmountsInTheCreditNote;
            poco.InActive = entityPM.InActive;
            poco.AllowARPaymentsTransfer = entityPM.AllowARPaymentsTransfer;
            poco.SearchFields = entityPM.Code + "," + entityPM.Name;
            poco.CanTransferToDropbox = entityPM.CanTransferToDropbox;
            poco.AllowAPPaymentsTransfer = entityPM.AllowAPPaymentsTransfer;
        }
    }
}
