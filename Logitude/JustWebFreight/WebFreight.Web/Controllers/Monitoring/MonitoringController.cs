using Logitude.Infrastructure.Data;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using static WebFreight.Web.Controllers.Monitoring.MonitoringController;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.Controllers.Monitoring
{
    public class MonitoringController : ApiController
    {
        public class pingdom_http_custom_check
        {
            public string status { get; set; }
            public int response_time { get; set; }
        }

        public static string XmlSerialize<T>(T entity) where T : class
        {
            // removes version
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (StringWriter sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                // removes namespace
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                xsSubmit.Serialize(writer, entity, xmlns);
                return sw.ToString(); // Your XML
            }
        }

        private static bool CheckIfAnyCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);

            return (from a in commonDataContext.CommunicationLogs
                    where a.CreateDateUTC > twoDaysBefore
                    && a.Subject == "Advanced Generic Interface" && ((a.CommunicationStatusTypeCode == "W"
                    && (DbFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5) || a.CommunicationStatusTypeCode == "F"))
                    select a).Any();

        }

        private static bool CheckIfAnyQueueMessagesFailed(int tenant)
        {

            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "AgentsSharedDocumentAnalyzeQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        private static bool CheckIfAnyCustomerTenantCommunicationLogsFailed(int tenant)
        {
 
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "CustomerTenantAccessQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        #region AdvancedGenericInterfaceMonitoringStatus
        [HttpGet]

        public HttpResponseMessage AdvancedGenericInterfaceMonitoringStatus(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckAdvancedGenericInterfaceMonitoringStatus(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckAdvancedGenericInterfaceMonitoringStatus(int tenant)
        {
            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AdvancedGenericInterfaceMonitoringStatus", "Bug in AdvancedGenericInterface Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        //https://test-accounting.amital.co.il/test/api/Monitoring/AdvancedGenericInterfaceMonitoringStatus

        #endregion


        #region AgentSharedDocumentAnalyzeMonitoring
        [HttpGet]

        public HttpResponseMessage AgentSharedDocumentAnalyzeMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckAgentSharedDocumentAnalyzeMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckAgentSharedDocumentAnalyzeMonitoring(int tenant)
        {
            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyQueueMessagesFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AgentSharedDocumentAnalyzeMonitoring", "Bug in AgentSharedDocumentAnalyzeMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }


        //http://localhost:9996/api/Monitoring/AgentSharedDocumentAnalyzeMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/AgentSharedDocumentAnalyzeMonitoring

        #endregion


        #region AgentSharedDocumentMonitoring
        [HttpGet]

        public HttpResponseMessage AgentSharedDocumentMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckAgentSharedDocumentMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckAgentSharedDocumentMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyQueueMessagesFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AgentsSharedDocumentQueue", "Bug in AgentsSharedDocumentQueue Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;

        }
        //http://localhost:9996/api/Monitoring/AgentSharedDocumentMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/AgentSharedDocumentMonitoring

        #endregion


        #region AgentSharedManifestMonitoring
        [HttpGet]

        public HttpResponseMessage AgentSharedManifestMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckAgentSharedManifestMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckAgentSharedManifestMonitoring()
        {

            bool isWaiting = false;


            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "AgentSharedManifestMonitoring", "Bug in AgentSharedManifestMonitoring", null);
                }

                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {
                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime twoDaysBefore = todayDateTime.AddDays(-2);


                try
                {
                    isWaiting = (from a in Context.CommunicationLogs
                                 where a.CreateDateUTC > twoDaysBefore
                                 && a.QueueName == "AgentsSharedLogisticsQueue" && a.CommunicationStatusTypeCode == "W"
                                 && (EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5)
                                 select a).Any();


                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "AgentsSharedLogistics", "Bug in AgentsSharedLogistics Method : IsFaild", null);
                }

                if (isWaiting)
                {
                    break;
                }
            }
            return isWaiting;
        }


        //http://localhost:9996/api/Monitoring/AgentSharedManifestMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/AgentSharedManifestMonitoring

        #endregion


        #region AutomationMonitoring
        [HttpGet]

        public HttpResponseMessage AutomationMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckAutomationMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckAutomationMonitoring()
        {

            bool isWaiting = false;


            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "AgentSharedManifestMonitoring", "Bug in AgentSharedManifestMonitoring", null);
                }

                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {
                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime twoDaysBefore = todayDateTime.AddDays(-2);


                try
                {
                    isWaiting = (from a in Context.CommunicationLogs
                                 where a.CreateDateUTC > twoDaysBefore
                                 && a.QueueName == "AgentsSharedLogisticsQueue" && a.CommunicationStatusTypeCode == "W"
                                 && (EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5)
                                 select a).Any();


                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "AgentsSharedLogistics", "Bug in AgentsSharedLogistics Method : IsFaild", null);
                }

                if (isWaiting)
                {
                    break;
                }
            }
            return isWaiting;
        }


        //http://localhost:9996/api/Monitoring/AutomationMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/AutomationMonitoring

        #endregion


        #region AutoSignUpMonitoring
        [HttpGet]

        public HttpResponseMessage AutoSignUpMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckAutoSignUpMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckAutoSignUpMonitoring()
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyAutoSignupEmailsFailed();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AutoSignUpMonitoring", "Bug in AutoSignUpMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyAutoSignupEmailsFailed()
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IGlobalContext globalContext = GlobalContext.GetContext();

            return (from a in globalContext.AutoSignupEmails
                    where a.CreateDate > twoDaysBefore
                    && ((a.Status == "New"
                    && (EntityFunctions.DiffMinutes(a.CreateDate, todayDateTime) > 5)) || a.Status == "Fail")
                    select a).Any();

        }



        //http://localhost:9996/api/Monitoring/AutoSignUpMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/AutoSignUpMonitoring

        #endregion


        #region BIReportsExecutionLogMonitoring
        [HttpGet]

        public HttpResponseMessage BIReportsExecutionLogMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckBIReportsExecutionLogMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckBIReportsExecutionLogMonitoring(int tenant)
        {
            bool isFailed = false;
            bool isWaitingStatus = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    DateTime twoDaysBefore = DateTime.Now.AddDays(-2);
                    DateTime todayDateTime = DateTime.Now;
                    IInfrastructureContext commonDataContext = InfrastructureContext.GetContext(tenant);
                    return (from a in commonDataContext.BIReportsExecutionLogs
                            where a.CreateDate > twoDaysBefore
                            && ((a.StatusCode == "W"
                            && (EntityFunctions.DiffMinutes(a.CreateDate, todayDateTime) > 5)) || a.StatusCode == "F")
                            select a).Any();

                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "BIReportsExecutionLogs", "Bug in AnyWaitingStatus Method : isWaitingStatus = (from a in commonDataContext.ReportExecutionLogs ...", null);
                }
                scope.Complete();
            }
            return isWaitingStatus;
        }


        //http://localhost:9996/api/Monitoring/BIReportsExecutionLogMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/BIReportsExecutionLogMonitoring

        #endregion


        #region CustomerTenantAccessMonitoring
        [HttpGet]

        public HttpResponseMessage CustomerTenantAccessMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckCustomerTenantAccessMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckCustomerTenantAccessMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyCustomerTenantCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "CustomerTenantAccess", "Bug in CustomerTenantAccessMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }


        //http://localhost:9996/api/Monitoring/CustomerTenantAccessMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/CustomerTenantAccessMonitoring

        #endregion


        #region DeclarationApprovalRequestMonitoring
        [HttpGet]

        public HttpResponseMessage DeclarationApprovalRequestMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckDeclarationApprovalRequestMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckDeclarationApprovalRequestMonitoring(int tenant)
        {
            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyDeclarationApprovalCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DeclarationApprovalRequest", "Bug in DeclarationApprovalRequestQueueMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyDeclarationApprovalCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "DeclarationApprovalRequestQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }



        //http://localhost:9996/api/Monitoring/DeclarationApprovalRequestMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/DeclarationApprovalRequestMonitoring

        #endregion


        #region DocumentApprovalQueueMonitoring
        [HttpGet]

        public HttpResponseMessage DocumentApprovalQueueMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckDocumentApprovalQueueMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckDocumentApprovalQueueMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyDocumentApprovalCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DocumentApprovalQueueMonitoring", "Bug in DocumentApprovalQueueMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyDocumentApprovalCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "DocumentApprovalQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        //http://localhost:9996/api/Monitoring/DocumentApprovalQueueMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/DocumentApprovalQueueMonitoring

        #endregion


        #region DocumentFilingBackupBatchMonitoring
        [HttpGet]

        public HttpResponseMessage DocumentFilingBackupBatchMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckDocumentFilingBackupBatchMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckDocumentFilingBackupBatchMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyDocumentFilingBackupCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DocumentFilingBackupBatchQueue", "Bug in DocumentFilingBackupBatchQueue Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyDocumentFilingBackupCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "DocumentFilingBackupBatchQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        //http://localhost:9996/api/Monitoring/DocumentFilingBackupBatchMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/DocumentFilingBackupBatchMonitoring

        #endregion


        #region FTPCommunicationMonitoring
        [HttpGet]

        public HttpResponseMessage FTPCommunicationMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckFTPCommunicationMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckFTPCommunicationMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyFTPCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "FTPCommunicationMonitoring", "Bug in FTPCommunicationMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyFTPCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);

            return (from a in commonDataContext.CommunicationLogs
                    where a.CreateDateUTC > twoDaysBefore
                    && a.QueueName == "FTPCommunicationLogQueue" && (a.CommunicationStatusTypeCode == "W"
                    && EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5)
                    select a).Any();

        }
        //http://localhost:9996/api/Monitoring/FTPCommunicationMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/FTPCommunicationMonitoring

        #endregion


        #region INTTRAMonitoring
        [HttpGet]

        public HttpResponseMessage INTTRAMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckINTTRAMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckINTTRAMonitoring()
        {

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            bool isFailed = false;
            bool IsWaiting = false;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "INTTRA", "Bug in INTTRA Method : globaldbRep.All()", null);
                }

                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {
                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime oneDaysBefore = todayDateTime.AddDays(-1);

                try
                {
                    isFailed = (from a in Context.CommunicationLogs
                                where a.CommunicationStatusTypeCode == "f"
                                 && a.To == "INTTRA" && a.Subject == "Shipping Instructions"
                                select a).Any();

                    IsWaiting = (from b in Context.CommunicationLogs
                                 where
                                 b.CommunicationStatusTypeCode == "W"
                                 && ((b.To == "INTTRA" && b.Subject == "Shipping Instructions") || (b.From == "INTTRA" && b.Subject == "Status"))
                                 && (System.Data.Entity.DbFunctions.DiffMinutes(b.CreateDateUTC, DateTime.Now) > 1440)
                                 select b).Any();


                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "INTTRA", "Bug in AnyFailedStatus Method : IsFaild = (from a in Context.CommunicationLogs [INTTRA] ...", null);
                }

                if (isFailed || IsWaiting)
                {
                    break;
                }
            }

            return (isFailed || IsWaiting);
        }
        //http://localhost:9996/api/Monitoring/INTTRAMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/INTTRAMonitoring

        #endregion


        #region MobileSMSMonitoring
        [HttpGet]

        public HttpResponseMessage MobileSMSMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckMobileSMSMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckMobileSMSMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyMobileSMSQueueMessagesFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AgentsSharedDocumentQueue", "Bug in AgentsSharedDocumentQueue Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyMobileSMSQueueMessagesFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "MobileSMS" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        //http://localhost:9996/api/Monitoring/MobileSMSMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/MobileSMSMonitoring

        #endregion


        #region PODImageConverterMonitoring
        [HttpGet]

        public HttpResponseMessage PODImageConverterMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckPODImageConverterMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckPODImageConverterMonitoring(int tenant)
        {
            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyPODQueueMessagesFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "PODImageConverterMonitoring", "Bug in PODImageConverterMonitoring Method ", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyPODQueueMessagesFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "PODImageConverterQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }


        //http://localhost:9996/api/Monitoring/PODImageConverterMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/PODImageConverterMonitoring

        #endregion


        #region PrivateLabelDenialMonitoring
        [HttpGet]

        public HttpResponseMessage PrivateLabelDenialMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckPrivateLabelDenialMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckPrivateLabelDenialMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyPrivateLabelDenialQueueMessagesFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "PrivateLabelDenialMonitoring", "Bug in PrivateLabelDenialMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyPrivateLabelDenialQueueMessagesFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "PrivateLabelDenialQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        //http://localhost:9996/api/Monitoring/PrivateLabelDenialMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/PrivateLabelDenialMonitoring

        #endregion


        #region ProfactMonitoring
        [HttpGet]

        public HttpResponseMessage ProfactMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckProfactMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckProfactMonitoring()
        {

            bool isWaiting = false;


            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "ProfactMonitoring", "Bug in ProfactMonitoring", null);
                }

                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {
                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime twoDaysBefore = todayDateTime.AddDays(-2);


                try
                {
                    isWaiting = (from a in Context.CommunicationLogs
                                 where a.CreateDateUTC > twoDaysBefore
                                 && a.QueueName == "SATInterface" && ((a.CommunicationStatusTypeCode == "W"
                                 && (EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5) || a.CommunicationStatusTypeCode == "F"))
                                 select a).Any();


                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "Profact", "Bug in Profact Method : IsFaild", null);
                }

                if (isWaiting)
                {
                    break;
                }
            }
            return isWaiting;
        }

        //http://localhost:9996/api/Monitoring/ProfactMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/ProfactMonitoring

        #endregion


        #region ReportExecutionRunTimeMonitoring
        [HttpGet]

        public HttpResponseMessage ReportExecutionRunTimeMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckReportExecutionRunTimeMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckReportExecutionRunTimeMonitoring(int tenant)
        {

            bool isWaitingStatus = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    DateTime oneDaysBefore = DateTime.Now.AddDays(-1);
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    isWaitingStatus = (from d in commonDataContext.ReportExecutionLogs where d.StatusCode == "P" && d.CreateDate > oneDaysBefore && (EntityFunctions.DiffMinutes(d.CreateDate, DateTime.Now) > 60) select d).Any();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "ReportExecutionRunTimeMonitoring", "Bug in AnyReportTakesMoreThanExpectedToRun Method : isWaitingStatus = (from a in commonDataContext.ReportExecutionLogs ...", null);
                }
                scope.Complete();
            }
            return isWaitingStatus;
        }

        //http://localhost:9996/api/Monitoring/ReportExecutionRunTimeMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/ReportExecutionRunTimeMonitoring

        #endregion


        #region SchedularMonitoring
        [HttpGet]

        public HttpResponseMessage SchedularMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckSchedularMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckSchedularMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnySchedularMonitoringCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "SchedularMonitoring", "Bug in SchedularMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnySchedularMonitoringCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);

            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "SchedularQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();


        }

        //http://localhost:9996/api/Monitoring/SchedularMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/SchedularMonitoring

        #endregion


        #region ShipmentInterfaceMonitoringStatus
        [HttpGet]

        public HttpResponseMessage ShipmentInterfaceMonitoringStatus(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckShipmentInterfaceMonitoringStatus(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckShipmentInterfaceMonitoringStatus(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyShipmentInterfaceCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "ShipmentInterfaceMonitoringStatus", "Bug in ShipmentInterfaceMonitoringStatus Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyShipmentInterfaceCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);

            return (from a in commonDataContext.CommunicationLogs
                    where a.CreateDateUTC > twoDaysBefore
                    && a.Subject == "Shipment Interface"
                    && ((a.CommunicationStatusTypeCode == "W" && (EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5)) || a.CommunicationStatusTypeCode == "F")
                    select a).Any();

        }

        //http://localhost:9996/api/Monitoring/ShipmentInterfaceMonitoringStatus
        //https://test-accounting.amital.co.il/test/api/Monitoring/ShipmentInterfaceMonitoringStatus

        #endregion


        #region SignupLeadMonitoringPage
        [HttpGet]

        public HttpResponseMessage SignupLeadMonitoringPage(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckSignupLeadMonitoringPage(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckSignupLeadMonitoringPage(int tenant)
        {

            bool isSignupLeadProcessFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isSignupLeadProcessFailed = CheckIfSignupLeadProcessFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "SignupLeadMonitoringPage", "Bug in Signup Lead Processs : globaldbRep.GetFirstNotCompletedLogitudeLead()", null);
                }
                scope.Complete();
            }
            return isSignupLeadProcessFailed;
        }

        private static bool CheckIfSignupLeadProcessFailed(int tenant)
        {
            bool isSignupLeadProcessFailed = false;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {

                DateTime twoDaysBefore = TenantServerConfigration.GetCurrentDateTime(tenant).AddDays(-2);
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                isSignupLeadProcessFailed = (from d in commonDataContext.CommunicationLogs
                                             where d.CommunicationStatusTypeCode == "f" && d.CommunicationLogTypeCode == "Lead" && (d.CreateDate > twoDaysBefore)
                                             select d).Any();
                scope.Complete();
            }

            return isSignupLeadProcessFailed;
        }



        //http://localhost:9996/api/Monitoring/SignupLeadMonitoringPage
        //https://test-accounting.amital.co.il/test/api/Monitoring/SignupLeadMonitoringPage

        #endregion


        #region SignUpMonitoring
        [HttpGet]

        public HttpResponseMessage SignUpMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckSignUpMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckSignUpMonitoring()
        {

            bool isFailed = false;

            try
            {
                CloudStorageAccount storageAccount = StorageAcountDetails.StorageAccount;
                CloudQueueClient queueclient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueclient.GetQueueReference("signupqueue");
                queue.CreateIfNotExists();
                return queue.ApproximateMessageCount > 50;
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "SignUpMonitoring", "Bug in SignUpMonitoring Method ", "");
            }

            return isFailed;

        }

        private static long GetMessageQueueCount(string emailqueueName)
        {
            long messagecount;
            QueueDescription queueDescription = new QueueDescription(emailqueueName);
            queueDescription.MaxSizeInMegabytes = 5120;
            queueDescription.MaxDeliveryCount = 99999;
            queueDescription.LockDuration = new TimeSpan(0, 2, 0);
            messagecount = StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription).MessageCount;
            return messagecount;
        }

        //http://localhost:9996/api/Monitoring/SignUpMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/SignUpMonitoring

        #endregion


        #region SocialMonitoring
        [HttpGet]

        public HttpResponseMessage SocialMonitoring()
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckSocialMonitoring();
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckSocialMonitoring()
        {

            bool isFailed = false;

            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
                {
                    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("socialqueue");
                    long messagecount = 0;
                    if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailqueueName))
                    {
                        messagecount = GetSocialMonitoringMessageQueueCount(emailqueueName);

                    }
                    else
                    {
                        messagecount = StorageAcountDetails.NameSpaceManager.GetQueue(emailqueueName).MessageCount;
                    }

                    return messagecount > 50;
                }
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "SocialMonitoring", "Bug in SocialMonitoring Method ", "");
            }

            return isFailed;

        }

        private static long GetSocialMonitoringMessageQueueCount(string emailqueueName)
        {
            long messagecount;
            QueueDescription queueDescription = new QueueDescription(emailqueueName);
            queueDescription.MaxSizeInMegabytes = 5120;
            queueDescription.MaxDeliveryCount = 99999;
            queueDescription.LockDuration = new TimeSpan(0, 2, 0);
            messagecount = StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription).MessageCount;
            return messagecount;
        }


        //http://localhost:9996/api/Monitoring/SocialMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/SocialMonitoring

        #endregion


        #region WebHookCommunicationMonitoring
        [HttpGet]

        public HttpResponseMessage WebHookCommunicationMonitoring(int tenant)
        {
            pingdom_http_custom_check pingdomCheck = new pingdom_http_custom_check();
            bool isOK = !CheckWebHookCommunicationMonitoring(tenant);
            pingdomCheck.status = isOK ? "OK" : "Fail";
            pingdomCheck.response_time = HttpContext.Current.Timestamp.Millisecond;

            string xml = XmlSerialize(pingdomCheck);


            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(xml, System.Text.Encoding.UTF8, "text/xml") };
        }


        private bool CheckWebHookCommunicationMonitoring(int tenant)
        {

            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyWebHookCommunicationLogsFailed(tenant);
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "WebHookCommunication", "Bug in WebHookCommunicationMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyWebHookCommunicationLogsFailed(int tenant)
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "WebHookCommunicationLogQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }

        //http://localhost:9996/api/Monitoring/WebHookCommunicationMonitoring
        //https://test-accounting.amital.co.il/test/api/Monitoring/WebHookCommunicationMonitoring

        #endregion











    }

}





