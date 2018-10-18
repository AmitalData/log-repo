using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationPaymentQueryService
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, DeclarationPaymentPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationPaymentKeys declarationPaymentKeys = entityKeys as DeclarationPaymentKeys;

            DeclarationPaymentMethodQueryService declarationPaymentMethodQueryService = new DeclarationPaymentMethodQueryService(context);
            entityPM.DeclarationPaymentMethods = declarationPaymentMethodQueryService.GetMulti(declarationPaymentKeys, false);


            DeclarationPaymentProtestQueryService declarationPaymentProtestQueryService = new DeclarationPaymentProtestQueryService(context);
            entityPM.DeclarationPaymentProtests = declarationPaymentProtestQueryService.GetMulti(declarationPaymentKeys, false);

            if (entityPM.DeclarationPaymentMethods.Count > 0)
            {
                entityPM.PaymentMethodLastLineNumber = entityPM.DeclarationPaymentMethods.Max(m => m.Line);
            }

            if (entityPM.DeclarationPaymentProtests.Count > 0)
            {
                entityPM.PaymentProtestLastLineNumber = entityPM.DeclarationPaymentProtests.Max(m => m.Line);
            }

            base.GetComposition(entityKeys, entityPM);
        }
    }
}
