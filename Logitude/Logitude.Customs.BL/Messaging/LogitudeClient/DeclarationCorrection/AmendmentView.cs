using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection
{
  public  class AmendmentView
    {



   
   
      public int? ParentLine { get; set; }
      public string ParentEntityName { get; set; }
   
     
      public string ErrorType { get; set; }
      public string Field { get; set; }
      public int? Line { get; set; }
      public string EntityName { get; set; }
  
      public string FieldNameTextCode { get; set; }
      public string TableNameTextCode { get; set; }

  
      public string LineNumber { get; set; }

      public string Code { get; set; }

      public string MessageError { get; set; }
      public string OldValue { get; set; }
      public string NewValue { get; set; }
      public List<field> FieldErrors { get; set; }
      
    }
}
