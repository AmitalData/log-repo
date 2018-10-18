using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.BlockListInWarehouseServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class ST_8331_BlockListInWarehouseDetailResponseService : ResponseServiceBase<
        BlockListInWarehouseResponseData,
        ST_8331_Web02_BlockListInWarehouseDetail,
        BlockListInWarehouseRequestParams>
    {
        public override void Update(ST_8331_Web02_BlockListInWarehouseDetail customResponse, BlockListInWarehouseRequestParams requestParams)
        {
            //Analyze message 8331- BlockListInWarehouseDetail
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData = new BlockListInWarehouseResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }

            List<BlockListInWarehouseResult> blockListInWarehouseList = new List<BlockListInWarehouseResult>();

            if (customResponse.BlockDetailList != null)
            {
                foreach (var blockListInWarehouseItem in customResponse.BlockDetailList)
                {
                    BlockListInWarehouseResult blockListInWarehouse = new BlockListInWarehouseResult();
                    blockListInWarehouse.DeclarationNumber = blockListInWarehouseItem.DeclarationNumber;
                    if (blockListInWarehouseItem.WarehouseBlockNumberSpecified)
                    {
                        blockListInWarehouse.WarehouseBlockNumber = blockListInWarehouseItem.WarehouseBlockNumber.ToString();
                    }
                    blockListInWarehouse.ImporterNumber = blockListInWarehouseItem.ImporterNumber.ToString();
                    blockListInWarehouse.ImporterTitle = blockListInWarehouseItem.ImporterTitle;
                    blockListInWarehouse.OpeningDate = blockListInWarehouseItem.OpeningDate.Date.ToString("dd/MM/yyyy");
                    blockListInWarehouse.OriginalOpeningDate = blockListInWarehouseItem.OriginalOpeningDate.Date.ToString("dd/MM/yyyy");
                    blockListInWarehouse.LogicalPackagesQuantityBalance = blockListInWarehouseItem.LogicalPackagesQuantityBalance.ToString("N2");
                    blockListInWarehouse.PhysicalPackagesQuantityBalance = blockListInWarehouseItem.PhysicalPackagesQuantityBalance.ToString("N2");
                    blockListInWarehouse.Value = blockListInWarehouseItem.Value.ToString("N2");
                    blockListInWarehouse.SpecialActivityTypeName = blockListInWarehouseItem.BlockSpecialActivities.FirstOrDefault();

                    blockListInWarehouseList.Add(blockListInWarehouse);
                }
            }

            this.MyResponseData = new BlockListInWarehouseResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.UserMessage = "לאתר " + customResponse.GeneralDetails.siteText + " קיימים " + customResponse.GeneralDetails.NumberOfBlocksInList + "גושים ";
            this.MyResponseData.BlockListInWarehouseResultList = blockListInWarehouseList;
            this.MyResponseData.NumberOfBlocksInList = customResponse.GeneralDetails.NumberOfBlocksInList;
        }

        public override BlockListInWarehouseResponseData GetResponse(ST_8331_Web02_BlockListInWarehouseDetail customResponse, BlockListInWarehouseRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
