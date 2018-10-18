using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ARInvoiceDepositDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public string Currency { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<ARPaymentDataProvider> ARPaymentDataList { get; set; }
    }


  
}