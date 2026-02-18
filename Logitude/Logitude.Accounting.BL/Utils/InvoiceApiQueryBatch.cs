using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.XSD.CW_API.ABM;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Logitude.Accounting.BL.Utils
{
    public class InvoiceApiQueryBatch
    {

        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public InvoiceApiQueryBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }



        public void RunInvoiceApiInvoicesQuery(string startDate, string endDate, int tenant)
        {
            try
            {
                var InvoiceApiService = new InvoiceApiService();
                if (!InvoiceApiService.OpenConnection("user", "password"))
                    throw new Exception("Failed to connect to InvoiceApi API");

                var (success, xml) = InvoiceApiService.QueryLog(
                    startDate,
                    endDate,
                    logEntryType: 0,
                    transType: "Invoice",
                    flags: 0);

                if (!success || string.IsNullOrWhiteSpace(xml))
                    throw new Exception("QueryLog failed or returned empty XML");

                var doc = XDocument.Parse(xml);
                XNamespace ns = "http://www.InvoiceApi.com/XMLSchema/V1";
                var items = doc.Descendants(ns + "GUIDItem")
                    .Select(x => new
                    {
                        Guid = (string)x.Element(ns + "GUID"),
                        Type = (string)x.Element(ns + "Type"),
                        LogType = (string)x.Element(ns + "LogType"),
                        LogDate = (string)x.Element(ns + "LogDate")
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Guid))
                    .ToList();

                if (!items.Any())
                    throw new Exception("No items found in XML");

                foreach (var item in items)
                {
                    string communicationId = AddCommuincationLog(tenant);
                    AddInvoiceApiCommuicationLog(communicationId, item.Guid, tenant);
                    SaveInvoiceApiInvoiceInQueue(item.Guid, item.Type, item.LogType, item.LogDate, communicationId, tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"SaveInvoiceApiInvoiceInQueue : {item}");

                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"RunInvoiceApiInvoicesQuery Exception: {ex.Message}");
                throw;
            }
        }



        public void SaveInvoiceApiInvoiceInQueue(string guid, string type, string logType, string logDate, string communicationId, int tenant)
        {
            try
            {

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("InvoiceApiQueue", tenant);
                queueservice.Send(new Dictionary<string, string>() { { "Guid", guid }, { "Type", type }, { "LogType", logType }, { "LogDate", logDate }, { "communicationId", communicationId } }, tenant, null, null, null, null);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"SaveInvoiceApiInvoiceInQueue Exception: {ex.Message}");
                throw;
            }
        }
        public string AddCommuincationLog(int tenant)
        {
            try
            {

                CommunicationsParams logParams = new CommunicationsParams()
                {
                    Tenant = tenant,
                    CommunicationLogTypeCode = "DCBK",
                    Priority = 1,
                    InOut = "O",
                    Status = "W",
                    Subject = "InvoiceApi Get Invoice API",
                    FolderName = "InvoiceApiBackup",

                };

                return Communications.AddCommunicationLog(logParams);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"AddCommuincationLog Exception: {ex.Message}");
                throw;
            }
        }

        public void AddInvoiceApiCommuicationLog(string communicationId, string guid, int tenant)
        {
            try
            {
                InvoiceApiCommunicationLog log = new InvoiceApiCommunicationLog()
                {
                    CommunicationId = communicationId,
                    CreateDate = DateTime.Now,
                    StatusCode = InvoiceApiStatusEnum.Created,
                    Step = InvoiceApiStepEnum.OpenInvoiceApiSession,
                    SearchFields = guid + "," + tenant,

                };
                InvoiceApiCommunicationLogRepository invoiceApiCommunicationLogRepository = new InvoiceApiCommunicationLogRepository(tenant);
                invoiceApiCommunicationLogRepository.Add(log);

            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"AddInvoiceApiCommuicationLog Exception: {ex.Message}");
                throw;
            }
        }
    }
}
