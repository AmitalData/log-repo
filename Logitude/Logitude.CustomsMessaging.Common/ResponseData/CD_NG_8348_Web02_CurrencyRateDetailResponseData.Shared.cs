using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
  public  class CD_NG_8348_Web02_CurrencyRateDetailResponseData: ResponseDataBase
    {

      public  List<CurrencyRateResult> CurrencyRateList { get; set; }
    //  public Exception[] Exception { get; set; }

      
    }

    public class CurrencyRateResult
    {
        public string CurrencyTypeId {get; set;}
        public string CurrencyTypeName {get; set;}
        public decimal? CustomsCurrencyRate {get; set;}
        public DateTime? EndDate {get; set;}
        public DateTime? StartDate {get; set;}
  //      public string Id { get; set; }
        public int Tenant { get; set; }
       // public DateTime? RateDate { get; set; }
      

    }
}
