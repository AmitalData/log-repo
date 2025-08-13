using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.ExternalServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Unifreight.BL.EntityQueryServices;

namespace Logitude.Customs.BL.Messaging.Customs
{
    public class SignQueue
    {
        static SignQueue _Instance;

        public static SignQueue Instance
        {
            get { return SignQueue._Instance = SignQueue._Instance ?? new SignQueue(); }

        }

        List<MyQueue> _MyQueue;
        List<SubscribeSignServer> _MySubscribeSignServerList;
        List<SubscribeSignServerStatus> _MySubscribeSignServerStatusList;


        object MyObj = new Hashtable();
        private DateTime _LastRefreshDb;
        private double _SignQueueTimeOutInMints =///5;
            1.5;
        SignQueue()
        {
            //if (_MyQueue == null)
            //{
            _MyQueue = new List<MyQueue>();
            _MySubscribeSignServerList = new List<SubscribeSignServer>();
            _MySubscribeSignServerStatusList = new List<SubscribeSignServerStatus>();
            //}
            ///Add(202, "PreferSignature", "1-16757","8347"); //CustomsRequestsSheetPM:1-16757

        }
        public string GetSignQueueWebFormUrl(int tenant, string SignByPersonalID, String CustomsRequestsSheetId, string InterfaceTypeCode, string SignStepName)
        {
            return LogitudeSettings.LogitudeURL + "/CustomWebServices/SignQueueWebForm.aspx?tenant=" + tenant.ToString() + "&SignByPersonalID=" + SignByPersonalID + "&CustomsRequestsSheetId=" + CustomsRequestsSheetId + "&InterfaceTypeCode=" + InterfaceTypeCode + "&SignStepName=" + SignStepName;
        }
        public void Add(int tenant,
            string mustSignByPersonalID, String CustomsRequestsSheetId, string InterfaceTypeCode, SignQueueByType SignQueueBy)
        {
            SignQueueByType SignatureBy = SignQueueByType.None;
            string requestMessageData;
            //string personalSignature = null;

            SignatureHubClient.SafeWakeUp(LogitudeSettings.LogitudeURL, LogitudeSettings.WorkEnvironment);



            lock ((this._MyQueue as ICollection).SyncRoot)
            {
                var q = _MyQueue.FirstOrDefault(rec => rec.CustomsRequestsSheetId == CustomsRequestsSheetId && rec.Tenant == tenant);
                if (q != null) /// change Person to sign !!!
                {
                    if (q.StartAt.HasValue)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug("In the middle of sign ?? ");
                        if (DateTime.Now.Subtract(q.StartAt.Value) < TimeSpan.FromMinutes(_SignQueueTimeOutInMints))
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("less 4 min , Waiting ...");
                            return;
                        }
                    }
                    _MyQueue.Remove(q);
                }
                var newQue = new MyQueue()
                {
                    Tenant = tenant,
                    CustomsRequestsSheetId = CustomsRequestsSheetId,
                    InterfaceTypeCode = InterfaceTypeCode,
                    MustSignByPersonalID = mustSignByPersonalID,
                    SignatureBy = SignQueueBy //GetSignatureBy(InterfaceTypeCode, SignQueueBy) // exchange rate 
                };
                if (
                    CustomsSettingQueryService.GetSettingByTenant(tenant).CompanyType == "B" //Courier
                    &&
                    newQue.SignatureBy == SignQueueByType.SignQueueByPersonId
                    )
                {
                    string availableSignServer = GetAvailableSignServer(newQue.Tenant, newQue.SignatureBy, newQue.MustSignByPersonalID);
                    if (!string.IsNullOrWhiteSpace(availableSignServer))
                    {
                        newQue.MustSignByPersonalID = SignCertificateClass.GetPersonID(availableSignServer);
                    }

                }
                

                _MyQueue.Add(newQue);
                switch (newQue.SignatureBy)
                {

                    case SignQueueByType.SignQueueByCustomsAgentId:
                        SignatureBy = SignQueueByType.SignQueueByCustomsAgentId;
                        requestMessageData = GetCustomsAgentIdFromTenant(tenant);
                        break;
                    case SignQueueByType.SignQueueByPersonId:
                        SignatureBy = SignQueueByType.SignQueueByPersonId;
                        requestMessageData = mustSignByPersonalID;
                        break;
                    default:
                        throw new Exception("newQue.SignatureBy is must be P/C !!");
                        break;
                }

