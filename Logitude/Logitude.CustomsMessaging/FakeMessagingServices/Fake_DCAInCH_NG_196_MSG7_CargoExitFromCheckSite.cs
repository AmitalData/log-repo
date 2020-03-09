using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.PhysicalCheck;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_DCAInCH_NG_196_MSG7_CargoExitFromCheckSite
    {
        private CH_NG_196_MSG7_CargoExitFromCheckSiteGeneralDetails _generalDetails;
        private CH_NG_196_MSG7_CargoExitFromCheckSiteCheckEntity _checkEntity;
        private RequestContentHeader _requestContentHeader;
        private PhysicalCheckPM _phy;
        private dynamic params1;
        public DeclarationPM _dec;

        internal CH_NG_196_MSG7_CargoExitFromCheckSite GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            params1 = JObject.Parse(requestParamsData.TestCase.Param1);
            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(requestParamsData.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            _dec = new DeclarationPM();
            _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            Boolean parseCheckId = int.TryParse(Convert.ToString(params1.checkId), out int checkId);
            if (params1.checkId != null && parseCheckId)
            {
                _phy = physicalCheckQueryService.GetPhysicalCheckByCheckId(Convert.ToString(checkId));
            }
            else
            {
                _phy = physicalCheckQueryService.GetPhysicalCheckByDeclarationId(requestParamsData.AppicationId);
            }
            GetRequestContentHeader();
            GetGeneralDetails();
            GetCheckEntity();
            return new CH_NG_196_MSG7_CargoExitFromCheckSite()
            {
                RequestContentHeader = _requestContentHeader,
                generalDetails = _generalDetails,
                CheckEntity = _checkEntity,
            };
        }
        public void GetGeneralDetails()
        {
            _generalDetails = new CH_NG_196_MSG7_CargoExitFromCheckSiteGeneralDetails();
            if(_phy.DeclarationId!= null)
            {
                _generalDetails.declarationID = _dec.DeclarationNumber;
            }
            if (_phy.CheckSiteCode != null)
            {
                _generalDetails.checkSiteNumber = _phy.CheckSiteCode;
            }
            if (_phy.CheckId != null)
            {
                _generalDetails.checkId = int.Parse(_phy.CheckId);
            }
            if (_phy.ImporterNumber != null)
            {
                _generalDetails.importerNumber = int.Parse(_phy.ImporterNumber);
            }
                _generalDetails.endDate = DateTime.Now;
            if(_phy.CargoTypeCode != null)
            {
                _generalDetails.entityType = int.Parse(_phy.CargoTypeCode);
            }
         
        }
        public void GetCheckEntity()
        {
            _checkEntity = new CH_NG_196_MSG7_CargoExitFromCheckSiteCheckEntity
            {
                cargoIdentifier = new cargoIdentifier()
            };
            if (_phy.CargoIdentifierTypeCode != null)
            {
                _checkEntity.cargoIdentifier.cargoIdentifierType = int.Parse(_phy.CargoIdentifierTypeCode);
            }
            if(_phy.CargoIdentifierKey1 !=null)
            {
                _checkEntity.cargoIdentifier.cargoIdentifierKey1 = _phy.CargoIdentifierKey1;
            }
            if (_phy.CargoIdentifierKey2 != null)
            {
                _checkEntity.cargoIdentifier.cargoIdentifierKey2 = _phy.CargoIdentifierKey2;
            }
            if (_phy.CargoIdentifierKey3 != null)
            {
                _checkEntity.cargoIdentifier.cargoIdentifierKey3 = _phy.CargoIdentifierKey3;
            }
            _checkEntity.containerNumber = "11111";
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
