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

        protected override void AfterUpdating(DeclarationPaymentMethodPM entityPM,DeclarationPaymentPM entityParentPM)
        {
            //if (entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            //{
            //    SubmitChanges();
            //    ICustomContext context = MainContext as CustomContext;
            //    DeclarationPaymentMethodRepository declarationPaymentMethodRepository = new DeclarationPaymentMethodRepository(context);
            //    List<DeclarationPaymentMethod> declarationPaymentMethods = declarationPaymentMethodRepository.GetMulti(new DeclarationPaymentKeys() { DeclarationId = entityPM.DeclarationId });
            //    declarationPaymentMethods = declarationPaymentMethods.OrderBy(d => d.Line).ToList();
            //    int index = 0;
            //    foreach (DeclarationPaymentMethod item in declarationPaymentMethods)
            //    {
            //        index += 1;
            //        item.SequenceNumeric = index;
            //        declarationPaymentMethodRepository.Update(item);
            //        if (item.DeclarationId == entityPM.DeclarationId && item.Line == entityPM.Line)
            //        {
            //            entityPM.SequenceNumeric = item.SequenceNumeric;
            //        }
            //    }
            //    declarationPaymentMethodRepository.SubmitChanges();
            //}
            
        }
    }
}
