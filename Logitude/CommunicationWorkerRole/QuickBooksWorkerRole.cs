using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Security;
using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;

namespace CommunicationWorkerRole
{
   public class QuickBooksWorkerRole : WorkerEntryPoint
    {

       QueueDescription queueDescription;
       QueueClient client;
     
       const int maxSizeToLoad = 500;
       public override void Run()
       {

           while (IsRunning)
           {
               if (!General.IsUpdating())
               {
                   try
                   {
                       int tenant = 0;
                       var message = client.Receive(new TimeSpan(0, 0, 30));
                       LastActivity = DateTime.UtcNow;
                       if (message != null)
                       {
                           try
                           {

                               string messageType = message.Properties["type"].ToString();
                                int.TryParse(message.Properties["Tenant"].ToString(), out tenant);

                                  if (messageType == "gettables")
                                {


                                    GetTables(tenant, "gettables");
                                }
                                else if (messageType == "starttransfer")
                                {
                                    StartTransfer(tenant, "starttransfer");
 
                                }
                               message.Complete();
                               LogDoneItemInMemory();

                           }
                           catch (Exception ex)
                           {
                               ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "QuickBooks worker role", "", null);
                          

                               // Thread.Sleep(new TimeSpan(0, 1, 0));
                           }
                           
                       }
                   }
                   catch (Exception ex)
                   {
                       ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "QuickBooks worker role start", null, null);
                       Thread.Sleep(10000);
                   }

               }
               else
               {
                   Thread.Sleep(60000);
               }
           }
       }

       public void StartTransfer(int tenant , string messageType)
       {
           string status = "succeeded";
           string accessToken = "qyprdTrX4ctgHwHaoejWy8u6VVvGpxkzIK5lbuMoAU03sTZM";
           string accessTokenSecret = "9MKgM4X9zCkiHKu7LBFAnDoK6UuoQgon7BPmKBY5";
           string consumerKey = "qyprd4Yz3bsRFpUsDPXbpF68hGzeRW";
           string consumerSecret = "BURunV1MvWvy170dq0awYwAjtb3ASKwP6wQezMfP";

           OAuthRequestValidator oauthValidator = new OAuthRequestValidator(
           accessToken, accessTokenSecret, consumerKey, consumerSecret);

           string appToken = "d16d4d95b6364b4975b8e00b64d7d8ef339b";
           string companyID = "312501836";
           ServiceContext context = new ServiceContext(appToken, companyID, IntuitServicesType.QBO, oauthValidator);
           DataService service = new DataService(context);

           ARInvoiceRepository invoicerepository = new ARInvoiceRepository(tenant);
           ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
   
           List<string> readyinvoicesIDs = invoicerepository.GetReadyForTransferORerrorInTransferARInvoices(tenant);
           int invoicesCount = readyinvoicesIDs.Count();
           int counter = 0;
           ExternalSystemsSyncStatusRepository ExternalSystemsRep = new ExternalSystemsSyncStatusRepository(tenant);
           ExternalSystemsSyncStatus externalSystemsStatus = ExternalSystemsRep.GetSingleExternalSystemsSyncStatusBySubject(messageType, tenant);
           if (externalSystemsStatus == null)
           {
               externalSystemsStatus = new ExternalSystemsSyncStatus()
               {
                   Id = IdCounter.GetNumber("ExternalSystemsSyncStatus", tenant),
                   Subject = "starttransfer",
                   StatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   Tenant = tenant,

               };

               ExternalSystemsRep.Add(externalSystemsStatus);
               ExternalSystemsRep.SubmitChanges();
           }

           foreach (string id in readyinvoicesIDs)
           {
               ++counter;

               externalSystemsStatus.ProgressDetails = "Transferring " + counter + "/" + invoicesCount;
               externalSystemsStatus.Status = "Transferring";
               ExternalSystemsRep.Update(externalSystemsStatus);
               ExternalSystemsRep.SubmitChanges();

              



               ARInvoicePM invoicePM = invoiceQuery.GetSinglePM(id, tenant);


               Intuit.Ipp.Data.Invoice invoice = new Intuit.Ipp.Data.Invoice()
               {
                   DocNumber = invoicePM.InvoiceNumber,
                   TxnDate =  invoicePM.InvoiceDate.Value,
                   CurrencyRef = new Intuit.Ipp.Data.ReferenceType() { name = "United States Dollar", Value= invoicePM.AccountingExternalCode },
                  
                   CustomerRef = new Intuit.Ipp.Data.ReferenceType() {  Value = invoicePM.DebitAccount },
                   SalesTermRef = new Intuit.Ipp.Data.ReferenceType() { Value = "3" },
                   //DueDate = DateTime.Now.AddDays(30),
                   TotalAmt = 200,
                   TotalAmtSpecified = true,
                   ApplyTaxAfterDiscount = false,
                   Balance = (decimal)invoicePM.AmountDue.Value,
                   BalanceSpecified = true,
                   Deposit = 0,
                   MetaData = new ModificationMetaData() { CreateTime = DateTime.Now, LastUpdatedTime = DateTime.Now, },

               };

               foreach (ARInvoiceLinePM aRInvoiceline in invoicePM.InvoiceLines)
               {
                   Intuit.Ipp.Data.Line line = new Intuit.Ipp.Data.Line()
                 
                    {
                       Amount = (decimal) 200,
                        AmountSpecified=true,
                      
                        DetailType = Intuit.Ipp.Data.LineDetailTypeEnum.SalesItemLineDetail,
                        
                        DetailTypeSpecified = true, 
                        AnyIntuitObject = new Intuit.Ipp.Data.SalesItemLineDetail()
                        { 
                            Qty =(decimal) aRInvoiceline.Quantity,
                            QtySpecified = true,
                            TaxCodeRef = new Intuit.Ipp.Data.ReferenceType() { Value =aRInvoiceline.ExternalVATCard  },
                            ItemRef = new Intuit.Ipp.Data.ReferenceType() { Value=aRInvoiceline.CreditAccount},
                            AnyIntuitObject=(decimal) aRInvoiceline.UnitPrice,
                            ItemElementName=ItemChoiceType.UnitPrice
                        },
                         
                  
                   
                };
               }

               try
               {
                   Invoice resultInv = service.Add(invoice) as Invoice;
               }
               catch (Exception ex)
               {
                   invoicePM.TransferError = ex.Message;
                   externalSystemsStatus.Status = "Error In Transfer";
                   ExternalSystemsRep.Update(externalSystemsStatus);
                   ExternalSystemsRep.SubmitChanges();
               }

               externalSystemsStatus.Status = "Transferred";
               invoicePM.TransferError = null;
               ExternalSystemsRep.Update(externalSystemsStatus);
               ExternalSystemsRep.SubmitChanges();
           }

           if (counter == invoicesCount)
           {
               externalSystemsStatus.Status = "Done";
               externalSystemsStatus.ProgressDetails = "Done";
               ExternalSystemsRep.Update(externalSystemsStatus);
               ExternalSystemsRep.SubmitChanges();
           }
      



       }


       public void GetTables(int tenant, string messageType)
       {
          
           string accessToken = "qyprdTrX4ctgHwHaoejWy8u6VVvGpxkzIK5lbuMoAU03sTZM";
           string accessTokenSecret = "9MKgM4X9zCkiHKu7LBFAnDoK6UuoQgon7BPmKBY5";
           string consumerKey = "qyprd4Yz3bsRFpUsDPXbpF68hGzeRW";
           string consumerSecret = "BURunV1MvWvy170dq0awYwAjtb3ASKwP6wQezMfP";

           OAuthRequestValidator oauthValidator = new OAuthRequestValidator(
           accessToken, accessTokenSecret, consumerKey, consumerSecret);

           string appToken = "d16d4d95b6364b4975b8e00b64d7d8ef339b";
           string companyID = "312501836";
           ServiceContext context = new ServiceContext(appToken, companyID, IntuitServicesType.QBO, oauthValidator);

           DataService service = new DataService(context);
           ExternalSystemsTablesCodeRepository externalSystemsRep = new ExternalSystemsTablesCodeRepository(tenant);

    
           ExternalSystemsSyncStatusRepository ExternalSystemsRep = new ExternalSystemsSyncStatusRepository(tenant);
           ExternalSystemsSyncStatus externalSystemsStatus = ExternalSystemsRep.GetSingleExternalSystemsSyncStatusBySubject(messageType, tenant);
      
           if (externalSystemsStatus == null)
           {
               externalSystemsStatus = new ExternalSystemsSyncStatus()
               {
                   Id = IdCounter.GetNumber("ExternalSystemsSyncStatus", tenant),
                   Subject = "gettables",
                   StatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   Tenant = tenant,

               };
               ExternalSystemsRep.Add(externalSystemsStatus);
               ExternalSystemsRep.SubmitChanges();      

           }                                                       
        

           Term payemntTerm = new Term();
           List<Term> paymentTerms = GetTableData<Term>(payemntTerm, 1, 500, service);

           externalSystemsStatus.Status = "working";
           externalSystemsStatus.ProgressDetails = "Getting Terms";
           ExternalSystemsRep.Update(externalSystemsStatus);
           ExternalSystemsRep.SubmitChanges();

           foreach (Term item in paymentTerms)
           {
               ExternalSystemsTablesCode external = new ExternalSystemsTablesCode()
               {
                   Name = item.Name,
                   Code = item.Id,
                   CreatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant),
                   LogitudeTable = "PaymentTerms",
                   Tenant = tenant,
                   UpdatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   SearchFields = item.Name + "," + item.Id + "," + "PaymentTerms",
               };
               externalSystemsRep.Add(external);
           }

           externalSystemsRep.SubmitChanges();


           Customer customer = new Customer();
           List<Customer> customers = GetTableData<Customer>(customer, 1, 500, service);

           externalSystemsStatus.Status = "working";
           externalSystemsStatus.ProgressDetails = "Getting Customers";
           ExternalSystemsRep.Update(externalSystemsStatus);
           ExternalSystemsRep.SubmitChanges();

           foreach (Customer item in customers)
           {
               ExternalSystemsTablesCode external = new ExternalSystemsTablesCode()
               {
                   Name = item.DisplayName,
                   Code = item.Id,
                   CreatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant),
                   LogitudeTable = "Customers",
                   Tenant = tenant,
                   UpdatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   SearchFields = item.DisplayName + "," + item.Id + "," + "Customers",
               };
               externalSystemsRep.Add(external);
           }

           externalSystemsRep.SubmitChanges();

           TaxCode vatType = new TaxCode();
           List<TaxCode> vatTypes = GetTableData<TaxCode>(vatType, 1, 500, service);

           externalSystemsStatus.Status = "working";
           externalSystemsStatus.ProgressDetails = "Getting TaxCodes";
           ExternalSystemsRep.Update(externalSystemsStatus);
           ExternalSystemsRep.SubmitChanges();

           foreach (TaxCode item in vatTypes)
           {
               ExternalSystemsTablesCode external = new ExternalSystemsTablesCode()
               {
                   Name = item.Name,
                   Code = item.Id,
                   CreatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant),
                   LogitudeTable = "VatTypes",
                   Tenant = tenant,
                   UpdatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   SearchFields = item.Name + "," + item.Id + "," + "VatTypes",
               };
               externalSystemsRep.Add(external);

           }
           externalSystemsRep.SubmitChanges();

           Currency currency = new Currency();

           ExternalSystemsTablesCode externalTable = new ExternalSystemsTablesCode()
           {
               Name = "United States Dollars",
               Code = "USD",
               CreatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
               Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant),
               LogitudeTable = "Currencies",
               Tenant = tenant,
               UpdatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
               SearchFields = "United States Dollars" + "," + "USD" + "," + "Currencies",
           };
           externalSystemsRep.Add(externalTable);
           externalSystemsRep.SubmitChanges();

           //List<Currency> currencies = GetTableData<Currency>(currency, 1, 500, service); TBD
           Item ChareType = new Item();
           List<Item> chareTypes = GetTableData<Item>(ChareType, 1, 500, service);

           externalSystemsStatus.Status = "working";
           externalSystemsStatus.ProgressDetails = "Getting Items";
           ExternalSystemsRep.Update(externalSystemsStatus);
           ExternalSystemsRep.SubmitChanges();

           foreach (Item item in chareTypes)
           {
               ExternalSystemsTablesCode external = new ExternalSystemsTablesCode()
               {
                   Name = item.FullyQualifiedName,
                   Code = item.Id,
                   CreatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   Id = IdCounter.GetNumber("ExternalSystemsTablesCode", tenant),
                   LogitudeTable = "ChargesTypes",
                   Tenant = tenant,
                   UpdatedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                   SearchFields = item.FullyQualifiedName + "," + item.Id + "," + "ChargesTypes",
               };
               externalSystemsRep.Add(external);
           }

           externalSystemsRep.SubmitChanges();

           externalSystemsStatus.Status = "Done";
           externalSystemsStatus.ProgressDetails = "Done";
           ExternalSystemsRep.Update(externalSystemsStatus);
           ExternalSystemsRep.SubmitChanges();


       }
       public override bool OnStart()
       {

           ThreadId = Guid.NewGuid().ToString();
           BatchServiceCode = "QuickBooks";
           DoneItemsInRange = new Dictionary<DateTime, int>();

           try
           {
               string queueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("quickbooksqueue");


               if (!StorageAcountDetails.NameSpaceManager.QueueExists(queueName))
               {
                   queueDescription = new QueueDescription(queueName);
                   queueDescription.MaxSizeInMegabytes = 5120;
                 

                   StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
               }

               client = StorageAcountDetails.CreateServiceBusQueueClient(queueName);
           }
           catch (Exception ex)
           {
               ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "QuickBooks worker role start", null,null);
           }

           // Set the maximum number of concurrent connections 
           ServicePointManager.DefaultConnectionLimit = 12;

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

       public List<T> GetTableData<T>(T entity, int startIndex, int endIndex, DataService service) where T : IEntity
       {
           List<T> result = null;

           List<T> dataEntities = service.FindAll<T>(entity, startIndex, endIndex).ToList();
           result = dataEntities;
           int diff = endIndex - startIndex + 1;
           if (diff == maxSizeToLoad && dataEntities.Count==maxSizeToLoad)
           {
               int newStart = endIndex + 1;
               int newEnd = endIndex + maxSizeToLoad;
               dataEntities = GetTableData<T>(entity, newStart, newEnd, service);
               result.Concat(dataEntities);
           }
           return result;
       }

    }
}
