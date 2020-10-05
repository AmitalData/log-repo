

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

        public static string getEntityIcon(string _sourceTypeCode)
        {

            string iconTxt = "";
            switch (_sourceTypeCode)
            {
                // 1-Journal
                case "1":
                    {
                        iconTxt = "JR";
                        break;
                    }

                // 2-ARInvoice
                case "2":
                    {
                        iconTxt = "IN";
                        break;
                    }

                // 3-ARPayment
                case "3":
                    {
                        iconTxt = "PY";
                        break;
                    }

                // 4-APInvoice
                case "4":
                    {
                        iconTxt = "IN";
                        break;
                    }

                // 5-APPayment
                case "5":
                    {
                        iconTxt = "PY";

                        break;
                    }

                // 6-Cheque Deposit
                case "6":
                    {
                        iconTxt = "DP";

                        break;
                    }

                // 7-Cash Deposit
                case "7":
                    {
                        iconTxt = "DP";

                        break;
                    }

                // 8-Revaluation
                case "8":
                    {
                        iconTxt = "RV";

                        break;
                    }

                // 9-PaymentCheque
                case "9":
                    {
                        iconTxt = "CH";

                        break;
                    }

                // 10-Adjustment
                case "10":
                    {
                        iconTxt = "AJ";

                        break;
                    }
            }
            return iconTxt;
        }
    }


   

}