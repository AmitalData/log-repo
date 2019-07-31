using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.FSR
{
    public class FSRManager
    {
        private int tenant;
        private string loggedEmail;
        private string loggedContactId;
        private FSRResultClass myResultClass;
        private ShipmentPM shipmentPM;
        private ICommonDataContext myCommonContext;
        private IShipmentsContext myShipmentContext;
        private ShipmentRepository shipmentRepository;
        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        public FSRManager(int tenant, string loggedEmail)
        {
            this.tenant = tenant;
            this.loggedEmail = loggedEmail;

            this.myResultClass = new FSRResultClass()
            {
                IsValid = true,
                IsUpgradingChamp = false,
            };

            this.GetGlobalVariables();

            if (this.myResultClass.IsValid)
            {
                this.myCommonContext = CommonDataContext.GetContext(tenant);
                this.myShipmentContext = ShipmentsContext.GetContext(tenant);
                this.shipmentRepository = new ShipmentRepository(myShipmentContext);
                this.shipmentMasterDataRepository = new ShipmentMasterDataRepository(myShipmentContext);

                if (!string.IsNullOrEmpty(loggedEmail))
                {
                    ContactRepository contactRepository = new ContactRepository(myCommonContext);
                    Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact = contactRepository.GetSingleContactByEmail(loggedEmail, tenant);
                    if (contact != null)
                    {
                        loggedContactId = contact.Id;
                    }
                }
            }
        }

        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (this.tenant != 290)
                {
                    SettingRepository settingRepository = new SettingRepository();
                    Setting setting = settingRepository.GetSingleSetting("1");
                    if (setting != null)
                    {
                        if (setting.IsUpgradingChamp)
                        {
                            this.myResultClass.IsValid = false;
                            this.myResultClass.IsUpgradingChamp = setting.IsUpgradingChamp;
                        }
                    }
                }

                scope.Complete();
            }
        }

        public FSRResultClass SendFSR(string shipmentId, string objectTableId, string myRecipient)
        {
            if (this.myResultClass.IsValid)
            {
                try
                {
                    #region
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string myTTY = null;
                        string myCCSMessageType = null;

                        #region GetGlobalVariables
                        using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
                        {
                            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                            TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);

                            if (tenantManagement != null)
                            {
                                myTTY = tenantManagement.TTY;
                                myCCSMessageType = tenantManagement.AWBMessagesCCSTypeCode;
                            }
                        }
                        #endregion

                        ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                        this.shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);

                        if (shipmentPM != null)
                        {
                            string myPrefix = shipmentPM.AirlinePrefix == null ? "" : shipmentPM.AirlinePrefix;
                            string myMaster = shipmentPM.Master == null ? "" : shipmentPM.Master;
                            myPrefix = string.Format("{0:d3}", myPrefix);

                            #region GetDemoTenantData
                            if (tenant == 65)
                            {
                                myResultClass.IsDemoTenant = true;

                                //if (!string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierId))
                                //{
                                //    Card airlineCard = CardRepository.GetSingleCard(shipmentPM.MainCarriageCarrierId, tenant, true);
                                //    if (airlineCard != null)
                                //    {
                                //        if (!string.IsNullOrEmpty(airlineCard.Code))
                                //        {
                                //            if (airlineCard.Code.ToUpper() != "XS")
                                //            {
                                //                myResultClass.IsDemoTenant = true;
                                //            }
                                //        }
                                //    }
                                //}
                            }
                            #endregion

                            CHAMP17.StatusRequest myStatusRequest = new CHAMP17.StatusRequest();

                            #region [0] MessageIdentification
                            myStatusRequest.StandardMessageIdentification = new CHAMP17.StandardMessageIdentification()
                            {
                                StandardMessageIdentifier = "FSR",
                            };
                            #endregion

                            #region [1] ConsignmentDetails
                            myStatusRequest.MasterAWBConsignmentDetail = new CHAMP17.FSR_AWBConsignmentDetails()
                            {
                                AWBIdentification = new CHAMP17.AWBIdentification()
                                {
                                    AirlinePrefix = myPrefix,
                                    AWBSerialNumber = myMaster,
                                },
                            };
                            #endregion

                            CHAMP17.Envelope envelop = new CHAMP17.Envelope()
                            {
                                Sender = myTTY,
                                Recipient = myRecipient,
                                Item = myStatusRequest,
                            };

                            MemoryStream memstream = new MemoryStream();
                            XmlSerializer ser = new XmlSerializer(typeof(CHAMP17.Envelope));
                            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");

                            XmlWriterSettings settings = new XmlWriterSettings()
                            {
                                Indent = true,
                                IndentChars = "",
                                OmitXmlDeclaration = true,
                                NewLineChars = "",
                                NewLineHandling = NewLineHandling.Replace,
                            };

                            XmlWriter writer = XmlTextWriter.Create(memstream, settings);

                            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

                            ser.Serialize(writer, envelop, ns);
                            memstream.Seek(0, SeekOrigin.Begin);
                            var reader = new StreamReader(memstream);
                            string content = reader.ReadToEnd();

                            content = content.Replace(" />", "/>");

                            byte[] bytearray = Encoding.ASCII.GetBytes(content);

                            #region Document

                            DocumentRepository documentrepository = new DocumentRepository(myCommonContext);

                            Document document = new Document()
                            {
                                CreateDate = DateTime.Now,
                                Extension = "xml",
                                FileSize = bytearray.Length,
                                Tenant = Convert.ToInt32(tenant),
                                Id = IdCounter.GetNumber("Document", tenant),
                                HasFile = true,
                                Folder = "champ",
                            };

                            documentrepository.Add(document);
                            documentrepository.SubmitChanges();
                            #endregion

                            #region CommunicationLog

                            string myObjectTableId = null;
                            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
                            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("Shipment", 0, true);
                            if (objectTable != null)
                            {
                                myObjectTableId = objectTable.Id;
                            }

                            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(myCommonContext);

                            CommunicationLog commLog = new CommunicationLog()
                            {
                                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                LastStatusDateUTC = DateTime.UtcNow,
                                To = "Champ",
                                InOut = "O",
                                EntityId = shipmentId,
                                ObjectTableId = myObjectTableId,
                                Subject = "FSR",
                                Tenant = tenant,
                                CommunicationLogTypeCode = "T",
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                CommunicationStatusTypeCode = "W",
                                CreatedByUserId = loggedContactId,
                                DocumentId = document.Id,
                                EntityReference = shipmentPM.ShipmentNumber,
                                CreateDateUTC = DateTime.UtcNow,
                                AWBNumber = myPrefix + "-" + myMaster,
                            };

                            if (myResultClass.IsDemoTenant)
                            {
                                commLog.CommunicationStatusTypeCode = "D";
                                commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                commLog.DoneDateUTC = DateTime.UtcNow;
                            }

                            communicationLogRepository.Add(commLog);
                            communicationLogRepository.SubmitChanges();

                            myResultClass.CreateDate = commLog.CreateDate;
                            #endregion

                            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                            {
                                FileName = document.Id,
                                FolderName = document.Folder,
                                Extension = document.Extension,
                                Tenant = document.Tenant,
                                FileSize = memstream.Length,
                            };

                            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                            storageservice.Write(memstream.ToArray(), fileInfo);

                            shipmentPM.IsFSRSent = true;
                            shipmentPM.LastSentByUserId = this.loggedContactId;
                            shipmentPM.LastFSRStatusRequestDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                            ShipmentService service = new ShipmentService(myShipmentContext, shipmentPM, this.loggedEmail);
                            service.Update();

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "CRLG",
                                UserId = loggedContactId,
                                EntityId = commLog.Id,
                                ObjectTableName = "CommunicationLog",
                                Notes = commLog.Subject,
                            });

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "FSRS",
                                UserId = loggedContactId,
                                EntityId = commLog.EntityId,
                                ObjectTableName = "Shipment",
                                Notes = commLog.Subject,
                            });

                            #region Send

                            if (!myResultClass.IsDemoTenant)
                            {
                                try
                                {
                                    //using (TransactionScope serializableScope = TransactionFactory.GetNewSerializableTransaction())
                                    //{
                                    //    BrokeredMessage message = new BrokeredMessage();

                                    //    message.Properties["CommunicationLogId"] = commLog.Id;
                                    //    message.Properties["Tenant"] = tenant;
                                    //    // message.TimeToLive = new TimeSpan(0, 15, 0);
                                    //    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("champmessageoutqueue");

                                    //    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);

                                    //    client.Send(message);

                                    //    serializableScope.Complete();
                                    //}

                                    DbQueueService queueservice = new DbQueueService("champmessageoutqueue", tenant);
                                    queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() } });
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

                                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "champ request answer web service", null, null);
                                }
                            }
                            #endregion

                            this.BuildTransmissionLog();
                        }

                        scope.Complete();
                    }
                    #endregion
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

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FSR web service (aka: simulator response web service)", null, ip);
                }
            }

            return myResultClass;
        }
        public FSRResultClass SendShipmentFSR(ShipmentPM entityPM)
        {
            if (this.myResultClass.IsValid)
            {
                this.shipmentPM = entityPM;

                Airline airline = null;
                string airlineId = entityPM.MainCarriageCarrierId;
                string airlineCode = null;
                string airlinePrefix = entityPM.AirlinePrefix;

                AirlineRepository airlineRepository = new AirlineRepository(myCommonContext);
                if (!string.IsNullOrEmpty(airlineId))
                {
                    airline = airlineRepository.GetSingleAirline(airlineId, tenant);
                }

                else if (!string.IsNullOrEmpty(airlinePrefix))
                {
                    airline = airlineRepository.GetSingleAirlineByPrefix(airlinePrefix, tenant);
                }

                if (airline != null)
                {
                    airlineId = airline.Id;
                    airlineCode = airline.Card.Code;
                }

                else
                {
                    airline = airlineRepository.GetSingleAirlineByPrefix(airlinePrefix, 0);

                    if (airline == null)
                    {
                        throw new ApplicationException("No airline with this prefix found");
                    }

                    else
                    {
                        #region
                        Airline newAirline = new Airline()
                        {
                            Id = IdCounter.GetNumber("Card", tenant).ToString(),
                            Tenant = tenant,
                            AccountNumber = airline.AccountNumber,
                            AWBAccount = airline.AWBAccount,
                            CheckDigit = airline.CheckDigit,
                            LimitedLength = airline.LimitedLength,
                            Prefix = airline.Prefix,
                            TTY = airline.TTY,
                            IsChampRegistered = airline.IsChampRegistered,
                            ChampNeedsRegistration = airline.ChampNeedsRegistration,
                            ChampFWB = airline.ChampFWB,
                            ChampFHL = airline.ChampFHL,
                            ChampFSU = airline.ChampFSU,
                            ChampFSRFSA = airline.ChampFSRFSA,
                            ChampFVRFVA = airline.ChampFVRFVA,
                            ChampFFRFFA = airline.ChampFFRFFA,
                            GLSHKFWB = airline.GLSHKFWB,
                            GLSHKFHL = airline.GLSHKFHL,
                            GLSHKFSU = airline.GLSHKFSU,
                            GLSHKFSRFSA = airline.GLSHKFSRFSA,
                            GLSHKFVRFVA = airline.GLSHKFVRFVA,
                            GLSHKFFRFFA = airline.GLSHKFFRFFA,
                        };

                        CardRepository cardRepository = new CardRepository(myCommonContext);
                        Card newCard = new Card()
                        {
                            Id = newAirline.Id,
                            Tenant = tenant,
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            ReceivablesAccountingCard = airline.Card.ReceivablesAccountingCard,
                            PayablesAccountingCard = airline.Card.PayablesAccountingCard,
                            AccountNumber = airline.Card.AccountNumber,
                            BankAddress = airline.Card.BankAddress,
                            BankName = airline.Card.BankName,
                            CityName = airline.Card.CityName,
                            Code = airline.Card.Code,
                            CountryName = airline.Card.CountryName,
                            EnglishName = airline.Card.EnglishName,
                            IBANNumber = airline.Card.IBANNumber,
                            LocalName = airline.Card.LocalName,
                            Website = airline.Card.Website,
                            Notes = airline.Card.Notes,
                            PartnerTypeId = "AL",
                            PartnerTypeName = airline.Card.PartnerTypeName,
                            VatNumber = airline.Card.VatNumber,
                            SearchFields = airline.Card.SearchFields,
                        };

                        cardRepository.Add(newCard);
                        airlineRepository.Add(newAirline);
                        cardRepository.SubmitChanges();
                        airlineRepository.SubmitChanges();

                        airlineId = newAirline.Id;
                        airlineCode = newCard.Code;
                        #endregion
                    }
                }

                string recipient = "";
                Airline tenantZeroAirline = airlineRepository.GetSingleAirlineByCode(airlineCode, 0);
                if (tenantZeroAirline != null)
                {
                    recipient = tenantZeroAirline.TTY;
                }

                if (string.IsNullOrEmpty(recipient))
                {
                    throw new ApplicationException("Status request can't be sent to the airline! the airline doesn't support it");
                }

                string shipmentId = "";
                string tableName = "";

                Shipment shipment = shipmentRepository.GetShipmentByFSRData(entityPM.Master, airlineId, entityPM.DirectionId, tenant);
                if (shipment != null)
                {
                    shipmentId = shipment.Id;
                    tableName = shipment.ShipmentLevelCode == "C" ? "Master" : "Shipment";

                    ShipmentMasterData entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(shipment.MasterShipmentDataId);
                    if(entityMasterData != null)
                    {
                        if(entityMasterData.AirlinePrefix != entityPM.AirlinePrefix)
                        {
                            entityMasterData.AirlinePrefix = entityPM.AirlinePrefix;
                            shipmentMasterDataRepository.Update(entityMasterData);
                            shipmentMasterDataRepository.SubmitChanges();
                        }
                    }
                }

                else
                {
                    PortRepository portRepository = new PortRepository(myCommonContext);
                    Port port = portRepository.GetUnsignedPort(tenant);

                    entityPM.MainCarriageCarrierPrefix = airlineCode;
                    entityPM.MainCarriageCarrierId = airlineId;
                    entityPM.MainCarriageToPortId = port.Id;
                    entityPM.MainCarriageFromPortId = port.Id;
                    entityPM.MainCarriageFinalDestinationPortId = port.Id;

                    ShipmentService service = new ShipmentService(myShipmentContext, entityPM, loggedEmail);
                    service.Create();

                    shipmentId = entityPM.Id;
                    tableName = entityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
                }

                string objectTableId = "";
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(tableName, 0, false);
                if (objectTable != null)
                {
                    objectTableId = objectTable.Id;
                }

                if (string.IsNullOrEmpty(shipmentId) || string.IsNullOrEmpty(objectTableId) || string.IsNullOrEmpty(recipient))
                {
                    throw new ApplicationException("Error sending");
                }

                this.myResultClass = this.SendFSR(shipmentId, objectTableId, recipient);
            }

            return this.myResultClass;
        }

        private void BuildTransmissionLog()
        {
            MessagesTransmissionHelper TransmissionHelper = new MessagesTransmissionHelper(tenant, "FSR");
            TransmissionHelper.Build(this.shipmentPM);
        }
    }

    public class FSRResultClass
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsValid { get; set; }
        public bool IsUpgradingChamp { get; set; }
    }
}
