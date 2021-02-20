using System.Collections.Generic;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Linq;
using System;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class AccountingTransferHeaderService
    {
        public AccountingTransferHeader entityPoco { get; set; }
        private int tenant;
        private bool isNewEntity;
        private Tenant loggedTenant;
        private string loggedContactId;
        private AccountingTransferHeaderPM entityPM;
        private IInvoiceContext objectContext;
        private AccountingTransferHeaderRepository entityRepository;
        private AccountingTransferLineRepository accountingTransferLineRepository;

        // ARIN: ARInvoice Transfer
        // APIN: APInvoice Transfer
        // ARPA: ARPayment Transfer
        // APPA: APPayment Transfer
        private List<ARInvoice> aRInvoices;
        private List<APInvoice> aPInvoices;
        private List<ARPayment> aRPayments;
        private List<APPayment> aPPayments;
        private ARInvoiceRepository aRInvoiceRepository;
        private APInvoiceRepository aPInvoiceRepository;
        private ARPaymentRepository aRPaymentRepository;
        private APPaymentRepository aPPaymentRepository;
        private MessageEntityService webService;
        public AccountingTransferHeaderService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AccountingTransferHeaderRepository(objectContext);
            this.accountingTransferLineRepository = new AccountingTransferLineRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);

            this.aRInvoices = new List<ARInvoice>();
            this.aPInvoices = new List<APInvoice>();
            this.aRPayments = new List<ARPayment>();
            this.aPPayments = new List<APPayment>();
            this.aRInvoiceRepository = new ARInvoiceRepository(objectContext);
            this.aPInvoiceRepository = new APInvoiceRepository(objectContext);
            this.aRPaymentRepository = new ARPaymentRepository(objectContext);
            this.aPPaymentRepository = new APPaymentRepository(objectContext);
            this.webService = new MessageEntityService();

            this.GetLoggedContact();            
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(tenant);

            string email = HttpContext.Current.User.Identity.Name;

            if (email != null)
            {
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                this.loggedContactId = loggedContact.Id;
            }

            else
            {
                ContactPM loggedContact = new ContactQuery(tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
                this.loggedContactId = loggedContact.Id;
            }
        }

        private void GetAllEntities()
        {
            List<string> ids = entityPM.TransferLines.Select(s => s.EntityId).ToList();

            if (ids.Count > 0)
            {
                switch (entityPM.AccountingTransferTypeCode)
                {
                    case "ARIN":
                        {
                            this.aRInvoices = aRInvoiceRepository.GetInvoicesListFromIdList(ids, tenant);
                            break;
                        }

                    case "APIN":
                        {
                            this.aPInvoices = aPInvoiceRepository.GetInvoicesListFromIdList(ids, tenant);
                            break;
                        }

                    case "ARPA":
                        {
                            this.aRPayments = aRPaymentRepository.GetPaymentsListFromIdList(ids, tenant);
                            break;
                        }

                    case "APPA":
                        {
                            this.aPPayments = aPPaymentRepository.GetPaymentsListFromIdList(ids, tenant);
                            break;
                        }
                }
            }
        }

        private List<AccountingTransferLinePM> transferLinesChangeSet;
        public void SetChangeSet(List<AccountingTransferLinePM> transferLinesChangeSet)
        {
            this.transferLinesChangeSet = transferLinesChangeSet;
        }

        public void Create(AccountingTransferHeaderPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.GetAllEntities();
            this.entityPM.Id = IdCounter.GetNumber("AccountingTransferHeader", tenant).ToString();

            this.entityPoco = new AccountingTransferHeader()
            {
                Id = entityPM.Id,
                Tenant = tenant,
                AccountingTransferTypeCode = entityPM.AccountingTransferTypeCode,
            };

            this.InitializeComponent();
            this.UpdateTransferLines();

            AccountingTransferHeaderTracing.Trace(entityPM, entityPoco, isNewEntity);
            AccountingTransferHeaderMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();

            this.TransferData();
        }

        public void Update(AccountingTransferHeaderPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.GetAllEntities();
            this.entityPoco = entityRepository.GetSingleEntity(entityPM.Id, tenant);

            if (mapComposition)
            {
                this.transferLinesChangeSet = this.entityPM.TransferLines;
            }

            this.InitializeComponent();
            this.UpdateTransferLines();

            AccountingTransferHeaderMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                if (entityPM.TransferDate == null)
                {
                    entityPM.TransferDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }

                if (string.IsNullOrEmpty(entityPM.TransferNumber))
                {
                    int counter = CodeCounter.GetNumber("AccountingTransferHeader", tenant);
                    entityPM.TransferNumber = counter.ToString();
                }

                this.SetFileName();
            }

            entityPM.UserId = loggedContactId;
        }

        private void SetFileName()
        {
            if (isNewEntity)
            {
                AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);

                switch (entityPM.AccountingTransferTypeCode)
                {
                    case "ARIN":
                        {
                            if (accountingSetting.AccountingSystemCode == "HV")
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".doc";
                            }

                            else if (accountingSetting.AccountingSystemCode == "GI" || accountingSetting.AccountingSystemCode == "AI")
                            {
                                entityPM.FileName = "ARInvoices" + entityPM.TransferNumber + ".xml";
                            }

                            else
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".dat";
                            }

                            break;
                        }

                    case "APIN":
                        {
                            if (accountingSetting.AccountingSystemCode == "HV")
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".doc";
                            }

                            else if (accountingSetting.AccountingSystemCode == "GI" || accountingSetting.AccountingSystemCode == "AI")
                            {
                                entityPM.FileName = "APInvoices" + entityPM.TransferNumber + ".xml";
                            }

                            else
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".dat";
                            }

                            break;
                        }

                    case "ARPA":
                        {
                            if (accountingSetting.AccountingSystemCode == "HV")
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".doc";
                            }

                            else if (accountingSetting.AccountingSystemCode == "GI" || accountingSetting.AccountingSystemCode == "AI")
                            {
                                entityPM.FileName = "ARPayments" + entityPM.TransferNumber + ".xml";
                            }

                            else
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".dat";
                            }

                            break;
                        }

                    case "APPA":
                        {
                            if (accountingSetting.AccountingSystemCode == "HV")
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".doc";
                            }

                            else if (accountingSetting.AccountingSystemCode == "GI" || accountingSetting.AccountingSystemCode == "AI")
                            {
                                entityPM.FileName = "APPayments" + entityPM.TransferNumber + ".xml";
                            }

                            else
                            {
                                entityPM.FileName = "movein" + entityPM.TransferNumber + ".dat";
                            }

                            break;
                        }
                }
            }
        }

        private void UpdateTransferLines()
        {
            if (isNewEntity)
            {
                foreach (AccountingTransferLinePM itemPM in entityPM.TransferLines)
                {
                    this.CreateAccountingTransferLine(itemPM);
                }
            }

            else
            {
                if (transferLinesChangeSet != null)
                {
                    foreach (AccountingTransferLinePM itemPM in transferLinesChangeSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateAccountingTransferLine(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateAccountingTransferLine(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteAccountingTransferLine(itemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }
        }

        private void CreateAccountingTransferLine(AccountingTransferLinePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("AccountingTransferLine", tenant).ToString();
            itemPM.AccountingTransferHeaderId = entityPM.Id;

            AccountingTransferLine itemPoco = new AccountingTransferLine()
            {
                Id = itemPM.Id,
                Tenant = tenant,
                AccountingTransferHeaderId = itemPM.AccountingTransferHeaderId,
            };

            AccountingTransferLineMapping.MapEntity(itemPM, itemPoco, true);
            accountingTransferLineRepository.Add(itemPoco);

            switch (entityPM.AccountingTransferTypeCode)
            {
                case "ARIN":
                    {
                        UpdateARInvoice(itemPM.EntityId);
                        break;
                    }

                case "APIN":
                    {
                        UpdateAPInvoice(itemPM.EntityId);
                        break;
                    }

                case "ARPA":
                    {
                        UpdateARPayment(itemPM.EntityId);
                        break;
                    }

                case "APPA":
                    {
                        UpdateAPPayment(itemPM.EntityId);
                        break;
                    }
            }
        }
        private void UpdateAccountingTransferLine(AccountingTransferLinePM itemPM)
        {
            AccountingTransferLine itemPoco = accountingTransferLineRepository.GetSingleEntity(itemPM.Id);
            if (itemPoco != null)
            {
                AccountingTransferLineMapping.MapEntity(itemPM, itemPoco, false);
                accountingTransferLineRepository.Update(itemPoco);
            }
        }
        private void DeleteAccountingTransferLine(AccountingTransferLinePM itemPM)
        {
            AccountingTransferLine itemPoco = accountingTransferLineRepository.GetSingleEntity(itemPM.Id);
            if (itemPoco != null)
            {
                accountingTransferLineRepository.Remove(itemPoco);
            }
        }

        private void UpdateARInvoice(string myEntityId)
        {
            ARInvoice invoice = aRInvoices.Where(d => d.Id == myEntityId).FirstOrDefault();

            if (invoice != null)
            {
                invoice.TransferStatusCode = "TR";
                invoice.TransferError = null;
                aRInvoiceRepository.Update(invoice);

            }
        }
        private void UpdateAPInvoice(string myEntityId)
        {
            APInvoice invoice = aPInvoices.Where(d => d.Id == myEntityId).FirstOrDefault();

            if (invoice != null)
            {
                invoice.TransferStatusCode = "TR";
                invoice.TransferError = null;
                aPInvoiceRepository.Update(invoice);
            }
        }
        private void UpdateARPayment(string myEntityId)
        {
            ARPayment myPayment = aRPayments.Where(d => d.Id == myEntityId).FirstOrDefault();

            if (myPayment != null)
            {
                myPayment.TransferStatusCode = "TR";
                myPayment.TransferError = null;
                aRPaymentRepository.Update(myPayment);
            }
        }
        private void UpdateAPPayment(string myEntityId)
        {
            APPayment myPayment = aPPayments.Where(d => d.Id == myEntityId).FirstOrDefault();

            if (myPayment != null)
            {
                myPayment.TransferStatusCode = "TR";
                myPayment.TransferError = null;
                aPPaymentRepository.Update(myPayment);
            }
        }
        private void TransferData()
        {
            if (isNewEntity)
            {
                switch (entityPM.AccountingTransferTypeCode)
                {
                    case "ARIN":
                        {
                            webService.TransferARInvoices(aRInvoices, entityPM.FileName, tenant);
                            break;
                        }

                    case "APIN":
                        {
                            webService.TransferAPInvoices(aPInvoices, entityPM.FileName, tenant);
                            break;
                        }

                    case "ARPA":
                        {
                            webService.TransferARPayments(aRPayments, entityPM.FileName, tenant);
                            break;
                        }

                    case "APPA":
                        {
                            webService.TransferAPPayments(aPPayments, entityPM.FileName, tenant);
                            break;
                        }
                }
            }
        }
    }
}