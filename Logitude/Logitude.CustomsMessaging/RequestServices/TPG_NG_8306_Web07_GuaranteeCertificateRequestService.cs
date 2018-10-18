//Yuval Chalup 08.09.2015 TASK-15038
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CreditQueryServiceReference;
using UnifreightIIG.Common.DeclarationFilterParamServiceReference;
using UnifreightIIG.Common.GuaranteeCertificateFilterParamServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class TPG_NG_8306_Web07_GuaranteeCertificateRequestService
        : RequestServiceBase<TPG_NG_8306_Web07_GuaranteeCertificateFilterParam, GuaranteeCertificateRequestParams>
    {
        public override TPG_NG_8306_Web07_GuaranteeCertificateFilterParam GetRequest(GuaranteeCertificateRequestParams requestParams)
        {
            var myTPG_NG_8306_Web07_GuaranteeCertificateFilterParam = new TPG_NG_8306_Web07_GuaranteeCertificateFilterParam();
            if (requestParams != null)
            {
                myTPG_NG_8306_Web07_GuaranteeCertificateFilterParam.GuaranteeCertificate = new TPG_NG_8306_Web07_GuaranteeCertificateFilterParamGuaranteeCertificate()
                {
                    certificateID = requestParams.certificateID,
                    certificateIDSpecified = requestParams.certificateID > 0 ? true : false,
                    guaranteeCertificateType = requestParams.guaranteeCertificateType,
                    guaranteeCertificateTypeSpecified = requestParams.guaranteeCertificateType > 0 ? true : false,
                    guaranteeExternalCertificateNumber = requestParams.guaranteeExternalCertificateNumber,
                    guarantorID = requestParams.guarantorID,
                    guarantorIDSpecified = requestParams.guarantorID > 0 ? true : false,
                };
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא לנתוני כתב ערבות";

            return myTPG_NG_8306_Web07_GuaranteeCertificateFilterParam;
        }
    }
}