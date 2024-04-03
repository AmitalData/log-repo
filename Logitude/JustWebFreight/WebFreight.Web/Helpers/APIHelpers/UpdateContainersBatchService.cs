using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class UpdateContainersBatchService : BatchTaskExecutionsService
    {
        public UpdateContainersBatchService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(UpdateContainersServiceArgs));
            UpdateContainersServiceArgs args = serializer.Deserialize(stringReader) as UpdateContainersServiceArgs;
            this.RunBatchService(args, BatchTaskExecution.Id);
        }

        public void RunBatchService(UpdateContainersServiceArgs serviceArgs, string batchTaskExecutionId)
        {
            List<ContainerPM> containers = this.GetContainers(serviceArgs);

            if (containers == null || containers.Count == 0) return;

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(serviceArgs.Tenant);
                ContainerService containerService = new ContainerService(shipmentsContext, serviceArgs.Tenant);
                foreach(ContainerPM container in containers)
                {
                    containerService.Update(container);
                }

                scope.Complete();
            }
        }
        private List<ContainerPM> GetContainers(UpdateContainersServiceArgs serviceArgs)
        {
            IBlobService iBlobService = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                Tenant = serviceArgs.Tenant,
                FileName = serviceArgs.FileName,
                FolderName = "others",
                Extension = "xml",
            };

            byte[] messageBody = iBlobService.Read(fileInfo);

            MemoryStream memoryStream = new MemoryStream(messageBody);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(memoryStream);
            memoryStream.Position = 0;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ContainerPM>));
            return (List<ContainerPM>)xmlSerializer.Deserialize(memoryStream);
        }
    }
}
