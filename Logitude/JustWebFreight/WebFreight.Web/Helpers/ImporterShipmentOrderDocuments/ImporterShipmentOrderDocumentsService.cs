using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;

namespace WebFreight.Web.Helpers.ImporterShipmentOrderDocuments
{
    public class ImporterShipmentOrderDocumentsService
    {
        private readonly int tenant;
        private readonly string correlationId;
        private IWebFreightContext webFreightContext;
        private ICommonDataContext objectContext;
        private ObjectTableRepository objectTableRepository;
        private APILogsQuery aPILogsQuery;
        private APILogsService apiLogsService;
        private ShipmentQuery shipmentQuery;
        private ShipmentOrderDocumentAmMapping shipmentOrderDocumentAmMapping;
        private DocumentsFilingService documentsFilingService;
        private ObjectTable objectTable;
        private DocumentsFilingAM documentsFilingAM;
        private APILogsPM apiLog;

        public ImporterShipmentOrderDocumentsService(int tenant, string correlationId)
        {
            this.tenant = tenant;
            this.correlationId = correlationId;
            InitiallizeServices();
            InitiallizeFields();

        }

        private void InitiallizeServices()
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
            objectContext = CommonDataContext.GetContext(tenant);

            objectTableRepository = new ObjectTableRepository(webFreightContext);
            aPILogsQuery = new APILogsQuery(tenant);
            apiLogsService = new APILogsService(webFreightContext, tenant);
            shipmentQuery = new ShipmentQuery(tenant);
            shipmentOrderDocumentAmMapping = new ShipmentOrderDocumentAmMapping(tenant);
            documentsFilingService = new DocumentsFilingService(objectContext,tenant);
        }
        private void InitiallizeFields()
        {
            objectTable = objectTableRepository.GetObjectTableByName("DocumentsFiling", tenant, true);
        }


        public DocumentsFilingPM CreateDocument(DocumentsFilingAM documentsFilingAM)
        {
            this.documentsFilingAM = documentsFilingAM;
            GetApiLog();
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Insert Documents To Importer Tenant From Shipment Order " + DateTime.Now, TrimXmlString(documentsFilingAM), null, null, "");
            try
            {
                ShipmentPM shipment = shipmentQuery.GetSingleShipmentPMByNumber(documentsFilingAM.EntityNumber, tenant);
                if (shipment == null) throw new Exception("Shipment number does not exist");
                DocumentsFilingPM documentsFilingPM = shipmentOrderDocumentAmMapping.Map(documentsFilingAM, shipment);
                CreateDocumentsFiling(documentsFilingPM);
                return documentsFilingPM;
            }
            catch (Exception ex)
            {
                HandleExeption(ex);
                return null;
            }
        }

        private void CreateDocumentsFiling(DocumentsFilingPM documentsFilingPM)
        {
            documentsFilingService.Create(documentsFilingPM, documentsFilingPM.FileData, null, false);

            var msg = "Insert Shipment Order Document Done Successfully " + DateTime.Now;
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, msg, null, documentsFilingPM.Id, null, "");
        }

        private static string TrimXmlString(DocumentsFilingAM documentFilingAM)
        {
            var objectString = LogitudeXmlSerializer.SerializeObjectToXmlString(documentFilingAM);
            int start = objectString.IndexOf("<FileData>");
            int end = objectString.IndexOf("</FileData>", start);
            string result = objectString.Substring(start + 10, end - start - 10);
            return objectString.Replace(result, result.Substring(0, 10));
        }

        private void HandleExeption(Exception exception)
        {
            string errorMessage = exception.Message + Environment.NewLine;

            if (exception.InnerException != null)
            {
                errorMessage = errorMessage + " (" + (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) + ")" + Environment.NewLine;
            }

            errorMessage = errorMessage + exception.StackTrace + Environment.NewLine;
            APILogsUtility.UpdateAPILogStatus(apiLog.Id, apiLog.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Insert Documents To Importer Tenant From Shipment Order Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

            throw exception;
        }

        private void GetApiLog()
        {
            apiLog = aPILogsQuery.GetSingleByCorrelationIdAndTenant(correlationId, tenant);
            if (apiLog != null) return;
            apiLog = CreateNewApiLogInstance();
            apiLogsService.Create(apiLog);
        }

        private APILogsPM CreateNewApiLogInstance()
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = correlationId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "I",
                EntityId = documentsFilingAM.EntityNumber,
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ObjectTableId = objectTable.Id,
                ExpirationDate = DateTime.Now.AddDays(90),
                Refrence = documentsFilingAM.EntityNumber,
                Status = "I",
                Tenant = documentsFilingAM.ImporterTenant,
                Subject = "Insert Documents To Importer Tenant From Shipment Order"
            };
        }
    }
}