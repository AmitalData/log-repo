

namespace Logitude.Accounting.Data
{
    public class InterestEntities
    {
        public static string ARInvoice { get { return "1"; } } 
        public static string ARPayment { get { return "2"; } }
        public static string Journal { get { return "3"; } }
        public static string OpenBalance { get { return "4"; } }

    }

    public class AccountingEntities
    {
        public static string ARInvoice { get { return "2"; } }
        public static string ARPayment { get { return "3"; } }
        public static string Journal { get { return "1"; } }
    }

}