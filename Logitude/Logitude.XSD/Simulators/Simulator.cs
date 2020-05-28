using Logitude.XSD.Analyzers.CHAMPAnalyzer;
using Logitude.XSD.Analyzers.GLSHKAnalyzer;
using Logitude.XSD.Simulators.CHAMPSimulators;
using Logitude.XSD.Simulators.GLSHKSimulators;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using Logitude.Server.Tools.QueueService;

namespace Logitude.XSD.Simulators
{
    public class Simulator
    {
        public int Tenant { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string AirlineTTY { get; set; }
        public string AirlinePIMA { get; set; }
        public string CCSTypeCode { get; set; }
        public SimulatorArgs Args { get; set; }
        public SimulatorResult Result { get; set; }
        public bool IsChampSimulator { get; set; }
        public Simulator(SimulatorArgs args)
        {
            this.Args = args;
            this.Tenant = args.Tenant;
            this.Result = new SimulatorResult() { Id = Tenant };

            if (args.MessageIdentifier == "AnalyzeQueueId")
            {

            }

            else
            {
                this.GetGlobalVariables();
                this.DefineSimulatorType();
            }
        }

        public void Run()
        {
            this.Validate();

            if (Result.IsValid)
            {
                if (Args.MessageIdentifier == "AnalyzeQueueId")
                {
                    AnalyzeQueue analyzeQueue = null;
                    AnalyzeQueueRepository analyzeQueueReposiory = null;

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        analyzeQueueReposiory = new AnalyzeQueueRepository();
                        analyzeQueue = analyzeQueueReposiory.GetSingleAnalyzeQueue(Args.AnalyzeQueueId);
                        scope.Complete();
                    }

                    if (analyzeQueue != null)
                    {
                        CHAMPAnalyzer analyzer = new CHAMPAnalyzer(analyzeQueue, analyzeQueueReposiory);
                        analyzer.Run();
                    }
                }

                else
                {
                    string xmlString = null;

                    if (Args.MessageIdentifier == "XML")
                    {
                        xmlString = Args.XmlText;
                    }

                    else
                    {
                        if (IsChampSimulator)
                        {
                            CHAMPSimulator mySimulator = new CHAMPSimulator(Args, TTY, AirlineTTY);
                            mySimulator.Run();

                            xmlString = this.BuildXML(mySimulator.Envelope);
                        }

                        else
                        {
                            GLSHKSimulator mySimulator = new GLSHKSimulator(Args, PIMA, AirlinePIMA);
                            mySimulator.Run();

                            xmlString = this.BuildXML(mySimulator.Message);
                        }
                    }

                    if (!string.IsNullOrEmpty(xmlString))
                    {
                        this.BuildAnalyzeQueue(xmlString);
                    }
                }
            }
        }

        private void Validate()
        {
            List<string> errors = new List<string>();

            if (Args.MessageIdentifier == "AnalyzeQueueId")
            {
                if (string.IsNullOrEmpty(Args.AnalyzeQueueId))
                {
                    errors.Add("AnalyzeQueueId is missing");
                }
            }

            else if (Args.MessageIdentifier == "XML")
            {
                if (string.IsNullOrEmpty(Args.XmlText))
                {
                    errors.Add("Xml text is missing");
                }

                else
                {
                    bool isXMLValid = this.ValidateXMLText(Args.XmlText);
                    if (!isXMLValid)
                    {
                        errors.Add("Xml text is invalid");
                    }
                }
            }

            else
            {
                if (Args.MessageIdentifier == "FVA")
                {
                    if (string.IsNullOrEmpty(Args.AirlineId))
                    {
                        errors.Add("Airline field is missing");
                    }

                    if (string.IsNullOrEmpty(Args.FVA.FromPortId))
                    {
                        errors.Add("From Port is missing");
                    }

                    if (string.IsNullOrEmpty(Args.FVA.ToPortId))
                    {
                        errors.Add("To Port field is missing");
                    }

                    if (Args.FVA.ETD == null)
                    {
                        errors.Add("ETD field is missing");
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(Args.Master))
                    {
                        errors.Add("Master field is missing");
                    }

                    if (string.IsNullOrEmpty(Args.AirlinePrefix))
                    {
                        errors.Add("Airline Prefix field is missing");
                    }

                    if (string.IsNullOrEmpty(Args.AirlineId))
                    {
                        errors.Add("Airline field is missing");
                    }

                    if (Args.EntityName == "Shipment")
                    {
                        if (Args.ShipmentLevelCode == "H")
                        {
                            if (string.IsNullOrEmpty(Args.House))
                            {
                                errors.Add("House field is missing");
                            }
                        }
                    }

                    if (IsChampSimulator)
                    {
                        if (string.IsNullOrEmpty(TTY))
                        {
                            errors.Add("Tenant communication parameter (TTY) is missing");
                        }

                        if (Args.AirlineId != null)
                        {
                            AirlineRepository myRepository = new AirlineRepository(Tenant);
                            Airline myAirline = myRepository.GetSingleAirline(Args.AirlineId, Tenant);
                            Airline myZeroAirline = myRepository.GetSingleAirlineByCode(myAirline.Card.Code, 0);
                            AirlineTTY = myZeroAirline.TTY;

                            if (string.IsNullOrEmpty(myZeroAirline.TTY))
                            {
                                errors.Add("This Airline doesn't support transmitting messages");
                            }
                        }
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(PIMA))
                        {
                            errors.Add("Tenant communication parameter (PIMA) is missing");
                        }

                        if (Args.AirlineId != null)
                        {
                            AirlineRepository myRepository = new AirlineRepository(Tenant);
                            Airline myAirline = myRepository.GetSingleAirline(Args.AirlineId, Tenant);
                            Airline myZeroAirline = myRepository.GetSingleAirlineByCode(myAirline.Card.Code, 0);
                            AirlinePIMA = myZeroAirline.GLSHKPIMA;

                            if (string.IsNullOrEmpty(myZeroAirline.GLSHKPIMA))
                            {
                                errors.Add("Airline communication parameter (PIMA) is missing");
                            }
                        }

                        if (string.IsNullOrEmpty(Args.ShipmentNumber))
                        {
                            errors.Add("Shipment Number field is missing");
                        }
                    }

                    switch (Args.MessageIdentifier)
                    {
                        case "FNA":
                            {
                                if (string.IsNullOrEmpty(Args.ReasonForRejection))
                                {
                                    errors.Add("Reason For Rejection field is missing");
                                }

                                break;
                            }

                        case "FMA":
                            {
                                if (!IsChampSimulator)
                                {
                                    if (string.IsNullOrEmpty(Args.ReasonForAcknowledgement))
                                    {
                                        errors.Add("Reason For Rejection field is missing");
                                    }
                                }

                                break;
                            }

                        case "FSU":
                            {
                                if (Args.FSA.AllStatusCodes.Count == 0)
                                {
                                    errors.Add("Please select one status at least");
                                }

                                if (string.IsNullOrEmpty(Args.FSA.FromPortId))
                                {
                                    errors.Add("From Port field is missing");
                                }

                                if (string.IsNullOrEmpty(Args.FSA.ToPortId))
                                {
                                    errors.Add("To Port field is missing");
                                }

                                if (string.IsNullOrEmpty(Args.FSA.FlightNumber))
                                {
                                    errors.Add("Flight Number field is missing");
                                }

                                break;
                            }

                        case "FSA":
                            {
                                if (string.IsNullOrEmpty(Args.FSA.FromPortId))
                                {
                                    errors.Add("From Port field is missing");
                                }

                                if (string.IsNullOrEmpty(Args.FSA.ToPortId))
                                {
                                    errors.Add("To Port field is missing");
                                }

                                if (string.IsNullOrEmpty(Args.FSA.FlightNumber))
                                {
                                    errors.Add("Flight Number field is missing");
                                }

                                break;
                            }
                    }
                }
            }

