using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.Practices.Unity;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using WebFreight.Web.CustomWebServices.SignChunks.Common;
using Logitude.Server.Tools.Utils;
using System.Collections;

namespace WebFreight.Web.CustomWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SignService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SignService.svc or SignService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class SignService : ISignService
    {
        public SignService()
        {

        }
        public void DoWork()
        {
        }
        public Byte[] ReceiveBytesToSign(string CurrentSignCertificate, bool isCompanySignOn, bool isPersonalSignOn,
            out String CustomsRequestsSheetId, out string InterfaceTypeCode, out int? currTenant)
        {

            try
            {
                if (!SignQueue.Instance.TryDequeueReqId(CurrentSignCertificate, isCompanySignOn, isPersonalSignOn, out CustomsRequestsSheetId, out InterfaceTypeCode, out currTenant))
                {

                    return null;
                    //var resDat = anaO.SendSheet(tenant, CustomsRequestsSheetId);
                    //MessagingServiceFactoryHelper.ResolveAndExecute(InterfaceTypeCode, tenant, CustomsRequestsSheetId, myCustomsCommandEnum);
                }
                MessagingServiceFactoryHelper.InitContainer();
                var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(InterfaceTypeCode);

                //anaO.CurrentCustomsCommandWR = myCustomsCommandEnum;
                Byte[] ReceiveBytesToSign = anaO.PasiveSignGetBytesToSign(currTenant.GetValueOrDefault(), CustomsRequestsSheetId);
                return ReceiveBytesToSign;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SignQueue.Instance.RefreshDb();
            }


        }
        private class MyQueue
        {

            public int Tenant { get; set; }
            public string CustomsRequestsSheetId { get; set; }
            public DateTime DoneAt { get; set; }

        }
        private static List<MyQueue> _CustomsRequestsSheetDoneList = new List<MyQueue>();
        public void CompleteResponseSignBytes(int tenant, String CustomsRequestsSheetId, string InterfaceTypeCode, string CurrentSignCertificate,
            Byte[] mySignBytes)
        {
            bool supressCompleteResponseSignBytes = false;
            try
            {

                bool toContinue = false;
                lock ((_CustomsRequestsSheetDoneList as ICollection).SyncRoot)
                {
                    if (_CustomsRequestsSheetDoneList.Exists(rec =>
                rec.CustomsRequestsSheetId == CustomsRequestsSheetId &&
                rec.Tenant == tenant
                ))
                    {
                        supressCompleteResponseSignBytes = true;
                        toContinue = false;
                    }
                    else
                    {
                        _CustomsRequestsSheetDoneList.Add(
                            new MyQueue()
                            {
                                Tenant = tenant,
                                CustomsRequestsSheetId = CustomsRequestsSheetId,
                                DoneAt = DateTime.Now

                            });
                        toContinue = true;
                    }
                }
                if (!toContinue) return;
                    
                var mySendSheetSignModel = new SendSheetSignModel();
                mySendSheetSignModel.CustomsRequestsSheetId = CustomsRequestsSheetId;
                mySendSheetSignModel.Tenant = tenant;
                mySendSheetSignModel.CustomRequestSignedByteArryPasiveSign = mySignBytes;
                mySendSheetSignModel.CurrentSignCertificateName = CurrentSignCertificate;
                Task.Factory.StartNew(() =>
                {
                    MessagingServiceFactoryHelper.InitContainer();
                    var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(InterfaceTypeCode);
                    //anaO.CompleteResponseSignBytes(tenant, CustomsRequestsSheetId, mySignBytes);
                    anaO.SendSheet(tenant, CustomsRequestsSheetId, mySendSheetSignModel);
                });

                //cancelCheckFile.Token,
                //TaskCreationOptions.LongRunning,
                //TaskScheduler.Default);
            }
            finally
            {
                //Logger.LogMe(
                //    "CustomsRequestsSheetId=" + CustomsRequestsSheetId + ";supressCompleteResponseSignBytes=" + supressCompleteResponseSignBytes.ToString(), false, "CompleteResponseSignBytes");
                try
                {
                    _CustomsRequestsSheetDoneList.RemoveAll(rec => DateTime.Now.Subtract(rec.DoneAt) > TimeSpan.FromSeconds(90));
                }
                catch (Exception)
                {


                }
            }


        }



        public void GetTenantFromCertificate(string CurrentSignCertificate, out string PersonalTenantCommaDelimitedList, out string CompanyTenant)
        {
            PersonalTenantCommaDelimitedList = SignQueue.GetTenantCommaDelimitedList(CurrentSignCertificate);
            CompanyTenant = SignQueue.GetCompanyTenant(CurrentSignCertificate);
        }


        public ReceiveBytesToSignResponse GetBytesToSign(ReceiveBytesToSignReq myReceiveBytesToSignReq)
        {
            String CustomsRequestsSheetId = "";
            string InterfaceTypeCode = "";
            int? currTenant = null;
            var bytes = this.ReceiveBytesToSign(myReceiveBytesToSignReq.CurrentSignCertificate, myReceiveBytesToSignReq.isCompanySignOn, myReceiveBytesToSignReq.isPersonalSignOn,
                out CustomsRequestsSheetId, out InterfaceTypeCode, out currTenant);
            var myReceiveBytesToSignResponse = new ReceiveBytesToSignResponse()
            {
                currTenant = currTenant.GetValueOrDefault(),
                InterfaceTypeCode = InterfaceTypeCode,
                CustomsRequestsSheetId = CustomsRequestsSheetId,
                ReceiveBytesToSign = bytes
            };
            return myReceiveBytesToSignResponse;
        }









    }
}
