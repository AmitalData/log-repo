using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CollateralRequestParams : RequestParamsBase
    {
        public string CustomCollateralId { get; set; }
        public List<CustomsCollateralsAnswerParams> CustomsCollateralsAnswers { get; set; }
    }
    public class CustomsCollateralsAnswerParams
    {
        public string CustomsCollateralId { get; set; }
        public int LineNumber { get; set; }
        public int Tenant { get; set; }
        public string AnswerEntityTypeCode { get; set; }
        public decimal? AllocatedAmount { get; set; }
        public string Remarks { get; set; }
        public string CustomsTapgFile { get; set; }
        public string CustomsNumeral { get; set; }
        public string AnswerForCollateralStatusCode { get; set; }
        public string Errors { get; set; }
        public string AnswerEntityType { get; set; }
        public string AnswerForCollateralStatus { get; set; }
    }
}
