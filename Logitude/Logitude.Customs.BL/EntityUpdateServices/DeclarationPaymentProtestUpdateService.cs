using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class DeclarationPaymentProtestUpdateService
    {


       protected override void OnCreating(DeclarationPaymentProtestPM entityPM, DeclarationPaymentPM entityParentPM)
       {
           entityPM.DeclarationId = entityParentPM.DeclarationId;


           entityParentPM.PaymentProtestLastLineNumber += 1;
           entityPM.Line = entityParentPM.PaymentProtestLastLineNumber;
       }


    }
}
