using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
   public class PDFRerportXMLData
    {

        //public decimal? ARinvoiceTotalAmount { get; set; }

        //public decimal? ARinvoiceTotalRecords { get; set; }
        //public decimal? CreditARinvoiceTotalAmount { get; set; }
        //public decimal? CreditARinvoiceTotalRecords { get; set; }

        //public decimal? ARPaymentTotalAmount { get; set; }
        //public decimal? ARPaymentTotalRecords { get; set; }
        //public decimal? DepositTotalAmount { get; set; }
        //public decimal? DepositTotalRecords { get; set; }
        //public decimal? APinvoiceTotalAmount { get; set; }
        //public decimal? APinvoiceTotalRecords { get; set; }

       public List<OpenFormatTotalRecord> OpenFormatTotalRecords;
       public List<OpenFormatTotalAmounts> OpenFormatTotalAmounts;
    }

    public class OpenFormatTotalAmounts
    {
        public string RecordCode { get; set; }
        public string RecordType { get; set; }
        public int TotalRecords { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OpenFormatTotalRecord
    {
        public string RecordCode { get; set; }
        public int RecordTotal { get; set; }
        public string RecordDescreption { get; set; }
    }
}
