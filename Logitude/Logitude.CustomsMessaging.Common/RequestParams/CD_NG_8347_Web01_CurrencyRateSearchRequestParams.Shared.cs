using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
   public class CD_NG_8347_Web01_CurrencyRateSearchRequestParams: RequestParamsBase
    {
        DateTime? _FromDate;

        public DateTime? FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; FirePropertyChanged("FromDate"); }
        }

        DateTime? _ToDate;

        public DateTime? ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; FirePropertyChanged("ToDate"); }
        }

        string _CurrencyTypeId;

        public string CurrencyTypeId
        {
            get { return _CurrencyTypeId; }
            set { _CurrencyTypeId = value; FirePropertyChanged("CurrencyTypeId"); }
        }
       ///public int Tenant { get; set; }
    }
}
