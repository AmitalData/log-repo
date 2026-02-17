
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class PaymentTermValidating
    {
        public static void Validate(EntityPMs.PaymentTermPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                PaymentTermRepository entityRepository = new PaymentTermRepository(entityPM.Tenant);
                PaymentTerm paymentTerm = entityRepository.GetSinglePaymentTermByCode(entityPM.Code, entityPM.Tenant);

                if (paymentTerm != null)
                {
                    if (paymentTerm.Id != entityPM.Id)
                    {
                        throw new ApplicationException("Payment Term with Code " + entityPM.Code + " already exists");
                    }
                }
            }
        }
    }
}