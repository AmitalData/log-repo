using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.WarehouseBlockBalanceServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class ST_8328_Web01_WarehouseBlockBalanceFilterParamRequestService
        : RequestServiceBase
        <ST_8328_Web01_WarehouseBlockBalanceFilterParam, ST_8328_Web01_WarehouseBlockBalanceRequestParams>
    {
        public override ST_8328_Web01_WarehouseBlockBalanceFilterParam GetRequest(ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams)
        {
            //Build request ST_8328_Web01_WarehouseBlockBalanceFilterParam - Filter Warehouse Params
            var myST_8328_Web01_WarehouseBlockBalanceFilterParam = new ST_8328_Web01_WarehouseBlockBalanceFilterParam();

            myST_8328_Web01_WarehouseBlockBalanceFilterParam.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myST_8328_Web01_WarehouseBlockBalanceFilterParam.BlockIdentifier = new ST_8328_Web01_WarehouseBlockBalanceFilterParamBlockIdentifier();
            myST_8328_Web01_WarehouseBlockBalanceFilterParam.BlockIdentifier.StorageSiteNumber = requestParams.StorageSiteNumber;
            myST_8328_Web01_WarehouseBlockBalanceFilterParam.BlockIdentifier.declerationNumber = requestParams.DeclarationNumber;
            int blockNumber;
            if (int.TryParse(requestParams.WarehouseBlockNumber,out blockNumber)) 
            {
                myST_8328_Web01_WarehouseBlockBalanceFilterParam.BlockIdentifier.warehouseBlockNumber = blockNumber;
                myST_8328_Web01_WarehouseBlockBalanceFilterParam.BlockIdentifier.warehouseBlockNumberSpecified = true;
            }

            myST_8328_Web01_WarehouseBlockBalanceFilterParam.DisplayOption = new ST_8328_Web01_WarehouseBlockBalanceFilterParamDisplayOption();
            if (!string.IsNullOrWhiteSpace(requestParams.DisplayGoodsItemByInvoice))
            {
                myST_8328_Web01_WarehouseBlockBalanceFilterParam.DisplayOption.displayGoodsItemByInvoice = true;
            }
            else
            {
                myST_8328_Web01_WarehouseBlockBalanceFilterParam.DisplayOption.displayGoodsItemByInvoice = false;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = "שאילתא יתרות מלאי בגוש " + requestParams.DeclarationNumber + requestParams.StorageSiteNumber;

            return myST_8328_Web01_WarehouseBlockBalanceFilterParam;
        }
        
    }
}
