using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
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

namespace Logitude.XSD.INTTRA.BL
{
    public class INTTRAHelper
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string LoggedContactId { get; set; }
        public System.DateTime TodayDate { get; set; }
        public System.DateTime TodayDateTime { get; set; }
        public Simplog.Data.CommonDataModel.EntityPOCOs.Contact LoggedContact { get; set; }
        public INTTRAResult Result { get; set; }
        public INTTRADataContext DataContext { get; set; }
        private ICommonDataContext CommonContext;
        private MessagingStockRepository stockRepository;
        private MessagingStockUsageHistoryRepository usageHistoryRepository;
        private string MessageTypeCode = "SI";
        public INTTRAHelper(string myShipmentId, int myTenant)
        {
            this.Tenant = myTenant;
            this.ShipmentId = myShipmentId;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.TodayDate = TenantServerConfigration.GetCurrentDateTime(myTenant).Date;
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(myTenant);

            ContactRepository contactRepository = new ContactRepository(this.CommonContext);
            this.LoggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
            this.LoggedContactId = LoggedContact.Id;

            this.DataContext = new INTTRADataContext(this.Tenant, this.ShipmentId, this.LoggedContact, this.CommonContext);

            this.Result = new INTTRAResult()
            {
                Id = Tenant,
                IsDemoTenant = this.DataContext.IsDemoTenant,
                IsStockPrepaid = this.DataContext.IsStockPrepaid,
                IsValid = this.DataContext.IsValid,
                IsLimited = this.DataContext.IsLimited,
                Errors = this.DataContext.Errors,
                IsCarrierRegisteredToINTTRA = this.DataContext.IsCarrierRegisteredToINTTRA,
                IsCarrierRegisteredToBranch = this.DataContext.IsCarrierRegisteredToBranch,
            };
        }

        public void Run()
        {
            if (this.DataContext.IsValid)
            {
                if (!this.DataContext.IsLimited)
                {
                    this.CheckStockValidity();

                    if (!this.Result.HasStockError)
                    {
                        this.DataContext.Build();

                        INTTRADataBuilder dataBuilder = new INTTRADataBuilder(this.DataContext);

                        INTTRA_Out.Message message = new INTTRA_Out.Message()
                        {
                            Header = dataBuilder.GetHeader(),

                            MessageBody = new INTTRA_Out.MessageBody()
                            {
                                MessageDetails = dataBuilder.GetMessageDetails(),

                                MessageProperties = dataBuilder.GetMessageProperties(),
                            }
                        };

                        this.SendXMLFile(message);
                        this.UpdateStock();
                        this.SaveChanges();

                        if (this.Result.IsDemoTenant)
                        {
                            this.BuildDemoRespond();
                        }
                    }
                }
            }
        }

