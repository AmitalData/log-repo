using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class AddMultiPendingsRequestParams
    {
        public string[] listPending { get; set; }
        public string[] listPendingRemark { get; set; }
        public string[] declarationIdsList { get; set; }
        public string[] allWithoutdeclarationIdsList { get; set; }

        public string courierMasterId { get; set; }
        public bool checkboxAll { get; set; }
        public bool isCreateInvoiceDocument { get; set; }
    }
}
