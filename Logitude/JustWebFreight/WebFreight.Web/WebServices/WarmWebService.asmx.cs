using System;
using System.Web.Services;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Collections.Generic;
using Logitude.SystemLogs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for WarmWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WarmWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void StartWarming()
        {
            long t1;
            long t2;
            string message = "";
            int tenant = 1;

            try
            {
                message = "Warming for common model started";
                t1 = DateTime.Now.Ticks;
                WarmingMethodsClass.WarmCommonModel(tenant);
                t2 = DateTime.Now.Ticks;
                message = message + Environment.NewLine + "Warming for common model completed in " + ((t2 - t1) / 10000000) + " Seconds";
                
            }
            catch (Exception ex)
            {
                string errorMessage = "An error happened while warming common model the message is: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "the inner exception is: " + ex.InnerException.Message;
                }
                AzureLog.SaveWarmingLogsInStorage(errorMessage, "E", 1);
            }

            try
            {
                message = message + Environment.NewLine + "Warming for Global model started";
                t1 = DateTime.Now.Ticks;
                WarmingMethodsClass.WarmGlobalModel(tenant);
                t2 = DateTime.Now.Ticks;
                message = message + Environment.NewLine + "Warming for Global model completed in " + ((t2 - t1) / 10000000) + " Seconds";
              
            }
            catch (Exception ex)
            {
                string errorMessage = "An error happened while warming Global model the message is: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "the inner exception is: " + ex.InnerException.Message;
                }
                AzureLog.SaveWarmingLogsInStorage(errorMessage, "E", 1);
            }

            try
            {

                message = message + Environment.NewLine + "Warming for Invoice model started";
                t1 = DateTime.Now.Ticks;
                WarmingMethodsClass.WarmInvoiceModel(tenant);
                t2 = DateTime.Now.Ticks;
                message = message + Environment.NewLine + "Warming for Invoice model completed in " + ((t2 - t1) / 10000000) + " Seconds";
               
            }
            catch (Exception ex)
            {
                string errorMessage = "An error happened while warming Invoice model the message is: " + ex.Message + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "the inner exception is: " + ex.InnerException.Message;
                }
                AzureLog.SaveWarmingLogsInStorage(errorMessage, "E", 1);
            }

            try
            {
                message = message + Environment.NewLine + "Warming for Quote model started";
                t1 = DateTime.Now.Ticks;
                WarmingMethodsClass.WarmQuoteModel(tenant);
                t2 = DateTime.Now.Ticks;
                message = message + Environment.NewLine + "Warming for Quote model completed in " + ((t2 - t1) / 10000000) + " Seconds";
                
            }
            catch (Exception ex)
            {
                string errorMessage = "An error happened while warming Quote model the message is: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "the inner exception is: " + ex.InnerException.Message;
                }
                AzureLog.SaveWarmingLogsInStorage(errorMessage, "E", 1);
            }

            try
            {
                message = message + Environment.NewLine + "Warming for shipment model started";
                t1 = DateTime.Now.Ticks;
                WarmingMethodsClass.WarmShipmentModel(tenant);
                t2 = DateTime.Now.Ticks;
                message = message + Environment.NewLine + "Warming for shipment model completed in " + ((t2 - t1) / 10000000) + " Seconds";
                
            }
            catch (Exception ex)
            {
                string errorMessage = "An error happened while warming shipment model the message is: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += Environment.NewLine + "the inner exception is: " + ex.InnerException.Message;
                }
                AzureLog.SaveWarmingLogsInStorage(errorMessage, "E", 1);
            }

            //try
            //{
            //    message = message + Environment.NewLine + "Warming for webFreight model started";
            //    t1 = DateTime.Now.Ticks;
            //    WarmingMethodsClass.WarmWebFreightModel(tenant);
            //    t2 = DateTime.Now.Ticks;
            //    message = message + Environment.NewLine + "Warming for webFreight model completed in " + ((t2 - t1) / 10000000) + " Seconds";
               
            //}
            //catch (Exception ex)
            //{
            //    string errorMessage = "An error happened while warming webFreight model the message is: " + ex.Message;
            //    if (ex.InnerException != null)
            //    {
            //        errorMessage += Environment.NewLine+"the inner exception is: " + ex.InnerException.Message;
            //    }
            //    AzureLog.SaveWarmingLogsInStorage(errorMessage, "E", 1);
            //}
            //AzureLog.SaveWarmingLogsInStorage(message, "M", 1);
        }


        [WebMethod]
        public void LoadMetaData()
        {

            AzureLog.SaveWarmingLogsInStorage("Loading MetaData started", "M", 1);
            //GlobalTenantRepository repository = new GlobalTenantRepository();
            //List<GlobalTenant> globalTenants = repository.GetActiveTenants();

            //foreach (GlobalTenant glbTenant in globalTenants)
            //{

                ObjectFieldQuery.GetTenantObjectFieldsWithTenantZero(0);
                TextCodeRepository.GetTenantTextCodesWithTenantZero(0);


            //}


            AzureLog.SaveWarmingLogsInStorage("Loading MetaData finished", "M", 1);
        }
    }

      
    public static class WarmingMethodsClass
    {
        
        public static void WarmShipmentModel(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
            {
                ShipmentRepository repository = new ShipmentRepository(tenant);
                Shipment shipment = repository.GetFirstShipment(tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(repository);

                ShipmentPM shipmentPm = shipmentQuery.GetSinglePM(shipment.Id, tenant);
                shipment.Notes = DateTime.Now.ToString();
                repository.Update(shipment);
                repository.SubmitChanges();
                scope.Complete();
            }
        }

        public static void WarmQuoteModel(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
            {
                QuoteRepository repository = new QuoteRepository(tenant);
                QuoteQuery quoteQuery = new QuoteQuery(repository);
                Quote quote = repository.GetFirstQuote(tenant);
                QuotePM quotePm = quoteQuery.GetSinglePM(quote.Id, tenant);
                quote.Notes = DateTime.Now.ToString();
                repository.Update(quote);
                repository.SubmitChanges();

                scope.Complete();
            }
        }

        public static void WarmCommonModel(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
            {
                PortRepository repository = new PortRepository(tenant);
                PortQuery portQuery = new PortQuery(repository);

                Port port = repository.GetFirstPort(tenant);
                PortPM portPm = portQuery.GetSinglePM(port.Id, tenant);
                port.Notes = DateTime.Now.ToString();
                repository.Update(port);
                repository.SubmitChanges();

                scope.Complete();
            }
        }

        public static void WarmInvoiceModel(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
            {
                ARInvoiceRepository repository = new ARInvoiceRepository(tenant);
                ARInvoice invoice = repository.GetFirstInvoice(tenant);
                ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(repository);
                ARInvoicePM invoicePm = arInvoiceQuery.GetSinglePM(invoice.Id, invoice.Tenant);
                invoice.PrintNotes = DateTime.Now.ToString();
                repository.Update(invoice);
                repository.SubmitChanges();

                scope.Complete();
            }
        }

        //public static void WarmWebFreightModel(int tenant)
        //{
        //    ObjectTabelRepository repository = new ObjectTabelRepository(0);
        //    ObjectTable objectTable = repository.GetObjectTableByName("Shipment", 0, false);
        //    objectTable.LookUp1 = DateTime.Now.ToString();
        //    repository.Update(objectTable);
        //    repository.SubmitChanges();
        //    ObjectTabelQuery objectTabelQuery = new ObjectTabelQuery(repository);
        //    ObjectTablePM objectTablePm = objectTabelQuery.GetObjectTablePMById(objectTable.Id, 0);


        //}

        public static void WarmGlobalModel(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
            {
                GlobalTenantRepository repository = new GlobalTenantRepository();
                GlobalTenant globalTenants = repository.GetGlobalTenantsByTenant(tenant);

                scope.Complete();
            }
        }


    }
}