        private void CheckStockValidity()
        {
            if (!this.Result.IsDemoTenant)
            {
                if (this.Result.IsStockPrepaid)
                {
                    this.stockRepository = new MessagingStockRepository(this.DataContext.shipmentContext);
                    this.usageHistoryRepository = new MessagingStockUsageHistoryRepository(this.DataContext.shipmentContext);

                    IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(this.Tenant, "INTTRA");
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(this.Tenant);

                    myStocksData = myStocksData.Where(d => d.StartDate <= TodayDate && d.EndDate > TodayDate && d.Remaining > 0 && !d.IsCancelled);

                    int sendingCount = 0;
                    int? myStocksRemaining = 0;
                    if (myStocksData.Count() > 0)
                    {
                        myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                    }

                    if (!myUsageHistoryData.Where(d => d.EntityId == this.ShipmentId && d.MessageType == this.MessageTypeCode).Any())
                    {
                        sendingCount = 1;
                    }

                    if (sendingCount > 0)
                    {
                        if (myStocksRemaining < sendingCount)
                        {
                            this.Result.IsValid = false;
                            this.Result.HasStockError = true;
                        }
                    }
                }
            }
        }
        private void UpdateStock()
        {
            if (!this.Result.IsDemoTenant)
            {
                if (this.Result.IsStockPrepaid)
                {
                    IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(this.Tenant, "INTTRA");
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(this.Tenant);

                    myStocksData = myStocksData.Where(d => d.StartDate <= TodayDate && d.EndDate > TodayDate && d.Remaining > 0 && !d.IsCancelled);

                    MessagingStockUsageHistory usageHistory = myUsageHistoryData.Where(d => d.EntityId == this.ShipmentId && d.MessageType == this.MessageTypeCode).FirstOrDefault();

                    if (usageHistory != null)
                    {
                        usageHistory.ActionType = "Transmission";
                        usageHistory.LastActionDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
                        usageHistory.LastActionByUserId = this.LoggedContactId;
                        usageHistoryRepository.Update(usageHistory);
                        usageHistoryRepository.SubmitChanges();
                    }

                    else
                    {
                        System.DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(this.Tenant);

                        MessagingStock myStock = myStocksData.OrderBy(o => o.EndDate).FirstOrDefault();

                        usageHistory = new MessagingStockUsageHistory()
                        {
                            Id = IdCounter.GetNumber("MessagingStockUsageHistory",this.Tenant),
                            Tenant = this.Tenant,
                            StockId = myStock.Id,
                            EntityId = this.ShipmentId,
                            EntityNumber =this.DataContext.Shipment.ShipmentNumber,
                            MessageType = this.MessageTypeCode,
                            ActionType = "Transmission",
                            FirstActionByUserId = this.LoggedContactId,
                            FirstActionDate = todayDateTime,
                            LastActionByUserId = this.LoggedContactId,
                            LastActionDate = todayDateTime,

                        };

                        //if (!string.IsNullOrEmpty(myBooking.MainCarriageCarrierId))
                        //{
                        //    AirlineRepository airlineRepository = new AirlineRepository(this.CommonContext);
                        //    Airline airline = airlineRepository.GetSingleAirline(myBooking.MainCarriageCarrierId, myTenant);
                        //    if (airline != null)
                        //    {
                        //        if (!string.IsNullOrEmpty(airline.Prefix) && !string.IsNullOrEmpty(myBooking.Master))
                        //        {
                        //            usageHistory.MAWB = airline.Prefix + "-" + myBooking.Master;
                        //        }
                        //    }
                        //}

                        usageHistoryRepository.Add(usageHistory);
                        usageHistoryRepository.SubmitChanges();

                        int myStockUsageCount = usageHistoryRepository.GetStockUsageCount(myStock.Id, myStock.TenantNumber);
                        myStock.Remaining = myStock.Amount - myStockUsageCount;
                        stockRepository.Update(myStock);
                        stockRepository.SubmitChanges();
                    }
                }
            }
        }

