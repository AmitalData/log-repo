using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class SendALLCorrectRequestParams : RequestParamsBase
    {
        public string CourierMasterId { get; set; }
        public string HAWB { get; set; }
        public string CourierDeclarationStatusCode { get; set; }
        public List<string> Declarations { get; set; }

        public bool IsWorkSheetFromExcel { get; set; }
        public string SelectedBOLValue { get; set; }
        public string SelectedStatusValue { get; set; }
        public string SelectedAvailableValue { get; set; }
        public string SelectedTotalInvoiceValue { get; set; }
        public string SelectedFastIndividualProcessValue { get; set; }
        public string SelectedCustomStatusValue { get; set; }
        public string SelectedFinalReleaseValue { get; set; }
    }
}
