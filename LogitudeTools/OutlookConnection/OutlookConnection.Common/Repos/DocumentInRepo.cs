using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.DocumentInWcfServiceReference;
using System.ServiceModel;
using OutlookConnection.Common.Utils;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Repos
{
    public class DocumentInRepo : IDocumentInRepo, IDisposable
    {
        string _DocumentInWcfService;
        string _token;


        public DocumentInRepo()
        {
            _DocumentInWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "DocumentInWcfService.svc?wsdl");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }



        public DocumentInWcfServiceReference.Response UpsertDocumentData(OutlookConnection.Common.DocumentInWcfServiceReference.DocumentDataPM documentDataPM, bool batch)
        {
            try
            {
                using (var repo = GetDocumentInWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        // DocumentInWcfServiceReference.Response myResponnse = new DocumentInWcfServiceReference.Response();
                        var myResponnse = repo.UpsertDocumentData(documentDataPM, batch);
                        if (myResponnse == null)
                        {
                            LogFileUtil.Log("DocumentInRepo Upsert() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return null;
                        }
                        else if (myResponnse.HasError)
                        {
                            LogFileUtil.Log("DocumentInRepo Upsert() Failed - return Error: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return myResponnse;
                        }
                        else
                        {
                            LogFileUtil.Log("DocumentInRepo Upsert() Finished successfully", LogFileUtil.LogLevel.Debug);
                            return myResponnse;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("DocumentInRepo Upsert() DocumentIn Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public DocumentInWcfServiceReference.Response Upsert(OutlookConnection.Common.DocumentInWcfServiceReference.DocumentsFilingPM documentsFilingPM, bool batch)
        {
            try
            {
                using (var repo = GetDocumentInWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        // DocumentInWcfServiceReference.Response myResponnse = new DocumentInWcfServiceReference.Response();
                        var myResponnse = repo.Upsert(documentsFilingPM, batch);
                        if (myResponnse == null)
                        {
                            LogFileUtil.Log("DocumentInRepo Upsert() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return null;
                        }
                        else if (myResponnse.HasError)
                        {
                            LogFileUtil.Log("DocumentInRepo Upsert() Failed - return Error: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return myResponnse;
                        }
                        else
                        {
                            LogFileUtil.Log("DocumentInRepo Upsert() Finished successfully", LogFileUtil.LogLevel.Debug);
                            return myResponnse;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("DocumentInRepo Upsert() DocumentIn Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }


        public async Task<DocumentInWcfServiceReference.Response> UpsertDocumentDataAsync(OutlookConnection.Common.DocumentInWcfServiceReference.DocumentDataPM documentDataPM, bool batch)
        {
            try
            {
                using (var repo = GetDocumentInWcfServiceClient())
                {
                        new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel);
                    
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        // DocumentInWcfServiceReference.Response myResponnse = new DocumentInWcfServiceReference.Response();
                        var myResponnse = await repo.UpsertDocumentDataAsync(documentDataPM, batch);

                        return myResponnse;
                        //if (myResponnse == null)
                        //{
                        //    LogFileUtil.Log("DocumentInRepo Upsert() Failed - return null", LogFileUtil.LogLevel.Debug);
                        //    return null;
                        //}
                        //else if (myResponnse.HasError)
                        //{
                        //    LogFileUtil.Log("DocumentInRepo Upsert() Failed - return Error: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                        //    return myResponnse;
                        //}
                        //else
                        //{
                        //    LogFileUtil.Log("DocumentInRepo Upsert() Finished successfully", LogFileUtil.LogLevel.Debug);
                        //    return myResponnse;
                        //}
                    
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("DocumentInRepo Upsert() DocumentIn Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;

            }
        }

        public async Task<Response> UploadDocumentDataAysnc(byte[] currentData, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId)
        {
            Response documentInResponse = new Response();
            try
            {
                using (var repo = GetDocumentInWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        documentInResponse = await repo.UploadDocumentFileDataAsync(currentData, fileSize, sentBytes, blockIdsList, bufferNumber, tenant, FileNameWithExtention, DocumentId);
                    }
                }
            }
            catch (Exception e)
            {
                documentInResponse.Result = "Error";
                documentInResponse.HasError = true;
                documentInResponse.ErrorMessage = e.Message;
                // EventMessangerUtil.SetMessage("Item Synchronization has failed ", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo uploadDocumentDataAysnc() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
            }
            return documentInResponse;
        }



        public DocumentInWcfServiceClient GetDocumentInWcfServiceClient()
        {
            var remoteAddress = new System.ServiceModel.EndpointAddress(_DocumentInWcfService);
            var my = new DocumentInWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
            (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
            (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
            (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
            (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
            return my;
        }

        public void Dispose()
        {
            //scope.Dispose();
            _DocumentInWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


    }
}
