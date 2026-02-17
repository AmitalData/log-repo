using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
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

namespace Logitude.XSD.Analyzers.CHAMPAnalyzer
{
    public partial class CHAMPAnalyzer
    {
        private int myTenant;
        private bool isFHLType;
        private string myMessageIdentifier;
        private string myMessageDetailCode;
        private string mySenderID;
        private string myRecipientID;
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        private MemoryStream myMemoryStream;
        private CHAMP17.Envelope myEnvelope;
        private string myPrefix;
        private string myMaster;
        private string myLongMaster;
        private string myHouse;        
        private bool isTechnicalFNA;
        private string myTechnicalIdentifier;
        private bool isUsingNewCode = true;
        public CHAMPAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.myTenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
            }
        }

        private IBookingContext iBookingContext;
        private IShipmentsContext iShipmentsContext;
        private ICommonDataContext iCommonContext;
        private IWebFreightContext iWebFreightContext;
        private BookingRepository myBookingRepository;
        private ShipmentRepository myShipmentRepository;
        private PortRepository myPortRepository;
        private AirlineRepository myAirlineRepository;
        private ObjectTableRepository myObjectTabelRepository;
        private CommunicationLogRepository myCommunicationLogRepository;
        private FlightsSchedulesRequestRepository myFlightsSchedulesRequestRepository;
        private FlightsSchedulesRequest myFlightsSchedulesRequest;
        private List<Airline> AllPrefixAirlines;
        private void InitializeComponent()
        {
            iBookingContext = BookingContext.GetContext(myTenant);
            iShipmentsContext = ShipmentsContext.GetContext(myTenant);
            iCommonContext = CommonDataContext.GetContext(myTenant);
            iWebFreightContext = WebFreightContext.GetContext(myTenant);
            myBookingRepository = new BookingRepository(iBookingContext);
            myShipmentRepository = new ShipmentRepository(iShipmentsContext);
            myPortRepository = new PortRepository(iCommonContext);
            myAirlineRepository = new AirlineRepository(iCommonContext);
            myObjectTabelRepository = new ObjectTableRepository(iWebFreightContext);
            myCommunicationLogRepository = new CommunicationLogRepository(iCommonContext);
            AllPrefixAirlines = new List<Airline>();
        }

        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();

                if (myEnvelope != null)
                {
                    this.InitializeComponent();
                    this.AnalyzeData();
                }
            }
        }
        private void Deserialize()
        {
            XmlDocument xmlDocument = null;

            try
            {
                int index = 0;
                while (index < myAnalyzeQueue.MessageBody.Length)
                {
                    int d = myAnalyzeQueue.MessageBody[index];
                    if (d == 60)
                    {
                        myAnalyzeQueue.MessageBody = myAnalyzeQueue.MessageBody.Skip(index).ToArray();
                        myAnalyzeQueue.FileSize = myAnalyzeQueue.FileSize - index;
                        break;
                    }

                    index++;
                }

                this.myMemoryStream = new MemoryStream(myAnalyzeQueue.MessageBody);
                xmlDocument = new XmlDocument();
                xmlDocument.Load(myMemoryStream);
                myMemoryStream.Position = 0;

                this.GetMessageIdentifier(xmlDocument);
                this.AppendingCDATACheck(xmlDocument);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(CHAMP17.Envelope));
                this.myEnvelope = (CHAMP17.Envelope)xmlSerializer.Deserialize(myMemoryStream);
            }

            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "Xml Document Load failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }

            if (myEnvelope != null)
            {
                this.mySenderID = myEnvelope.Sender;
                this.myRecipientID = myEnvelope.Recipient;
            }
        }
        private void GetMessageIdentifier(XmlDocument xmlDocument)
        {
            XmlNodeList xnList = xmlDocument.GetElementsByTagName("StandardMessageIdentification");

            foreach (XmlNode node in xnList)
            {
                if (node.Name == "StandardMessageIdentification")
                {
                    this.myMessageIdentifier = node.Attributes["StandardMessageIdentifier"].Value;
                    break;
                }
            }
        }
        private void AppendingCDATACheck(XmlDocument xmlDocument)
        {
            if (myMessageIdentifier == "FNA")
            {
                string innerXml = xmlDocument.InnerXml;

                if (!string.IsNullOrEmpty(innerXml))
                {
                    string myElement = "<ReceivedMessageDetail>";
                    if (innerXml.Contains(myElement))
                    {
                        int indexOfElement = innerXml.IndexOf(myElement);
                        string nextChar = innerXml.Substring(indexOfElement + myElement.Length, 1);
                        if (nextChar == "<")
                        {
                            isTechnicalFNA = true;

                            int indexOfStart = innerXml.IndexOf("<ReceivedMessageDetail>");
                            innerXml = innerXml.Insert(indexOfStart + "<ReceivedMessageDetail>".Length, "<![CDATA[");

                            int indexOfEnd = innerXml.IndexOf("</ReceivedMessageDetail>");
                            innerXml = innerXml.Insert(indexOfEnd, "]]>");

                            byte[] myMessageBody = Encoding.ASCII.GetBytes(innerXml);

                            int index = 0;
                            while (index < myMessageBody.Length)
                            {
                                int d = myMessageBody[index];
                                if (d == 60)
                                {
                                    myMessageBody = myMessageBody.Skip(index).ToArray();
                                    break;
                                }

                                index++;
                            }

                            this.myMemoryStream = new MemoryStream(myMessageBody);
                            xmlDocument.Load(myMemoryStream);
                            myMemoryStream.Position = 0;
                        }
                    }
                }
            }
        }
        private void AnalyzeData()
        {
            switch (myMessageIdentifier)
            {
                case "FSA":
                case "FSU":
                    {
                        try
                        {
                            this.AnalyzeBaseData_FSA();
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        break;
                    }

                case "FNA":
                    {
                        try
                        {
                            this.AnalyzeBaseData_FNA();
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        break;
                    }

                case "FMA":
                    {
                        try
                        {
                            this.AnalyzeBaseData_FMA();
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        break;
                    }

                case "FFA":
                    {
                        try
                        {
                            this.AnalyzeBaseData_FFA();
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        break;
                    }

                case "FVA":
                    {
                        try
                        {
                            this.AnalyzeBaseData_FVA();
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        break;
                    }

                default:
                    {
                        if (myMessageIdentifier == null)
                        {
                            myMessageIdentifier = "";
                        }

                        myAnalyzeQueue.Status = "F";
                        myAnalyzeQueue.ErrorMessage = "The message type is not known! " + myMessageIdentifier;
                        myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                        analyzeQueueRepository.Update(myAnalyzeQueue);
                        analyzeQueueRepository.SubmitChanges();
                        break;
                    }
            }
        }

        private void ConnectAnalyzeQueue()
        {

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
                    switch (myMessageIdentifier)
                    {
                        case "FNA":
                        case "FMA":
                            {
                                if (mySenderID != "BCSSYS03AWBCPY")
                                {
                                    AnalyzeMessageQueue();
                                }

                                break;
                            }

                        default:
                            {
                                AnalyzeMessageQueue();
                                break;
                            }
                    }

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
            string myTTY = myRecipientID;

            if (!string.IsNullOrEmpty(myTTY))
            {
                TenantManagement tenantManagement = null;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    tenantManagement = tenantManagementRepository.GetTenantByTTY(myTTY);
                    scope.Complete();
                }

                if (tenantManagement != null)
                {
                    this.myTenant = tenantManagement.Id;
                    this.myAnalyzeQueue.Tenant = tenantManagement.Id;
                    this.myAnalyzeQueue.Subject = myMessageIdentifier;

                    if (myMessageIdentifier == "FNA" && isTechnicalFNA)
                    {
                        this.myAnalyzeQueue.Subject = "Technical FNA";
                    }

                    return CreateCommunicationLogForTenant(myAnalyzeQueue.From, Convert.ToInt32(myAnalyzeQueue.FileSize));
                }

                else
                {
                    throw new Exception("There is no tenant for this TTY: " + myTTY);
                }
            }

            else
            {
                throw new Exception("There is no TTY for this tenant");
            }
        }
        private string CreateCommunicationLogForTenant(string from, int fileSize)
        {
            #region Document
            DocumentRepository documentrepository = new DocumentRepository(iCommonContext);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = myTenant,
                Id = IdCounter.GetNumber("Document", myTenant),
                HasFile = true,
                Folder = "champ",
            };

            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            #endregion

            #region CommunicationLog

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", myTenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                From = from,
                InOut = "I",
                Subject = myMessageIdentifier,
                Tenant = myTenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
                AWBNumber = this.GetAWBNumber(),
            };

            if (myMessageIdentifier == "FNA" && isTechnicalFNA)
            {
                commLog.Subject = "Technical FNA";
            }

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

            //string filename = document.Id + "." + document.Extension;
            //var blobContainer = StorageAcountDetails.GetCurrentContainer(myTenant);
            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

            //using (Stream blobstream = blobfile.OpenWrite())
            //{
            //    blobstream.Write(myMemoryStream.ToArray(), 0, (int)myMemoryStream.Length);
            //}
            #endregion

            return commLog.Id;
        }
        private void ConnectQueueToEntity()
        {
            if (!string.IsNullOrEmpty(myPrefix))
            {
                this.AllPrefixAirlines = myAirlineRepository.GetAllAirlinesByPrefix(myPrefix, myTenant).ToList();
            }

            switch (myMessageIdentifier)
            {
                case "FSU":
                case "FMA":
                    {
                        #region

                        bool analyzeBookingFirst = false;

                        Booking myBooking = this.GetBookingByPrefix();

                        if (myBooking != null)
                        {
                            if (myBooking.FFRStatusCode == "BRQ" || myBooking.FFRStatusCode == "CFM" || myBooking.FFRStatusCode == "RBA")
                            {
                                analyzeBookingFirst = true;
                            }
                        }

                        if (analyzeBookingFirst)
                        {
                            string myObjectTableName = "Booking";

                            string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                            CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                            commlog.EntityId = myBooking.Id;
                            commlog.EntityReference = myBooking.BookingNumber;
                            commlog.ObjectTableId = objectTabelId;
                            commlog.AWBNumber = this.GetAWBNumber();
                            myCommunicationLogRepository.Update(commlog);
                            myCommunicationLogRepository.SubmitChanges();

                            myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                            myAnalyzeQueue.EntityReference = myBooking.BookingNumber;
                            myAnalyzeQueue.ObjectTableName = myObjectTableName;
                        }

                        else
                        {
                            Shipment myShipment = this.GetShipmentByPrefix(); ;

                            if (myShipment != null)
                            {
                                string myObjectTableName = "Shipment";

                                string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                                CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                                commlog.EntityId = myShipment.Id;
                                commlog.EntityReference = myShipment.ShipmentNumber;
                                commlog.ObjectTableId = objectTabelId;
                                commlog.AWBNumber = this.GetAWBNumber();
                                myCommunicationLogRepository.Update(commlog);
                                myCommunicationLogRepository.SubmitChanges();

                                myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                                myAnalyzeQueue.EntityReference = myShipment.ShipmentNumber;
                                myAnalyzeQueue.ObjectTableName = myObjectTableName;

                                if (myMessageIdentifier == "FNA")
                                {
                                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                                    {
                                        Tenant = myTenant,
                                        EventTypeCode = "FNAR",
                                        UserId = null,
                                        EntityId = myShipment.Id,
                                        ObjectTableName = myObjectTableName,
                                    });
                                }

                                else if (myMessageIdentifier == "FMA")
                                {
                                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                                    {
                                        Tenant = myTenant,
                                        EventTypeCode = "FMAR",
                                        UserId = null,
                                        EntityId = myShipment.Id,
                                        ObjectTableName = myObjectTableName,
                                    });
                                }
                            }

                            else
                            {
                                if (myBooking != null)
                                {
                                    string myObjectTableName = "Booking";

                                    string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                                    CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                                    commlog.EntityId = myBooking.Id;
                                    commlog.EntityReference = myBooking.BookingNumber;
                                    commlog.ObjectTableId = objectTabelId;
                                    commlog.AWBNumber = this.GetAWBNumber();
                                    myCommunicationLogRepository.Update(commlog);
                                    myCommunicationLogRepository.SubmitChanges();

                                    myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                                    myAnalyzeQueue.EntityReference = myBooking.BookingNumber;
                                    myAnalyzeQueue.ObjectTableName = myObjectTableName;
                                }

                                else
                                {
                                    throw new ApplicationException("There is no shipment or booking with AWBno. : " + myMaster);
                                }
                            }
                        }

                        #endregion

                        break;
                    }

                case "FSA":
                    {
                        #region

                        Shipment myShipment = this.GetShipmentByPrefix(); ;

                        if (myShipment != null)
                        {
                            string myObjectTableName = "Shipment";

                            string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                            CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                            commlog.EntityId = myShipment.Id;
                            commlog.EntityReference = myShipment.ShipmentNumber;
                            commlog.ObjectTableId = objectTabelId;
                            commlog.AWBNumber = this.GetAWBNumber();
                            myCommunicationLogRepository.Update(commlog);
                            myCommunicationLogRepository.SubmitChanges();

                            myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                            myAnalyzeQueue.EntityReference = myShipment.ShipmentNumber;
                            myAnalyzeQueue.ObjectTableName = myObjectTableName;
                        }

                        else
                        {
                            Booking myBooking = this.GetBookingByPrefix();

                            if (myBooking != null)
                            {
                                string myObjectTableName = "Booking";

                                string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                                CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                                commlog.EntityId = myBooking.Id;
                                commlog.EntityReference = myBooking.BookingNumber;
                                commlog.ObjectTableId = objectTabelId;
                                commlog.AWBNumber = this.GetAWBNumber();
                                myCommunicationLogRepository.Update(commlog);
                                myCommunicationLogRepository.SubmitChanges();

                                myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                                myAnalyzeQueue.EntityReference = myBooking.BookingNumber;
                                myAnalyzeQueue.ObjectTableName = myObjectTableName;
                            }

                            else
                            {
                                throw new ApplicationException("There is no shipment or booking with AWBno. : " + myMaster);
                            }
                        }

                        #endregion

                        break;
                    }

                case "FNA":
                    {
                        if (myMessageIdentifier == "FNA" && isTechnicalFNA && myTechnicalIdentifier == "FVR")
                        {
                            this.ConnectFVREntity();
                        }

                        else if (myMessageIdentifier == "FNA" && myMessageDetailCode == "FVR")
                        {
                            this.ConnectFVREntity();
                        }

                        else
                        {
                            #region

                            Shipment myShipment = this.GetShipmentByPrefix(); ;

                            if (myShipment != null)
                            {
                                string myObjectTableName = "Shipment";

                                string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                                CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                                commlog.EntityId = myShipment.Id;
                                commlog.EntityReference = myShipment.ShipmentNumber;
                                commlog.ObjectTableId = objectTabelId;
                                commlog.AWBNumber = this.GetAWBNumber();
                                myCommunicationLogRepository.Update(commlog);
                                myCommunicationLogRepository.SubmitChanges();

                                myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                                myAnalyzeQueue.EntityReference = myShipment.ShipmentNumber;
                                myAnalyzeQueue.ObjectTableName = myObjectTableName;

                                if (myMessageIdentifier == "FNA")
                                {
                                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                                    {
                                        Tenant = myTenant,
                                        EventTypeCode = "FNAR",
                                        UserId = null,
                                        EntityId = myShipment.Id,
                                        ObjectTableName = myObjectTableName,
                                    });
                                }

                                else if (myMessageIdentifier == "FMA")
                                {
                                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                                    {
                                        Tenant = myTenant,
                                        EventTypeCode = "FMAR",
                                        UserId = null,
                                        EntityId = myShipment.Id,
                                        ObjectTableName = myObjectTableName,
                                    });
                                }
                            }

                            else
                            {
                                Booking myBooking = this.GetBookingByPrefix();

                                if (myBooking != null)
                                {
                                    string myObjectTableName = "Booking";

                                    string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                                    CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                                    commlog.EntityId = myBooking.Id;
                                    commlog.EntityReference = myBooking.BookingNumber;
                                    commlog.ObjectTableId = objectTabelId;
                                    commlog.AWBNumber = this.GetAWBNumber();
                                    myCommunicationLogRepository.Update(commlog);
                                    myCommunicationLogRepository.SubmitChanges();

                                    myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                                    myAnalyzeQueue.EntityReference = myBooking.BookingNumber;
                                    myAnalyzeQueue.ObjectTableName = myObjectTableName;
                                }

                                else
                                {
                                    throw new ApplicationException("There is no shipment or booking with AWBno. : " + myMaster);
                                }
                            }
                        }
                        #endregion

                        break;
                    }

                case "FFA":
                    {
                        #region

                        Booking myBooking = this.GetBookingByPrefix();

                        if (myBooking != null)
                        {
                            string myObjectTableName = "Booking";

                            string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                            CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                            commlog.EntityId = myBooking.Id;
                            commlog.EntityReference = myBooking.BookingNumber;
                            commlog.ObjectTableId = objectTabelId;
                            commlog.AWBNumber = this.GetAWBNumber();
                            myCommunicationLogRepository.Update(commlog);
                            myCommunicationLogRepository.SubmitChanges();

                            myAnalyzeQueue.EntityReference = myBooking.BookingNumber;
                            myAnalyzeQueue.ObjectTableName = myObjectTableName;
                            myAnalyzeQueue.AWBNumber = commlog.AWBNumber;
                        }

                        else
                        {
                            throw new ApplicationException("There is no booking with AWBno. : " + myMaster);
                        }

                        #endregion

                        break;
                    }

                case "FVA":
                    {
                        this.ConnectFVREntity();
                        break;
                    }
            }
        }

        private Booking GetBookingByPrefix()
        {
            Booking iResult = null;

            if (this.AllPrefixAirlines.Count == 0)
            {
                throw new Exception("There is no airline with prefix. : " + myPrefix);
            }

            else
            {
                foreach (Airline item in this.AllPrefixAirlines)
                {
                    iResult = myBookingRepository.GetBookingByMasterAndAirline(myMaster, item.Id, myTenant);

                    if (iResult != null)
                    {
                        break;
                    }
                }
            }

            return iResult;
        }
        private Shipment GetShipmentByPrefix()
        {
            Shipment iResult = null;

            if (this.AllPrefixAirlines.Count == 0)
            {
                throw new Exception("There is no airline with prefix. : " + myPrefix);
            }

            else
            {
                foreach (Airline item in this.AllPrefixAirlines)
                {
                    if (isFHLType)
                    {
                        iResult = myShipmentRepository.GetShipmentByHouseAndAirline(myMaster, myHouse, item.Id, myTenant);
                    }

                    else
                    {
                        iResult = myShipmentRepository.GetShipmentByMasterAndAirline(myMaster, item.Id, myTenant);
                    }

                    if (iResult != null)
                    {
                        break;
                    }
                }
            }

            return iResult;
        }

        private void ConnectFVREntity()
        {            
            if (myFlightsSchedulesRequestRepository == null)
            {
                myFlightsSchedulesRequestRepository = new FlightsSchedulesRequestRepository(this.iBookingContext);
            }

            if (myFlightsSchedulesRequest == null)
            {
                myFlightsSchedulesRequest = myFlightsSchedulesRequestRepository.GetFlightsSchedulesRequestByDetails(myRequestDetails, myTenant);
            }

            if (myFlightsSchedulesRequest != null)
            {
                string myObjectTableName = "FlightsSchedulesRequest";

                string objectTabelId = myObjectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                commlog.EntityId = myFlightsSchedulesRequest.Id;
                commlog.EntityReference = myFlightsSchedulesRequest.Id;
                commlog.ObjectTableId = objectTabelId;
                myCommunicationLogRepository.Update(commlog);
                myCommunicationLogRepository.SubmitChanges();

                myAnalyzeQueue.EntityReference = myFlightsSchedulesRequest.Id;
                myAnalyzeQueue.ObjectTableName = myObjectTableName;
            }

            else
            {
                throw new ApplicationException("There is no Schedules Request with same Details");
            }
        }

        private void AnalyzeMessageQueue()
        {
            CommunicationLog commlog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);

            if (commlog != null)
            {
                string myEntityId = commlog.EntityId;

                if (myEntityId == null)
                {
                    myAnalyzeQueue.ConnectedToEntity = false;

                    switch (myMessageIdentifier)
                    {
                        case "FSA":
                        case "FSU":
                        case "FNA":
                        case "FMA":
                            {
                                if (myMessageIdentifier == "FNA" && isTechnicalFNA && myTechnicalIdentifier == "FVR")
                                {
                                    throw new Exception("Analyzing Schedule Request faild, Schedule Request not found");
                                }

                                else
                                {
                                    throw new Exception("Analyzing shipment or booking faild, shipment or booking not found");
                                }
                            }

                        case "FFA":
                            {
                                throw new Exception("Analyzing booking faild, booking not found");
                            }

                        case "FVA":
                            {
                                throw new Exception("Analyzing Schedule Request faild, Schedule Request not found");
                            }

                        default:
                            {
                                throw new Exception("Analyzing entity faild, entity not found");
                            }
                    }
                }

                else
                {
                    bool isUpdatingCommunicationLog = true;
                    bool isUpdatingAnalyzeQueue = true;

                    switch (myMessageIdentifier)
                    {
                        case "FSA":
                        case "FSU":
                        case "FNA":
                        case "FMA":
                            {
                                #region

                                ObjectTable table = ObjectTableRepository.GetSingleObjectTableById(commlog.ObjectTableId, commlog.Tenant);

                                if (table != null)
                                {
                                    if (table.Name == "FlightsSchedulesRequest")
                                    {
                                        if (myMessageIdentifier == "FNA")
                                        {
                                            if (this.iBookingContext == null)
                                            {
                                                this.iBookingContext = BookingContext.GetContext(myTenant);
                                            }

                                            if (myFlightsSchedulesRequestRepository == null)
                                            {
                                                myFlightsSchedulesRequestRepository = new FlightsSchedulesRequestRepository(this.iBookingContext);
                                            }

                                            if (myFlightsSchedulesRequest == null)
                                            {
                                                myFlightsSchedulesRequest = myFlightsSchedulesRequestRepository.GetFlightsSchedulesRequestByDetails(myRequestDetails, myTenant);
                                            }

                                            if (myFlightsSchedulesRequest != null)
                                            {
                                                AnalyzeMessageQueue_FNA(myFlightsSchedulesRequest, myFlightsSchedulesRequestRepository);
                                            }
                                        }
                                    }

                                    else if (table.Name == "Shipment")
                                    {
                                        ShipmentRepository shipmentRepository = new ShipmentRepository(this.iShipmentsContext);
                                        ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                                        ShipmentPM myShipmentPM = shipmentQuery.GetSinglePM(myEntityId, myTenant);

                                        if (myShipmentPM != null)
                                        {
                                            switch (myMessageIdentifier)
                                            {
                                                case "FSA":
                                                case "FSU":
                                                    {
                                                        if (this.isUsingNewCode)
                                                        {
                                                            AnalyzeMessageQueue_FSA_Shipment(myShipmentPM);
                                                        }

                                                        else
                                                        {
                                                            AnalyzeMessageQueue_FSA(myShipmentPM, this.iShipmentsContext);
                                                        }

                                                        break;
                                                    }

                                                case "FNA":
                                                    {
                                                        AnalyzeMessageQueue_FNA(myShipmentPM, this.iShipmentsContext);
                                                        break;
                                                    }

                                                case "FMA":
                                                    {
                                                        AnalyzeMessageQueue_FMA(myShipmentPM, this.iShipmentsContext);
                                                        break;
                                                    }
                                            }
                                        }
                                    }

                                    else if (table.Name == "Booking")
                                    {
                                        if (this.iBookingContext == null)
                                        {
                                            this.iBookingContext = BookingContext.GetContext(myTenant);
                                        }

                                        BookingQueryService bookingQueryService = new BookingQueryService(this.iBookingContext);
                                        BookingPM myBookingPM = bookingQueryService.GetSingle(myEntityId, true, false);
                                        if (myBookingPM != null)
                                        {
                                            switch (myMessageIdentifier)
                                            {
                                                case "FSA":
                                                case "FSU":
                                                    {
                                                        AnalyzeMessageQueue_FSA(myBookingPM, this.iBookingContext, myAnalyzeQueue);
                                                        break;
                                                    }

                                                case "FNA":
                                                    {
                                                        AnalyzeMessageQueue_FNA(myBookingPM, this.iBookingContext);
                                                        break;
                                                    }

                                                case "FMA":
                                                    {
                                                        AnalyzeMessageQueue_FMA(myBookingPM, this.iBookingContext);
                                                        break;
                                                    }
                                            }
                                        }

                                        else
                                        {
                                            isUpdatingCommunicationLog = false;
                                            throw new Exception("Analyzing shipment or booking faild, shipment or booking not found");
                                        }
                                    }
                                }
                                #endregion

                                break;
                            }

                        case "FFA":
                            {
                                isUpdatingAnalyzeQueue = false;

                                if (this.iBookingContext == null)
                                {
                                    this.iBookingContext = BookingContext.GetContext(myTenant);
                                }

                                BookingQueryService bookingQueryService = new BookingQueryService(this.iBookingContext);
                                BookingPM myBookingPM = bookingQueryService.GetSingle(myEntityId, true, false);
                                if (myBookingPM != null)
                                {
                                    AnalyzeMessageQueue_FFA(myBookingPM, this.iBookingContext, myAnalyzeQueue);
                                }

                                else
                                {
                                    isUpdatingCommunicationLog = false;
                                    throw new Exception("Analyzing booking faild, booking not found");
                                }

                                break;
                            }

                        case "FVA":
                            {
                                if (this.iBookingContext == null)
                                {
                                    this.iBookingContext = BookingContext.GetContext(myTenant);
                                }

                                if (myFlightsSchedulesRequestRepository == null)
                                {
                                    myFlightsSchedulesRequestRepository = new FlightsSchedulesRequestRepository(this.iBookingContext);
                                }

                                if (myFlightsSchedulesRequest == null)
                                {
                                    myFlightsSchedulesRequest = myFlightsSchedulesRequestRepository.GetFlightsSchedulesRequestByDetails(myRequestDetails, myTenant);
                                }

                                if (myFlightsSchedulesRequest != null)
                                {
                                    AnalyzeMessageQueue_FVA(myFlightsSchedulesRequest, myFlightsSchedulesRequestRepository);
                                }

                                else
                                {
                                    isUpdatingCommunicationLog = false;
                                    isUpdatingAnalyzeQueue = false;
                                    throw new Exception("Analyzing Schedules Request faild, Schedules Request not found");
                                }

                                break;
                            }
                    }

                    if (isUpdatingCommunicationLog)
                    {
                        commlog.CommunicationStatusTypeCode = "D";
                        commlog.DoneDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                        commlog.DoneDateUTC = DateTime.UtcNow;
                        commlog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                        commlog.LastStatusDateUTC = DateTime.UtcNow;
                        myCommunicationLogRepository.Update(commlog);
                        myCommunicationLogRepository.SubmitChanges();
                    }

                    //if (isUpdatingAnalyzeQueue)
                    //{
                    //    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //    {
                    //        analyzeQueueRepository.Update(myAnalyzeQueue);
                    //        analyzeQueueRepository.SubmitChanges();

                    //        scope.Complete();
                    //    }
                    //}
                }
            }
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

            else if (ex.Message.StartsWith("There is no airline with prefix"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else if (ex.Message.StartsWith("There is no shipment"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else if (ex.Message.StartsWith("There is no tenant for this TTY") || ex.Message.StartsWith("There is no TTY for this tenant"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";
                }
            }

            if (myAnalyzeQueue.Status == "F")
            {
                if (myAnalyzeQueue.ConnectedToTenant && myAnalyzeQueue.CommunicationLogId != null)
                {
                    CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                    if (commLog != null)
                    {
                        commLog.CommunicationStatusTypeCode = "F";
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
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

            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }
        private string GetAWBNumber()
        {
            if (!string.IsNullOrEmpty(this.myLongMaster))
            {
                return this.myLongMaster;
            }

            else
            {
                return this.myMaster;
            }
        }
    }
}
