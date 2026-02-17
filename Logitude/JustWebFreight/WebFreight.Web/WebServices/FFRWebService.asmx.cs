using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using CHAMP17;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.XSD.Analyzers;
using Logitude.XSD.Analyzers.CHAMPAnalyzer;
using Logitude.XSD.FFR;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.Security;
using Logitude.BookingLib.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.XSD.Simulators;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using WebFreight.Web.Helpers;
using Logitude.XSD;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for FFRWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FFRWebService : System.Web.Services.WebService
    {
        private FFRResult myResult;
        private Booking myBooking;
        private IShipmentsContext shipmentContext;
        private IBookingContext bookingContext;
        private ICommonDataContext commonContext;
        private DocumentRepository documentRepository;
        private CommunicationLogRepository communicationLogRepository;
        private ContactRepository contactRepository;
        private PortRepository portRepository;
        private BookingRepository bookingRepository;
        private BookingLastRequestRepository lastRequestRepository;
        private BookingAnswerRepository bookingAnswerRepository;
        private MessagingStockRepository stockRepository;
        private MessagingStockUsageHistoryRepository usageHistoryRepository;
        public DateTime TodayDate { get; set; }
        public DateTime TodayDateTime { get; set; }
        private int myTenant;
        private string myBookingId;
        private string loggedContactId;
        private string TTY;
        private string PIMA;
        private string AWBMessagesCCSTypeCode;
        private string myRecipient;
        private string myObjectTableId;
        private bool cancellationSent;
        private bool IsDemoTenant;
        private bool IsEAWBOnlyDemo;
        private bool IsAWBStockPrepaid;
        private bool IsValid;
        private List<string> validationErrors;
        private string fromPortCode;
        private string toPortCode;
        private string leg1FromPortCode;
        private string leg1ToPortCode;
        private string leg2FromPortCode;
        private string leg2ToPortCode;
        private string finalDestinationPortCode;
        private string MessageTypeCode = "FFR";

        [WebMethod]
        public FFRResult SendFFR(string bookingId, int tenant, string recipient, bool iscancelled)
        {
            myResult = new FFRResult()
            {
                Id = tenant,
                IsValid = true,
                IsUpgradingChamp = false,
            };

            this.IsValid = true;
            this.myBookingId = bookingId;
            this.myTenant = tenant;
            this.cancellationSent = iscancelled;
            this.myRecipient = recipient;
            this.TodayDate = TenantServerConfigration.GetCurrentDateTime(myTenant).Date;
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(myTenant);

            this.GetGlobalVariables();

            if (IsValid)
            {
                this.GetBookingObject();
                this.CheckStockValidity();
                this.Validate();
            }

            if (IsValid)
            {
                myResult.IsValid = IsValid;

                if (commonContext == null)
                {
                    commonContext = CommonDataContext.GetContext(tenant);
                }

                documentRepository = new DocumentRepository(commonContext);
                communicationLogRepository = new CommunicationLogRepository(commonContext);
                contactRepository = new ContactRepository(commonContext);
                portRepository = new PortRepository(commonContext);

                Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                this.loggedContactId = loggedContact.Id;

                this.GetPortsCodes();
                this.StartSending();
                this.UpdateStock();
            }

            return myResult;
        }

        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (this.myTenant != 290)
                {
                    SettingRepository settingRepository = new SettingRepository();
                    Setting setting = settingRepository.GetSingleSetting("1");
                    if (setting != null)
                    {
                        if (setting.IsUpgradingChamp)
                        {
                            this.IsValid = false;
                            this.myResult.IsValid = false;
                            this.myResult.IsUpgradingChamp = setting.IsUpgradingChamp;
                        }
                    }
                }

                if (this.IsValid)
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(myTenant);
                    if (tenantManagement != null)
                    {
                        TTY = tenantManagement.TTY;
                        PIMA = tenantManagement.PIMA;
                        IsEAWBOnlyDemo = tenantManagement.IsEAWBOnlyDemo;
                        IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid;
                        AWBMessagesCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;
                    }

                    if (myTenant == 65 || IsEAWBOnlyDemo)
                    {
                        this.IsDemoTenant = true;
                        this.myResult.IsDemoTenant = true;
                    }
                }

                scope.Complete();
            }
        }
        private void GetBookingObject()
        {
            if (bookingContext == null)
            {
                bookingContext = BookingContext.GetContext(myTenant);
            }

            if (shipmentContext == null)
            {
                shipmentContext = ShipmentsContext.GetContext(myTenant);
            }

            lastRequestRepository = new BookingLastRequestRepository(bookingContext);
            bookingAnswerRepository = new BookingAnswerRepository(bookingContext);
            bookingRepository = new BookingRepository(bookingContext);

            myBooking = bookingRepository.GetSingle(myBookingId, myTenant);

            if (IsAWBStockPrepaid)
            {
                this.stockRepository = new MessagingStockRepository(shipmentContext);
                this.usageHistoryRepository = new MessagingStockUsageHistoryRepository(shipmentContext);
            }
        }
        private void CheckStockValidity()
        {
            if (!this.cancellationSent)
            {
                if (!IsDemoTenant)
                {
                    if (IsAWBStockPrepaid)
                    {
                        IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(myTenant, "Champ");
                        IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(myTenant);

                        myStocksData = myStocksData.Where(d => d.StartDate <= TodayDate && d.EndDate > TodayDate && d.Remaining > 0 && !d.IsCancelled);

                        int sendingCount = 0;
                        int? myStocksRemaining = 0;
                        if (myStocksData.Count() > 0)
                        {
                            myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                        }

                        if (!myUsageHistoryData.Where(d => d.EntityId == myBooking.Id && d.MessageType == this.MessageTypeCode).Any())
                        {
                            sendingCount = 1;
                        }

                        if (sendingCount > 0)
                        {
                            if (myStocksRemaining < sendingCount)
                            {
                                this.IsValid = false;
                                this.myResult.IsValid = false;
                                this.myResult.HasStockError = true;
                            }
                        }
                    }
                }
            }
        }

        private void Validate()
        {
            this.validationErrors = new List<string>();

            if (!IsEAWBOnlyDemo)
            {
                if (AWBMessagesCCSTypeCode == "GLSHK")
                {
                    if (string.IsNullOrEmpty(PIMA))
                    {
                        this.IsValid = false;
                        validationErrors.Add("PIMA field is required");
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(TTY))
                    {
                        this.IsValid = false;
                        validationErrors.Add("TTY field is required");
                    }
                }
            }

            if (myBooking != null)
            {
                if (string.IsNullOrEmpty(myBooking.Master))
                {
                    this.IsValid = false;
                    validationErrors.Add("Master field is required");
                }

                if (string.IsNullOrEmpty(myBooking.MainCarriageCarrierNumber) || string.IsNullOrEmpty(myBooking.MainCarriageCarrierPrefix))
                {
                    this.IsValid = false;
                    validationErrors.Add("Flight Number field is required");
                }

                if (myBooking.MainCarriageETD == null)
                {
                    this.IsValid = false;
                    validationErrors.Add("Main Carriage ETD field is required");
                }

                if (myBooking.GrossWeight == null || myBooking.GrossWeight == 0)
                {
                    this.IsValid = false;
                    validationErrors.Add("Gross Weight cannot be zero");
                }

                if (myBooking.ChargeableWeight == null || myBooking.ChargeableWeight == 0)
                {
                    this.IsValid = false;
                    validationErrors.Add("Chargeable Weight cannot be zero");
                }

                if (myBooking.NumberOfPackages == null || myBooking.NumberOfPackages == 0)
                {
                    this.IsValid = false;
                    validationErrors.Add("Number of Packages cannot be zero");
                }

                if (myBooking.Volume == null || myBooking.Volume == 0)
                {
                    this.IsValid = false;
                    validationErrors.Add("Volume cannot be zero");
                }

                if (myBooking.VolumetricWeight == null || myBooking.VolumetricWeight == 0)
                {
                    this.IsValid = false;
                    validationErrors.Add("Volumetric Weight cannot be zero");
                }
            }
        }
        private void GetPortsCodes()
        {
            if (!string.IsNullOrEmpty(myBooking.MainCarriageFromPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.MainCarriageFromPortId, myTenant);

                if (port != null)
                {
                    fromPortCode = port.Code;
                }
            }

            if (!string.IsNullOrEmpty(myBooking.MainCarriageToPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.MainCarriageToPortId, myTenant);

                if (port != null)
                {
                    toPortCode = port.Code;
                }
            }

            if (!string.IsNullOrEmpty(myBooking.Transshipment1FromPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.Transshipment1FromPortId, myTenant);

                if (port != null)
                {
                    leg1FromPortCode = port.Code;
                }
            }

            if (!string.IsNullOrEmpty(myBooking.Transshipment1ToPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.Transshipment1ToPortId, myTenant);

                if (port != null)
                {
                    leg1ToPortCode = port.Code;
                }
            }

            if (!string.IsNullOrEmpty(myBooking.Transshipment2FromPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.Transshipment2FromPortId, myTenant);

                if (port != null)
                {
                    leg2FromPortCode = port.Code;
                }
            }

            if (!string.IsNullOrEmpty(myBooking.Transshipment2ToPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.Transshipment2ToPortId, myTenant);

                if (port != null)
                {
                    leg2ToPortCode = port.Code;
                }
            }

            if (!string.IsNullOrEmpty(myBooking.MainCarriageFinalDestinationPortId))
            {
                Port port = portRepository.GetSinglePort(myBooking.MainCarriageFinalDestinationPortId, myTenant);

                if (port != null)
                {
                    finalDestinationPortCode = port.Code;
                }
            }
        }
        private void StartSending()
        {
            FFRDataContext dataContext = new FFRDataContext(myBooking);
            FFRDataBuilder dataBuilder = new FFRDataBuilder(dataContext, lastRequestRepository);

            CHAMP17.Envelope envelop = new CHAMP17.Envelope()
            {
                Sender = TTY,
                Recipient = myRecipient,
                Item = dataBuilder.GetBookingFFR(cancellationSent),
            };

            this.ModifyBookingRequestAndAnwer();
            this.UpdateBookingStatus();
            this.SendXMLFile(envelop, "champmessageoutqueue");
        }
        private void ModifyBookingRequestAndAnwer()
        {
            myBooking.HasResponse = false;
            myBooking.HasErrors = false;
            myBooking.FNAReason = null;
            myBooking.FMAAcknowledgementReason = null;
            myBooking.AnswerOtherServicesInformation = null;

            List<BookingLastRequest> requestsList = lastRequestRepository.GetBookingLastRequestsByBookingId(myBookingId, myTenant).ToList();
            List<BookingAnswer> answersList = bookingAnswerRepository.GetBookingAnswersForBookingTenant(myBookingId, myTenant).ToList();

            if (requestsList.Count != 0)
            {
                foreach (BookingLastRequest item in requestsList)
                {
                    lastRequestRepository.Remove(item);
                }
            }

            if (answersList.Count != 0)
            {
                foreach (BookingAnswer item in answersList)
                {
                    bookingAnswerRepository.Remove(item);
                }
            }

            BookingLastRequest entity = new BookingLastRequest()
            {
                Id = IdCounter.GetNumber("BookingLastRequest", myTenant),
                Tenant = myTenant,
                BookingId = myBookingId,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                ETD = myBooking.MainCarriageETD,
                FlightNumber = myBooking.MainCarriageCarrierPrefix + myBooking.MainCarriageCarrierNumber,
                Origin = fromPortCode,
                Destination = toPortCode,
                CarrierId = myBooking.MainCarriageCarrierId,
            };

            lastRequestRepository.Add(entity);

            if (!string.IsNullOrEmpty(myBooking.Transshipment1FromPortId))
            {
                BookingLastRequest entity1 = new BookingLastRequest()
                {
                    Id = IdCounter.GetNumber("BookingLastRequest", myTenant),
                    Tenant = myTenant,
                    BookingId = myBookingId,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                    ETD = myBooking.Transshipment1ETD,
                    FlightNumber = myBooking.Transshipment1CarrierPrefix + myBooking.Transshipment1CarrierNumber,
                    Origin = leg1FromPortCode,
                    Destination = leg1ToPortCode,
                    CarrierId = myBooking.Transshipment1CarrierId,
                };

                lastRequestRepository.Add(entity1);
            }

            if (!string.IsNullOrEmpty(myBooking.Transshipment2FromPortId))
            {
                BookingLastRequest entity2 = new BookingLastRequest()
                {
                    Id = IdCounter.GetNumber("BookingLastRequest", myTenant),
                    Tenant = myTenant,
                    BookingId = myBookingId,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                    ETD = myBooking.Transshipment2ETD,
                    FlightNumber = myBooking.Transshipment2CarrierPrefix + myBooking.Transshipment2CarrierNumber,
                    Origin = leg2FromPortCode,
                    Destination = leg2ToPortCode,
                    CarrierId = myBooking.Transshipment2CarrierId,
                };

                lastRequestRepository.Add(entity2);
            }
        }
        private string oldFFRStatus;
        private string oldBookingStatus;
        private void UpdateBookingStatus()
        {
            oldBookingStatus = myBooking.BookingStatusCode;
            oldFFRStatus = myBooking.FFRStatusCode;

            myBooking.WaitingForResponse = true;
            myBooking.HasResponse = false;
            myBooking.HasErrors = false;
            myBooking.FFRStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            myBooking.UpdateDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
            myBooking.LastSentByUserId = loggedContactId;

            if (cancellationSent)
            {
                myBooking.FFRStatusCode = "CRS";
            }
            else
            {
                myBooking.FFRStatusCode = "BRQ";
                myBooking.BookingStatusCode = "WCF";
            }

            this.DoTracing();
        }
        private void DoTracing()
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = myTenant,
                EventTypeCode = this.MessageTypeCode,
                UserId = null,
                EntityId = myBookingId,
                ObjectTableName = "Booking",
            });

            if (myBooking.BookingStatusCode != oldBookingStatus)
            {
                BookingStatusRepository statusRep = new BookingStatusRepository(myTenant);

                BookingStatus newStatus = statusRep.GetSingle(myBooking.BookingStatusCode);
                BookingStatus oldStatus = statusRep.GetSingle(oldBookingStatus);

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "BOKS",
                    UserId = null,
                    EntityId = myBookingId,
                    ObjectTableName = "Booking",
                    Notes = "Booking status changed from " + oldStatus.Name + " to " + newStatus.Name,
                });
            }

            if (myBooking.FFRStatusCode != oldFFRStatus)
            {
                FFRStatusRepository statusRep = new FFRStatusRepository(myTenant);

                FFRStatus newStatus = statusRep.GetSingle(myBooking.FFRStatusCode);
                FFRStatus oldStatus = statusRep.GetSingle(oldFFRStatus);

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = myTenant,
                    EventTypeCode = "BOKF",
                    UserId = null,
                    EntityId = myBookingId,
                    ObjectTableName = "Booking",
                    Notes = "Messaging status changed from " + oldStatus.Name + " to " + newStatus.Name,
                });
            }
        }
        private void SendXMLFile(object myEnvelop, string queueName)
        {
            this.GetObjectTableData();

            Type myType = myEnvelop.GetType();
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
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

            ser.Serialize(writer, myEnvelop, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();

            content = content.Replace(" />", "/>");

            byte[] bytearray = Encoding.ASCII.GetBytes(content);
            this.BuildCommunicationLog(bytearray.Length);
            this.BuildTransmissionLog();

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = myTenant,
                FileSize = memstream.Length,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(memstream.ToArray(), fileInfo);

            this.SaveChanges();

            if (IsDemoTenant)
            {
                this.BuildDemoResponse();
            }

            else
            {
                try
                {
					//using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
					//{
					//    BrokeredMessage message = new BrokeredMessage();
					//    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(new TimeSpan(0, 0, 3));

					//    message.Properties["CommunicationLogId"] = myCommunicationLogId;
					//    message.Properties["Tenant"] = myTenant;

					//    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName);//"emailqueue"
					//    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
					//    client.Send(message);

					//    scope.Complete();
					//}
					//string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(queueName);//"emailqueue"
					DbQueueService queueservice = new DbQueueService("EmailQueue", myTenant);
					queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", myCommunicationLogId }, { "Tenant", myTenant.ToString() } });
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

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FFR web service", null, ip);
                }
            }
        }

        private void SaveChanges()
        {
            bookingContext.SaveChanges();
            commonContext.SaveChanges();
        }
        private void GetObjectTableData()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(myTenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("Booking", 0, true);
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }
        }
        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private void BuildCommunicationLog(int? fileSize)
        {
            string xmlTarget = "Champ";
            string xmlSubject = "FFR";

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = Convert.ToInt32(myTenant),
                Id = IdCounter.GetNumber("Document", myTenant),
                HasFile = true,
                Folder = xmlTarget.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", myTenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                EntityId = myBookingId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = myTenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(myTenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = loggedContactId,
                DocumentId = document.Id,
                EntityReference = myBooking.BookingNumber,
                SearchFields = myBooking.BookingNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = DateTime.UtcNow,
                AWBNumber = myBooking.AirlinePrefix + "-" + myBooking.Master,
            };

            if (IsDemoTenant)
            {
                commLog.CommunicationStatusTypeCode = "E";
                commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
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
            MessagesTransmissionHelper TransmissionHelper = new MessagesTransmissionHelper(this.myTenant, "FFR");
            TransmissionHelper.Build(this.myBooking);
        }
        private void BuildDemoResponse()
        {
            SimulatorArgs args = new SimulatorArgs() 
            {
                Id = myTenant,
                Tenant = myTenant,
                MessageIdentifier = "FFA",
                Master = myBooking.Master,
                AirlinePrefix = myBooking.AirlinePrefix,
                EntityId = myBooking.Id,
                EntityName = "Booking",
                AirlineId = myBooking.MainCarriageCarrierId,
            };

            args.FFA = new SimulatorFFA() 
            {
                Weight = myBooking.GrossWeight,
                NumberOfPieces = myBooking.NumberOfPackages == null ? 0 : myBooking.NumberOfPackages.Value,
                DescriptionOfGoods = myBooking.DescriptionOfGoods,
                SpecialServicesRequest = myBooking.SpecialServicesRequest,

                MainCarriageFromPortId = myBooking.MainCarriageFromPortId,
                MainCarriageToPortId = myBooking.MainCarriageFinalDestinationPortId,
                MainCarriageETD = myBooking.MainCarriageETD,
                MainCarriageCarrierId = myBooking.MainCarriageCarrierId,
                MainCarriageCarrierCode = myBooking.MainCarriageCarrierPrefix,
                MainCarriageCarrierNumber = myBooking.MainCarriageCarrierNumber,
                MainCarriageSpaceAllocationCode = cancellationSent ? "CN" : "KK",
                OSIFirstLine = "THIS IS A DEMO MESSAGE",
            };

            Simulator sim = new Simulator(args);
            sim.Run();
        }

        #region UpdateStock
        public void UpdateStock()
        {
            if (!this.cancellationSent)
            {
                if (!IsDemoTenant)
                {
                    if (IsAWBStockPrepaid)
                    {
                        IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(myTenant, "Champ");
                        IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(myTenant);

                        myStocksData = myStocksData.Where(d => d.StartDate <= TodayDate && d.EndDate > TodayDate && d.Remaining > 0 && !d.IsCancelled);

                        MessagingStockUsageHistory usageHistory = myUsageHistoryData.Where(d => d.EntityId == myBookingId && d.MessageType == MessageTypeCode).FirstOrDefault();

                        if (usageHistory != null)
                        {
                            usageHistory.ActionType = "Transmission";
                            usageHistory.LastActionDate = TenantServerConfigration.GetCurrentDateTime(myTenant);
                            usageHistory.LastActionByUserId = loggedContactId;
                            usageHistoryRepository.Update(usageHistory);
                            usageHistoryRepository.SubmitChanges();
                        }

                        else
                        {
                            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(myTenant);

                            MessagingStock myStock = myStocksData.OrderBy(o => o.EndDate).FirstOrDefault();

                            usageHistory = new MessagingStockUsageHistory()
                            {
                                Id = IdCounter.GetNumber("MessagingStockUsageHistory", myTenant),
                                Tenant = myTenant,
                                StockId = myStock.Id,
                                EntityId = myBooking.Id,
                                EntityNumber = myBooking.BookingNumber,
                                MessageType = MessageTypeCode,
                                ActionType = "Transmission",
                                FirstActionByUserId = loggedContactId,
                                FirstActionDate = todayDateTime,
                                LastActionByUserId = loggedContactId,
                                LastActionDate = todayDateTime,

                            };

                            if (!string.IsNullOrEmpty(myBooking.MainCarriageCarrierId))
                            {
                                AirlineRepository airlineRepository = new AirlineRepository(commonContext);
                                Airline airline = airlineRepository.GetSingleAirline(myBooking.MainCarriageCarrierId, myTenant);
                                if (airline != null)
                                {
                                    if (!string.IsNullOrEmpty(airline.Prefix) && !string.IsNullOrEmpty(myBooking.Master))
                                    {
                                        usageHistory.MAWB = airline.Prefix + "-" + myBooking.Master;
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
                    }
                }
            }
        }
        #endregion
    }

    public class FFRResult
    {
        [Key]
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsUpgradingChamp { get; set; }
        public bool HasStockError { get; set; }
        public FFRResult()
        {
            this.IsValid = true;
        }
    }
}
