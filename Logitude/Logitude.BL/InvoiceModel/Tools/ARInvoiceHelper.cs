using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools;
using System.Transactions;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Microsoft.Practices.Unity;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Intuit.Ipp.Core;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
using Logitude.SystemLogs;
using Logitude.Server.Tools.QueueService;
using Simplog.Global.Data.GlobalModel;
using Intuit.Ipp.OAuth2PlatformClient;
using System.Net;

namespace Logitude.BL.InvoiceModel.Tools
{
    public class ARInvoiceHelper
    {
        private int tenant;
        private string loggedContactId;
        public ARInvoiceHelper()
        {

        }
        public ARInvoiceHelper(int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
            this.tenant = tenant;
            loggedContactId = loggedContact.Id;
        }

        public ARInvoiceHelper(int tenant, string loggedUserId)
        {
            this.tenant = tenant;
            this.loggedContactId = loggedUserId;
        }

        private  string tenantName { set; get; }
        private  ICommonDataContext commonContext;
        private IGlobalContext globalContext;
        private  DocumentRepository documentRepository;
        private  CommunicationLogRepository communicationLogRepository;

        private TenantManagementRepository tenantManagementRepository;
        private  string ARInvoiceId;
        private  ARInvoicePM ARInvoice;
        private  string myObjectTableId;
        private  ARInvoiceLineRepository invoiceLineRepository;
        private  IInvoiceContext objectContext;
        private  ARInvoiceTotalVATRepository invoiceTotalVatRepository;
        private  string ExternalCurrencyCode;
        private string OldTransferStatusCode;
        private string ExternalTableIdCustomerRef;
        public  string PaymentTermExternalCode = null;
        private  List<string> ExternalChargesTypesCode;
        private  List<string> ExternalVatTypesCode;
        private  Boolean IsNewEntity;
        private  Boolean IsSameHomeCurrency = false;
        private  string AccountingSystemCode;
        List<ARInvoiceLinePM> lines;
        private bool isIndiaCountry=false;
        private string IndiaExternalQBOStates = "";
        private Tenant loggedTenant;
        private AccountingSystemPM accountingSystem;

