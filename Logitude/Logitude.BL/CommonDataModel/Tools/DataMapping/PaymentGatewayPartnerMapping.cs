using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    class PaymentGatewayPartnerMapping
    {


        public static void MapEntity(PaymentGatewayPartnerPM paymentGatewayPartnersPM, PaymentGatewayPartner paymentGatewayPartners, bool isNewState)
        {
            if (isNewState)
            {
                paymentGatewayPartners.Code = paymentGatewayPartnersPM.Code;


            }

        }
    }
}
