using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.DeclarationDeal;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_DCAInDF_NG_2470_MSG16001_ReleaseGoodsMessagingService
    {
        private DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageConsignment[] _consigment;
        private DF_NG_2470_DF_MSG16001_ReleaseGoodsMessagePackagesInDeliverySite[] _deliverySite;
        private ActualPackagesInSite[] _packageSite;
        private DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageGeneralData _generalData;
        private DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageCustomers _messageCustomers;
        private DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageSites _messageSites;
        private RequestContentHeader _requestContentHeader;
        internal DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            SetDeliverySite();
            SetConsigment();
            SetPackageSite();
            SetGeneralData(requestParamsData);
            SetMessageCustomers();
            SetMessageSites();
            GetRequestContentHeader();
            DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage fake = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessage
            {
                GeneralData = _generalData,
                Customers = _messageCustomers,
                Sites = _messageSites,
                Consignment = _consigment,
                PackagesInDeliverySite = _deliverySite,
                ActualPackagesInSite = _packageSite,
                RequestContentHeader = _requestContentHeader
            };
            return fake;
        }
        public void SetDeliverySite()
        {
            _deliverySite = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessagePackagesInDeliverySite[1];
            
        }
        public void SetConsigment()
        {
            _consigment = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageConsignment[1];

        }
        public void SetPackageSite()
        {
            _packageSite = new ActualPackagesInSite[1];

        }
        public void SetGeneralData(GenericRequestParams requestParamsData)
        {
            _generalData = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageGeneralData();
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            _generalData.declarationID = _dec.DeclarationNumber;
            _generalData.type = "1";
            _generalData.governmentProcedureType = 4000001;
            _generalData.ReleaseMessageCode = 1;
            _generalData.currentDate = DateTime.Now;
            _generalData.releaseDate = DateTime.Now;
            _generalData.dealValueNISSpecified = true;
            _generalData.dealValueNIS = _dec.DealValue;
            _generalData.CifValueNisSpecified = true;
            _generalData.CifValueNis = _dec.CIFValue;
            
        }
        public void SetMessageCustomers()
        {
            _messageCustomers = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageCustomers();
        }
        public void SetMessageSites()
        {
            _messageSites = new DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageSites();

        }
        public void GetRequestContentHeader()
        {
            _requestContentHeader = new RequestContentHeader
            {
                TransmitionDateTime = DateTime.Now
            };
        }
    }

}
