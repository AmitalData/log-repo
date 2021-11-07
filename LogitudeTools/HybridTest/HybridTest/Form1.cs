using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using HypredTest.AgentProxy;
using HypredTest.BranchProxy;
using HypredTest.CustomerProxy;
using HypredTest.DepartmentProxy;
using HypredTest.PortProxy;
using HypredTest.ShipmentProxy;
using HypredTest.UserProxy;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Server.Infrastructure.Helpers;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Description;
using HypredTest.QuoteProxy;
using Simplog.Server.Infrastructure.DataContracts;
using HypredTest.DocumentInProxy;
using System.Reflection;
using HypredTest.CountryProxy;
using System.Diagnostics;
using System.Net.Http;
using Json2KeyValue;
using HypredTest.CurrencyProxy;
using Logitude.Server.Tools;
using HypredTest.VendorProxy;
using HypredTest.AirlineProxy;
using HypredTest.ShippingAgentProxy;
using HypredTest.ShippingLineProxy;
using HypredTest.AccountingPartnerProxy;
using HypredTest.PaymentTermProxy;
using HypredTest.HybridTenantStateProxy;

namespace HypredTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSendXml_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    ShipmentPM pm = new ShipmentPM()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        //CreateDateTime = DateTime.Now,
            //        CreatedByUserId = "1-2",
            //        SalesmanUserId = "1-31",
            //        DepartmentId = "1-4",
            //        Tenant = 1,
            //        ShipperId = "1-711",
            //        ShipperAddressId = "1-116",
            //        TransportModeId = "A",
            //        DirectionId = "I",
            //        FreightPrepaidCollectId = "P",
            //        OtherPrepaidCollectId = "P",
            //        CustomerId = "1-711",
            //        CustomerAddressId = "1-116",
            //        Notes = "created by Hybrid data",
            //        ShipmentCustomerTypeCode = "SHI",
            //        ToPortId = "1-3823",
            //        FromPort = "1-3911",
            //        MainCarriageFromPortId = "1-3911",
            //        MainCarriageToPortId = "1-3823",
            //        ShipmentLevelCode = "D",
            //        BranchId = "1-7",


            //    };


            //    XmlSerializer xsSubmit = new XmlSerializer(typeof(ShipmentPM));

            //    StringWriter sww = new StringWriter();
            //    XmlWriter writer = XmlWriter.Create(sww);
            //    xsSubmit.Serialize(writer, pm);
            //    var xml = sww.ToString();

            //    MemoryStream memory = new MemoryStream();
            //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(pm.GetType());
            //    x.Serialize(memory, pm);

            //    UploadFile(memory.ToArray(), pm.Id);

            //}

            //MessageBox.Show("Sending files Completed");
        }



        private void UploadFile(byte[] fileData, string id)
        {

            string filename = id;

            CloudBlobContainer blobContainer = GetCurrentContainer("Hybriddata");
            //temp file ( to be deleted when upload done)
            CloudBlockBlob tempcloudBlob = blobContainer.GetBlockBlobReference(filename);

            MemoryStream memorystream = new MemoryStream(fileData);

            tempcloudBlob.UploadFromStream(memorystream);

            SetQueue(id);
        }

        private void SetQueue(string id)
        {
            BrokeredMessage message = new BrokeredMessage();

            message.Properties["FileId"] = id;
            //message.Properties["Tenant"] = tenant;
            // message.TimeToLive = new TimeSpan(0, 15, 0);
            string HybridqueueName = "HybridDataQueue";
            HybridqueueName = HybridqueueName + Environment.MachineName;
            //if (WebFreightEntryPoint.DeploymentStage == "Dev")
            //{
            //    emailqueueName = emailqueueName + Environment.MachineName;
            //}
            //else
            //{
            //    emailqueueName = "Production" + emailqueueName;
            //}
            QueueClient client = CreateServiceBusQueueClient(HybridqueueName);

            client.Send(message);
        }

        public QueueClient CreateServiceBusQueueClient(string QueueName)
        {
            //var messagingFactory = MessagingFactory.Create(NameSpaceManager.Address,NameSpaceManager.Settings.TokenProvider);

            return Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(GetSettingByName("Microsoft.ServiceBus.ConnectionString"), QueueName);
        }


        public static string GetSettingByName(string name)
        {
            string result = "";

            switch (name)
            {
                case "Microsoft.ServiceBus.ConnectionString":
                    result = "Endpoint=sb://logitudetest1.servicebus.windows.net;SharedSecretIssuer=owner;SharedSecretValue=5iKNFIINnT+5u3Zj5SFkaRou/0QYxx7OWzZL/Wlh7us=";
                    break;
            }

            return result;
        }

        #region Azure Storage


        public CloudBlobContainer GetCurrentContainer(string containername)
        {


            CloudBlobContainer blobContainer = BlobClient.GetContainerReference(containername);
            blobContainer.CreateIfNotExists();

            return blobContainer;
        }

        private static CloudBlobClient blobClient;

        public static CloudBlobClient BlobClient
        {
            get
            {
                if (StorageAccount != null)
                {
                    blobClient = StorageAccount.CreateCloudBlobClient();
                }
                return blobClient;
            }
        }

        private static CloudQueueClient queueClient;
        public static CloudQueueClient QueueClient
        {
            get
            {
                if (StorageAccount != null)
                {
                    queueClient = StorageAccount.CreateCloudQueueClient();
                }
                return queueClient;
            }
        }



        public static CloudStorageAccount StorageAccount
        {
            get
            {


                return new CloudStorageAccount(null, false);
//                CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="),
//new Uri(@"http://127.0.0.1:10000/devstoreaccount1/"),
//new Uri(@"http://127.0.0.1:10001/devstoreaccount1/"),
//new Uri(@"http://127.0.0.1:10002/devstoreaccount1/"));

//                return storageAccount;
            }

        }

        #endregion



        public static string Token { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {

            Login();
            TestShipmentService();
            //this.GetExternalTasksFromFastQueue();
            //IncotermProxy.IncotermWcfServiceClient incotermservice = new IncotermProxy.IncotermWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)incotermservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    var response = new IncotermProxy.Response();
            //    var result = incotermservice.GetIncoterms(ref response);
            //}

            TestCustomerService(Token);
            InsertCommTaskProxy.InsertCommTaskWcfServiceClient taskService = new InsertCommTaskProxy.InsertCommTaskWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)taskService.InnerChannel))
            {
                //taskService.Endpoint.Behaviors.Add(new MessageInspectionBehavior());

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                //QueueTask[] mytasks = {
                //                                              new InsertCommTaskProxy.QueueTask()
                //                                             {
                //                                                  Action = "Test",
                //                                                  Parameters = new InsertCommTaskProxy.Parameter[]{
                //                                                      new InsertCommTaskProxy.Parameter(){  Name = "Param1", Order =1, Value = "<Param1></Param1>" }
                //                                                  },
                //                                              }
                //                                          };
                //var result = taskService.InsertTask(1, 1, 1, "testing the comm task", mytasks);


                //var headers = System.ServiceModel.Web.WebOperationContext.Current.IncomingResponse.Headers["SentQueueMessages"];


            }



            //HybridPartnerProxy.HybridPartnerWcfServiceClient hybridPartnerservice = new HybridPartnerProxy.HybridPartnerWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)hybridPartnerservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    var response = new HybridPartnerProxy.Response();
            //    var result = hybridPartnerservice.GetMislakaPartners(1, ref response);
            //}

            //
            //TestEntityStatusService();

            // LoginToCloud();

            //UserProxy.UserWcfServiceClient userservice = new UserProxy.UserWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)userservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    UserProxy.Response response22 = new UserProxy.Response();
            //    var result = userservice.GetUser(new UserApiFilters() { SearchCode = "SARA", ByCode = true }, 3, ref response22);
            //}


            //CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            //{

            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    CustomerProxy.Response resultResponse = new CustomerProxy.Response();
            //    CustomerPM pm = customerservice.GetCustomerPM(new CustomerApiFilters() { ByVatNumber = true, SearchCode = "309534519" }, 3, ref resultResponse);

            //    //var rres = customerservice.Upsert(customer111, false);
            //    //  var customer = customerservice.GetCustomerPM(new CustomerApiFilters() { ByVatNumber = true, SearchCode = "1234562322222" }, 1);
            //    //  var customer2 = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "470690c4-8f9a-4-121212" }, 1);
            //    // var contacts = customerservice.GetCustomerContacts(new CustomerApiFilters() { ByCode = true, SearchCode = "70002-1212" }, 1);
            //    //  var addresses = customerservice.GetCustomerAddresses(new CustomerApiFilters() { ByCode = true, SearchCode = "70002-1212" }, 1);

            //}
            
            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                string shipmentnumber = "myship3254";//"CUSTOMF1";//"MYHYBRIDSHIP3";//Guid.NewGuid().ToString().Substring(0, 15);
                ShipmentProxy.ShipmentPM pm = new ShipmentPM()
                {

                    DirectionId = "C",
                    ShipmentLevelCode = "A",
                    ShipmentTypeId = null,
                    CustomerId = "61522eec-fa18-4",
                    Tenant = 1,
                    AccessDate = DateTime.Now,
                    AWBCurrencyId = "USD",
                    BranchId = "HybridB1",
                    DepartmentId = "HybridD1",
                    ChargeableWeightUnitCode = "KG",
                    ConsigneeId = "70000",
                    ShipperId = "70000",
                    ConsigneeReference1 = "PO35104",
                    CreateDateTime = DateTime.Now.AddDays(-12),
                    CreatedByUserId = "HybridU1",
                    CutoffDate = DateTime.Now,
                    DimensionsUnitCode = "CM",

                    FinalDistenationPortId = "TLV",
                    FreightPrepaidCollectId = "C",
                    FromPortId = "JFK",
                    GrossWeightUnitCode = "KG",
                    House = "4545",
                    IncotermId = "CIF",
                    MainCarriageCarrierId = "LY",
                    MainCarriageFinalDestinationPortId = "TLV",
                    MainCarriageFromPortId = "JFK",
                    MainCarriageToPortId = "TLV",
                    OtherPrepaidCollectId = "C",

                    ShipmentCustomerTypeCode = "SHI",

                    ShipmentNumber = shipmentnumber,

                    StatusId = "SHOR",
                    ToPortId = "TLV",
                    TransportModeId = "A",
                    VolumeUnitCode = "CBM",
                    ChargeableWeight = 0.9999984133,
                    GrossWeight = 0.9999984133,
                    Master = "45454",
                    MainCarriageATA = DateTime.Now,
                    MainCarriageETA = DateTime.Now,
                    AccountedPayablesInLocalCurrency = 100,
                    AccountedPayablesInProfitCurrency = 200,
                    AccountedReceivablesInLocalCurrency = 300,
                    AccountedReceivablesInProfitCurrency = 400,

                    OpenPayablesInLocalCurrency = 900,
                    OpenPayablesInProfitCurrency = 450,
                    OpenReceivablesInProfitCurrency = 300,
                    OpenReceivablesInLocalCurrency = 500,
                    ProfitCurrencyId = "NIS",
                    PaymentDateTime = DateTime.Now,
                    PaymentRequestXML = @"<Payment>
< CustomerName > Islam </ CustomerName >
< AmountInNIS > 2000.2 </ AmountInNIS >
</ Payment >",
                    //CustomFileNumber = "CUSTOMF1",

                };
                //CUSTOMF1

                //pm.ShipmentPackages = new ShipmentPackagePM[]
                //{
                // new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 1",Quantity=  10,ContainerNumber = "cont1111", Weight = 12,Volume = 1.563}, //InsideShipmentPackages = new InsideShipmentPackagePM[]{ new InsideShipmentPackagePM(){ PackageTypeId ="45GP", Quantity = 25,Tenant = 1 },}, },
                // new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 3",Quantity = 11,ContainerNumber = "cont2222",Weight=  44,Volume = 1.982},
                // new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 4",Quantity = 12,ContainerNumber = "cont2222",Weight=  55,Volume = 1.45452},

                //};

                Response response = shipmentservice.Upsert(pm, false);
                //var tenants = loginService.GetUserTenants("islam@fnarsoft.com", ref res2);
            }


            ActivityProxy.ActivityWcfServiceClient activityService = new ActivityProxy.ActivityWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)activityService.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                //ActivityProxy.ActivityPM activity = new ActivityProxy.ActivityPM()
                //{
                //    Tenant = 1,
                //    ActivityStatusCode = "I",
                //    ActivityTypeCode = "AP",
                //    Subject = "Bank of Palestine",
                //    DueDate = DateTime.Now.AddDays(2),
                //    OutlookId = "7337a1cc-fa98-420c-a581-1b387c0ttr",//Guid.NewGuid().ToString(),
                //    StartDateTime = DateTime.Now.AddDays(2),
                //    EndDateTime = DateTime.Now.AddDays(3),
                //    CompleteDate = DateTime.Now.AddDays(3),
                //    CreateDate = DateTime.Now,
                //    UpdateDate = DateTime.Now,
                //    BusinessUnitId = "1",
                //    Description = "yyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiyyyyiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiisdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsdddddddddddddddddddddddsdsdsddddddddddddddddddddddd"
                //};

                //activity.ActivityInvitees = new ActivityProxy.ActivityInviteePM[] { new ActivityProxy.ActivityInviteePM() { Email = "mohammad@fnarsoft.com", Tenant = 1 }, new ActivityProxy.ActivityInviteePM() { Email = "abbas@fnarsoft.com", IsRequired = true, Tenant = 1 }, new ActivityProxy.ActivityInviteePM() { Email = "morsi@fnarsoft.com", IsRequired = true, Tenant = 1, } };
                //activity.ActivityEmailRecipients = new ActivityProxy.ActivityEmailRecipientPM[] { new ActivityProxy.ActivityEmailRecipientPM() { Email = "mohammad@fnarsoft.com", Tenant = 1, RecipientTypeCode = "TO" }, new ActivityProxy.ActivityEmailRecipientPM() { Email = "islam@fnarsoft.com", Tenant = 1, RecipientTypeCode = "BCC", }, new ActivityProxy.ActivityEmailRecipientPM() { Email = "mohammad@fnarsoft.com", Tenant = 1, RecipientTypeCode = "CC" } };
                //ActivityProxy.Response actresponse = activityService.Upsert(activity, "islam@fnarsoft.com");
                Response respRef = new Response();
                var activit = activityService.GetActivities("eladan@amital.co.il", 1, ref respRef);
                var act = activityService.GetActivityPM("1-1473", 1, ref respRef);

            }







            //OpportunityProxy.OpportunityWcfServiceClient opportunityService = new OpportunityProxy.OpportunityWcfServiceClient();
            //OpportunityProxy.OpportunityApiFilters filters = new OpportunityProxy.OpportunityApiFilters()
            //{
            //    IsOpen = false,
            //    MyOpportunities = true,
            //};
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)opportunityService.InnerChannel))
            //{

            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    OpportunityProxy.Response opp_Response = new OpportunityProxy.Response();
            //    var result = opportunityService.GetOpportunityList("islam@fnarsoft.com", null, 1, 0, 30, filters, ref opp_Response);

            //}
            //ActivityProxy.ActivityWcfServiceClient activityService = new ActivityProxy.ActivityWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)activityService.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                ActivityProxy.ActivityPM activity = new ActivityProxy.ActivityPM()
                {
                    Tenant = 1,
                    ActivityStatusCode = "I",
                    ActivityTypeCode = "AP",
                    Subject = "Bank of Palestine",
                    DueDate = DateTime.Now.AddDays(2),
                    OutlookId = "7337a1cc-fa98-420c-a581-1b387c0522r",//Guid.NewGuid().ToString(),
                    StartDateTime = DateTime.Now.AddDays(2),
                    EndDateTime = DateTime.Now.AddDays(3),
                    CompleteDate = DateTime.Now.AddDays(3),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    BusinessUnitId = "1",
                };

                activity.ActivityInvitees = new ActivityProxy.ActivityInviteePM[] { new ActivityProxy.ActivityInviteePM() { Email = "mohammad@fnarsoft.com", Tenant = 1 }, new ActivityProxy.ActivityInviteePM() { Email = "abbas@fnarsoft.com", IsRequired = true, Tenant = 1 }, new ActivityProxy.ActivityInviteePM() { Email = "morsi@fnarsoft.com", IsRequired = true, Tenant = 1, } };

                //ActivityProxy.Response actresponse = activityService.Upsert(activity, "islam@fnarsoft.com");
                //ActivityProxy.Response respRef = new ActivityProxy.Response();
                //var activit = activityService.GetActivities("islam@fnarsoft.com", 1, ref respRef);

            }




            //LoginProxy.Response res2 = new LoginProxy.Response();


            //ServiceHost host = new ServiceHost(typeof(CustomerWcfServiceClient));

            //ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

            //// if not found - add behavior with setting turned on 
            //if (debug == null)
            //{
            //    host.Description.Behaviors.Add(
            //         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            //}
            //else
            //{
            //    // make sure setting is turned ON
            //    if (!debug.IncludeExceptionDetailInFaults)
            //    {
            //        debug.IncludeExceptionDetailInFaults = true;
            //    }
            //}

            //host.Open();


            //AddressProxy.AddressWcfServiceClient addressService = new AddressProxy.AddressWcfServiceClient();
            //AddressProxy.AddressPM address = new AddressProxy.AddressPM()
            //{
            //    ExternalId = "E00001",
            //    Tenant = 1,
            //    Address1 = "Al Beireh",
            //    Address2 = "Jawwal",
            //    AddressTypeId = "M", // M: main address, B: billing // O:Other
            //    ATTN = "11111",
            //    CardId = "70002",
            //    City = "Ramallah",
            //    CountryId = "PS",
            //    Name = "Main Address",
            //    ZipCode = "0972",
            //    Description = "ramallah address",
            //};
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)addressService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", "aa");
            //    AddressProxy.Response response = addressService.Upsert(address, false);
            //}




            //List<ActivityProxy.ActivityPM> notSyncActivities = activityService.GetActivities("islam@fnarsoft.com", 1).ToList();
            //foreach (ActivityProxy.ActivityPM acpm in notSyncActivities)
            //{
            //    //if (!string.IsNullOrEmpty(pm.OutlookId))
            //    //{
            //    ActivityProxy.Response res = activityService.SetAsSynchronized(acpm.Id, "islam@fnarsoft.com", acpm.Tenant);
            //    //}
            //    //else
            //    //{
            //    //    ActivityProxy.Response response3 = activityService.UpdateOutlookID(pm.Id, Guid.NewGuid().ToString(), pm.Tenant);
            //    //}
            //}




            //OpportunityProxy.OpportunityWcfServiceClient opportunityService = new OpportunityProxy.OpportunityWcfServiceClient();
            //OpportunityProxy.OpportunityApiFilters filters = new OpportunityProxy.OpportunityApiFilters()
            //{
            //     IsOpen = true,
            //};

            //var result = opportunityService.GetOpportunityList("islam@fnarsoft.com", null, 1, 0, 10, filters);


            //ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            //ShipmentProxy.ShipmentPM pm = new ShipmentPM()
            //{
            //    Tenant = 1,
            //    AccessDate = DateTime.Now,
            //    AWBCurrencyId = "USD",
            //    BranchId = "HybridB1",
            //    DepartmentId = "HybridD1",
            //    ChargeableWeightUnitCode = "KG",
            //    ConsigneeId = "70000",
            //    ShipperId = "70000",
            //    ConsigneeReference1 = "PO35104",
            //    CreateDateTime = DateTime.Now,
            //    CreatedByUserId = "HybridU1",
            //    CutoffDate = DateTime.Now,
            //    DimensionsUnitCode = "CM",
            //    DirectionId = "I",
            //    FinalDistenationPortId = "TLV",
            //    FreightPrepaidCollectId = "C",
            //    FromPortId = "JFK",
            //    GrossWeightUnitCode = "KG",
            //    House = "4545",
            //    IncotermId = "CIF",
            //    MainCarriageCarrierId = "LY",
            //    MainCarriageFinalDestinationPortId = "TLV",
            //    MainCarriageFromPortId = "JFK",
            //    MainCarriageToPortId = "TLV",
            //    OtherPrepaidCollectId = "C",
            //    ProfitCurrencyId = "NIS",
            //    ShipmentCustomerTypeCode = "SHI",
            //    ShipmentLevelCode = "H",
            //    ShipmentNumber = "Shp75459",
            //    ShipmentTypeId = null,//"LCL",
            //    StatusId = "SHOR",
            //    ToPortId = "ILTLV",
            //    TransportModeId = "A",
            //    VolumeUnitCode = "CBM",
            //    ChargeableWeight = 0.9999984133,
            //    GrossWeight = 0.9999984133,
            //    Master = "45454",
            //    MainCarriageATA = DateTime.Now,
            //    MainCarriageETA = DateTime.Now,
            //    //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            //    // ShipperReference2 
            //    // IsCancelled = true,
            //    IsCancelled = false,
            //    Notes = "ola",
            //};

            //ShipmentProxy.Response rspo = shipmentservice.Upsert(pm, false);
            //// ShipmentProxy.Response eventResponse = shipmentservice.CreateEvent(1, "222", "5de84ff0-bc5e-4", "HybridSystemUser", "ARR", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), "Testing hybrid with id");

            // List<TraceEventPM> events = new List<TraceEventPM>() 
            // { 
            //     new TraceEventPM() { Tenant = 1, ExternalId = "55555", UserId = "HybridSystemUser", EventTypeCode = "ARR12", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now.AddDays(-1), Notes = "Testing hybrid list1" },
            //     new TraceEventPM() { Tenant = 1, ExternalId = "4055550", UserId = "HybridSystemUser", EventTypeCode = "ARR12", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now.AddDays(-1), Notes = "Testing hybrid list2" },
            //     //new TraceEventPM() { Tenant = 1, ExternalId = "10", UserId = "HybridSystemUser", EventTypeCode = "PICD", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now.AddDays(-1), Notes = "Testing hybrid list3" }
            // };

            // ShipmentProxy.Response eventResponse2 = shipmentservice.BuildEventsList(1, "84f7208d-2a6a-4", events.ToArray());


           // UserProxy.UserWcfServiceClient userservice = new UserProxy.UserWcfServiceClient();

            //UserPM user = new UserPM()
            //{
            //    Id = "ISLM",
            //    Code = "ISLM",
            //    BranchId = "HybridB1",
            //    DepartmentId = "HybridD1",
            //    EnglishName = "Yaron Cohen",
            //    LocalName = "المستخدم1",
            //    Tenant = 1,
            //    Email = "TEST@islam.COM",
            //    Password = "123",
            //    Notes = "new",
            //    UserType = "R",
            //    InActive = true,

            //};
            //UserProxy.Response rr = userservice.Upsert(user, false);




            //ShipmentProxy.Response eventResponse2 = shipmentservice.BuildEventsList(1, "84f7208d-2a6a-4", "HybridSystemUser", events.ToArray());
            //int n = 0;
            //EventTypeProxy.EventTypeWcfServiceClient EventTypeService = new EventTypeProxy.EventTypeWcfServiceClient();
            //EventTypeProxy.EventTypePM EventTypePM = new EventTypeProxy.EventTypePM()
            //{
            //    Code = "TEST",
            //    Tenant = 1,
            //    ObjectTableName = "Shipment",
            //    EntityStatusId = "TEST",
            //    EnglishName = "Testing event type",
            //    ObjectTableId = "Shipment",

            //};

            //EventTypeProxy.Response tyoeResp = EventTypeService.Upsert(EventTypePM, false);


            //EntityStatusProxy.EntityStatusWcfServiceClient entityStatusService = new EntityStatusProxy.EntityStatusWcfServiceClient();
            //EntityStatusProxy.EntityStatusPM entityStatusPM = new EntityStatusProxy.EntityStatusPM()
            //{
            //    Code = "TEST",
            //    Name = "test hybrid status",
            //    Tenant = 1,
            //    ObjectTableName = "Shipment",


            //};

            //EntityStatusProxy.Response statusResp = entityStatusService.Upsert(entityStatusPM, false);




            //   ShippingAgentProxy.ShippingAgentWcfServiceClient shippingagentservice = new ShippingAgentProxy.ShippingAgentWcfServiceClient();

            //   ShippingAgentProxy.ShippingAgentPM shippingagent = new ShippingAgentProxy.ShippingAgentPM()
            //   {
            //       Code = "HybridSAG1",
            //       EnglishName = "Hybrid shipping agent",
            //       PartnerTypeId = "SG",
            //       CreateDate = DateTime.Now,
            //       LocalName = "عميل الشحن",
            //       Notes = "new",

            //   };

            //   ShippingAgentProxy.Response shipagentresp = shippingagentservice.Upsert(shippingagent, false);



            //   TruckerProxy.TruckerWcfServiceClient truckerService = new TruckerProxy.TruckerWcfServiceClient();
            //   TruckerProxy.TruckerPM trucker = new TruckerProxy.TruckerPM()
            //   {
            //       Tenant = 1,
            //       Code = "TRU",
            //       EnglishName = "Turkey airlines",


            //   };

            //   TruckerProxy.Response truckerResponse = truckerService.Upsert(trucker, false);


            ////   CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerWcfServiceClient();
            ////var  ress =  customerservice.GetCustomerList("", 1, 0, 1);

            //ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();

            //ShipmentProxy.Response deleteResponse1 = shipmentservice.Delete("SHIP_7092", 1); // Deleting Direct Shipment
            //ShipmentProxy.Response deleteResponse2 = shipmentservice.Delete("SHIP_7093", 1); // Deleting House Shipment
            //ShipmentProxy.Response deleteResponse3 = shipmentservice.Delete("E10012", 1); // Deleting Console Shipment
            //Console.Write("finished");
            //ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();


            int m = 0;
            //pm.ShipmentPackages = new ShipmentPackagePM[]
            //{
            //     new ShipmentPackagePM()
            //     {
            //         ChangeOp = Simplog.Server.Infrastructure.ChangeSetOperation.None,
            //         ContainerNumber = "MSCU7897890",

            //         //PackageTypeId = "45GP", Tenant=1, Description="test package 1",Quantity=  10,ContainerNumber = "cont1111", Weight = 12,
            //         //InsideShipmentPackages = new InsideShipmentPackagePM[]{ new InsideShipmentPackagePM(){ PackageTypeId ="45GP", Quantity = 25,Tenant = 1 },},
            //     },

            //     //new ShipmentPackagePM(){ PackageTypeId = "45GP", Tenant=1, Description="test package 2",Quantity = 11,ContainerNumber = "cont2222",Weight=  44},

            //};

            //




            //UserProxy.UserWcfServiceClient userservice = new UserProxy.UserWcfServiceClient();

            //  UserPM user = new UserPM()
            //  {
            //      Id = "YARONC77",
            //      Code = "YARONC77",
            //      BranchId = "HybridB1",
            //      DepartmentId = "HybridD1",
            //      EnglishName = "Yaron Cohen",
            //      LocalName = "المستخدم1",
            //      Tenant = 1,
            //      Email = "TEST@GMAIL77.COM",
            //      Password = "123",
            //      Notes = "new",
            //      UserType = "R",
            //      InActive = true,

            //  };
            //  UserProxy.Response rr = userservice.Upsert(user, false);


            //PortProxy.PortWcfServiceClient portservice = new PortProxy.PortWcfServiceClient();
            //    PortPM port = new PortPM()
            //    {
            //        AddedManually = false,

            //        Code = "HFA",
            //        CountryId = "IL",
            //        EnglishName = "HAIFA",
            //        IsHybrid = true,
            //        IsInland = false,
            //        IsOcean = true,
            //        LocalName = "HAIFA",
            //        StateId = "TN",
            //        Tenant = 1,
            //    };
            //    PortProxy.Response re = portservice.Upsert(port, false);



            //    BranchProxy.BranchWcfServiceClient branchservice = new BranchProxy.BranchWcfServiceClient();

            //BranchProxy.Response res =    branchservice.Upsert(new BranchPM()
            //    {
            //        Code = "HybridB1",
            //        EnglishName = "Hybrid Branch",
            //        LocalName = "فرع",
            //        Tenant = 1,
            //        Notes = "new",
            //    }, false);



            //    CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerProxy.CustomerWcfServiceClient();

            //   CustomerList[] list = customerservice.GetCustomersByContact("islam@logitudeworld.com", 1);
            //    //ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();





            //    //ShipmentProxy.Response resp = shipmentservice.Delete("84f7208d-2a6a-4", 1);







            //    ActivityProxy.ActivityWcfServiceClient activityService = new ActivityProxy.ActivityWcfServiceClient();

            //    //ActivityProxy.ActivityPM activity = new ActivityProxy.ActivityPM()
            //    //{
            //    //    Tenant = 1,
            //    //    ActivityStatusCode = "O",
            //    //    ActivityTypeCode = "AP",
            //    //    Subject = "Bank of Palestine",
            //    //    DueDate = DateTime.Now.AddDays(2),
            //    //    OutlookId = Guid.NewGuid().ToString(),
            //    //};

            //    //ActivityProxy.Response response = activityService.Upsert(activity, "islam@fnarsoft.com");



            //    ActivityProxy.ActivityPM activity = new ActivityProxy.ActivityPM()
            //    {
            //        Tenant = 1,
            //       // ActivityStatusCode = "O",
            //     //   ActivityTypeCode = "AP",
            //      //  Subject = "test activity",
            //      //  DueDate = DateTime.Now.AddDays(7),
            //       // OutlookId = Guid.NewGuid().ToString(),
            //    };

            //   ActivityProxy.Response response2 = activityService.Upsert(activity,"jalal@mail.com");


            //   //List<ActivityProxy.ActivityPM> notSyncActivities = activityService.GetActivities("jalal@mail.com", 1).ToList();
            //   //foreach (ActivityProxy.ActivityPM pm in notSyncActivities)
            //   //{
            //   //    if (!string.IsNullOrEmpty(pm.OutlookId))
            //   //    {
            //   //        activityService.SetAsSynchronized(pm.OutlookId, pm.Tenant);
            //   //    }
            //   //    else
            //   //    {
            //   //        ActivityProxy.Response response3 = activityService.UpdateOutlookID(pm.Id, Guid.NewGuid().ToString(), pm.Tenant);
            //   //    }
            //   //}




        }


        public void TestWarehouseService(string token)
        {

            //WarehouseProxy.WarehouseWcfServiceClient warehouseService = new WarehouseProxy.WarehouseWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)warehouseService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

            //    WarehouseProxy.Response resultResponse = new WarehouseProxy.Response();
            //    // CustomerPM pm = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "10107933" }, 8, ref resultResponse);
            //    WarehouseProxy.WarehousePM newCustomer = new WarehouseProxy.WarehousePM()
            //    {
            //        Code = "HBWR",
            //        EnglishName = "Hybrid warehouse",
            //        LocalName = "Hybrid warehouse",
            //        Tenant = 1,

            //    };



            //    var response = warehouseService.Upsert(newCustomer, false);
            //    if (response.HasError)
            //        MessageBox.Show("Failed: " + response.ErrorMessage);
            //    else
            //        MessageBox.Show("Success: " + response.Result);

            //}

          
         
        }
        private void TestEntityStatusService()
        {

            //EntityStatusProxy.EntityStatusWcfServiceClient entityStatusService = new EntityStatusProxy.EntityStatusWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)entityStatusService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    EntityStatusProxy.EntityStatusPM entityStatusPM = new EntityStatusProxy.EntityStatusPM()
            //    {
            //        Code = "SARR",
            //        Name = "test hybrid status",
            //        Tenant = 0,
            //        ObjectTableName = "Customer",
            //        ObjectTableId = "Customer",
                     
            //    };

            //    EntityStatusProxy.Response statusResp = entityStatusService.Upsert(entityStatusPM, false);
            //}

        }

        private void btnThreads_Click(object sender, EventArgs e)
        {
            //LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
            //LoginProxy.Response loginResponse = loginService.Login("islam@fnarsoft.com", "0");
            //if (!loginResponse.HasError)
            //{
            //    Token = loginResponse.Result;
            //}

            //int threads = 5;
            //for (int i = 0; i < threads; i++)
            //{
            //    Thread workerThread = new Thread(UpsertShipments);
            //    workerThread.Name = "Test" + i;
            //    // Start the worker thread.
            //    workerThread.Start("");//"Shipment_H_O" + i//Guid.NewGuid().ToString().Substring(0,15));

            //}

        }

        private static void UpsertUsers(object param)
        {
            //UserProxy.UserWcfServiceClient userservice = new UserWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)userservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    UserProxy.UserPM userpm = new UserPM()
            //    {
            //        Email = "thread2@mail.com",
            //        Code = "thread2",
            //        EnglishName = "thread test",
            //        Tenant = 1,
            //        BranchId = "HybridB1",
            //        DepartmentId = "HybridD1",
            //        Password = "123",
            //        UserType = "R",
            //        InActive = true,
            //        BusinessUnitId = "1"
            //    };
            //     UserProxy.Response response =  userservice.Upsert(userpm,false);
            //     if (response.HasError)
            //     {

            //     }
            //}

             //Id = "ISLM",
            //    Code = "ISLM",
            //    BranchId = "HybridB1",
            //    DepartmentId = "HybridD1",
            //    EnglishName = "Yaron Cohen",
            //    LocalName = "المستخدم1",
            //    Tenant = 1,
            //    Email = "TEST@islam.COM",
            //    Password = "123",
            //    Notes = "new",
            //    UserType = "R",
            //    InActive = true,
        }


        public static ProcessThread GetProcessThreadFromWin32ThreadId(Int32 threadId)
        {
            //if (threadId == 0) threadId = ThreadUtility.GetCurrentWin32ThreadId();
            foreach (Process process in Process.GetProcesses())
            {
                foreach (ProcessThread processThread in process.Threads)
                {
                    if (processThread.Id == threadId) return processThread;
                }
            }
            throw new InvalidOperationException("No thread matching specified thread Id was found.");
        }

        private static void UpsertShipments(object param)
        {



            //int unmanagedId = 2345;
            Process p = Process.GetCurrentProcess();
            ProcessThreadCollection coll = Process.GetCurrentProcess().Threads;
            var m = ThreadCpuCalculator.GetProcessThreadFromWin32ThreadId(Thread.CurrentThread.ManagedThreadId);

            //ProcessThread pthread = (ProcessThread)Thread.CurrentThread;
            ProcessThread myThread = (from ProcessThread entry in Process.GetCurrentProcess().Threads
                                      where entry.Id == Thread.CurrentThread.ManagedThreadId
                                      select entry).FirstOrDefault();

            if (myThread != null)
            {
            }

            string shipmentnumber = param as string;
            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            //shipmentservice.Endpoint.Binding.SendTimeout = new TimeSpan(0, 10, 0);
            //shipmentservice.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 10, 0);
            //shipmentservice.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            //shipmentservice.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 10, 0);

            for (int i = 0; i < 50; i++)
            {
                shipmentnumber = Guid.NewGuid().ToString().Substring(0, 15);//"df35cfcb-cbbc-4";//
                ShipmentProxy.ShipmentPM pm = new ShipmentPM()
                {
                    Tenant = 1,
                    AccessDate = DateTime.Now,
                    AWBCurrencyId = "USD",
                    BranchId = "HybridB1",
                    DepartmentId = "HybridD1",
                    ChargeableWeightUnitCode = "KG",
                    ConsigneeId = "70000",
                    ShipperId = "70000",
                    ConsigneeReference1 = "PO35104",
                    CreateDateTime = DateTime.Now,
                    CreatedByUserId = "HybridU1",
                    CutoffDate = DateTime.Now,
                    DimensionsUnitCode = "CM",
                    DirectionId = "I",
                    FinalDistenationPortId = "TLV",
                    FreightPrepaidCollectId = "C",
                    FromPortId = "JFK",
                    GrossWeightUnitCode = "KG",
                    House = "4545",
                    IncotermId = "CIF",
                    MainCarriageCarrierId = "LY",
                    MainCarriageFinalDestinationPortId = "TLV",
                    MainCarriageFromPortId = "JFK",
                    MainCarriageToPortId = "TLV",
                    OtherPrepaidCollectId = "C",
                    ProfitCurrencyId = "NIS",
                    ShipmentCustomerTypeCode = "SHI",
                    ShipmentLevelCode = "H",
                    ShipmentNumber = shipmentnumber,
                    ShipmentTypeId = null,
                    StatusId = "SHOR",
                    ToPortId = "TLV",
                    TransportModeId = "A",
                    VolumeUnitCode = "CBM",
                    ChargeableWeight = 0.9999984133,
                    GrossWeight = 0.9999984133,
                    Master = "45454",
                    MainCarriageATA = DateTime.Now,
                    MainCarriageETA = DateTime.Now,
                    Notes = "Update" + (i + 1),

                };

                pm.ShipmentPackages = new ShipmentPackagePM[]
                {
                 new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 1",Quantity=  10,ContainerNumber = "cont1111", Weight = 12}, //InsideShipmentPackages = new InsideShipmentPackagePM[]{ new InsideShipmentPackagePM(){ PackageTypeId ="45GP", Quantity = 25,Tenant = 1 },}, },
                 new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 3",Quantity = 11,ContainerNumber = "cont2222",Weight=  44},
                 new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 4",Quantity = 12,ContainerNumber = "cont2222",Weight=  55},
                 new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 5",Quantity = 13,ContainerNumber = "cont2222",Weight=  66},
                 new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 6",Quantity = 14,ContainerNumber = "cont2222",Weight=  77},
                // //new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 7",Quantity = 15,ContainerNumber = "cont2222",Weight=  88},
                // //new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 8",Quantity = 16,ContainerNumber = "cont2222",Weight=  99},
                // //new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 8",Quantity = 16,ContainerNumber = "cont2222",Weight=  99},
                // //new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 8",Quantity = 16,ContainerNumber = "cont2222",Weight=  99},
                // //new ShipmentPackagePM(){ PackageTypeCode = "45GP", Tenant=1, Description="test package 8",Quantity = 16,ContainerNumber = "cont2222",Weight=  99},
                
                };
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
                {

                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    Response response = shipmentservice.Upsert(pm, false);
                    if (response.HasError)
                    {
                        Console.WriteLine("Error: Shipment no. " + shipmentnumber + " Update" + i + Environment.NewLine + response.ErrorMessage + Environment.NewLine + response.InnerErrorMessage);
                    }
                    else
                    {
                        Console.WriteLine("Shipment Id. " + response.Result + " Update"  );
                        Console.WriteLine("Thread Name: " + System.Threading.Thread.CurrentThread.Name + " ,Id: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                    }
                }
            }

        }



        private static void UpsertCustomers(object param)
        {

            string customerCode = param as string;
            CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerProxy.CustomerWcfServiceClient();

            for (int i = 0; i < 50; i++)
            {
                customerCode = Guid.NewGuid().ToString().Substring(0, 15);
                CustomerProxy.CustomerPM customer = new CustomerProxy.CustomerPM()
                {
                    Code = customerCode,
                    EnglishName = "Hybrid customer" + i,
                    PartnerTypeId = "CS",
                    CreateDate = DateTime.Now,
                    Notes = "new",
                    Tenant = 1,

                };

                Response response = customerservice.Upsert(customer, false);
                if (response.HasError)
                {
                    Console.WriteLine("Error: Shipment no. " + customerCode + " Update" + i + Environment.NewLine + response.ErrorMessage + Environment.NewLine + response.InnerErrorMessage);
                }
            }

        }

        private void btnUploadQuoteDocument_Click(object sender, EventArgs e)
        {

			string m = "hello word";
			var s = m.Split("word".ToArray());

			OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            // openFileDialog.Filter = "mrt|*.mrt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                using (Stream filestream = openFileDialog.OpenFile())
                {
                    // Create a byte array of file stream length
                    byte[] fileData = new byte[filestream.Length];
                    filestream.Position = 0;
                    //XmlSerializer xsSubmit33 = new XmlSerializer(typeof(ShipmentPM));
                    //ShipmentPM pm11 = xsSubmit33.Deserialize(filestream) as ShipmentPM;


                    string fileExtension = openFileDialog.SafeFileName.Split('.')[1];
                    //long fileBytes = openFileDialog.File.Length;


                    //Read block of bytes from stream into the byte array
                    filestream.Read(fileData, 0, System.Convert.ToInt32(filestream.Length));
                    filestream.Position = 0;
                    //LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
                    //LoginProxy.Response loginResponse = loginService.Login("customercare@logitudeworld.com", "!C123456");
                    //if (!loginResponse.HasError)
                    //{
                    //    Token = loginResponse.Result;
                    //}

                    // LoginToCustoms();

                    // Login("yaronc@amital.co.il", "!Y123456");
                    //LoginByCredential("3969c070-65c7-4f4a-bc69-785d0b4fc300", 1071);
                    Token = "z6VGJluczfJoScdV1F+/01ZSxShUxCjS2Fc=";
                    ShipmentProxy.ShipmentWcfServiceClient shipmentService = new ShipmentWcfServiceClient();

                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentService.InnerChannel))
                    {
                        ShipmentProxy.ShipmentPM pm = ObjectXmlSerializer.DeserializeObject<ShipmentProxy.ShipmentPM>(filestream);
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                        //pm.Field1 = new CustomFieldClass("Field1", "Shipment", null);
                        //pm.Field2 = new CustomFieldClass("Field2", "Shipment", null);
                        //pm.Field3 = new CustomFieldClass("Field3", "Shipment", null);
                        //pm.Field4 = new CustomFieldClass("Field4", "Shipment", null);
                        //pm.Field5 = new CustomFieldClass("Field5", "Shipment", null);
                        //pm.Field6 = new CustomFieldClass("Field6", "Shipment", null);
                        //pm.Field7 = new CustomFieldClass("Field7", "Shipment", null);
                        //pm.Field8 = new CustomFieldClass("Field8", "Shipment", null);
                        //pm.Field9 = new CustomFieldClass("Field9", "Shipment", null);
                        //pm.Field10 = new CustomFieldClass("Field10","Shipment", null);
                        //pm.Field11 = new CustomFieldClass("Field11", "Shipment", null);
                        //pm.Field12 = new CustomFieldClass("Field12", "Shipment", null);
                        //pm.Field13 = new CustomFieldClass("Field13", "Shipment", null);
                        //pm.Field14 = new CustomFieldClass("Field14", "Shipment", null);
                        //pm.Field15 = new CustomFieldClass("Field15", "Shipment", null);
                        //pm.Field16 = new CustomFieldClass("Field16", "Shipment", null);
                        //pm.Field17 = new CustomFieldClass("Field17", "Shipment", null);
                        //pm.Field18 = new CustomFieldClass("Field18", "Shipment", null);
                        //pm.Field19 = new CustomFieldClass("Field19", "Shipment", null);
                       // pm.Field20 = new CustomFieldClass("Field20", "Shipment", null);

                        Response myresp = shipmentService.Upsert(pm, false);

                    }


                    XmlSerializer xsSubmit = new XmlSerializer(typeof(HypredTest.DocumentInProxy.DocumentsFilingPM));
                    HypredTest.DocumentInProxy.DocumentsFilingPM documentFiling = xsSubmit.Deserialize(filestream) as HypredTest.DocumentInProxy.DocumentsFilingPM;
                    DocumentInProxy.DocumentInWcfServiceClient documentsInService = new DocumentInProxy.DocumentInWcfServiceClient();
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documentsInService.InnerChannel))
                    {
                        documentFiling.Id = documentFiling.Id.Trim();
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                        var result = documentsInService.Upsert(documentFiling, false);

                    }

                   

                     
                    //Login();

                   

                    //XmlSerializer xsSubmit = new XmlSerializer(typeof(CustomerPM));
                    //CustomerPM customerpm = xsSubmit.Deserialize(filestream) as CustomerPM;

                    //CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerProxy.CustomerWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
                    //{

                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    //    CustomerProxy.Response response = new CustomerProxy.Response();

                    //    var result = customerservice.Upsert(customerpm, false);



                    //}

                    //UserProxy.UserWcfServiceClient userservice = new UserWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)userservice.InnerChannel))
                    //{
                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    //    XmlSerializer serializer = new XmlSerializer(typeof(UserProxy.UserPM));
                    //    UserProxy.UserPM userpm = serializer.Deserialize(filestream) as UserProxy.UserPM;

                    //    var result = userservice.Upsert(userpm, false);

                    //}

                    //ActivityProxy.ActivityWcfServiceClient activityProxy = new ActivityProxy.ActivityWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)activityProxy.InnerChannel))
                    //{

                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    //    XmlSerializer serializer = new XmlSerializer(typeof(ActivityProxy.ActivityPM));
                    //    ActivityProxy.ActivityPM mypm = serializer.Deserialize(filestream) as ActivityProxy.ActivityPM;
                    //    mypm.BusinessUnitId = mypm.Tenant.ToString();
                    //    mypm.ActivityDocumentDatas.FirstOrDefault().EntityId = null;
                    //    //PropertyInfo[] properties = mypm.GetType().GetProperties();

                    //    var response = activityProxy.Upsert(mypm, "eladan@amital.co.il");

                    //    var m = "";
                    //}
                    //foreach (PropertyInfo inf in properties)
                    //{
                    //    var value = inf.GetValue(mypm);
                    //    if (value != null)
                    //    {
                    //        if (string.IsNullOrEmpty(value.ToString()))
                    //        {
                    //            inf.SetValue(mypm, null);
                    //        }
                    //    }
                    //}


                    //QuoteProxy.QuoteWcfServiceClient quoteService = new QuoteProxy.QuoteWcfServiceClient();

                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)quoteService.InnerChannel))
                    //{
                    //    filestream.Position = 0;
                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    //    XmlSerializer xsSubmit33 = new XmlSerializer(typeof(QuotePM));
                    //    QuotePM pm11 = xsSubmit33.Deserialize(filestream) as QuotePM;
                    //    QuoteProxy.Response quoteresponse = quoteService.Upsert(pm11, false);



                    //    // QuoteProxy.Response quoteresponse = quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);


                    //}





                    //QuoteProxy.QuoteWcfServiceClient quoteService = new QuoteProxy.QuoteWcfServiceClient();

                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)quoteService.InnerChannel))
                    //{
                    //    filestream.Position = 0;
                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    //    XmlSerializer xsSubmit33 = new XmlSerializer(typeof(QuotePM));
                    //    QuotePM pm11 = xsSubmit33.Deserialize(filestream) as QuotePM;
                    //    QuoteProxy.Response quoteresponse = quoteService.Upsert(pm11, false);



                    //    // QuoteProxy.Response quoteresponse = quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);


                    //}

                    //ShipmentProxy.ShipmentWcfServiceClient shipmentService = new ShipmentWcfServiceClient();

                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentService.InnerChannel))
                    //{
                    //    ShipmentProxy.ShipmentPM pm = ObjectXmlSerializer.DeserializeObject<ShipmentProxy.ShipmentPM>(filestream);
                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    //    pm.ConvertFromDirectToHouse = true;
                    //    ShipmentProxy.Response myresp = shipmentService.Upsert(pm, false);

                    //}





                    //AddressProxy.AddressWcfServiceClient addressService = new AddressProxy.AddressWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)addressService.InnerChannel))
                    //{
                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    //    AddressProxy.AddressPM address = ObjectXmlSerializer.DeserializeObject<AddressProxy.AddressPM>(filestream);

                    //    var result = addressService.Upsert(address, false);
                    //}

                    //DocumentTypeProxy.DocumentTypeWcfServiceClient doctypeservice = new DocumentTypeProxy.DocumentTypeWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)doctypeservice.InnerChannel))
                    //{
                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    //    DocumentTypeProxy.DocumentTypePM dotype = ObjectXmlSerializer.DeserializeObject<DocumentTypeProxy.DocumentTypePM>(filestream);

                    //    var result = doctypeservice.Upsert(dotype,false);
                    //}
                    // LoginToAmitalIIGTest();

                    //ActivityProxy.ActivityWcfServiceClient activityProxy = new ActivityProxy.ActivityWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)activityProxy.InnerChannel))
                    //{

                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    //    XmlSerializer serializer = new XmlSerializer(typeof(ActivityProxy.ActivityPM));
                    //    ActivityProxy.ActivityPM mypm = serializer.Deserialize(filestream) as ActivityProxy.ActivityPM;

                    //    PropertyInfo[] properties = mypm.GetType().GetProperties();

                    //    foreach (PropertyInfo inf in properties)
                    //    {
                    //        var value = inf.GetValue(mypm);
                    //        if ( value != null)
                    //        {
                    //            if(string.IsNullOrEmpty(value.ToString()))
                    //            {
                    //                inf.SetValue(mypm, null);
                    //            }
                    //        }
                    //    }


                    //    foreach (ActivityProxy.ActivityEmailRecipientPM pm in mypm.ActivityEmailRecipients)
                    //    {
                    //        PropertyInfo[] properties2 = pm.GetType().GetProperties();

                    //        foreach (PropertyInfo inf in properties2)
                    //        {
                    //            var value = inf.GetValue(pm);
                    //            if (value != null)
                    //            {
                    //                if (string.IsNullOrEmpty(value.ToString()))
                    //                {
                    //                    inf.SetValue(pm, null);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    foreach (DocumentDataPM pm in mypm.ActivityDocumentDatas)
                    //    {
                    //        PropertyInfo[] properties3 = pm.GetType().GetProperties();

                    //        foreach (PropertyInfo inf in properties3)
                    //        {
                    //            var value = inf.GetValue(pm);
                    //            if (value != null)
                    //            {
                    //                if (string.IsNullOrEmpty(value.ToString()))
                    //                {
                    //                    inf.SetValue(pm, null);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    var result = activityProxy.Upsert(mypm, "eladan@amital.co.il");



                    //}

                    //AmitalIIGTestCustomerProxy.CustomerWcfServiceClient customerservice = new AmitalIIGTestCustomerProxy.CustomerWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
                    //{

                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", "2");

                    //    XmlSerializer serializer = new XmlSerializer(typeof(AmitalIIGTestCustomerProxy.CustomerPM));
                    //    AmitalIIGTestCustomerProxy.CustomerPM mypm = serializer.Deserialize(filestream) as AmitalIIGTestCustomerProxy.CustomerPM;

                    //    AmitalIIGTestCustomerProxy.Response response = new AmitalIIGTestCustomerProxy.Response();
                    //    var result = customerservice.Upsert(mypm, false);



                    //}


                    //Login();



                    //LoginToCloud();
                    //XmlSerializer xsSubmit = new XmlSerializer(typeof(CustomerPM));
                    //CustomerPM pm = xsSubmit.Deserialize(filestream) as CustomerPM;

                    //CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerProxy.CustomerWcfServiceClient();
                    //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
                    //{

                    //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                    //    CustomerProxy.Response response = new CustomerProxy.Response();

                    //    var result = customerservice.Upsert(pm, false);



                    //}
                    //  }

                }
            }
        }

        private void btnDocsIn_Click(object sender, EventArgs e)
        {//DocumentsMetaDataType
            Login("yaronc@amital.co.il", "!Y123456");
           // Login();
           // LoginToCustoms();
          

           // UpsertDocumentData();

            FileInfo fileInfo = UploadFile();
           // InsertActivity(fileInfo);

            DocumentInProxy.DocumentInWcfServiceClient documentsInService = new DocumentInProxy.DocumentInWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documentsInService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                HypredTest.DocumentInProxy.DocumentsFilingPM documentDataPM = new HypredTest.DocumentInProxy.DocumentsFilingPM()
                {
                    Code = "mn65r1",
                    Id = "998989889898000000000000000000",
                    DirectionCode = "I",
                    Tenant = 1,
                    DocumentTypeId = "770",
                    Description = "hola you! hola you!hyou!!",
                    // EntityId = "1-55",
                    FileData = fileInfo.FileData,
                    FileExtension = fileInfo.FileExtension.ToUpper(),
                    Notes = "api service",
                    //ObjectTableId = "Opportunity",
                    CreatedByUserId = "HybridSystemUser",
                    OwnerId = "HybridSystemUser",
                    UpdatedByUserId = "HybridSystemUser",
                    ExternalEntityName = "customsfile",
                    ExternalEntityReference = "87454545",
                    FileName = fileInfo.FieName,
                    ObjectTableName = "Customer",
                    EntityNumber = "70000",
                    // IsDeleted = true,
                    //  DeletedByUserId = "HybridSystemUser",
                    // DeleteDateTime = DateTime.Now,
                    IsDigitallySigned = true,
                    SignersList = "islam, morsi"
                };


                Response response = documentsInService.Upsert(documentDataPM, false);

                //DocumentsFilingPM documentDataPM = new DocumentsFilingPM()
                //{
                //    Code = "lll",
                //    Id = "oppeppp3",
                //    DirectionCode = "I",
                //    Tenant = 1,
                //    DocumentTypeId = "770",
                //   // EntityId = "1-55",
                //    FileData = fileInfo.FileData,
                //    FileExtension = fileInfo.FileExtension.ToUpper(),
                //    Notes = "api service",
                //    //ObjectTableId = "Opportunity",
                //    CreatedByUserId = "HybridSystemUser",
                //    OwnerId = "HybridSystemUser",
                //    UpdatedByUserId = "HybridSystemUser",
                //    ExternalEntityName="customsfile",
                //    ExternalEntityReference="99999999",
                //    FileName = fileInfo.FieName,
                //    ObjectTableName = "Shipment",
                //    EntityNumber = "1047",
                //   // IsDeleted = true,
                //  //  DeletedByUserId = "HybridSystemUser",
                //   // DeleteDateTime = DateTime.Now,
                //   IsDigitallySigned = true,
                //   SignersList = "islam, morsi"
                //};
               
               // documentDataPM.DocumentsFilingMetaDataValues = new DocumentsFilingMetaDataValuePM[] { new DocumentsFilingMetaDataValuePM() { DocumentsMetaDataTypeId = "AAA", Tenant = 1, MetaDataValue = "hello this is me!" } };


                //DocumentDataPM documentData = new DocumentDataPM()
                //{
                //    DocumentTypeId = "CUST",
                //    DocumentTypeName = "Document1",
                //    EntityId = "1-87583",
                //    FileData = fileInfo.FileData,
                //    FileExtension = fileInfo.FileExtension.ToLower(),
                //    FileName = "Testing",
                //    Notes = "testing api",
                //    Tenant = 7,
                //    UserEmail = "eladan@amital.co.il",
                //    ObjectTableName = "Customer"
                //};
                //DocumentInProxy.Response response = documentsInService.UpsertDocumentData(documentData, false);


            }
        }


        private void UpsertDocumentData()
        {
            FileInfo fileInfo = UploadFile();

            DocumentInProxy.DocumentInWcfServiceClient documentsInService = new DocumentInProxy.DocumentInWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documentsInService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                DocumentDataPM documentDataPM = new DocumentDataPM()
                {
                    Code = "4544",
                    ExternalCode = "T99R",
                
                    Tenant = 1,
                    DocumentTypeId = "770",
                    // EntityId = "1-55",
                    FileData = fileInfo.FileData,
                    FileExtension = fileInfo.FileExtension,
                    Notes = "api service",
                    //ObjectTableId = "Opportunity",
                    UserEmail = "islam@fnarsoft.com",
                    ExternalEntityName = "customsfile",
                    ExternalEntityReference = "99999999",
                    FileName = fileInfo.FieName,
                    ObjectTableName = "Shipment",
                    EntityId = "1-4",
                };

                //documentDataPM.DocumentsFilingMetaDataValues = new DocumentsFilingMetaDataValuePM[] { new DocumentsFilingMetaDataValuePM() { DocumentsMetaDataTypeId = "AAA", Tenant = 1, MetaDataValue = "hello this is me!" } };

                Response response = documentsInService.UpsertDocumentData(documentDataPM, false);

            }

        }
        private void InsertActivity(FileInfo attachment)
        {

            ActivityProxy.ActivityWcfServiceClient activityService = new ActivityProxy.ActivityWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)activityService.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                ActivityProxy.ActivityPM activity = new ActivityProxy.ActivityPM()
                {
                    Tenant = 1,
                    ActivityStatusCode = "I",
                    ActivityTypeCode = "EI",
                    Subject = "yellow activity",
                    DueDate = DateTime.Now.AddDays(2),
                    OutlookId = "qwqw-333-4100-174027",//Guid.NewGuid().ToString(),
                    StartDateTime = DateTime.Now.AddDays(2),
                    EndDateTime = DateTime.Now.AddDays(3),
                    CompleteDate = DateTime.Now.AddDays(3),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    BusinessUnitId = "1",
                    Description = "my email in",
                    //
                    //QuoteId = "1-107",
                    //QuoteNumber = "A1035",
                    ObjectTableName = "Opportunity",
                    OpportunityId = "1-54",
                   
                    
                };

                // activity.ActivityInvitees = new ActivityProxy.ActivityInviteePM[] { new ActivityProxy.ActivityInviteePM() { Email = "mohammad@fnarsoft.com", Tenant = 1 }, new ActivityProxy.ActivityInviteePM() { Email = "abbas@fnarsoft.com", IsRequired = true, Tenant = 1 }, new ActivityProxy.ActivityInviteePM() { Email = "morsi@fnarsoft.com", IsRequired = true, Tenant = 1, } };
                activity.ActivityEmailRecipients = new ActivityProxy.ActivityEmailRecipientPM[] { new ActivityProxy.ActivityEmailRecipientPM() { Email = "mohammad@fnarsoft.com", Tenant = 1, RecipientTypeCode = "TO" }, new ActivityProxy.ActivityEmailRecipientPM() { Email = "islam@fnarsoft.com", Tenant = 1, RecipientTypeCode = "BCC", }, new ActivityProxy.ActivityEmailRecipientPM() { Email = "mohammad@fnarsoft.com", Tenant = 1, RecipientTypeCode = "CC" } };
                if (attachment != null)
                {
                    activity.ActivityDocumentDatas = new DocumentDataPM[]{
                    new DocumentDataPM(){
                   // ExternalId = "hybd_D_1888",
                    Tenant = 1,
                    DocumentTypeId = "OPPO", //carrier quote
                    FileData = attachment.FileData,
                    FileExtension = attachment.FileExtension,
                    Notes = "activity",
                    ObjectTableName = "Activity",
                    UserEmail = "islam@fnarsoft.com",
                    FileName = attachment.FieName,
                     
                      },
                
                };
                }
                
                Response response = activityService.Upsert(activity, "islam@fnarsoft.com");

            }

        }

        private string LoginByCredential(string key, int tenant)
        {
            LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
            Response loginResponse = loginService.LoginByCredential("", new LoginProxy.APICredentialsParameters() { PrimaryKey = key, Tenant = tenant });
            if (!loginResponse.HasError)
            {
                Token = loginResponse.Result;
            }

            return Token;
        }

        private string Login()
        {
            LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
            Response loginResponse = loginService.Login("maheera@fnarsoft.com", "0");//"tomerp@amital.co.il", "!T123456");  ("islam@logitudeworld.com", "!I123456");//("yaronc@amital.co.il", "!Y123456");//"yaronc@amital.co.il", "!Y123456");//
            if (!loginResponse.HasError)
            {
                Token = loginResponse.Result;
            }

            return Token;
        }

        private string LoginToTest()
        {
            LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
            Response loginResponse = loginService.Login("ahmadb@logitudeworld.com", @"!A123456");//"tomerp@amital.co.il", "!T123456");  ("islam@logitudeworld.com", "!I123456");//("yaronc@amital.co.il", "!Y123456");//"yaronc@amital.co.il", "!Y123456");//
            if (!loginResponse.HasError)
            {
                Token = loginResponse.Result;
            }

            return Token;
        }

        private string Login(string email,string password)
        {
            LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
            Response loginResponse = loginService.Login(email, password);//"tomerp@amital.co.il", "!T123456");
            if (!loginResponse.HasError)
            {
                Token = loginResponse.Result;
            }

            return Token;
        }


        private string LoginToCustoms()
        {
            LoginProxy.LoginWcfServiceClient loginService = new LoginProxy.LoginWcfServiceClient();
            Response loginResponse = loginService.Login("mohammad@fnarsoft.com", "1");//"tomerp@amital.co.il", "!T123456");
            if (!loginResponse.HasError)
            {
                Token = loginResponse.Result;
            }

            return Token;
        }

    

        private string LoginToCloudWithCredentails(string primaryKey, int tenant)
        {
            //LogingProxyCloud.LoginWcfServiceClient loginService = new LogingProxyCloud.LoginWcfServiceClient();
            //LogingProxyCloud.Response loginResponse = loginService.LoginByCredential(null, new LogingProxyCloud.APICredentialsParameters() { PrimaryKey = primaryKey, Tenant = tenant });
            //if (!loginResponse.HasError)
            //{
            //    Token = loginResponse.Result;
            //}

            return Token;
        }

        //

        private string LoginToAmitalIIGTest()
        {
            //AmitalIIGTestLoginProxy.LoginWcfServiceClient loginService = new AmitalIIGTestLoginProxy.LoginWcfServiceClient();
            //AmitalIIGTestLoginProxy.Response loginResponse = loginService.Login("yaronc@amital.co.il", "!Y123456");//"tomerp@amital.co.il", "!T123456");
            //if (!loginResponse.HasError)
            //{
            //    Token = loginResponse.Result;
            //}

            return Token;
        }


        private string LoginToCloudOnline()
        {

            //LogingProxyCloud.LoginWcfServiceClient loginService = new LogingProxyCloud.LoginWcfServiceClient();
            //LogingProxyCloud.Response loginResponse = loginService.Login("eladan@amital.co.il", "!E123456");//"tomerp@amital.co.il", "!T123456");
            //if (!loginResponse.HasError)
            //{
            //    Token = loginResponse.Result;
            //}

            return Token;
        }

        private FileInfo UploadFile()
        {

            FileInfo fileinfo = new FileInfo();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            // openFileDialog.Filter = "mrt|*.mrt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                using (Stream filestream = openFileDialog.OpenFile())
                {
                    byte[] fileData = new byte[filestream.Length];
                    filestream.Read(fileData, 0, System.Convert.ToInt32(filestream.Length));
                    string[] fileinfoarr = openFileDialog.SafeFileName.Split('.');
                    fileinfo.FieName = fileinfoarr[0];
                    fileinfo.FileExtension = fileinfoarr[1];
                    fileinfo.FileData = fileData;

                }

            }

            return fileinfo;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Login();
            TestCustomerService(Token);
            //TestShipmentService();

            //decimal mydec = 1298989741233.8784431545454999m;
            //mydec = Math.Truncate(mydec * 1000000m) / 1000000m;
            //Console.WriteLine(mydec); // 12.878

            // LoginToCloud();
            //Login("yaronc@amital.co.il","!Y123456");

            //DocumentInProxy.DocumentInWcfServiceClient documentsInService = new DocumentInProxy.DocumentInWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documentsInService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    DocumentInProxy.Response response = new DocumentInProxy.Response();
            //    var m = documentsInService.GetDocumentDataByExternalIdWithoutBinaray("ngctzfuhbky5_qbkhh+kew00000000", 20, ref response);
            //}

            //Token = "AVhvxABWFNq5dmKYIaelKhInSqsDsqQqdWQ=";
            Login();
            TestWarehouseService(Token);

            return;

            TestCustomerService(Token);
            GetExternalTasksFromQueue(1, 1);
            TestShipmentService();

            Token = "e77I+6qjFreZnZ0rjp0gMruKhBOOp6plHW4=";//Token: e77I+6qjFreZnZ0rjp0gMruKhBOOp6plHW4=
            TestShipmentService();
            //TenantManagementDWProxy.TenantManagementDWWcfServiceClient tenantManagementService = new TenantManagementDWProxy.TenantManagementDWWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)tenantManagementService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    TenantManagementDWProxy.Response response = new TenantManagementDWProxy.Response();

            //    var result = tenantManagementService.GetTenantManagements(341, 0, 10000,ref response);
            //}

            

            //

            // return;
            //LoginToCloud();

           
            TestShipmentEvents();

            //CardContactProxy.CardContactWcfServiceClient cardContactService = new CardContactProxy.CardContactWcfServiceClient();
            //cardContactService.Endpoint.Address= new EndpointAddress("http://localhost:9996/WcfApi/CardContactWcfService.svc");
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)cardContactService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    //CardContactProxy.Response response = new CardContactProxy.Response();
            //    //var result = cardContactService.GetCardContactPM("HybridCont", "HypredC1", 1, ref response);
            //    var cardContact = new CardContactProxy.CardContactPM()
            //    {
            //        CardId = "1d710cd0-db3c-4",
            //        ContactId = "HybridCont",
            //        IsHybrid = true,
            //        IsAll = true,
            //        Tenant = 1,
            //    };
            //    cardContact.CardContactProducts = new List<CardContactProxy.CardContactProductPM>()
            //    {
            //        new CardContactProxy.CardContactProductPM()
            //        {
            //             ProductTypeCode=  "AD",
            //              Tenant = 1,

            //        },
            //           new CardContactProxy.CardContactProductPM()
            //        {
            //             ProductTypeCode=  "AI",
            //              Tenant = 1,

            //        },
            //        //        new CardContactProxy.CardContactProductPM()
            //        //{
            //        //     ProductTypeCode=  "DL",
            //        //      Tenant = 1,

            //        //},

            //    }.ToArray();
            //    var m = cardContactService.Upsert(cardContact, false);

            //}


            UserProxy.UserWcfServiceClient userservice = new UserProxy.UserWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)userservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                UserPM user = new UserPM()
                {
                    Id = "ISLM",
                    Code = "IEEE",
                    BranchId = "HybridB1",
                    DepartmentId = "HybridD1",
                    EnglishName = "Yaron Cohen",
                    LocalName = "المستخدم1",
                    Tenant = 1,
                    Email = "islam-ext5@external.com",
                    Password = "123",
                    Notes = "new",
                    UserType = "R",
                    InActive = false,
                    BusinessUnitId = "1",

                };
                var m = userservice.Upsert(user, false);
            }

            GetExternalTasksFromQueue(1, 1);

            //HybridTenantStateProxy.HybridTenantStateWcfServiceClient hubridservice = new HybridTenantStateProxy.HybridTenantStateWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)hubridservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    var result = hubridservice.Upsert(new HybridTenantStateProxy.HybridTenantStatePM() { Tenant = 1, FailedQueue = 0, WaitingQueue = 0,LastQueueDateTime = DateTime.Now.AddDays(-1) }, false);

            //}

            //TestCurrencyService();

            //HybridPartnersPermissionsProxy.HybridPartnersPermissionsWcfServiceClient hybridPartnerPermService = new HybridPartnersPermissionsProxy.HybridPartnersPermissionsWcfServiceClient();
             
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)hybridPartnerPermService.InnerChannel))
            //{

            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    HybridPartnersPermissionsProxy.Response response = new HybridPartnersPermissionsProxy.Response();
            //    var result = hybridPartnerPermService.Upsert(3, 2, false);
            //    var result2 = hybridPartnerPermService.Upsert(6, 2, false);
            //    var result3 = hybridPartnerPermService.Upsert(6, 3, false);

            //    var allowedPartners = hybridPartnerPermService.GetAllowedPartners(6, ref response);
            //}
            //TestQuotationDocument();
            //TestShipmentEvents();

            TestCustomerService(Token);




            //TestQuotationDocument();

            //MAIN_20274309
            //20274309


            //CardContactProxy.CardContactWcfServiceClient cardContactService = new CardContactProxy.CardContactWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)cardContactService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    //CardContactProxy.Response response = new CardContactProxy.Response();
            //    //var result = cardContactService.GetCardContactPM("HybridCont", "HypredC1", 1, ref response);
            //    var m = cardContactService.Upsert(new CardContactProxy.CardContactPM()
            //    {
            //        CardId = "20274309",
            //        ContactId = "MAIN_20274309",
            //        IsHybrid = true,
            //        IsAll = true,
            //        Tenant = 3,
            //    }, false);
            //}

            //ContactProxy.ContactWcfServiceClient contactService = new ContactProxy.ContactWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)contactService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    ContactProxy.ContactPM pm = new ContactProxy.ContactPM()
            //    {
            //        ExternalId = "IslamExternalCon2",
            //        EnglishName = "test2",
            //        IsHybrid = true,
            //        Email = "islam-ext2@external.com",
            //        Tenant = 1,
            //    };

            //    ContactProxy.Response response = new ContactProxy.Response();
            //    var result = contactService.Upsert(pm, false);
            //}













            TestShipmentService();

            //FeatureProxy.FeatureWcfServiceClient featuresService = new FeatureProxy.FeatureWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)featuresService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    FeatureProxy.Response response = new FeatureProxy.Response();
            //    List<FeatureProxy.FeatureAccessInfo> list = new List<FeatureProxy.FeatureAccessInfo>() { };
            //    list.Add(new FeatureProxy.FeatureAccessInfo() { FeatureCode = "READ", ObjectTableName = "Shipment" });
            //    list.Add(new FeatureProxy.FeatureAccessInfo() { FeatureCode = "READ", ObjectTableName = "DocumentFolder" });
            //    var result = featuresService.GetActiveFeaturesForUser(list.ToArray(), 1, ref response);

            //}



         


            //HybridTenantStateProxy.HybridTenantStateWcfServiceClient hubridservice = new HybridTenantStateProxy.HybridTenantStateWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)hubridservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    var result = hubridservice.Upsert(new HybridTenantStateProxy.HybridTenantStatePM() { Tenant = 1, FailedQueue = 0, WaitingQueue = 0 }, false);

            //}


            //TestPortService();
            //TestUserService();
            //InsertActivity(null);


            //string m = "545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545545454554545455454545yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttthisisessparta";
            //int l = m.Length;
            //string mySearchFields = m.Substring(0, 1000);//TestPortService();
            //int l1 = mySearchFields.Length;
           // TestQuotationDocument();
