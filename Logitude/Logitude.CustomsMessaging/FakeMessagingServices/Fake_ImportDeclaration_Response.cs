using Logitude.Customs.Def.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
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
        public ResponseError[] _Constraints;
        public ResponseError[] _Errors;

        public Fake_ImportDeclaration_Response(GenericRequestParams requestParams)
        {
            _requestParams = requestParams;
            DF_MSG10000_ImportDeclarationRequestService reqService = new DF_MSG10000_ImportDeclarationRequestService();
            request = reqService.GetRequest(requestParams);
            fakeRespond = new DF_NG_2754_MSG10004_ImportDeclarationResponse();
            _ResponseHeader = new ResponseHeader();
        }
        public void UpdateDeclaration(GenericRequestParams requestParams)
        {
            fakeRespond.Response = new Response();
            DeclarationDMExtensions _dm = new DeclarationDMExtensions();

            DeclarationDMExtensionsCustomsValueComponent _customsValueComponent = new DeclarationDMExtensionsCustomsValueComponent();
            DeclarationDutyTaxFee[] taxFree = new DeclarationDutyTaxFee[2];
            Declaration declaration = request.Declaration;
            declaration.GoodsShipment[0].GovernmentAgencyGoodsItem = AddGovernmentAgencyGoodsItem(declaration.GoodsShipment[0].GovernmentAgencyGoodsItem);
            _customsValueComponent.TotalDealValueAmountNIS = new TotalDealValueAmountNISType() { Value = 99 };
            _customsValueComponent.CifValueNIS = new CifValueNISType() { Value = 99 };
            _customsValueComponent.TaxAssessedAmount = new DutyTaxFeeAssessed() { Value = 99 };
            _customsValueComponent.TotalMADDealValueAmountNIS = new TotalMADDealValueAmountNISType() { Value = 99 };
            _dm.VersionID = new DeclarationDMExtensionsVersionID() { Value = "0.6" };
            _dm.CustomsValueComponent = _customsValueComponent;
            _dm.TaxationDateTime = XmlConvert.ToString(DateTime.Now);
            _dm.AgentFileReferenceID = declaration.DMExtensions.AgentFileReferenceID;

            //DutyTaxFee
            taxFree[0] = new DeclarationDutyTaxFee
            {
                AdValoremTaxBaseAmount = new DutyTaxFeeAdValoremTaxBaseAmountType() { Value = 99 },
                TypeCode = new DutyTaxFeeTypeCodeType() { Value = "15" },
                DMExtensions = new DeclarationDutyTaxFeeDMExtensions()
            };
            taxFree[0].DMExtensions.CalculatedTax = new DeclarationDutyTaxFeeDMExtensionsCalculatedTax
            {
                Amount = new AmountAmountType() { Value = 99 },
                DeferedTaxAmount = new deferedTaxAmountType() { Value = 0 }
            };
            taxFree[1] = new DeclarationDutyTaxFee
            {
                AdValoremTaxBaseAmount = new DutyTaxFeeAdValoremTaxBaseAmountType() { Value = 0 },
                TypeCode = new DutyTaxFeeTypeCodeType() { Value = "16" },
                DMExtensions = new DeclarationDutyTaxFeeDMExtensions()
            };
            taxFree[1].DMExtensions.CalculatedTax = new DeclarationDutyTaxFeeDMExtensionsCalculatedTax
            {
                Amount = new AmountAmountType() { Value = 0 },
                DeferedTaxAmount = new deferedTaxAmountType() { Value = 0 }
            };

            declaration.DutyTaxFee = taxFree;
            _dm.ExpenseLoadingFactor = new DeclarationDMExtensionsExpenseLoadingFactor() { Value = 99 };
            declaration.DMExtensions = _dm;
            declaration.AcceptanceDateTime = DateTime.Now.ToString();
            if (requestParams.InterfaceTypeCode == "5117")
            {
                declaration.IssueDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            else
            {
                declaration.IssueDateTime = DateTime.Now.ToString();
            }
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
                ApplicationID = 0,
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


        public DeclarationGoodsShipmentGovernmentAgencyGoodsItem[] AddGovernmentAgencyGoodsItem(DeclarationGoodsShipmentGovernmentAgencyGoodsItem[] _governmentAgencyGoodsItem)
        {
            foreach (DeclarationGoodsShipmentGovernmentAgencyGoodsItem item in _governmentAgencyGoodsItem)
            {
                DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFee[] taxFree = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFee[1];
                taxFree[0] = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFee();
                taxFree[0].AdValoremTaxBaseAmount = new DutyTaxFeeAdValoremTaxBaseAmountType
                {
                    currencyID = ISO3AlphaCurrencyCodeContentType.ILS,
                    Value = 99
                };
                taxFree[0].TypeCode = new DutyTaxFeeTypeCodeType
                {
                    Value = "15",
                };
                taxFree[0].TaxRate = 9;
                taxFree[0].DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFeeDMExtensions
                {
                    CalculatedTax = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax
                    {
                        Amount = new AmountAmountType() { Value = 99 },
                        DeferedTaxAmount = new deferedTaxAmountType() { Value = 0 },
                        AlternateRate = new AlternateRateType() { Value = 0 },
                        TotalBtlCoverageNIS = new totalBtlCoverageNISType() { Value = 0 }
                    }
                };
                item.Commodity.DutyTaxFee = taxFree;

            }
            return _governmentAgencyGoodsItem;
        }
        public void AddConstraints()
        {
            _Constraints = new ResponseError[1];
            _Constraints[0] = new ResponseError() { ValidationCode = new ErrorValidationCodeType() { name = "2685-fake contraint", listName = "105" } };
            //DocumentSection
            _Constraints[0].Pointer = new ResponseErrorPointer[4];

            _Constraints[0].Pointer[0] = new ResponseErrorPointer();
            _Constraints[0].Pointer[1] = new ResponseErrorPointer();
            _Constraints[0].Pointer[2] = new ResponseErrorPointer();
            _Constraints[0].Pointer[3] = new ResponseErrorPointer();

            _Constraints[0].Pointer[0].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "42A" };
            _Constraints[0].Pointer[1].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "67A" };
            _Constraints[0].Pointer[2].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "28A" };
            _Constraints[0].Pointer[3].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "30B" };

            //TagId
            _Constraints[0].Pointer[0].TagID = new PointerTagIDType();
            _Constraints[0].Pointer[1].TagID = new PointerTagIDType();
            _Constraints[0].Pointer[2].TagID = new PointerTagIDType();
            _Constraints[0].Pointer[3].TagID = new PointerTagIDType() { Value = "D024" };
            // SequenceNumeric
            _Constraints[0].Pointer[0].SequenceNumeric = 0;
            _Constraints[0].Pointer[1].SequenceNumeric = 0;
            _Constraints[0].Pointer[2].SequenceNumeric = 1;
            //DMExtensions
            _Constraints[0].Pointer[0].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
            _Constraints[0].Pointer[1].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
            _Constraints[0].Pointer[2].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
            _Constraints[0].Pointer[3].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
            _Constraints[0].DMExtensions = new ResponseErrorDMExtensions()
            {
                ConstraintID = 18,
                ConstraintType = 1, //  פרט זיהוי המטען לא קיימים במערכת המכס
                ConstraintStatus = 2,  // 1 = פוטנציאל 

            };
            fakeRespond.Response.Error = _Constraints;
        }
        public void AddErrors(string code,string  tagID , string documentSectionCode)
        {
            _Errors = new ResponseError[1];
            _Errors[0] = new ResponseError() { ValidationCode = new ErrorValidationCodeType() { name = "תשאל את סוהיב", listVersionID = "1", Value = code } };
            _Errors[0].Pointer = new ResponseErrorPointer[1];

            _Errors[0].Pointer[0] = new ResponseErrorPointer();
            //_Errors[0].Pointer[1] = new ResponseErrorPointer();
            //_Errors[0].Pointer[2] = new ResponseErrorPointer();
            //_Errors[0].Pointer[3] = new ResponseErrorPointer();

            if(string.IsNullOrEmpty(documentSectionCode))
            _Errors[0].Pointer[0].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "30B" };
            else
                _Errors[0].Pointer[0].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = documentSectionCode };

            //_Errors[0].Pointer[1].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "67A" };
            //_Errors[0].Pointer[2].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "28A" };
            //_Errors[0].Pointer[3].DocumentSectionCode = new PointerDocumentSectionCodeType() { Value = "30B" };

            //TagId
            if (string.IsNullOrEmpty(tagID))
                _Errors[0].Pointer[0].TagID = new PointerTagIDType() { Value = "D024" };
            else
            _Errors[0].Pointer[0].TagID = new PointerTagIDType() { Value = tagID };

            //_Errors[0].Pointer[1].TagID = new PointerTagIDType();
            //_Errors[0].Pointer[2].TagID = new PointerTagIDType();
            //_Errors[0].Pointer[3].TagID = new PointerTagIDType() { Value = "D024" };
            // SequenceNumeric
            _Errors[0].Pointer[0].SequenceNumeric = 0;
         //   _Errors[0].Pointer[1].SequenceNumeric = 0;
         //   _Errors[0].Pointer[2].SequenceNumeric = 1;
            //DMExtensions
            _Errors[0].Pointer[0].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
       //     _Errors[0].Pointer[1].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
        //    _Errors[0].Pointer[2].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
        //    _Errors[0].Pointer[3].DMExtensions = new ResponseErrorPointerDMExtensions() { NaturalKey = new NaturalKeyType() };
            fakeRespond.Response.Error = _Errors;

        }
        public void CastDeclaration()
        {
            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(fakeRespond.Response.Declaration.GetType());
                serializer.Serialize(stringwriter, fakeRespond.Response.Declaration);
                DeclarationString = stringwriter.ToString();
            }


        }
    }
}
