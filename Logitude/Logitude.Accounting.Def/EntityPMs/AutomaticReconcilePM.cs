using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public partial class AutomaticReconcilePM
    {
        

        /*
C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(152):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "1", EnglishName = "Open Amount", LocalName = "סכום פתוח" }, automaticReconcileRepository);
  C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(153):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "2", EnglishName = "Reference Date", LocalName = "תאריך אסמכתא" }, automaticReconcileRepository);
  C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(154):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "3", EnglishName = "Due Date", LocalName = "תאריך ערך" }, automaticReconcileRepository);
  C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(155):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "4", EnglishName = "Accounting Date", LocalName = "תאריך חשבונאי" }, automaticReconcileRepository);
  C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(156):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "5", EnglishName = "Reference 1", LocalName = "אסמכתא 1" }, automaticReconcileRepository);
  C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(157):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "6", EnglishName = "Reference 2", LocalName = "אסמכתא 2" }, automaticReconcileRepository);
  C:\Source\Accounting\JustWebFreight\WebFreight.Web\MetaDataUpdate\UpdateClasses\AccountingUpdate.cs(158):            AddClosedTables.AddAutomaticReconcile(new AutomaticReconcileDetails() { Code = "7", EnglishName = "Reference 3", LocalName = "אסמכתא 3" }, automaticReconcileRepository);
           * 
         */

        public enum AutomaticReconcileEnum
        {
            none=0,
            OpenAmountABS=1,
            ReferenceDate=2,
            DueDate=3,
            AccountingDate=4,
            Reference1=5,
            Reference2 = 6,
            Reference3 = 7,
            FIFOAccountingDate = 8,
            FIFODueDate = 9

    }
    }
}
