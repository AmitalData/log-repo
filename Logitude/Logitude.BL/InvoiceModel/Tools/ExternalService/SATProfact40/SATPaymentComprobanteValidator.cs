using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATPaymentComprobanteValidator
    {
        private ARPaymentPM arPaymentPM;
        private IInvoiceContext invoiceContext;
        private ICommonDataContext commonContext;
        private TenantRepository tenantRepository;
        private AddressRepository addressReposirory;
        private CardRepository cardRepository;
        private BranchRepository branchRepository;
        private Tenant currentTenant;
        public SATPaymentComprobanteValidator(ARPaymentPM arPaymentPM)
        {
            this.arPaymentPM = arPaymentPM;
            InitalizeContexts(arPaymentPM.Tenant);
            InitalizeRepositories();
            currentTenant = tenantRepository.GetSingleTenant(arPaymentPM.Tenant);
        }

        private void InitalizeContexts(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant); 
            commonContext = CommonDataContext.GetContext(tenant);
        }

        private void InitalizeRepositories()
        {
            tenantRepository = new TenantRepository(commonContext);
            addressReposirory = new AddressRepository(commonContext);
            cardRepository = new CardRepository(commonContext);
            branchRepository = new BranchRepository(commonContext);
        }

        public void Validate()
        {
            ValidateMetodoPago();
            ValidateCancellARPayment();
            ValidatePaymentARInvoicesApprovedBySAT();
            ValidateBillToAddress();
            ValidateCompanyVat(currentTenant);
            ValidateFormaPago();
            ValidateBranchAddress();
            ValidatePaymentMethodBankName();
        }

        private void ValidateMetodoPago()
        {
            if (arPaymentPM.MetodoPagoCode == "PUE")
            {
                throw new Exception("You can't proceed with this operation. You are allowed to send to SAT only when the Metodo Pago value is PPD");
            }
        }

        private void ValidateCancellARPayment()
        {
            if (arPaymentPM.SATTransferStatusCode == "TD" && (arPaymentPM.StatusCode == "VD"))
            {
                throw new Exception("You can't send this payment to SAT because it was cancelled");
            }
        }

        private void ValidatePaymentARInvoicesApprovedBySAT()
        {
            List<string> invoiceIds = arPaymentPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

            List<ARInvoice> paymentARInvoices = (from a in invoiceContext.ARInvoices
                                                 where a.Tenant == arPaymentPM.Tenant && invoiceIds.Contains(a.Id)
                                                 select a).ToList();

            if (paymentARInvoices.Any(p => string.IsNullOrEmpty(p.SATXML)))
            {
                throw new Exception("Some Invoices are not approved from sat");
            }
        }

        private void ValidateBillToAddress()
        {
            if (string.IsNullOrEmpty(arPaymentPM.BillToAddressId))
                throw new ApplicationException("Bill to Address is required ");
        }

        private static void ValidateCompanyVat(Tenant currentTenant)
        {
            if (string.IsNullOrEmpty(currentTenant.VatNumber))
            {
                throw new ApplicationException("Company Vat Number is required");
            }
        }

        private void ValidateFormaPago()
        {
            Card billToCard = cardRepository.GetSingleCard(arPaymentPM.BillToId, arPaymentPM.Tenant);
            if (string.IsNullOrEmpty(billToCard.SATPaymentMethodCode))
            {
                throw new ApplicationException("Bill to Forma Pago is required");
            }

            if (string.IsNullOrEmpty(arPaymentPM.SATPaymentMethodCode))
            {
                throw new ApplicationException("Forma Pago is required");
            }
        }

        private void ValidateBranchAddress()
        {
            Address branchAddress = null;
            if (!string.IsNullOrEmpty(arPaymentPM.BranchId))
            {
                branchAddress = GetBranchAddress(branchAddress);
            }

            if (branchAddress != null && string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                throw new ApplicationException("Branch Address ZipCode is required ");
            }
            else if (branchAddress != null)
            {
                ValidateCurrentTenantAddress();
            }
        }

        private Address GetBranchAddress(Address branchAddress)
        {
            Branch branch = branchRepository.GetSingleBranch(arPaymentPM.BranchId, arPaymentPM.Tenant);
            if (!string.IsNullOrEmpty(branch.AddressId))
            {
                branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
            }

            return branchAddress;
        }

        private void ValidateCurrentTenantAddress()
        {
            if (currentTenant.Address == null)
            {
                throw new ApplicationException("Company Address is required ");
            }
            if (string.IsNullOrEmpty(currentTenant.Address.ZipCode))
            {
                throw new ApplicationException("Company Address ZipCode is required ");
            }
        }

        private void ValidatePaymentMethodBankName()
        {
            if (ComplementoPagosPagoNomBancoOrdExt.IsPaymentMethodRequiredBankName(arPaymentPM) && string.IsNullOrEmpty(arPaymentPM.Bank))
            {
                throw new ApplicationException("Payment Bank Name is required");
            }
        }
    }
}
