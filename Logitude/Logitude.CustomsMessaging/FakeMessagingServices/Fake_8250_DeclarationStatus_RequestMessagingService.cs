using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_8250_DeclarationStatus_RequestMessagingService
    {
        private ResponseContentHeader _responseContentHeader;
        private DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswer[] _declarationStatusAnswer;
        private DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetails _decDetails;
        internal DF_NG_8251_Web02_DeclarationStatus_Response GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            SetResponseContentHeader();
            SetDF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswer(requestParamsData);
            return new DF_NG_8251_Web02_DeclarationStatus_Response()
            {
                ResponseContentHeader = _responseContentHeader,
                DeclarationStatusAnswer= _declarationStatusAnswer
            };
        }

        private void SetDF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswer(GenericRequestParams requestParamsData)
        {
            _declarationStatusAnswer = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswer[2];
            SetDeclarationStatusDetails(requestParamsData);

            _declarationStatusAnswer[0] = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswer
            {
                SequenceNumber = 1,
                DeclarationStatusDetails=_decDetails
            };
        }

        private void SetDeclarationStatusDetails(GenericRequestParams requestParamsData)
        {
            _decDetails = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetails();
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsData.Tenant);
            DeclarationPM _dec = declarationQueryService.GetSingle(requestParamsData.AppicationId, false, false);
            ConsignmentQueryService consignmentQueryService = new ConsignmentQueryService(requestParamsData.Tenant);
            ConsignmentPM _con = consignmentQueryService.GetSingle(_dec.Id, 1, false, false);
            _decDetails.DeclarationTypeCode = 1;
            _decDetails.DeclarationVersion = _dec.VersionId;
            _decDetails.DeclarationID = _dec.DeclarationNumber;
            _decDetails.DeclarationOfficeID = _dec.DeclarationOfficeCode;
            _decDetails.DeclarationOfficeText = _dec.DeclarationOfficeName;
            //_decDetails.TaxationDateTime=

            _decDetails.DeclarationStatusCode = _dec.DeclarationStatusTypeCode;
            _decDetails.DeclarationAvailabilityLog = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetailsDeclarationAvailabilityLog();
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetailsDeclarationAvailabilityLogAvailabiltyLogDeclarationCargoLocation[2];
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation[0] = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetailsDeclarationAvailabilityLogAvailabiltyLogDeclarationCargoLocation();
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation[0].CargoIdentifierTypeCode = int.Parse(_con.CargoTypeCode);
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation[0].CargoIdentifierKey1 = _con.ManifestNumber;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation[0].CargoIdentifierKey2 = _con.SecondCargoID;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation[0].CargoIdentifierKey3 = _con.ThirdCargoID;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoLocation[0].SiteID = _con.StorageSiteCode;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetailsDeclarationAvailabilityLogAvailabiltyLogDeclarationCargoQuantities[2];
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0] = new DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetailsDeclarationAvailabilityLogAvailabiltyLogDeclarationCargoQuantities();
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].CargoIdentifierTypeCode = int.Parse(_con.CargoTypeCode);
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].CargoIdentifierKey1 = _con.ManifestNumber;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].CargoIdentifierKey2 = _con.SecondCargoID;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].CargoIdentifierKey3 = _con.ThirdCargoID;
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].CargoPackageTypeCode = "PP";
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].CargoWeightMeasurementUnitCode = "KGM";
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].DeclarationPackageTypeCode = "PP";
            _decDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities[0].DeclarationWeightMeasurementUnitCode = "KGM";

        }

        private void SetResponseContentHeader()
        {
            _responseContentHeader = new ResponseContentHeader()
            {
                TransmitionDateTime=DateTime.Now
            };
        }
    }
}