            Result.Errors = errors;
            Result.IsValid = errors.Count == 0 ? true : false;
        }

        private bool ValidateXMLText(string xmlText)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlText);
                return true;
            }

            catch (XmlException ex)
            {
                return false;
            }
        }

        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(Tenant);

                if (tenantManagement != null)
                {
                    TTY = tenantManagement.TTY;
                    PIMA = tenantManagement.PIMA;
                    CCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;

                    if (CCSTypeCode != null)
                    {
                        CCSTypeCode = CCSTypeCode.ToUpper();
                    }
                }
            }
        }

        private void DefineSimulatorType()
        {
            bool isChampSimulator = false;

            if (this.Args.IsChampSimulator)
            {
                isChampSimulator = true;
            }

            else
            {
                if (Args.MessageIdentifier == "XML")
                {
                    if (Args.XmlText.Contains("<Message"))
                    {
                        isChampSimulator = false;
                    }

                    else
                    {
                        isChampSimulator = true;
                    }
                }

                else
                {
                    if (CCSTypeCode == "CHAMP")
                    {
                        isChampSimulator = true;
                    }

                    else if (Args.EntityName == "Booking" || Args.EntityName == "FlightsSchedulesRequest")
                    {
                        isChampSimulator = true;
                    }

                    else
                    {
                        switch (Args.MessageIdentifier)
                        {
                            case "FFA":
                            case "FVA":
                                {
                                    isChampSimulator = true;
                                    break;
                                }
                        }
                    }
                }
            }

            this.IsChampSimulator = isChampSimulator;
        }

        private string BuildXML(object xmlObject)
        {
            Type myType = xmlObject.GetType();
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            if (IsChampSimulator)
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

            XmlWriter writer = XmlTextWriter.Create(memoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            xmlSerializer.Serialize(writer, xmlObject, ns);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(memoryStream);
            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            return xmlString;
        }

        private void BuildAnalyzeQueue(string xmlString)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            byte[] messageBytes = Encoding.ASCII.GetBytes(xmlString);

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = IsChampSimulator ? "Champ" : "GLSHK",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                Tenant = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = xmlString.Length,
            };

            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();

            if (this.Args.IsLocalAnalyze)
            {
                if (IsChampSimulator)
                {
                    CHAMPAnalyzer analyzer = new CHAMPAnalyzer(analyzeQueue, analyzeQueueReposiory);
                    analyzer.Run();
                }

                else
                {
                    GLSHKAnalyzer analyzer = new GLSHKAnalyzer(analyzeQueue, analyzeQueueReposiory);
                    analyzer.Run();
                }
            }

            else
            {
                DbQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ChampAnalyzer", 0);
                queueservice.Send(new Dictionary<string, string>() { { "AnalyzeQueueId", analyzeQueue.Id } });
                queueservice.Complete();
            }
        }
    }

    public class SimulatorResult
    {
        [Key]
        public int Id { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
        public SimulatorResult()
        {
            this.IsValid = true;
            this.Errors = new List<string>();
        }
    }
}
