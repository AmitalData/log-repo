using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.BlockListInWarehouseServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class ST_8330_BlockListInWarehouseFilterRequestService : RequestServiceBase
        <ST_8330_Web01_BlockListInWarehouseFilterParam, BlockListInWarehouseRequestParams>
    {
        public override ST_8330_Web01_BlockListInWarehouseFilterParam GetRequest(BlockListInWarehouseRequestParams requestParams)
        {
            //Build request ST_Web01_BlockListInWarehouseFilterParam- Filter Warehouse Params
            var myST_8330_Web01_BlockListInWarehouseFilterParam = new ST_8330_Web01_BlockListInWarehouseFilterParam();

            myST_8330_Web01_BlockListInWarehouseFilterParam.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myST_8330_Web01_BlockListInWarehouseFilterParam.BlockListParam = new ST_8330_Web01_BlockListInWarehouseFilterParamBlockListParam();
            myST_8330_Web01_BlockListInWarehouseFilterParam.BlockListParam.StorageSiteNumber = requestParams.StorageSiteNumber;
            myST_8330_Web01_BlockListInWarehouseFilterParam.BlockListParam.FromBlockOpenDate = (DateTime)requestParams.FromDate;
            myST_8330_Web01_BlockListInWarehouseFilterParam.BlockListParam.ToBlockOpenDate = (DateTime)requestParams.ToDate;
            myST_8330_Web01_BlockListInWarehouseFilterParam.BlockListParam.ShowResetBlocks = (int)requestParams.ShowResetBlocks;

            return myST_8330_Web01_BlockListInWarehouseFilterParam;
        }
    }
}
