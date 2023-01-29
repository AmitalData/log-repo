using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
   public class CD_NG_8314_Web01_CustomsItemDetailsRequestParams: RequestParamsBase
    {
        DateTime _ValidToDate;

        public DateTime ValidToDate
        {
            get { return _ValidToDate; }
            set { _ValidToDate = value; FirePropertyChanged("ValidToDate"); }
        }

        string  _Classification;

        public string Classification
        {
            get { return _Classification; }
            set { _Classification = value; FirePropertyChanged("Classification"); }
        }

        int _CustomsBookType;

        public int CustomsBookType
        {
            get { return _CustomsBookType; }
            set { _CustomsBookType = value; FirePropertyChanged("CustomsBookType"); }
        }
        //public Boolean UpdateAllTenants { get; set; }

        /////public int Tenant { get; set; }
    }
}
