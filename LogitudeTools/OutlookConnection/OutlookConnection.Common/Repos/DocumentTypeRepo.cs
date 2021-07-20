using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.DocumentTypeWcfServiceReference;
using System.ServiceModel;
using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.Utils;

namespace OutlookConnection.Common.Repos
{
    public class DocumentTypeRepo : IDocumentTypeRepo, IDisposable
    {

        string _DocumentTypeWcfService;
        string _token;


        public DocumentTypeRepo()
        {
            _DocumentTypeWcfService = SettingServiceLocator.Instance.CRMSettings.ServerURL.Replace("ActivityWcfService.svc", "DocumentTypeWcfService.svc");
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }


        public DocumentTypeList[] GetDocumentTypes(string objectTableName, int tenant, int skip, int take, ref Response response)
        {
            try
            {
                using (var repo = DocumentTypeWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        DocumentTypeList[] myDocumentList = new DocumentTypeList[] { };
                        myDocumentList = repo.GetDocumentTypes(objectTableName, tenant, skip, take, ref response).OrderBy(a => a.OrderBy).ToArray();
                        if (myDocumentList.Length > 0)
                        {
                            return myDocumentList;
                        }
                        else
                        {
                            if (response == null)
                            {
                                LogFileUtil.Log("DocumentTypeRepo GetDocumentTypes() Failed - return null", LogFileUtil.LogLevel.Debug);
                                return null;
                            }
                            else if (response.HasError)
                            {
                                LogFileUtil.Log("DocumentTypeRepo GetDocumentTypes() - return Error: " + response.ErrorMessage, LogFileUtil.LogLevel.Debug);
                                return null;
                            }
                            else
                            {
                                LogFileUtil.Log("DocumentTypeRepo GetDocumentTypes() Failed - NO ErrorMessage", LogFileUtil.LogLevel.Debug);
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {        
                LogFileUtil.Log("DocumentTypeRepo GetDocumentTypes() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }


        }


        public DocumentTypeWcfServiceReference.Response Upsert(DocumentTypePM entityPM, bool batch)
        {
            try
            {
                using (var repo = DocumentTypeWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        DocumentTypeWcfServiceReference.Response myResponnse = new DocumentTypeWcfServiceReference.Response();
                        myResponnse = repo.Upsert(entityPM, batch);
                        if (myResponnse == null)
                        {
                            LogFileUtil.Log("DocumentTypeRepoUpsert Upsert() Failed - return null", LogFileUtil.LogLevel.Debug);
                            return null;
                        }
                        else if (myResponnse.HasError)
                        {
                            LogFileUtil.Log("DocumentTypeRepoUpsert UpsUpsert()ert Failed - return Error: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return null;
                        }
                        else
                        {
                            LogFileUtil.Log("DocumentTypeRepoUpsertUpsert() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return myResponnse;
                        }
                    }
                }
            }
            catch (Exception e)
            {

                LogFileUtil.Log("DocumentTypeRepo Upsert()  Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }





        public DocumentTypeWcfServiceClient DocumentTypeWcfServiceClient()
        {
            var remoteAddress = new System.ServiceModel.EndpointAddress(_DocumentTypeWcfService);
            var my = new DocumentTypeWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
            (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
            (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
            (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
            (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;        
            return my;
        }


        public void Dispose()
        {
            _DocumentTypeWcfService = null;
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }


    }

}
