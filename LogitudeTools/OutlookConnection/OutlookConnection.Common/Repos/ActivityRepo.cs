using OutlookConnection.Common.ActivityWcfServiceReference;
using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Repos
{
    public class ActivityRepo : IDisposable
    {
       
        string _token;

        public ActivityRepo()
        {
            
            _token = SettingServiceLocator.Instance.CRMSettings.Token;
        }


        public Response Upsert(ActivityPM entityPM)
        {
            Response activityResponse = new Response();
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {


                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        var tenent = SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant;
                        entityPM.BusinessUnitId = tenent.ToString();
                        entityPM.Tenant = tenent;

                        //  activityResponse = repo.Upsert(entityPM, SettingServiceLocator.Instance.CRMSettings.User); //******************************************************************************

                        activityResponse = repo.Upsert(entityPM, SettingServiceLocator.Instance.CRMSettings.User);
                        //  activityResponse = repo.UploadDocumentFileData(entityPM, SettingServiceLocator.Instance.CRMSettings.User); 

                        if (activityResponse == null)
                        {
                            activityResponse.Result = "Error";
                            LogFileUtil.Log("Item Synchronization Failed, Return null on Upsert.  Item: " + entityPM.Subject, LogFileUtil.LogLevel.Debug);
                            throw new Exception("Upsert return null");
                        }

                        if (activityResponse.HasError)
                        {
                            RaiseInvalidToken(activityResponse);
                            // EventMessangerUtil.SetMessage("Item Synchronization failed ", PriorityEnum.ERROR);
                            LogFileUtil.Log("Upsert Item Failed, Return HasError on Upsert, Item:  " + entityPM.Subject + " Error: " + activityResponse.ErrorMessage.ToString(), LogFileUtil.LogLevel.Debug);
                            activityResponse.Result = "Error";
                        }
                        else
                        {
                            LogFileUtil.Log("Item Synchronized: " + entityPM.Subject + " and returned activityId: " + activityResponse.Result, LogFileUtil.LogLevel.Debug);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                activityResponse.Result = "Error";
                activityResponse.HasError = true;
                activityResponse.ErrorMessage = e.Message;
                // EventMessangerUtil.SetMessage("Item Synchronization has failed ", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo Upsert() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
            }


            return activityResponse;
        }



        public async Task<Response> UpsertAsync(ActivityPM entityPM)
        {
            Response activityResponse = new Response();
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {

                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        var tenent = SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant;
                        entityPM.BusinessUnitId = tenent.ToString();
                        entityPM.Tenant = tenent;

                        activityResponse = await repo.UpsertAsync(entityPM, SettingServiceLocator.Instance.CRMSettings.User);

                        if (activityResponse == null)
                        {
                            activityResponse.Result = "Error";
                            LogFileUtil.Log("Item Synchronization Failed, Return null on Upsert.  Item: " + entityPM.Subject, LogFileUtil.LogLevel.Debug);
                            throw new Exception("Upsert return null");
                        }

                        if (activityResponse.HasError)
                        {
                            RaiseInvalidToken(activityResponse);
                            // EventMessangerUtil.SetMessage("Item Synchronization failed ", PriorityEnum.ERROR);
                            LogFileUtil.Log("Upsert Item Failed, Return HasError on Upsert, Item:  " + entityPM.Subject + " Error: " + activityResponse.ErrorMessage.ToString(), LogFileUtil.LogLevel.Debug);
                            activityResponse.Result = "Error";
                        }
                        else
                        {
                            LogFileUtil.Log("Item Synchronized: " + entityPM.Subject + " and returned activityId: " + activityResponse.Result, LogFileUtil.LogLevel.Debug);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                activityResponse.Result = "Error";
                activityResponse.HasError = true;
                activityResponse.ErrorMessage = e.Message;
                // EventMessangerUtil.SetMessage("Item Synchronization has failed ", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo Upsert() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
            }


            return activityResponse;
        }




        private static void RaiseInvalidToken(Response activityResponse)
        {
            if (activityResponse.ErrorMessage.Contains("WebFreight.Web.Security.AutenticationException") || activityResponse.IsAuthenticationError)
            {
                EventMessangerUtil.SetMessage("Token is not Vailed - Please Log-In", PriorityEnum.ERROR);
                EventMessangerUtil.SetMessage("Erasing Old Token ", PriorityEnum.DEBUG);
                LogFileUtil.Log("Token is not Vailed, Erasing Old Token", LogFileUtil.LogLevel.Debug);
                SettingServiceLocator.Instance.CRMSettings.Token = "";
            }


        }

        public static ActivityWcfServiceClient GetActivityWcfServiceClient()
        {
            try
            {

                var remoteAddress = new System.ServiceModel.EndpointAddress(SettingServiceLocator.Instance.CRMSettings.ServerURL);
                var my = new ActivityWcfServiceClient(new System.ServiceModel.BasicHttpBinding(), remoteAddress);
                (my.Endpoint.Binding as BasicHttpBinding).MaxReceivedMessageSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).MaxBufferSize = 2147483647;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxStringContentLength = 67108864;
                (my.Endpoint.Binding as BasicHttpBinding).ReaderQuotas.MaxArrayLength = 67108864;
                return my;



                //var my = new OutlookConnection.Common.ActivityWcfServiceReference.ActivityWcfServiceClient();
                //my.Endpoint.Address = new EndpointAddress(AmitaSettingStorage.Instance.CRMSettings.ServerURL);
                //return my;
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ActivityRepo GetActivityWcfServiceClient() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }


        }

        public Response UnTrack(string activityId)
        {

            Response ResponseUnTrack = new Response();

            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        ResponseUnTrack = repo.Delete(activityId, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant);

                        if (ResponseUnTrack == null)
                        {
                            EventMessangerUtil.SetMessage("Disconnect Item Failed", PriorityEnum.ERROR);
                            LogFileUtil.Log("ActiviyRepo UnTrack() Response returned with null", LogFileUtil.LogLevel.Debug);

                        }
                        else if (ResponseUnTrack.HasError)
                        {
                            EventMessangerUtil.SetMessage("Disconnect Item Failed", PriorityEnum.ERROR);
                            RaiseInvalidToken(ResponseUnTrack);
                            LogFileUtil.Log("ActiviyRepo UnTrack() Response returned with Error Message: " + ResponseUnTrack.ErrorMessage, LogFileUtil.LogLevel.Debug);
                        }
                        else
                        {
                            LogFileUtil.Log("ActiviyRepo UnTrack() Delete Item Finished successfully", LogFileUtil.LogLevel.Debug);
                        }
                    }
                }
                return ResponseUnTrack;

            }
            catch (Exception e)
            {
                EventMessangerUtil.SetMessage("Item Canceling failed, Please check with your system administrator", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo UnTrack() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);

                ResponseUnTrack.HasError = true;
                ResponseUnTrack.ErrorMessage = e.Message;

                return ResponseUnTrack;
            }
        }

        public ActivityPM[] GetActivities()
        {
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        Response myResponnse = new Response();
                        var activitiePMList = repo.GetActivities(SettingServiceLocator.Instance.CRMSettings.User, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref myResponnse);

                        if (myResponnse == null)
                        {
                            LogFileUtil.Log("ActiviyRepo GetActivities() Response returned with null", LogFileUtil.LogLevel.Debug);
                            return null;
                        }
                        else if (myResponnse.HasError)
                        {
                            RaiseInvalidToken(myResponnse);
                            LogFileUtil.Log("ActiviyRepo GetUserTenants() Response returned with Error Message: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return null;
                        }
                        else
                        {
                            LogFileUtil.Log("ActiviyRepo GetActivities() Finished successfully: ", LogFileUtil.LogLevel.Debug);
                            return activitiePMList;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventMessangerUtil.SetMessage("Checking For Updated Items In CRM failed ", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo GetActivities() Failed: " + e.Message.ToString(), LogFileUtil.LogLevel.Debug);
                return null;
            }
        }

        public static bool IsOnline()
        {
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        Response response = new Response();
                        repo.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(900);
                        response = repo.isOnline();

                        if (response.HasError)
                        {
                            RaiseInvalidToken(response);
                            EventMessangerUtil.SetMessage("Connection with the CRM server Failed", PriorityEnum.ERROR);
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                EventMessangerUtil.SetMessage("Connection with the CRM server Failed", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo IsOnline() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
                return false;
            }

        }

        public bool UpdateCRMAmital(string activityID, string outlookID)
        {
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {
                        Response myResponnse = new Response();
                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);
                        myResponnse = repo.UpdateOutlookID(activityID, outlookID, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant);

                        if (myResponnse == null)
                        {
                            LogFileUtil.Log("ActiviyRepo UpdateCRMAmital() Response Ended with null", LogFileUtil.LogLevel.Debug);
                            return false;
                        }
                        else if (myResponnse.HasError)
                        {
                            RaiseInvalidToken(myResponnse);
                            LogFileUtil.Log("ActiviyRepo UpdateCRMAmital() Response Ended with Error Message: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return false;
                        }
                        else
                        {
                            LogFileUtil.Log("ActiviyRepo UpdateCRMAmital() Finished successfully " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogFileUtil.Log("ActiviyRepo UpdateCRMAmital() Failed: " + e.Message, LogFileUtil.LogLevel.Debug);
                return false;
            }
        }

        public void SetAsSyncCRMAmital(string activityID)
        {
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {

                        Response myResponnse = new Response();

                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        myResponnse = repo.SetAsSynchronized(activityID, SettingServiceLocator.Instance.CRMSettings.User, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant);

                        if (myResponnse == null)
                        {
                            LogFileUtil.Log("SetAsSynchronized return with null: ", LogFileUtil.LogLevel.Debug);
                            throw new Exception("SetAsSynchronized return null ");
                        }
                        else if (myResponnse.HasError)
                        {
                            LogFileUtil.Log("SetAsSynchronized Failed With ErrorMessage: " + myResponnse.ErrorMessage, LogFileUtil.LogLevel.Debug);
                            RaiseInvalidToken(myResponnse);
                            throw new Exception("SetAsSynchronized Failed With ErrorMessage: " + myResponnse.ErrorMessage);
                        }
                        else
                        {
                            EventMessangerUtil.SetMessage("SetAsSynchronized Finished successfully: ", PriorityEnum.DEBUG);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                EventMessangerUtil.SetMessage("SetAsSynchronized Failed: " + e.ToString(), PriorityEnum.DEBUG);

            }
        }

        public ActivityPM GetActivityByID(string id)
        {





            using (var repo = ActivityRepo.GetActivityWcfServiceClient())
            {
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                    try
                    {


                        Response myResponnse = new Response();
                        //var activitiePMList = repo.GetActivities(AmitaSettingStorage.Instance.CRMSettings.AccountID, AmitaSettingStorage.Instance.CRMSettings.MyTenantInfo.Tenant, ref myResponnse);
                        ActivityPM activitiePM = repo.GetActivityPM(id, SettingServiceLocator.Instance.CRMSettings.MyTenantInfo.Tenant, ref  myResponnse);



                        if (myResponnse == null)
                        {
                            EventMessangerUtil.SetMessage("GetActivityPM Failed", PriorityEnum.DEBUG);
                            throw new Exception("GetActivityPM return null");
                        }
                        else if (myResponnse.HasError)
                        {
                            EventMessangerUtil.SetMessage("GetActivityPM Failed: " + myResponnse.ErrorMessage, PriorityEnum.DEBUG);

                            RaiseInvalidToken(myResponnse);



                            return null;
                        }
                        else
                        {
                            //_availableTenants = new List<TenantInfo>();
                            //foreach (TenantInfo item in myUserTenantList)
                            //{
                            //    _availableTenants.Add(item);
                            //}

                            EventMessangerUtil.SetMessage("GetActivityPM Finished successfully: ", PriorityEnum.DEBUG);
                            return activitiePM;



                        }






                    }
                    catch (Exception e)
                    {
                        //EventMessangerUtil.SetMessage("Checking For Updated Items In CRM Has failed, Please check with your system administrator", PriorityEnum.ERROR);
                        EventMessangerUtil.SetMessage("Getting GetActivityPM Failed: " + e.ToString(), PriorityEnum.DEBUG);

                        return null;
                    }
                }
            }





        }


        public async Task<Response> UploadDocumentDataAysnc(byte[] currentData, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId)
        {
            Response activityResponse = new Response();
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {


                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        // activityResponse = repo.Upsert(entityPM, SettingServiceLocator.Instance.CRMSettings.User);

                         activityResponse = await repo.UploadDocumentFileDataAsync(currentData, fileSize, sentBytes, blockIdsList, bufferNumber, tenant, FileNameWithExtention, DocumentId);
                    }
                }
            }
            catch (Exception e)
            {
                activityResponse.Result = "Error";
                activityResponse.HasError = true;
                activityResponse.ErrorMessage = e.Message;
                // EventMessangerUtil.SetMessage("Item Synchronization has failed ", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo uploadDocumentDataAysnc() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
            }
            return activityResponse;


        }


        public  Response uploadDocumentData(byte[] currentData, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId)
        {
            Response activityResponse = new Response();
            try
            {
                using (var repo = ActivityRepo.GetActivityWcfServiceClient())
                {
                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)repo.InnerChannel))
                    {


                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", SettingServiceLocator.Instance.CRMSettings.Token);

                        // activityResponse = repo.Upsert(entityPM, SettingServiceLocator.Instance.CRMSettings.User);

                        activityResponse = repo.UploadDocumentFileData(currentData, fileSize, sentBytes, blockIdsList, bufferNumber, tenant, FileNameWithExtention, DocumentId);
                    }
                }
            }
            catch (Exception e)
            {
                activityResponse.Result = "Error";
                activityResponse.HasError = true;
                activityResponse.ErrorMessage = e.Message;
                // EventMessangerUtil.SetMessage("Item Synchronization has failed ", PriorityEnum.ERROR);
                LogFileUtil.Log("ActivityRepo uploadDocumentDataAysnc() Failed: " + e.ToString(), LogFileUtil.LogLevel.Debug);
            }
            return activityResponse;


        }

        public void Dispose()
        {
          
            _token = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

    }
}

