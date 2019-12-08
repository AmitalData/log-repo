using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ConsolidationService
    {
        private int tenant;
        private ARInvoicePM entityPM;
        private string blobFileName;
        public ConsolidationService()
        {

        }

        public void RunService(ARInvoicePM entityPM)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;

            this.InitValues();
            this.BuildXMLFile();
            this.SendBatchService();
        }

        private void InitValues()
        {
            this.entityPM.IsFromConsolidationBatch = true;

            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);

            if (entityPM.Id == null)
            {
                entityPM.IsCreatingConsolidation = true;
                entityPM.Id = IdCounter.GetNumber("ARInvoice", entityPM.Tenant).ToString();
                entityPM.CreatedByUserId = loggedContact.Id;
            }

            entityPM.UpdatedByUserId = loggedContact.Id;
        }
        private void BuildXMLFile()
        {
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ARInvoicePM));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace
            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);

            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, entityPM, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();

            content = content.Replace(" />", "/>");

            byte[] bytearray = Encoding.UTF8.GetBytes(content);

            if (bytearray != null)
            {
                this.blobFileName = "ConsolidationServiceBatch_" + IdCounter.GetNumber("ConsolidationServiceBatch", tenant);

                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = blobFileName,
                    Extension = "xml",
                    Tenant = tenant,
                    FileSize = bytearray.Length,
                    FolderName = "others",
                };

                Logitude.Server.Tools.StorageService.IBlobService iBlobService = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                iBlobService.Write(bytearray, fileInfo);
            }
        }
        private void SendBatchService()
        {
            ConsolidationServiceArgs args = new ConsolidationServiceArgs()
            {
                Tenant = tenant,
                FileName = this.blobFileName,
            };

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(ConsolidationServiceArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();

            BatchTaskExecutionRepository iRepository = new BatchTaskExecutionRepository(tenant);

            BatchTaskExecution iBatchTaskExecution = new BatchTaskExecution()
            {
                Id = IdCounter.GetNumber("BatchTaskExecution", tenant),
                Subject = "Update Consolidation Shipments",
                Tenant = tenant,
                ClassName = "WebFreight.Web.Helpers.APIHelpers.ConsolidationServiceBatch,WebFreight.Web",

                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreatedByUserId = this.entityPM.UpdatedByUserId,
                PrametersXml = xmlParameters,
                StatusCode = "C",
            };

            iRepository.Add(iBatchTaskExecution);
            iRepository.SubmitChanges();

            this.entityPM.BatchTaskExecutionId = iBatchTaskExecution.Id;

            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            bool isLocalHost = false;
            if (url != null)
            {
                if (url.ToLower().Contains("localhost"))
                {
                    isLocalHost = true;
                }
            }

            //if (isLocalHost)
            //{
            //    this.RunBatchService(args, iBatchTaskExecution.Id);
            //}

            //else
            //{
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                    {
                        { "BatchTaskExecutionId", iBatchTaskExecution.Id },
                        { "Tenant", tenant.ToString() }
                    });
            //}
        }

        public void RunBatchService(ConsolidationServiceArgs serviceArgs, string batchTaskExecutionId)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.RetrieveInvoicePM(serviceArgs);

                this.UpdateProgressPercentage(20, batchTaskExecutionId, serviceArgs.Tenant);

                if (entityPM != null)
                {
                    if (entityPM.IsConsolidationInvoice)
                    {                        
                        //bool isVoiding = entityPM.SetVoided;
                        //bool isApproving = entityPM.SetApproved;

                        IInvoiceContext invoiceContext = InvoiceContext.GetContext(entityPM.Tenant);
                        ARInvoiceService service = new ARInvoiceService(invoiceContext, entityPM.Tenant);

                        this.UpdateProgressPercentage(20, batchTaskExecutionId, serviceArgs.Tenant);

                        if (entityPM.IsCreatingConsolidation)
                        {
                            service.Create(entityPM);
                        }

                        else
                        {
                            service.Update(entityPM);
                        }

                        this.UpdateProgressPercentage(20, batchTaskExecutionId, serviceArgs.Tenant);

                        invoiceContext.SaveChanges();

                        this.UpdateProgressPercentage(20, batchTaskExecutionId, serviceArgs.Tenant);

                        service.UpdateConsolidationShipments();

                        this.UpdateProgressPercentage(20, batchTaskExecutionId, serviceArgs.Tenant);
                    }
                }

                scope.Complete();
            }
        }

        private void RetrieveInvoicePM(ConsolidationServiceArgs serviceArgs)
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

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ARInvoicePM));
            this.entityPM = (ARInvoicePM)xmlSerializer.Deserialize(memoryStream);
            this.tenant = this.entityPM.Tenant;
        }

        private void UpdateProgressPercentage(int step, string batchTaskExecutionId, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                BatchTaskExecutionRepository iRepository = new BatchTaskExecutionRepository(tenant);
                BatchTaskExecution iBatchTaskExecution = iRepository.GetSingle(batchTaskExecutionId, tenant);
                if (iBatchTaskExecution != null)
                {
                    if (iBatchTaskExecution.ProgressPercentage <= 1)
                    {
                        iBatchTaskExecution.ProgressPercentage = step;
                    }

                    else
                    {
                        iBatchTaskExecution.ProgressPercentage += step;
                    }

                    if (iBatchTaskExecution.ProgressPercentage > 100)
                    {
                        iBatchTaskExecution.ProgressPercentage = 100;
                    }

                    iRepository.Update(iBatchTaskExecution);
                    iRepository.SubmitChanges();
                }

                scope.Complete();
            }
        }

    }

    public class ConsolidationServiceArgs
    {
        public int Tenant { get; set; }
        public string FileName { get; set; }
    }

    public class ConsolidationServiceArgsItem
    {
        public string ShipmentId { get; set; }
        public string ConstituentId { get; set; }
    }
}
