using Amital.UpDown.Common;
using Amital.UpDown.Common.Client;
using Amital.UpDown.Common.ModelShared.Filling;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Helpers;
using System;
using System.Configuration;

namespace AmitalCloud.Infrastructure.Data.Services
{
    public class UnifreightFillingService
    {
        public UnifreightFillingService()
        {
            //ONPREMISEFILLINGSERVICE
            //CustomsSettingQueryService.GetSettingByTenant(tenant)
            //DMStroe
            if (!AmitalCloudSettings.IsCostomsDeploy)
            {
                throw new Exception("!AmitalCloudSettings.IsCostomsDeploy()");
            }
        }
        public void UploadByTenantComId(BlobFileInfo fileInfo,
            string externalDocumentId, DateTime? createDate,
            byte[] buffer)
        {
            ResponsePutStreamByTenantComIdM_V1 res = null;
            if (fileInfo.FolderName != "docsin")
            {
                throw new Exception(@"UnifreightFillingService works 4 fileInfo.FolderName == docsin");
            }
            if (String.IsNullOrWhiteSpace(externalDocumentId))
            {
                throw new Exception(@"UnifreightFillingService fileInfo.ExternalDocumentId is must");
            }
            if (String.IsNullOrWhiteSpace(fileInfo.Extension))
            {
                throw new Exception(@"UnifreightFillingService Extension is must");
            }
            if (!createDate.HasValue)
            {
                throw new Exception(@"UnifreightFillingService !fileInfo.CreateDate.HasValue");
            }
            int chunkSizeInKB = GetUploadChunkSizeInKB();

            var comId = Guid.NewGuid();
            var client = new UploadClient<RequestPutStreamByTenantComIdM_V1, ResponsePutStreamByTenantComIdM_V1>();
            var OnPremiseFillingService = AmitalCloudSettings.GetLogitudeCustomsSettingsMInject(fileInfo.Tenant).OnPremiseFillingService;
            var UFileVer = fileInfo.UFileVer ?? 1;
            res = client.Upload(OnPremiseFillingService, 60, new RequestPutStreamByTenantComIdM_V1()
            {
                ComId = externalDocumentId,
                CreateDate = createDate.Value,
                CustomsTenant = fileInfo.Tenant,
                myFillingFolderType = FillingFolderType.BLOB,
                Version = UFileVer,
                ExtensionWithoutPoint = fileInfo.Extension,
                strMoreParams = ""

            },
            buffer, chunkSizeInKB);



            if (!String.IsNullOrWhiteSpace(res.strErrorMessage))
            {
                throw new Exception("UnifreightFillingService.UploadByTenantComId():" + res.strErrorMessage);
            }
            else
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug(res.ServerFilePath);
            }
        }

       
        private static int GetUploadChunkSizeInKB()
        {
            int chunkSizeInKB = 100;
            try
            {
                var sUploadChunkSizeInKB = ConfigurationManager.AppSettings["20180711.UploadChunkSizeInKB"] as string;
                if (!string.IsNullOrWhiteSpace(sUploadChunkSizeInKB))
                {
                    int.TryParse(sUploadChunkSizeInKB, out chunkSizeInKB);
                }

            }
            catch
            {


            }
            if (chunkSizeInKB < 100)
            {
                chunkSizeInKB = 100;
            }
            return chunkSizeInKB;
        }

        public byte[]  Download(BlobFileInfo fileInfo,
            string externalDocumentId, DateTime? createDate
            )
        {
            DownloadServiceResponseData res = null;
            byte[] AllDataCalcOnClient = null;
            try
            {


                var reqBase64StringByFolderTenantComIdM_V1 = new ReqBase64StringByFolderTenantComIdM_V1()
                {
                    ComId = externalDocumentId,
                    CustomsTenant = fileInfo.Tenant,
                    myFillingFolderType = FillingFolderType.BLOB,

                };
                reqBase64StringByFolderTenantComIdM_V1.PutStreamByTenantComIdParams =
                new RequestPutStreamByTenantComIdM_V1()
                {
                    ComId = externalDocumentId,
                    CreateDate = createDate.Value,
                    CustomsTenant = fileInfo.Tenant,
                    myFillingFolderType = FillingFolderType.BLOB,
                    Version = fileInfo.UFileVer ?? 1,
                    ExtensionWithoutPoint = fileInfo.Extension,
                    strMoreParams = ""

                };
                var DownloadClient = new DownloadClient<ReqBase64StringByFolderTenantComIdM_V1, ResBase64StringByFolderTenantComIdM_V1>();


                var OnPremiseFillingService = AmitalCloudSettings.GetLogitudeCustomsSettingsMInject(fileInfo.Tenant).OnPremiseFillingService;
                //OnPremiseFillingService = @"http://itzik-7-new:5057/Unifreight/FilingManagerSplit/basic";

                res = DownloadClient.Download(OnPremiseFillingService, reqBase64StringByFolderTenantComIdM_V1);
                if (res == null)
                {
                    throw new Exception("OnPremiseFillingService.Download Return null");
                }
                var p = res.ParamOut as ResBase64StringByFolderTenantComIdM_V1;
                var data = res.AllDataCalcOnClient ?? new byte[0];
                if (!String.IsNullOrWhiteSpace(p.strMsg)
                    //&& data.Length<3
                    )
                {
                    throw new Exception("OnPremiseFillingService.Message=" + p.strMsg);
                }
                AllDataCalcOnClient = res.AllDataCalcOnClient;
            }
            catch (Exception e)
            {

                throw;
            }

            return AllDataCalcOnClient;
        }

        public byte[] DownloadTiff(int tenant, string externalDocumentId, int CurrPage,
            out string TiffPageLines, out string ErrorMessage)
        {
            ErrorMessage = TiffPageLines = null;
            DownloadServiceResponseData res = null;
            byte[] AllDataCalcOnClient = null;
            try
            {
                var reqGetPageTiffAsB64FromTarByTenantComIdPageM_V1 = new ReqGetPageTiffAsB64FromTarByTenantComIdPageM_V1()
                {
                    ComId = externalDocumentId,
                    CustomsTenant = tenant,
                    currPage = CurrPage
                };
                var DownloadClient = new DownloadClient<ReqGetPageTiffAsB64FromTarByTenantComIdPageM_V1, ResGetPageTiffAsB64FromTarByTenantComIdPageM_V1>();
                var OnPremiseFillingService = AmitalCloudSettings.GetLogitudeCustomsSettingsMInject(tenant).OnPremiseFillingService;
                res = DownloadClient.Download(OnPremiseFillingService, reqGetPageTiffAsB64FromTarByTenantComIdPageM_V1);
                if (res == null)
                {
                    ErrorMessage = "OnPremiseFillingService.Download Return null";
                    return null;
                }
                var p = res.ParamOut as ResGetPageTiffAsB64FromTarByTenantComIdPageM_V1;
                if (p.NoTiffVersion) return null;
                if (!String.IsNullOrWhiteSpace(p.strMsg)
                    //&& data.Length<3
                    )
                {
                    ErrorMessage = p.strMsg;
                    return null;
                    throw new Exception("OnPremiseFillingService.Message=" + p.strMsg);
                } 
                var data = res.AllDataCalcOnClient ?? new byte[0];
                TiffPageLines = p.TiffPageLines;
                AllDataCalcOnClient = res.AllDataCalcOnClient;
            }
            catch (Exception e)
            {
                throw;
            }
            return AllDataCalcOnClient;
        }
    }
}