using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
  public  class CB_NG_8314_8888_CustomsItemDetailsResponseData : ResponseDataBase
    {

      public  List<CustomsItem> CustomsItemList { get; set; }
    //  public Exception[] Exception { get; set; }

      
    }

    public class CustomsItem
    {
        public string fullClassification { get; set;}
        public string statisticMeasurementUnitExternalID { get; set;}
        public bool?  isDiscountCode { get; set;}
        public string  goodsDescription { get; set;}
        public string customsBookTypeName { get; set;}
  

    }
}
