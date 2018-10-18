using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class SuccessMobile
    {
         public string ExceptionMessage { get; set; }
         public bool IsScceed { get; set; }
         public string ObjectTableId  { get; set; }
         public string DocumentTypeId  { get; set; }
         public string ShipmentId  { get; set; }
         public string UserId { get; set; }
         public string DocumentsFilingId { get; set; }
         public string DocumentId { get; set; }
         public bool IsCheckedSecuritykey { get; set; }
         public string ExceptionTitle { get; set; }
        
    }
}