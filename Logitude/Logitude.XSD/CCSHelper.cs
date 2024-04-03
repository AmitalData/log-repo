using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
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
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD
{
    public class CCSHelper
    {
        #region Members
        public bool IsFHL { get; set; }
        public int Tenant { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string Recipient { get; set; }
        public string FWBStockCode { get; set; }
        public string FHLStockCode { get; set; }
        public bool IsValid { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public bool IsCargonautEnabled { get; set; }
        public bool IsCargonautSending { get; set; }
        public bool IsDEXXEnabled { get; set; }
        public bool IsDEXXSending { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public string LoggedContactId { get; set; }
        public DateTime TodayDate { get; set; }
        public DateTime TodayDateTime { get; set; }
        public string ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
        public Shipment FHLMasterShipment { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        public CCSResult Result { get; set; }
        public string AWBMessagesCCSTypeCode { get; set; }
        public bool isMultiHS { get; set; }
        #endregion

        #region Private Members
        private IShipmentsContext shipmentContext;
        private ShipmentRepository shipmentRepository;
        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        private MessagingStockRepository stockRepository;
        private MessagingStockUsageHistoryRepository usageHistoryRepository;
        private ShipmentCarrierStatusRepository shipmentCarrierStatusRepository;
        private ICommonDataContext commonContext;
        private DocumentRepository documentRepository;
        private CommunicationLogRepository communicationLogRepository;
        #endregion

        public CCSHelper(string myShipmentId, int myTenant, string myRecipient, bool isCargonautSending, bool isDEXXSending, bool isMultiHS = false)
        {
            this.Tenant = myTenant;
            this.ShipmentId = myShipmentId;
            this.Recipient = myRecipient;
            this.IsDEXXSending = isDEXXSending;
            this.IsCargonautSending = isCargonautSending;
            this.IsValid = true;
            this.TodayDate = TenantServerConfigration.GetCurrentDateTime(myTenant).Date;
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(myTenant);
            this.isMultiHS = isMultiHS;

            this.Result = new CCSResult()
            {
                Id = Tenant,
                SendingCount = 1,
                IsValid = true,
                HasStockError = false,
                IsUpgradingChamp = false,
            };

            this.GetStockTypeCodes();
            this.GetGlobalVariables();

            if (this.IsValid)
            {
                this.GetShipmentObjects();
                this.CheckStockValidity();
                this.Validate();
            }

            if (this.IsValid)
            {
                if (IsCargonautEnabled && IsCargonautSending)
                {
                    // TTY (Old)
                    //this.Recipient = "REUCGNP";

                    // PIMA (New)
                    this.Recipient = "CGNCCS88CGN";   
                }

                else if (IsDEXXEnabled && IsDEXXSending)
                {
                    // TTY (Old)
                    //this.Recipient = "REUBCSP";

                    // PIMA (New)
                    this.Recipient = "BCSSYS03AWBCPY";
                }

                commonContext = CommonDataContext.GetContext(Tenant);
                documentRepository = new DocumentRepository(commonContext);
                communicationLogRepository = new CommunicationLogRepository(commonContext);

                ContactRepository contactRepository = new ContactRepository(commonContext);
                Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
                LoggedContactId = loggedContact.Id;
            }
        }     

        private void GetStockTypeCodes()
        {
            string myFHLCode = "FHL";
            string myFWBCode = "FWB";

            if (IsDEXXSending)
            {
                myFHLCode = "FHL DEXX";
                myFWBCode = "FWB DEXX";
            }

            else if (IsCargonautSending)
            {
                myFHLCode = "FHL Cargonaut";
                myFWBCode = "FWB Cargonaut";
            }

            this.FHLStockCode = myFHLCode;
            this.FWBStockCode = myFWBCode;
        }
        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (Tenant != 290)
                {
                    SettingRepository settingRepository = new SettingRepository();
                    Setting setting = settingRepository.GetSingleSetting("1");
                    if (setting != null)
                    {
                        if (setting.IsUpgradingChamp)
                        {
                            this.IsValid = false;
                            this.Result.IsValid = false;
                            this.Result.IsUpgradingChamp = setting.IsUpgradingChamp;
                        }
                    }
                }

                if (this.IsValid)
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(Tenant);
                    SettingRepository mySettingRepository = new SettingRepository();
                    var isDemoTenant = mySettingRepository.IsDemoTenant(Tenant.ToString());

                    if (tenantManagement != null)
                    {
                        TTY = tenantManagement.TTY;
                        PIMA = tenantManagement.PIMA;
                        IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid;
                        IsCargonautEnabled = tenantManagement.IsCargonautEnabled;
                        IsDEXXEnabled = tenantManagement.IsDEXXConnectionEnabled;
                        AWBMessagesCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;
                        IsEAWBOnlyDemo = tenantManagement.IsEAWBOnlyDemo;
                    }

                    if (isDemoTenant || IsEAWBOnlyDemo)
                    {
                        this.IsDemoTenant = true;
                        this.Result.IsDemoTenant = true;
                    }
                }

                scope.Complete();
            }
        }
        private void GetShipmentObjects()
        {
            if (shipmentContext == null)
            {
                shipmentContext = ShipmentsContext.GetContext(Tenant);
            }

            this.shipmentRepository = new ShipmentRepository(shipmentContext);
            this.shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentContext);
            this.Shipment = shipmentRepository.GetSingleShipment(ShipmentId, Tenant);
            this.MasterData = shipmentMasterDataRepository.GetSingleMasterData(Shipment.MasterShipmentDataId);

            if (Shipment.ShipmentLevelCode == "H")
            {
                this.IsFHL = true;
                this.FHLMasterShipment = shipmentRepository.GetSingleShipment(MasterData.Id, Tenant);
            }

            if (IsAWBStockPrepaid)
            {
                this.stockRepository = new MessagingStockRepository(shipmentContext);
                this.usageHistoryRepository = new MessagingStockUsageHistoryRepository(shipmentContext);
            }
        }
        private void CheckStockValidity()
        {
            if (!IsDemoTenant)
            {
                if (IsAWBStockPrepaid)
                {
                    IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(Tenant, "Champ");
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(Tenant);

                    myStocksData = myStocksData.Where(d => d.StartDate <= TodayDate && d.EndDate > TodayDate && d.Remaining > 0 && !d.IsCancelled);

                    int sendingCount = 0;
                    int? myStocksRemaining = 0;
                    if (myStocksData.Count() > 0)
                    {
                        myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                    }

                    if (Shipment.ShipmentLevelCode == "H")
                    {
                        if (!myUsageHistoryData.Where(d => d.EntityId == ShipmentId && d.MessageType == FHLStockCode).Any())
                        {
                            sendingCount = 1;
                        }
                    }

                    else
                    {
                        if (Shipment.BookingId != null)
                        {
                            if (!myUsageHistoryData.Where(d => d.EntityId == Shipment.BookingId && d.MessageType == "FFR").Any())
                            {
                                if (!myUsageHistoryData.Where(d => d.EntityId == ShipmentId && d.MessageType == FWBStockCode).Any())
                                {
                                    sendingCount = 1;
                                }
                            }
                        }

                        else
                        {
                            if (!myUsageHistoryData.Where(d => d.EntityId == ShipmentId && d.MessageType == FWBStockCode).Any())
                            {
                                sendingCount = 1;
                            }
                        }
                    }

                    this.Result.SendingCount = sendingCount;
                    this.Result.StockRemainingBefore = myStocksRemaining == null ? 0 : myStocksRemaining.Value;

                    if (sendingCount > 0)
                    {
                        if (myStocksRemaining < sendingCount)
                        {
                            this.IsValid = false;
                            this.Result.IsValid = false;
                            this.Result.HasStockError = true;
                        }
                    }
                }
            }
        }        
        private void Validate()
        {
            this.CheckMasterField();
        }
        private void CheckMasterField()
        {
            if (string.IsNullOrEmpty(MasterData.Master))
            {
                this.IsValid = false;
                this.Result.IsValid = false;
                this.Result.IsMasterFieldMissing = true;
            }
        }

        public void Run()
        {
            if (this.IsValid)
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (this.IsFHL)
                    {
                        #region FHL

                        FHLDataContext dataContext = new FHLDataContext(this.Shipment, this.MasterData, this.FHLMasterShipment);
                        FHLDataBuilder dataBuilder = new FHLDataBuilder(dataContext);

                        if (this.AWBMessagesCCSTypeCode == "CHAMP")
                        {
                            CHAMP17.Envelope envelop = new CHAMP17.Envelope()
                            {
                                Sender = this.TTY,
                                Recipient = this.Recipient,
                                Item = dataBuilder.GetChampFHL5(isMultiHS),
                            };

                            this.SendXMLFile(envelop, "champmessageoutqueue");
                            this.UpdateStock();
                        }

                        else if (this.AWBMessagesCCSTypeCode == "GLSHK")
                        {
                            #region GLSHK

                            //string myRefID = "HMF" + dataContext.Master.Substring(0, 7) + "X" + dataContext.AirlinePrefix;
                            string myFullRefID = "HMF" + dataContext.Master + "X" + dataContext.AirlinePrefix;

                            string myShipmentNumber = dataContext.ShipmentNumber;
                            if (myShipmentNumber != null)
                            {
                                if (myShipmentNumber.Length > 14)
                                {
                                    myShipmentNumber = myShipmentNumber.Substring(0, 14);
                                }
                            }

                            GLSHK.Message message = new GLSHK.Message()
                            {
                                Envelope = new GLSHK.Envelope()
                                {
                                    SenderID = this.PIMA,
                                    RecipientID = this.Recipient,
                                    MsgFormat = "XML",
                                    MsgType = "CIMFHL",
                                    Version = 5,
                                    MsgDateTime = this.TodayDateTime,
                                    RefID = myFullRefID,
                                    MessageRefNum = myShipmentNumber,
                                    InterchangeControlRef = myShipmentNumber,
                                    Item = this.Tenant.ToString(),
                                    ItemElementName = GLSHK.ItemChoiceType4.CompanyID,
                                },

                                Item = dataBuilder.GetGLSHKFHL(),
                                version = GLSHK.MessageVersion.Item20,
                            };

                            if (dataContext.IsViaColoader && !string.IsNullOrEmpty(dataContext.ColoaderKey))
                            {
                                message.Envelope.Item = dataContext.ColoaderKey;
                                message.Envelope.ItemElementName = GLSHK.ItemChoiceType4.ColoaderKey;
                            }

                            //else if (dataContext.IsViaColoader && !string.IsNullOrEmpty(dataContext.ColoaderName))
                            //{
                            //    message.Envelope.Item = dataContext.ColoaderName;
                            //    message.Envelope.ItemElementName = GLSHK.ItemChoiceType4.ColoaderKey;
                            //}

                            this.SendXMLFile(message, "glshkmessageoutqueue");
                            this.UpdateStock();
                            #endregion
                        }
                        #endregion
                    }

                    else
                    {
                        #region FWB
                        FWBDataContext dataContext = new FWBDataContext(this.Shipment, this.MasterData, this.AWBMessagesCCSTypeCode);
                        FWBDataBuilder dataBuilder = new FWBDataBuilder(dataContext);
                        var champ17Item = dataBuilder.GetChampFWB17(this.isMultiHS);
                        this.ValidateFNAStringLength(dataContext);

                        if (this.IsValid)
                        {
                            if (this.AWBMessagesCCSTypeCode == "CHAMP")
                            {
                                CHAMP17.Envelope envelop = new CHAMP17.Envelope()
                                {
                                    Sender = this.TTY,
                                    Recipient = this.Recipient,
                                    Item = champ17Item,
                                };

                                this.SendXMLFile(envelop, "champmessageoutqueue");
                                this.UpdateStock();
                            }

                            else if (this.AWBMessagesCCSTypeCode == "GLSHK")
                            {
                                #region GLSHK

                                //string myRefID = "HMF" + dataContext.Master.Substring(0, 7) + "X" + dataContext.AirlinePrefix;
                                string myFullRefID = "HMF" + dataContext.Master + "X" + dataContext.AirlinePrefix;

                                string myShipmentNumber = dataContext.ShipmentNumber;
                                if (myShipmentNumber != null)
                                {
                                    if (myShipmentNumber.Length > 14)
                                    {
                                        myShipmentNumber = myShipmentNumber.Substring(0, 14);
                                    }
                                }

                                GLSHK.Message message = new GLSHK.Message()
                                {
                                    Envelope = new GLSHK.Envelope()
                                    {
                                        SenderID = this.PIMA,
                                        RecipientID = this.Recipient,
                                        MsgFormat = "XML",
                                        MsgType = "CIMFWB",
                                        Version = 17,
                                        MsgDateTime = this.TodayDateTime,
                                        RefID = myFullRefID,
                                        MessageRefNum = myShipmentNumber,
                                        InterchangeControlRef = myShipmentNumber,
                                        Item = this.Tenant.ToString(),
                                        ItemElementName = GLSHK.ItemChoiceType4.CompanyID,
                                    },

                                    Item = dataBuilder.GetGLSHKFWB(),
                                    version = GLSHK.MessageVersion.Item20,
                                };

                                if (dataContext.IsViaColoader && !string.IsNullOrEmpty(dataContext.ColoaderKey))
                                {
                                    message.Envelope.Item = dataContext.ColoaderKey;
                                    message.Envelope.ItemElementName = GLSHK.ItemChoiceType4.ColoaderKey;
                                }

                                this.SendXMLFile(message, "glshkmessageoutqueue");
                                this.UpdateStock();
                                #endregion
                            }
                        }
                        #endregion
                    }

                    if (this.IsDemoTenant)
                    {
                        if (!this.IsFHL)
                        {
                            this.BuildDemoRespond();
                        }
                    }

                    this.SaveChanges();

                    scope.Complete();
                }
            }
        }

        private void ValidateFNAStringLength(FWBDataContext dataContext)
        {
            var FNAString = dataContext.Shipment.SCI + dataContext.Shipment.AWBHandlingInformation + dataContext.Shipment.AWBComments + dataContext.FNANotifyDetails;
            if (FNAString != null && FNAString.Count() > 216)
            {
                this.IsValid = false;
                this.Result.IsValid = false;
                this.Result.IsFNAValidationLong = true;
            }
        }

        #region DemoRespond
        public void BuildDemoRespond()
        {
            string fromPortId = MasterData.MainCarriageFromPortId;
            string toPortId = MasterData.MainCarriageFinalDestinationPortId;
            string flightNumber = MasterData.MainCarriageCarrierNumber;
            double weight = Shipment.GrossWeight == null ? 0 : Shipment.GrossWeight.Value;
            int pieces = Shipment.NumberOfPackages == null ? 0 : Shipment.NumberOfPackages.Value;

            string airlinename = null;
            if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierId))
            {
                Card myCard = CardRepository.GetSingleCard(MasterData.MainCarriageCarrierId, Tenant, true);
                if (myCard != null)
                {
                    airlinename = myCard.EnglishName;
                }
            }

            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);

            DateTime? nextETD = MasterData.Transshipment1ETD;
            DateTime? nextATD = MasterData.Transshipment1ATD;
            if (nextETD == null)
            {
                nextETD = MasterData.Transshipment2ETD;
            }
            if (nextATD == null)
            {
                nextATD = MasterData.Transshipment2ATD;
            }

            DateTime? myETD = MasterData.MainCarriageETD;
            DateTime? myATD = MasterData.MainCarriageATD;
            DateTime? myETA = MasterData.MainCarriageETA;
            DateTime? myATA = MasterData.MainCarriageATA;
            if (myETD == null)
            {
                if (nextETD != null)
                {
                    myETD = nextETD.Value.AddDays(-1);
                }

                else
                {
                    myETD = todayDateTime.AddDays(-1);
                }
            }

            if (myATD == null || myATD < myETD)
            {
                myATD = myETD.Value.AddHours(1);                
            }

            if (myETA == null || myETA <= myATD)
            {
                myETA = myATD.Value.AddHours(1);

                if (nextETD != null)
                {
                    if (nextETD < myETA)
                    {
                        myETA = nextETD.Value.AddMinutes(-5);
                    }
                }
            }

            if (myATA == null || myATA < myETA)
            {
                myATA = myETA.Value.AddHours(1);

                if (nextATD != null)
                {
                    if (nextATD < myATA)
                    {
                        myATA = nextATD.Value.AddMinutes(-5);
                    }
                }
            }

            ShipmentCarrierStatus status_RCS = this.BuildDemoCarrierStatus("RCS", fromPortId, toPortId, fromPortId, airlinename, flightNumber, pieces, weight, myETD, null, "E", null, todayDateTime, todayDateTime.AddDays(-1).AddMinutes(-5));
            ShipmentCarrierStatus status_DEP = this.BuildDemoCarrierStatus("DEP", fromPortId, toPortId, fromPortId, airlinename, flightNumber, pieces, weight, myATD, myETA, "A", "E", todayDateTime, todayDateTime.AddDays(-1).AddMinutes(-4));
            ShipmentCarrierStatus status_RCF = this.BuildDemoCarrierStatus("RCF", fromPortId, toPortId, toPortId, airlinename, flightNumber, pieces, weight, null, null, null, null, todayDateTime, todayDateTime.AddDays(-1).AddMinutes(-3));
            ShipmentCarrierStatus status_ARR = this.BuildDemoCarrierStatus("ARR", fromPortId, toPortId, toPortId, airlinename, flightNumber, pieces, weight, null, myATA, null, "A", todayDateTime, todayDateTime.AddDays(-1).AddMinutes(-2));
            ShipmentCarrierStatus status_DLV = this.BuildDemoCarrierStatus("DLV", fromPortId, toPortId, toPortId, airlinename, flightNumber, pieces, weight, null, null, null, null, todayDateTime, todayDateTime.AddDays(-1).AddMinutes(-1));

            ShipmentCarrierStatusRepository shipmentCarrierStatusRepository = new ShipmentCarrierStatusRepository(shipmentContext);
            shipmentCarrierStatusRepository.Add(status_RCS);
            shipmentCarrierStatusRepository.Add(status_DEP);
            shipmentCarrierStatusRepository.Add(status_RCF);
            shipmentCarrierStatusRepository.Add(status_ARR);
            shipmentCarrierStatusRepository.Add(status_DLV);

            MasterData.MainCarriageETD = myETD;
            MasterData.MainCarriageATD = myATD;
            MasterData.MainCarriageETA = myETA;
            MasterData.MainCarriageATA = myATA;
            Shipment.CarrierLastStatusDate = todayDateTime;

            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(Tenant);
            Simplog.Data.InfrastructureModel.EntityPOCOs.EntityStatus entityStatus = entityStatusRepository.GetSingleEntityStatusByCode("SARR", Tenant);
            if (entityStatus != null)
            {
                Shipment.StatusId = entityStatus.Id;
                Shipment.StatusDate = todayDateTime;
            }
        }

        private ShipmentCarrierStatus BuildDemoCarrierStatus(string myStatus, string myFromPortId, string myToPortId, string myLocationPortId, string myAirlineName, string myFlightNumber, int myPieces, double myWeight, DateTime? myDepartureDate, DateTime? myArrivalDate, string myTimeofDepartureInfo, string myTimeofArrivalInfo, DateTime myReceivingDate, DateTime myEventDate)
        {
            ShipmentCarrierStatus myResult = new ShipmentCarrierStatus()
            {
                Id = IdCounter.GetNumber("ShipmentCarrierStatus", this.Tenant).ToString(),
                Tenant = Tenant,
                ShipmentId = ShipmentId,
                Partial = false,
                Status = myStatus,
                Details = myStatus,
                FromPortId = myFromPortId,
                ToPortId = myToPortId,
                Location = myLocationPortId,
                AirlineName = myAirlineName,
                FlightNumber = myFlightNumber,
                Pieces = myPieces,
                Weight = myWeight,
                ReceivingDate = myReceivingDate,
                EventDate = myEventDate,
                DepartureDate = myDepartureDate,
                ArrivalDate = myArrivalDate,
                TimeOfArrivalInfo = myTimeofArrivalInfo,
                TimeOfDepartureInfo = myTimeofDepartureInfo,
            };

            string myRecordInfo = Tenant.ToString() + myFromPortId + myToPortId + myAirlineName + myResult.Details + myStatus + myFlightNumber + myResult.Partial + myPieces + myWeight + ShipmentId;
            string myRecordHash = GetHashedData(myRecordInfo);

            myResult.RecordHash = myRecordHash;
            return myResult;
        }

        private string GetHashedData(string information)
        {
            string myResult = null;

            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(information);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);

            myResult = Convert.ToBase64String(hashedTextInBytes);

            return myResult;
        }
        #endregion

        #region Send XML
        public void SendXMLFile(object myEnvelop, string queueName)
        {
            this.GetObjectTableData();
            this.UpdateShipmentStatus();

            Type myType = myEnvelop.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            if (this.AWBMessagesCCSTypeCode == "CHAMP")
            {
                ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
            }

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, myEnvelop, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");

            //if (this.AWBMessagesCCSTypeCode == "CHAMP")
            //{
            //    if (xmlString.Contains("<ShipmentReferenceInformation"))
            //    {
            //        if (!xmlString.Contains("</ShipmentReferenceInformation>"))
            //        {
            //            xmlString = Regex.Replace(xmlString, @"(<\s*ShipmentReferenceInformation[^>]*)/\s*>", @"$1></ShipmentReferenceInformation>");
            //        }
            //    }
            //}

            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);

            this.BuildCommunicationLog(myByteArray.Length);
            this.BuildTransmissionLog();

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = Tenant,
                FileSize = myByteArray.Length,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            if (!IsDemoTenant)
            {
                try
                {
					//using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
					//{
					//    BrokeredMessage message = new BrokeredMessage();
					//    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(new TimeSpan(0,0,10));

					//    message.Properties["CommunicationLogId"] = myCommunicationLogId;
					//    message.Properties["Tenant"] = Tenant;

					//    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName);//"emailqueue"
					//    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
					//    client.Send(message);

					//    scope.Complete();
					//}

					DbQueueService queueservice = new DbQueueService(queueName, Tenant);
					queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", myCommunicationLogId }, { "Tenant", Tenant.ToString() } }, Tenant);
				}

                catch (Exception ex)
                {
                    string ip = "";

                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "champ web service", null, ip);
                }
            }
        }
       
        private string myObjectTableId;
        private void GetObjectTableData()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(Tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }
        }
        private void UpdateShipmentStatus()
        {
            string eventTypeCode = null;
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            
            if (Shipment.ShipmentLevelCode == "H")
            {
                if (IsCargonautSending || IsDEXXSending)
                {
                    eventTypeCode = "FHLS";
                    Shipment.CargonautFHLStatusCode = "SENT";
                    Shipment.CargonautFHLStatusDate = todayDateTime;
                    shipmentRepository.Update(Shipment);

                    string myMasterFHLStatus = shipmentRepository.GetMasterCargonautFHLStatus(Shipment.MasterShipmentDataId);
                    if (!string.IsNullOrEmpty(myMasterFHLStatus))
                    {
                        Shipment master = shipmentRepository.GetSingleShipment(Shipment.MasterShipmentDataId, Tenant);
                        master.CargonautFHLStatusCode = myMasterFHLStatus;
                        master.CargonautFHLStatusDate = todayDateTime;
                        shipmentRepository.Update(master);
                    }
                }

                else
                {
                    eventTypeCode = "FHLS";
                    Shipment.FHLStatusCode = "SENT";
                    Shipment.FHLStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                    shipmentRepository.Update(Shipment);

                    string myMasterFHLStatus = shipmentRepository.GetMasterFHLStatus(Shipment.MasterShipmentDataId);
                    if (!string.IsNullOrEmpty(myMasterFHLStatus))
                    {
                        Shipment master = shipmentRepository.GetSingleShipment(Shipment.MasterShipmentDataId, Tenant);
                        master.FHLStatusCode = myMasterFHLStatus;
                        master.FHLStatusDate = todayDateTime;
                        shipmentRepository.Update(master);
                    }
                }
            }

            else
            {
                if (IsCargonautSending || IsDEXXSending)
                {
                    eventTypeCode = "FWBS";
                    MasterData.CargonautFWBStatusCode = "SENT";
                    MasterData.CargonautFWBStatusDate = todayDateTime;
                    shipmentMasterDataRepository.Update(MasterData);
                }

                else
                {
                    eventTypeCode = "FWBS";
                    MasterData.FWBStatusCode = "SENT";
                    MasterData.FWBStatusDate = todayDateTime;
                    shipmentMasterDataRepository.Update(MasterData);
                }
            }

            Shipment.LastSentByUserId = LoggedContactId;
            shipmentRepository.Update(Shipment);

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = Tenant,
                EventTypeCode = eventTypeCode,
                UserId = LoggedContactId,
                EntityId = ShipmentId,
                ObjectTableName = "Shipment",
            });
        }

        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private void BuildCommunicationLog(int? fileSize)
        {
            string xmlTarget = this.AWBMessagesCCSTypeCode == "CHAMP" ? "Champ" : "GLSHK";
            string xmlSubject = Shipment.ShipmentLevelCode == "H" ? "FHL" : "FWB";

            if (IsCargonautSending)
            {
                xmlSubject = "Cargonaut " + xmlSubject;
            }

            else if (IsDEXXSending)
            {
                xmlSubject = "DEXX " + xmlSubject;
            }

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = xmlTarget.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                EntityId = ShipmentId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = LoggedContactId,
                DocumentId = document.Id,
                EntityReference = Shipment.ShipmentNumber,
                SearchFields = Shipment.ShipmentNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = DateTime.UtcNow,
                AWBNumber = EntityFieldsHelper.GetLongMasterField(Shipment, MasterData),
            };

            if (IsDemoTenant)
            {
                commLog.CommunicationStatusTypeCode = "D";
                commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                commLog.DoneDateUTC = DateTime.UtcNow;
            }

            communicationLogRepository.Add(commLog);

            this.myDocumentId = document.Id;
            this.myDocumentFolder = document.Folder;
            this.myDocumentExtension = document.Extension;
            this.myCommunicationLogId = commLog.Id;
        }
        private void BuildTransmissionLog()
        {
            MessagesTransmissionHelper TransmissionHelper = new MessagesTransmissionHelper(this.Tenant, this.IsFHL ? "FHL" : "FWB");

            if (IsCargonautSending || IsDEXXSending)
            {
                TransmissionHelper.IsDEXXCargonaut = true;
            }

            TransmissionHelper.Build(this.Shipment, this.MasterData);
        }
        #endregion

        #region UpdateStock
        public void UpdateStock()
        {
            string myMessageType = this.FWBStockCode;

            if (IsFHL)
            {
                myMessageType = this.FHLStockCode;
            }

            if (!IsDemoTenant)
            {
                if (IsAWBStockPrepaid)
                {
                    int? myStocksRemaining = 0;

                    IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(Tenant, "Champ");
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(Tenant);

                    myStocksData = myStocksData.Where(d => d.StartDate <= TodayDate && d.EndDate > TodayDate && d.Remaining > 0 && !d.IsCancelled);

                    if (myStocksData.Count() > 0)
                    {
                        myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                    }

                    Result.StockRemainingBefore = myStocksRemaining == null ? 0 : myStocksRemaining.Value;

                    MessagingStockUsageHistory usageHistory = null;

                    if (Shipment.BookingId != null)
                    {
                        usageHistory = myUsageHistoryData.Where(d => d.EntityId == Shipment.BookingId && d.MessageType == "FFR").FirstOrDefault();
                    }

                    if (usageHistory == null)
                    {
                        usageHistory = myUsageHistoryData.Where(d => d.EntityId == ShipmentId && d.MessageType == myMessageType).FirstOrDefault();
                    }

                    if (usageHistory != null)
                    {
                        usageHistory.ActionType = "Transmission";
                        usageHistory.LastActionDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                        usageHistory.LastActionByUserId = LoggedContactId;
                        usageHistoryRepository.Update(usageHistory);
                        usageHistoryRepository.SubmitChanges();
                    }

                    else
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);

                        MessagingStock myStock = myStocksData.OrderBy(o => o.EndDate).FirstOrDefault();

                        usageHistory = new MessagingStockUsageHistory()
                        {
                            Id = IdCounter.GetNumber("MessagingStockUsageHistory", Tenant),
                            Tenant = Tenant,
                            StockId = myStock.Id,
                            EntityId = ShipmentId,
                            EntityNumber = Shipment.ShipmentNumber,
                            MessageType = myMessageType,
                            MAWB = MasterData.Master,
                            HAWB = Shipment.House,
                            ActionType = "Transmission",
                            FirstActionByUserId = LoggedContactId,
                            FirstActionDate = todayDateTime,
                            LastActionByUserId = LoggedContactId,
                            LastActionDate = todayDateTime,
                            
                        };

                        if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierId))
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(commonContext);
                            Airline airline = airlineRepository.GetSingleAirline(MasterData.MainCarriageCarrierId, Tenant);
                            if (airline != null)
                            {
                                if (!string.IsNullOrEmpty(airline.Prefix) && !string.IsNullOrEmpty(MasterData.Master))
                                {
                                    usageHistory.MAWB = airline.Prefix + "-" + MasterData.Master;
                                }
                            }
                        }

                        usageHistoryRepository.Add(usageHistory);
                        usageHistoryRepository.SubmitChanges();

                        int myStockUsageCount = usageHistoryRepository.GetStockUsageCount(myStock.Id, myStock.TenantNumber);
                        myStock.Remaining = myStock.Amount - myStockUsageCount;
                        stockRepository.Update(myStock);
                        stockRepository.SubmitChanges();
                    }

                    myStocksRemaining = 0;
                    if (myStocksData.Count() > 0)
                    {
                        myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                    }

                    Result.StockRemainingAfter = myStocksRemaining == null ? 0 : myStocksRemaining.Value;
                }
            }
        }
        #endregion

        #region SaveChanges
        public void SaveChanges()
        {
            shipmentRepository.Update(Shipment);
            shipmentMasterDataRepository.Update(MasterData);
            shipmentContext.SaveChanges();
            commonContext.SaveChanges();
            RunStoredProcedureClass.UpdateShipmentStatus(Shipment.Id, Shipment.Tenant);




        }
        #endregion        
    }

    public class CCSResult
    {
        [Key]
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public bool IsMasterFieldMissing { get; set; }
        public bool IsFNAValidationLong { get; set; }
        public bool HasStockError { get; set; }
        public int SendingCount { get; set; }
        public int StockRemainingBefore { get; set; }
        public int StockRemainingAfter { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsUpgradingChamp { get; set; }
        public CCSResult()
        {
            this.IsValid = true;
            this.HasStockError = false;
        }
    }
}
