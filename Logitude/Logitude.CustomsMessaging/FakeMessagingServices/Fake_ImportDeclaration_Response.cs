using Logitude.Customs.Def.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using Exception = UnifreightIIG.Common.ImportDeclarationServiceReference.Exception;


namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_ImportDeclaration_Response
    {
        private GenericRequestParams _requestParams;
        private readonly DF_MSG10000_ImportDeclaration request;
        public DF_NG_2754_MSG10004_ImportDeclarationResponse fakeRespond;
        public ResponseHeader _ResponseHeader;
        public Fake_ImportDeclaration_Response(GenericRequestParams requestParams)
        {
            _requestParams = requestParams;
            DF_MSG10000_ImportDeclarationRequestService reqService = new DF_MSG10000_ImportDeclarationRequestService();
            request = reqService.GetRequest(requestParams);
            fakeRespond = new DF_NG_2754_MSG10004_ImportDeclarationResponse();
            _ResponseHeader = new ResponseHeader();
        }
        public void UpdateDeclaration()
        {
            fakeRespond.Response = new Response();
            DeclarationDMExtensions _dm = new DeclarationDMExtensions();

            DeclarationDMExtensionsCustomsValueComponent _customsValueComponent = new DeclarationDMExtensionsCustomsValueComponent();
            DeclarationDutyTaxFee[] taxFree = new DeclarationDutyTaxFee[2];
            Declaration declaration = request.Declaration;

            _customsValueComponent.TotalDealValueAmountNIS = new TotalDealValueAmountNISType() { Value = 273 };
            _customsValueComponent.CifValueNIS = new CifValueNISType() { Value = 273 };
            _customsValueComponent.TaxAssessedAmount = new DutyTaxFeeAssessed() { Value = 80 };
            _customsValueComponent.TotalMADDealValueAmountNIS = new TotalMADDealValueAmountNISType() { Value = 263 };
            _dm.VersionID = new DeclarationDMExtensionsVersionID() { Value = "0.6" };
            _dm.CustomsValueComponent = _customsValueComponent;
            _dm.TaxationDateTime = XmlConvert.ToString(DateTime.Now);

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
            _dm.ExpenseLoadingFactor = new DeclarationDMExtensionsExpenseLoadingFactor() { Value = 99 };
            declaration.DMExtensions = _dm;
            declaration.AcceptanceDateTime = DateTime.Now.ToString();
            declaration.IssueDateTime = DateTime.Now.ToString();
            declaration.TypeCode = new DeclarationTypeCodeType();
            fakeRespond.Response.Declaration = declaration;
        }
        public void UpdateStatus(string code)
        {
            ResponseStatus status = new ResponseStatus
            {
                NameCode = new StatusNameCodeType() { Value = code },
                EffectiveDateTime = DateTime.Now.ToString()
            };
            this.fakeRespond.Response.Status = status;
        }
        public void UpdateFakeResponseContentHeader()
        {
            Exception[] exception = new Exception[1];
            exception[0] = new Exception // שגיאות
            {
                ExeptionDescription = "testing"
            };
            this.fakeRespond.ResponseContentHeader = new ResponseContentHeader
            {
                TransmitionDateTime = DateTime.Now,
                Remark = "",
                Exception = null,
            };
        }
        public void AddSign()
        {
            RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName = "CN=Fake  SignByX509SubjectName ID_031561053, SN=Azbarga, G=Sana, SERIALNUMBER=01-031561053, O=05-511262073, OU=Amital Data, T=Manager, C=IL";
        }
        public void AddResponseHeader()
        {

            _ResponseHeader.CorrelationId = Guid.NewGuid().ToString();
            _ResponseHeader.ExternalId = Guid.NewGuid().ToString();
            _ResponseHeader.Status = "Success";
            _ResponseHeader.ErrorDescription = "";
            _ResponseHeader.ErrorCode = "None";
           
        }

    }
}
