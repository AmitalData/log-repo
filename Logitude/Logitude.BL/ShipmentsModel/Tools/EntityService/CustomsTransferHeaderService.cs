using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class CustomsTransferHeaderService
    {
        public CustomsTransferHeader entityPoco { get; set; }
        private int tenant;
        private bool isNewEntity;
        private Tenant loggedTenant;
        private string loggedContactId;
        private CustomsTransferHeaderPM entityPM;
        private IShipmentsContext objectContext;
        private CustomsTransferHeaderRepository entityRepository;
        private CustomsTransferLineRepository CustomsTransferLineRepository;
        
        //private List<ARInvoice> aRInvoices;
        //private List<APInvoice> aPInvoices;
        //private List<ARPayment> aRPayments;
        //private List<APPayment> aPPayments;
        //private ARInvoiceRepository aRInvoiceRepository;
        //private APInvoiceRepository aPInvoiceRepository;
        //private ARPaymentRepository aRPaymentRepository;
        //private APPaymentRepository aPPaymentRepository;
        //private MessageEntityService webService;
        public CustomsTransferHeaderService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CustomsTransferHeaderRepository(objectContext);
            this.CustomsTransferLineRepository = new CustomsTransferLineRepository(objectContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);

            //this.aRInvoices = new List<ARInvoice>();
            //this.aPInvoices = new List<APInvoice>();
            //this.aRPayments = new List<ARPayment>();
            //this.aPPayments = new List<APPayment>();
            //this.aRInvoiceRepository = new ARInvoiceRepository(objectContext);
            //this.aPInvoiceRepository = new APInvoiceRepository(objectContext);
            //this.aRPaymentRepository = new ARPaymentRepository(objectContext);
            //this.aPPaymentRepository = new APPaymentRepository(objectContext);
            //this.webService = new MessageEntityService();

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
            List<string> ids = entityPM.CustomsTransferLines.Select(s => s.ShipmentId).ToList();

            if (ids.Count > 0)
            {
                switch (entityPM.CustomsTransferTypeCode)
                {
                    //case "ARIN":
                    //    {
                    //        this.aRInvoices = aRInvoiceRepository.GetInvoicesListFromIdList(ids, tenant);
                    //        break;
                    //    }

                    //case "APIN":
                    //    {
                    //        this.aPInvoices = aPInvoiceRepository.GetInvoicesListFromIdList(ids, tenant);
                    //        break;
                    //    }

                    //case "ARPA":
                    //    {
                    //        this.aRPayments = aRPaymentRepository.GetPaymentsListFromIdList(ids, tenant);
                    //        break;
                    //    }

                    //case "APPA":
                    //    {
                    //        this.aPPayments = aPPaymentRepository.GetPaymentsListFromIdList(ids, tenant);
                    //        break;
                    //    }
                }
            }
        }

        private List<CustomsTransferLinePM> transferLinesChangeSet;
        public void SetChangeSet(List<CustomsTransferLinePM> transferLinesChangeSet)
        {
            this.transferLinesChangeSet = transferLinesChangeSet;
        }

        public void Create(CustomsTransferHeaderPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.GetAllEntities();
            this.entityPM.Id = IdCounter.GetNumber("CustomsTransferHeader", tenant).ToString();

            this.entityPoco = new CustomsTransferHeader()
            {
                Id = entityPM.Id,
                Tenant = tenant,
                CustomsTransferTypeCode = entityPM.CustomsTransferTypeCode,
            };

            this.InitializeComponent();
            this.UpdateTransferLines();

            CustomsTransferHeaderTracing.Trace(entityPM, entityPoco, isNewEntity);
            CustomsTransferHeaderMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();

            //this.TransferData();
        }

        public void Update(CustomsTransferHeaderPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.GetAllEntities();
            this.entityPoco = entityRepository.GetSingleEntity(entityPM.Id, tenant);

            if (mapComposition)
            {
                this.transferLinesChangeSet = this.entityPM.CustomsTransferLines;
            }

            this.InitializeComponent();
            this.UpdateTransferLines();

            CustomsTransferHeaderMapping.MapEntity(entityPM, entityPoco, isNewEntity);

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
                    int counter = CodeCounter.GetNumber("CustomsTransferHeader", tenant);
                    entityPM.TransferNumber = counter.ToString();
                }

                this.SetFileName();
            }

            entityPM.CreatedByUserId = loggedContactId;
        }

        private void SetFileName()
        {
            if (isNewEntity)
            {
                AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);

                switch (entityPM.CustomsTransferTypeCode)
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
                foreach (CustomsTransferLinePM itemPM in entityPM.CustomsTransferLines)
                {
                    this.CreateCustomsTransferLine(itemPM);
                }
            }

            else
            {
                if (transferLinesChangeSet != null)
                {
                    foreach (CustomsTransferLinePM itemPM in transferLinesChangeSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateCustomsTransferLine(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateCustomsTransferLine(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteCustomsTransferLine(itemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }
        }

        private void CreateCustomsTransferLine(CustomsTransferLinePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CustomsTransferLine", tenant).ToString();
            itemPM.CustomsTransferHeaderId = entityPM.Id;

            CustomsTransferLine itemPoco = new CustomsTransferLine()
            {
                Id = itemPM.Id,
                Tenant = tenant,
                CustomsTransferHeaderId = itemPM.CustomsTransferHeaderId,
            };

            CustomsTransferHeaderMapping.MapLine(itemPM, itemPoco, true);
            CustomsTransferLineRepository.Add(itemPoco);

            switch (entityPM.CustomsTransferTypeCode)
            {
                //case "ARIN":
                //    {
                //        UpdateARInvoice(itemPM.ShipmentId);
                //        break;
                //    }

                //case "APIN":
                //    {
                //        UpdateAPInvoice(itemPM.EntityId);
                //        break;
                //    }

                //case "ARPA":
                //    {
                //        UpdateARPayment(itemPM.EntityId);
                //        break;
                //    }

                //case "APPA":
                //    {
                //        UpdateAPPayment(itemPM.EntityId);
                //        break;
                //    }
            }
        }
        private void UpdateCustomsTransferLine(CustomsTransferLinePM itemPM)
        {
            CustomsTransferLine itemPoco = CustomsTransferLineRepository.GetSingleEntity(itemPM.Id);
            if (itemPoco != null)
            {
                CustomsTransferHeaderMapping.MapLine(itemPM, itemPoco, false);
                CustomsTransferLineRepository.Update(itemPoco);
            }
        }
        private void DeleteCustomsTransferLine(CustomsTransferLinePM itemPM)
        {
            CustomsTransferLine itemPoco = CustomsTransferLineRepository.GetSingleEntity(itemPM.Id);
            if (itemPoco != null)
            {
                CustomsTransferLineRepository.Remove(itemPoco);
            }
        }

        //private void UpdateARInvoice(string myEntityId)
        //{
        //    ARInvoice invoice = aRInvoices.Where(d => d.Id == myEntityId).FirstOrDefault();

        //    if (invoice != null)
        //    {
        //        invoice.TransferStatusCode = "TR";
        //        invoice.TransferError = null;
        //        aRInvoiceRepository.Update(invoice);
        //    }
        //}
        //private void UpdateAPInvoice(string myEntityId)
        //{
        //    APInvoice invoice = aPInvoices.Where(d => d.Id == myEntityId).FirstOrDefault();

        //    if (invoice != null)
        //    {
        //        invoice.TransferStatusCode = "TR";
        //        invoice.TransferError = null;
        //        aPInvoiceRepository.Update(invoice);
        //    }
        //}
        //private void UpdateARPayment(string myEntityId)
        //{
        //    ARPayment myPayment = aRPayments.Where(d => d.Id == myEntityId).FirstOrDefault();

        //    if (myPayment != null)
        //    {
        //        myPayment.TransferStatusCode = "TR";
        //        myPayment.TransferError = null;
        //        aRPaymentRepository.Update(myPayment);
        //    }
        //}
        //private void UpdateAPPayment(string myEntityId)
        //{
        //    APPayment myPayment = aPPayments.Where(d => d.Id == myEntityId).FirstOrDefault();

        //    if (myPayment != null)
        //    {
        //        myPayment.TransferStatusCode = "TR";
        //        myPayment.TransferError = null;
        //        aPPaymentRepository.Update(myPayment);
        //    }
        //}
        //private void TransferData()
        //{
        //    if (isNewEntity)
        //    {
        //        switch (entityPM.AccountingTransferTypeCode)
        //        {
        //            case "ARIN":
        //                {
        //                    webService.TransferARInvoices(aRInvoices, entityPM.FileName, tenant);
        //                    break;
        //                }

        //            case "APIN":
        //                {
        //                    webService.TransferAPInvoices(aPInvoices, entityPM.FileName, tenant);
        //                    break;
        //                }

        //            case "ARPA":
        //                {
        //                    webService.TransferARPayments(aRPayments, entityPM.FileName, tenant);
        //                    break;
        //                }

        //            case "APPA":
        //                {
        //                    webService.TransferAPPayments(aPPayments, entityPM.FileName, tenant);
        //                    break;
        //                }
        //        }
        //    }
        //}
    }
}
