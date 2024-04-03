using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Base;
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
        public static ComprobanteReceptor Get(Tenant currentTenant)
        {
            ComprobanteReceptor comprobanteReceptor = new ComprobanteReceptor();
            CardRepository cardRepository = new CardRepository(commonContext);
            Card billToCard = cardRepository.GetSingleCard(arPaymentPM.BillToId, arPaymentPM.Tenant);
            string billToAddressZipCode = GetbillToAddressZipCode(arPaymentPM);
            const string electronicPaymentReceiptCode = "CP01";
            string billToCountryCode = SATBaseProfact40Service.GetBillToCountryCode(arPaymentPM.BillToAddressId, arPaymentPM.Tenant, commonContext);
            bool isBillToMexicoCountry = IsBillToMexicoCountry(billToCountryCode);

            comprobanteReceptor.RegimenFiscalReceptor = billToCard.RegimenFiscalCode;
            comprobanteReceptor.DomicilioFiscalReceptor = billToAddressZipCode;
            comprobanteReceptor.Nombre = GetSATCustomerName(billToCard);
            comprobanteReceptor.UsoCFDI = electronicPaymentReceiptCode;


            
            if (isBillToMexicoCountry)
            {
                return GetMexicoRfcReceptor(comprobanteReceptor, billToCard, currentTenant);
            }

            comprobanteReceptor.Rfc = SATData.OutSideMexicoRfc;
            comprobanteReceptor.ResidenciaFiscal = billToCountryCode;
            comprobanteReceptor.ResidenciaFiscalSpecified = true;
            comprobanteReceptor.NumRegIdTrib = !string.IsNullOrEmpty(billToCard.SATForeignRFC) ? billToCard.SATForeignRFC : SATData.OutSideMexicoRfc;
            comprobanteReceptor.DomicilioFiscalReceptor = LugarExpedicion.Get(new LugarExpedicionArgs { CommonContext = commonContext, BranchId = arPaymentPM.BranchId, CurrentTenantZipCode = currentTenant.Address.ZipCode, Tenant = arPaymentPM.Tenant });
            const string regimenFiscalReceptor616Code = "616";
            comprobanteReceptor.RegimenFiscalReceptor = regimenFiscalReceptor616Code;

            return comprobanteReceptor;
        }

        private static ComprobanteReceptor GetMexicoRfcReceptor(ComprobanteReceptor comprobanteReceptor, Card billToCard, Tenant currentTenant)
        {
            string publicInGeneralMexicoRfc = GetPublicInGeneralMexicoRfc(billToCard);
            bool IsPublicInGeneral = publicInGeneralMexicoRfc == SATData.PublicInGeneralMexicoRfc;
            ComprobanteReceptor MexicoReceptor = comprobanteReceptor;
            MexicoReceptor.Rfc = GetMexicoReceptorRfc(billToCard, publicInGeneralMexicoRfc);
            if (!IsPublicInGeneral)
            {
                return MexicoReceptor;
            }
            const string regimenFiscalReceptor616Code = "616";
            MexicoReceptor.RegimenFiscalReceptor = regimenFiscalReceptor616Code;
            MexicoReceptor.DomicilioFiscalReceptor = LugarExpedicion.Get(new LugarExpedicionArgs { CommonContext = commonContext, BranchId = arPaymentPM.BranchId, CurrentTenantZipCode = currentTenant.Address.ZipCode, Tenant = arPaymentPM.Tenant });
            MexicoReceptor.Nombre = IsPublicInGeneral ? SATData.PublicInGeneralNombre : MexicoReceptor.Nombre;
            return MexicoReceptor;
        }

        private static string GetSATCustomerName(Card billToCard)
        {
            if (!String.IsNullOrEmpty(billToCard.SATCustomerName))
                return billToCard.SATCustomerName;

            return billToCard.EnglishName;
        }

        private static string GetMexicoReceptorRfc(Card billToCard, string publicInGeneralMexicoRfc)
        {
            return !string.IsNullOrEmpty(billToCard.VatNumber) ? billToCard.VatNumber : publicInGeneralMexicoRfc;
        }

        private static bool IsBillToMexicoCountry(string billToCountryCode)
        {
            const string mexicoCountryCode = "MEX";
            const string additionalMexicoCountryCode = "MX";

            return billToCountryCode == mexicoCountryCode || billToCountryCode == additionalMexicoCountryCode;
        }

        private static string GetPublicInGeneralMexicoRfc(Card billToCard)
        {
            return string.IsNullOrEmpty(billToCard.VatNumber) ? SATData.PublicInGeneralMexicoRfc : "";
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
