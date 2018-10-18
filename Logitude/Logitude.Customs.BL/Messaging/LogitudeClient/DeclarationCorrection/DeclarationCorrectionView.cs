using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection
{
   public class DeclarationCorrectionView
    {
       [Key]
       public string Id { get; set; }

       [Key]
       public string DeclarationId { get; set; }
       public List<GeneralDataView> GeneralDataViews { get; set; }

      




    }
}