//
           
           // CreateShipmentEvent();
           // TestDocumentIn();
            //FileInfo fileInfo = UploadFile();
            // InsertActivity(fileInfo);
            //TestCustomerActivation();
           // TestDocumentsMetaDataTypeService();
          //  TestCustomerService(Token);
           // TestCustomerService("11111");
           // TestContactPasswordService();
           // TestPackageTypeService();
           // TestDeleteEvent();
            //TestAutoSignupTest();
           // TestCustomerService();
            //TestQuoteEvents();
            //TestCityService();
           // TestContactService();
           // TestShipmentEvents();
        //    TetShipmentService();
          // TestQuoteService();
          //  TestDocumentIn();
           // TestCustomerActivation();

            //DocumentTypeProxy.DocumentTypeWcfServiceClient documenttypeservice = new DocumentTypeProxy.DocumentTypeWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documenttypeservice.InnerChannel))
            //{

            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    DocumentTypeProxy.Response response = new DocumentTypeProxy.Response();
            //    DocumentTypeProxy.DocumentTypePM pm = new DocumentTypeProxy.DocumentTypePM()
            //    {
            //        Code = "DSA",
            //        Name = "Testing the document type api",
            //        Tenant = 48,
            //        ObjectTableName = "Shipment",
            //        IsDocIn = true,
            //        TemplateFormatCode = "M",
            //    };

            //    response = documenttypeservice.Upsert(pm, false);

            //    var resut = documenttypeservice.GetDocumentTypes("Shipment", 48, 0, 10, ref response);
            //}

        }


        void TestUserService()
        {
            UserProxy.UserWcfServiceClient userservice = new UserProxy.UserWcfServiceClient();

          


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)userservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                UserPM user = new UserPM()
                {
                    Id = "ISLM",
                    Code = "I4SS",
                    BranchId = "HybridB1",
                    DepartmentId = "HybridD1",
                    EnglishName = "Yaron Cohen",
                    LocalName = "المستخدم1",
                    Tenant = 1,
                    Email = "test_Email@islam.COM",
                    Password = "123",
                    Notes = "new",
                    UserType = "R",
                    InActive = false,
                    BusinessUnitId = "1",

                };
                var m = userservice.Upsert(user, false);
            }

        }

        void TestCurrencyService()
        {
            //CurrencyProxy.CurrencyWcfServiceClient currencyservice = new CurrencyWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)currencyservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    CurrencyProxy.Response response = new CurrencyProxy.Response();
                 
            //    var result = currencyservice.GetList(new CurrencyProxy.ApiSearchFilters() { SearchFields = null, Take = 10, Skip = 0 }, 0, ref response);

                 

            //}



            //CountryProxy.CountryWcfServiceClient countryservice = new CountryWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)countryservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    CountryProxy.Response response = new CountryProxy.Response();
            //    //quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);
            //    //var result = countryservice.GetList(new HypredTest.CountryProxy.WcfApiFilters() { SearchFields = "us", Take = 10, Skip = 0 }, 1, ref response);



            //}
        }
        void TestPortService()
        {
            PortProxy.PortWcfServiceClient portservice = new PortWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)portservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                Response response = new Response();
                //quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);
               //var result = portservice.GetList(new HypredTest.PortProxy.WcfApiFilters() { SearchFields = null, Take = 10, Skip = 0 }, 1, ref response);

                var id=  portservice.GetPortId(new PortApiFilters() { PortCode = "TLV", CountryCode = "IL" }, 1, ref response);

            }



            CountryProxy.CountryWcfServiceClient countryservice = new CountryWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)countryservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

               Response response = new Response();
                //quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);
                //var result = countryservice.GetList(new HypredTest.CountryProxy.WcfApiFilters() { SearchFields = "us", Take = 10, Skip = 0 }, 1, ref response);

                

            }
        }
        //
        void TestQuotationDocument()
        {
            FileInfo fileInfo = UploadFile();

            QuoteProxy.QuoteWcfServiceClient quoteservice = new QuoteWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)quoteservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);


                //quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);
                Response eventResponse = quoteservice.UploadQuotationDocument("AEAH-13712", fileInfo.FileData, fileInfo.FileExtension, "NOFARG", 4);
 


            }
        }


        //private async void LoginWithCredentials()
        //{


        //    APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
        //    {
        //        PrimaryKey = "V4ZcefFhdn/I1vXmbR95Gg==",

        //    };
        //    using (var client = new HttpClient())
        //    {
        //        string uri = "http://localhost:9996/api/";
        //        string AuthURI = uri + "APIAuthentication";
        //        var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
        //        var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        //        var result = await client.PostAsync(AuthURI, content);
        //        var tempUser = result.Content.ReadAsStringAsync().Result;
        //        ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(tempUser);
        //        Token = User.Token;


        //    }

        //}



        private void CreateShipmentEvent()
        {
            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                ShipmentProxy.TraceEventPM opopevent = new HypredTest.ShipmentProxy.TraceEventPM() { Tenant = 1, ExternalId = "EV1114444", UserId = "HybridSystemUser", EventTypeCode = "OPOP", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid operational opend (list call)" };
               // ShipmentProxy.TraceEventPM depevent = new HypredTest.ShipmentProxy.TraceEventPM() { Tenant = 1, ExternalId = "EV222243", UserId = "HybridSystemUser", EventTypeCode = "DEP", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now.AddDays(-1), Notes = "Testing hybrid departed (list call)" };

                List<ShipmentProxy.TraceEventPM> eventsList = new List<ShipmentProxy.TraceEventPM>();
                eventsList.Add(opopevent);
                //eventsList.Add(depevent);
              

                //ShipmentProxy.Response response1 = shipmentservice.CreateEvent(1, "EV11111", "I10009", "HybridSystemUser", "OPOP", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-2), "Testing hybrid operational opend (single event call)");
                //ShipmentProxy.Response response2 = shipmentservice.CreateEvent(1, "EV22222", "I10009", "HybridSystemUser", "DEP", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), "Testing hybrid departed (single event call)");



                Response eventResponse2 = shipmentservice.BuildEventsList(1, "ship_7126", eventsList.ToArray());

            }
        }

        private void TestDocumentsMetaDataTypeService()
        {
            //DocumentsMetaDataTypeProxy.DocumentsMetaDataTypeWcfServiceClient documentsservice = new DocumentsMetaDataTypeProxy.DocumentsMetaDataTypeWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documentsservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    DocumentsMetaDataTypeProxy.DocumentsMetaDataTypePM pm = new DocumentsMetaDataTypeProxy.DocumentsMetaDataTypePM()
            //    {
            //        Code = "AAA",
            //        EnglishName = "Test",
            //        LocalName = "فحص",

            //        Format = "txt",
            //        Tenant = 1,


            //    };
            //    DocumentsMetaDataTypeProxy.Response response = documentsservice.Upsert(pm, false);
            //}
        }

        private void TestContactPasswordService()
        {
            //ContactPasswordProxy.ContactPasswordServiceClient passwordservice = new ContactPasswordProxy.ContactPasswordServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)passwordservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
            //    ContactPasswordProxy.Response response = passwordservice.ChangeContactPassword("islam@fnarsoft.com", "1", "0");
            //}
        }


        private void TestPackageTypeService()
        {
            //PackageTypeProxy.PackageTypeWcfServiceClient packageTypeService = new PackageTypeProxy.PackageTypeWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)packageTypeService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    PackageTypeProxy.Response response = new PackageTypeProxy.Response();
            //    var result = packageTypeService.GetPackageTypeList(new PackageTypeProxy.PackageTypeApiFilters() { Skip = 0, Take = 20, SearchFields = "zzz"}, 1, ref response);
            //}
        }

        private void TestDeleteEvent()
        {
        //    ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();


        //    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
        //    {
        //        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);


        //        ShipmentProxy.Response eventResponse = shipmentservice.DeleteShipmentEvent("SHIP_7095", "1234", 1);


        //    }


            QuoteProxy.QuoteWcfServiceClient quoteservice = new QuoteProxy.QuoteWcfServiceClient();


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)quoteservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);


                Response eventResponse = quoteservice.DeleteQuoteEvent("a1036", "111", 1);


            }
        }

        private void TestAutoSignupTest()
        {
            //AutoSignUpProxy.AutoSignUpWcfServiceClient autoservice = new AutoSignUpProxy.AutoSignUpWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)autoservice.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    AutoSignUpProxy.AutoSignUpData data = new AutoSignUpProxy.AutoSignUpData()
            //    {
            //        Email = "abdallah@mail1.com",
            //        ContactName = "abdallah",
            //        CompanyName = "hejjawi",
            //        Country = "Palestine",
            //        NumberOfUsers = 10,
            //        NumberOfBranches = 2,
            //        PhoneNumber = "022984254",
            //        Comments = "testing",
            //    };

            //    var response = autoservice.Insert(data, false);

            //}
        }
        CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerWcfServiceClient();
        public Response TestCustomerService(string token, string paymentTermId = null)
        {
           

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                // CustomerPM pm = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "10107933" }, 8, ref resultResponse);
                CustomerProxy.CustomerPM newCustomer = new CustomerProxy.CustomerPM()
                {
                    Code = "HBRDWesam",
                    EnglishName = "hybrid customer Wesam",
                    Tenant = 1,
                    PartnerTypeId = "CS",
                    SalesmanUserId = "HybridU1",
                    VatNumber = "123456",
                    CustomerStatusCode = "POT",
                    //SetActivated = true,
                    CreditLimitAmount = 50.65,
                    CountryCode = "IL",
                    PaymentTermId = paymentTermId,
                    IsAutonomy = false,

                    
                };

                //CustomerSalesmanByProductPM[] salesmanbyproducts = new CustomerSalesmanByProductPM[] { 
                //    new CustomerSalesmanByProductPM(){ ProductTypeCode = "AD", SalesmanUserId = "HybridU1", Tenant =1},
                //    new CustomerSalesmanByProductPM(){ ProductTypeCode = "AE", SalesmanUserId = "HybridU1", Tenant =1},
                //    new CustomerSalesmanByProductPM(){ ProductTypeCode = "CI", SalesmanUserId = "HybridU1", Tenant =1},
                        

                //};

                //newCustomer.CustomerSalesmanByProducts = salesmanbyproducts;

                var response = customerservice.Upsert(newCustomer, false);

                return response;
                //  var customer = customerservice.GetCustomerPM(new CustomerApiFilters() { ByVatNumber = true, SearchCode = "1234562322222" }, 1);
                //  var customer2 = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "470690c4-8f9a-4-121212" }, 1);
                // var contacts = customerservice.GetCustomerContacts(new CustomerApiFilters() { ByCode = true, SearchCode = "70002-1212" }, 1);
                 //var addresses = customerservice.GetCustomerAddresses(new CustomerApiFilters() { ByCode = true, SearchCode = "10110529" }, 10, ref resultResponse);

            }

        //      [Key]
        //public string ProductTypeCode { get; set; }

        //public string SalesmanUserId { get; set; }
        //[Key]
        //public string CustomerId { get; set; }

        //public int Tenant { get; set; }
        //public string SalesmanUserName { get; set; }
        }


        public Response TestAccountingPartnerService(string token, string paymentTermId = null)
        {


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)accountingPartnerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response(); 
                AccountingPartnerProxy.AccountingPartnerPM newAccountingPartner = new AccountingPartnerProxy.AccountingPartnerPM()
                {
                    Code = "HEHYBRID6",
                    EnglishName = "H Test",
                    Tenant = 1,
                    PartnerTypeId = "AC", 
                    VatNumber = "199996",  
                    CountryCode = "IL",
                    PaymentTermId = paymentTermId,
                    PrimaryContactName = "H TEst",
                    CollectorId = "test1"

                };
                  
                var response = accountingPartnerservice.Upsert(newAccountingPartner, false); 
                return response; 
            }
             
        }

        public Response TestGetCustomerPMService(string token)
        {


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                CustomerProxy.CustomerPM customerPM = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "TCA" }, 1, ref resultResponse);
               
                return resultResponse;
            }
        }

        public Response TestGetCustomerListByIdService(string token)
        {


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                CustomerProxy.CustomerList customerList = customerservice.GetCustomerListById("1-11108", 1, ref resultResponse);

                return resultResponse;
            }
        }
        public Response TestGetCustomerListByEmailService(string token)
        {


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                //CustomerProxy.CustomerList customerList = customerservice.GetCustomerListByEmail("1-11108", 1, ref resultResponse);

                return resultResponse;
            }
        }
        
        private void TestQuoteEvents()
        {
            QuoteProxy.QuoteWcfServiceClient quoteservice = new QuoteWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)quoteservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);


                //quoteService.UploadDocument("A1029", fileData, fileExtension, "HybridU1", 1);
                Response eventResponse = quoteservice.CreateEvent(1, "222", "A1029", "HybridSystemUser", "REMF", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), "Testing hybrid with id");

                List<HypredTest.QuoteProxy.TraceEventPM> events = new List<HypredTest.QuoteProxy.TraceEventPM>() 
                 { 
                     new HypredTest.QuoteProxy.TraceEventPM() { Tenant = 1, ExternalId = "555545", UserId = "HybridSystemUser", EventTypeCode = "QEMO", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid arrived" },
                     new HypredTest.QuoteProxy.TraceEventPM() { Tenant = 1, ExternalId = "2244222", UserId = "HybridSystemUser", EventTypeCode = "QUSG", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now.AddDays(-1), Notes = "Testing hybrid custom cleared" },

                 };

                Response eventResponse2 = quoteservice.BuildEventsList(1, "A1029", events.ToArray());


            }
        }

        private void TestCityService()
        {
            //Login();
            //CityProxy.CityWcfServiceClient cityService = new CityProxy.CityWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)cityService.InnerChannel))
            //{

            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    CityProxy.CountryCityPM pm = new CityProxy.CountryCityPM()
            //    {
            //        Code = "TTT",
            //        EnglishName = "Testing city",
            //        CountryId = "IL",
            //        LocalName = "فحص",
            //        Tenant = 1,
            //        IsHybrid = true,
            //    };

            //    CityProxy.Response quoteresponse = cityService.Upsert(pm, false);

            //    var result = cityService.GetCityListByCode("TTT", "IL", 1, ref quoteresponse);


            //}

        }

        private void TestContactService()
        {
            //ContactProxy.ContactWcfServiceClient contactService = new ContactProxy.ContactWcfServiceClient();
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)contactService.InnerChannel))
            //{
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    ContactProxy.ContactApiFilters filters = new ContactProxy.ContactApiFilters()
            //    {
                     
            //        Take = 10,
            //         Skip = 30,
            //         //SearchFields = "islam@fnarsoft.com"
            //    };

            //    ContactProxy.Response response = new ContactProxy.Response();
            //    var result = contactService.GetContactList(filters, 1, ref response);
            //}
        }

        private void TestShipmentEvents()
        {
            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            ShipmentProxy.ShipmentPM consolepm = new ShipmentPM()
            {
                ShipmentNumber = "SHIP_11444",
                ShipmentLevelCode = "A",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",
                DirectionId = "I",
                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                // House = "4545",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",
         

                ShipmentTypeId = null,//"LCL",
                //StatusId = "SHOR",
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                Master = "123456789",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                // ShipperReference2 
                // IsCancelled = true,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
            };


            //ShipmentProxy.ShipmentPM housepm = new ShipmentPM()
            //{
            //    ShipmentNumber = "HIBRID_H_10",
            //    MasterShipmentNumber = "HIBRID_C_10",
            //    MasterShipmentDataId = "1-77042",
            //    ShipmentLevelCode = "H",
            //    IsHybrid = true,
            //    Tenant = 1,
            //    AccessDate = DateTime.Now,
            //    AWBCurrencyId = "USD",
            //    BranchId = "HybridB1",
            //    DepartmentId = "HybridD1",
            //    ChargeableWeightUnitCode = "KG",
            //    ConsigneeId = "70000",
            //    ShipperId = "70000",
            //    ConsigneeReference1 = "PO35104",
            //    CreateDateTime = DateTime.Now,
            //    CreatedByUserId = "HybridU1",
            //    CutoffDate = DateTime.Now,
            //    DimensionsUnitCode = "CM",
            //    DirectionId = "I",
            //    FinalDistenationPortId = "TLV",
            //    FreightPrepaidCollectId = "C",
            //    FromPortId = "JFK",
            //    GrossWeightUnitCode = "KG",
            //    House = "1111111",
            //    IncotermId = "CIF",
            //    MainCarriageCarrierId = "LY",
            //    MainCarriageFinalDestinationPortId = "TLV",
            //    MainCarriageFromPortId = "JFK",
            //    MainCarriageToPortId = "TLV",
            //    OtherPrepaidCollectId = "C",
            //    ProfitCurrencyId = "NIS",
            //    ShipmentCustomerTypeCode = "SHI",
            //    ShipmentTypeId = null,//"LCL",
            //    //StatusId = "SHOR",
            //    ToPortId = "ILTLV",
            //    TransportModeId = "A",
            //    VolumeUnitCode = "CBM",
            //    ChargeableWeight = 0.9999984133,
            //    GrossWeight = 0.9999984133,
            //    //Master = "123456789",
            //    MainCarriageATA = DateTime.Now,
            //    MainCarriageETA = DateTime.Now,
            //    //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            //    // ShipperReference2 
            //    // IsCancelled = true,
            //    IsCancelled = false,
            //    Notes = "testing console via hybrid",

            //    //MasterShipmentDataId = "",
            //};

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                Response rspo1 = shipmentservice.Upsert(consolepm, false);

               // ShipmentProxy.Response rspo2 = shipmentservice.Upsert(housepm, false);

               //// ShipmentProxy.Response eventResponse = shipmentservice.CreateEvent(1, "olatest111", consolepm.ShipmentNumber, "HybridSystemUser", "DEP", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), "Testing hybrid with id");

               // List<HypredTest.ShipmentProxy.TraceEventPM> events = new List<HypredTest.ShipmentProxy.TraceEventPM>() 
               //  { 
               //      new HypredTest.ShipmentProxy.TraceEventPM() { Tenant = 1, ExternalId = Guid.NewGuid().ToString(), UserId = "HybridSystemUser", EventTypeCode = "PIOD", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now, Notes = "Testing hybrid Delivered" },
               //      new HypredTest.ShipmentProxy.TraceEventPM() { Tenant = 1, ExternalId = Guid.NewGuid().ToString(), UserId = "HybridSystemUser", EventTypeCode = "OPOP", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now, Notes = "Testing hybrid arrived" },
               //      new HypredTest.ShipmentProxy.TraceEventPM() { Tenant = 1, ExternalId = Guid.NewGuid().ToString(), UserId = "HybridSystemUser", EventTypeCode = "EXCE", EventDateTime = DateTime.Now.AddDays(-1), LogDateTime = DateTime.Now, Notes = "Testing hybrid Exception" },

               //  };


                //ShipmentProxy.Response eventResponse2 = shipmentservice.BuildEventsList(1, consolepm.ShipmentNumber, events.ToArray());


            }
            
        }

        private void TestCustomerActivation()
        {
            CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerProxy.CustomerWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                Response response = new Response();

                var result = customerservice.GetCustomerPM(new CustomerApiFilters() { ById = true, SearchCode = "1-1000" }, 1, ref response);

                //var result = customerservice.GetReadyForActivationCustomer(1, ref response);

                //if (result != null)
                //{
                //    result.SetReady = true;
                //    CustomerProxy.Response response2 = customerservice.Upsert(result, false);
                //    if (!response2.HasError)
                //    {
                //        CustomerProxy.Response response3 = customerservice.RemoveFromCustomersQueue(result.QueueMessageLockToken, result.Tenant);
                //    }
                //}

            }
        }

        private void TestQuoteService()
        {

            QuoteProxy.QuoteWcfServiceClient quoteservice = new QuoteProxy.QuoteWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)quoteservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                //QuoteProxy.Response response = new QuoteProxy.Response();
                //QuoteProxy.QuoteApiFilters filters = new QuoteApiFilters()
                //{
                //    Skip = 0,
                //    Take = 5,
                //    OperationallyOpen = true,
                //    MyQuotes = true,
                //    Email = "islam@fnarsoft.com",
                //    SearchFields = "1011",

                //};

                //var result = quoteservice.GetQuoteList(filters, 1, ref response);
                ////quoteservice.

                QuoteProxy.QuotePM quotepm = new QuoteProxy.QuotePM()
                {
                    QuoteNumber = "Q82228888",
                    CreatedByUserId = "HybridU1",
                    QuoteTypeCode = "A",
                    Tenant = 1,
                    BranchId = "HybridB1",
                    DepartmentId = "HybridD1",
                    CustomerId = "70000",
                    ShipperId = "70000",
                    ConsigneeId = "70000",
                    FromPortId = "TLV",
                    ToPortId = "JFK",
                    MainCarriageCarrierId = "LY",
                    SalesmanUserId = "HybridU1",
                    SaleCurrencyId = "USD",
                    IncotermId = "CIF",
                    DirectionId = "I",
                    TransportModeId = "A",
                    ExchangeRate = 1,
                    QuoteCustomerTypeCode = "SHI",
                    CustomerName = "abdallah",
                    IsClosed = true,
                    ActionType = "Accept",
                    UpdatedByUserId = "HybridU1",
                    UpdateDate = DateTime.Now,
                    LastActivityDate = DateTime.Now,
                  
                    OpenDate =DateTime.Now,
                };

                var result = quoteservice.Upsert(quotepm, false);
            }
        }

        private void TestShipmentService()
        {
            ShipmentProxy.ShipmentPM consolepm = new ShipmentPM()
            {
                ShipmentNumber = "SHIP_718741",
                DirectionId = "R",
                ShipmentLevelCode = "A",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",
                
                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                // House = "4545",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",


                ShipmentTypeId = null,//"LCL",
                //StatusId = "SHOR",
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                Master = "123456789",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                // ShipperReference2 
                // IsCancelled = true,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
            };

            consolepm.ShipmentPickUps = new ShipmentPickUpPM[]
            {
                new ShipmentPickUpPM()
                { 
                    CarrierId = "TK",
                    FromAddressCity = "London",
                    FromAddressCountryId = "GB",

                    ToAddressCountryId = "FR",
                    ToAddressCity = "Paris",
                    
                    TruckNumber = "1111",
                    Driver = "My driver",

                    ETA = DateTime.Now.AddDays(-1),
                    ETD = DateTime.Now.AddDays(-1),
                    ATA = DateTime.Now,
                },
            };

           // TK
//RTR
//MTR
            consolepm.ShipmentDeliveries = new ShipmentDeliveryPM[]
           {
                 new ShipmentDeliveryPM()
                {
                    CarrierId = "MTR",

                    FromAddressCity = "Marseille",
                    FromAddressCountryId = "FR",

                    ToAddressCountryId = "GB",
                    ToAddressCity = "Bristol",

                    TruckNumber = "1111",
                    Driver = "Baker driver",

                    ETA = DateTime.Now.AddDays(-1),
                    ETD = DateTime.Now.AddDays(-1),
                    ATA = DateTime.Now,
                },
           };


            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(consolepm, false);
                //ShipmentProxy.ShipmentApiFilters filters = new ShipmentApiFilters()
                //{
                //    Skip = 0,
                //    Take = 5,
                //    // OperationallyOpen = true,
                //    // MyShipments= true,
                //    Email = "islam@fnarsoft.com",
                //    SearchFields = "89"

                //};

                //var result = shipmentservice.GetShipmentList(filters, 1, ref response);
                //string m = "";
                //foreach (var s in result)
                //    m += Environment.NewLine + s.SearchFieldsText;

                //MessageBox.Show(m);
            }


        }

        private void TestDocumentIn()
        {
            DocumentInProxy.DocumentInWcfServiceClient documentsInService = new DocumentInProxy.DocumentInWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)documentsInService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                var m = documentsInService.GetDocumentDataByExternalId("TRR", 1, ref response);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            FileInfo fileInfo = UploadFile();
            //LoginToCloud();

            LoginToCloudWithCredentails("80ef4241-a819-4a41-ae4e-76bc4c72bf93", 14);
            if (fileInfo.FileData != null)
            {
                //XmlSerializer xsSubmit = new XmlSerializer(typeof(ShipmentPM));
                //ShipmentPM pm = xsSubmit.Deserialize(new MemoryStream(fileInfo.FileData)) as ShipmentPM;

                XmlSerializer xsSubmit = new XmlSerializer(typeof(TraceEventsList));
                TraceEventsList eventsList = xsSubmit.Deserialize(new MemoryStream(fileInfo.FileData)) as TraceEventsList;

                //ShipmentCloudProxy.ShipmentWcfServiceClient shipmentService = new ShipmentCloudProxy.ShipmentWcfServiceClient();

                ShipmentProxy.ShipmentWcfServiceClient shipmentService = new ShipmentProxy.ShipmentWcfServiceClient();

                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentService.InnerChannel))
                {

                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    //var eventResponse2 = shipmentService.BuildEventsList(14, "4160209336", eventsList.Events.ToArray());

                  //  pm.MainCarriageATA = DateTime.Now;
                   // ShipmentProxy.Response myresp = shipmentService.Upsert(pm, false);

                }
            }

        }

        private void btnTestCloud_Click(object sender, EventArgs e)
        {
            //LoginToCloud();


            //CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerWcfServiceClient();

            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
            //{

            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    Response resultResponse = new Response();
            //    CustomerPM pm = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "10107933" }, 8, ref resultResponse);

            //    //var rres = customerservice.Upsert(customer111, false);
            //    //  var customer = customerservice.GetCustomerPM(new CustomerApiFilters() { ByVatNumber = true, SearchCode = "1234562322222" }, 1);
            //    //  var customer2 = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "470690c4-8f9a-4-121212" }, 1);
            //    // var contacts = customerservice.GetCustomerContacts(new CustomerApiFilters() { ByCode = true, SearchCode = "70002-1212" }, 1);
            //    //  var addresses = customerservice.GetCustomerAddresses(new CustomerApiFilters() { ByCode = true, SearchCode = "70002-1212" }, 1);

            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login();

            while (true)
            {
                CustomerProxy.CustomerWcfServiceClient customerservice = new CustomerProxy.CustomerWcfServiceClient();
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerservice.InnerChannel))
                {

                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    Response response = new Response();
                    var result = customerservice.GetReadyForActivationCustomer(1, ref response);
                    if (result != null)
                    {
                        customerservice.RemoveFromCustomersQueue(result.QueueMessageLockToken, result.Tenant);
                    }

                    if (response.HasError)
                    {

                    }
                  
                }
            }
        }

        private void btnMultiTaskQueueTest_Click(object sender, EventArgs e)
        {
            LoginToAmitalIIGTest();
           // Login();
        
            Thread fastWorkerThread = new Thread(GetExternalTasksFromFastQueue);
            fastWorkerThread.Start();

           // Thread slowWorkerThread = new Thread(GetExternalTasksFromSlowQueue);
           // slowWorkerThread.Start();

            
        }

        private void GetExternalTasksFromFastQueue()
        {
            while (true)
            {
                GetExternalTasksFromQueue(1);
            }
        }

        private void GetExternalTasksFromSlowQueue()
        {
            while (true)
            {
                GetExternalTasksFromQueue(2);
            }
        }
        private static void GetExternalTasksFromQueue(object priority,int tenant = 1)
        {
            ExternalTasksQueueProxy.ExternalTasksQueueWcfServiceClient externaltasksservice = new ExternalTasksQueueProxy.ExternalTasksQueueWcfServiceClient();
            //ExternalTasksQueueProxy.ExternalTasksQueueWcfServiceClient externaltasksservice = new ExternalTasksQueueProxy.ExternalTasksQueueWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)externaltasksservice.InnerChannel))
            {
                int pr = 1;
                if (priority != null)
                    pr = (int)priority;

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                //ExternalTasksQueueProxy.Response response = new ExternalTasksQueueProxy.Response();
                var result = externaltasksservice.GetTaskFromQueue(tenant, pr);
                if (result != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
                    using (StringReader reader = new StringReader(result))
                    {
                        //Envelope envelope = (Envelope)(serializer.Deserialize(reader));

                        ////string LockToken = response.Result;
                        //QueueTask task = envelope.Tasks.Where(a => a.Action.ToLower() == "documentsfiling.uploadbinarydata").FirstOrDefault();
                        //if (task != null)
                        //{

                        //}

                        //QueueTask task2 = envelope.Tasks.Where(a => a.Action.ToLower() == "documentsfiling.upsert").FirstOrDefault();
                        //if (task2 != null)
                        //{

                        //}

                        //var respo = externaltasksservice.MarkTaskAsDone(envelope.CommunicationLogId, 1, pr);
                    }
                }
                //if (response.HasError)
                //{

                //}

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process p = Process.GetCurrentProcess();
            ProcessThreadCollection coll = Process.GetCurrentProcess().Threads;
            ProcessThread myThread = (from ProcessThread entry in Process.GetCurrentProcess().Threads
                                      where entry.Id == Thread.CurrentThread.ManagedThreadId
                                      select entry).FirstOrDefault();

        }

        private void btnCustomerWD_Click(object sender, EventArgs e)
        {
            ////LoginToCloudWithCredentails("92467eed-d734-44d5-b73e-b9282cebac3d", 341);
            //LoginToCloud();

            ////BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            ////binding.MaxBufferSize = 2147483647;
            ////binding.MaxReceivedMessageSize = 2147483647;
            ////binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            ////binding.ReaderQuotas.MaxArrayLength = 2147483647;
            ////(binding, customerDWWcfServiceA.Endpoint.Address);
            //List<CustomerWDProxy.CustomerDW> customersList = new List<CustomerWDProxy.CustomerDW>();
            //CustomerWDProxy.CustomerDWWcfServiceClient customerWDService = new CustomerWDProxy.CustomerDWWcfServiceClient();

            //CustomerWDProxy.Response response = new CustomerWDProxy.Response();
            //int take = 20;
            //int skip = 0;
            //int i = 0;
            //using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)customerWDService.InnerChannel))
            //{
            //    customerWDService.Endpoint.Binding.SendTimeout = new TimeSpan(0, 3, 0);
            //    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

            //    while (i <= 232 )
            //    {
            //        try
            //        {
            //            skip = take * i;
            //            var r = customerWDService.GetCustomersByUpdateDate(341, new DateTime(2010, 1, 1), skip, take, ref response);
            //            if (r != null && !response.HasError)
            //            {
            //                List<CustomerWDProxy.CustomerDW> result = r.ToList();
            //                if (result.Count > 0)
            //                {
            //                    customersList = customersList.Concat(result).ToList();
            //                    i++;
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //            else
            //                break;
            //        }
            //        catch (Exception ex)
            //        {
            //            break;
            //        }
                    
            //    }


            //    ExportToExcel(customersList);
                //List<CustomerWDProxy.CustomerDW> result2 = customerWDService.GetCustomersByUpdateDate(341, new DateTime(2010, 1, 1), 200, 100, ref response).ToList();
                //customersList = customersList.Concat(result2).ToList();

                //List<CustomerWDProxy.CustomerDW> result3 = customerWDService.GetCustomersByUpdateDate(341, new DateTime(2010, 1, 1), 300, 100, ref response).ToList();
                //customersList = customersList.Concat(result3).ToList();

           // }
        }


        //public void ExportToExcel(List<CustomerWDProxy.CustomerDW> customersList)
        //{
        //    //// Load Excel application
        //    //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

        //    //// Create empty workbook
        //    //excel.Workbooks.Add();

        //    //// Create Worksheet from active sheet
        //    //Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;

        //    //// I created Application and Worksheet objects before try/catch,
        //    //// so that i can close them in finnaly block.
        //    //// It's IMPORTANT to release these COM objects!!
        //    //try
        //    //{
        //    //    // ------------------------------------------------
        //    //    // Creation of header cells
        //    //    // ------------------------------------------------
        //    //    workSheet.Cells[1, "A"] = "Name";
        //    //    workSheet.Cells[1, "B"] = "Code";
        //    //    workSheet.Cells[1, "C"] = "IsCustomer";

        //    //    // ------------------------------------------------
        //    //    // Populate sheet with some real data from "cars" list
        //    //    // ------------------------------------------------
        //    //    int row = 2; // start row (in row 1 are header cells)
        //    //    foreach (CustomerWDProxy.CustomerDW customer in customersList)
        //    //    {
        //    //        workSheet.Cells[row, "A"] = customer.Name;
        //    //        workSheet.Cells[row, "B"] = customer.Code;
        //    //        workSheet.Cells[row, "C"] = customer.IsCustomer;

        //    //        row++;
        //    //    }

        //    //    // Apply some predefined styles for data to look nicely :)
        //    //    workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

        //    //    // Define filename
        //    //    string fileName = string.Format(@"{0}\CustomersDWExcelData.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

        //    //    // Save this data as a file
        //    //    workSheet.SaveAs(fileName);

        //    //    // Display SUCCESS message
        //    //    MessageBox.Show(string.Format("The file '{0}' is saved successfully!", fileName));
        //    //}
        //    //catch (Exception exception)
        //    //{
        //    //    MessageBox.Show("Exception",
        //    //    "There was a PROBLEM saving Excel file!\n" + exception.Message,
        //    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //}
        //    //finally
        //    //{
        //    //    // Quit Excel application
        //    //    excel.Quit();

        //    //    // Release COM objects (very important!)
        //    //    if (excel != null)
        //    //        System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

        //    //    if (workSheet != null)
        //    //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

        //    //    // Empty variables
        //    //    excel = null;
        //    //    workSheet = null;

        //    //    // Force garbage collector cleaning
        //    //    GC.Collect();
        //    //}
        //}

      


        //private string LoginToCloud()
        //{
        //    LogingProxyCloud.LoginWcfServiceClient loginService = new LogingProxyCloud.LoginWcfServiceClient();
        //    LogingProxyCloud.Response loginResponse = loginService.Login("yaronc@amital.co.il", "!Y123456");//("zaki@amital.co.il", "!Zz123456");
        //    if (!loginResponse.HasError)
        //    {
        //        Token = loginResponse.Result;
        //    }

        //    return Token;
        //}

        private void Button5_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = UploadFile();
            LoginToTest();

            Login();
            if (fileInfo.FileData != null)
            {
                //XmlSerializer xsSubmit = new XmlSerializer(typeof(ShipmentPM));
                //ShipmentPM pm = xsSubmit.Deserialize(new MemoryStream(fileInfo.FileData)) as ShipmentPM;

                XmlSerializer xsSubmit = new XmlSerializer(typeof(TraceEventsList));
                TraceEventsList eventsList = xsSubmit.Deserialize(new MemoryStream(fileInfo.FileData)) as TraceEventsList;

                //ShipmentCloudProxy.ShipmentWcfServiceClient shipmentService = new ShipmentCloudProxy.ShipmentWcfServiceClient();

                ShipmentProxy.ShipmentWcfServiceClient shipmentService = new ShipmentProxy.ShipmentWcfServiceClient();

                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentService.InnerChannel))
                {

                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                    var eventResponse2 = shipmentService.BuildEventsList(1062, "91340107", eventsList.Events.ToArray());

                    //  pm.MainCarriageATA = DateTime.Now;
                    // ShipmentProxy.Response myresp = shipmentService.Upsert(pm, false);

                }
            }
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            Login();
            var response = new Response();
            var partnersTester = new PartnersTester();
             
            switch (cmdServices.SelectedItem)
            {
                case "Warehouse":
                    response = partnersTester.TestWarehouseService(Token);
                    break;
                case "Customer":
                    response = SetCustomerTests(Token);
                    break;
                case "FreightForwarder":
                    response = TestFreightForwarderService();
                    break;
                case "Vendor":
                    response = partnersTester.TestVendorService(Token);
                    break;
                case "Agent":
                    response = partnersTester.TestAgentService(Token);
                    break;
                case "Airline":
                    response = partnersTester.TestAirlineService(Token);
                    break;
                case "ShippingAgent":
                    response = partnersTester.TestShippingAgentService(Token);
                    break;
                case "Trucker":
                    response = partnersTester.TestTruckerService(Token);
                    break;
                case "Vessel":
                    response = partnersTester.TestVesselService(Token);
                    break;
                case "ShippingLine":
                    response = partnersTester.TestShippingLineService(Token);
                    break;
                case "Shipment Pickups & Deliveries":
                    response = TestShipmentPickupsDeliveriesService();
                    break;
                case "ShipmentWarehouseLeg":
                    response = TestShipmentWarehouseLegService();
                    break;
                case "ShipmentTrucker":
                    response = TestShipmentTruckerService();
                    break;
                case "Shipment CustomAgentImportId":
                    response = TestShipmentCustomsAgentService();
                    break;
                case "Shipment PaymentRequestDateTime":
                    response = TestShipmentPaymentRequestDateTimee();
                    break;
                case "AccountingPartner":
                    response = SetAccountingPartnerTests(Token);
                    break;
                case "Hybrid Tenant State":
                    response = TestHybridTenantStateService();
                    break;
                case "Address":
                    response =  TestAddressService();
                    break;
                default:
                    MessageBox.Show("select a service to test");
                    break;
            }

            if(response != null && response.HasError)
            {
                var errorMessage = response.ErrorMessage + (response.InnerErrorMessage ?? "");
                MessageBox.Show(errorMessage);
            }

            MessageBox.Show("Success " + response?.Result);

        }

        private Response TestFreightForwarderService()
        {
 
            ShipmentProxy.ShipmentPM shipmentPM = new ShipmentPM()
            {
                ShipmentNumber = "1083",
                DirectionId = "R",
                ShipmentLevelCode = "D",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",
                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",
                ShipmentTypeId = null,
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                //Master = "12345678",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
                TruckerId = "TEP",
                AssignedToTruckerDate = DateTime.Today,
                FreightForwarderId = "BV",
            };

            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(shipmentPM, false);

                if (!response.HasError)
                { //update warehouseleg values
                    shipmentPM.WarehouseLegWarehouseId = "sss";
                    shipmentPM.WarehouseLegActualEntryDate = DateTime.Today;
                    response = shipmentservice.Upsert(shipmentPM, false);
                }

                return response;
            }

 
        }

        private Response TestAddressService()
        {
            AddressProxy.AddressWcfServiceClient addressService = new AddressProxy.AddressWcfServiceClient();
            AddressProxy.AddressPM address = new AddressProxy.AddressPM()
            {
                ExternalId = "EXT1234",
                Tenant = 1,
                Address1 = "Al Beireh",
                Address2 = "Jawwal",
                AddressTypeId = "M", // M: main address, B: billing // O:Other
                ATTN = "11111",
                CardId = "70132",
                City = "Ramallah",
                CountryId = "PS",
                Name = "Main Address",
                ZipCode = "0972",
                Description = "ramallah address updated",
            };
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)addressService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = addressService.Upsert(address, false);

                return response;
            }

        }

        AccountingPartnerProxy.AccountingPartnerWcfServiceClient accountingPartnerservice = new AccountingPartnerWcfServiceClient();
        private Response SetAccountingPartnerTests(string token)
        {
            Response response = new Response();
            ActionNames.SelectedItem = "";

            switch (ActionNames.SelectedItem)
            {
                case "upsert":
                    response = TestAccountingPartnerService(token);
                    break;
                case "getAccountingPartnerPM":
                    response = TestGetAccountingPartnerPMService(token);
                    break; 
                default:
                    MessageBox.Show("select an action to test");
                    break;
            }
            return response;
        }


        public Response TestGetAccountingPartnerPMService(string token)
        {


            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)accountingPartnerservice.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
               AccountingPartnerProxy.AccountingPartnerPM accountingPartnerPM = accountingPartnerservice.GetAccountingPartnerPM(new AccountingPartnerApiFilters() { ByCode = true, SearchCode = "HEHYBRID6" }, 1, ref resultResponse);

              if(accountingPartnerPM != null)
                {
                    MessageBox.Show("Success: AccountingPartnerId = " + accountingPartnerPM.Id);

                }

                return resultResponse;
            }
        }


        private Response SetCustomerTests(string token)
        {
            Response response = new Response();
            ActionNames.SelectedItem = "";

            switch (ActionNames.SelectedItem)
            {
                case "upsert":
                    response = TestCustomerService(token);
                    break;
                case "getCustomerPM":
                    response = TestGetCustomerPMService(token);
                    break;
                case "getCustomerListById":
                    response = TestGetCustomerListByIdService(token);
                    break;
                case "getCustomerListByEmail":
                    response = TestGetCustomerListByEmailService(token);
                    break;
                default:
                    MessageBox.Show("select an action to test");
                    break;
            }
            return response;
        }

        private Response TestShipmentPickupsDeliveriesService()
        {
            ShipmentProxy.ShipmentPM consolepm = new ShipmentPM()
            {
                ShipmentNumber = "SHIP_7777",
                DirectionId = "R",
                ShipmentLevelCode = "A",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",

                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                // House = "4545",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",


                ShipmentTypeId = null,//"LCL",
                //StatusId = "SHOR",
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                Master = "123456789",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                // ShipperReference2 
                // IsCancelled = true,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
            };

            consolepm.ShipmentPickUps = new ShipmentPickUpPM[]
            {
                new ShipmentPickUpPM()
                {
                    CarrierId = "TK",
                    FromAddressCity = "London",
                    FromAddressCountryId = "GB",

                    ToAddressCountryId = "FR",
                    ToAddressCity = "Paris",

                    TruckNumber = "1111",
                    Driver = "My driver",

                    //ETA = DateTime.Now.AddDays(-2),
                    //ETD = DateTime.Now.AddDays(-1),
                    ATA = DateTime.Now,
                },
            };

           
           // consolepm.ShipmentDeliveries = new ShipmentDeliveryPM[]
           //{
           //      new ShipmentDeliveryPM()
           //     {
           //         CarrierId = "MTR",

           //         FromAddressCity = "Marseille",
           //         FromAddressCountryId = "FR",

           //         ToAddressCountryId = "GB",
           //         ToAddressCity = "Bristol",

           //         TruckNumber = "1111",
           //         Driver = "Baker driver",

           //         ETA = DateTime.Now.AddDays(-2),
           //         ETD = DateTime.Now.AddDays(-1),
           //         ATA = DateTime.Now,
           //     },
           //};


            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(consolepm, false);

                return response;
            }


        }

        private Response TestShipmentWarehouseLegService()
        {
            ShipmentProxy.ShipmentPM consolepm = new ShipmentPM()
            {
                ShipmentNumber = "SHIP_718756",
                DirectionId = "R",
                ShipmentLevelCode = "D",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",

                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                // House = "4545",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",


                ShipmentTypeId = null,//"LCL",
                //StatusId = "SHOR",
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                //Master = "12345678",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                // ShipperReference2 
                // IsCancelled = true,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
            };


            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(consolepm, false);

                if (!response.HasError)
                { //update warehouseleg values
                    consolepm.WarehouseLegWarehouseId = "sss";
                    consolepm.WarehouseLegActualEntryDate = DateTime.Today;
                    response = shipmentservice.Upsert(consolepm, false);
                }

                return response;
            }
        }

        private Response TestShipmentTruckerService()
        {
            ShipmentProxy.ShipmentPM consolepm = new ShipmentPM()
            {
                ShipmentNumber = "1083",
                DirectionId = "R",
                ShipmentLevelCode = "D",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",

                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                // House = "4545",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",


                ShipmentTypeId = null,//"LCL",
                //StatusId = "SHOR",
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                //Master = "12345678",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                // ShipperReference2 
                // IsCancelled = true,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
                TruckerId = "TEP",
                AssignedToTruckerDate = DateTime.Today,

            };


            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(consolepm, false);
                
                if (!response.HasError)
                { //update warehouseleg values
                    consolepm.WarehouseLegWarehouseId = "sss";
                    consolepm.WarehouseLegActualEntryDate = DateTime.Today;
                    response = shipmentservice.Upsert(consolepm, false);
                }

                return response;
            }
        }
         
         private Response TestHybridTenantStateService()
        {
            HybridTenantStateProxy.HybridTenantStatePM consolepm = new HybridTenantStatePM()
            { 
                Tenant = 2,
                WaitingQueue = 0,
                FailedQueue = 0,
                LastQueueDateTime = DateTime.Today,
                LastUpdateDateTime = DateTime.Today, 
                VersionDate = DateTime.Today,
                VersionNumber = "10.10"
            };


            HybridTenantStateProxy.HybridTenantStateWcfServiceClient hybridTenantStateServiceservice = new HybridTenantStateProxy.HybridTenantStateWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)hybridTenantStateServiceservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = hybridTenantStateServiceservice.Upsert(consolepm, false); 
                return response;
            }
        }

        private Response TestShipmentCustomsAgentService()
        {
            ShipmentProxy.ShipmentPM consolepm = new ShipmentPM()
            {
                ShipmentNumber = "13666",
                DirectionId = "R",
                ShipmentLevelCode = "D",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",

                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                // House = "4545",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",


                ShipmentTypeId = null,//"LCL",
                //StatusId = "SHOR",
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                //Master = "12345678",
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                //ShipperReference1 = "saaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasaaaaaaaasdasdasdasaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                // ShipperReference2 
                // IsCancelled = true,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
                TruckerId = "TEP",
                AssignedToTruckerDate = DateTime.Today,
                CustomAgentImportId = "sss",
                AssginedToCustomsAgentDate = DateTime.Today,

            };


            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(consolepm, false);

                if (!response.HasError)
                { //update warehouseleg values
                    consolepm.WarehouseLegWarehouseId = "sss";
                    consolepm.WarehouseLegActualEntryDate = DateTime.Today;
                    response = shipmentservice.Upsert(consolepm, false);
                }

                return response;
            }
        } 


        private Response TestShipmentPaymentRequestDateTimee()
        {
            ShipmentPM shipmentPM = GetShipmentPMInstance();

            ShipmentProxy.ShipmentWcfServiceClient shipmentservice = new ShipmentProxy.ShipmentWcfServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                Response response = new Response();
                response = shipmentservice.Upsert(shipmentPM, false);
                if (!response.HasError)
                { 
                    response = shipmentservice.Upsert(shipmentPM, false);
                }
                return response;
            }
        }

        private static ShipmentPM GetShipmentPMInstance()
        {
            return new ShipmentPM()
            {
                ShipmentNumber = "1083",
                DirectionId = "R",
                ShipmentLevelCode = "D",
                Tenant = 1,
                AccessDate = DateTime.Now,
                AWBCurrencyId = "USD",
                BranchId = "HybridB1",
                DepartmentId = "HybridD1",
                ChargeableWeightUnitCode = "KG",
                ConsigneeId = "70000",
                ShipperId = "70000",
                ConsigneeReference1 = "PO35104",
                CreateDateTime = DateTime.Now,
                CreatedByUserId = "HybridU1",
                CutoffDate = DateTime.Now,
                DimensionsUnitCode = "CM",
                FinalDistenationPortId = "TLV",
                FreightPrepaidCollectId = "C",
                FromPortId = "JFK",
                GrossWeightUnitCode = "KG",
                IncotermId = "CIF",
                MainCarriageCarrierId = "LY",
                MainCarriageFinalDestinationPortId = "TLV",
                MainCarriageFromPortId = "FRD",
                MainCarriageToPortId = "TLV",
                OtherPrepaidCollectId = "C",
                ProfitCurrencyId = "NIS",
                ShipmentCustomerTypeCode = "SHI",
                ShipmentTypeId = null,
                ToPortId = "ILTLV",
                TransportModeId = "A",
                VolumeUnitCode = "CBM",
                ChargeableWeight = 0.9999984133,
                GrossWeight = 0.9999984133,
                MainCarriageATA = DateTime.Now,
                MainCarriageETA = DateTime.Now,
                IsCancelled = false,
                Notes = "testing console via hybrid 123",
                IsHybrid = true,
                AccountManagerUserId = "HybridU1",
                QuoteNumber = "1000",
                TruckerId = "TEP",
                AssignedToTruckerDate = DateTime.Today,
                CustomAgentImportId = "sss",
                AssginedToCustomsAgentDate = DateTime.Today,
                PaymentRequestDateTime = DateTime.Today,
            };
        }

        private void btnPaymentTerms_Click(object sender, EventArgs e)
        {
            Login();

            PaymentTermProxy.PaymentTermWcfServiceClient serviceReference = new PaymentTermProxy.PaymentTermWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)serviceReference.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);

                var response = new Response();
                var result = serviceReference.GetPaymentTerms(ref response, 1);
                switch (this.cmdServices.SelectedItem)
                {
                    case "Customer":
                        TestCustomerPaymentTerms(response, result); 
                        break;
                    case "AccountingPartner":
                        TestAccountingPartnerPaymentTerms(response, result);
                        break;
                    default:
                        MessageBox.Show("select a service to test"); 
                        break;
                }


             
            }

        }

        private void TestAccountingPartnerPaymentTerms(Response response, PaymentTermList[] result)
        {
            if (!response.HasError && result != null && result.Length > 0)
            {
                var paymentTermId = result[0].Id;
                var accountingPartnerResponse = TestAccountingPartnerService(Token, paymentTermId);
                if (accountingPartnerResponse.HasError)
                    MessageBox.Show("Failed: " + accountingPartnerResponse.ErrorMessage);
                else
                    MessageBox.Show("Success: AccountingPartnerId = " + accountingPartnerResponse.Result + ", PaymentTermId: " + paymentTermId);
            }
            else
            {
                MessageBox.Show("Failed: " + response.ErrorMessage);
            }
             
        }

        private void TestCustomerPaymentTerms(Response response, PaymentTermList[] result)
        {
            if (!response.HasError && result != null && result.Length > 0)
            {
                var paymentTermId = result[0].Id;
                var customerResponse = TestCustomerService(Token, paymentTermId);
                if (customerResponse.HasError)
                    MessageBox.Show("Failed: " + customerResponse.ErrorMessage);
                else
                    MessageBox.Show("Success: CustomerId = " + customerResponse.Result + ", PaymentTermId: " + paymentTermId);
            }
            else
            {
                MessageBox.Show("Failed: " + response.ErrorMessage);
            }

        }
        private void cmdServices_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.ActionNames.SelectedItem = "";
            this.ActionNames.Items.Clear();
            ActionNames.Text = "";
            switch (this.cmdServices.SelectedItem)
            {
                case "Customer":
                
                this.ActionNames.Visible = true;
                this.ActionNames.Items.AddRange(new object[] {
                "upsert",
                "getCustomerPM",
                "getCustomerListById",
                "getCustomerListByEmail",
                });
                    break;
                case "AccountingPartner":
                 this.ActionNames.Visible = true;
                 this.ActionNames.Items.AddRange(new object[] {
                "upsert",
                "getAccountingPartnerPM",
                });
                    break;
                default:
                this.ActionNames.Visible = false;
                this.ActionNames.SelectedItem = "";
                this.ActionNames.Items.Clear();
                    break;
            }
             
        }

        private void ActionNames_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //ExternalTasksQueueIIGTestProxy
    }



    public class TraceEventsList
    {
        public List<ShipmentProxy.TraceEventPM> Events { get; set; }
    }




    public class Envelope
    {

        [XmlAttribute("CommunicationLogId")]
        public string CommunicationLogId { get; set; }

        [XmlAttribute("HasError")]
        public bool HasError { get; set; }
        [XmlAttribute("IsAuthenticationError")]
        public bool IsAuthenticationError { get; set; }

        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }

        public List<QueueTask> Tasks { get; set; }


    }

    public class QueueTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }

        public List<Parameter> Parameters { get; set; }

    }


    public class Parameter
    {
        [XmlAttribute("order")]
        public int Order { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}