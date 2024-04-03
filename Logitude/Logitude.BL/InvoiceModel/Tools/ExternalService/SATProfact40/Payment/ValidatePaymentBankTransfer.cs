using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class ValidatePaymentBankTransfer
    {
        public static bool IsValid(ARPaymentPM entityPM)
        {
            if (entityPM.SATPaymentMethodCode != SATData.LogitudeSATPaymentBankTransferMethod || entityPM.TipoCadenaPago != SATData.SATPaymentBankTransferTypeCode) 
                return false;
            
            ValidatePagosPago(entityPM);
            
            return true;
        }

        private static void ValidatePagosPago(ARPaymentPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.CertPago))
                throw new ApplicationException("Cert Pago is required");

            if (string.IsNullOrEmpty(entityPM.SelloPago))
                throw new ApplicationException("Sello Pago is required");

            if (string.IsNullOrEmpty(entityPM.CadPago))
                throw new ApplicationException("Cad Pago is required");
        }
    }
}
