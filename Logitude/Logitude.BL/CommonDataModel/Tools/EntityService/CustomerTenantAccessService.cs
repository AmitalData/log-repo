using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerTenantAccessService
    {
        private Tenant loggedTenant;
        private ContactPM loggedContact;
        bool isNewEntity;
        private int tenant;
        private string serviceContextUser;
        public CustomerTenantAccess Poco { get; set; }
        public CustomerTenantAccessCardPM customerTenantAccessCardPM;
        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private CustomerTenantAccess entity;
        private CustomerTenantAccessPM entityPM;
        private ICommonDataContext objectContext;
        private CustomerTenantAccessRepository entityRepository;
        private CustomerTenantAccessCardRepository customerTenantAccessCardRepository;
        private CustomerTenantAccessCardsBatchRepository customerTenantAccessCardsBatchRepository;
        public CustomerTenantAccessService(ICommonDataContext objectContext, int tenant, CustomerTenantAccessPM entityPM, string serviceContextUser)
        {
            this.tenant = tenant;
            this.entityPM = entityPM;
            this.ObjectContext = objectContext;
            this.serviceContextUser = serviceContextUser;
            this.entityRepository = new CustomerTenantAccessRepository(objectContext);
            this.customerTenantAccessCardRepository = new CustomerTenantAccessCardRepository(objectContext);
            this.customerTenantAccessCardsBatchRepository = new CustomerTenantAccessCardsBatchRepository(objectContext);

            ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(serviceContextUser, tenant);
            }
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);


        }

        public CustomerTenantAccessService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            //this.entityPM = entityPM;
            this.ObjectContext = objectContext;
            this.serviceContextUser = SecurityUtility.GetAuthenticatedUser();//serviceContextUser;
            this.entityRepository = new CustomerTenantAccessRepository(objectContext);
            this.customerTenantAccessCardRepository = new CustomerTenantAccessCardRepository(objectContext);
            this.customerTenantAccessCardsBatchRepository = new CustomerTenantAccessCardsBatchRepository(objectContext);

            ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(serviceContextUser, tenant);
            }
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);


        }
        private List<CustomerTenantAccessCardPM> customerTenantAccessCardsChangeSet;
        public void SetChangeSet(List<CustomerTenantAccessCardPM> customerTenantAccessCardsChangeSet)
        {
            this.customerTenantAccessCardsChangeSet = customerTenantAccessCardsChangeSet;
        }

        public void Create(CustomerTenantAccessPM entityPMParam = null)
        {
            if (entityPMParam != null)
            {
                this.entityPM = entityPMParam;
            }
            this.isNewEntity = true;

            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository rep = new ContactRepository(tenant);
            var contact = rep.GetSingleContactByEmail(email, tenant, false);

            this.entityPM.Id = IdCounter.GetNumber("CustomerTenantAccess", tenant).ToString();
            this.Poco = new CustomerTenantAccess();
            this.Poco.Id = this.entityPM.Id;
            CustomerTenantAccessMapping.MapEntity(entityPM, Poco, true, loggedContact.Id, loggedTenant);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            foreach (CustomerTenantAccessCardPM itemPM in entityPM.CustomerTenantAccessCards)
            {
                this.CreateCustomerTenantAccessCard(itemPM);
            }

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CustomerTenantAccess", 0, true);
            //ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", contact.Id);

        }

        public void Delete()
        {
            this.Poco = entityRepository.GetSingleCustomerTenantAccess(entityPM.Id, tenant);
            foreach (CustomerTenantAccessCardPM pm in entityPM.CustomerTenantAccessCards)
            {
                this.DeleteCustomerTenantAccessCard(pm);
            }
        }

        public void Update(CustomerTenantAccessPM paramentityPM = null, bool temp = false)
        {
            if (paramentityPM != null)
            {
                ValidateCustomerTenantAccessCards(paramentityPM);
                this.entityPM = paramentityPM;
            }
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleCustomerTenantAccess(entityPM.Id, tenant);
            CustomerTenantAccessMapping.MapEntity(entityPM, Poco, false, loggedContact.Id, loggedTenant);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            //IQueueService queueservice = new DbQueueService();
            //queueservice.InitializeQueue("CustomerTenantAccessQueue", 0);
            //queueservice.Send(new Dictionary<string, string>() { { "CustomerTenant", entityPM.CustomerTenant.ToString() }, { "PartnerTenant", entityPM.Tenant.ToString() }, { "Id", entityPM.Id.ToString() }, { "Tenant", tenant.ToString() } });
            SetChangeSet(entityPM.CustomerTenantAccessCards);
            this.CustomerTenantAccessCardsCollection();


            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CustomerTenantAccess", 0, true);
            //ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.UpdatedByUserId);
        }

        private static void ValidateCustomerTenantAccessCards(CustomerTenantAccessPM paramentityPM)
        {
            if (paramentityPM.CustomerTenantAccessCards == null)
                return;
            var InValidCustomerTenantAccessCards = paramentityPM.CustomerTenantAccessCards.Find(a => a.IsCustomsActivated == false && a.IsExportActivated == false);
            if (InValidCustomerTenantAccessCards != null)
            {
                throw new Exception("You have to choose either Export or Customs option.");
            }
        }

        private void CustomerTenantAccessCardsCollection()
        {
            if (customerTenantAccessCardsChangeSet != null)
            {
                foreach (CustomerTenantAccessCardPM itemPM in customerTenantAccessCardsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerTenantAccessCard(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerTenantAccessCard(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerTenantAccessCard(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateCustomerTenantAccessCard(CustomerTenantAccessCardPM itemPM)
        {
            bool BuildBatch = itemPM.BuildBatch;
            itemPM.Tenant = entityPM.Tenant;
            itemPM.CustomerTenantAccessId = entityPM.Id;

            CustomerTenantAccessCard itemPoco = new CustomerTenantAccessCard()
            {
                CustomerId = itemPM.CustomerId,
                CustomerTenantAccessId = itemPM.CustomerTenantAccessId,
            };

            CustomerTenantAccessMapping.MapCustomerTenantAccessCard(itemPM, itemPoco, true, loggedContact.Id, loggedTenant);
            customerTenantAccessCardRepository.Add(itemPoco);
            customerTenantAccessCardRepository.SubmitChanges();
            if (itemPoco.StatusTypeCode == "A")
            {
                CustomerQuery CustomerQuery = new CustomerQuery(itemPoco.Tenant);
                var TempCustomer = CustomerQuery.GetSinglePMForLogBox(itemPoco.CustomerId, itemPoco.Tenant);
                if (this.entityPM.IsPrivateLabelCustomer == true)
                {
                    TempCustomer.IsPrivateLabelCustomer = true;
                    TempCustomer.LogBoxActivated = false;
                }
                else
                {
                    TempCustomer.IsPrivateLabelCustomer = false;
                    TempCustomer.LogBoxActivated = true;
                }

                TempCustomer.IsLogBox = true;

                CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
                bool ishascard = customerTenantAccessCardQuery.IsCustomerTenantAccessHasCards(entityPM.Id);
                if (!ishascard)
                {
                    InitializeCustomerTenantAccessQueue(itemPM);

                }

                var contactRepository = new ContactRepository(objectContext);
                var Contact = contactRepository.GetSingleContactByEmailAndTenant(serviceContextUser, itemPoco.Tenant);
                CustomerService CustomerService = new CustomerService(ObjectContext, TempCustomer, Contact.Id);
                CustomerService.Update();
            }
            else
            {
                CustomerQuery CustomerQuery = new CustomerQuery(itemPoco.Tenant);
                var TempCustomer = CustomerQuery.GetSinglePMForLogBox(itemPoco.CustomerId, itemPoco.Tenant);
                TempCustomer.LogBoxActivated = false;
                TempCustomer.IsPrivateLabelCustomer = false;
                TempCustomer.IsLogBox = true;
                var contactRepository = new ContactRepository(objectContext);
                var Contact = contactRepository.GetSingleContactByEmailAndTenant(serviceContextUser, itemPoco.Tenant);
                CustomerService CustomerService = new CustomerService(ObjectContext, TempCustomer, Contact.Id);
                CustomerService.Update();
            }
            string key = itemPM.CustomerId + "_" + tenant + "_info";
            if (CacheManager.CacheWrapper.Get(key) != null)
            {
                CacheManager.CacheWrapper.Invalidate(key);
            }
            try
            {



                if (BuildBatch)
                {
                    CustomerTenantAccessCardsBatchPM CustomerTenantAccessCardsBatchPM = new CustomerTenantAccessCardsBatchPM()
                    {
                        CustomerId = itemPoco.CustomerId,
                        CustomerTenantAccessId = itemPoco.CustomerTenantAccessId,
                        ToDatetime = DateTime.Now,
                        FromDatetime = itemPM.HybridStartDate,//itemPoco.HybridStartDate,
                        Status = "Created",
                        Tenant = itemPoco.Tenant,
                        TotalFailed = 0,
                        TotalShipment = 0,
                        Totalsucceeded = 0
                    };

                    CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(ObjectContext, CustomerTenantAccessCardsBatchPM.Tenant, CustomerTenantAccessCardsBatchPM);
                    customerTenantAccessCardsBatchService.Create();

                    //queueservice.InitializeQueue("ImporterShipmentsQueueBuilderQueue", 0);
                    //queueservice.Send(new Dictionary<string, string>() { { "CustomerId", CustomerTenantAccessCardsBatchPM.CustomerId.ToString() }, { "CustomerTenantAccessId", CustomerTenantAccessCardsBatchPM.CustomerTenantAccessId.ToString() }, { "tenant", tenant.ToString() }, { "BatchNumber", CustomerTenantAccessCardsBatchPM.BatchNumber } });

                }
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
            }

        }
        private void UpdateCustomerTenantAccessCard(CustomerTenantAccessCardPM itemPM)
        {
            bool BuildBatch = itemPM.BuildBatch;
            CustomerTenantAccessCard itemPoco = customerTenantAccessCardRepository.GetSingleCustomerTenantAccessCard(itemPM.CustomerTenantAccessId, itemPM.CustomerId, itemPM.Tenant);
            CustomerTenantAccessMapping.MapCustomerTenantAccessCard(itemPM, itemPoco, false, loggedContact.Id, loggedTenant);
            this.customerTenantAccessCardPM = itemPM;



            customerTenantAccessCardRepository.Update(itemPoco);
            customerTenantAccessCardRepository.SubmitChanges();



            if (itemPM.StatusTypeCode == "A")
            {
                InitializeCustomerTenantAccessQueue(itemPM);

                CustomerQuery CustomerQuery = new CustomerQuery(itemPM.Tenant);
                var TempCustomer = CustomerQuery.GetSinglePMForLogBox(itemPM.CustomerId, itemPM.Tenant);
                if (this.entityPM.IsPrivateLabelCustomer == true)
                {
                    TempCustomer.IsPrivateLabelCustomer = true;
                    TempCustomer.LogBoxActivated = false;
                }
                else
                {
                    TempCustomer.IsPrivateLabelCustomer = false;
                    TempCustomer.LogBoxActivated = true;
                }
                TempCustomer.IsLogBox = true;
                var contactRepository = new ContactRepository(objectContext);
                var Contact = contactRepository.GetSingleContactByEmailAndTenant(serviceContextUser, itemPM.Tenant);
                TempCustomer.CustomerTenant = this.entityPM.CustomerTenant;
                TempCustomer.AddLogboxCustomerQueue = true;
                CustomerService CustomerService = new CustomerService(ObjectContext, TempCustomer, Contact.Id);

                CustomerService.Update();
                TempCustomer.AddLogboxCustomerQueue = false;
            }
            else
            {
                CustomerQuery CustomerQuery = new CustomerQuery(itemPM.Tenant);
                var TempCustomer = CustomerQuery.GetSinglePMForLogBox(itemPM.CustomerId, itemPM.Tenant);
                TempCustomer.LogBoxActivated = false;
                TempCustomer.IsPrivateLabelCustomer = false;
                TempCustomer.IsLogBox = true;
                var contactRepository = new ContactRepository(objectContext);
                var Contact = contactRepository.GetSingleContactByEmailAndTenant(serviceContextUser, itemPM.Tenant);
                TempCustomer.CustomerTenant = this.entityPM.CustomerTenant;
                //TempCustomer.AddLogboxCustomerQueue = true;
                CustomerService CustomerService = new CustomerService(ObjectContext, TempCustomer, Contact.Id);
                CustomerService.Update();
                TempCustomer.AddLogboxCustomerQueue = false;
            }
            string key = itemPM.CustomerId + "_" + tenant + "_info";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(key) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(key);
                }
            }
            //if (itemPM.StatusTypeCode.ToUpper() == "A" && BuildBatch)
            //{
            //    CustomerTenantAccessCardsBatchPM CustomerTenantAccessCardsBatchPM = new CustomerTenantAccessCardsBatchPM()
            //    {
            //        CustomerId = itemPoco.CustomerId,
            //        CustomerTenantAccessId = itemPoco.CustomerTenantAccessId,
            //        ToDatetime = DateTime.Now,
            //        //DoneDate = itemPoco.DoneDate,
            //        FromDatetime = itemPoco.HybridStartDate,
            //        Status = "Created",
            //        Tenant = itemPoco.Tenant,
            //        TotalFailed = 0,
            //        TotalShipment = 0,
            //        Totalsucceeded = 0
            //    };

            //    CustomerTenantAccessCardsBatchService customerTenantAccessCardsBatchService = new CustomerTenantAccessCardsBatchService(ObjectContext, CustomerTenantAccessCardsBatchPM.Tenant, CustomerTenantAccessCardsBatchPM);
            //    customerTenantAccessCardsBatchService.Create();
            //    IQueueService queueservice = new DbQueueService();
            //    queueservice.InitializeQueue("ImporterShipmentsQueueBuilderQueue", 0);
            //    queueservice.Send(new Dictionary<string, string>() { { "CustomerId", CustomerTenantAccessCardsBatchPM.CustomerId.ToString() }, { "CustomerTenantAccessId", CustomerTenantAccessCardsBatchPM.CustomerTenantAccessId.ToString() }, { "tenant", tenant.ToString() }, { "BatchNumber", CustomerTenantAccessCardsBatchPM.BatchNumber } });

            //}

        }

        private void InitializeCustomerTenantAccessQueue(CustomerTenantAccessCardPM itemPM)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CustomerTenantAccessQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "IsCustomsActivated", itemPM.IsCustomsActivated.ToString() }, { "IsExportActivated", itemPM.IsExportActivated.ToString() }, { "CustomerTenant", entityPM.CustomerTenant.ToString() }, { "PartnerTenant", entityPM.Tenant.ToString() }, { "Id", entityPM.Id.ToString() }, { "Tenant", tenant.ToString() } }, tenant);
        }

        private void DeleteCustomerTenantAccessCard(CustomerTenantAccessCardPM itemPM)
        {
            CustomerTenantAccessCard itemPoco = customerTenantAccessCardRepository.GetSingleCustomerTenantAccessCard(itemPM.CustomerTenantAccessId, itemPM.CustomerId, itemPM.Tenant);

            customerTenantAccessCardRepository.Remove(itemPoco);


        }

    }
}
