using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class Receptor : SATComprobante
    {
        public static ComprobanteReceptor Get()
        {
            ComprobanteReceptor comprobanteReceptor = new ComprobanteReceptor();
            CardRepository cardRepository = new CardRepository(commonContext);
            Card billToCard = cardRepository.GetSingleCard(arPaymentPM.BillToId, arPaymentPM.Tenant);
            string billToAddressZipCode = GetbillToAddressZipCode(arPaymentPM);
            const string electronicPaymentReceiptCode = "CP01";

            comprobanteReceptor.RegimenFiscalReceptor = billToCard.RegimenFiscalCode;
            comprobanteReceptor.DomicilioFiscalReceptor = billToAddressZipCode;
            comprobanteReceptor.Nombre = GetSATCustomerName(billToCard);
            comprobanteReceptor.UsoCFDI = electronicPaymentReceiptCode;


            string billToCountryCode = SATBaseProfact40Service.GetBillToCountryCode(arPaymentPM.BillToAddressId, arPaymentPM.Tenant, commonContext);
            if (IsBillToMexicoCountry(billToCountryCode))
            {
                comprobanteReceptor.Rfc = GetMexicoReceptorRfc(billToCard);
                return comprobanteReceptor;
            }

            comprobanteReceptor.Rfc = !string.IsNullOrEmpty(billToCard.SATForeignRFC) ? billToCard.SATForeignRFC : SATData.OutSideMexicoRfc;
            comprobanteReceptor.ResidenciaFiscal = billToCountryCode;
            comprobanteReceptor.ResidenciaFiscalSpecified = true;

            return comprobanteReceptor;
        }

        private static string GetSATCustomerName(Card billToCard)
        {
            if (!String.IsNullOrEmpty(billToCard.SATCustomerName))
                return billToCard.SATCustomerName;

            return billToCard.EnglishName;
        }

        private static string GetMexicoReceptorRfc(Card billToCard)
        {
            return !string.IsNullOrEmpty(billToCard.VatNumber) ? billToCard.VatNumber : SATData.MexicoRfc;
        }

        private static bool IsBillToMexicoCountry(string billToCountryCode)
        {
            const string mexicoCountryCode = "MEX";
            const string additionalMexicoCountryCode = "MX";

            return billToCountryCode == mexicoCountryCode || billToCountryCode == additionalMexicoCountryCode;
        }

        private static string GetbillToAddressZipCode(ARPaymentPM arPaymentPM)
        {
            Address billToAddress = null;
            if (!string.IsNullOrEmpty(arPaymentPM.BillToAddressId))
            {
                billToAddress = GetBillToAddress(arPaymentPM);
            }
            if (billToAddress != null && !string.IsNullOrEmpty(billToAddress.ZipCode))
            {
                return SATBaseProfact40Service.GetBillToAddressZipCode(billToAddress, arPaymentPM.Tenant);
            }

            return null;
        }

        private static Address GetBillToAddress(ARPaymentPM arPaymentPM)
        {
            AddressRepository addressRepository = new AddressRepository(commonContext);
            Address billToAddress = addressRepository.GetSingleAddress(arPaymentPM.BillToAddressId, arPaymentPM.Tenant);
            
            return billToAddress;
        }
    }
}