                SignatureHubClient.SafeSend(LogitudeSettings.LogitudeURL, LogitudeSettings.WorkEnvironment,
                    SignatureBy, requestMessageData);
            }

        }

        //public static SignatureByType GetSignatureBy(string InterfaceTypeCode, bool forcePersonalSign)
        //{
        //    if (forcePersonalSign)
        //    {
        //        return  SignatureByType.SignQueueByPersonId;
        //    }
        //    var qs = new InterfaceManagementQueryService(0);
        //    var pm = qs.GetSingle(InterfaceTypeCode,false, true);
        //    return pm.SignatureBy;

        //}



        public bool TryDequeueReqId(string CurrentSignCertificate, bool isServerSignOn, bool isPersonalSignOn,
            out String CustomsRequestsSheetId, out string InterfaceTypeCode, out int? currTenant)
        {

            try
            {

                
                InterfaceTypeCode = CustomsRequestsSheetId = "";
                currTenant = null;
                var PersonId = SignCertificateClass.Get(CurrentSignCertificate).PersonId;
                var customsAgentId = SignCertificateClass.Get(CurrentSignCertificate).CustomsAgentId;

                var courierPersonalIdDefault = GetCourierPersonalIdDefault(GetTenantBy(customsAgentId));

                lock ((this._MyQueue as ICollection).SyncRoot)
                {


                    MyQueue myQueue = null;
                    if (isPersonalSignOn)
                    {
                        myQueue = _MyQueue
                            .FirstOrDefault(rec => rec.StartAt == null && rec.SignatureBy == SignQueueByType.SignQueueByPersonId && rec.MustSignByPersonalID == PersonId);
                    }
                    if (myQueue == null)
                    {
                        if (isServerSignOn)
                        {
                            //var tenantCommaDelimitedList = GetTenantCommaDelimitedList(CurrentSignCertificate);
                            //var tenantList = tenantCommaDelimitedList.Split(',').ToList();
                            myQueue = _MyQueue.FirstOrDefault(rec => rec.StartAt == null && rec.SignatureBy == SignQueueByType.SignQueueByCustomsAgentId && rec.Tenant.ToString() == GetCompanyTenant(CurrentSignCertificate));
                        }
                    }


                    if (myQueue != null)
                    {
                        //_MyQueue.Remove(q);

                        myQueue.StartAt = TenantServerConfigration.GetCurrentDateTime(myQueue.Tenant);// DateTime.Now;
                        CustomsRequestsSheetId = myQueue.CustomsRequestsSheetId;
                        InterfaceTypeCode = myQueue.InterfaceTypeCode;
                        currTenant = myQueue.Tenant;
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Task.Factory.StartNew(() => //may tak time witou lock 
                {
                    UpsertMySubscribeSignServerList(CurrentSignCertificate, isPersonalSignOn, isServerSignOn);
                });
            }
        }

        private int GetTenantBy(string customsAgentId)
        {
            var customsSettingQueryService = new CustomsSettingQueryService(SignQueue.CurrentTenant);
            CustomsSettingPM customsSettingPM = customsSettingQueryService.GetSettingPMByCustomsAgentId(customsAgentId);
            return customsSettingPM?.Tenant ?? 0;
        }

        public void UpsertMySubscribeSignServerList(string CurrentSignCertificate, bool isPersonalSignOn, bool isServerSignOn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentSignCertificate)) { return; }

                var companyTenant = SignQueue.GetCompanyTenant(CurrentSignCertificate);
                var tenantListOfPersonID = GetTenantListOfPersonID(SignCertificateClass.Get(CurrentSignCertificate).PersonId, 0);

                lock ((this._MyQueue as ICollection).SyncRoot)
                {
                    var mySignServer = _MySubscribeSignServerList
                    .FirstOrDefault(rec => !string.IsNullOrWhiteSpace(rec.SignCertificate) && rec.SignCertificate.Equals(CurrentSignCertificate, StringComparison.OrdinalIgnoreCase));
                    if (mySignServer != null)
                    {
                        _MySubscribeSignServerList.Remove(mySignServer);
                    }

                    mySignServer = new SubscribeSignServer()
                    {
                        SignCertificate = CurrentSignCertificate,
                        CompanyTenant = companyTenant,
                        TenantListFromPersonID = tenantListOfPersonID,
                        LastAccessedAt = DateTime.Now,
                        IsPersonalSignOn = isPersonalSignOn,
                        IsCompanySignOn = isServerSignOn
                    };
                    _MySubscribeSignServerList.Add(mySignServer);


                }
            }
            catch (Exception)
            {

                ///throw;
            }

        }

        public void UpsertSignStationStatus(SignStationStatus myReqSignData)
        {
            try
            {
                //var companyTenant = SignQueue.GetCompanyTenant(myReqSignData.CurrentSignCertificate);
                //var tenantListOfPersonID = GetTenantListOfPersonID(SignCertificateClass.Get(myReqSignData.CurrentSignCertificate).PersonId, 0);

                lock ((this._MyQueue as ICollection).SyncRoot)
                {
                    var mySignServer = _MySubscribeSignServerStatusList
                        .Where(rec => (rec.MachineName ?? "").Equals(myReqSignData.MachineName, StringComparison.OrdinalIgnoreCase))
                        .Where(rec => (rec.UserName ?? "").Equals(myReqSignData.UserName, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                    if (mySignServer != null)
                    {
                        _MySubscribeSignServerStatusList.Remove(mySignServer);
                    }

                    mySignServer = new SubscribeSignServerStatus()
                    {
                        MachineName = myReqSignData.MachineName,
                        UserName = myReqSignData.UserName,

                        SignCertificate = myReqSignData.CurrentSignCertificate,

                        CompanyTenant = "-1",
                        TenantListFromPersonID = new List<int>(),
                        LastAccessedAt = DateTime.Now,
                        IsPersonalSignOn = myReqSignData.isPersonalSignOn,
                        IsCompanySignOn = myReqSignData.isCompanySignOn,

                        Status = myReqSignData.Status,
                        LastSuccessSignningAt = myReqSignData.LastSuccessSignningAt,
                        IsOk = myReqSignData.IsOk,
                        VersionByFeatures = myReqSignData.VersionByFeatures


                    };
                    _MySubscribeSignServerStatusList.Add(mySignServer);


                }
            }
            catch (Exception)
            {

                ///throw;
            }

        }
        //public bool Done(int tenant, String CustomsRequestsSheetId)
        //{

        //    lock ((this._MyQueue as ICollection).SyncRoot)
        //    {
        //        var q = _MyQueue.FirstOrDefault(rec => rec.StartAt == null && rec.Tenant == tenant);
        //        if (q != null)
        //        {
        //            q.DoneAt = DateTime.Now;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public bool GetStartAt(int tenant, String CustomsRequestsSheetId, out DateTime StartAt)
        {
            StartAt = DateTime.Now;
            lock ((this._MyQueue as ICollection).SyncRoot)
            {
                var q = _MyQueue.FirstOrDefault(rec => rec.CustomsRequestsSheetId == CustomsRequestsSheetId && rec.Tenant == tenant);
                if (q != null)
                {
                    //duration=q.DoneAt.GetValueOrDefault().Subtract(q.StartAt.GetValueOrDefault());
                    StartAt = q.StartAt.GetValueOrDefault();
                    //_MyQueue.Remove(q);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(int tenant, String CustomsRequestsSheetId)
        {
            //StartAt = DateTime.Now;
            lock ((this._MyQueue as ICollection).SyncRoot)
            {
                var q = _MyQueue.FirstOrDefault(rec => rec.CustomsRequestsSheetId == CustomsRequestsSheetId && rec.Tenant == tenant);
                if (q != null)
                {
                    //duration=q.DoneAt.GetValueOrDefault().Subtract(q.StartAt.GetValueOrDefault());
                    //      StartAt = q.StartAt.GetValueOrDefault();
                    _MyQueue.Remove(q);
                    return true;
                }
            }
            return false;
        }



        public void RefreshDb(
            //int tenant, 
            int delta2RefreshDb = 7
            )
        {

            if (DateTime.Now.Subtract(this._LastRefreshDb) >
                //TimeSpan.FromMinutes(delta2RefreshDb))
                TimeSpan.FromSeconds(delta2RefreshDb))
            {
                this._LastRefreshDb = DateTime.Now;
                Task.Factory.StartNew(() =>
                {

                    try
                    {
                        Logitude.Server.Tools.ExternalServices.SignatureHubClient.Instance.WakeUp();
                    }
                    catch //(Exception)
                    {


                    }
                    lock ((this._MyQueue as ICollection).SyncRoot)
                    {
                        var listtoclear = _MyQueue
                            .Where(rec => rec.StartAt.HasValue &&
                                 DateTime.Now.Subtract(rec.StartAt.GetValueOrDefault()) >
                                 TimeSpan.FromMinutes(_SignQueueTimeOutInMints)
                                 ).ToList();
                        if (listtoclear != null)
                        {
                            for (int i = 0; i < listtoclear.Count; i++)
                            {
                                this._MyQueue.Remove(listtoclear[i]);
                            }
                        }
                    }

                    var customsSettingQueryService = new CustomsSettingQueryService(SignQueue.CurrentTenant);
                    var listSetting = customsSettingQueryService.GetAll()
                        .Where(rec => !string.IsNullOrWhiteSpace(rec.CustomsAgentId) && !string.IsNullOrWhiteSpace(rec.IIGServiceAddress))
                        .Where(rec => rec.Tenant > 0)
                        
                        .Where(rec => rec.IsConnectedToUniFreight)// export pilot multi tenant => use queue db 
                        ;
                    ;

                    foreach (var item in listSetting)
                    {
                        JustRefreshDb(item.Tenant);
                    }


                });

            }

        }

        private void JustRefreshDb(int tenant)
        {
            //var commonContext = CommonDataContext.GetContext(tenant);
            var customContext = CustomContext.GetContext(tenant);

            bool forcePersonalSign = false;
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(customContext);
            var waitingToSignList = customsRequestsSheetQueryService.GetWaitingForSigningListIncludeSignStepName(tenant);
            var cancellList = new List<CustomsRequestsSheetPM>();
            var us = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, IContext>(), tenant);

            foreach (var item in waitingToSignList)
            {

                if (item.RequestCreateDate.HasValue)
                {
                    var currentDateTime = TenantServerConfigration.GetCurrentDateTime(item.Tenant);
                    if (currentDateTime.Subtract(item.RequestCreateDate.GetValueOrDefault()) > TimeSpan.FromMinutes(150))
                    {
                        try
                        {

                            using (var scope = TransactionFactory.GetTransaction())
                            {


                                //cancellList.Add(item);
                                item.RequestStatusCode = "99";

                                item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                us.Update(item, true);

                                CustomsRequestsSheetDomainModelUtil.SetExceptionMessage(item.Tenant, item.Id, "Singing timeout exceeded (150Min) Cancelling  CustomsRequestsSheet !!!");
                                scope.Complete();
                            }
                            break;
                        }
                        catch (Exception eeee)
                        {

                            ///throw;
                            ///continue try sign ....
                        }

                    }
                }
                SignQueueByType mySignQueueByType = GetSignQueueByTypeFromName(item.SignStepName);
                this.Add(item.Tenant, GetUserPersonID(item.RequestOwnerId, item.Tenant), item.Id, item.InterfaceTypeCode, mySignQueueByType);
            }
        }

        public static SignQueueByType GetSignQueueByTypeFromName(string SignStepName)
        {
            if (SignStepName == SignQueue.CustomRequestSignPersonal)
            {
                return SignQueueByType.SignQueueByPersonId;
            }
            else if (SignStepName == SignQueue.CustomRequestSign)
            {
                return SignQueueByType.SignQueueByCustomsAgentId;
            }
            return SignQueueByType.None;
            //throw new Exception("GetSignQueueByTypeFromName() not found SignStepName == " + SignStepName);
        }
        public string GetUserPersonID(string RequestOwnerId, int tenant)
        {
            bool isTenantZero = false;
            var userRep = new Simplog.Data.CommonDataModel.Repositories.UserRepository(tenant);
            var user = userRep.GetSingleUser(RequestOwnerId, tenant, true);
            if (user == null)// Customer care build in tenant 0
            {
                user =
                    //userRep.GetSingleUserByCodeOrEmail(RequestOwnerId, null, 0, true);
                    userRep.GetSingleUser(RequestOwnerId, 0, true);
                isTenantZero = true;
            }
            if (user == null)
            {
                //throw new Exception("GetUserPersonID(string RequestOwnerId, int Tenant) =" + RequestOwnerId); 
                return null;
            }


            if (string.IsNullOrWhiteSpace(user.PersonalId) && user.Tenant == 0)
            {
                user = userRep.GetSingleUserByCodeOrEmailForTenant("", user.Contact.Email, tenant, true);
                if (user == null)
                {
                    return null;
                }
            }
            var userPersonID = user.PersonalId; //PersonID
            //userPersonID = "049028392";//yaron c;
            return userPersonID;
        }
        public static List<int> GetTenantListOfPersonID(string userPersonID, int seedDbTenant)
        {
            var userRep = new Simplog.Data.CommonDataModel.Repositories.UserRepository(seedDbTenant);
            List<int> myPersonTenantList = userRep.GetPersonTenantList(userPersonID) ?? new List<int>();

            return myPersonTenantList;
        }

        private class MyQueue
        {


            public int Tenant { get; set; }
            public string CustomsRequestsSheetId { get; set; }
            public string InterfaceTypeCode { get; set; }

            public string MustSignByPersonalID { get; set; }
            public SignQueueByType SignatureBy { get; set; }


            public DateTime? StartAt { get; set; }
            //public DateTime? DoneAt { get; set; }

        }


        public bool IsPasiveSignMode()
        {
            return true;
        }

        public static string GetTenantCommaDelimitedList(string CurrentSignCertificate)
        {
            var mySignCertificateClass = SignCertificateClass.Get(CurrentSignCertificate);

            var userRep = new Simplog.Data.CommonDataModel.Repositories.UserRepository(SignQueue.CurrentTenant);
            var intLIst = userRep.GetPersonTenantList(mySignCertificateClass.PersonId);
            var tenantCommaDelimitedList = "";
            if (intLIst.Count < 1) return tenantCommaDelimitedList;
            tenantCommaDelimitedList = string.Join(",", intLIst.Select(t => t.ToString()));
            return tenantCommaDelimitedList;
        }
        public static string GetCustomsAgentIdFromTenant(int tenant)
        {
            var customsSettingQueryService = new CustomsSettingQueryService(SignQueue.CurrentTenant);
            var pm = customsSettingQueryService.GetSettingByTenantN(tenant);
            return pm.CustomsAgentId;
        }
        public static string GetCompanyTenant(string CurrentSignCertificate)
        {
            var mySignCertificateClass = SignCertificateClass.Get(CurrentSignCertificate);
            var customsSettingQueryService = new CustomsSettingQueryService(SignQueue.CurrentTenant);
            var pm = customsSettingQueryService.GetSettingPMByCustomsAgentId(mySignCertificateClass.CustomsAgentId);
            if (pm == null || String.IsNullOrWhiteSpace(pm.CustomsAgentId))
            {
                return "";
            }
            return pm.Tenant.ToString();

        }

        public string GetAvailableSignServer(int tenant, SignQueueByType SignatureBy
            , string personId
            )
        {
       
            SubscribeSignServer availableSignServer = null;
            var copyOfMySubscribeSignServerList = GetCopyOfMySubscribeSignServerList(tenant);
            if (copyOfMySubscribeSignServerList == null)
            {
                return null;
            }
            switch (SignatureBy)
            {

                case SignQueueByType.SignQueueByCustomsAgentId:

                    availableSignServer = copyOfMySubscribeSignServerList
                    .FirstOrDefault(rec =>
                        //rec.CompanyTenant == tenant.ToString() && rec.IsCompanySignOn == true);
                        rec.MySignCertificateClass.CustomsAgentId == GetCustomsAgentIdFromTenant(tenant)
                        && rec.IsCompanySignOn == true);
                    break;
                case SignQueueByType.SignQueueByPersonId:

                    var courierPersonalIdDefault = GetCourierPersonalIdDefault(tenant);
                    if (String.IsNullOrWhiteSpace(personId))
                    {
                        if (String.IsNullOrWhiteSpace(courierPersonalIdDefault))
                        {
                            return null;
                        }    
                    }
                    availableSignServer = copyOfMySubscribeSignServerList
                    .FirstOrDefault(rec => rec.IsPersonalSignOn == true &&
                        rec.MySignCertificateClass
                        .PersonId.Equals(personId, StringComparison.OrdinalIgnoreCase));
                    if (availableSignServer == null)
                    {
                        
                        availableSignServer = copyOfMySubscribeSignServerList
                    .FirstOrDefault(rec => rec.IsPersonalSignOn == true &&
                        rec.MySignCertificateClass
                        .PersonId.Equals(courierPersonalIdDefault, StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                default:
                    throw new Exception("GetAvailableSignServer() while SignatureBy Not P/C");
                    break;
            }

            if (availableSignServer == null)
            {
                return null;
            }
            return availableSignServer.SignCertificate;
        }

        public static string GetCourierPersonalIdDefault(int tenant)
        {

            if (CustomsSettingQueryService.GetSettingByTenant(tenant).CompanyType == "B")//Courier)
            {
                DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);

                string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_PERSONID", "NON", "NON", tenant);
                return defValue;
            }
            return null;
        }

        public List<SignQueuePM> GetSignQueueList(int tenant)
        {

            List<SignQueuePM> myCopy = null;
            lock ((this._MyQueue as ICollection).SyncRoot)
            {
                myCopy = _MyQueue
                    .Where(rec => rec.Tenant == tenant)
                    .Select(myQueue => new SignQueuePM()
                    {
                        Tenant = myQueue.Tenant,
                        CustomsRequestsSheetId = myQueue.CustomsRequestsSheetId,
                        InterfaceTypeCode = myQueue.InterfaceTypeCode,
                        SignatureBy = myQueue.SignatureBy,
                        MustSignByPersonalID = myQueue.MustSignByPersonalID,
                        StartAt = myQueue.StartAt
                    })
                    .ToList();
            }

            return myCopy;

        }

        public List<SubscribeSignServer> GetCopyOfMySubscribeSignServerList(int tenant, int SubscribeSignServerTimeOutInMinutes = 3)
        {
            List<SubscribeSignServer> myCopy = null;


            lock ((this._MyQueue as ICollection).SyncRoot)
            {
                myCopy = _MySubscribeSignServerList
                    .Where(rec => rec.CompanyTenant == tenant.ToString() || rec.TenantListFromPersonID.Contains(tenant)
                      || rec.MySignCertificateClass.CustomsAgentId  == GetCustomsAgentIdFromTenant(tenant)) //multi tenant 
                    .ToList();
                myCopy = myCopy.Where(rec => DateTime.Now.Subtract(rec.LastAccessedAt) < TimeSpan.FromMinutes(SubscribeSignServerTimeOutInMinutes))
                    .ToList(); ;
            }
            return myCopy;
        }

        public List<SubscribeSignServerStatus> GetCopyOfMySubscribeSignServerStatusList(int SubscribeSignServerTimeOutInMinutes = 3)
        {
            List<SubscribeSignServerStatus> myCopy = null;

            lock ((this._MyQueue as ICollection).SyncRoot)
            {
                myCopy = _MySubscribeSignServerStatusList
                    .Where(rec => DateTime.Now.Subtract(rec.LastAccessedAt) < TimeSpan.FromMinutes(SubscribeSignServerTimeOutInMinutes))
                    .ToList();

            }
            return myCopy;
        }


        public class SignQueuePM
        {

            public int Tenant { get; set; }
            public string CustomsRequestsSheetId { get; set; }
            public string InterfaceTypeCode { get; set; }

            public string MustSignByPersonalID { get; set; }
            public SignQueueByType SignatureBy { get; set; }


            public DateTime? StartAt { get; set; }




        }

        public static string CustomRequestSign { get { return "CustomRequestSign"; } }

        public static string CustomRequestSignPersonal { get { return "CustomRequestSignPersonal"; } }
        
        public static int CurrentTenant = SettingUtil.GetCurrentTenant();
    }


}
