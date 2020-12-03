using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Card = Simplog.Data.CommonDataModel.EntityPOCOs.Card;
using Currency = Simplog.Data.CommonDataModel.EntityPOCOs.Currency;
using PaymentTerm = Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PaymentTerm;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
    public partial class APInvoiceQueryService
    {
        private ICommonDataContext commonDataContext;
        private AccountingSettingRepository accountingSettingRepository;
        private UserRepository userRepository;
        private TenantRepository tenantRepository;
        private VatTypePercentageRepository vatTypePercentageRepository;
        private PaymentTermRepository paymentTermRepository;
        private MeasurementRepository measurementRepository;
        private PackageTypeRepository packageTypeRepository;
        private int tenant;
        private APInvoicePM aPInvoicePM;
        private string billToAccountingCard;
        private string currencyAccountingCard;
        private List<TransferStatusCodeItem> transferstatusCodes;
        private string defaultPaymentTermId;
        public APInvoicePM APInvoiceCustomDataMappingAndValidating(APInvoice MyEntity, int tenant, string ComputingPartnerCode = "", APInvoicePM RestClientAPIAPInvoice=null)
        {
            try
            {
                this.tenant = tenant;
                commonDataContext = CommonDataContext.GetContext(tenant);
                accountingSettingRepository = new AccountingSettingRepository(commonDataContext);
                userRepository = new UserRepository(commonDataContext);
                tenantRepository = new TenantRepository(commonDataContext);
                vatTypePercentageRepository = new VatTypePercentageRepository(commonDataContext);
                paymentTermRepository = new PaymentTermRepository(commonDataContext);
                measurementRepository = new MeasurementRepository(commonDataContext);
                packageTypeRepository = new PackageTypeRepository(commonDataContext);

                transferstatusCodes = new List<TransferStatusCodeItem>();

                string accountingSysytemCode = "";
                string payableVATCard = "";
                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(tenant);
                if (accountingSetting != null)
                {
                    accountingSysytemCode = accountingSetting.AccountingSystemCode;
                    payableVATCard = accountingSetting.PayableVATCard;
                }

                this.InitAPInvoice(MyEntity, ComputingPartnerCode, RestClientAPIAPInvoice);                
                this.InitAndValidateVendor();
                this.InitAndValidateGeneralData(accountingSetting);
                this.InitAndValidateInvoiceCurrency();
                this.InitAndValidateCurrencyRateData();
                this.InitAndValidateShipmentReference();
                this.InitAndValidatePaymentTerm_DueDate();
                this.InitAndValidateTotalVATsOnly();
                this.InitAndValidateInvoiceLines();
                this.FillVATTransferExternalCodes(accountingSysytemCode, payableVATCard);
                this.InitAndValidateTransferStatus();
                this.ComputeInvoiceAmounts();

                return aPInvoicePM;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitAPInvoice(APInvoice myEntity, string computingPartnerName, APInvoicePM RestClientAPIAPInvoice)
        {
            User myUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, false);
            Tenant myTenant = tenantRepository.GetSingleTenant(tenant);

            this.aPInvoicePM = RestClientAPIAPInvoice!=null? RestClientAPIAPInvoice: APInvoiceDataMappingAndValidatin(myEntity, tenant, computingPartnerName);
            aPInvoicePM.Tenant = tenant;
            aPInvoicePM.StatusCode = "AD";
            aPInvoicePM.CreatedByUserId = myUser.Id;
            aPInvoicePM.UpdatedByUserId = myUser.Id;
            aPInvoicePM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            aPInvoicePM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            this.defaultPaymentTermId = aPInvoicePM.PaymentTermId;

            if (string.IsNullOrEmpty(aPInvoicePM.BranchId))
            {
                aPInvoicePM.BranchId = myUser.BranchId;
            }

            if (string.IsNullOrEmpty(this.defaultPaymentTermId))
            {
                this.defaultPaymentTermId = myTenant.PaymentTermId;
            }

            if (string.IsNullOrEmpty(aPInvoicePM.LocalCurrencyId))
            {
                aPInvoicePM.LocalCurrencyId = myTenant.CurrencyId;
            }

            this.billToAccountingCard = aPInvoicePM.CreditAccount;
            this.currencyAccountingCard = aPInvoicePM.AccountingExternalCode;
        }        
        private void InitAndValidateVendor()
        {
            if (string.IsNullOrEmpty(this.aPInvoicePM.VendorId))
            {
                throw new ApplicationException("Vendor is required");
            }
            else
            {
                CardRepository cardRepository = new CardRepository(commonDataContext);
                Card vendor = cardRepository.GetSingleCard(this.aPInvoicePM.VendorId, tenant);
                if (vendor != null)
                {
                    if (string.IsNullOrEmpty(this.billToAccountingCard))
                    {
                        AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                        this.billToAccountingCard = accountingSystemHelper.GetGenericCreditAccount(vendor.Id, aPInvoicePM.InvoiceCurrencyId, tenant, true);
                    }

                    if (string.IsNullOrEmpty(this.aPInvoicePM.VATNumber))
                    {
                        this.aPInvoicePM.VATNumber = vendor.VatNumber;
                    }

                    if (string.IsNullOrEmpty(this.aPInvoicePM.InvoiceCurrencyId))
                    {
                        this.aPInvoicePM.InvoiceCurrencyId = vendor.InvoiceCurrencyId;
                    }

                    if (string.IsNullOrEmpty(this.defaultPaymentTermId))
                    {
                        defaultPaymentTermId = vendor.PaymentTermId;
                    }
                }
            }

            transferstatusCodes.Add(new TransferStatusCodeItem("BLTO", billToAccountingCard));
        }
        private void InitAndValidateGeneralData(AccountingSetting accountingSetting)
        {
            if (string.IsNullOrEmpty(this.aPInvoicePM.InvoiceNumber))
            {
                throw new ApplicationException("Invoice Number is required");
            }

            if (this.aPInvoicePM.InvoiceDate == null)
            {
                throw new ApplicationException("Invoice Date is required");
            }

            if (this.aPInvoicePM.AmountInInvoiceCurrency == null || this.aPInvoicePM.AmountInInvoiceCurrency == 0)
            {
                throw new ApplicationException("Invoice Amount is required");
            }
            else
            {
                this.aPInvoicePM.InvoiceExpectedAmount = this.aPInvoicePM.AmountInInvoiceCurrency;
            }

            if (accountingSetting != null && accountingSetting.IsVatNumberMandatoryInAP)
            {
                if (string.IsNullOrEmpty(this.aPInvoicePM.VATNumber))
                {
                    throw new ApplicationException("VAT Number is required");
                }
            }
        }
        private void InitAndValidateInvoiceCurrency()
        {
            if (string.IsNullOrEmpty(this.aPInvoicePM.InvoiceCurrencyId))
            {
                throw new ApplicationException("Invoice Currency is required");
            }

            else
            {
                Currency invoiceCurrency = CurrencyRepository.GetSingleCurrency(this.aPInvoicePM.InvoiceCurrencyId, tenant, true);
                if (invoiceCurrency != null)
                {
                    if (string.IsNullOrEmpty(this.currencyAccountingCard))
                    {
                        this.currencyAccountingCard = invoiceCurrency.AccountingExternalCode;
                    }
                }

                if (this.aPInvoicePM.InvoiceCurrencyId == this.aPInvoicePM.LocalCurrencyId)
                {
                    if (this.aPInvoicePM.InvoiceCurrencyExchangeRate != null && this.aPInvoicePM.InvoiceCurrencyExchangeRate != 0)
                    {
                        if (this.aPInvoicePM.InvoiceCurrencyExchangeRate != 1)
                        {
                            throw new ApplicationException("Invoice Currency Exchange Rate should be 1 when Invoice Currency same as Local Currency");
                        }
                    }
                }
            }

            transferstatusCodes.Add(new TransferStatusCodeItem("CURR", currencyAccountingCard));
        }
        private void InitAndValidateCurrencyRateData()
        {
            double? rate = null;
            DateTime? rateDate = null;
            if (!string.IsNullOrEmpty(this.aPInvoicePM.InvoiceCurrencyId))
            {
                if (this.aPInvoicePM.InvoiceCurrencyId == this.aPInvoicePM.LocalCurrencyId)
                {
                    rate = 1;
                }

                else
                {
                    DateTime? loadingDate = this.aPInvoicePM.InvoiceDate;
                    if (loadingDate == null)
                    {
                        loadingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    LastRate myRate = this.GetCurrencysExchangeRate(tenant, this.aPInvoicePM.LocalCurrencyId, this.aPInvoicePM.InvoiceCurrencyId, loadingDate.Value);
                    if (myRate != null)
                    {
                        rate = myRate.Rate;
                        rateDate = myRate.ValueDate;
                    }
                }
            }

            if (this.aPInvoicePM.InvoiceCurrencyExchangeRate == null || this.aPInvoicePM.InvoiceCurrencyExchangeRate == 0)
            {
                this.aPInvoicePM.InvoiceCurrencyExchangeRate = rate;
                this.aPInvoicePM.ExchangeRateDate = rateDate;
            }

            if (this.aPInvoicePM.InvoiceCurrencyExchangeRate == null || this.aPInvoicePM.InvoiceCurrencyExchangeRate == 0)
            {
                throw new ApplicationException("Invoice Currency Exchange Rate is required");
            }
        }
        private void InitAndValidateShipmentReference()
        {
            if (!string.IsNullOrEmpty(this.aPInvoicePM.MainEntityReference))
            {
                ShipmentQuery shipmentRepository = new ShipmentQuery(tenant);
                ShipmentPM shipment = shipmentRepository.GetSinglePMByShipmentNumber(this.aPInvoicePM.MainEntityReference, tenant, false);
                if (shipment != null)
                {
                    if (shipment.IsAccountingClosed)
                    {
                        throw new ApplicationException("The Shipment is Accounting Closed");
                    }

                    this.aPInvoicePM.ShipmentTransportModeId = shipment.TransportModeId;
                    this.aPInvoicePM.MainEntityId = shipment.Id;
                    this.aPInvoicePM.HouseNumber = shipment.House;
                    this.aPInvoicePM.MasterNumber = shipment.LongMaster;
                    this.aPInvoicePM.ProfitCurrencyId = shipment.ProfitCurrencyId;                    
                    this.aPInvoicePM.OperationalDate = shipment.OperationalDate;

                    if (this.aPInvoicePM.ProfitCurrencyId == this.aPInvoicePM.LocalCurrencyId)
                    {
                        this.aPInvoicePM.ProfitCurrencyExchangeRate = 1;
                    }

                    else
                    {
                        DateTime? loadingDate = this.aPInvoicePM.InvoiceDate;
                        if (loadingDate == null)
                        {
                            loadingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }
                        LastRate myRate = this.GetCurrencysExchangeRate(tenant, this.aPInvoicePM.LocalCurrencyId, this.aPInvoicePM.ProfitCurrencyId, loadingDate.Value);
                        if (myRate != null)
                        {
                            this.aPInvoicePM.ProfitCurrencyExchangeRate = myRate.Rate;
                        }
                    }

                    switch (shipment.DirectionId)
                    {
                        case "E": { this.aPInvoicePM.Description = "Export to " + shipment.MainCarriageFinalDestinationPortCode; break; }
                        case "I": { this.aPInvoicePM.Description = "Import from " + shipment.MainCarriageFromPortCode; break; }
                        case "D": { this.aPInvoicePM.Description = "Ship to " + shipment.ToPartnerCity; break; }
                    }

                    if (string.IsNullOrEmpty(this.aPInvoicePM.BranchId))
                    {
                        this.aPInvoicePM.BranchId = shipment.BranchId;
                    }
                }

                else
                {
                    throw new ApplicationException("No Shipment Found");
                }
            }
        }
        private void InitAndValidatePaymentTerm_DueDate()
        {
            bool calculateDueDate = true;

            if (string.IsNullOrEmpty(this.aPInvoicePM.PaymentTermId))
            {
                if (this.aPInvoicePM.DueDate != null)
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.PaymentTerm manuallySetPaymentTerm = paymentTermRepository.GetSinglemanuallySetPaymentTerm(tenant);
                    if (manuallySetPaymentTerm != null)
                    {
                        this.aPInvoicePM.PaymentTermId = manuallySetPaymentTerm.Id;
                        calculateDueDate = false;
                    }
                }

                else
                {
                    this.aPInvoicePM.PaymentTermId = this.defaultPaymentTermId;                   
                }
            }
            
            if(calculateDueDate)
            {
                DateTime? expectedDueDate = this.ComputeAPInvoiceDueDate(this.aPInvoicePM, paymentTermRepository);

                if (this.aPInvoicePM.DueDate != null)
                {
                    if (this.aPInvoicePM.DueDate != expectedDueDate)
                    {
                        throw new ApplicationException("Wrong Due Date regarding Payment Term");
                    }
                }

                else
                {
                    this.aPInvoicePM.DueDate = expectedDueDate;
                }
            }

            if (string.IsNullOrEmpty(this.aPInvoicePM.PaymentTermId))
            {
                throw new ApplicationException("Payment Term is required");
            }
        }
        public void InitAndValidateTotalVATsOnly()
        {

            if (this.aPInvoicePM.TotalVATOnly)
            {
                foreach (APInvoiceLinePM line in this.aPInvoicePM.InvoiceLines)
                {
                    if (line.VatTypeId != null || line.VatPercentage != null)
                    {
                        throw new ApplicationException("VAT types or percentages can't be sent in the lines with total VATs only invoices");
                    }
                }

                if (this.aPInvoicePM.TotalVATs == null || this.aPInvoicePM.TotalVATs.Count == 0)
                {
                    throw new ApplicationException("Total VATs is Missing");
                }

                foreach (APInvoiceTotalVATPM item in this.aPInvoicePM.TotalVATs)
                {
                    if(item.VatTypeId == null)
                    {
                        throw new ApplicationException("Total VAT Type is Missing");
                    }

                    if (item.VatPercent == null)
                    {
                        throw new ApplicationException("Total VAT Percent is Missing");
                    }

                    //if (item.InvoiceCurrencyVATAmount == null)
                    //{
                    //    throw new ApplicationException("Total VAT Amount is Missing");
                    //}

                    item.VatPercent = MethodHelper.Round(item.VatPercent, 3);
                    item.InvoiceCurrencyVATAmount = MethodHelper.Roundd(item.InvoiceCurrencyVATAmount, 2);
                    item.LocalVATAmount = MethodHelper.Roundd(item.InvoiceCurrencyVATAmount * aPInvoicePM.InvoiceCurrencyExchangeRate, 2);
                    item.ProfitCurrencyVATAmount = MethodHelper.Roundd(item.LocalVATAmount / aPInvoicePM.ProfitCurrencyExchangeRate, 2);
                }
            }
        }
        private void InitAndValidateInvoiceLines()
        {
            foreach (APInvoiceLinePM line in this.aPInvoicePM.InvoiceLines)
            {
                string chargeDebitAccount = line.DebitAccount;

                if (string.IsNullOrEmpty(line.ChargesTypeId))
                {
                    throw new ApplicationException("Line Charges Type is Missing");
                }

                else
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.ChargesType chargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, true);
                    if (chargesType != null)
                    {
                        if (!chargesType.IsPayable)
                        {
                            throw new ApplicationException(chargesType.Code + " should be marked as Payable");
                        }

                        if (!string.IsNullOrEmpty(chargesType.ContainerMeasurementId))
                        {
                            Simplog.Data.CommonDataModel.EntityPOCOs.Measurement measurement = measurementRepository.GetSingleMeasurement(chargesType.ContainerMeasurementId, tenant);
                            if (measurement != null)
                            {
                                if (measurement.Code == "BCNT")
                                {
                                    if (!string.IsNullOrEmpty(line.ContainerTypeId))
                                    {
                                        if (line.Quantity == null || line.Quantity == 0)
                                        {
                                            throw new ApplicationException(chargesType.Code + " Line Quantity is Missing when Charges is BCNT");
                                        }
                                    }
                                    else
                                    {
                                        if (line.Quantity != null && line.Quantity != 0)
                                        {
                                            throw new ApplicationException(chargesType.Code + " Line Container Type is Missing when Charges is BCNT");
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(line.ContainerTypeId) && line.Quantity != null && line.Quantity != 0)
                                    {
                                        Simplog.Data.CommonDataModel.EntityPOCOs.PackageType packageType = packageTypeRepository.GetSinglePackageType(line.ContainerTypeId, tenant);
                                        if(packageType != null)
                                        {
                                            if(!packageType.IsContainer)
                                            {
                                                throw new ApplicationException(chargesType.Code + " Line Container Type should be is Container");
                                            }

                                            if(!packageType.IsOcean)
                                            {
                                                throw new ApplicationException(chargesType.Code + " Line Container Type should be Ocean");
                                            }
                                            line.ContainerTypeCode = packageType.Code;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(line.ContainerTypeId))
                                    {
                                        throw new ApplicationException(chargesType.Code + " Line Container Type is not Allowed when Charges is not BCNT");
                                    }

                                    if (line.Quantity != null && line.Quantity != 0)
                                    {
                                        throw new ApplicationException(chargesType.Code + " Line Quantity is not Allowed when Charges is not BCNT");
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(chargeDebitAccount))
                        {
                            chargeDebitAccount = chargesType.PayableDebitAccount;
                        }

                        line.Description = chargesType.EnglishName;
                        line.LocalDescription = chargesType.LocalName;

                        if (!aPInvoicePM.TotalVATOnly)
                        {
                            if (string.IsNullOrEmpty(line.VatTypeId))
                            {
                                line.VatTypeId = chargesType.VatTypeId;
                            }

                            if (!string.IsNullOrEmpty(line.VatTypeId))
                            {
                                Simplog.Data.CommonDataModel.EntityPOCOs.VatType vatType = VatTypeRepository.GetSingleVatType(line.VatTypeId, tenant, true);
                                if (vatType != null)
                                {
                                    line.VatTypeName = vatType.EnglishName;
                                    line.VatIsMultiPercentage = vatType.IsMultiPercentage;

                                    if (line.VatPercentage == null)
                                    {
                                        if (!vatType.IsMultiPercentage)
                                        {
                                            VatTypePercentage vatTypePercentage = vatTypePercentageRepository.GetVatTypePercentageByDate(vatType.Id, tenant, this.aPInvoicePM.InvoiceDate);
                                            if (vatTypePercentage != null)
                                            {
                                                line.VatPercentage = vatTypePercentage.Percentage;
                                            }
                                        }
                                    }
                                }
                            }

                            else
                            {
                                throw new ApplicationException(chargesType.Code + " Line VAT Type is Missing");
                            }
                        }

                        if (string.IsNullOrEmpty(line.ForiegnCurrencyId))
                        {
                            line.ForiegnCurrencyId = this.aPInvoicePM.InvoiceCurrencyId;
                        }

                        this.ComputeInvoiceLineAmounts(line);
                    }
                }

                transferstatusCodes.Add(new TransferStatusCodeItem("CHRGE", chargeDebitAccount));

                if (string.IsNullOrEmpty(line.ForiegnCurrencyId))
                {
                   line.ForiegnCurrencyId = this.aPInvoicePM.InvoiceCurrencyId;
                }

                line.EntityReference = this.aPInvoicePM.MainEntityReference;
                line.EntityId = this.aPInvoicePM.MainEntityId;
            }
        }
        
        private void FillVATTransferExternalCodes(string accountingSysytemCode, string payableVATCard)
        {
            var myGroup = (from a in this.aPInvoicePM.InvoiceLines
                           where a.VatTypeId != null
                           && a.VatPercentage != null
                           && a.VatPercentage != 0
                           group a by new { a.VatTypeId, a.ExternalVATCard, } into g
                           select new
                           {
                               VatTypeId = g.Key.VatTypeId,
                               ExternalVATCard = g.Key.ExternalVATCard,
                           });

            foreach (var g in myGroup)
            {
                string externalVATCard = g.ExternalVATCard;

                if (string.IsNullOrEmpty(externalVATCard))
                {
                    if (accountingSysytemCode == "HV" || accountingSysytemCode == "RH")
                    {
                        externalVATCard = payableVATCard;
                    }
                    else
                    {
                        Simplog.Data.CommonDataModel.EntityPOCOs.VatType vatType = VatTypeRepository.GetSingleVatType(g.VatTypeId, tenant, true);
                        if (vatType != null)
                        {
                            externalVATCard = vatType.ExternalTAXItemId;
                        }
                    }
                }

                transferstatusCodes.Add(new TransferStatusCodeItem("VAT", externalVATCard));
            }
        }
        private void InitAndValidateTransferStatus()
        {
            string expectedStatus = "";

            bool isNotReady = transferstatusCodes.Where(d => string.IsNullOrEmpty(d.ExternalAccount)).Any();
            expectedStatus = isNotReady ? "NR" : "RD";

            if (!string.IsNullOrEmpty(this.aPInvoicePM.TransferStatusCode))
            {
                if (this.aPInvoicePM.TransferStatusCode == "RD" || this.aPInvoicePM.TransferStatusCode == "NR")
                {
                    if (this.aPInvoicePM.TransferStatusCode != expectedStatus)
                    {
                        throw new ApplicationException("Transfer Status should be " + expectedStatus);
                    }
                }
            }

            else
            {
                this.aPInvoicePM.TransferStatusCode = expectedStatus;
            }
        }
        private void ComputeInvoiceAmounts()
        {
            if (!string.IsNullOrEmpty(this.aPInvoicePM.InvoiceCurrencyId)
                && this.aPInvoicePM.InvoiceCurrencyExchangeRate != null && this.aPInvoicePM.InvoiceCurrencyExchangeRate != 0
                && this.aPInvoicePM.AmountInInvoiceCurrency != null && this.aPInvoicePM.AmountInInvoiceCurrency != 0)
            {
                double? setValue = MethodHelper.Round(this.aPInvoicePM.AmountInInvoiceCurrency, 2);

                //AmountInLocalCurrency
                if (this.aPInvoicePM.AmountInLocalCurrency == null || this.aPInvoicePM.AmountInLocalCurrency == 0)
                {
                    if (this.aPInvoicePM.LocalCurrencyId == this.aPInvoicePM.InvoiceCurrencyId)
                    {
                        this.aPInvoicePM.AmountInLocalCurrency = setValue;
                    }
                    else
                    {
                        this.aPInvoicePM.AmountInLocalCurrency = MethodHelper.Round(setValue * this.aPInvoicePM.InvoiceCurrencyExchangeRate, 2);
                    }
                }

                //AmountInProfitCurrency
                if (this.aPInvoicePM.AmountInProfitCurrency == null || this.aPInvoicePM.AmountInProfitCurrency == 0)
                {
                    if (this.aPInvoicePM.ProfitCurrencyId == this.aPInvoicePM.InvoiceCurrencyId)
                    {
                        this.aPInvoicePM.AmountInProfitCurrency = setValue;
                    }
                    else if (this.aPInvoicePM.ProfitCurrencyId == this.aPInvoicePM.LocalCurrencyId)
                    {
                        this.aPInvoicePM.AmountInProfitCurrency = this.aPInvoicePM.AmountInLocalCurrency;
                    }
                    else
                    {
                        this.aPInvoicePM.AmountInProfitCurrency = MethodHelper.Round(this.aPInvoicePM.AmountInLocalCurrency / this.aPInvoicePM.ProfitCurrencyExchangeRate, 2);
                    }
                }

                //SubTotalInLocalCurrency
                if (this.aPInvoicePM.SubTotalInLocalCurrency == null || this.aPInvoicePM.SubTotalInLocalCurrency == 0)
                {
                    this.aPInvoicePM.SubTotalInLocalCurrency = MethodHelper.Round(this.aPInvoicePM.InvoiceLines.Sum(s => s.LocalCurrencyAmount), 2);
                }

                //SubTotalInInvoiceCurrency
                if (this.aPInvoicePM.SubTotalInInvoiceCurrency == null || this.aPInvoicePM.SubTotalInInvoiceCurrency == 0)
                {
                    this.aPInvoicePM.SubTotalInInvoiceCurrency = MethodHelper.Round(this.aPInvoicePM.InvoiceLines.Sum(s => s.InvoiceCurrencyAmount), 2);
                }
            }
        }
        private void ComputeInvoiceLineAmounts(APInvoiceLinePM line)
        {
            //ForiegnExchangeRate
            if (line.ForiegnExchangeRate == null || line.ForiegnExchangeRate == 0)
            {
                if (line.ForiegnCurrencyId == this.aPInvoicePM.InvoiceCurrencyId)
                {
                    line.ForiegnExchangeRate = this.aPInvoicePM.InvoiceCurrencyExchangeRate;
                }

                else
                {
                    DateTime? loadingDate = this.aPInvoicePM.InvoiceDate;
                    if (loadingDate == null)
                    {
                        loadingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    }

                    LastRate myRate = this.GetCurrencysExchangeRate(tenant, this.aPInvoicePM.LocalCurrencyId, line.ForiegnCurrencyId, loadingDate.Value);
                    if (myRate != null)
                    {
                        line.ForiegnExchangeRate = myRate.Rate;
                    }
                }
            }

            //LocalCurrencyAmount
            if (line.LocalCurrencyAmount == null || line.LocalCurrencyAmount == 0)
            {
                line.LocalCurrencyAmount = MethodHelper.Round(line.InvoiceCurrencyAmount * this.aPInvoicePM.InvoiceCurrencyExchangeRate, 2);
            }

            //ForiegnCurrencyAmount
            if (line.ForiegnCurrencyAmount == null || line.ForiegnCurrencyAmount == 0)
            {
                if (line.ForiegnCurrencyId == this.aPInvoicePM.InvoiceCurrencyId)
                {
                    line.ForiegnCurrencyAmount = line.InvoiceCurrencyAmount;
                }

                else
                {
                    line.ForiegnCurrencyAmount = line.LocalCurrencyAmount / line.ForiegnExchangeRate;
                }
            }

            //ProfitCurrencyAmount
            if (line.ProfitCurrencyAmount == null || line.ProfitCurrencyAmount == 0)
            {
                if (line.ForiegnCurrencyId == this.aPInvoicePM.ProfitCurrencyId)
                {
                    line.ProfitCurrencyAmount = line.ForiegnCurrencyAmount;
                }

                else
                {
                    line.ProfitCurrencyAmount = line.LocalCurrencyAmount / this.aPInvoicePM.ProfitCurrencyExchangeRate;
                }
            }
        }
        private LastRate GetCurrencysExchangeRate(int tenant, string localCurrencyId, string currencyId, DateTime rateDate)
        {
            LastRate result = null;
            
            RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);

            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, currencyId, localCurrencyId, rateDate);
            if (lastRate != null)
            {
                result = lastRate;
            }

            return result;
        }
        public DateTime? ComputeAPInvoiceDueDate(APInvoicePM invoice, PaymentTermRepository paymentTermRepository)
        {
            DateTime? expectedDueDate = null;

            Simplog.Data.CommonDataModel.EntityPOCOs.PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTerm(invoice.PaymentTermId, invoice.Tenant);
            if (paymentTerm != null)
            {
                if (paymentTerm.IsManuallySet)
                {
                    if (invoice.DueDate == null)
                    {
                        throw new ApplicationException("Due Date is Required when Payment Term is Manually Set");
                    }
                }

                else
                {
                    DateTime? myComparativeDate = null;

                    if (paymentTerm.FromDateTypeCode == "SHI")
                    {
                        myComparativeDate = invoice.OperationalDate;

                        if (myComparativeDate == null)
                        {
                            myComparativeDate = invoice.InvoiceDate;
                        }
                    }

                    else
                    {
                        myComparativeDate = invoice.InvoiceDate;
                    }

                    if (paymentTerm.Days != 0)
                    {
                        if (myComparativeDate != null)
                        {
                            myComparativeDate = myComparativeDate.Value.AddDays(paymentTerm.Days);
                        }
                    }

                    if (expectedDueDate != myComparativeDate)
                    {
                        expectedDueDate = myComparativeDate;
                    }
                }
            }

            return expectedDueDate;
        }
        
        public APInvoice GetAPInvoiceByInvoiceNumber(string number, int tenant)
        {
            try
            {


                //var temp = query.GetSinglePM(null, tenant, number);
                var temp = query.GetSinglePMByNumber(number, tenant);
                if (temp == null)
                    throw new ApplicationException("APInvoice with number " + number + " doesn't exist");

                return APInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public APInvoice GetAPInvoiceByInvoiceNumberAndExternalId(string number,string externalId, int tenant)
        {
            try
            {


                var temp = query.GetSinglePMByNumberAndExternalId(number, externalId, tenant);
                if (temp == null)
                    throw new ApplicationException("APInvoice with number " + number + " doesn't exist");

                return APInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public APInvoice GetAPInvoiceByInternalNumber(string number, int tenant)
        {
            try
            {
                APInvoicePM temp = query.GetSinglePMByInternalNumber(number, tenant);

                if (temp == null)
                {
                    throw new ApplicationException("APInvoice with internal number " + number + " doesn't exist");
                }

                return APInvoiceDataMapping(temp, tenant);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public APInvoice GetSingleInvoiceByExternalEntityId(string externalId, int tenant)
        {
            try
            {
                var temp = query.GetSingleInvoiceByExternlaEntityId(externalId, tenant);
                if (temp == null)
                    throw new ApplicationException("APInvoice with external ID " + externalId + " doesn't exist");

                return APInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void APInvoiceCustomDataMapping(APInvoice apinvoice, int tenant)
        {
        
            apinvoice.Tenant = tenant;
            apinvoice.InvoiceExpectedAmount = Math.Round((double)apinvoice.AmountInInvoiceCurrency, 2);
            apinvoice.AmountInInvoiceCurrency= Math.Round((double)apinvoice.AmountInInvoiceCurrency, 2);
            apinvoice.AmountInLocalCurrency= Math.Round((double)apinvoice.AmountInLocalCurrency, 2);

            if(apinvoice.SubTotalInInvoiceCurrency != null)
                apinvoice.SubTotalInInvoiceCurrency = Math.Round((double)apinvoice.SubTotalInInvoiceCurrency, 2);

            if(apinvoice.SubTotalInLocalCurrency != null)
            apinvoice.SubTotalInLocalCurrency = Math.Round((double)apinvoice.SubTotalInLocalCurrency, 2);

            if (apinvoice.InvoiceCurrencyExchangeRate == null)
            {
                apinvoice.InvoiceCurrencyExchangeRate = GetInvoiceCurrencyExchangeRate(apinvoice, tenant);
            }

            TenantPM tenantPM = GetTenantPM(tenant);
            if (apinvoice.LocalCurrency == null)
            {
                apinvoice.LocalCurrency = GetCurrency(tenantPM?.CurrencyId, tenant);
            }
            if (apinvoice.ProfitCurrency == null)
            {
                apinvoice.ProfitCurrency = GetCurrency(tenantPM?.ProfitCurrencyId, tenant);
                apinvoice.ProfitCurrencyExchangeRate = GetRateByTenantAndCurrency(apinvoice.ProfitCurrency.Id, tenantPM);
            }

            apinvoice.AmountDue = apinvoice.AmountInInvoiceCurrency == null ? 0 : apinvoice.AmountInInvoiceCurrency;
            apinvoice.IsExternalEntity = true;
            apinvoice.IsGeneralInvoice = true;

            if (apinvoice.AccountingDate == null)
            {
                apinvoice.AccountingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            }
       
        }

        public void PaymentTermMapAndValidate(APInvoice apinvoice, APInvoicePM apinvoicePM, int tenant)
        {
            if (apinvoice.PaymentTerm == null && apinvoice.DueDate != null)
            {
                double daysDifference = GetDaysDiffernceForDate(apinvoice.DueDate, tenant);
                var paymentTerm = GetPaymentTermByDaysDifference(tenant, daysDifference);
                if (paymentTerm == null)
                    apinvoice.PaymentTerm = GetManuallySetPaymentTerm(tenant);
                else
                    apinvoice.PaymentTerm = paymentTerm;
            }
            else if (apinvoice.PaymentTerm != null && apinvoice.DueDate != null)
            {
                double daysDifference = GetDaysDiffernceForDate(apinvoice.DueDate, tenant);
                if (apinvoice.PaymentTerm.Days != daysDifference)
                {
                    apinvoice.PaymentTerm = GetManuallySetPaymentTerm(tenant);
                }
            }
            else if (apinvoice.PaymentTerm != null && apinvoice.DueDate == null)
            {
                int daysDifference = apinvoice.PaymentTerm.Days;
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime dueDate = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day + daysDifference, 0, 0, 0);

                apinvoicePM.DueDate = dueDate;
            }
            else
            {
                throw new ApplicationException("Neither Due Date nor Payment Term is provided!");
            }

            if (apinvoicePM.PaymentTermId == null)
                apinvoicePM.PaymentTermId = apinvoice.PaymentTerm?.Id;
        }

        private static PaymentTerm GetManuallySetPaymentTerm(int tenant)
        {
            PaymentTermQuery paymentTermQuery = new PaymentTermQuery(tenant);
            PaymentTermPM paymentTerm = paymentTermQuery.GetSinglePMByExternalId("MS", tenant);

            if (paymentTerm == null)
                throw new ApplicationException("No 'Manually Set' payment term with ExternalId='MS', tenant=" + tenant);

            PaymentTerm manuallySetPaymentTerm = new PaymentTerm()
            {
                Days = paymentTerm.Days,
                EnglishName = paymentTerm.EnglishName,
                Id = paymentTerm.Id,
                LocalName = paymentTerm.LocalName,
                ExternalId = paymentTerm.ExternalId,
            };
            return manuallySetPaymentTerm;
        }

        private static double GetDaysDiffernceForDate(DateTime? dueDateTime, int tenant)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime todayDate = new DateTime(todayDateTime.Year, todayDateTime.Month, todayDateTime.Day, 0, 0, 0);
            DateTime dueDate = new DateTime(dueDateTime.Value.Year, dueDateTime.Value.Month, dueDateTime.Value.Day, 0, 0, 0);
            double daysDifference = (dueDate - todayDate).TotalDays;
            return daysDifference;
        }

        private static PaymentTerm GetPaymentTermByDaysDifference(int tenant, double daysDifference)
        {
            PaymentTermQuery paymentTermQuery = new PaymentTermQuery(tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.PaymentTerm paymentTerm = paymentTermQuery.GetSingleByDaysDifference((int)daysDifference, tenant);

            PaymentTerm paymentTermAPI = null;
            if (paymentTerm != null)
            {
                paymentTermAPI = new PaymentTerm()
                {
                    Days = paymentTerm.Days,
                    EnglishName = paymentTerm.EnglishName,
                    Id = paymentTerm.Id,
                    LocalName = paymentTerm.LocalName,
                    ExternalId = paymentTerm.ExternalId,
                };
            }
            return paymentTermAPI;
        }

        public void CustomeValidateAPInvoice(APInvoice apinvoice)
        {
            if (apinvoice.InvoiceDate == null)
            {
                throw new ApplicationException("InvoiceDate is not provided");
            }

            if (apinvoice.AccountingDate == null)
            {
                throw new ApplicationException("AccountingDate is not provided");
            }

            if (apinvoice.Vendor == null)
            {
                throw new ApplicationException("Vendor is not provided");
            }

            if (apinvoice.VATNumber == null)
            {
                throw new ApplicationException("VATNumber is not provided");
            }

            if (apinvoice.AmountInInvoiceCurrency == null)
            {
                throw new ApplicationException("AmountInInvoiceCurrency is not provided");
            }

            if (apinvoice.InvoiceCurrency == null)
            {
                throw new ApplicationException("InvoiceCurrency is not provided");
            }

            if (apinvoice.Branch == null)
            {
                throw new ApplicationException("Branch is not provided");
            }

            foreach (APInvoiceLine line in apinvoice.InvoiceLines)
            {
                if (line.ChargesType == null)
                {
                    throw new ApplicationException("ChargesType is not provided");
                }

                if (line.InvoiceCurrencyAmount == null)
                {
                    throw new ApplicationException("InvoiceCurrencyAmount is not provided");
                }

                if (line.VatType == null && !apinvoice.TotalVATOnly)
                {
                    throw new ApplicationException("VatType is not provided");
                }
            }

            this.CheckIfExternlaEntiityIdExist(apinvoice);

            // validate totals
            double SubTotalInLocalCurrency = Math.Round(apinvoice.InvoiceLines.Sum(d => d.LocalCurrencyAmount).Value, 2);
            double linesInvoiceAmount = Math.Round(apinvoice.InvoiceLines.Sum(d => d.InvoiceCurrencyAmount.Value), 2);

            // commented to allow code to calculate totals with vat.
            //if(apinvoice.AmountInInvoiceCurrency != linesInvoiceAmount)
            //{
            //    throw new ApplicationException("Invoice Amount field doesnt match the total amount");
            //}
        }

        private void CheckIfExternlaEntiityIdExist(APInvoice apinvoice)
        {

            if (apinvoice.ExternalAccountingEntityId != null)
            {
                APInvoicePM invoice = query.GetSingleInvoiceByExternlaEntityId(apinvoice.ExternalAccountingEntityId, apinvoice.Tenant);
                if (invoice != null)
                {
                    throw new Exception("invoice with the same externla id already exist!");
                }
            }

        }

        private double GetInvoiceCurrencyExchangeRate(APInvoice apinvoice, int tenant)
        {
            CurrencyPM invoiceCurrency = GetInvoiceCurrency(apinvoice, tenant);
            TenantPM tenantPM = GetTenantPM(tenant);

            return GetRateByTenantAndCurrency(invoiceCurrency.Id, tenantPM);
        }

        private double GetRateByTenantAndCurrency(string currencyId, TenantPM tenantPM)
        {
            var tenant = tenantPM.Id;
            double rate;
            if (currencyId == tenantPM?.CurrencyId)
            {
                rate = 1;

            }
            else
            {
                RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
                LastRate lastRate = ratesTableQuery.GetLastRecord(tenant, currencyId, tenantPM?.CurrencyId);
                rate = lastRate != null? (double)lastRate.Rate: 1;
            }

            return rate;
        }

        private static TenantPM GetTenantPM(int tenant)
        {
            //TenantQuery tenantQuery = new TenantQuery();
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant);
            return tenantPM;
        }

        private static CurrencyPM GetInvoiceCurrency(APInvoice apinvoice, int tenant)
        {
            CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(tenant);
            CurrencyPM invoiceCurrency = InvoiceCurrencyCurrencyService.CurrencyDataMappingAndValidatin(apinvoice.InvoiceCurrency, tenant, "");
            return invoiceCurrency;
        }

        private CommonDataModel.APIDataContract.ApiV1.Currency GetCurrency(string id, int tenant)
        {
            CurrencyQueryService currencyQuery = new CurrencyQueryService(tenant);
            CommonDataModel.APIDataContract.ApiV1.Currency currency = currencyQuery.GetCurrencyById(id, tenant);
            return currency;
        }
    }

    public class TransferStatusCodeItem
    {
        public TransferStatusCodeItem(string itemCode, string externalAccount)
        {
            ItemCode = itemCode;
            ExternalAccount = externalAccount;
        }

        public string ItemCode { get; set; }
        public string ExternalAccount { get; set; }
    }
}
