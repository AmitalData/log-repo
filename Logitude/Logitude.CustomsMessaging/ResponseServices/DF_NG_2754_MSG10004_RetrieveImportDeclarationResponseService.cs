
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnifreightIIG.Common.RetrieveImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2754_MSG10004_RetrieveImportDeclarationResponseService
        : ResponseServiceBase<DeclarationRestoreResponseData, DF_NG_2754_MSG10004_ImportDeclarationResponse, DeclarationRestoreRequestParams>
    {
        private DF_NG_2754_MSG10004_ImportDeclarationResponseService _DF_NG_2754_MSG10004_ImportDeclarationResponseService;
        private DeclarationRestoreResponseData _MyDefaultResponseData;
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption { get; set; }

        public override void Update(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, DeclarationRestoreRequestParams requestParams)
        {
            if (requestParams.RequestName == "Restore From ResetDeclaration")
            {
				this.MyResponseData = new DeclarationRestoreResponseData();
                string declarationStatus = customResponse?.Response?.Status?.NameCode.Value;

                if (string.IsNullOrEmpty(declarationStatus) || declarationStatus == "12" || declarationStatus == "13")
                {
					this.MyResponseData.Succeeded = true;
					this.MyResponseData.UserMessage = "ניתן לבצע איפוס הצהרה";	
				}
                else
                {
					this.MyResponseData.Succeeded = false;
					this.MyResponseData.UserMessage = "נראה שהתיק שולם יש לבצע שחזור הצהרה";
				}
				return;
			}
            if (requestParams.ShowData)
            {
                GetDeclarationDataRespons(customResponse, requestParams.Tenant);
                return;

            }

            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response == null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                _MyDefaultResponseData = new DeclarationRestoreResponseData()
                {
                    ApplicationID = customResponse.ResponseContentHeader.ApplicationID.ToString(),
                    Succeeded = true,
                    UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription,
                    HasException = false,
                    ResponseStatusXML = GetDummyXml(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription+customResponse.ResponseContentHeader.Remark,requestParams.IsAngularClient),
                };
                return;
            }
            //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
            //[XmlType(AnonymousType = true, Namespace = "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration")]
            UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse ser = null;
            var xml = XmlGenericUtil<DF_NG_2754_MSG10004_ImportDeclarationResponse>.SerializeObject(customResponse);
            ser = XmlGenericUtil<UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse>.DeSerializeObject(xml);
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService = new DF_NG_2754_MSG10004_ImportDeclarationResponseService();
            //ITZIK+MIRT  _DF_NG_2754_MSG10004_ImportDeclarationResponseService._ResponseHeaderExeption = _ResponseHeaderExeption;
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService._IsRetrieveDeclarationResponse = true;
            _DF_NG_2754_MSG10004_ImportDeclarationResponseService.Update(ser, requestParams);

            //<--- Yuval Chalup 13.05.2015 TASK-11915
            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response != null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                _MyDefaultResponseData = new DeclarationRestoreResponseData();
                if (customResponse.Response != null)
                {
                    if (requestParams.AppicationId != null && requestParams.DeclarationId == null) requestParams.DeclarationId = requestParams.AppicationId; // moran 10.1.16 Task 19724 + add to condition --><--
                    if (customResponse.Response.Declaration != null && _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.HasException != true)
                    {
                        xml = XmlGenericUtil<Declaration>.SerializeObject(customResponse.Response.Declaration);
                        _MyDefaultResponseData.ResponseStatusXML = xml;
                        if (requestParams.IsAngularClient)
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(xml);

                            _MyDefaultResponseData.ResponseStatusXML = //JsonConvert.SerializeObject(customResponse.Response.Declaration);
                                JsonConvert.SerializeXmlNode(doc);
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage))
                        {
                            _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.HasException = false; // moran 10.1.16 Task 19724
                            _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage,requestParams.IsAngularClient);
                        }
                    }
                }
            }
            //Yuval Chalup 13.05.2015 TASK-11915 --->
        }
    

        public override DeclarationRestoreResponseData GetResponse(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, DeclarationRestoreRequestParams requestParams)
        {
            if (_DF_NG_2754_MSG10004_ImportDeclarationResponseService == null || _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData == null)
            {
                return _MyDefaultResponseData;
            }
            var response = new DeclarationRestoreResponseData()
            {
                ApplicationID = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.ApplicationID,
                UserMessage = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage,
                HasException = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.HasException,
                Succeeded = _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.Succeeded,
            };

            if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage) && _DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage.Contains("נתוני ההצהרה לא עודכנו"))
            {
                response.IsShowUserMessage = true;
            }

            //<--- Yuval Chalup 13.05.2015 TASK-11915
            if (customResponse.ResponseContentHeader != null &&
                customResponse.Response != null &&
                customResponse.AddAGlobalScannedAttachmentToEntityResponse == null &&
                customResponse.CollateralRequestDetails == null)
            {
                if (this._MyDefaultResponseData != null)
                {
                    response.ResponseStatusXML = this._MyDefaultResponseData.ResponseStatusXML;
                    return response;
                }

                if (customResponse.Response != null)
                {
                    if (customResponse.Response.Declaration != null)
                    {
                        if (requestParams.IsAngularClient)
                        {
                            _MyDefaultResponseData.ResponseStatusXML = JsonConvert.SerializeObject(customResponse.Response.Declaration);
                        }
                        else
                        {
                            string xml = XmlGenericUtil<Declaration>.SerializeObject(customResponse.Response.Declaration);
                            _MyDefaultResponseData.ResponseStatusXML = xml;
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage))
                        {
                            _MyDefaultResponseData.ResponseStatusXML = GetDummyXml(_DF_NG_2754_MSG10004_ImportDeclarationResponseService.MyResponseData.UserMessage, requestParams.IsAngularClient);
                        }
                    }
                }
            }
            //Yuval Chalup 13.05.2015 TASK-11915 --->

            return response;
        }

        public void GetDeclarationDataRespons(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse, int Tenant)
        {

            this.MyResponseData = new DeclarationRestoreResponseData();
            this.MyResponseData.exportDeclarationDataResponseData = new ExportDeclarationDataResponseData();
            this.MyResponseData.Succeeded = true;
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }


            if (customResponse.Response.Declaration != null)
            {
                if (this.MyRequestSheetParam == null)
                    this.MyRequestSheetParam = new RequestSheetParam();

                this.MyRequestSheetParam.CustomFileNo = GetValueIDType(customResponse.Response.Declaration.DMExtensions?.AgentFileReferenceID);
                this.MyResponseData.exportDeclarationDataResponseData.Title = customResponse.Response.Declaration.ID.Value;
       
                this.MyResponseData.exportDeclarationDataResponseData.CalculationDate = Convert.ToDateTime(customResponse.Response.Declaration.IssueDateTime).ToString("dd/MM/yyyy");
                if (customResponse.Response.Declaration.Agent != null && customResponse.Response.Declaration.Agent.Count() > 0)
                {
                    var agent = customResponse.Response.Declaration?.Agent.FirstOrDefault();
                    if (agent != null)
                    {
                        this.MyResponseData.exportDeclarationDataResponseData.AgentCustomerExternalID = GetValueIDType(agent.ID);
                    }
                }



                if (customResponse.Response.Declaration.GoodsShipment != null)
                {
                    this.MyResponseData.exportDeclarationDataResponseData.InvoiceList = new List<Invoice>();
                    foreach (var invoiceItem in customResponse.Response.Declaration.GoodsShipment.OrderBy(x => x.SequenceNumeric))
                    {
                        Invoice invoiceResult = new Invoice();
                        invoiceResult.InvoiceId = GetValueIDType(invoiceItem.Invoice.ID) + invoiceItem.SequenceNumeric.ToString();
                        invoiceResult.SequenceNumber = invoiceItem.SequenceNumeric.ToString();
                        invoiceResult.ExternalID = GetValueIDType(invoiceItem.Invoice.ID);
                        invoiceResult.InvoiceAmount = GetValueAmountType(invoiceItem.Invoice.DMExtensions.InvoiceAmount).ToString();
                        invoiceResult.InvoiceAmountCurrency = GetValueAmountType(invoiceItem.Invoice.DMExtensions.InvoiceAmount).ToString(); ;
                     
                        invoiceResult.InvoiceCurrency = invoiceItem.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                        invoiceResult.InvoiceAmountCurrency += " (" + invoiceResult.InvoiceAmountCurrency + ")";
                      
                        this.MyResponseData.exportDeclarationDataResponseData.InvoiceList.Add(invoiceResult);

                        //GoodsItem
                        if (invoiceItem.GovernmentAgencyGoodsItem != null)
                        {
                            if (this.MyResponseData.exportDeclarationDataResponseData.RequestList == null)
                            {
                                this.MyResponseData.exportDeclarationDataResponseData.RequestList = new List<Request>();
                            }
                            foreach (var governmentAgencyGoodsItem in invoiceItem.GovernmentAgencyGoodsItem)
                            {
                                Request requestResult = new Request();
                                requestResult.InvoiceId = GetValueIDType(invoiceItem.Invoice.ID) + invoiceItem.SequenceNumeric.ToString();

                                requestResult.SequenceNumber = governmentAgencyGoodsItem.SequenceNumeric.ToString();
                                var classification2 = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "HS");

                                if (classification2 != null)
                                {
                                    requestResult.CustomsItem = GetValueIDType(classification2.ID);//.Replace("/", "");
                                }


                                foreach (var goodsMeasure in governmentAgencyGoodsItem.GoodsMeasure)
                                {
                                    if (goodsMeasure.DMExtensions != null && goodsMeasure.TariffQuantity != null)
                                    {
                                        switch (goodsMeasure.DMExtensions.MeasureQualifier.Value)
                                        {
                                            
                                            case "2":
                                                {
                                                   
                                                    requestResult.ValueQuantity = goodsMeasure.TariffQuantity.Value.ToString();
                                                    break;
                                                }
                                               
                                        }
                                    }
                                }


                                if (governmentAgencyGoodsItem.Origin != null)
                                {
                                    requestResult.OriginCountry = GetValueCodeType(governmentAgencyGoodsItem.Origin.CountryCode).ToString();
                                }

                                if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
                                {


                                    var cur = customResponse.Response.Declaration.GoodsShipment[0].Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                                    var GoodsItemAmount = governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount.Where(x => x.AmountType.Value == "1").FirstOrDefault();
                                    if (GoodsItemAmount != null) {
                                       requestResult.ForeignAmount = GetValueAmountType(GoodsItemAmount.CustomsValueAmount).ToString();
                                       requestResult.ForeignCurrency = GoodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                       requestResult.ForeignCurrencyAmount = GetValueAmountType(GoodsItemAmount.CustomsValueAmount).ToString(); ;
                                       int i = 0;
                                       int.TryParse(GetValueCodeType(GoodsItemAmount.AmountType), out i);
                                       if (/*requestItem.CurrencyTypeID*/ i > 0)
                                       {
                                           requestResult.ForeignCurrencyAmount += " (" + GoodsItemAmount.CustomsValueAmount.currencyID.ToString() + ")";
                                       }
                                    }
                                }
                                //GovernmentProcedure
                                requestResult.GovernmentProcedureList = new List<GovernmentProcedure>();
                                if (governmentAgencyGoodsItem.Commodity.GovernmentProcedure != null)
                                {
                                    foreach (var governmentProcedureItem in governmentAgencyGoodsItem.Commodity.GovernmentProcedure)
                                    {
                                        if (governmentProcedureItem.CurrentCode != null )
                                        {
                                            GovernmentProcedure governmentProcedureResult = new GovernmentProcedure();
                                            governmentProcedureResult.ItemGovernmentProcedureType = governmentProcedureItem.CurrentCode.Value.ToString();
                                            if (!string.IsNullOrEmpty(governmentProcedureResult.ItemGovernmentProcedureType)) // Table 1422
                                            {
                                                ItemGovernmentProcedureTypeQueryService itemGovernmentProcedureTypeQueryService = new ItemGovernmentProcedureTypeQueryService(Tenant);
                                                ItemGovernmentProcedureTypePM itemGovernmentProcedureTypePM = itemGovernmentProcedureTypeQueryService.GetSingle(governmentProcedureResult.ItemGovernmentProcedureType, false, true);
                                                if (itemGovernmentProcedureTypePM != null)
                                                {
                                                    governmentProcedureResult.ItemGovernmentProcedureName = itemGovernmentProcedureTypePM.LocalName;
                                                }
                                            }
                                            requestResult.GovernmentProcedureList.Add(governmentProcedureResult);
                                        }
                                    }
                                }

                                //Vehicle
                                requestResult.VehicleList = new List<Vehicle>();
                                if (governmentAgencyGoodsItem.DMExtensions.Vehicle != null)
                                {
                                    foreach (var vehicleItem in governmentAgencyGoodsItem.DMExtensions.Vehicle)
                                    {
                                        Vehicle vehicleResult = new Vehicle();
                                      
                                        vehicleResult.CargoIdentityQualifierID = vehicleItem.IDTypeCode.Value.ToString();
                                       
                                        vehicleResult.RichbitNumber = vehicleItem.ID.Value;
                                        requestResult.VehicleList.Add(vehicleResult);
                                    }
                                }

                                this.MyResponseData.exportDeclarationDataResponseData.RequestList.Add(requestResult);
                            }
                        }
                    }
                }


            }

            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "ניתוח בוצע בהצלחה";

            return;
        }

        private string GetDummyXml(string message,bool toJson)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            if (toJson)
            {
                return JsonConvert.SerializeObject(myDummyXml);
            }
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }
        #region Helpers
        private string GetValueIDType<T>(T codeType) where T : IDType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }

        private string GetValueCodeType<T>(T codeType) where T : CodeType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }

        private string GetValueTextType<T>(T codeType) where T : TextType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }


        private decimal GetValueAmountType<T>(T codeType) where T : AmountType
        {
            if (codeType != null)
                return codeType.Value;
            return 0;
        }

        #endregion
        public class GeneralMessage
        {
            public string Message { get; set; }
        }
    }
}
