using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using Exception = UnifreightIIG.Common.ImportDeclarationServiceReference.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake1DF_NG_2754_MSG10004_ImportDeclarationResponse :FakeResponseBase
    {
        public ResponseHeader CallWS(
            DF_MSG10000_ImportDeclaration customRequest,
            GenericRequestParams requestParams,
            out string exceptionMessage, out DF_NG_2754_MSG10004_ImportDeclarationResponse response )
        {
            response = new DF_NG_2754_MSG10004_ImportDeclarationResponse();
            ResponseHeader _ResponseHeader = new ResponseHeader();
            Exception[] exception = new Exception[1];
            exception[0] = new Exception
            {
                ExeptionDescription = "testing"
            };
            response.Response = new Response();
            DeclarationDMExtensions _dm = new DeclarationDMExtensions();
            ResponseStatus status = new ResponseStatus();
            DeclarationDMExtensionsCustomsValueComponent _customsValueComponent = new DeclarationDMExtensionsCustomsValueComponent();
            DeclarationDutyTaxFee[] taxFree = new DeclarationDutyTaxFee[2];
            Declaration declaration = customRequest.Declaration;
            //status
            status.NameCode = new StatusNameCodeType() { Value = "13" };             // 12 => טיוטה שגויה
            status.EffectiveDateTime = DateTime.Now.ToString();
           
            _customsValueComponent.TotalDealValueAmountNIS = new TotalDealValueAmountNISType() { Value = 273 };
            _customsValueComponent.CifValueNIS = new CifValueNISType() { Value = 273 };
            _customsValueComponent.TaxAssessedAmount = new DutyTaxFeeAssessed() { Value = 80 };
            _customsValueComponent.TotalMADDealValueAmountNIS = new TotalMADDealValueAmountNISType() { Value = 263 };
            _dm.VersionID = new DeclarationDMExtensionsVersionID() { Value = "0.6" };
            _dm.CustomsValueComponent = _customsValueComponent;
            _dm.TaxationDateTime = DateTime.Now.ToString();

            
            //DutyTaxFee
            taxFree[0] = new DeclarationDutyTaxFee
            {
                AdValoremTaxBaseAmount = new DutyTaxFeeAdValoremTaxBaseAmountType() { Value = 273 },
                TypeCode = new DutyTaxFeeTypeCodeType() { Value = "15" },
                DMExtensions = new DeclarationDutyTaxFeeDMExtensions()
            };
            taxFree[0].DMExtensions.CalculatedTax = new DeclarationDutyTaxFeeDMExtensionsCalculatedTax
            {
                Amount = new AmountAmountType() { Value = 29 },
                DeferedTaxAmount = new deferedTaxAmountType() { Value = 0 }
            };
            taxFree[1] = new DeclarationDutyTaxFee
            {
                AdValoremTaxBaseAmount = new DutyTaxFeeAdValoremTaxBaseAmountType() { Value = 51 },
                TypeCode = new DutyTaxFeeTypeCodeType() { Value = "16" },
                DMExtensions = new DeclarationDutyTaxFeeDMExtensions()
            };
            taxFree[1].DMExtensions.CalculatedTax = new DeclarationDutyTaxFeeDMExtensionsCalculatedTax
            {
                Amount = new AmountAmountType() { Value = 51 },
                DeferedTaxAmount = new deferedTaxAmountType() { Value = 0 }
            };

            declaration.DutyTaxFee = taxFree;
            _dm.ExpenseLoadingFactor = new DeclarationDMExtensionsExpenseLoadingFactor() { Value=99};
            declaration.DMExtensions = _dm;
            declaration.AcceptanceDateTime = DateTime.Now.ToString();
            declaration.DeclarationOfficeID = new DeclarationDeclarationOfficeIDType();
            declaration.DeclarationOfficeID = customRequest.Declaration.DeclarationOfficeID;
            declaration.ID = new DeclarationIdentificationIDType();
            declaration.ID= customRequest.Declaration.ID;
            declaration.IssueDateTime = DateTime.Now.ToString();
            declaration.TypeCode = new DeclarationTypeCodeType();
           
            response.Response.Declaration = declaration;
            response.Response.Status = status;
            response.ResponseContentHeader = new ResponseContentHeader
            {
                TransmitionDateTime = DateTime.Now,
                Remark = "",
                Exception = null,
            };
            RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName = "CN=Fake  SignByX509SubjectName ID_031561053, SN=Azbarga, G=Sana, SERIALNUMBER=01-031561053, O=05-511262073, OU=Amital Data, T=Manager, C=IL";
            exceptionMessage =null;
            _ResponseHeader.CorrelationId = Guid.NewGuid().ToString();
            _ResponseHeader.ExternalId = Guid.NewGuid().ToString();
            _ResponseHeader.Status = "Success";
            _ResponseHeader.ErrorDescription = "";
            _ResponseHeader.ErrorCode = "None";
            return _ResponseHeader;

        }

    }
}
