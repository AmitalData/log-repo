using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATInvoiceComprobanteValidator
    {
        private ARInvoicePM arInvoicePM;
        private BranchRepository branchRepository;
        private AddressRepository addressReposirory;
        private CardRepository cardRepository;
        private MeasurementRepository measurementRepository;
        private ComputingPartnerTranslationHelper computingPartnerHelper;
        private ICommonDataContext commonContext;
        private Tenant currentTenant;
        private Card billToCard;
        public SATInvoiceComprobanteValidator(ARInvoicePM arInvoicePM, Tenant currentTenant)
        {
            this.arInvoicePM = arInvoicePM;
            this.currentTenant = currentTenant;
            InitalizeContexts(arInvoicePM.Tenant);
            InitalizeRepositories(arInvoicePM.Tenant);
        }

        private void InitalizeContexts(int tenant)
        {
            commonContext = CommonDataContext.GetContext(tenant);
        }

        private void InitalizeRepositories(int tenant)
        {
            computingPartnerHelper = new ComputingPartnerTranslationHelper(tenant);
            addressReposirory = new AddressRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            branchRepository = new BranchRepository(commonContext);
            measurementRepository = new MeasurementRepository(commonContext);
        }

        public void Validate(List<ChargesType> allChargesTypes)
        {
            ValidateBranchAddress();
            ValidateCompanyVatNumber();
            ValidateCompanyName();
            ValidateARInvoiceDate();
            ValidateFormaPago();
            ValidateMetodoPago();
            ValidateBillToCard();
            ValidateInformacionGlobal();
            ValidateBillToAddress();
            ValidateARInvoiceLines(allChargesTypes);
        }

        private void ValidateBranchAddress()
        {
            Address branchAddress = null;

            if (!string.IsNullOrEmpty(arInvoicePM.BranchId))
            {
                branchAddress = GetBranchAddress(branchAddress);
            }

            if (branchAddress != null && string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                throw new ApplicationException("Branch Address ZipCode is required ");
            }
            else if ((branchAddress == null))
            {
                ValidateCurrentTenantAddress();
            }
        }

        private Address GetBranchAddress(Address branchAddress)
        {
            Branch branch = branchRepository.GetSingleBranch(arInvoicePM.BranchId, arInvoicePM.Tenant);
            if (!string.IsNullOrEmpty(branch.AddressId))
            {
                branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
            }

            return branchAddress;
        }

        private void ValidateCurrentTenantAddress()
        {
            if (currentTenant.Address != null && string.IsNullOrEmpty(currentTenant.Address.ZipCode))
            {
                throw new ApplicationException("Company Address ZipCode is required ");
            }
            else if (currentTenant.Address == null)
            {
                throw new ApplicationException("Company Address is required ");
            }
        }

        private void ValidateCompanyVatNumber()
        {
            if (string.IsNullOrEmpty(currentTenant.VatNumber))
            {
                throw new ApplicationException("Company Vat Number is required");
            }
        }

        private void ValidateCompanyName()
        {
            if (string.IsNullOrEmpty(currentTenant.Company))
            {
                throw new ApplicationException("Company Name is required");
            }
        }

        private void ValidateARInvoiceDate()
        {
            if (arInvoicePM.InvoiceDate == null)
            {
                throw new ApplicationException("Invoice Date is required");
            }
        }

        private void ValidateFormaPago()
        {
            if (string.IsNullOrEmpty(arInvoicePM.SATPaymentMethodCode))
            {
                throw new ApplicationException("Forma Pago is required");
            }
        }

        private void ValidateMetodoPago()
        {
            if (string.IsNullOrEmpty(arInvoicePM.MetodoPagoCode))
            {
                throw new ApplicationException("Metodo Pago is required ");
            }
        }

        private void ValidateBillToCard()
        {
            billToCard = cardRepository.GetSingleCard(arInvoicePM.BillToId, arInvoicePM.Tenant);
            if (string.IsNullOrEmpty(billToCard.EnglishName))
            {
                throw new ApplicationException("Bill to Name is required");
            }

            if (string.IsNullOrEmpty(arInvoicePM.RegimenFiscalCode) && string.IsNullOrEmpty(billToCard.RegimenFiscalCode))
            {
                throw new ApplicationException("Regimen Fiscal is required ");
            }
        }

        private void ValidateInformacionGlobal()
        {
            if (!arInvoicePM.IsConsolidationInvoice) return;

            if (string.IsNullOrEmpty(arInvoicePM.PeriodCode))
            {
                throw new ApplicationException("Period is required ");
            }
            string regimenFiscalCode = arInvoicePM.RegimenFiscalCode;
            if (string.IsNullOrEmpty(regimenFiscalCode) && string.IsNullOrEmpty(billToCard.RegimenFiscalCode))
                regimenFiscalCode = billToCard.RegimenFiscalCode;

            if (arInvoicePM.PeriodCode == SATData.BimestralPeriod && regimenFiscalCode != SATData.IncorporacionFiscalRegimen)
            {
                throw new ApplicationException("Regimen Fiscal must be equal to Incorporación Fiscal");
            }

            int invoiceDateYear = ((DateTime)arInvoicePM.InvoiceDate).Year;
            int currentDateYear = DateTime.Now.Year;
            if (invoiceDateYear != currentDateYear && invoiceDateYear != currentDateYear + 1)
            {
                throw new ApplicationException("Invoice Date Year must be equal to the current year or the immediately preceding year");
            }
        }

        private void ValidateBillToAddress()
        {
            Address billToAddress = GetBillToAddress();
            string billToAddressZipCode = "";
            if (billToAddress != null && !string.IsNullOrEmpty(billToAddress.ZipCode))
            {
                billToAddressZipCode = GetBillToAddressZipCode(billToAddress);
            }

            if (string.IsNullOrEmpty(billToAddressZipCode))
            {
                throw new ApplicationException("Bill To Address Zip Code is required");
            }
        }

        private Address GetBillToAddress()
        {
            if (!string.IsNullOrEmpty(arInvoicePM.BillToAddressId))
            {
                return addressReposirory.GetSingleAddress(arInvoicePM.BillToAddressId, arInvoicePM.Tenant);
            }
            else
                throw new ApplicationException("Bill to Address is required ");
        }

        private string GetBillToAddressZipCode(Address billToAddress)
        {
            PostalCodeQuery postalCodeQuery = new PostalCodeQuery(arInvoicePM.Tenant);
            PostalCodePM postalCodePM = postalCodeQuery.GetSinglePM(billToAddress.ZipCode);
            if (postalCodePM == null)
            {
                throw new ApplicationException("Bill To Address Zip Code is not valid");
            }
            else
            {
                return billToAddress.ZipCode;
            }
        }

        private void ValidateARInvoiceLines(List<ChargesType> allChargesTypes)
        {
            List<Measurement> allMeasurements = measurementRepository.GetMeasurements(arInvoicePM.Tenant).ToList();
            foreach (ARInvoiceLinePM line in arInvoicePM.InvoiceLines)
            {
                if (!allChargesTypes.First(c => c.Id == line.ChargesTypeId).IsExpense)
                {
                    ValidateARInvoiceLine(allChargesTypes, allMeasurements, line);
                }
            }
        }

        private void ValidateARInvoiceLine(List<ChargesType> allChargesTypes, List<Measurement> allMeasurements, ARInvoiceLinePM line)
        {
            string claveProdServ = allChargesTypes.FirstOrDefault(c => c.Id == line.ChargesTypeId).SATExternalId;
            var lineMeasurement = allMeasurements.FirstOrDefault(m => m.Id == line.MeasurementId);

            if (lineMeasurement != null && string.IsNullOrEmpty(computingPartnerHelper.GetComputingPartnerCodeTranslation(lineMeasurement.Code, "G-Profact", "Measurement")))
            {
                throw new Exception("Measurement Code is required");
            }

            if (string.IsNullOrEmpty(claveProdServ))
            {
                throw new Exception("Measurement on charge type is required");
            }

            if (string.IsNullOrEmpty(claveProdServ))
            {
                throw new Exception("SAT External Id on charge type is required");
            }
        }
    }
}
