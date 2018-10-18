using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;

namespace CommunicationWorkerRole
{
    public class TenantStatisticsWorkerRole : WorkerEntryPoint
    {

        private bool serviceStarted = true;
        private int interval = 1;
        private int systemErrorCount = 0;
        QueueDescription queueDescription;
        QueueClient client;
       
        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                        GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();

                        List<TenantManagement> tenantManagements = tenantManagementRepository.GetTenants().ToList();
                        LastActivity = DateTime.UtcNow;
                        DateTime? oldestDate = tenantManagements.Min(t => t.StatisticsUpdateDate);
                        double total = 24;
                        if (oldestDate != null)
                        {
                            total = (DateTime.Now - oldestDate.Value).TotalHours;
                        }
                        //else
                        //{
                        //    total = 24;
                        //}
                        // refresh for all tenants
                        //
                        string hour = DateTime.Now.ToString("HH");
                        if (hour == "00" && total >= 24)
                        {
                            foreach (TenantManagement tenant in tenantManagements)
                            {
                                if (tenant.StatisticsUpdateDate != null)
                                {
                                    if ((DateTime.Now.Date - tenant.StatisticsUpdateDate.Value.Date).TotalHours >= 24)//tenant.StatisticsUpdateDate != null && 
                                    {
                                        RefreshTenantStatistics(tenant);
                                    }
                                }
                                else
                                {
                                    RefreshTenantStatistics(tenant);
                                }
                            }

                            tenantManagementRepository.SubmitChanges();
                            LogDoneItemInMemory();
                            Thread.Sleep(10000);
                        }
                        else
                        {
                            Thread.Sleep(60000);
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "tenant statistics worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }




        public void RefreshTenantStatistics(TenantManagement tenantManagement)
        {
            DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(tenantManagement.Id);
            DateTime lastweek = currentDate.AddDays(-7);
            DateTime lastmonth = currentDate.AddDays(-30);
          
            tenantManagement.TTY = tenantManagement.GlobalTenant.TTY;
            tenantManagement.StatisticsUpdateDate = DateTime.UtcNow;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {                
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenantManagement.Id);
                QuoteRepository quoteRepository = new QuoteRepository(tenantManagement.Id);
                CustomerRepository customerRepository = new CustomerRepository(tenantManagement.Id);
                ARInvoiceRepository arInvoiceRepository = new ARInvoiceRepository(tenantManagement.Id);
                APInvoiceRepository apInvoiceRepository = new APInvoiceRepository(tenantManagement.Id);
                TenantRepository tenantRepository = new TenantRepository(tenantManagement.Id);
                BookingRepository bookingRepository = new BookingRepository(tenantManagement.Id);
                ActivityRepository myActivityRepository = new ActivityRepository(tenantManagement.Id);
                OpportunityRepository myOpportunityRepository = new OpportunityRepository(tenantManagement.Id);
                CommunicationLogRepository myCommunicationLogRepository = new CommunicationLogRepository(tenantManagement.Id);
                ContactActivityLogRepository contactActivityLogRepository;
                Tenant tenant = tenantRepository.GetSingleTenant(tenantManagement.Id);
                if (tenant != null && tenant.Address != null)
                {
                    if (tenant.Address.Country != null)
                    {
                        tenantManagement.CountryName = tenant.Address.Country.EnglishName;
                    }
                }

                var objectContext = (shipmentRepository.context as IObjectContextAdapter).ObjectContext;
                objectContext.CommandTimeout = 120;

                var qobjectContext = (quoteRepository.context as IObjectContextAdapter).ObjectContext;
                qobjectContext.CommandTimeout = 120;


                var contactActivityLogContext = (quoteRepository.context as IObjectContextAdapter).ObjectContext;
                contactActivityLogContext.CommandTimeout = 120;
                
                var arobjectContext = (arInvoiceRepository.context as IObjectContextAdapter).ObjectContext;
                qobjectContext.CommandTimeout = 120;

                IQueryable<Shipment> shipments = shipmentRepository.GetShipmentsWithMasterData(tenantManagement.Id);
                IQueryable<Shipment> sharingShipments = shipmentRepository.GetSharingShipments(tenantManagement.Id);
                IQueryable<Quote> quotes = quoteRepository.GetQuotes(tenantManagement.Id);
                IQueryable<ARInvoice> arInvoices = arInvoiceRepository.GetIQueryableInvoices(tenantManagement.Id);
                IQueryable<APInvoice> apInvoices = apInvoiceRepository.GetIQueryableInvoices(tenantManagement.Id);
                IQueryable<Customer> customers = customerRepository.GetCustomers(tenantManagement.Id);
                IQueryable<Booking> bookings = bookingRepository.GetAll(tenantManagement.Id);
                IQueryable<Activity> myActivities = myActivityRepository.GetAll(tenantManagement.Id);
                IQueryable<Opportunity> myOpportunities = myOpportunityRepository.GetAll(tenantManagement.Id);
                IQueryable<CommunicationLog> myCommunicationLogs = myCommunicationLogRepository.GetCommunicationLogsByTenant(tenantManagement.Id);

                if (shipments.Count() > 0)
                {
                    tenantManagement.ShipmentLastDate = shipments.Max(s => s.CreateDateTime);
                    tenantManagement.LastFHLSentDate = shipments.Max(s => s.FHLStatusDate);
                    tenantManagement.LastFWBSentDate = shipments.Where(s => s.MasterShipmentDataId != null).Max(s => s.ShipmentMasterData.FWBStatusDate);                    
                    tenantManagement.LastFHLCargonautSentDate = shipments.Max(s => s.CargonautFHLStatusDate);
                    tenantManagement.LastFWBCargonautSentDate = shipments.Where(s => s.MasterShipmentDataId != null).Max(s => s.ShipmentMasterData.CargonautFWBStatusDate);
                    tenantManagement.ShipmentTotalLastWeek = shipments.Where(s => s.CreateDateTime >= lastweek).Count();
                    tenantManagement.ShipmentTotalLastMonth = shipments.Where(s => s.CreateDateTime >= lastmonth).Count();
                }

                if (quotes.Count() > 0)
                {
                    tenantManagement.QuoteLastDate = quotes.Max(s => s.OpenDate);
                    tenantManagement.QuoteTotalLastWeek = quotes.Where(s => s.OpenDate >= lastweek).Count();
                    tenantManagement.QuoteTotalLastMonth = quotes.Where(s => s.OpenDate >= lastmonth).Count();
                }

                if (arInvoices.Count() > 0)
                {
                    tenantManagement.ARInvoiceLastDate = arInvoices.Max(s => s.CreateDate);
                    tenantManagement.ARInvoiceTotalLastWeek = arInvoices.Where(s => s.CreateDate >= lastweek).Count();
                    tenantManagement.ARInvoiceTotalLastMonth = arInvoices.Where(s => s.CreateDate >= lastmonth).Count();
                }

                if (apInvoices.Count() > 0)
                {
                    tenantManagement.APInvoiceLastDate = apInvoices.Max(s => s.CreateDate);
                    tenantManagement.APInvoiceTotalLastWeek = apInvoices.Where(s => s.CreateDate >= lastweek).Count();
                    tenantManagement.APInvoiceTotalLastMonth = apInvoices.Where(s => s.CreateDate >= lastmonth).Count();
                }

                if (customers.Count() > 0)
                {
                    tenantManagement.CustomerLastDate = customers.Max(s => s.Card.CreateDate);
                    tenantManagement.CustomerTotalLastWeek = customers.Where(s => s.Card.CreateDate >= lastweek).Count();
                    tenantManagement.CustomerTotalLastMonth = customers.Where(s => s.Card.CreateDate >= lastmonth).Count();
                }

                if (bookings.Count() > 0)
                {
                    tenantManagement.LastFFRSentDate = bookings.Max(s => s.FFRStatusDate);
                }

                if (myActivities.Count() > 0)
                {
                    tenantManagement.ActivityLastDate = myActivities.Max(s => s.CreateDate);
                    tenantManagement.ActivityTotalLastWeek = myActivities.Where(s => s.CreateDate >= lastweek).Count();
                    tenantManagement.ActivityTotalLastMonth = myActivities.Where(s => s.CreateDate >= lastmonth).Count();
                }

                if (myOpportunities.Count() > 0)
                {
                    tenantManagement.OpportunityLastDate = myOpportunities.Max(s => s.CreateDate);
                    tenantManagement.OpportunityTotalLastWeek = myOpportunities.Where(s => s.CreateDate >= lastweek).Count();
                    tenantManagement.OpportunityTotalLastMonth = myOpportunities.Where(s => s.CreateDate >= lastmonth).Count();
                }


                if (sharingShipments.Count() > 0)
                {
                    tenantManagement.AgentSharedLogisticsStatisticsLastDate = sharingShipments.Max(s => s.ManifestLastSharingDate);
                    tenantManagement.AgentSharedLogisticsStatisticsLastWeek = sharingShipments.Where(s => s.ManifestLastSharingDate >= lastweek).Count();
                    tenantManagement.AgentSharedLogisticsStatisticsLastMonth = sharingShipments.Where(s => s.ManifestLastSharingDate >= lastmonth).Count();
                }


                try
                {
                    using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
                    {

                        contactActivityLogRepository = new ContactActivityLogRepository();
                        IQueryable<ContactActivityLog> contactActivityLogs = contactActivityLogRepository.GetContactActivityLogsInLogin(tenantManagement.Id);

                        if (contactActivityLogs.Count() > 0)
                        {
                            IQueryable<ContactActivityLog> MobilecontactActivityLogs = contactActivityLogs.Where(d => d.Via == "Mobile");
                            IQueryable<ContactActivityLog> ShardcontactActivityLogs = contactActivityLogs.Where(d => d.Via == "PC");

                            if (MobilecontactActivityLogs.Count() > 0)
                            {
                                tenantManagement.MobileLastDate = MobilecontactActivityLogs.Max(s => s.LogDateTime);
                                tenantManagement.MobileTotalLastWeek = MobilecontactActivityLogs.Where(s => s.LogDateTime >= lastweek).Count();
                                tenantManagement.MobileTotalLastMonth = MobilecontactActivityLogs.Where(s => s.LogDateTime >= lastmonth).Count();
                            }

                            if (ShardcontactActivityLogs.Count() > 0)
                            {
                                tenantManagement.ShardLogisticLastDate = ShardcontactActivityLogs.Max(s => s.LogDateTime);
                                tenantManagement.ShardLogisticTotalLastWeek = ShardcontactActivityLogs.Where(s => s.LogDateTime >= lastweek).Count();
                                tenantManagement.ShardLogisticTotalLastMonth = ShardcontactActivityLogs.Where(s => s.LogDateTime >= lastmonth).Count();
                            }

                        }

                        scope2.Complete();
                    }

                }
                catch(Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "tenant statistics worker" + ex.Message, null, null);

                }

                if (myCommunicationLogs.Count() > 0)
                {
                    if (myCommunicationLogs.Where(s => s.Subject == "FSR").Any())
                    {
                        tenantManagement.FSRLastSentDate = myCommunicationLogs.Where(s => s.Subject == "FSR").Max(s => s.CreateDate);
                    }
                    if (myCommunicationLogs.Where(s => s.Subject == "FSU").Any())
                    {
                        tenantManagement.FSULastReceivedDate = myCommunicationLogs.Where(s => s.Subject == "FSU").Max(s => s.CreateDate);
                    }
                    if (myCommunicationLogs.Where(s => s.Subject == "FSA").Any())
                    {
                        tenantManagement.FSALastReceivedDate = myCommunicationLogs.Where(s => s.Subject == "FSA").Max(s => s.CreateDate);
                    }
                }

                scope.Complete();
            }         
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TenantStatistics";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {

                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("TenantStatisticsdataqueue");


                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    queueDescription = new QueueDescription(emailQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    // queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);

                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "tenant statistics worker role start", null,null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
