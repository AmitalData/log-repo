using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Profact.TimbraCFDI40;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATCommunicationLogBuilder
    {
        private int tenant;
        private ICommonDataContext commonContext;
        private string logFolder;
        private ContactPM loggedContact;

        public SATCommunicationLogBuilder(int tenant)
        {
            this.tenant = tenant;
            commonContext = CommonDataContext.GetContext(tenant);
            loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
        }

        public void Build(SATCommunicationLogArgs args)
        {
            logFolder = "SATInterface";
            string logSubject = GetSATLogSubject(args);
            byte[] profactoXmlData = GetProfactoXmlData(args);
            Document document = CreateNewDocument(profactoXmlData);
            CommunicationLog commLog = CreateNewCommunicationLog(args, logSubject, document);
            WriteDocumentOnBlobStorage(profactoXmlData, document);
            SendSATInterfaceQueueMessage(args, commLog);
            CreateSATTraceEvent(args);
        }

        private string GetSATLogSubject(SATCommunicationLogArgs args)
        {
            string logSubject = "SAT Interface";
            if (args.IsCancellation)
            {
                logSubject = args.IsPayment ? "Payment SAT Interface Cancellation" : "SAT Interface Cancellation Request";
            }
            else if (args.IsPayment)
            {
                logSubject = "Payment SAT Interface";
            }

            return logSubject;
        }

        private byte[] GetProfactoXmlData(SATCommunicationLogArgs args)
        {
            byte[] profactoXmlData = { };
            if (args.IsCancellation)
            {
                profactoXmlData = GetCancellationProfactoXmlData(args, profactoXmlData);
            }
            else
            {
                profactoXmlData = LogitudeXmlSerializer.SerializeObject<Comprobante>(args.Comprobante);
            }

            return profactoXmlData;
        }

        private byte[] GetCancellationProfactoXmlData(SATCommunicationLogArgs args, byte[] profactoXmlData)
        {
            if (args.Comprobante.Complemento.Any == null) return profactoXmlData;

            List<System.Xml.XmlElement> myLXmlComplementos = args.Comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();

            if (timbreFiscalDigitalElement == null) return profactoXmlData;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            string rfcEmisor = args.Comprobante.Emisor.Rfc.Trim();
            string folioFiscal = digitalTi.UUID.Trim();

            SATCancellation cancellatio = new SATCancellation() { rfcEmisor = rfcEmisor, folioFiscal = folioFiscal };
            profactoXmlData = LogitudeXmlSerializer.SerializeObject<SATCancellation>(cancellatio);

            return profactoXmlData;
        }

        private Document CreateNewDocument(byte[] profactoXmlData)
        {
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "xml",
                FileSize = profactoXmlData.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = logFolder.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            return document;
        }

        private CommunicationLog CreateNewCommunicationLog(SATCommunicationLogArgs args, string logSubject, Document document)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = "Profact " + SATData.CurrentComprobanteVersion,
                InOut = "O",
                From = currentTenant.Company,
                EntityId = args.EntityId,
                ObjectTableId = GetObjectTableId(args),
                Subject = logSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = loggedContact.Id,
                DocumentId = document.Id,
                EntityReference = args.EntityReference,
                SearchFields = GetSeactFields(args, logSubject),
                CreateDateUTC = DateTime.UtcNow,
                QueueName = SATData.SATInterfaceQueueMessageCode,
            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }

        private string GetObjectTableId(SATCommunicationLogArgs args)
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            string tableName = GetObjectTableName(args);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(tableName, 0, true);
            string myObjectTableId = null;
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }

            return myObjectTableId;
        }

        private string GetObjectTableName(SATCommunicationLogArgs args)
        {
            if (args.IsPayment) return SATData.ARPaymentObjectTableName;
            else return SATData.ARInvoiceObjectTableName;
        }

        private string GetSeactFields(SATCommunicationLogArgs args, string logSubject)
        {
            return args.EntityReference + "," + logFolder + "," + "O" + "," + logSubject;
        }

        private void WriteDocumentOnBlobStorage(byte[] profactoXmlData, Document document)
        {
            Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Server.Tools.StorageService.IBlobService), "StorageService", new Microsoft.Practices.Unity.ParameterOverride("", 1)) as Server.Tools.StorageService.IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = profactoXmlData.Length,
            };

            storageservice.Write(profactoXmlData, fileInfo);
        }

        private void SendSATInterfaceQueueMessage(SATCommunicationLogArgs args, CommunicationLog commLog)
        {
            DbQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(SATData.SATInterfaceQueueMessageCode, 0);
            Dictionary<string, string> param = new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() }, { "CancellationRequest", args.IsCancellation.ToString() } };
            queueservice.Send(param, tenant);
        }

        private void CreateSATTraceEvent(SATCommunicationLogArgs args)
        {
            if (args.IsCancellation) return;

            string eventTypeCode = SATData.InvoiceTransferingEventTypecode;
            if (args.IsPayment) eventTypeCode = SATData.PaymentTransferingEventTypecode;

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = args.EntityId,
                ObjectTableName = GetObjectTableName(args),
                Tenant = tenant,
                UserId = loggedContact.Id,
                EventTypeCode = eventTypeCode,
            });
        }
    }

    public class SATCommunicationLogArgs
    {
        public Comprobante Comprobante { get; set; }
        public string EntityId { get; set; }
        public string EntityReference { get; set; }
        public bool IsCancellation { get; set; }
        public bool IsPayment { get; set; }
    }
}
