using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CustomsWorkerRole.Test
{
    public class LoadTestService
    {
        int _Tenant;
        DeclarationQueryService _DeclarationQueryService;
        private DeclarationUpdateService _DeclarationUpdateService;
        private List<DeclarationPM> _MyDeclarationPMList;
        private ICustomContext _CustomContext;
        private int _TotalReq;
        private DateTime _LastFound;
        private DateTime _DoOneDeclartionAt;
        private string _MyUserId;
        private CustomsDocumentQueryService _CustomsDocumentQueryService;
        private CustomsDocumentUpdateService _CustomsDocumentUpdateService;
        private List<CustomsDocumentPM> _MyCustomsDocumentList;
        private List<string> _MyDecKeyList;

        private int _CustomsDocument;
        private int _ByWorkerRole;
        private int _ByDca;
        private DeclarationPM _My1stDeclarationPM;

        //public bool StressWeb { get; set; }

        public LoadTestService(int tenant)
        {
            _Tenant = tenant;



        }
        public List<String> GetDeclarationPMLoadTestTop(int top)
        {
            _CustomContext = CustomContext.GetContext(_Tenant);
            _DeclarationQueryService = new DeclarationQueryService(_Tenant);
            _DeclarationUpdateService = new DeclarationUpdateService(_CustomContext, new Dictionary<string, IContext>(), _Tenant);

            _MyDeclarationPMList = _DeclarationQueryService.GetLoadTest(_Tenant, top);
            _My1stDeclarationPM = _MyDeclarationPMList.FirstOrDefault();
            _MyDecKeyList = _MyDeclarationPMList.Select(r => r.Id).ToList();
            return _MyDeclarationPMList.Select(rec => rec.Id + "," + rec.CustomFileNo).ToList();
        }
        public void Start(int totalReq, String myUserId)
        {
            _MyUserId = myUserId;
            _TotalReq = totalReq;
            if (_MyDeclarationPMList != null)
            {
                DoDeclaration();

            }
            if (_MyCustomsDocumentList != null)
            {
                DoDocument();
            }
            if (CheckExchangeRate)
            {
                ExchangeRateTest();
                


            }
        }

        private void ExchangeRateTest()
        {
            //SendExchangeRate(MethodEnum.ByDca);
            //SendExchangeRate(MethodEnum.ByWebRole);
            //SendExchangeRate(MethodEnum.ByWorkerRole);
            //int i = 1;
            for (int i = 0; i < 2; i++)
            {

                Task.Factory.StartNew(() =>
                {
                    while (_TotalReq > 0)
                    {
                        Thread.Sleep(1000);
                        SendExchangeRate(MethodEnum.ByWebRole, 
                            DateTime.Now.AddDays(-1*i));
                        
                    }
                    Debug.WriteLine("End!!!!!!!!!!!!!!"); ;
                    Debug.WriteLine("End!!!!!!!!!!!!!!"); ;

                }
                );
            }
            
            Task.Factory.StartNew(() =>
            {
                while (_TotalReq > 0)
                {
                    //Thread.Sleep(TimeSpan.FromMinutes(1));
                    //SendExchangeRate(MethodEnum.ByWorkerRole, DateTime.Now.AddDays(-5 ));
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                    SendExchangeRate(MethodEnum.ByDca, DateTime.Now.AddDays(-7));
                    //Thread.Sleep(TimeSpan.FromMinutes());
                }
                Debug.WriteLine("End!!!!!!!!!!!!!!"); ;
                Debug.WriteLine("End!!!!!!!!!!!!!!"); ;


            }
            );
        }
        void SendExchangeRate(MethodEnum via,DateTime date)
        {

            
                try
                {
                    var requestParams = new CD_NG_8347_Web01_CurrencyRateSearchRequestParams()
                    {
                        IsFakeResponse = true,
                        LoggingEnabled = false,
                        RequestName = "Send Currency Rate Request(load Test)",
                        ResponseName = "Send Currency Rate Response(load Test)",
                        Tenant = _Tenant,
                        CurrencyTypeId = "USD",
                        FromDate = date,
                        ToDate = date,



                    };
                    switch (via)
                    {

                        case MethodEnum.ByWebRole:
                            requestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                            break;
                        case MethodEnum.ByDca:
                            requestParams.RequestVIA = SendRequestVIA.DCABatch;
                            break;
                        default:
                        case MethodEnum.ByWorkerRole:
                            requestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                            break;
                    }


                    MemoryStream memstream = new MemoryStream();
                    XmlSerializer ser = new XmlSerializer(typeof(CD_NG_8347_Web01_CurrencyRateSearchRequestParams));
                    ser.Serialize(memstream, requestParams);
                    memstream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(memstream);
                    string content = reader.ReadToEnd();
                    byte[] bytearray = memstream.ToArray();

                    BasicHttpBinding binding = GetHttpBinding();



                    var uri = LogitudeSettings.LogitudeURL + "/CustomWebServices/CustomsExchangeRateWebService.asmx";

                    var declarationServiceReference = new CustomsExchangeRateWebServiceReference.CustomsExchangeRateWebServiceSoapClient(binding, new EndpointAddress(uri));

                    if (declarationServiceReference.Endpoint.Address.Uri.Scheme == "https")
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                    }
                    else
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.None;
                    }


                    declarationServiceReference.SearchCurrencyRateRequest(bytearray);
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("SearchCurrencyRateRequest:via" + via.ToString());



                }
                catch (Exception eee)
                {

                    Debug.WriteLine(eee.ToString()); ;
                    Thread.Sleep(5000);
                }
                

        }

        private void DoDocument()
        {
            foreach (var item in _MyCustomsDocumentList)
            {
                DoOneCustomsDocument(item);
            }

            Task.Factory.StartNew(() =>
            {
                while (_TotalReq > 0)
                {

                    try
                    {
                        _CustomContext = CustomContext.GetContext(_Tenant);
                        _CustomsDocumentQueryService = new CustomsDocumentQueryService(_Tenant);
                        _CustomsDocumentUpdateService = new CustomsDocumentUpdateService(_CustomContext, new Dictionary<string, IContext>(), _Tenant);


                        CustomsDocumentPM item = FindEndedCustomsDocumentPM();
                        if (item != null)
                        {

                            DoOneCustomsDocument(item);
                            _CustomsDocument++;
                            _TotalReq--;
                        }
                        else
                        {
                            if (DateTime.Now.Subtract(_DoOneDeclartionAt) > TimeSpan.FromMinutes(3))
                            {
                                _CustomContext = CustomContext.GetContext(_Tenant);
                                _CustomsDocumentQueryService = new CustomsDocumentQueryService(_Tenant);
                                _CustomsDocumentUpdateService = new CustomsDocumentUpdateService(_CustomContext, new Dictionary<string, IContext>(), _Tenant);

                                foreach (var item1 in _MyCustomsDocumentList)
                                {

                                    DoOneCustomsDocument(item1);

                                }
                            }
                        }



                    }
                    catch (Exception eee)
                    {

                        Debug.WriteLine(eee.ToString()); ;
                        Thread.Sleep(5000);
                    }

                    Thread.Sleep(1000);
                }
                Debug.WriteLine("End!!!!!!!!!!!!!!"); ;
                Debug.WriteLine("End!!!!!!!!!!!!!!"); ;
            });
        }

        private void ReFreshDecList()
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {

                _MyDeclarationPMList = _DeclarationQueryService.GetMultiByKeys(_Tenant, _MyDecKeyList);
            }

        }

        private CustomsDocumentPM FindEndedCustomsDocumentPM()
        {

            using (var scope = TransactionFactory.GetNewTransaction())
            {

                var listKeys = _MyCustomsDocumentList.Select(rec => rec.DocumentsFilingId).ToList();
                var l = _CustomsDocumentQueryService.GetLoadTest(_Tenant, 10, listKeys);
                var dec = l.FirstOrDefault();
                if (dec != null)
                {
                    _LastFound = DateTime.Now;
                    return dec;
                }
                if (DateTime.Now.Subtract(_LastFound) > TimeSpan.FromMinutes(5))
                {

                }
                return null;
            }
        }

        private void DoDeclaration()
        {
            foreach (var item in _MyDeclarationPMList)
            {

                DoOneDeclaration(item);

            }
            Task.Factory.StartNew(() =>
            {
                while (_TotalReq > 0)
                {

                    try
                    {
                        _CustomContext = CustomContext.GetContext(_Tenant);
                        _DeclarationQueryService = new DeclarationQueryService(_Tenant);
                        _DeclarationUpdateService = new DeclarationUpdateService(_CustomContext, new Dictionary<string, IContext>(), _Tenant);


                        DeclarationPM item = FindEndedDeclarationPM();
                        if (item != null)
                        {

                            DoOneDeclaration(item);
                            switch (GetMethodEnum(item.CustomFileNo))
                            {
                                case MethodEnum.ByWorkerRole:
                                    _ByWorkerRole++;
                                    break;
                                case MethodEnum.ByWebRole:
                                    _ByWebRole++;
                                    break;
                                case MethodEnum.ByDca:
                                    _ByDca++;
                                    break;

                            }
                            _TotalReq--;
                        }
                        else
                        {
                            if (DateTime.Now.Subtract(_DoOneDeclartionAt) > TimeSpan.FromMinutes(3))
                            {
                                _CustomContext = CustomContext.GetContext(_Tenant);
                                _DeclarationQueryService = new DeclarationQueryService(_Tenant);
                                _DeclarationUpdateService = new DeclarationUpdateService(_CustomContext, new Dictionary<string, IContext>(), _Tenant);
                                ReFreshDecList();
                                foreach (var item1 in _MyDeclarationPMList)
                                {

                                    var byMethodEnum = GetMethodEnum(item1.CustomFileNo);
                                    if (byMethodEnum != MethodEnum.ByDca)
                                    {
                                        DoOneDeclaration(item1);
                                    }

                                }
                            }
                        }



                    }
                    catch (Exception eee)
                    {

                        Debug.WriteLine(eee.ToString()); ;
                        Thread.Sleep(5000);
                    }

                    Thread.Sleep(1000);
                }
                Debug.WriteLine("End!!!!!!!!!!!!!!"); ;
                Debug.WriteLine("End!!!!!!!!!!!!!!"); ;

                Debug.WriteLine("ByDca:"+_ByDca.ToString()); ;
                Debug.WriteLine("ByWebRole:" + _ByWebRole.ToString()); ;
                Debug.WriteLine("ByWorkerRole:" + _ByWorkerRole.ToString()); ;
                Debug.WriteLine("CustomsDocument:" + _CustomsDocument.ToString()); ;
            });
        }

        private void DoOneCustomsDocument(CustomsDocumentPM item)
        {

            GenericRequestParams requestParams = null;


            using (var scope = TransactionFactory.GetNewTransaction())
            {
                try
                {

                    item.ForceRemoveCustomsDocId = true;
                    item.CustomsDocId = "";//clear
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    _CustomsDocumentUpdateService.Update(item, true);


                    item.DocumentRemarks = CustomsDocumentUpdateService.LoadTestSendMessageToQueue;
                    item.DocumentVersion++;
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    _CustomsDocumentUpdateService.Update(item, true);
                    scope.Complete();

                }
                catch (Exception eee)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(eee.ToString());
                    Thread.Sleep(300);
                    return;
                }


            }

        }

        private DeclarationPM FindEndedDeclarationPM()
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {

                var listKeys = _MyDeclarationPMList.Select(rec => rec.Id).ToList();
                var l = _DeclarationQueryService.GetLoadTest(_Tenant, 10, listKeys);
                var dec = l.OrderBy(R => R.UpdateDateTime).FirstOrDefault();
                if (dec != null)
                {
                    _LastFound = DateTime.Now;
                    return dec;
                }
                if (DateTime.Now.Subtract(_LastFound) > TimeSpan.FromMinutes(5))
                {

                }
                return null;
            }


        }

        enum MethodEnum
        {
            ByWorkerRole = 0,
            ByWebRole = 1,
            ByDca = 2,
            BySignWebRole = 3,
        }

        private void DoOneDeclaration(DeclarationPM item)
        {
            GenericRequestParams requestParams = null;

            var byMethodEnum =//StressWeb && 
                GetMethodEnum(item.CustomFileNo);
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                try
                {

                    item.UserNotes = "LoadTestOnProgress";
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    _DeclarationUpdateService.Update(item, true);

                    var mySBQMessage = new SBQMessageService();

                    requestParams = new GenericRequestParams()
                    {
                        // RequestVIA = byWeb ? SendRequestVIA.WebServiceInteractive : SendRequestVIA.WebServiceBatch,
                        Tenant = _Tenant,
                        AppicationId = item.Id,
                        TestCase = null,

                        ///<-- itzik (yaron ask )
                        LoggingEnabled = true,
                        LoggingEntityId = item.Id,
                        LoggingEntityReference = item.DeclarationNumber,
                        LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                        //LoggingUserId = TenantContext.Current.LoggedContactId,
                        RequestName = "Declaration Request",
                        ResponseName = "Declaration Response(LoadTest)",


                        ///itzik (yaron ask )-->
                    };
                    switch (byMethodEnum)
                    {
                        case MethodEnum.ByWorkerRole:
                            requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                            break;
                        case MethodEnum.BySignWebRole:
                            if (!String.IsNullOrWhiteSpace(_MyUserId))
                            {
                                requestParams.LoggingUserId = _MyUserId;//Set LoggingUserIdRequestOwnerId = Get Personal Id
                                requestParams.ForcePersonalSign = true;
                            }
                            requestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                            break;
                        case MethodEnum.ByWebRole:
                            requestParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                            break;
                        case MethodEnum.ByDca:
                            requestParams.RequestVIA = SendRequestVIA.DCABatch;
                            break;
                        default:
                            requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                            break;
                    }
                    
                }
                catch (Exception eee)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(eee.ToString());
                    Thread.Sleep(300);
                    return;
                }

                //var _CustomsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(requestParams.Tenant);
                if (byMethodEnum == MethodEnum.ByWebRole || byMethodEnum == MethodEnum.BySignWebRole )
                {
                    scope.Complete();
                    MemoryStream memstream = new MemoryStream();
                    XmlSerializer ser = new XmlSerializer(typeof(GenericRequestParams));
                    ser.Serialize(memstream, requestParams);
                    memstream.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(memstream);
                    string content = reader.ReadToEnd();
                    byte[] bytearray = memstream.ToArray();

                    BasicHttpBinding binding = GetHttpBinding();



                    var uri = LogitudeSettings.LogitudeURL + "/CustomWebServices/DeclarationWebService.asmx";

                    var declarationServiceReference = new DeclarationWebServiceReference.DeclarationWebServiceSoapClient(binding, new EndpointAddress(uri));

                    if (declarationServiceReference.Endpoint.Address.Uri.Scheme == "https")
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                    }
                    else
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.None;
                    }


                    declarationServiceReference.SendDeclarationAsync(bytearray, item.Id, item.Tenant);
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Web:SendDeclarationAsync" + item.Id.ToString());
                }
                else
                {
                    try
                    {
                        var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
                        var responseData = messService.SendSheet(requestParams);
                        scope.Complete();
                        _DoOneDeclartionAt = DateTime.Now;
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Queue:SendSheet(requestParams)" + item.Id.ToString() + ":" + requestParams.RequestVIA.ToString());
                        //SBQMessageService
                        //    .CreateSheetSBQMessage
                        //    <Logitude.CustomsMessaging.Common.RequestParams.GenericRequestParams>
                        //    (requestParams, false);

                    }
                    catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                    {
                        if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                        {
                            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("2715 RequestInProgress stop create a new one !! ");

                        }
                        throw;
                    }
                }

            }

        }

        private static BasicHttpBinding GetHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.SendTimeout = TimeSpan.FromMinutes(5);//huge !!!
            return binding;
        }

        private MethodEnum GetMethodEnum(string customFileNo)
        {

            if (!String.IsNullOrWhiteSpace(_MyUserId))
            {
                if (_My1stDeclarationPM != null && _My1stDeclarationPM.CustomFileNo == customFileNo)
                {
                    return MethodEnum.BySignWebRole;
                }
            }
            
            customFileNo = customFileNo ?? "1";
            customFileNo = customFileNo.Substring(customFileNo.Length - 1, 1);


            int my = int.Parse(customFileNo) % 3;
            switch (my)
            {
                case 1:
                    return MethodEnum.ByWebRole;
                    break;
                case 2:
                    return MethodEnum.ByDca;
                    break;
                case 0:
                default:
                    return MethodEnum.ByWorkerRole;
                    break;
            }

        }



        public static string GetUserIdByPersonalId(string personalId, int tenant)
        {
            var repo = new Simplog.Data.CommonDataModel.Repositories.UserRepository(tenant);
            var id = repo.GetUserIdByPersonalId(personalId, tenant);
            return id;

        }

        public List<string> GetCustomsDocumentLoadTestTop(int p)
        {
            _CustomContext = CustomContext.GetContext(_Tenant);
            _CustomsDocumentQueryService = new CustomsDocumentQueryService(_Tenant);
            _CustomsDocumentUpdateService = new CustomsDocumentUpdateService(_CustomContext, new Dictionary<string, IContext>(), _Tenant);

            _MyCustomsDocumentList = _CustomsDocumentQueryService.GetLoadTest(_Tenant, 50);
            return _MyCustomsDocumentList.Select(rec => rec.DocumentsFilingId).ToList();
        }

        public bool CheckExchangeRate { get; set; }

        public int _ByWebRole { get; set; }
    }
}
