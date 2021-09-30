using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class UploadImageController : ApiController
    {

        public SuccessMobile PostImageByte(int tenant, ImageParameter filters)
        {
            try
            {
                string shipmentId = filters.ShipmentId;
                string documentTypeId = filters.DocumentTypeId;

                if (string.IsNullOrEmpty(filters.DocumentType)) filters.DocumentType = "POD";

                SuccessMobile result = AuthorizationVersion();

                if (!result.IsScceed)
                {
                    result.ExceptionMessage = "Your application version is out-of-date. Please upgrade your application to the latest version";
                    result.ExceptionTitle = "Unifreight POD Update";
                    return result;
                }

                #region Authentication ShipmentNumber
                if (string.IsNullOrEmpty(shipmentId))
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    shipmentId = shipmentRepository.GetShipmentIdByShipmentNumber(filters.ShipmentNumber, tenant);
                    result.ShipmentId = shipmentId;
                    if (string.IsNullOrEmpty(shipmentId))
                    {
                        result.IsScceed = false;
                        result.ExceptionMessage = "התיק לא אותר";
                        return result;
                    }
                }

                #endregion

                #region Authentication SecurityKey

                if (!string.IsNullOrEmpty(filters.SecurityKey))
                {
                    ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                    var isScceed = shipmentQuery.CheckPODSecurityKeyValidation(filters.ShipmentNumber, filters.SecurityKey, tenant);
                    if (!isScceed)
                    {
                        result.IsScceed = false;
                        result.ExceptionMessage = "זיהוי משלוח לא תקין- אנא פנה לסוכן מכס";
                        return result;
                    }
                }

                #endregion

                #region DocumentType
                if (string.IsNullOrEmpty(documentTypeId))
                {
                    DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                    documentTypeId = result.DocumentTypeId = documentTypeRepository.GetDocumentTypeIdByCode(filters.DocumentType, tenant);
                    if(string.IsNullOrEmpty(documentTypeId) && filters.DocumentType!="POD" && filters.IsReadDocumentFromBarCode)
                    {
                        filters.DocumentType = "POD";
                        documentTypeId = result.DocumentTypeId = documentTypeRepository.GetDocumentTypeIdByCode(filters.DocumentType, tenant);
                    }

                    if (string.IsNullOrEmpty(documentTypeId))
                    {
                        result.IsScceed = false;
                        result.ExceptionMessage = filters.DocumentType + "סוג מסמך POD לא נמצא";
                        return result;
                    }
                }

                #endregion

                #region Upload Document 
                DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper();
                Response response = documentFileUploadHelper.UploadDocumentFileData(filters.buffer, filters.FileSize, filters.SentSize, filters.BlockIdsList.ToArray(), filters.BufferNumber, tenant, filters.FileName, filters.DocumentId);

                if (!response.HasError)
                {
                    if (string.IsNullOrEmpty(filters.DocumentId)) filters.DocumentId = result.DocumentId = response.Result;

                    if (filters.FileSize == filters.SentSize)
                    {
                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(filters.Tenant);
                        ContactRepository contactRepository = new ContactRepository(filters.Tenant);

                        string objectTableId = objectTabelRepository.GetObjectTableIdByName("Shipment");
                        string userId = contactRepository.GetConactIdByemail("system@tenant" + filters.Tenant.ToString() + ".com", filters.Tenant);

                        ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                        DocumentsFilingService documentsService = new DocumentsFilingService(objectContext, tenant);

                        DocumentsFilingPM extDocPM = new DocumentsFilingPM()
                        {
                            Id = IdCounter.GetNumber("DocumentsFiling", tenant).ToString(),
                            DirectionCode = "I",
                            Tenant = tenant,
                            DocumentId = filters.DocumentId,
                            EntityId = shipmentId,
                            DocumentTypeId = documentTypeId,
                            ObjectTableId = objectTableId,
                            CreatedByUserId = userId,
                            CreateDate = DateTime.Now,
                            OwnerId = userId,
                            UpdatedByUserId = userId,
                            ExternalEntityName = "Shipment",
                            EntityReference = filters.ShipmentNumber,
                            FileExtension = "jpg",
                            FileSize = filters.FileSize,
                            Folder = "docsin",
                            HasFile = true,
                            FileName = filters.FileName.Split('.')[0],
                            Received = true,
                            ReceivedDate = DateTime.Now,
                            ReceivedByUserId = userId,
                            Notes = filters.DeviceName + " _ " + filters.PhoneNumber,
                            Description = filters.DocumentType + " for file " + filters.ShipmentNumber,
                            IsFromUnifreightPodMobile = true,
                        };

                        documentsService.Create(extDocPM, null, null, true);
                        result.DocumentsFilingId = extDocPM.Id;

                        if (FeatureToggleHelper.HasFeatureToggle("POD", extDocPM.Tenant))
                        {
                            AddConvertImagetoPDFQueue(extDocPM);
                        }

                    }
                    result.IsScceed = true;
                }
                else
                {
                    result.IsScceed = false;
                    result.ExceptionMessage = response.ErrorMessage;
                }

                #endregion

                return result;
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0,"", "", "UploadImageController : PostImageByte", null);
                SuccessMobile data = new SuccessMobile();
                data.IsScceed = false;
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += Environment.NewLine + e.InnerException.Message;
                }
                data.ExceptionMessage = message;
                return data;
            }
        }

        private  void AddConvertImagetoPDFQueue(DocumentsFilingPM extDocPM)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("PODImageConverterQueue", extDocPM.Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "DocumentId", extDocPM.DocumentId }, { "NewType", "PDF" } ,  { "Tenant", extDocPM.Tenant.ToString() } }, extDocPM.Tenant, null, null, null, null);
        }

        private SuccessMobile AuthorizationVersion()
        {
            SuccessMobile result = new SuccessMobile();
            result.IsScceed = true;
            string mobileVersion = HttpContext.Current.Request.Headers["MobileVersion"];
            string Platform = HttpContext.Current.Request.Headers["Platform"];
            if (!string.IsNullOrEmpty(mobileVersion) && !string.IsNullOrEmpty(Platform))
            {
                double version = 0;
                if (double.TryParse(mobileVersion, out version))
                {
                    if (Platform == "IOS")
                    {
                        if (version < LogitudeSettings.IOSPodAppMinimumVersion) result.IsScceed = false;

                    }
                    else
                    {
                        if (version < LogitudeSettings.AndroidPodAppMinimumVersion) result.IsScceed = false;

                    }
                }
            }

            return result;

        }


    }

}