        private void GetObjectTableData()
        {

            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                tenantManagementRepository = new TenantManagementRepository(this.globalContext);
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                if (tenantManagement != null)
                {
                    if (tenantManagement.CountryName == "India")
                    {
                        this.isIndiaCountry = true;
                    }
                }
                scope.Complete();
            }
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("ARInvoice", 0, true);
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }
        }
        public  void ARInvoiceQuickbooksValidating(ARInvoice entityPOCO, ARInvoicePM entityPM, Boolean IsSetApproved, Boolean isNewEntity, IInvoiceContext InvoiceContext, ICommonDataContext CommonContext,Boolean isSetVoided)
        {
            if (entityPM.TransferStatusCode == "BL")
            {
                return;
            }

            if ((isSetVoided && entityPM.ARInvoiceTypeCode != "CD" && entityPM.ARInvoiceTypeCode != "CC") || (entityPM.StatusCode == "VD" && entityPM.SetReSendQBO == true))
            {
                bool isTransferingVoiding = true;

                if (entityPOCO.StatusCode == null || entityPOCO.StatusCode == "DR")
                {
                    isTransferingVoiding = false;
                }

                else if (entityPOCO.TransferStatusCode == "ET")
                {
                    isTransferingVoiding = false;
                }

                if (entityPM.IsConstituentInvoice && string.IsNullOrEmpty(entityPM.ConsolidationInvoiceId))
                {
                    isTransferingVoiding = false;
                }

                if (isTransferingVoiding)
                {
                    commonContext = CommonContext;
                    this.globalContext = GlobalContext.GetContext(tenant);
                    loggedTenant = (from a in commonContext.Tenants.Include("AccountingSetting") where a.Id == entityPM.Tenant select a).FirstOrDefault();
                    tenant = loggedTenant.Id;
                    tenantName = loggedTenant.Company;
                    AccountingSystemCode = loggedTenant.AccountingSetting.AccountingSystemCode;
                    AccountingSystemQuery query = new AccountingSystemQuery(tenant);
                    accountingSystem = query.GetSingleAccountingSystemPM(AccountingSystemCode);
                    if (loggedTenant.AccountingSetting != null)
                        if (IsQuickBooksAccoutingSystemTransfer(entityPM))
                        {
                            if (entityPM.ExternalAccountingEntityId != null)
                            {
                                ARInvoice = entityPM;
                                ARInvoiceId = entityPM.Id;
                                GetObjectTableData();
                                documentRepository = new DocumentRepository(commonContext);
                                communicationLogRepository = new CommunicationLogRepository(commonContext);
                                //ContactRepository contactRepository = new ContactRepository(commonContext);
                                //Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                                //LoggedContactId = loggedContact.Id;

                                entityPM.TransferStatusCode = "IP";
                                entityPM.TransferError = null;
                                this.SendXMLFileInvoiceVoid(entityPM.ExternalAccountingEntityId, "QBO");
                            }

                            else
                            {
                                throw new ApplicationException("This Invoice is not transfered yet to quickbooks online.");
                            }
                        }
                }
            }

            else if (IsSetApproved || (entityPM.IsAutoCredit && entityPM.ExternalAccountingEntityId==null))
            {
                commonContext = CommonContext;
                loggedTenant = (from a in commonContext.Tenants.Include("AccountingSetting") where a.Id == entityPM.Tenant select a).FirstOrDefault();
                tenant = loggedTenant.Id;
                this.globalContext = GlobalContext.GetContext(tenant);
                tenantName = loggedTenant.Company;
                AccountingSystemCode = loggedTenant.AccountingSetting.AccountingSystemCode;
                AccountingSystemQuery query = new AccountingSystemQuery(tenant);
                accountingSystem = query.GetSingleAccountingSystemPM(AccountingSystemCode);
                if (loggedTenant.AccountingSetting != null)
                    if (IsQuickBooksAccoutingSystemTransfer(entityPM))
                    {
                        ARInvoice = entityPM;
                        ARInvoiceId = entityPM.Id;
                        GetObjectTableData();
                        documentRepository = new DocumentRepository(commonContext);
                        communicationLogRepository = new CommunicationLogRepository(commonContext);
                        objectContext = InvoiceContext;
                        invoiceLineRepository = new ARInvoiceLineRepository(objectContext);
                        invoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);
                        //ContactRepository contactRepository = new ContactRepository(commonContext);
                        //Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                        //LoggedContactId = loggedContact.Id;
                        lines = new List<ARInvoiceLinePM>();

                        if (loggedTenant.CurrencyId == entityPM.InvoiceCurrencyId)
                            IsSameHomeCurrency = true;
                        IsNewEntity = isNewEntity;

                        ExternalChargesTypesCode = new List<string>();
                        ExternalVatTypesCode = new List<string>();
                        #region

                        bool isReady = true;
                        string myError = null;
                        string ExternalCodeError = "Bill To: " + entityPM.BillToName + ". External ID is missing"; //External ID
                        string paymentTermError = "Payment Term: " + entityPM.PaymentTermName + ". External ID is missing"; // 
                        string vatError = "";
                        string chargeTypeError = "";
                        string CurrencyError = "";
                        string DifferentCurrencies = "The Currencies are different on your Customer External Currency  and System External Currency ";

                        CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
                        IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entityPM.BillToId && d.CurrencyId == entityPM.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            if (FieldIsEmpty(myCardExternal.ExternalRecievableTableId))
                            {
                                isReady = false;
                                myError = ExternalCodeError;


                            }
                            else
                            {
                                ExternalTableIdCustomerRef = myCardExternal.ExternalRecievableTableId;                                              }
                              }
                        else
                        {
                            isReady = false;
                            myError = ExternalCodeError;                            
                        }


                        if (this.isIndiaCountry)
                        {
                            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                            {
                                Simplog.Data.CommonDataModel.EntityPOCOs.Address address2 = commonContext.Addresses.Include("Country").Where(p => p.CardId == entityPM.BillToId && p.Id == entityPM.BillToAddressId && p.Country.Code == "IN" ).FirstOrDefault();
                                if (address2!=null)
                                {
                                    State state = commonContext.States.Include("Country").Where(p => p.Id == address2.StateId && p.Tenant == tenant).FirstOrDefault();
                                    if (state != null)
                                    {
                                        if (state.Country.EnglishName != "India")
                                        {
                                            this.IndiaExternalQBOStates = "97";
                                        }
                                        else
                                        {
                                            this.IndiaExternalQBOStates = state.QBOTransactionLocationCode;
                                        }
                                    }
                                }
                                scope.Complete();
                            }
                        }



                        PaymentTermRepository paymentTermRepository = new PaymentTermRepository(tenant);
                        if (entityPM.PaymentTermId != null)
                        {
                            PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPM.PaymentTermId, tenant);
                            if (FieldIsEmpty(myPaymentTerm.ExternalId))
                            {
                                isReady = false;
                                myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + ";" + paymentTermError;

                            }
                            else
                            {
                                PaymentTermExternalCode = myPaymentTerm.ExternalId;

                            }
                        }
                        else
                        {

                            isReady = false;
                            myError = string.IsNullOrEmpty(myError) ? "Payment term Field Is Required " : myError + ";" + "Payment term Field Is Required";

                        }
                      
                            Simplog.Data.CommonDataModel.EntityPOCOs.Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.InvoiceCurrencyId, tenant, false);
                        if (loggedTenant.CurrencyId != myCurrency.Id)
                        {
                            if (FieldIsEmpty(myCurrency.AccountingExternalCode))
                            {
                                isReady = false;
                                CurrencyError = "Currency: " + myCurrency.Code + ". External ID is missing.";
                                myError = string.IsNullOrEmpty(myError) ? CurrencyError : myError + ";" + CurrencyError;


                            }
                            else
                            {
                                ExternalCurrencyCode = myCurrency.AccountingExternalCode;                               
                            }
                        }
                        else
                            ExternalCurrencyCode = myCurrency.AccountingExternalCode;                     


                        if (isNewEntity)
                        {
                            lines = entityPM.InvoiceLines.ToList();
                        }

                        else
                        {
                            lines = entityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                        }

                        foreach (ARInvoiceLinePM line in lines)
                        {
                            ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, false);
                            if (myChargesType.ReceivablesChargesTypeExternalCode == null || (myChargesType.ReceivablesChargesTypeExternalCode != null && string.IsNullOrEmpty(myChargesType.ReceivablesChargesTypeExternalCode.Trim())))
                            {
                                isReady = false;
                                chargeTypeError = "Charge Type: " + myChargesType.EnglishName + ". External ID is missing.";
                                myError = string.IsNullOrEmpty(myError) ? chargeTypeError : myError + ";" + chargeTypeError;
                            }
                            else
                            {

                                ExternalChargesTypesCode.Add(myChargesType.ReceivablesChargesTypeExternalCode);

                            }
                          
                                VatType myVatType = VatTypeRepository.GetSingleVatType(line.VatTypeId, tenant, false);
                            ExternalVatTypesCode.Add(myVatType.ReceivablesExternalId);

                            if (line.VatPercentage != 0 || AccountingSystemCode == "QBOG")
                            {
                                if (AccountingSystemCode=="QBO")
                                {
                                    var error = "VAT Percentage must be 0 in Quickbooks online US";
                                    isReady = false;
                                    myError = string.IsNullOrEmpty(myError) ? error : myError + ";" + error;
                                    if (myError != null ? !myError.Contains(error) : true)
                                        myError = string.IsNullOrEmpty(myError) ? error : myError + ";" + error;                                    
                                }

                                if (FieldIsEmpty(myVatType.ReceivablesExternalId))
                                {
                                    vatError = "VAT Type: " + myVatType.EnglishName + ". External ID is missing.";
                                    isReady = false;
                                    if (myError != null ? !myError.Contains(vatError) : true)
                                        myError = string.IsNullOrEmpty(myError) ? vatError : myError + ";" + vatError;
                                }
                            }                                                      
                            
                        }



                        if (isReady)
                        {
                            OldTransferStatusCode = entityPM.TransferStatusCode;
                            entityPM.TransferStatusCode = "IP";
                            entityPM.TransferError = null;
                            Run(entityPM);
                        }

                        else
                        {
                            entityPM.TransferStatusCode = "NR";
                            myError.Replace(',', ' ');
                            entityPM.TransferError = myError;
                            throw new ApplicationException(myError);

                        }
                        #endregion


                    }



            }
        }

        private bool IsQuickBooksAccoutingSystemTransfer(ARInvoicePM aRInvoicePM)
        {
            if (!(AccountingSystemCode == "QBO" || AccountingSystemCode == "QBOG")) return false;
            if (!(loggedTenant.AccountingSetting.IsARInvoicesTransferEnabled)) return false;
            if (!(accountingSystem.AllowARInvoicesTransfer)) return false;
            if ((loggedTenant.AccountingSetting.ARInvoiceTransferStartDate != null && aRInvoicePM.InvoiceDate < loggedTenant.AccountingSetting.ARInvoiceTransferStartDate)) return false;
            return true;
        }

        public  List<Intuit.Ipp.Data.Customer> GetQuickBooksOnlineCustomersByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Customer> customerQueryService = new QueryService<Intuit.Ipp.Data.Customer>(context);
                List<Intuit.Ipp.Data.Customer> myResult = customerQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }


        public List<Intuit.Ipp.Data.Customer> GetQuickBooksOnlineCustomersByText2(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Customer> customerQueryService = new QueryService<Intuit.Ipp.Data.Customer>(context);
              List<Intuit.Ipp.Data.Customer> myResult = customerQueryService.ExecuteIdsQuery(sql).ToList();
                return null;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }



        public  List<Intuit.Ipp.Data.Vendor> GetQuickBooksOnlineVendorByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Vendor> VendorQueryService = new QueryService<Intuit.Ipp.Data.Vendor>(context);
                List<Intuit.Ipp.Data.Vendor> myResult = VendorQueryService.ExecuteIdsQuery(sql).ToList();                
                return myResult;
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }


        }

        public List<Intuit.Ipp.Data.PaymentMethod> GetQuickBooksOnlinePaymentMethodByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.PaymentMethod> VendorQueryService = new QueryService<Intuit.Ipp.Data.PaymentMethod>(context);
                List<Intuit.Ipp.Data.PaymentMethod> myResult = VendorQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;                
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }




        public  List<Intuit.Ipp.Data.TaxCode> GetQuickBooksOnlineVatTypesByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.TaxCode> TaxCodeQueryService = new QueryService<Intuit.Ipp.Data.TaxCode>(context);
                List<Intuit.Ipp.Data.TaxCode> myResult = TaxCodeQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }



        public  List<Intuit.Ipp.Data.Item> GetQuickBooksOnlineReceivableChargesTypesByText(String sql, String tenant)
        {

            try
            {

                ServiceContext context = QuickbooksService.GetServiceContext(tenant);

                QueryService<Intuit.Ipp.Data.Item> ItemQueryService = new QueryService<Intuit.Ipp.Data.Item>(context);
                List<Intuit.Ipp.Data.Item> myResult = ItemQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }


        public  List<Intuit.Ipp.Data.Account> GetQuickBooksOnlinePayablesChargesTypesByText(String sql, String tenant)
        {

            try
            {

                ServiceContext context = QuickbooksService.GetServiceContext(tenant);

                QueryService<Intuit.Ipp.Data.Account> AccountQueryService = new QueryService<Intuit.Ipp.Data.Account>(context);
                List<Intuit.Ipp.Data.Account> myResult = AccountQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }


        



        public  List<Intuit.Ipp.Data.CompanyCurrency> GetQuickBooksOnlineCurrenciesByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.CompanyCurrency> CompanyCurrencyQueryService = new QueryService<Intuit.Ipp.Data.CompanyCurrency>(context);
                List<Intuit.Ipp.Data.CompanyCurrency> myResult = CompanyCurrencyQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }



        public  List<Intuit.Ipp.Data.Term> GetQuickBooksOnlinePaymentTermsByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Term> TermQueryService = new QueryService<Intuit.Ipp.Data.Term>(context);
                List<Intuit.Ipp.Data.Term> myResult = TermQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }










        private  void Run(ARInvoicePM invoice)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                var sum=lines.Sum(p => p.InvoiceCurrencyAmount);

                if ((invoice.ARInvoiceTypeCode == "CD" || invoice.ARInvoiceTypeCode == "CC") && sum<0)
                {
                    Intuit.Ipp.Data.CreditMemo QBOInvoice = new CreditMemo();
                    QBOInvoice.CustomerRef = new ReferenceType { Value = ExternalTableIdCustomerRef };
                    QBOInvoice.DocNumber = invoice.InvoiceNumber;
                    string notes = "";
                    if (!String.IsNullOrEmpty(invoice.PrintNotes))
                    {
                        notes = " , " + invoice.PrintNotes;
                    }
                    QBOInvoice.CustomerMemo = new MemoRef { Value = "Shipment Number : "+invoice.MainEntityReference+ notes };

                    if (PaymentTermExternalCode != null)
                        QBOInvoice.SalesTermRef = new ReferenceType { Value = PaymentTermExternalCode };

                    System.Collections.Generic.List<Line> lineList = new List<Line>();
                    string ExternalVatTypeCodeWhereIsNotZeroPercentage = "";
                    if (this.isIndiaCountry)
                        QBOInvoice.TransactionLocationType = IndiaExternalQBOStates;
                    bool IsMinus = true;
                    if (invoice.AmountInInvoiceCurrency > 0)
                        IsMinus = false;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Line line = new Line();
                        line.Description = lines[i].Description;
                            line.Amount =IsMinus? Decimal.Parse(lines[i].InvoiceCurrencyAmount + "") * -1 : Decimal.Parse(lines[i].InvoiceCurrencyAmount + "");
                    
                        line.AmountSpecified = true;
                        line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line.DetailTypeSpecified = true;

                        SalesItemLineDetail lineSalesItemLineDetail = new SalesItemLineDetail();                  
                        lineSalesItemLineDetail.Qty = (decimal)lines[i].Quantity;
                        lineSalesItemLineDetail.QtySpecified = true;
                        decimal UnitPrice = line.Amount/ (decimal)lines[i].Quantity;                       
                        lineSalesItemLineDetail.AnyIntuitObject = UnitPrice;
                        lineSalesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;

                        if (AccountingSystemCode == "QBO")
                        {
                            lineSalesItemLineDetail.ItemRef = new ReferenceType()
                            {
                                Value = ExternalChargesTypesCode[i]
                            };

                            if (lines[i].VatPercentage != 0)
                            {
                                ExternalVatTypeCodeWhereIsNotZeroPercentage = ExternalVatTypesCode[i];                              
                                lineSalesItemLineDetail.TaxCodeRef = new ReferenceType() { Value = "TAX" };

                            }
                        }

                        else if (AccountingSystemCode == "QBOG")
                        {                           
                            lineSalesItemLineDetail.ItemRef = new ReferenceType()
                            {
                                Value = ExternalChargesTypesCode[i]
                            };                         
                            lineSalesItemLineDetail.TaxCodeRef = new ReferenceType() { Value = ExternalVatTypesCode[i] };
                        }

                        line.AnyIntuitObject = lineSalesItemLineDetail;
                        lineList.Add(line);
                    }


                    if (ExternalVatTypeCodeWhereIsNotZeroPercentage != "")
                    {
                        TxnTaxDetail txnTaxDetail = new TxnTaxDetail();
                        txnTaxDetail.TxnTaxCodeRef = new ReferenceType()
                        {
                            Value = ExternalVatTypeCodeWhereIsNotZeroPercentage
                        };
                        QBOInvoice.TxnTaxDetail = txnTaxDetail; // VatType set
                    }
                    QBOInvoice.Line = lineList.ToArray();
                    if (!IsSameHomeCurrency)
                    {
                        QBOInvoice.ExchangeRate = decimal.Parse(invoice.InvoiceCurrencyExchangeRate + "");
                        QBOInvoice.ExchangeRateSpecified = true;
                        QBOInvoice.CurrencyRef = new ReferenceType { Value = ExternalCurrencyCode };
                    }
                    else
                    {
                        QBOInvoice.ExchangeRateSpecified = false;
                        QBOInvoice.CurrencyRef = null;
                    }
                    QBOInvoice.TxnDate = invoice.InvoiceDate.Value;
                    QBOInvoice.TxnDateSpecified = true;
                    SendXMLFileMemo(QBOInvoice, "QBO");

                }
                else
                {
                    Intuit.Ipp.Data.Invoice QBOInvoice = new Invoice();
                    QBOInvoice.CustomerRef = new ReferenceType { Value = ExternalTableIdCustomerRef };
                    QBOInvoice.DocNumber = invoice.InvoiceNumber;
                    string notes = "";
                    if (!String.IsNullOrEmpty(invoice.PrintNotes))
                    {
                        notes = " , " + invoice.PrintNotes;
                    }
                    QBOInvoice.CustomerMemo = new MemoRef { Value = "Shipment Number : " + invoice.MainEntityReference + notes };
                    if(this.isIndiaCountry)
                    QBOInvoice.TransactionLocationType = IndiaExternalQBOStates;
                    if (PaymentTermExternalCode != null)
                        QBOInvoice.SalesTermRef = new ReferenceType { Value = PaymentTermExternalCode };

                    System.Collections.Generic.List<Line> lineList = new List<Line>();
                    string ExternalVatTypeCodeWhereIsNotZeroPercentage = "";
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Line line = new Line();
                        line.Description = lines[i].Description;
                        line.Amount = Decimal.Parse(lines[i].InvoiceCurrencyAmount + "");
                        line.AmountSpecified = true;
                        line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                        line.DetailTypeSpecified = true;

                        SalesItemLineDetail lineSalesItemLineDetail = new SalesItemLineDetail();
                        lineSalesItemLineDetail.Qty = (decimal)lines[i].Quantity;
                        lineSalesItemLineDetail.QtySpecified = true;
                        lineSalesItemLineDetail.AnyIntuitObject = line.Amount/ (decimal)lines[i].Quantity;
                        lineSalesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;

                        if (AccountingSystemCode == "QBO")
                        {
                            lineSalesItemLineDetail.ItemRef = new ReferenceType()
                            {
                                Value = ExternalChargesTypesCode[i]
                            };
                            if (lines[i].VatPercentage != 0)
                            {
                                ExternalVatTypeCodeWhereIsNotZeroPercentage = ExternalVatTypesCode[i];                                                                                               
                                    lineSalesItemLineDetail.TaxCodeRef = new ReferenceType() { Value = "TAX" };                                

                            }
                        }

                        else if (AccountingSystemCode == "QBOG")
                        {

                            lineSalesItemLineDetail.ItemRef = new ReferenceType()
                            {
                                Value = ExternalChargesTypesCode[i]
                            };
                                 lineSalesItemLineDetail.TaxCodeRef = new ReferenceType() { Value = ExternalVatTypesCode[i] };
                            
                        }


                      
                        line.AnyIntuitObject = lineSalesItemLineDetail;
                        lineList.Add(line);
                    }


                    if (ExternalVatTypeCodeWhereIsNotZeroPercentage != "")
                    {
                        TxnTaxDetail txnTaxDetail = new TxnTaxDetail();
                        txnTaxDetail.TxnTaxCodeRef = new ReferenceType()
                        {
                            Value = ExternalVatTypeCodeWhereIsNotZeroPercentage
                        };
                        QBOInvoice.TxnTaxDetail = txnTaxDetail; // VatType set
                    }
                    QBOInvoice.Line = lineList.ToArray();
                    if (!IsSameHomeCurrency)
                    {
                        QBOInvoice.ExchangeRate = decimal.Parse(invoice.InvoiceCurrencyExchangeRate + "");
                        QBOInvoice.ExchangeRateSpecified = true;
                        QBOInvoice.CurrencyRef = new ReferenceType { Value = ExternalCurrencyCode };

                    }
                    else
                    {
                        QBOInvoice.ExchangeRateSpecified = false;
                        QBOInvoice.CurrencyRef = null;
                    }
                    QBOInvoice.TxnDate = invoice.InvoiceDate.Value;
                    QBOInvoice.TxnDateSpecified = true;
                    SendXMLFile(QBOInvoice, "QBO");

                }


                scope.Complete();

            }

        }

        private  string myDocumentId;
        private  string myDocumentFolder;
        private  string myDocumentExtension;
        private  string myCommunicationLogId;

        private  void SendXMLFile(Intuit.Ipp.Data.Invoice myQBArInvoice, string queueName)
        {


            Type myType = myQBArInvoice.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();



            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, myQBArInvoice, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AR Invoice");


            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);
            
            try
            {

                DbQueueService queueservice;
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("QBO", 0);
                Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "Invoice" },{ "OldTransferStatusCode", OldTransferStatusCode } };
                queueservice.Send(param, tenant);
                queueservice.Complete();          
                  }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quickbooks online web service", null, ip);
                throw ex;
            }



        }

        private  void SendXMLFileMemo(Intuit.Ipp.Data.CreditMemo myQBArInvoice, string queueName)
        {


            Type myType = myQBArInvoice.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();



            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, myQBArInvoice, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AR Invoice");


            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            try
            {
                DbQueueService queueservice;
                queueservice = new DbQueueService();

                queueservice.InitializeQueue("QBO", 0);
                queueservice.Send(new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "MEMO" } }, tenant);
                queueservice.Complete();
                  }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quickbooks online web service", null, ip);
                throw ex;
            }



        }

        private void SendXMLFileInvoiceVoid(string ARInvoiceExternalId, string queueName)
        {


            Type myType = ARInvoiceExternalId.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();



            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, ARInvoiceExternalId, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AR Invoice Void");

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            try
            {
                DbQueueService queueservice;
                queueservice = new DbQueueService();

                queueservice.InitializeQueue("QBO", 0);
                queueservice.Send(new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "ARInvoiceVoid" } }, tenant);
                queueservice.Complete();
            }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quickbooks online web service", null, ip);
                throw ex;
            }



        }


        private void BuildCommunicationLog(int? fileSize,string subject)
        {

            string xmlSubject = subject;
            string xmlTarget = "QBO";


            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = xmlTarget.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();


            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                From = tenantName,
                EntityId = ARInvoiceId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = loggedContactId,
                DocumentId = document.Id,
                EntityReference = ARInvoice.InvoiceNumber,
                SearchFields = ARInvoice.InvoiceNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = DateTime.UtcNow,
            };


            communicationLogRepository.Add(commLog);

            myDocumentId = document.Id;
            myDocumentFolder = document.Folder;
            myDocumentExtension = document.Extension;
            myCommunicationLogId = commLog.Id;
            communicationLogRepository.SubmitChanges();
        }


        private  bool FieldIsEmpty(string myField)
        {
            bool myResult = false;

            if (myField == null || (myField != null && string.IsNullOrEmpty(myField.Trim())))
            {
                myResult = true;
            }

            return myResult;
        }





    }
}
