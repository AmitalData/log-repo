using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
//using Profact.TimbraCFDI;
//using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Resolvers;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40
{
    public class SATBaseProfact40Service
    {
        private int tenant;
        private ICommonDataContext commonContext;
        private string logFolder;
        private ContactPM loggedContact;

        public SATBaseProfact40Service(int tenant)
        {
            this.tenant = tenant;
            commonContext = CommonDataContext.GetContext(tenant);
            loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
        }

        public static Profact.TimbraCFDI.ResultadoConsultaEstatusSAT GetSATStatus(int tenant, string entitySATXML)
        {
            Profact.TimbraCFDI.ResultadoConsultaEstatusSAT resultadoConsultaEstatusSAT = null;
            Profact.TimbraCFDI40.Conector conector = GetProfactConnector(tenant);
            Profact.TimbraCFDI40.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(entitySATXML);
            if (comprobante.Complemento.Any == null) return resultadoConsultaEstatusSAT;

            List<XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
            if (timbreFiscalDigitalElement == null) return resultadoConsultaEstatusSAT;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
            string rfcEmisor = comprobante.Emisor.Rfc.Trim();
            string uuID = digitalTi.UUID.Trim();
            resultadoConsultaEstatusSAT = conector.ConsultaEstatusSAT(uuID);


            return resultadoConsultaEstatusSAT;
        }

        private static Profact.TimbraCFDI40.Conector GetProfactConnector(int tenant)
        {
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);

            bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
            Profact.TimbraCFDI40.Conector conector = new Profact.TimbraCFDI40.Conector(isProduction);
            conector.EstableceCredenciales(satSetting.Token);

            return conector;
        }

        public void BuildProfactCommunicationLog40(Profact40CommunicationLogArgs args)
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

        private string GetSATLogSubject(Profact40CommunicationLogArgs args)
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

        private byte[] GetProfactoXmlData(Profact40CommunicationLogArgs args)
        {
            byte[] profactoXmlData = { };
            if (args.IsCancellation)
            {
                profactoXmlData = GetCancellationProfactoXmlData(args, profactoXmlData);
            }
            else
            {
                profactoXmlData = LogitudeXmlSerializer.SerializeObject<Profact.TimbraCFDI40.Comprobante>(args.Comprobante);
            }

            return profactoXmlData;
        }

        private byte[] GetCancellationProfactoXmlData(Profact40CommunicationLogArgs args, byte[] profactoXmlData)
        {
            if (args.Comprobante.Complemento.Any == null) return profactoXmlData;

            List<System.Xml.XmlElement> myLXmlComplementos = args.Comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
            var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();

            if (timbreFiscalDigitalElement == null) return profactoXmlData;

            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
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
        
        private CommunicationLog CreateNewCommunicationLog(Profact40CommunicationLogArgs args, string logSubject, Document document)
        {
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = "Profact 4.0",
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
                SearchFields = args.EntityReference + "," + logFolder + "," + "O" + "," + logSubject,
                CreateDateUTC = DateTime.UtcNow,
                QueueName = "SATInterface",
            };

            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            return commLog;
        }

        private string GetObjectTableId(Profact40CommunicationLogArgs args)
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

        private string GetObjectTableName(Profact40CommunicationLogArgs args)
        {
            string tableName = "ARInvoice";
            if (args.IsPayment) tableName = "ARPayment";

            return tableName;
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

        private void SendSATInterfaceQueueMessage(Profact40CommunicationLogArgs args, CommunicationLog commLog)
        {
            DbQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("SATInterface", 0);
            Dictionary<string, string> param = new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() }, { "CancellationRequest", args.IsCancellation.ToString() } };
            queueservice.Send(param, tenant);
        }
        
        private void CreateSATTraceEvent(Profact40CommunicationLogArgs args)
        {
            if (args.IsCancellation) return;

            string eventTypeCode = "INTS";
            if (args.IsPayment) eventTypeCode = "PATS";
            
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = args.EntityId,
                ObjectTableName = GetObjectTableName(args),
                Tenant = tenant,
                UserId = loggedContact.Id,
                EventTypeCode = eventTypeCode,
            });
        }

        public decimal GetDecimalWith3DigitsAfterPointIfZero(decimal dNumber)
        {
            decimal result = decimal.Parse(dNumber.ToString("0.00"));
            if (result == 0 && dNumber != 0)
                result = decimal.Parse(dNumber.ToString("0.000"));
            return result;
        }

        public decimal GetDecimalWith2DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.00"));
        }

        public decimal GetDecimalWith6DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.000000"));
        }
    }

    public class Profact40CommunicationLogArgs
    {
        public Profact.TimbraCFDI40.Comprobante Comprobante { get; set; }
        public string EntityId { get; set; }
        public string EntityReference { get; set; }
        public bool IsCancellation { get; set; }
        public bool IsPayment { get; set; }
    }
}
