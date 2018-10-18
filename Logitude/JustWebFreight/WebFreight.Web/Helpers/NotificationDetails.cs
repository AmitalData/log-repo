using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class NotificationDetails
    {
     
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string Message { get; set; }
        public DateTime CreateData { get; set; }
        public string EntitiyId { get; set; }
        public string NotificationId { get; set; }
        public int Tenant { get; set; }
        public bool IsRead { get; set; }
        public bool IsException { get; set; }
        public bool IsNewDesignNotification { get; set; }
        public string ForeignPartnerCountryCode { get; set; }
        public string Supplier { get; set; }
        public string Route { get; set; }
        public string Refernce { get; set; }
        public string Forwarder { get; set; }
        public string StatusName { get; set; }
        public string ExceptionNote { get; set; }
        

        //public string FromCountryCode { get; set; }
        //public string ToCountryCode { get; set; }
        //public string Row0Colum0 { get; set; }
        //public string Row0Colum1 { get; set; }
        //public string Row1Colum0 { get; set; }
        //public string Row1Colum1 { get; set; }
        //public string Row2Colum0 { get; set; }
        //public string Row2Colum1 { get; set; }
        //public string Row3Colum0 { get; set; }
        //public string Row3Colum1 { get; set; }
    }
}