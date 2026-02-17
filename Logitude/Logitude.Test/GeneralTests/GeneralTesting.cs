using System;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using System.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.DatabaseMigration.LogitudeModel;
namespace Logitude.Test.GeneralTests
{
    [TestClass]
    public class GeneralTesting
    {
        public GeneralTesting()
        {

        }
        string globalConnectionString = @"2015R4_Global,sa,Saas256,.";
        string mainConnectionString = @"2015R4_MainMigrationTest2,sa,Saas256,.";
        [TestMethod]
        public void GlobalContextInitializationTest()
        {
            DbConnection connection = DatabaseInitializer.GetConnection(globalConnectionString);
            GlobalContext context = new GlobalContext(connection);

            

            string isUpgrading = (from a in context.GlobalDBs
                                select a).FirstOrDefault().DBConnection;
            Assert.AreNotEqual(isUpgrading, null);
        }

        [TestMethod]
        public void WebFreightContextInitializationTest()
        {

            DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);


                IWebFreightContext cont = new WebFreightContext(connection);
                EventTypeCategory categories = (from a in cont.EventTypeCategories
                                                select a).FirstOrDefault();
                Assert.AreNotEqual(categories, null);
          
        }

        [TestMethod]
        public void InvoiceContextInitializationTest()
        {
            DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);

            IInvoiceContext invcont = new InvoiceContext(connection);
            ARInvoice invoices = (from a in invcont.ARInvoices
                                  select a).FirstOrDefault();

            Assert.AreNotEqual(invoices, null);
        }

        [TestMethod]
        public void CommonContextInitializationTest()
        {
            DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);

            ICommonDataContext context = CommonDataContext.GetContext(1);//new CommonDataContext(connection);
            ChargeTypeAccounting Ports = (from a in context.ChargeTypeAccountings
                                          select a).FirstOrDefault();

            Assert.AreNotEqual(Ports, null);
        }

        [TestMethod]
        public void ShipmentContextInitializationTest()
        {
            DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);

            IShipmentsContext invcont = new ShipmentsContext(connection);
            Shipment shipment = (from a in invcont.Shipments
                          select a).FirstOrDefault();

            Assert.AreNotEqual(shipment, null);
        }

        [TestMethod]
        public void QuoteContextInitializationTest()
        {
            DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);

            IQuotesContext invcont = new QuotesContext(connection);
            Quote quote = (from a in invcont.Quotes
                                 select a).FirstOrDefault();

            Assert.AreNotEqual(quote, null);
        }

        [TestMethod]
        public void CustomContextInitializationTest()
        {
            DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);
            PhysicalCheck d = null;
            ICustomContext invcont = new CustomContext(connection);
            try
            {
                 d = (from a in invcont.PhysicalChecks
                                   select a).FirstOrDefault();
            }
            catch { }

            Assert.AreEqual(d, null);
        }

        [TestMethod]
        public void TestTopic()
        {
            BrokeredMessage message = new BrokeredMessage();
            message.Label = "InvalidateCache";
            message.Properties["Key"] = "123";
           // message.TimeToLive = new TimeSpan(0, 5, 0);
          //  TopicClient client = Microsoft.ServiceBus.Messaging.TopicClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(WebFreightEntryPoint.DeploymentStage), "DataCacheTopic");
            //client.BeginSend(message, callbackmethod, "");

            //client.Send(message);
        }

        [TestMethod]
        public void LogitudeContextInitializationTest()
        {
            //DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);

            LogitudeMigrationContext context = new LogitudeMigrationContext();//new CommonDataContext(connection);
            //ChargeTypeAccounting entities = (from a in context.ChargeTypeAccountings
            //                                 select a).FirstOrDefault();

            //Assert.AreNotEqual(entities, null);
        }

    }
}
