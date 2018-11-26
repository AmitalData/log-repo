using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.Analyzers.INTTRAAnalyzer
{
    public partial class INTTRAAnalyzer
    {
        private int Tenant;
        private string Subject;
        private string Recipient;
        private string ShipmentId;
        private string ShipmentNumber;
        private bool IsAccepted;
        //private string HeaderDocumentIdentifier;
        private ShipmentPM shipmentPM;
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private MemoryStream myMemoryStream;
        private INTTRA.Message iMessage;
        private INTTRA_Status.MessageType iMessage_Status;
        public INTTRAAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.Tenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
            }
        }

        private IShipmentsContext myShipmentContext;
        private ICommonDataContext myCommonContext;
        private IWebFreightContext myWebFreightContext;
        private ShipmentRepository iShipmentRepository;
        private ShipmentQuery iShipmentQuery;
        private CommunicationLogRepository myCommunicationLogRepository;
        private ObjectTableRepository myObjectTabelRepository;
        private PortRepository iPortRepository;
        private CountryRepository iCountryRepository;
        private GlobalZoneRepository iGlobalZoneRepository;
        private ShipmentContainerStatusRepository iShipmentContainerStatusRepository;
        private void InitializeComponent()
        {
            myShipmentContext = ShipmentsContext.GetContext(this.Tenant);
            myCommonContext = CommonDataContext.GetContext(this.Tenant);
            myWebFreightContext = WebFreightContext.GetContext(this.Tenant);
            iShipmentRepository = new ShipmentRepository(myShipmentContext);
            iShipmentQuery = new ShipmentQuery(iShipmentRepository);
            iPortRepository = new PortRepository(myCommonContext);
            iCountryRepository = new CountryRepository(myCommonContext);
            iGlobalZoneRepository = new GlobalZoneRepository(myCommonContext);
            myCommunicationLogRepository = new CommunicationLogRepository(myCommonContext);
            myObjectTabelRepository = new ObjectTableRepository(myWebFreightContext);
            iShipmentContainerStatusRepository = new ShipmentContainerStatusRepository(myShipmentContext);
        }

        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();

                if (!string.IsNullOrEmpty(this.Subject))
                {
                    this.AnalyzeData();
                }
            }
        }
        private void Deserialize()
        {
            try
            {
                int index = 0;
                while (index < myAnalyzeQueue.MessageBody.Length)
                {
                    int d = myAnalyzeQueue.MessageBody[index];
                    if (d == 60)
                    {
                        if (index > 0)
                        {
                            myAnalyzeQueue.MessageBody = myAnalyzeQueue.MessageBody.Skip(index).ToArray();
                            myAnalyzeQueue.FileSize = myAnalyzeQueue.FileSize - index;
                        }

                        break;
                    }

                    index++;
                }

                this.myMemoryStream = new MemoryStream(myAnalyzeQueue.MessageBody);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(myMemoryStream);
                myMemoryStream.Position = 0;

                this.GetXmlSubject(xmlDocument);
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "Xml Document Load failed: " + ex.Message + " InnerException: " + ex.InnerException;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }
        }
        private void GetXmlSubject(XmlDocument xmlDocument)
        {
            XmlNodeList xnList = xmlDocument.GetElementsByTagName("MessageType");

            foreach (XmlNode node in xnList)
            {
                this.Subject = node.InnerText;
                break;
            }

            myAnalyzeQueue.Subject = this.Subject;

            switch (this.Subject)
            {
                case "CONTRL":
                case "ApplicationAcknowledgment":
                    {
                        this.GetXmlAcknowledgment(xmlDocument);
                        break;
                    }
            }
        }

        private void GetXmlAcknowledgment(XmlDocument xmlDocument)
        {
            XmlNodeList xnList = xmlDocument.GetElementsByTagName("ShipmentIdentifier");

            foreach (XmlNode node in xnList)
            {
                XmlAttributeCollection d = node.Attributes;

                foreach (XmlAttribute i in d)
                {
                    if (i.Name == "Acknowledgment")
                    {
                        if (i.InnerText == "Accepted" || i.Value == "Accepted")
                        {
                            this.IsAccepted = true;
                        }
                    }
                }

                break;
            }
        }

        private void AnalyzeData()
        {
            switch (this.Subject)
            {
                case "Status":
                case "CONTRL":
                case "ApplicationAcknowledgment":
                    {
                        #region
                        try
                        {
                            switch (this.Subject)
                            {
                                case "CONTRL":
                                case "ApplicationAcknowledgment":
                                    {
                                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(INTTRA.Message));
                                        INTTRA.Message iMessage = (INTTRA.Message)xmlSerializer.Deserialize(myMemoryStream);
                                        this.iMessage = iMessage;

                                        if (iMessage != null)
                                        {
                                            INTTRA.Header iHeader = iMessage.Header;
                                            INTTRA.MessageBody iMessageBody = iMessage.MessageBody;

                                            if (iHeader != null)
                                            {
                                                INTTRA.PartnerInformation iRecipient = iHeader.Parties.Where(d => d.PartnerRole == INTTRA.PartnerInformationPartnerRole.Recipient).FirstOrDefault();
                                                this.Recipient = iRecipient.PartnerIdentifier.Value;

                                                string HeaderDocumentIdentifier = iHeader.DocumentIdentifier;

                                                if (!string.IsNullOrEmpty(HeaderDocumentIdentifier))
                                                {
                                                    if (HeaderDocumentIdentifier.Length <= 20)
                                                    {
                                                        this.myAnalyzeQueue.AWBNumber = HeaderDocumentIdentifier;

                                                    }

                                                    else
                                                    {
                                                        this.myAnalyzeQueue.AWBNumber = HeaderDocumentIdentifier.Substring(0, 20);
                                                    }
                                                }                                                
                                            }

                                            if (iMessageBody != null)
                                            {
                                                INTTRA.MessageProperties iMessageProperties = iMessageBody.MessageProperties;
                                                this.ShipmentNumber = iMessageProperties.ShipmentID.ShipmentIdentifier.Value;
                                                //this.IsAccepted = iMessageProperties.ShipmentID.ShipmentIdentifier.Acknowledgment == INTTRA.Acknowledgment.Accepted ? true : false;
                                            }
                                        }

                                        break;
                                    }

                                case "Status":
                                    {
                                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(INTTRA_Status.MessageType));
                                        INTTRA_Status.MessageType iMessage = (INTTRA_Status.MessageType)xmlSerializer.Deserialize(myMemoryStream);
                                        this.iMessage_Status = iMessage;

                                        if (iMessage != null)
                                        {
                                            INTTRA_Status.HeaderType iHeader = iMessage.Header;
                                            INTTRA_Status.MessageBodyType iMessageBody = iMessage.MessageBody;

                                            if(iHeader != null)
                                            {
                                                INTTRA_Status.PartnerInformationType iRecipient = iHeader.Parties.Where(d => d.PartnerRole == INTTRA_Status.PartnerInformationTypePartnerRole.Recipient).FirstOrDefault();
                                                this.Recipient = iRecipient.PartnerIdentifier.Value;
                                            }

                                            if (iMessageBody != null)
                                            {
                                                INTTRA_Status.MessagePropertiesType iMessageProperties = iMessageBody.MessageProperties;

                                                if (iMessageProperties.ReferenceInformation != null)
                                                {
                                                    INTTRA_Status.MessagePropertiesTypeReferenceInformation FreightForwarderReference = iMessageProperties.ReferenceInformation.Where(d => d.ReferenceType == INTTRA_Status.ReferenceInformationTypeReferenceType.FreightForwarderReference).FirstOrDefault();
                                                    if(FreightForwarderReference != null)
                                                    {
                                                        this.ShipmentNumber = FreightForwarderReference.Value;
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                    }
                            }
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        #endregion

                        break;
                    }

                default:
                    {
                        if (Subject == null)
                        {
                            Subject = "";
                        }

                        myAnalyzeQueue.Status = "F";
                        myAnalyzeQueue.ErrorMessage = "The message type is not known! " + Subject;
                        myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                        analyzeQueueRepository.Update(myAnalyzeQueue);
                        analyzeQueueRepository.SubmitChanges();
                        break;
                    }
            }          
        }
        private void ConnectAnalyzeQueue()
        {
            this.InitializeComponent();

            try
            {
                if (!myAnalyzeQueue.ConnectedToTenant)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        myAnalyzeQueue.CommunicationLogId = ConnectQueueToTenant();
                        scope.Complete();
                    }

                    myAnalyzeQueue.ConnectedToTenant = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                if (!myAnalyzeQueue.ConnectedToEntity)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        ConnectQueueToEntity();
                        scope.Complete();
                    }

                    myAnalyzeQueue.ConnectedToEntity = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    this.AnalyzeMessageQueue();
                    scope.Complete();
                }

                myAnalyzeQueue.Status = "D";
                myAnalyzeQueue.ErrorMessage = null;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }
        private string ConnectQueueToTenant()
        {
            int? iMessageTenant = null;
            string iMessageTenantError = null;

            switch (this.Subject)
            {
                case "CONTRL":
                    {
                        #region
                        iMessageTenantError = "Unknown message tenant";

                        if (this.iMessage != null)
                        {
                            INTTRA.Header iHeader = this.iMessage.Header;

                            if (iHeader != null)
                            {
                                string HeaderDocumentIdentifier = iHeader.DocumentIdentifier;

                                if (!string.IsNullOrEmpty(HeaderDocumentIdentifier))
                                {
                                    string[] Parts = HeaderDocumentIdentifier.Split('-');

                                    int CONTRL_Tenant = 0;

                                    bool is_CONTRL_Tenant = Int32.TryParse(Parts[1], out CONTRL_Tenant);

                                    if (is_CONTRL_Tenant)
                                    {
                                        iMessageTenant = CONTRL_Tenant;
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    }

                case "ApplicationAcknowledgment":
                    {
                        #region
                        iMessageTenantError = "Unknown message tenant";

                        if (this.iMessage != null)
                        {
                            INTTRA.MessageBody iMessageBody = this.iMessage.MessageBody;

                            if (iMessageBody != null)
                            {
                                if (iMessageBody.MessageProperties != null)
                                {
                                    if (iMessageBody.MessageProperties.Parties != null)
                                    {
                                        INTTRA.PartnerInformation Requestor = iMessageBody.MessageProperties.Parties.Where(d => d.PartnerRole == INTTRA.PartnerInformationPartnerRole.Requestor).FirstOrDefault();
                                        if (Requestor != null)
                                        {
                                            if (Requestor.PartnerIdentifier != null)
                                            {
                                                if (Requestor.PartnerIdentifier.Value != null)
                                                {
                                                    Branch iBranch = this.myCommonContext.Branches.Where(d => d.INTTRAAlias == Requestor.PartnerIdentifier.Value).FirstOrDefault();
                                                    //INTTRASetting myINTTRASetting = this.myCommonContext.INTTRASettings.Where(d => d.INTTRAAlias == Requestor.PartnerIdentifier.Value).FirstOrDefault();
                                                    if (iBranch != null)
                                                    {
                                                        iMessageTenant = iBranch.Tenant;
                                                    }

                                                    else
                                                    {
                                                        iMessageTenantError = "There is no Tenant for this Requestor";
                                                    }
                                                }
                                            }
                                        }

                                        else
                                        {
                                            iMessageTenantError = "Unknown message Requestor";
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    }

                case "Status":
                    {
                        iMessageTenantError = "Unknown message tenant";

                        if (this.iMessage_Status != null)
                        {
                            INTTRA_Status.MessageBodyType iMessageBody = iMessage_Status.MessageBody;

                            if (iMessageBody != null)
                            {
                                INTTRA_Status.MessageDetailsType iMessageDetails = iMessageBody.MessageDetails;

                                if (iMessageDetails != null)
                                {
                                    INTTRA_Status.EquipmentDetailsType equipmentDetails = iMessageDetails.EquipmentDetails;

                                    if (equipmentDetails != null)
                                    {
                                        if (equipmentDetails.EquipmentIdentifier != null)
                                        {
                                            if (!string.IsNullOrEmpty(equipmentDetails.EquipmentIdentifier.Value))
                                            {
                                                string iContainerNumber = equipmentDetails.EquipmentIdentifier.Value;

                                                if (iContainerNumber != null)
                                                {
                                                    iContainerNumber = iContainerNumber.Trim();
                                                }

                                                if (!string.IsNullOrEmpty(iContainerNumber))
                                                {
                                                    if (!string.IsNullOrEmpty(this.ShipmentNumber))
                                                    {
                                                        #region By Shipment number
                                                        List<Shipment> iShipments = (from d in myShipmentContext.Shipments where d.ShipmentNumber == this.ShipmentNumber select d).ToList();

                                                        foreach (Shipment item in iShipments)
                                                        {
                                                            ShipmentPackage iPackage = (from d in myShipmentContext.ShipmentPackages where d.ShipmentId == item.Id && d.ContainerNumber == iContainerNumber select d).FirstOrDefault();

                                                            if (iPackage != null)
                                                            {
                                                                iMessageTenant = item.Tenant;
                                                                break;
                                                            }
                                                        }
                                                        #endregion
                                                    }

                                                    else
                                                    {
                                                        #region By Booking number
                                                        INTTRA_Status.MessagePropertiesType iMessageProperties = iMessageBody.MessageProperties;

                                                        if (iMessageProperties != null)
                                                        {
                                                            if (iMessageProperties.ReferenceInformation != null)
                                                            {
                                                                INTTRA_Status.MessagePropertiesTypeReferenceInformation iReferenceInformation = iMessageProperties.ReferenceInformation.Where(d => d.ReferenceType == INTTRA_Status.ReferenceInformationTypeReferenceType.BookingNumber).FirstOrDefault();
                                                                if (iReferenceInformation != null)
                                                                {
                                                                    string iBookingNumber = iReferenceInformation.Value;

                                                                    if (!string.IsNullOrEmpty(iBookingNumber))
                                                                    {
                                                                        List<ShipmentMasterData> iShipmentMasterDatas = (from d in myShipmentContext.ShipmentMasterDatas where d.BookingConfirmationNumber == iBookingNumber select d).ToList();

                                                                        foreach (ShipmentMasterData item in iShipmentMasterDatas)
                                                                        {
                                                                            ShipmentPackage iPackage = (from d in myShipmentContext.ShipmentPackages where d.ShipmentId == item.Id && d.ContainerNumber == iContainerNumber select d).FirstOrDefault();

                                                                            if (iPackage != null)
                                                                            {
                                                                                iMessageTenant = item.Tenant;

                                                                                Shipment iShipment = (from d in myShipmentContext.Shipments where d.Id == item.Id select d).FirstOrDefault();

                                                                                if (iShipment != null)
                                                                                {
                                                                                    this.ShipmentNumber = iShipment.ShipmentNumber;
                                                                                }

                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }

                default:
                    {
                        if (!string.IsNullOrEmpty(this.Recipient))
                        {
                            INTTRASetting myINTTRASetting = this.myCommonContext.INTTRASettings.Where(d => d.INTTRAAlias == this.Recipient).FirstOrDefault();

                            if (myINTTRASetting != null)
                            {
                                this.Tenant = myINTTRASetting.Tenant;
                                this.myAnalyzeQueue.Tenant = this.Tenant;
                                this.myAnalyzeQueue.Subject = this.Subject;

                                return CreateCommunicationLogForTenant(myAnalyzeQueue.From, Convert.ToInt32(myAnalyzeQueue.FileSize));
                            }

                            else
                            {
                                iMessageTenantError = "There is no Tenant for this Recipient: " + this.Recipient;
                            }
                        }

                        else
                        {
                            iMessageTenantError = "Unknown message Recipient";
                        }

                        break;
                    }
            }

            if (iMessageTenant != null)
            {
                this.Tenant = iMessageTenant.Value;
                this.myAnalyzeQueue.Tenant = this.Tenant;
                this.myAnalyzeQueue.Subject = this.Subject;
                return CreateCommunicationLogForTenant(myAnalyzeQueue.From, Convert.ToInt32(myAnalyzeQueue.FileSize));
            }

            else
            {
                throw new Exception(iMessageTenantError);
            }
        }
        private void ConnectQueueToEntity()
        {
            if (!string.IsNullOrEmpty(this.ShipmentNumber))
            {
                Shipment myShipment = iShipmentRepository.GetSingleShipmentByShipmentNumber(this.ShipmentNumber, this.Tenant);
                if (myShipment != null)
                {
                    this.ShipmentId = myShipment.Id;

                    string myObjectTableName = "Shipment";

                    string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                    CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, this.Tenant);
                    commlog.EntityId = this.ShipmentId;
                    commlog.EntityReference = this.ShipmentNumber;
                    commlog.ObjectTableId = objectTabelId;
                    //commlog.AWBNumber = myMaster;
                    myCommunicationLogRepository.Update(commlog);
                    myCommunicationLogRepository.SubmitChanges();

                    //myAnalyzeQueue.AWBNumber = myMaster;
                    myAnalyzeQueue.EntityReference = myShipment.ShipmentNumber;
                    myAnalyzeQueue.ObjectTableName = myObjectTableName;
                }

                else
                {
                    throw new ApplicationException("There is no shipment with No. : " + this.ShipmentNumber);
                }
            }

            else
            {
                throw new Exception("Unknown message Shipment number");
            }
        }
        private void AnalyzeMessageQueue()
        {
            CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, this.Tenant);
            if (commlog != null)
            {
                if (commlog.EntityId == null)
                {
                    myAnalyzeQueue.ConnectedToEntity = false;
                    throw new Exception("Analyzing shipment faild, shipment not found");
                }

                else
                {
                    this.shipmentPM = iShipmentQuery.GetSinglePM(commlog.EntityId, this.Tenant);

                    if (this.shipmentPM == null)
                    {
                        throw new Exception("Analyzing shipment faild, shipment not found");
                    }

                    else
                    {
                        this.ShipmentId = commlog.EntityId;

                        switch (this.Subject)
                        {
                            case "Status":
                                {
                                    Analyze_Status();
                                    break;
                                }

                            case "CONTRL":
                                {
                                    Analyze_CONTRL();
                                    break;
                                }

                            case "ApplicationAcknowledgment":
                                {
                                    Analyze_Acknowledgment();
                                    break;
                                }
                        }

                        commlog.CommunicationStatusTypeCode = "D";
                        commlog.DoneDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
                        commlog.DoneDateUTC = DateTime.UtcNow;
                        commlog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
                        commlog.LastStatusDateUTC = DateTime.UtcNow;
                        myCommunicationLogRepository.Update(commlog);
                        myCommunicationLogRepository.SubmitChanges();
                    }
                }
            }
        }

        private string CreateCommunicationLogForTenant(string from, int fileSize)
        {
            #region Document
            DocumentRepository documentrepository = new DocumentRepository(myCommonContext);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = this.Tenant,
                Id = IdCounter.GetNumber("Document", this.Tenant),
                HasFile = true,
                Folder = "inttra",
            };

            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            #endregion

            #region CommunicationLog

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", this.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                From = from,
                InOut = "I",
                Subject = this.Subject,
                Tenant = this.Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
            };

            myCommunicationLogRepository.Add(commLog);
            myCommunicationLogRepository.SubmitChanges();
            #endregion

            #region Blob Stream
            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = myMemoryStream.Length,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myMemoryStream.ToArray(), fileInfo);
            #endregion

            return commLog.Id;
        }
        private void OnCatchAnalyzingError(Exception ex)
        {
            myAnalyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            myAnalyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            myAnalyzeQueue.ErrorMessage = myAnalyzeQueue.ErrorMessage.Length > 7950 ? myAnalyzeQueue.ErrorMessage.Substring(0, 7950) : myAnalyzeQueue.ErrorMessage;
            myAnalyzeQueue.StackTrace = myAnalyzeQueue.StackTrace.Length > 7950 ? myAnalyzeQueue.StackTrace.Substring(0, 7950) : myAnalyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";

                    if (myAnalyzeQueue.ConnectedToTenant)
                    {
                        CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, this.Tenant);
                        if (commLog != null)
                        {
                            commLog.CommunicationStatusTypeCode = "F";
                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                            commLog.ExceptionMessage = myAnalyzeQueue.ErrorMessage;

                            if (myAnalyzeQueue.StackTrace != null)
                            {
                                commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + myAnalyzeQueue.StackTrace;
                            }

                            myCommunicationLogRepository.Update(commLog);
                            myCommunicationLogRepository.SubmitChanges();
                        }
                    }
                }
            }

            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
    }
}
