using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CloseTables
{

    public struct IsActiveEnum
	{
		public const string NotActive = "1";
		public const string ActiveAllCustomers = "2";
		public const string ActiveSelectedCustomers = "3";
    }
	public struct TypesDebtsEnum
	{
		public const string Obligato = "1";
		public const string AccountingBalance = "2";
		public const string BalanceRegarding = "3";
	}
	public struct DebtLevelEnum
	{
		public const string Percentage = "1";
		public const string TotalAmount = "2";
	}

}
