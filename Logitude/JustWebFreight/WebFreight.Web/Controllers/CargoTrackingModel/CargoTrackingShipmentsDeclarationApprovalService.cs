using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using System;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public class CargoTrackingShipmentsDeclarationApprovalService
    {
        CargoTrackingShipmentPM cargoTrackingShipmentPM;
        public void HandleDeclarationApproval(DeclarationApprovalArgs declarationApprovalArgs)
        {
            GetCargoShipment(declarationApprovalArgs);

            if (declarationApprovalArgs.Approved == true)
                ApproveDeclaration(declarationApprovalArgs);
            else if (declarationApprovalArgs.Denied == true)
                DeclineDeclaration(declarationApprovalArgs);
        }

        private void GetCargoShipment(DeclarationApprovalArgs declarationApprovalArgs)
        {
            CargoTrackingShipmentQueryService cargoTrackingShipmentQuery = new CargoTrackingShipmentQueryService(declarationApprovalArgs.Tenant);
            cargoTrackingShipmentPM = cargoTrackingShipmentQuery.GetSinglePMBySecurityKey(declarationApprovalArgs.ShipmentSecurityKey, declarationApprovalArgs.Tenant);
        }
        private void ApproveDeclaration(DeclarationApprovalArgs declarationApprovalArgs)
        {
            SetCloudDataAsApproved(declarationApprovalArgs);
            SendDeclarationApproveTask(declarationApprovalArgs);
        }
        private void DeclineDeclaration(DeclarationApprovalArgs declarationApprovalArgs)
        {
            SetCloudDataAsDeclined(declarationApprovalArgs);
            SendDeclarationDeclineTask(declarationApprovalArgs);
        }

        private void SetCloudDataAsApproved(DeclarationApprovalArgs declarationApprovalArgs)
        {
            ShipmentAdditionalCloudData cloudData = GetCloudDataBySecurityKey(declarationApprovalArgs);
            if (cloudData != null)
            {
                UpdateDeclarationVersion(cloudData);

                cloudData.ApproveDateTime = TenantServerConfigration.GetCurrentDateTime(declarationApprovalArgs.Tenant);
                SubmitCloudData(declarationApprovalArgs, cloudData);
            }
        }

        private void UpdateDeclarationVersion(ShipmentAdditionalCloudData cloudData)
        {
            string xmlData = DeserializeDeclarationXMLData(cloudData.DeclarationXmlData);
            string declarationVersion = GetDeclarationVersionFromXML(xmlData);

            if (declarationVersion != null)
                cloudData.VersionApproved = declarationVersion;
        }

        private static string GetDeclarationVersionFromXML(string data_out)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(data_out);
            XmlNodeList VersionId = xmldoc.GetElementsByTagName("version_id");
            string declarationVersion = null;
            if (VersionId[0] != null)
            {
                declarationVersion = VersionId[0].InnerText;
            }

            return declarationVersion;
        }

        private string DeserializeDeclarationXMLData(string declarationXmlData)
        {
            int defaultSize = 1024;
            byte[] bytesUncompressed = new byte[defaultSize];
            StringBuilder encodedUncompressMessage = new StringBuilder();
            try
            {
                var memoryStream = new MemoryStream(Convert.FromBase64String(declarationXmlData));
                var zipInputStream = new BZip2InputStream(memoryStream);
                StringBuilder MyUncompressMessage = new StringBuilder();

                Encoding windows1252Encoding = Encoding.GetEncoding(1255);
                Encoding utf8Encoding = Encoding.UTF8;
                byte[] utf8Bytes = new byte[defaultSize];
                while (true)
                {
                    defaultSize = zipInputStream.Read(bytesUncompressed, 0, defaultSize);
                    if (defaultSize > 0)
                    {
                        utf8Bytes = Encoding.Convert(windows1252Encoding, utf8Encoding, bytesUncompressed, 0, defaultSize);
                        encodedUncompressMessage.Append(Encoding.UTF8.GetString(utf8Bytes));
                        MyUncompressMessage.Append(Encoding.UTF8.GetString(bytesUncompressed, 0, defaultSize));
                    }
                    else
                        break;
                }
            }
            catch (Exception)
            {
                encodedUncompressMessage.Append(Encoding.UTF8.GetString(Convert.FromBase64String(declarationXmlData)));
            }

            var data_out = encodedUncompressMessage.ToString();
            return data_out;
        }

        public void SendDeclarationApproveTask(DeclarationApprovalArgs declarationApprovalArgs)
        {

            CommunicationsParams communicationParameters = CreateCommunicationParametersForApproval(declarationApprovalArgs);

            List<QueueTask> queueTasks = CreateDeclarationApproveTask(declarationApprovalArgs);

            communicationParameters.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);

            Communications.AddCommunicationLog(communicationParameters);

        }


        private void SetCloudDataAsDeclined(DeclarationApprovalArgs declarationApprovalArgs)
        {
            ShipmentAdditionalCloudData cloudData = GetCloudDataBySecurityKey(declarationApprovalArgs);
            if (cloudData != null)
            {
                cloudData.DenyReason = declarationApprovalArgs.DenyReason;
                SubmitCloudData(declarationApprovalArgs, cloudData);
            }
        }

        public void SendDeclarationDeclineTask(DeclarationApprovalArgs declarationApprovalArgs)
        {

            CommunicationsParams comParams = CreateCommunicationParametersForDecline(declarationApprovalArgs);

            List<QueueTask> queueTasks = CreateDeclarationDeclineTask(declarationApprovalArgs);

            comParams.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);

            Communications.AddCommunicationLog(comParams);

        }

        private void SubmitCloudData(DeclarationApprovalArgs declarationApprovalArgs, ShipmentAdditionalCloudData cloudData)
        {
            ShipmentAdditionalCloudDataRepository cloudDataRepository = new ShipmentAdditionalCloudDataRepository(declarationApprovalArgs.Tenant);
            cloudDataRepository.Update(cloudData);
            cloudDataRepository.SubmitChanges();
        }

        private ShipmentAdditionalCloudData GetCloudDataBySecurityKey(DeclarationApprovalArgs declarationApprovalArgs)
        {
            ShipmentAdditionalCloudDataRepository cloudDataRepository = new ShipmentAdditionalCloudDataRepository(declarationApprovalArgs.Tenant);
            ShipmentAdditionalCloudData cloudData = cloudDataRepository.GetSingleShipmentAdditionalCloudData(cargoTrackingShipmentPM.EntityId, declarationApprovalArgs.Tenant);
            return cloudData;
        }

        private CommunicationsParams CreateCommunicationParametersForApproval(DeclarationApprovalArgs declarationApprovalArgs)
        {
            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode("CargoTrackingShipment", 0);

            CommunicationsParams comParams = new CommunicationsParams()
            {
                Tenant = declarationApprovalArgs.Tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = "VDA",
                Priority = 1,
                LoggingEntityId = cargoTrackingShipmentPM.EntityId,
                InOut = "O",
                Status = "W",
                LoggingObjectTableId = table?.Id,
                Subject = "Status Update",
                FolderName = "ExternalTasksQueue",
            };
            return comParams;
        }
        private CommunicationsParams CreateCommunicationParametersForDecline(DeclarationApprovalArgs declarationApprovalArgs)
        {
            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode("CargoTrackingShipment", 0);

            CommunicationsParams comParams = new CommunicationsParams()
            {
                Tenant = declarationApprovalArgs.Tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = "VDD",
                Priority = 1,
                InOut = "O",
                LoggingEntityId = cargoTrackingShipmentPM.EntityId,
                Status = "W",
                LoggingObjectTableId = table?.Id,
                Subject = "Status Update",
                FolderName = "ExternalTasksQueue",
            };
            return comParams;
        }

        private List<QueueTask> CreateDeclarationApproveTask(DeclarationApprovalArgs declarationApprovalArgs)
        {
            List<QueueTask> queueTasks = new List<QueueTask>
                {
                    new QueueTask()
                    {
                        Action = "StatusUpdate",
                        Parameters = new List<Parameter>()
                        {
                            new Parameter{ Name = "Code", Order = 0, Value = "VDA" }
                            ,new Parameter{ Name = "ShipmentSecurityKey", Order = 1, Value = declarationApprovalArgs.ShipmentSecurityKey }
                        }
                    }
                };
            return queueTasks;
        }

        private List<QueueTask> CreateDeclarationDeclineTask(DeclarationApprovalArgs declarationApprovalArgs)
        {
            List<QueueTask> queue1Tasks = new List<QueueTask>
                {
                    new QueueTask()
                    {
                        Action = "StatusUpdate",
                        Parameters = new List<Parameter>()
                        {
                            new Parameter{ Name = "Code", Order = 0, Value = "VDD" },
                            new Parameter{ Name = "ShipmentSecurityKey", Order = 1, Value = declarationApprovalArgs.ShipmentSecurityKey }
                        }
                    }
                };
            return queue1Tasks;
        }
    }


    public class DeclarationApprovalArgs
    {
        public int Tenant { get; set; }
        public string ShipmentSecurityKey { get; set; }
        public bool? Approved { get; set; }
        public bool? Denied { get; set; }
        public string DenyReason { get; set; }
    }
}