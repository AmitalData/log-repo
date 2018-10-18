using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public partial class RevenueExpenseTypePM
    {
        //RevenueExpenseTypeRepository revenueExpenseRepository = new RevenueExpenseTypeRepository(accountingContext);
        //    AddClosedTables.AddRevenueExpenseType(new RevenueExpenseTypeDetails() { Code = "1", EnglishName = "Revenue", LocalName = "הכנסות" }, revenueExpenseRepository);
        //    AddClosedTables.AddRevenueExpenseType(new RevenueExpenseTypeDetails() { Code = "2", EnglishName = "Expense", LocalName = "הוצאות" }, revenueExpenseRepository);
        //    AddClosedTables.AddRevenueExpenseType(new RevenueExpenseTypeDetails() { Code = "3", EnglishName = "Other", LocalName = "אחר" }, revenueExpenseRepository);
        public enum RevenueExpenseTypeEnum
        {
            Revenue=1,
            Expense=2,
            Other=3

        }
    }
}
