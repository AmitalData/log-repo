using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.WarehouseBlockBalanceServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_8329_Web02_WarehouseBlockBalanceDetailResponseService : ResponseServiceBase<
        ST_8328_Web01_WarehouseBlockBalanceResponseData,
        ST_8329_Web02_WarehouseBlockBalanceDetail,
        ST_8328_Web01_WarehouseBlockBalanceRequestParams>
    {

        public override void Update(ST_8329_Web02_WarehouseBlockBalanceDetail customResponse, ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new ST_8328_Web01_WarehouseBlockBalanceResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
                return;
            }

            this.MyResponseData = new ST_8328_Web01_WarehouseBlockBalanceResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            //Get Block General Details
            this.MyResponseData.SiteNumber = customResponse.BlockDetails.site;
            this.MyResponseData.SiteText = customResponse.BlockDetails.siteText;
            this.MyResponseData.WarehouseBlockNumber = customResponse.BlockDetails.WarehouseBlockNumber.ToString();
            this.MyResponseData.DeclarationNumber = customResponse.BlockDetails.DeclarationNumber;
            this.MyResponseData.ImporterNumber = customResponse.BlockDetails.ImporterNumber.ToString();
            this.MyResponseData.ImporterTitle = customResponse.BlockDetails.ImporterTitle;
            this.MyResponseData.OpeningDate = String.Format("{0:g}", customResponse.BlockDetails.OpeningDate);
            this.MyResponseData.OriginalOpeningDate = String.Format("{0:g}", customResponse.BlockDetails.OriginalOpeningDate);
            this.MyResponseData.MaxStorageDate = String.Format("{0:g}", customResponse.BlockDetails.MaxStorageDate);
            this.MyResponseData.BlockClosureDate = String.Format("{0:g}", customResponse.BlockDetails.BlockClosureDate);
            this.MyResponseData.LogicalPackagesQuantityBalance = customResponse.BlockDetails.LogicalPackagesQuantityBalance.ToString("N");
            this.MyResponseData.PhysicalPackagesQuantityBalance = customResponse.BlockDetails.PhysicalPackagesQuantityBalance.ToString("N2"); ;
            this.MyResponseData.Value = string.Format("{0:N2}", customResponse.BlockDetails.Value);
            this.MyResponseData.StorageEntryPortChargeBalance = string.Format("{0:N2}", customResponse.BlockDetails.StorageEntryPortChargeBalance.Value);
            if (!string.IsNullOrEmpty(customResponse.BlockDetails.StorageEntryPortChargeCurrencyType))
            {
                this.MyResponseData.StorageEntryPortChargeBalance = string.Concat(this.MyResponseData.StorageEntryPortChargeBalance, " (", customResponse.BlockDetails.StorageEntryPortChargeCurrencyType, ")");
            }
            this.MyResponseData.StorageEntryPortChargeCurrencyType = customResponse.BlockDetails.StorageEntryPortChargeCurrencyType;
            this.MyResponseData.StorageEntryTransportBalance = string.Format("{0:N2}", customResponse.BlockDetails.StorageEntryTransportBalance.Value);
            if (!string.IsNullOrEmpty(customResponse.BlockDetails.StorageEntryTransportCurrencyType))
            {
                this.MyResponseData.StorageEntryTransportBalance = string.Concat(this.MyResponseData.StorageEntryTransportBalance, " (", customResponse.BlockDetails.StorageEntryTransportCurrencyType, ")");
            }
            this.MyResponseData.StorageEntryTransportCurrencyType = customResponse.BlockDetails.StorageEntryTransportCurrencyType;
            this.MyResponseData.StorageEntryInsuranceBalance = string.Format("{0:N2}", customResponse.BlockDetails.StorageEntryInsuranceBalance.Value);
            if (!string.IsNullOrEmpty(customResponse.BlockDetails.StorageEntryInsuranceCurrencyType))
            {
                this.MyResponseData.StorageEntryInsuranceBalance = string.Concat(this.MyResponseData.StorageEntryInsuranceBalance, " (", customResponse.BlockDetails.StorageEntryInsuranceCurrencyType, ")");
            }
            this.MyResponseData.StorageEntryInsuranceCurrencyType = customResponse.BlockDetails.StorageEntryInsuranceCurrencyType;
            //Get Block Special Activities
            if (customResponse.BlockDetails.BlockSpecialActivities != null)
            {
                List<BlockSpecialActivities> blockSpecialActivitiesList = new List<BlockSpecialActivities>();
                foreach (var SpecialActivitiy in customResponse.BlockDetails.BlockSpecialActivities)
                {
                    if (!string.IsNullOrWhiteSpace(SpecialActivitiy))
                    {
                        BlockSpecialActivities blockSpecialActivitiy = new BlockSpecialActivities();
                        blockSpecialActivitiy.SpecialActivityTypeName = SpecialActivitiy;
                        blockSpecialActivitiesList.Add(blockSpecialActivitiy);
                    }
                }
                if (blockSpecialActivitiesList.Count > 0)
                {
                    this.MyResponseData.BlockSpecialActivitiesList = blockSpecialActivitiesList;
                }
            }

            //Get Block Logical Activities
            if (customResponse.ActionList != null)
            {
                List<ActionActivities> actionList = new List<ActionActivities>();
                foreach (var actionItem in customResponse.ActionList)
                {
                    ActionActivities actionActivitiy = new ActionActivities();
                    actionActivitiy.ActionDate = actionItem.ActionDate.Date.ToString("dd/MM/yyyy");
                    actionActivitiy.ActionPackagesQuantity = actionItem.ActionPackagesQuantity.ToString("N");
                    actionActivitiy.PackagesQuantityAfterAction = actionItem.PackagesQuantityAfterAction.ToString("N2");
                    actionActivitiy.ActionValue = String.Format("{0:N2}", actionItem.ActionValue);
                    actionActivitiy.ValueAfterAction = String.Format("{0:N2}", actionItem.ValueAfterAction);
                    actionActivitiy.GovernmentProcedureType = actionItem.GovernmentProcedureType;
                    actionActivitiy.GovernmentProcedureTypeText = actionItem.GovernmentProcedureTypeText;
                    actionActivitiy.DeclarationNumber = actionItem.DeclarationNumber;

                    actionList.Add(actionActivitiy);
                }
                this.MyResponseData.ActionList = actionList;
            }

            //Get Block Storage Activities
            if (customResponse.StorageActionList != null)
            {
                List<StorageAction> storageActivitiesList = new List<StorageAction>();
                foreach (var storageAction in customResponse.StorageActionList)
                {
                    StorageAction storageActivitiy = new StorageAction();
                    storageActivitiy.StorageActionDate = storageAction.StorageActionDate.Date.ToString("dd/MM/yyyy");
                    storageActivitiy.StorageActionType = storageAction.StorageActionType;
                    storageActivitiy.StorageActionPackagesQuantity = storageAction.StorageActionPackagesQuantity.ToString("N");
                    storageActivitiy.PackagesQuantityAfterStorageAction = storageAction.PackagesQuantityAfterStorageAction.ToString("N2");
                    storageActivitiy.StorageReferenceType = storageAction.StorageReferenceType;
                    storageActivitiy.CargoMovementReference = storageAction.CargoMovementReference;
                    
                    storageActivitiy.PackingDetailsList = new List<PackingDetails>();
                    if (storageAction.PackingDetailsList != null)//Eitan H 5/2/18 CALL 302799
                    {
                        foreach (var packingItem in storageAction.PackingDetailsList)
                        {
                            PackingDetails packingDetails = new PackingDetails();
                            packingDetails.PackingType = packingItem.PackingType;
                            packingDetails.PackingTypeText = packingItem.PackingTypeText;
                            packingDetails.StorageActionPackagesQuantity = packingItem.StorageActionPackagesQuantity.ToString("N2");
                            packingDetails.PackagesQuantityAfterStorageAction = packingItem.PackagesQuantityAfterStorageAction.ToString("N2");
                            packingDetails.StorageActionPackagesWeight = packingItem.StorageActionPackagesWeight.ToString();
                            packingDetails.PackagesWeightAfterStorageAction = packingItem.PackagesWeightAfterStorageAction.ToString();
                            storageActivitiy.PackingDetailsList.Add(packingDetails);
                        }
                    }
                    if (storageAction.UnloadingException != null)
                    {
                        storageActivitiy.StorageUnloadingExceptiontype = storageAction.UnloadingException.StorageUnloadingExceptiontype.ToString();
                        storageActivitiy.StorageUnloadingExceptionTypeText = storageAction.UnloadingException.StorageUnloadingExceptionTypeText;
                    }
                    storageActivitiesList.Add(storageActivitiy);
                }
                this.MyResponseData.StorageActionList = storageActivitiesList;
            }

            //Get Block Details of Goods By Declaration
            if (customResponse.GoodsItemByInvoice != null)
            {
                List<GoodsItemByInvoice> goodsItemByInvoiceList = new List<GoodsItemByInvoice>();
                foreach (var goodsItemByInvoiceItem in customResponse.GoodsItemByInvoice)
                {
                    GoodsItemByInvoice goodsItemByInvoice = new GoodsItemByInvoice();
                    goodsItemByInvoice.InvoiceSequenceNumber = goodsItemByInvoiceItem.InvoiceSequenceNumber.ToString();
                    goodsItemByInvoice.GoodsItemSequenceNumber = goodsItemByInvoiceItem.GoodsItemSequenceNumber.ToString();
                    goodsItemByInvoice.RemainingQuantity = goodsItemByInvoiceItem.RemainingQuantity.ToString();
                    goodsItemByInvoice.GoodsPriceBase = goodsItemByInvoiceItem.GoodsPrice_Base.ToString();
                    goodsItemByInvoice.GoodsPriceMAD = goodsItemByInvoiceItem.GoodsPrice_MAD.ToString();
                    goodsItemByInvoice.GoodsPriceMADAEF = goodsItemByInvoiceItem.GoodsPrice_MAD_AEF.ToString();
                    goodsItemByInvoice.CurrencyType = goodsItemByInvoiceItem.CurrencyType;
                    goodsItemByInvoice.ExchangeRate = goodsItemByInvoiceItem.ExchangeRate.ToString();
                    goodsItemByInvoiceList.Add(goodsItemByInvoice);
                }
                this.MyResponseData.GoodsItemByInvoiceList = goodsItemByInvoiceList;
            }


            this.MyResponseData.UserMessage = "שליחת מסר יתרות מלאי בגוש בוצעה בהצלחה";

        }

        public override ST_8328_Web01_WarehouseBlockBalanceResponseData GetResponse(ST_8329_Web02_WarehouseBlockBalanceDetail customResponse, ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
