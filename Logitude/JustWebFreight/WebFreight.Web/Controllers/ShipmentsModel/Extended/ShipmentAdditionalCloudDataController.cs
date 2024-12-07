using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel.Extended
{
    public class ShipmentAdditionalCloudDataController : ApiController
    {

        public HttpResponseMessage GetSingle(string id)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ShipmentAdditionalCloudDataRepository Repository = new ShipmentAdditionalCloudDataRepository(authToken.Tenant);

                ShipmentAdditionalCloudData data = Repository.GetSingleShipmentAdditionalCloudData(id, authToken.Tenant);

                ShipmentCloudCustomDataDeserializer deserializer = new ShipmentCloudCustomDataDeserializer();
                ShipmentAdditionalCloudCustomData CustomData = deserializer.BuildCustomDataFromXML(data);

                return Request.CreateResponse(HttpStatusCode.OK, CustomData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public ShipmentAdditionalCloudCustomData BuildCustomDataFromXML(ShipmentAdditionalCloudData data)
        {
            ShipmentAdditionalCloudCustomData CustomData = new ShipmentAdditionalCloudCustomData();
            CustomData.IsPaymentRequired = data.IsPaymentRequired;
            CustomData.PaymentDateTime = data.PaymentDateTime;
            if (data != null && !string.IsNullOrEmpty(data.DeclarationXmlData))
            {
                //byte[] myByteArray = Convert.FromBase64String(data.DeclarationXmlData);
                //using (var mem = new MemoryStream(myByteArray))
                //using (var zipStream = new ZipInputStream(mem))
                //{
                //    ZipEntry currentEntry;
                //    while ((currentEntry = zipStream.GetNextEntry()) != null)
                //    {
                //        //Console.WriteLine("{0} is {1} bytes", currentEntry.Name, currentEntry.Size);
                //        byte[] zipdata = new byte[currentEntry.Size];
                //        //ZipEntry.Read(data, 0, zipdata.Length);

                //        // do what ever with the data
                //    }
                //}
                int MySize = 1024;
                byte[] BytesUncompressed = new byte[MySize];
                StringBuilder MyEncodedUncompressMessage = new StringBuilder();
                try
                {
                    var MyMemoryStream = new MemoryStream(Convert.FromBase64String(data.DeclarationXmlData));
                    var MyZipInputStream = new BZip2InputStream(MyMemoryStream);
                    StringBuilder MyUncompressMessage = new StringBuilder();


                    Encoding wind1252 = Encoding.GetEncoding(1255);
                    Encoding utf8 = Encoding.UTF8;
                    byte[] utf8Bytes = new byte[MySize];
                    //byte[] MyBytesUncompressed = new byte[MyMemoryStream.Length];
                    while (true)
                    {
                        MySize = MyZipInputStream.Read(BytesUncompressed, 0, MySize);
                        //MyBytesUncompressed = (MyBytesUncompressed.Concat(BytesUncompressed)).ToArray();
                        if (MySize > 0)
                        {

                            utf8Bytes = Encoding.Convert(wind1252, utf8, BytesUncompressed, 0, MySize);
                            MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(utf8Bytes));
                            MyUncompressMessage.Append(Encoding.UTF8.GetString(BytesUncompressed, 0, MySize));
                        }

                        else
                            break;
                    }
                }
                catch (Exception)
                {
                    MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(Convert.FromBase64String(data.DeclarationXmlData)));
                }


                //Encoding wind1252 = Encoding.GetEncoding(1255);
                //Encoding utf8 = Encoding.UTF8;
                //byte[] wind1252Bytes = MyBytesUncompressed;
                //byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                //string utf8String = Encoding.UTF8.GetString(utf8Bytes);

                var data_out = MyEncodedUncompressMessage.ToString();
                BytesUncompressed = null;
                //MyBytesUncompressed = null;
                //MyUncompressMessage.Remove(0, MyUncompressMessage.Length);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(data_out);

                CustomData.IsImporterApprovalRequried = data.IsImporterApprovalRequried;
                CustomData.ApprovedByUserName = data.ApprovedByUserName;
                CustomData.VersionApproved = data.VersionApproved;
                CustomData.ApproveDateTime = data.ApproveDateTime;
                CustomData.DenyReason = data.DenyReason;
                CustomData.DocumentsApprovedByUserName = data.DocumentsApprovedByUserName;
                CustomData.GoodsValueDetails = new List<GoodsValueDetails>();
                CustomData.TaxesDetails = new List<TaxesDetails>();

                XmlNodeList CustomsFileNo = xmldoc.GetElementsByTagName("customs_file_num");
                if (CustomsFileNo[0] != null)
                {
                    CustomData.CustomsFileNo = CustomsFileNo[0].InnerText;
                }

                XmlNodeList DeclarationNo = xmldoc.GetElementsByTagName("declaration_num");
                if (DeclarationNo[0] != null)
                {
                    CustomData.DeclarationNo = DeclarationNo[0].InnerText;
                }

                XmlNodeList MishgorDescOfGoods1 = xmldoc.GetElementsByTagName("mishgor-desc_of_goods1");
                if (MishgorDescOfGoods1[0] != null)
                {
                    CustomData.MishgorDescOfGoods1 = MishgorDescOfGoods1[0].InnerText;
                }

                XmlNodeList GoodsValue = xmldoc.GetElementsByTagName("goods_value");
                if (GoodsValue[0] != null)
                {
                    CustomData.GoodsValue = GoodsValue[0].InnerText;
                }

                XmlNodeList CifValue = xmldoc.GetElementsByTagName("cif_value");
                if (CifValue[0] != null)
                {
                    CustomData.CifValue = CifValue[0].InnerText;
                }

                XmlNodeList TotalTax = xmldoc.GetElementsByTagName("total_tax");
                if (TotalTax[0] != null)
                {
                    CustomData.TotalTax = TotalTax[0].InnerText;
                }

                XmlNodeList MishgorPackageQuantity = xmldoc.GetElementsByTagName("mishgor-package-quantity");
                if (MishgorPackageQuantity[0] != null)
                {
                    CustomData.MishgorPackageQuantity = MishgorPackageQuantity[0].InnerText;
                }

                XmlNodeList MishgorPackageWeight = xmldoc.GetElementsByTagName("mishgor-package-weight");
                if (MishgorPackageWeight[0] != null)
                {
                    CustomData.MishgorPackageWeight = MishgorPackageWeight[0].InnerText;
                }

                XmlNodeList VersionId = xmldoc.GetElementsByTagName("version_id");
                if (VersionId[0] != null)
                {
                    CustomData.VersionId = VersionId[0].InnerText;
                }
                ////////////////////////////////////

                XmlNodeList TaxDetailsList = xmldoc.GetElementsByTagName("tax");
                for (int i = 0; i < TaxDetailsList.Count; i++)
                {
                    var ChildNodes = TaxDetailsList[i].ChildNodes;
                    var TaxDetails = new TaxesDetails();
                    for (int j = 0; j < ChildNodes.Count; j++)
                    {
                        if (ChildNodes[j].Name == "tax-taxtype")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "tax-taxtype-name")
                                {
                                    TaxDetails.Taxtypename = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                                else if (ChildNodes[j].ChildNodes[x].Name == "tax-taxtype-id")
                                {
                                    TaxDetails.TaxTypeCode = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                        }
                        else if (ChildNodes[j].Name == "tax-tax_basis")
                        {
                            TaxDetails.TaxBasis = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "tax-tax_to_pay")
                        {
                            TaxDetails.TaxToPay = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "tax-tax_amount")
                        {
                            TaxDetails.TaxAmount = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "tax-postponed_tax")
                        {
                            TaxDetails.TaxPostponed = ChildNodes[j].InnerText;
                        }
                    }
                    CustomData.TaxesDetails.Add(TaxDetails);
                }


                ///////////////////////////////////////////////////


                ////////////////////////////////

                XmlNodeList GoodsDetailsList = xmldoc.GetElementsByTagName("acc_supplier");
                for (int i = 0; i < GoodsDetailsList.Count; i++)
                {
                    var ChildNodes = GoodsDetailsList[i].ChildNodes;
                    var GoodValue = new GoodsValueDetails();
                    for (int j = 0; j < ChildNodes.Count; j++)
                    {
                        if (ChildNodes[j].Name == "acc_supplier-sup_account")
                        {
                            GoodValue.SupAccount = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-incoterm")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-incoterm-id")
                                {
                                    GoodValue.IncotermId = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-value")
                        {
                            GoodValue.Value = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-currency")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-currency-name" || ChildNodes[j].ChildNodes[x].Name == "acc_supplier-currency-id")
                                {
                                    GoodValue.CurrencyName = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                            //GoodValue.CurrencyName = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-country")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-country-name")
                                {
                                    GoodValue.CountryName = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                            //GoodValue.CountryName = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-supplier")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-supplier-name")
                                {
                                    GoodValue.SupplierName = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }

                        }
                        else if (ChildNodes[j].Name == "Acc_supplierfreight")
                        {
                            GoodValue.SupplierFreight = ChildNodes[j].InnerText;
                        }
                    }
                    CustomData.GoodsValueDetails.Add(GoodValue);
                }
                //XmlNodeList SupAccount = xmldoc.GetElementsByTagName("acc_supplier-sup_account");
                //if (SupAccount[0] != null)
                //{
                //    GoodValue.SupAccount = SupAccount[0].InnerText;
                //}

                //XmlNodeList IncotermId = xmldoc.GetElementsByTagName("acc_supplier-incoterm-id");
                //if (IncotermId[0] != null)
                //{
                //    GoodValue.IncotermId = IncotermId[0].InnerText;
                //}

                //XmlNodeList Value = xmldoc.GetElementsByTagName("Acc_supplierfreight");
                //if (Value[0] != null)
                //{
                //    GoodValue.Value = Value[0].InnerText;
                //}

                //XmlNodeList CurrencyName = xmldoc.GetElementsByTagName("acc_supplier-currency-name");
                //if (CurrencyName[0] != null)
                //{
                //    GoodValue.CurrencyName = CurrencyName[0].InnerText;
                //}

                //XmlNodeList CountryName = xmldoc.GetElementsByTagName("acc_supplier-country-name");
                //if (CountryName[0] != null)
                //{
                //    GoodValue.CountryName = CountryName[0].InnerText;
                //}

                //XmlNodeList SupplierName = xmldoc.GetElementsByTagName("acc_supplier-supplier-name");
                //if (SupplierName[0] != null)
                //{
                //    GoodValue.SupplierName = SupplierName[0].InnerText;
                //}

                //XmlNodeList SupplierFreight = xmldoc.GetElementsByTagName("Acc_supplierfreight");
                //if (SupplierFreight[0] != null)
                //{
                //    GoodValue.SupplierFreight = SupplierFreight[0].InnerText;
                //}
                //CustomData.GoodsValueDetails.Add(GoodValue);

            }
            if (data != null && !string.IsNullOrEmpty(data.PaymentRequestXML))
            {
                var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(data.PaymentRequestXML);
                CustomData.RequestPaymentData = MyPaymentData;
            }

            return CustomData;
        }

        public HttpResponseMessage GetSingleData(string id)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ShipmentAdditionalCloudDataRepository Repository = new ShipmentAdditionalCloudDataRepository(authToken.Tenant);

                ShipmentAdditionalCloudData data = Repository.GetSingleShipmentAdditionalCloudData(id, authToken.Tenant);



                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [ActionName("PutMain")]
        public HttpResponseMessage PutMain(ShipmentAdditionalCloudData entity)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticationOnTenant(entity.Tenant);
                    SecurityUtility.AuthenticationOnEntityTenant("ShipmentAdditionalCloudData", entity.Tenant, authToken.Tenant);

                    ShipmentAdditionalCloudDataRepository Repository = new ShipmentAdditionalCloudDataRepository(authToken.Tenant);
                    ShipmentRepository SHRepository = new ShipmentRepository(authToken.Tenant);
                    EntityStatusRepository EntityRepository = new EntityStatusRepository(authToken.Tenant);
                    EntityStatus status = null;
                    if (!string.IsNullOrEmpty(entity.DenyReason))
                    {
                        status = EntityRepository.GetSingleEntityStatusByCode("DDDE", authToken.Tenant);
                    }
                    else
                    {
                        status = EntityRepository.GetSingleEntityStatusByCode("DDAP", authToken.Tenant);
                    }

                    ShipmentAdditionalCloudData data = Repository.GetSingleShipmentAdditionalCloudData(entity.Id, authToken.Tenant);
                    Shipment Ship = SHRepository.GetSingleShipment(entity.Id, authToken.Tenant);
                    Ship.StatusId = status.Id;
                    int MySize = 1024;
                    byte[] BytesUncompressed = new byte[MySize];
                    StringBuilder MyEncodedUncompressMessage = new StringBuilder();
                    try
                    {
                        var MyMemoryStream = new MemoryStream(Convert.FromBase64String(data.DeclarationXmlData));
                        var MyZipInputStream = new BZip2InputStream(MyMemoryStream);
                        StringBuilder MyUncompressMessage = new StringBuilder();


                        Encoding wind1252 = Encoding.GetEncoding(1255);
                        Encoding utf8 = Encoding.UTF8;
                        byte[] utf8Bytes = new byte[MySize];
                        //byte[] MyBytesUncompressed = new byte[MyMemoryStream.Length];
                        while (true)
                        {
                            MySize = MyZipInputStream.Read(BytesUncompressed, 0, MySize);
                            //MyBytesUncompressed = (MyBytesUncompressed.Concat(BytesUncompressed)).ToArray();
                            if (MySize > 0)
                            {

                                utf8Bytes = Encoding.Convert(wind1252, utf8, BytesUncompressed, 0, MySize);
                                MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(utf8Bytes));
                                MyUncompressMessage.Append(Encoding.UTF8.GetString(BytesUncompressed, 0, MySize));
                            }

                            else
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(Convert.FromBase64String(data.DeclarationXmlData)));
                    }
                    //var MyMemoryStream = new MemoryStream(Convert.FromBase64String(data.DeclarationXmlData));
                    //var MyZipInputStream = new BZip2InputStream(MyMemoryStream);
                    //StringBuilder MyUncompressMessage = new StringBuilder();
                    //StringBuilder MyEncodedUncompressMessage = new StringBuilder();
                    //int MySize = 1024;
                    //byte[] BytesUncompressed = new byte[MySize];
                    //Encoding wind1252 = Encoding.GetEncoding(1255);
                    //Encoding utf8 = Encoding.UTF8;
                    //byte[] utf8Bytes = new byte[MySize];
                    ////byte[] MyBytesUncompressed = new byte[MyMemoryStream.Length];
                    //while (true)
                    //{
                    //    MySize = MyZipInputStream.Read(BytesUncompressed, 0, MySize);
                    //    //MyBytesUncompressed = (MyBytesUncompressed.Concat(BytesUncompressed)).ToArray();
                    //    if (MySize > 0)
                    //    {

                    //        utf8Bytes = Encoding.Convert(wind1252, utf8, BytesUncompressed, 0, MySize);
                    //        MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(utf8Bytes));
                    //        MyUncompressMessage.Append(Encoding.UTF8.GetString(BytesUncompressed, 0, MySize));
                    //    }

                    //    else
                    //        break;
                    //}

                    //Encoding wind1252 = Encoding.GetEncoding(1255);
                    //Encoding utf8 = Encoding.UTF8;
                    //byte[] wind1252Bytes = MyBytesUncompressed;
                    //byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                    //string utf8String = Encoding.UTF8.GetString(utf8Bytes);

                    var data_out = MyEncodedUncompressMessage.ToString();
                    BytesUncompressed = null;
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(data_out);
                    XmlNodeList VersionId = xmldoc.GetElementsByTagName("version_id");
                    if (VersionId[0] != null)
                    {
                        data.VersionApproved = VersionId[0].InnerText;
                    }
                    if (!string.IsNullOrEmpty(entity.DenyReason))
                    {
                        data.IsImporterApprovalRequried = false;
                    }
                    else
                    {
                        data.IsImporterApprovalRequried = entity.IsImporterApprovalRequried;
                    }
                    if (!string.IsNullOrEmpty(entity.DenyReason))
                    {
                        data.DenyReason = entity.DenyReason;
                    }
                    else
                    {
                        data.ApproveDateTime = TenantServerConfigration.GetCurrentDateTime(data.Tenant);
                        data.ApprovedByUserName = entity.ApprovedByUserName;
                    }
                    Repository.Update(data);
                    Repository.SubmitChanges();
                    SHRepository.Update(Ship);
                    SHRepository.SubmitChanges();

                    RunStoredProcedureClass.UpdateShipmentStatus(Ship.Id, Ship.Tenant);


                    IQueueService queueservice = new DbQueueService();
                    if (!string.IsNullOrEmpty(entity.DenyReason))
                    {
                        queueservice.InitializeQueue("PrivateLabelDenialQueue", 0);
                    }
                    else
                    {
                        queueservice.InitializeQueue("PrivateLabelApprovalQueue", 0);
                    }
                    queueservice.Send(new Dictionary<string, string>() { { "Id", data.Id }, { "Tenant", data.Tenant.ToString() } }, data.Tenant);
                    scope.Complete();

                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [ActionName("PutUserId")]
        public HttpResponseMessage PutUserId(ShipmentAdditionalCloudData entity)//Not Secure , need to ask Yaron about the Requirement -- Rabaia
        {
            try
            {

                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ShipmentAdditionalCloudDataRepository Repository = new ShipmentAdditionalCloudDataRepository(entity.Tenant);


                ShipmentAdditionalCloudData data = Repository.GetSingleShipmentAdditionalCloudData(entity.Id, entity.Tenant);


                data.IsUserIDNumberRequired = entity.IsUserIDNumberRequired;
                data.UserIdNumber = entity.UserIdNumber;
                data.UserIdNumberUpdateDate = TenantServerConfigration.GetCurrentDateTime(data.Tenant);
                data.ApprovedByUserName = entity.ApprovedByUserName;
                data.UserAcceptSaveID = entity.UserAcceptSaveID;
                 
                Repository.Update(data);
                Repository.SubmitChanges();

                AddUserIdNumberTaskForUnif(data);



                return Request.CreateResponse(HttpStatusCode.OK, data);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private void AddUserIdNumberTaskForUnif(ShipmentAdditionalCloudData entity)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entity.Tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentrepository = new DocumentRepository(commonContext);
            ObjectTableRepository objecttableRep = new ObjectTableRepository(entity.Tenant);
            ObjectTable objectTable = null;
            //ShipmentRepository ShipmentRepository = new ShipmentRepository(entity.Tenant);

            //Shipment MyShipment = ShipmentRepository.GetSingleShipment(entity.Id, entity.Tenant);
            var MyShipment = LogitudeXmlSerializer.DeserializeObject<UserIdNumberRequestPM>(entity.UserIdNumberXMLData);

            objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);

            List<QueueTask> tasks = new List<QueueTask>();
            tasks.Add(new QueueTask()
            {
                Action = "UserIdNumber",
                Parameters = new List<Logitude.Server.Tools.Parameter>() {
                    new Logitude.Server.Tools.Parameter { Name = "ForwarderShipmentNumber", Value = MyShipment.ForwarderShipmentNumber},
                    new Logitude.Server.Tools.Parameter { Name = "UserIdNumber", Value = entity.UserIdNumber},
                    new Logitude.Server.Tools.Parameter { Name = "UserAcceptSaveID", Value = entity.UserAcceptSaveID.ToString()}
                }
            });




            var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = ByteData.Length,
                Tenant = Convert.ToInt32(entity.Tenant),
                Id = IdCounter.GetNumber("Document", entity.Tenant),
                HasFile = true,
                Folder = "ExternalTasksQueue",
            };
            documentrepository.Add(document);
            documentrepository.SubmitChanges();
            var commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", entity.Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant),
                InOut = "O",
                ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                Subject = "UserId Number",
                Tenant = entity.Tenant,
                CommunicationLogTypeCode = "Q",
                CommunicationStatusTypeCode = "W",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(entity.Tenant),
                DocumentId = document.Id,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDateUTC = DateTime.UtcNow,
                QueueName = "externaltasksqueue" + entity.Tenant + 1,
                Priority = 1,
                EntityReference = MyShipment.ForwarderShipmentNumber

            };

                communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();
            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = entity.Tenant,
                FileSize = ByteData.Length,

            };

            storageservice.Write(ByteData, fileInfo);
            Communications.SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, commLog.Tenant);

        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length    
                buffer = new byte[length];            // create buffer     
                int count;                            // actual number of bytes read     
                int sum = 0;                          // total number of bytes read    

                // read until Read method returns 0 (end of the stream has been reached)    
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public HttpResponseMessage GetSingleWithoutToken(string securityId, int? tenant = null)
        {
            try
            {
                ShipmentAdditionalCloudDataRepository Repository;
                ShipmentRepository ShipmentRepository;
                Shipment MyShipment;
                ShipmentAdditionalCloudData data;
                if (tenant == null)
                {
                    if (HttpContext.Current.Items.Contains("Tenant"))
                        tenant = (int)HttpContext.Current.Items["Tenant"];
                }

                const string testKey = "d5e6d15f4cb24f12a8ac9c5e8c54a06d";
                if (securityId == testKey)
                {
                    Repository = new ShipmentAdditionalCloudDataRepository((int)tenant);
                    ShipmentRepository = new ShipmentRepository((int)tenant);
                    data = Repository.GetSingleShipmentAdditionalCloudDataTest();
                }
                else
                {
                     Repository = new ShipmentAdditionalCloudDataRepository(tenant.Value);
                     ShipmentRepository = new ShipmentRepository(tenant.Value);
                     MyShipment = ShipmentRepository.getSingleShipmentBySecurityId(securityId, tenant.Value);
                     data = Repository.GetSingleShipmentAdditionalCloudData(MyShipment.Id, tenant.Value);
                }
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static readonly HttpClient client = new HttpClient();
        //public HttpResponseMessage GetPaymentData(string id, int tenant)
        //{
        //    try
        //    {   
        //        ShipmentAdditionalCloudDataRepository Repository = new ShipmentAdditionalCloudDataRepository(tenant);

        //        ShipmentAdditionalCloudData data = Repository.GetSingleShipmentAdditionalCloudData(id, tenant); 
        //        if (data != null && !string.IsNullOrEmpty(data.PaymentRequestXML))
        //        {
        //            var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(data.PaymentRequestXML);
        //            var myId = Path.GetRandomFileName().Replace("&", "").Replace(".", "").Replace("=", "");
        //            //"sum=199.9&supplier=amitaltest&TranzilaPW=4Jwdsb&currency=1&op=1&DCdisable="
        //            string myParams = "sum=" + MyPaymentData.TotalChargesInNIS + "&supplier=amitaltest&TranzilaPW=4Jwdsb&currency=1&op=1&DCdisable=" + myId + "&DclickTK=" + myId;
        //            Dictionary<string, string> dict = GetParamsAsDict(myParams);
        //            string result = "";
        //            var success = GetRequestToken(dict, out result);
        //            if (success)
        //            {
        //                var MyPaymentInfo = ForwardToPaymentLink(result,myParams);
        //                //return Request.CreateResponse(HttpStatusCode.OK, MyPaymentInfo);
        //            }  
        //        }

        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "No Data");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

        private PaymentData ForwardToPaymentLink(string thtk, string MyParams)
        {
            PaymentData MyPaymentData = new PaymentData();

            string postbackUrl = @"https://direct.tranzila.com/amitaltest/";
            //StringBuilder sb = new StringBuilder();
            //if (!textbox_result.Text.Contains("thtk="))
            //{
            //    MsgBox("HandShake parameter 'thtk' is missing !!!", this.Page, this);
            //    return;
            //}
            Dictionary<string, string> dict = GetParamsAsDict(MyParams + "&" + thtk);
            MyPaymentData.currency = dict["currency"];
            MyPaymentData.sum = dict["sum"];
            MyPaymentData.op = dict["op"];
            MyPaymentData.DCdisable = dict["DCdisable"];
            MyPaymentData.DclickTK = dict["DclickTK"];
            MyPaymentData.thtk = dict["thtk"];
            return MyPaymentData;
            //sb.Append("<html>");
            //sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            //sb.AppendFormat("<form name='form' action='{0}' method='post'>", postbackUrl);

            //foreach (var item in dict)
            //{
            //    if (item.Key != "supplier" && item.Key != "TranzilaPW")
            //        sb.AppendFormat("<input type='hidden' name='{0}' value='{1}'>", item.Key, item.Value);
            //}
            //sb.AppendLine(thtk);


            ////Response.Clear();

            //// Other params go here
            //sb.Append("</form>");
            //sb.Append("</body>");
            //sb.Append("</html>");

            //Response.Write(sb.ToString());

            //Response.End();
        }

        private Dictionary<string, string> GetParamsAsDict(string text)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var list = text.Split('&');
            foreach (var item in list)
            {
                dict.Add(item.Split('=')[0], item.Split('=')[1]);
            }
            return (dict);
        }

        private bool GetRequestToken(Dictionary<string, string> myDict, out string result)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var content = new FormUrlEncodedContent(myDict);
            var response = client.PostAsync("https://secure5.tranzila.com/cgi-bin/tranzila71dt.cgi", content);
            var httpResponse = response.Result.Content.ReadAsStringAsync();// .Content.ReadAsStringAsync();
            result = httpResponse.Result;
            return (result.Contains("thtk") ? true : false);
        }


    }

    //class PaymentData
    //{
    //    public string sum { get; set; }
    //    public string currency { get; set; }
    //    public string op { get; set; }
    //    public string DCdisable { get; set; }
    //    public string DclickTK { get; set; }
    //    public string thtk { get; set; }
    //}
}