using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.Analyzers.GLSHKAnalyzer
{
    public partial class GLSHKAnalyzer
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
        private GLSHK.Message myMessage;
        private GLSHK.Envelope myEnvelope;
        private string myPrefix;
        private string myMaster;
        private string myHouse;
        private ShipmentPM shipmentPM;
        private string myObjectTableName;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext myShipmentContext;
        public GLSHKAnalyzer(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.myTenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
            }
        }

        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();

                if (myEnvelope != null)
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

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(GLSHK.Message));
                this.myMessage = (GLSHK.Message)xmlSerializer.Deserialize(myMemoryStream);
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

            if (myMessage != null)
            {
                this.myEnvelope = myMessage.Envelope;

                if (myEnvelope != null)
                {
                    this.myMessageIdentifier = myEnvelope.MsgType;
                    this.mySenderID = myEnvelope.SenderID;
                    this.myRecipientID = myEnvelope.RecipientID;
                }
            }
        }

        private void AnalyzeData()
        {
            switch (myMessageIdentifier)
            {
                case "CIMFSA":
                case "CIMFSU":
                    {
                        try
                        {
                            this.AnalyzeBaseData_FSU();
                        }

                        catch (Exception ex)
                        {
                            this.OnCatchAnalyzingError(ex);
                            break;
                        }

                        this.ConnectAnalyzeQueue();
                        break;
                    }

                case "CIMFNA":
                    {
                        if (mySenderID == "RHKAPT01HKGFMCR" || mySenderID == "RHKAPT01HKGSTCR" || mySenderID == "RHKAPT01HKGLACX")
                        {
                            try
                            {
                                ConnectAnalyzeQueue_ISAC();
                            }

                            catch (Exception ex)
                            {
                                this.OnCatchAnalyzingError(ex);
                                break;
                            }
                        }

                        else
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
                        }

                        break;
                    }

                case "CIMFMA":
                    {
                        if (mySenderID == "RHKAPT01HKGFMCR" || mySenderID == "RHKAPT01HKGSTCR" || mySenderID == "RHKAPT01HKGLACX")
                        {
                            try
                            {
                                ConnectAnalyzeQueue_ISAC();
                            }

                            catch (Exception ex)
                            {
                                this.OnCatchAnalyzingError(ex);
                                break;
                            }
                        }

                        else
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
                        }

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
                        case "CIMFNA":
                        case "CIMFMA":
                            {
                                //if (mySenderID != "REUBCSP")
                                //{
                                //    AnalyzeMessageQueue();
                                //}

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
        private void ConnectAnalyzeQueue_ISAC()
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
                        ConnectQueueToEntity_ISAC();
                        scope.Complete();
                    }

                    myAnalyzeQueue.ConnectedToEntity = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    //if (mySenderID != "REUBCSP")
                    //{
                    //    AnalyzeMessageQueue_ISAC();
                    //}

                    if (mySenderID != "BCSSYS03AWBCPY")
                    {
                        AnalyzeMessageQueue_ISAC();
                    }

                    scope.Complete();
                }

                myAnalyzeQueue.Status = "D";
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                myAnalyzeQueue.ErrorMessage = null;
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
            string myPIMA = myRecipientID;

            if (!string.IsNullOrEmpty(myPIMA))
            {
                TenantManagement tenantManagement = null;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    tenantManagement = tenantManagementRepository.GetTenantByPIMA(myPIMA);
                }

                if (tenantManagement != null)
                {
                    this.myTenant = tenantManagement.Id;
                    this.myAnalyzeQueue.Tenant = tenantManagement.Id;
                    this.myAnalyzeQueue.Subject = myMessageIdentifier;

                    return CreateCommunicationLogForTenant(myAnalyzeQueue.From, Convert.ToInt32(myAnalyzeQueue.FileSize));
                }

                else
                {
                    throw new Exception("There is no tenant for this PIMA");
                }
            }

            else
            {
                throw new Exception("There is no PIMA for this tenant");
            }
        }

        private string CreateCommunicationLogForTenant(string from, int fileSize)
        {
            if (myCommonContext == null)
            {
                myCommonContext = CommonDataContext.GetContext(myTenant);
            }

            #region Document
            DocumentRepository documentrepository = new DocumentRepository(myCommonContext);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = myTenant,
                Id = IdCounter.GetNumber("Document", myTenant),
                HasFile = true,
                Folder = "glshk",
            };

            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            #endregion

            #region CommunicationLog
            CommunicationLogRepository commLogRep = new CommunicationLogRepository(myCommonContext);

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
            };

            commLogRep.Add(commLog);
            commLogRep.SubmitChanges();
            #endregion

            #region Blob Stream
            //string filename = document.Id + "." + document.Extension;
            //var blobContainer = StorageAcountDetails.GetCurrentContainer(myTenant);
            //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

            //using (Stream blobstream = blobfile.OpenWrite())
            //{
            //    blobstream.Write(myMemoryStream.ToArray(), 0, (int)myMemoryStream.Length);
            //}

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

        private void ConnectQueueToEntity()
        {
            if (myCommonContext == null)
            {
                myCommonContext = CommonDataContext.GetContext(myTenant);
            }

            AirlineRepository airlineRepository = new AirlineRepository(myTenant);
            Airline airline = airlineRepository.GetSingleAirlineByPrefix(myPrefix, myTenant);
            if (airline != null)
            {
                this.myObjectTableName = "Shipment";

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myTenant);
                string objectTabelId = objectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                Shipment shipment = null;
                ShipmentRepository shipmentRepository = new ShipmentRepository(myTenant);

                if (isFHLType)
                {
                    shipment = shipmentRepository.GetShipmentByHouseAndAirline(myMaster, myHouse, airline.Id, myTenant);
                }

                else
                {
                    shipment = shipmentRepository.GetShipmentByMasterAndAirline(myMaster, airline.Id, myTenant);
                }

                if (shipment != null)
                {
                    CommunicationLogRepository commLogRep = new CommunicationLogRepository(myCommonContext);
                    CommunicationLog commlog = commLogRep.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                    commlog.EntityId = shipment.Id;
                    commlog.EntityReference = shipment.ShipmentNumber;
                    commlog.ObjectTableId = objectTabelId;
                    commlog.AWBNumber = myMaster;
                    commLogRep.Update(commlog);
                    commLogRep.SubmitChanges();

                    myAnalyzeQueue.EntityReference = shipment.ShipmentNumber;
                    myAnalyzeQueue.ObjectTableName = myObjectTableName;
                    myAnalyzeQueue.AWBNumber = myMaster;

                    if (myMessageIdentifier == "CIMFNA")
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = myTenant,
                            EventTypeCode = "FNAR",
                            UserId = null,
                            EntityId = shipment.Id,
                            ObjectTableName = myObjectTableName,
                        });
                    }

                    else if (myMessageIdentifier == "CIMFMA")
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = myTenant,
                            EventTypeCode = "FMAR",
                            UserId = null,
                            EntityId = shipment.Id,
                            ObjectTableName = myObjectTableName,
                        });
                    }
                }

                else
                {
                    throw new ApplicationException("There is no shipment with AWBno. : " + myMaster);
                }
            }

            else
            {
                throw new Exception("There is no airline with prefix. : " + myPrefix);
            }
        }
        private void ConnectQueueToEntity_ISAC()
        {
            string myShipmentNumber = myEnvelope.MessageRefNum;

            ShipmentRepository shipmentRepository = new ShipmentRepository(myTenant);
            Shipment shipment = shipmentRepository.GetSingleShipmentByShipmentNumber(myShipmentNumber, myTenant);

            if (shipment != null)
            {
                this.myObjectTableName = "Shipment";
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myTenant);
                string objectTabelId = objectTabelRepository.GetObjectTableByName(myObjectTableName, 0, true).Id;

                CommunicationLogRepository commLogRep = new CommunicationLogRepository(myCommonContext);
                CommunicationLog commlog = commLogRep.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
                commlog.EntityId = shipment.Id;
                commlog.EntityReference = shipment.ShipmentNumber;
                commlog.ObjectTableId = objectTabelId;
                commlog.AWBNumber = myMaster;
                commLogRep.Update(commlog);
                commLogRep.SubmitChanges();

                myAnalyzeQueue.EntityReference = shipment.ShipmentNumber;
                myAnalyzeQueue.ObjectTableName = myObjectTableName;
                myAnalyzeQueue.AWBNumber = myMaster;

                if (myMessageIdentifier == "CIMFNA")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "FNAR",
                        UserId = null,
                        EntityId = shipment.Id,
                        ObjectTableName = myObjectTableName,
                    });
                }

                else if (myMessageIdentifier == "CIMFMA")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myTenant,
                        EventTypeCode = "FMAR",
                        UserId = null,
                        EntityId = shipment.Id,
                        ObjectTableName = myObjectTableName,
                    });
                }
            }

            else
            {
                throw new ApplicationException("There is no shipment with Number : " + myShipmentNumber);
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

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";

                    if (myAnalyzeQueue.ConnectedToTenant)
                    {
                        CommunicationLogRepository commLogRep = new CommunicationLogRepository(myTenant);
                        CommunicationLog commLog = commLogRep.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);
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

                            commLogRep.Update(commLog);
                            commLogRep.SubmitChanges();
                        }
                    }
                }
            }
            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }

        private void AnalyzeMessageQueue()
        {
            if (myCommonContext == null)
            {
                myCommonContext = CommonDataContext.GetContext(myTenant);
            }

            if (myShipmentContext == null)
            {
                myShipmentContext = ShipmentsContext.GetContext(myTenant);
            }

            CommunicationLogRepository commLogRep = new CommunicationLogRepository(myCommonContext);
            CommunicationLog commlog = commLogRep.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);

            if (commlog != null)
            {
                if (commlog.EntityId == null)
                {
                    myAnalyzeQueue.ConnectedToEntity = false;
                    throw new Exception("Analyzing shipment faild, shipment not found");
                }

                else
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(myShipmentContext);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    this.shipmentPM = shipmentQuery.GetSinglePM(commlog.EntityId, myTenant);

                    if (shipmentPM == null)
                    {
                        throw new Exception("Analyzing shipment faild, shipment not found");
                    }

                    else
                    {
                        switch (myMessageIdentifier)
                        {
                            case "CIMFSA":
                            case "CIMFSU":
                                {
                                    AnalyzeMessageQueue_FSU();
                                    break;
                                }

                            case "CIMFNA":
                                {
                                    AnalyzeMessageQueue_FNA();
                                    break;
                                }


                            case "CIMFMA":
                                {
                                    AnalyzeMessageQueue_FMA();
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }

                        commlog.CommunicationStatusTypeCode = "D";
                        commlog.DoneDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                        commlog.DoneDateUTC = DateTime.UtcNow;
                        commlog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                        commlog.LastStatusDateUTC = DateTime.UtcNow;
                        commLogRep.Update(commlog);
                        commLogRep.SubmitChanges();
                    }
                }
            }
        }
        private void AnalyzeMessageQueue_ISAC()
        {
            if (myCommonContext == null)
            {
                myCommonContext = CommonDataContext.GetContext(myTenant);
            }

            if (myShipmentContext == null)
            {
                myShipmentContext = ShipmentsContext.GetContext(myTenant);
            }

            CommunicationLogRepository commLogRep = new CommunicationLogRepository(myCommonContext);
            CommunicationLog commlog = commLogRep.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, myTenant);

            if (commlog != null)
            {
                if (commlog.EntityId == null)
                {
                    myAnalyzeQueue.ConnectedToEntity = false;
                    throw new Exception("Analyzing shipment faild, shipment not found");
                }

                else
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(myShipmentContext);
                    ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                    this.shipmentPM = shipmentQuery.GetSinglePM(commlog.EntityId, myTenant);

                    if (shipmentPM == null)
                    {
                        throw new Exception("Analyzing shipment faild, shipment not found");
                    }

                    else
                    {
                        switch (myMessageIdentifier)
                        {
                            case "CIMFNA":
                                {
                                    AnalyzeMessageQueue_FNA_ISAC();
                                    break;
                                }


                            case "CIMFMA":
                                {
                                    AnalyzeMessageQueue_FMA_ISAC();
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }

                        commlog.CommunicationStatusTypeCode = "D";
                        commlog.DoneDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                        commlog.DoneDateUTC = DateTime.UtcNow;
                        commlog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                        commlog.LastStatusDateUTC = DateTime.UtcNow;
                        commLogRep.Update(commlog);
                        commLogRep.SubmitChanges();
                    }
                }
            }
        }

        private string GetCheckDigit(string serialNumberText)
        {
            string myCheckDigit = "";

            if (!string.IsNullOrEmpty(serialNumberText))
            {
                if (serialNumberText.Length == 7)
                {
                    int serialNumber = 0;
                    bool succeeded = Int32.TryParse(serialNumberText, out serialNumber);
                    if (succeeded)
                    {
                        int checkDigit = serialNumber % 7;
                        if (checkDigit >= 7)
                        {
                            checkDigit = checkDigit % 7;
                        }

                        myCheckDigit = checkDigit.ToString();
                    }
                }
            }

            return myCheckDigit;
        }
    }
}