        #region Send XML
        public void SendXMLFile(object myRequest)
        {
            this.GetObjectTableData();
            this.UpdateShipmentStatus();

            Type myType = myRequest.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

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

            ser.Serialize(writer, myRequest, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");

            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);

            CommunicationLog commLog = this.BuildCommunicationLog(myByteArray.Length);

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = Tenant,
                FileSize = myByteArray.Length,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            if (!this.Result.IsDemoTenant)
            {
                try
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue(commLog.QueueName, 0);
                    queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", Tenant.ToString() } });
                }

                catch (Exception ex)
                {
                    string ip = "";

                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        ip = HttpContext.Current.Request.UserHostAddress;
                    }

                    ExceptionHandler.HandleException(ex, System.DateTime.Now, 0, null, "INTTRA controller", null, ip);
                }
            }
        }

        private void UpdateShipmentStatus()
        {
            this.DataContext.Shipment.INTTRASIStatusCode = "SENT";
            this.DataContext.Shipment.INTTRABookingStatusCode = "SI";
            this.DataContext.Shipment.INTTRASIStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.DataContext.shipmentRepository.Update(this.DataContext.Shipment);
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

        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private CommunicationLog BuildCommunicationLog(int? fileSize)
        {
            DocumentRepository documentRepository = new DocumentRepository(this.CommonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(this.CommonContext);

            string xmlTarget = "INTTRA";
            string xmlSubject = "Shipping Instructions";

            Document document = new Document()
            {
                CreateDate = System.DateTime.Now,
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
                Id = this.DataContext.CommunicationLogIdCounter,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = System.DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                EntityId = ShipmentId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = this.LoggedContactId,
                DocumentId = document.Id,
                EntityReference = this.DataContext.ShipmentNumber,
                SearchFields = this.DataContext.ShipmentNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = System.DateTime.UtcNow,
                LogSettings = this.GetLogSettings(xmlSubject),
                QueueName = "FTPCommunicationLogQueue",
            };

            if (this.DataContext.MasterData != null)
            {
                commLog.AWBNumber = this.DataContext.MasterData.Master;
            }

            if (this.Result.IsDemoTenant)
            {
                commLog.CommunicationStatusTypeCode = "D";
                commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                commLog.DoneDateUTC = System.DateTime.UtcNow;
            }

            communicationLogRepository.Add(commLog);

            this.myDocumentId = document.Id;
            this.myDocumentFolder = document.Folder;
            this.myDocumentExtension = document.Extension;
            this.myCommunicationLogId = commLog.Id;
            return commLog;
        }
        private string GetLogSettings(string xmlSubject)
        {
            string myResult = null;

            if (this.DataContext.INTTRA_OutSettingsId != null)
            {
                FTPDetail fTPDetail = (from d in this.CommonContext.FTPDetails where d.Id == this.DataContext.INTTRA_OutSettingsId select d).FirstOrDefault();

                if (fTPDetail != null)
                {
                    string filename = Tenant + "_" + DataContext.ShipmentNumber + "_SI_" + this.DataContext.XMLCreateDate + "_" + this.DataContext.CommunicationLogIdCounter;

                    CommunicationLogSettings settings = new CommunicationLogSettings()
                    {
                        Host = fTPDetail.Host,
                        Folder = fTPDetail.Folder,
                        Username = fTPDetail.UserName,
                        Password = fTPDetail.Password,
                        Filename = filename,
                        UseSFTP = fTPDetail.UseSFTP,
                    };

                    myResult = JsonConvert.SerializeObject(settings);
                }
            }

            return myResult;
        }
        #endregion

        public void SaveChanges()
        {
            this.DataContext.shipmentRepository.SubmitChanges();
            this.CommonContext.SaveChanges();
        }

        #region DemoRespond
        List<INTTRA.PartnerInformation> DemoHeaderParties;
        List<INTTRA.PartnerInformation> DemoBodyParties;
        List<INTTRA.ShipmentComments> DemoBodyInstructions;
        private void BuildDemoRespond()
        {
            this.BuildDemo_Data();
            this.BuildDemo_CONTRL();
            this.BuildDemo_APERAK();
            //this.BuildDemo_Status();
        }
        private void BuildDemo_Data()
        {
            // HeaderParties 
            this.DemoHeaderParties = new List<INTTRA.PartnerInformation>();
            this.DemoHeaderParties.Add(new INTTRA.PartnerInformation()
            {
                PartnerRole = INTTRA.PartnerInformationPartnerRole.Sender,

                PartnerIdentifier = new INTTRA.PartnerIdentifier()
                {
                    Value = this.DataContext.Recipient.PartnerIdentifier.Value,
                    Agency = PartnerIdentifierAgency.AssignedBySender,
                },
            });

            this.DemoHeaderParties.Add(new INTTRA.PartnerInformation()
            {
                PartnerRole = INTTRA.PartnerInformationPartnerRole.Recipient,

                PartnerIdentifier = new INTTRA.PartnerIdentifier()
                {
                    Value = this.DataContext.Sender.PartnerIdentifier.Value,
                    Agency = PartnerIdentifierAgency.AssignedByRecipient,
                },
            });

            // BodyInstructions
            this.DemoBodyInstructions = new List<INTTRA.ShipmentComments>();
            this.DemoBodyInstructions.Add(new INTTRA.ShipmentComments()
            {
                CommentType = INTTRA.ShipmentCommentsCommentType.General,
                Value = "Accepted",
            });

            // BodyParties
            this.DemoBodyParties = new List<PartnerInformation>();
            INTTRA_Out.PartnerInformation iOutRequestor = this.DataContext.MessagePropertiesParties.Where(d => d.PartnerRole == INTTRA_Out.PartnerInformationPartnerRole.Requestor).FirstOrDefault();
            if (iOutRequestor != null)
            {
                this.DemoBodyParties.Add(new INTTRA.PartnerInformation()
                {
                    PartnerRole = INTTRA.PartnerInformationPartnerRole.Requestor,

                    PartnerName = iOutRequestor.PartnerName,

                    PartnerIdentifier = new INTTRA.PartnerIdentifier()
                    {
                        Value = iOutRequestor.PartnerIdentifier.Value,
                        Agency = PartnerIdentifierAgency.AssignedByRecipient,
                    },
                });
            }

            INTTRA_Out.PartnerInformation iOutCarrier = this.DataContext.MessagePropertiesParties.Where(d => d.PartnerRole == INTTRA_Out.PartnerInformationPartnerRole.Carrier).FirstOrDefault();
            if (iOutCarrier != null)
            {
                this.DemoBodyParties.Add(new INTTRA.PartnerInformation()
                {
                    PartnerRole = INTTRA.PartnerInformationPartnerRole.Carrier,

                    PartnerName = iOutCarrier.PartnerName,

                    PartnerIdentifier = new INTTRA.PartnerIdentifier()
                    {
                        Value = iOutCarrier.PartnerIdentifier.Value,
                        Agency = PartnerIdentifierAgency.AssignedByRecipient,
                    },
                });
            }
        }
        private void BuildDemo_CONTRL()
        {
            INTTRA.Message iMessage_CONTROL = new INTTRA.Message()
            {
                Header = new INTTRA.Header()
                {
                    MessageType = new INTTRA.MessageType() { Value = "CONTRL", MessageVersion = 1 },

                    DocumentIdentifier = this.DataContext.ShipmentNumber + "-" + this.DataContext.Tenant + "-" + this.DataContext.XMLCreateDate + "-" + this.DataContext.CommunicationLogIdCounter,

                    DateTime = new INTTRA.DateTime()
                    {
                        DateType = INTTRA.DateTimeDateType.Document,
                        Value = this.DataContext.XMLCreateDate,
                    },

                    Parties = this.DemoHeaderParties.ToArray(),
                },

                MessageBody = new INTTRA.MessageBody()
                {
                    MessageProperties = new INTTRA.MessageProperties()
                    {
                        ShipmentID = new INTTRA.ShipmentID()
                        {
                             ShipmentIdentifier = new INTTRA.ShipmentIdentifier()
                             {
                                  Acknowledgment = INTTRA.Acknowledgment.Accepted,
                                   MessageStatus = INTTRA.ShipmentIdentifierMessageStatus.Original,
                                    Value = this.DataContext.ShipmentNumber,
                             },                              
                        },

                        DateTime = new INTTRA.DateTime()
                        {
                            DateType = INTTRA.DateTimeDateType.Document,
                            Value = this.DataContext.XMLCreateDate_Long,
                        },

                        Instructions = this.DemoBodyInstructions.ToArray(),
                    },
                },
            };

            this.BuildDemoXMLFile(iMessage_CONTROL);
        }
        private void BuildDemo_APERAK()
        {
            INTTRA.Message iMessage_APERAK = new INTTRA.Message()
            {
                Header = new INTTRA.Header()
                {
                    MessageType = new INTTRA.MessageType() { Value = "ApplicationAcknowledgment", MessageVersion = 1 },

                    DocumentIdentifier = this.DataContext.XMLCreateDate.ToString(),

                    DateTime = new INTTRA.DateTime()
                    {
                        DateType = INTTRA.DateTimeDateType.Document,
                        Value = this.DataContext.XMLCreateDate,
                    },

                    Parties = this.DemoHeaderParties.ToArray(),
                },

                MessageBody = new INTTRA.MessageBody()
                {
                    MessageProperties = new INTTRA.MessageProperties()
                    {
                        ShipmentID = new INTTRA.ShipmentID()
                        {
                            ShipmentIdentifier = new INTTRA.ShipmentIdentifier()
                            {
                                Acknowledgment = INTTRA.Acknowledgment.Accepted,
                                MessageStatus = INTTRA.ShipmentIdentifierMessageStatus.Original,
                                Value = this.DataContext.ShipmentNumber,
                            },
                        },

                        DateTime = new INTTRA.DateTime()
                        {
                            DateType = INTTRA.DateTimeDateType.StatusChange,
                            Value = this.DataContext.XMLCreateDate_Long,
                        },

                        Instructions = this.DemoBodyInstructions.ToArray(),

                        Parties = this.DemoBodyParties.ToArray(),
                    },
                },
            };

            #region ReferenceInformation 
            if (this.DataContext.ReferenceInformations != null)
            {
                if (this.DataContext.ReferenceInformations.Count > 0)
                {
                    List<INTTRA.ReferenceInformation> iReferenceInformations = new List<INTTRA.ReferenceInformation>();

                    foreach (INTTRA_Out.ReferenceInformation item in this.DataContext.ReferenceInformations)
                    {
                        bool isExists = true;
                        INTTRA_Out.ReferenceInformationReferenceType itemReferenceType = item.ReferenceType;
                        INTTRA.ReferenceInformationReferenceType iReferenceType = INTTRA.ReferenceInformationReferenceType.BookingNumber;

                        switch (itemReferenceType)
                        {
                            case INTTRA_Out.ReferenceInformationReferenceType.BookingNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.BookingNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.BillOfLadingNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.BillOfLadingNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.BrokerReferenceNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.BrokerReferenceNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ConsigneeOrderNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.ConsigneeOrderNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ContractNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.ContractNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ExportersReferenceNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.ExportersReferenceNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.FreightForwarderReference:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.FreightForwarderReference;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.FederalMaritimeComNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.FederalMaritimeComNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.InvoiceNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.InvoiceNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.PurchaseOrderNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.PurchaseOrderNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ShipperIdentifyingNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.ShipperIdentifyingNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.TransactionReferenceNumber:
                                {
                                    iReferenceType = INTTRA.ReferenceInformationReferenceType.TransactionReferenceNumber;
                                    break;
                                }

                            default:
                                {
                                    isExists = false;
                                    break;
                                }
                        }

                        if (isExists)
                        {
                            iReferenceInformations.Add(new INTTRA.ReferenceInformation()
                            {
                                ReferenceType = iReferenceType,
                                Value = item.Value,
                            });
                        }
                    }

                    if (iReferenceInformations.Count > 0)
                    {
                        iMessage_APERAK.MessageBody.MessageProperties.ReferenceInformation = iReferenceInformations.ToArray();
                    }
                }
            }
            #endregion

            this.BuildDemoXMLFile(iMessage_APERAK);
        }
        private void BuildDemo_Status()
        {
            #region HeaderParties  
            List<INTTRA_Status.PartnerInformationType> iDemoHeaderParties = new List<INTTRA_Status.PartnerInformationType>();
            iDemoHeaderParties.Add(new INTTRA_Status.PartnerInformationType()
            {
                PartnerRole = INTTRA_Status.PartnerInformationTypePartnerRole.Sender,

                PartnerIdentifier = new INTTRA_Status.PartnerIdentifierType()
                {
                    Value = this.DataContext.Recipient.PartnerIdentifier.Value,
                    Agency = INTTRA_Status.PartnerIdentifierTypeAgency.AssignedBySender,
                },
            });

            iDemoHeaderParties.Add(new INTTRA_Status.PartnerInformationType()
            {
                PartnerRole = INTTRA_Status.PartnerInformationTypePartnerRole.Recipient,

                PartnerIdentifier = new INTTRA_Status.PartnerIdentifierType()
                {
                    Value = this.DataContext.Sender.PartnerIdentifier.Value,
                    Agency = INTTRA_Status.PartnerIdentifierTypeAgency.AssignedByRecipient,
                },
            });
            #endregion

            INTTRA_Status.MessageType iMessage_STATUS = new INTTRA_Status.MessageType()
            {
                Header = new INTTRA_Status.HeaderType()
                {
                    MessageType = new INTTRA_Status.HeaderTypeMessageType() { Value = "Status", MessageVersion = 1 },

                    DocumentIdentifier = this.DataContext.XMLCreateDate.ToString(),

                    DateTime = new INTTRA_Status.HeaderTypeDateTime()
                    {
                        DateType = INTTRA_Status.DateTimeTypeDateType.Document,
                        Value = this.DataContext.XMLCreateDate,
                    },

                    Parties = iDemoHeaderParties.ToArray(),
                },

                MessageBody = new INTTRA_Status.MessageBodyType()
                {
                    MessageProperties = new INTTRA_Status.MessagePropertiesType()
                    {
                        EventCode = "EE",

                        EventLocation = new INTTRA_Status.EventLocationType()
                        {
                            Location = new INTTRA_Status.LocationType()
                            {
                                LocationType1 = INTTRA_Status.LocationTypeLocationType.ActivityLocation,

                                LocationCode = new INTTRA_Status.LocationTypeLocationCode()
                                {
                                    Agency = INTTRA_Status.LocationCodeTypeAgency.UN,
                                    Value = this.DataContext.FromPortCountry.Code + this.DataContext.FromPort.Code,
                                },

                                LocationName = this.DataContext.FromPort.Code,

                                LocationCountry = this.DataContext.FromPortCountry.Code,

                                DateTime = new INTTRA_Status.LocationTypeDateTime()
                                {
                                    DateType = INTTRA_Status.DateTimeType1DateType.StatusChange,
                                    Value = this.DataContext.XMLCreateDate_Long,
                                },
                            },
                        },

                        //ReferenceInformation = new List<INTTRA_Status.MessagePropertiesTypeReferenceInformation>().ToArray(),

                        TransportationDetails = new INTTRA_Status.TransportationDetailsType()
                        {
                            ConveyanceInformation = new INTTRA_Status.ConveyanceInformationType()
                            {
                                CarrierSCAC = this.DataContext.MainShippingLine.SCACCode,
                                ConveyanceName = "TEST123",
                                VoyageTripNumber = "TEST123",
                                TransportIdentification = new INTTRA_Status.ConveyanceInformationTypeTransportIdentification()
                                {
                                    TransportIdentificationType1 = INTTRA_Status.TransportIdentificationTypeTransportIdentificationType.LloydsCode,
                                    TransportIdentificationType1Specified = true,
                                    Value = "1234567",
                                }
                            },

                            TransportStage = INTTRA_Status.TransportationDetailsTypeTransportStage.Main,
                            TransportMode = INTTRA_Status.TransportationDetailsTypeTransportMode.Maritime,
                            TransportModeSpecified = true,
                        },

                        Parties = new INTTRA_Status.PartiesType1()
                        {
                            PartnerInformation = new INTTRA_Status.PartnerInformationType1()
                            {
                                PartnerRole = INTTRA_Status.PartnerInformationType1PartnerRole.Carrier,
                                PartnerIdentifier = new INTTRA_Status.PartnerIdentifierType1()
                                {
                                    Agency = INTTRA_Status.PartnerIdentifierType1Agency.AssignedByCarrier,
                                    Value = this.DataContext.MainShippingLine.SCACCode,
                                }
                            },
                        },
                    },
                },
            };

            #region MessageProperties: ReferenceInformation 
            if (this.DataContext.ReferenceInformations != null)
            {
                if (this.DataContext.ReferenceInformations.Count > 0)
                {
                    List<INTTRA_Status.MessagePropertiesTypeReferenceInformation> iReferenceInformations = new List<INTTRA_Status.MessagePropertiesTypeReferenceInformation>();

                    foreach (INTTRA_Out.ReferenceInformation item in this.DataContext.ReferenceInformations)
                    {
                        bool isExists = true;
                        INTTRA_Out.ReferenceInformationReferenceType itemReferenceType = item.ReferenceType;
                        INTTRA_Status.ReferenceInformationTypeReferenceType iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.BillOfLadingNumber;

                        switch (itemReferenceType)
                        {
                            case INTTRA_Out.ReferenceInformationReferenceType.BookingNumber:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.BookingNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.BillOfLadingNumber:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.BillOfLadingNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ConsigneeOrderNumber:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.ConsigneeOrderNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ContractNumber:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.ContractNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.FreightForwarderReference:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.FreightForwarderReference;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.PurchaseOrderNumber:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.PurchaseOrderNumber;
                                    break;
                                }

                            case INTTRA_Out.ReferenceInformationReferenceType.ShipperIdentifyingNumber:
                                {
                                    iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.ShipperIdentifyingNumber;
                                    break;
                                }

                            //case INTTRA_Out.ReferenceInformationReferenceType.ContractPartyReferenceNumber:
                            //    {
                            //        iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.ContractPartyReferenceNumber;
                            //        break;
                            //    }

                            //case INTTRA_Out.ReferenceInformationReferenceType.ConsigneeReferenceNumber:
                            //    {
                            //        iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.ConsigneeReferenceNumber;
                            //        break;
                            //    }

                            //case INTTRA_Out.ReferenceInformationReferenceType.InttraBookingNumber:
                            //    {
                            //        iReferenceType = INTTRA_Status.ReferenceInformationTypeReferenceType.InttraBookingNumber;
                            //        break;
                            //    }

                            default:
                                {
                                    isExists = false;
                                    break;
                                }
                        }

                        if (isExists)
                        {
                            iReferenceInformations.Add(new INTTRA_Status.MessagePropertiesTypeReferenceInformation()
                            {
                                ReferenceType = iReferenceType,
                                Value = item.Value,
                            });
                        }
                    }

                    if (iReferenceInformations.Count > 0)
                    {
                        iMessage_STATUS.MessageBody.MessageProperties.ReferenceInformation = iReferenceInformations.ToArray();
                    }
                }
            }
            #endregion

            #region MessageDetails
            ShipmentPackage iContainer = this.DataContext.ShipmentPackages.FirstOrDefault();
            if (iContainer != null)
            {
                PackageTypeRepository iPackageTypeRepository = new PackageTypeRepository(this.CommonContext);
                PackageType iPackageType = iPackageTypeRepository.GetSinglePackageType(iContainer.PackageTypeId, this.Tenant);
                if (iPackageType != null)
                {
                    iMessage_STATUS.MessageBody.MessageDetails = new INTTRA_Status.MessageDetailsType()
                    {
                        EquipmentDetails = new INTTRA_Status.EquipmentDetailsType()
                        {
                            LineNumber = "1",

                            EquipmentIdentifier = new INTTRA_Status.EquipmentDetailsTypeEquipmentIdentifier()
                            {
                                LoadType = INTTRA_Status.EquipmentIdentifierTypeLoadType.Empty,
                                Value = iContainer.ContainerNumber,
                            },

                            EquipmentType = new INTTRA_Status.EquipmentTypeType()
                            {
                                EquipmentTypeCode = iPackageType.Code,
                            },
                        },
                    };
                }
            }
            #endregion

            this.BuildDemoXMLFile(iMessage_STATUS);
        }
        private void BuildDemoXMLFile(object myRequest)
        {
            Type myType = myRequest.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            // this will remove the namespaces
            // <Message xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            ns.Add("", "");

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

            ser.Serialize(writer, myRequest, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");

            this.BuildAnalyzeQueue(xmlString);
        }
        private void BuildAnalyzeQueue(string xmlString)
        {
            System.DateTime iCreateDate =  TenantServerConfigration.GetCurrentDateTime(0);

            using (TransactionScope scope1 = TransactionFactory.GetNewTransaction())
            {
                byte[] fileBytes = Encoding.ASCII.GetBytes(xmlString);

                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                {
                    CreateDate = iCreateDate,
                    From = "INTTRA",
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = fileBytes,
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = false,
                    Tenant = 0,
                    FileSize = fileBytes.Length,
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();

                scope1.Complete();
            }
        }
        #endregion
    }
    public class INTTRAResult
    {
        [Key]
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public bool IsLimited { get; set; }
        public List<string> Errors { get; set; }
        public bool IsCarrierRegisteredToINTTRA { get; set; }
        public bool IsCarrierRegisteredToBranch { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool HasStockError { get; set; }
        public bool IsStockPrepaid { get; set; }
        public INTTRAResult()
        {
            this.IsValid = true;
        }
    }
    public class CommunicationLogSettings
    {
        public string Host { get; set; }
        public string Folder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Filename { get; set; }
        public bool UseSFTP { get; set; }
    }
}
