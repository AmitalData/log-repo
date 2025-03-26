using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationPaymentMethodUpdateService
    {

        protected override void OnCreating(DeclarationPaymentMethodPM entityPM, DeclarationPaymentPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
      

            entityParentPM.PaymentMethodLastLineNumber += 1;
            entityPM.Line = entityParentPM.PaymentMethodLastLineNumber;
        }
        protected override void OnUpdating(DeclarationPaymentMethodPM entityPM)
        {
            if(entityPM.InternalBankId == "")
            {
                entityPM.InternalBankId = null;
            }
            base.OnUpdating(entityPM);
        }

        protected override void AfterUpdating(DeclarationPaymentMethodPM entityPM,DeclarationPaymentPM entityParentPM)
        {
            
        }
    }
}
