using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.XSD.CW_API.ABM;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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

                InvoiceApiService.OpenConnection(tenant);

                var  xml= InvoiceApiService.QueryLog(
                    startDate,
                    endDate,
                    logEntryType: 0x01,
                    transType: "IN",
                    flags: 0x00);
                InvoiceApiService.EndSession();

                if (string.IsNullOrWhiteSpace(xml))
                    throw new Exception("QueryLog failed or returned empty XML");

                var doc = XDocument.Parse(xml);
                XNamespace ns = "http://www.magaya.com/XMLSchema/V1";
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
                    _ResponseText = "No items found in XML";

                foreach (var item in items)
                {
                    string InvoiceApiCommunicationLogId=AddInvoiceApiCommuicationLog(item?.Guid, tenant);
                    var messageBody = new Dictionary<string, string>
                    {
                        { "Guid", item?.Guid },
                        { "Type", item?.Type },
                        { "LogType", item ?.LogType },
                        { "LogDate", item ?.LogDate },
                        { "InvoiceApiId", InvoiceApiCommunicationLogId},
                    };
                    SaveInvoiceApiInvoiceInQueue(messageBody , tenant);
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"SaveInvoiceApiInvoiceInQueue : {item}");

                }

            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"RunInvoiceApiInvoicesQuery Exception: {ex.Message}");
                throw;
            }
        }



        public void SaveInvoiceApiInvoiceInQueue( Dictionary<string, string> messageBody, int tenant)
        {
            try
            {

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("InvoiceApiWR", tenant);
                queueservice.Send(messageBody, tenant, null, null, null, null);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"SaveInvoiceApiInvoiceInQueue Exception: {ex.Message}");
                throw;
            }
        }
       

        public string AddInvoiceApiCommuicationLog( string guid, int tenant)
        {
            try
            {
                InvoiceApiCommunicationLogPM log = new InvoiceApiCommunicationLogPM()
                {
                    DocumentId = null,
                    CreateDate = DateTime.Now,
                    StatusCode = InvoiceApiStatusEnum.Created,
                    Step = InvoiceApiStepEnum.OpenInvoiceApiSession,
                    SearchFields = guid + "," + tenant ,
                    Tenant = tenant,
                     ExternalID = guid,

                };
                log.ChangeSetOp = ChangeSetOperation.Insert;
                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);

                InvoiceApiCommunicationLogUpdateService invoiceApiCommunicationLogRepository = new InvoiceApiCommunicationLogUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                invoiceApiCommunicationLogRepository.Update(log,true);
                return log.Id;

            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"AddInvoiceApiCommuicationLog Exception: {ex.Message}");
                throw;
            }
        }
    }
}
