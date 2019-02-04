using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ExportDeclarationDataRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_9071_ExportDeclarationDataResponseService : ResponseServiceBase<ExportDeclarationDataResponseData, DF_MSG_9071_ExportDeclarationDataResponse, ExportDeclarationDataRequestParams>
    {
        public override ExportDeclarationDataResponseData GetResponse(DF_MSG_9071_ExportDeclarationDataResponse customResponse, ExportDeclarationDataRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DF_MSG_9071_ExportDeclarationDataResponse customResponse, ExportDeclarationDataRequestParams requestParams)
        {
            //Analayze Message 9071- Export Declaration Data
            List<InternalCargoResult> internalCargoResultList = new List<InternalCargoResult>();

            this.MyResponseData = new ExportDeclarationDataResponseData();
            this.MyResponseData.Succeeded = true;
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }
            if (customResponse.Exception != null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.Exception.ExeptionDescription;
                return;
            }

            if (customResponse.Declaration != null)
            {
                this.MyResponseData.ReshimonNumber = customResponse.Declaration.ReshimonNumber;
                this.MyResponseData.Title = customResponse.Declaration.Title;
                if (customResponse.Declaration.LoadingDate != null && customResponse.Declaration.LoadingDateSpecified == true)
                {
                    this.MyResponseData.LoadingDate = customResponse.Declaration.LoadingDate.Value.ToString("dd/MM/yyyy");
                }
                this.MyResponseData.CalculationDate = customResponse.Declaration.CalculationDate.Date.ToString("dd/MM/yyyy");
                this.MyResponseData.AgentCustomerExternalID = customResponse.Declaration.Agent_CustomerExternalIDNum.ToString();
                if (customResponse.Declaration.FOBNetoNISAmount != null && customResponse.Declaration.FOBNetoNISAmountSpecified == true)
                {
                    this.MyResponseData.FOBNetoNISAmount = String.Format("{0:N2}", customResponse.Declaration.FOBNetoNISAmount);
                }
                if (customResponse.Declaration.FOBNISAmount != null && customResponse.Declaration.FOBNISAmountSpecified == true)
                {
                    this.MyResponseData.FOBNISAmount = String.Format("{0:N2}", customResponse.Declaration.FOBNISAmount);
                }

                if(customResponse.Declaration.Invoice != null)
                {
                    this.MyResponseData.InvoiceList = new List<Invoice>();
                    foreach (var invoiceItem in customResponse.Declaration.Invoice)
                    {
                        Invoice invoiceResult = new Invoice();
                        invoiceResult.SequenceNumber = invoiceItem.SequenceNumber.ToString();
                        invoiceResult.ExternalID = invoiceItem.ExternalIDNum;
                        invoiceResult.InvoiceAmount = invoiceItem.InvoiceAmount.ToString();
                        invoiceResult.InvoiceAmountCurrency = invoiceItem.InvoiceAmount.ToString();
                        if (invoiceItem.InvoiceCurrencyTypeID != null /*&& invoiceItem.InvoiceCurrencyTypeIDSpecified == true*/)
                        {
                            invoiceResult.InvoiceCurrency = invoiceItem.InvoiceCurrencyTypeID.ToString();
                            invoiceResult.InvoiceAmountCurrency += " (" + invoiceItem.InvoiceCurrencyTypeID + ")";
                        }
                        this.MyResponseData.InvoiceList.Add(invoiceResult);

                        //GoodsItem
                        if (invoiceItem.GoodsItem != null)
                        {
                            if (this.MyResponseData.RequestList == null)
                            {
                                this.MyResponseData.RequestList = new List<Request>();
                            }
                            foreach (var requestItem in invoiceItem.GoodsItem)
                            {
                                Request requestResult = new Request();
                                requestResult.SequenceNumber = requestItem.SequenceNumber.ToString();
                                requestResult.CustomsItem = requestItem.CustomsItem;
                                if (requestItem.ValueQuantity != null && requestItem.ValueQuantitySpecified == true)
                                {
                                    requestResult.ValueQuantity = requestItem.ValueQuantity.ToString();
                                }
                                if (requestItem.OriginCountry != null && requestItem.OriginCountrySpecified == true)
                                {
                                    requestResult.OriginCountry = requestItem.OriginCountry.ToString();
                                }
                                requestResult.ForeignAmount = requestItem.ForeignCurrencyAmount.ToString();
                                requestResult.ForeignCurrency = requestItem.CurrencyTypeID.ToString();
                                requestResult.ForeignCurrencyAmount = requestItem.ForeignCurrencyAmount.ToString();
                                int i = 0;
                                int.TryParse(requestItem.CurrencyTypeID, out i);
                                if (/*requestItem.CurrencyTypeID*/ i > 0)
                                {
                                    requestResult.ForeignCurrencyAmount += " (" + requestItem.CurrencyTypeID + ")";
                                }

                                //GovernmentProcedure
                                requestResult.GovernmentProcedureList = new List<GovernmentProcedure>();
                                if (requestItem.GoodsItemGovernmentProcedure != null)
                                {
                                    foreach (var governmentProcedureItem in requestItem.GoodsItemGovernmentProcedure)
                                    {
                                        if (governmentProcedureItem.ItemGovernmentProcedureType != null /*&& governmentProcedureItem.ItemGovernmentProcedureTypeSpecified*/)
                                        {
                                            GovernmentProcedure governmentProcedureResult = new GovernmentProcedure();
                                            governmentProcedureResult.ItemGovernmentProcedureType = governmentProcedureItem.ItemGovernmentProcedureType.ToString();
                                            if(!string.IsNullOrEmpty(governmentProcedureResult.ItemGovernmentProcedureType)) // Table 1422
                                            {
                                                ItemGovernmentProcedureTypeQueryService itemGovernmentProcedureTypeQueryService = new ItemGovernmentProcedureTypeQueryService(requestParams.Tenant);
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
                                if (requestItem.VehicleForGoodsItem != null)
                                {
                                    foreach (var vehicleItem in requestItem.VehicleForGoodsItem)
                                    {
                                        Vehicle vehicleResult = new Vehicle();
                                        if (vehicleItem.CargoIdentityQualifierID != null && vehicleItem.CargoIdentityQualifierIDSpecified)
                                        {
                                            vehicleResult.CargoIdentityQualifierID = vehicleItem.CargoIdentityQualifierID.ToString();
                                        }
                                        vehicleResult.RichbitNumber = vehicleItem.RichbitNumber;
                                        vehicleResult.VehicleExternalIDNum = vehicleItem.VehicleExternalIDNum;
                                        requestResult.VehicleList.Add(vehicleResult);
                                    }
                                }

                                this.MyResponseData.RequestList.Add(requestResult);
                            }
                        }
                    }
                }

                    
            }

            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "ניתוח בוצע בהצלחה";
            
            return;
        }

        //private string GetCurrencyTypeName(int currencyTypeCode)
        //{
        //    string messageTypeName = null;
        //    if (messageType >= 0)
        //    {
        //        ContinuousMessagesTypeCodeQueryService continuousMessagesTypeCodeQueryService = new ContinuousMessagesTypeCodeQueryService(this._MyClaimPM.Tenant);
        //        ContinuousMessagesTypeCodePM continuousMessagesTypeCodePM = continuousMessagesTypeCodeQueryService.GetSingle(messageType.ToString(), false, true);
        //        if (continuousMessagesTypeCodePM != null)
        //        {
        //            messageTypeName = continuousMessagesTypeCodePM.LocalName;
        //        }
        //    }
        //    return messageTypeName;
        //}
    }
}
