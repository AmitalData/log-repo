using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public partial class CommodityQueryService
    {


        public Commodity CommodityCustomDataMapping(string Id, int tenant)
        {
            var MyCommodity = query.GetSinglePM(Id, tenant);

            var temp = new Commodity();
            temp.Id = MyCommodity.Id;
            temp.DescriptionOfGoods = MyCommodity.DescriptionOfGoods;
            temp.ChargeableWeight = MyCommodity.ChargeableWeight;
            temp.ChargeRate = MyCommodity.ChargeRate;
            temp.ChargeAmount = MyCommodity.ChargeAmount;
            temp.CommodityNumber = MyCommodity.CommodityNumber;
            temp.NumberOfPackages = MyCommodity.NumberOfPackages;
            temp.GrossWeight = MyCommodity.GrossWeight;
            temp.Volume = MyCommodity.Volume;
            temp.VolumetricWeight = MyCommodity.VolumetricWeight;

            return temp;
        }

        public string CommodityCustomDataMapping(ShipmentPM MyEntityPM,List<ShipmentCommodityPM> Commodities, int tenant)
        {
            if (Commodities.Count > 0)
            {
                return Commodities.FirstOrDefault().CommodityNumber;
            }
            else
            {
                return "";
            }
            
        }

        public ShipmentCommodityPM CommodityCustomDataMappingAndValidatin(Commodity MyEntity, int Tenant)
        {
            var temp = new ShipmentCommodityPM();
            if (!string.IsNullOrEmpty(MyEntity.Id))
            {
                temp = query.GetSinglePM(MyEntity.Id, Tenant);
            }
            if (string.IsNullOrEmpty(temp.Id))
            {
                temp.Id = MyEntity.Id;
            }
            temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;
            temp.ChargeableWeight = MyEntity.ChargeableWeight;
            temp.ChargeRate = MyEntity.ChargeRate;
            temp.ChargeAmount = MyEntity.ChargeAmount;
            temp.CommodityNumber = MyEntity.CommodityNumber;
            temp.NumberOfPackages = MyEntity.NumberOfPackages;
            temp.GrossWeight = MyEntity.GrossWeight;
            temp.Volume = MyEntity.Volume;
            temp.VolumetricWeight = MyEntity.VolumetricWeight;
            
            return temp;

        }

        public List<ShipmentCommodityPM> CommodityCustomDataMappingAndValidatin(House MyEntity,string MyCommodity, int Tenant)
        {
            var temp = new ShipmentCommodityPM();
            var myList = new List<ShipmentCommodityPM>();
            //if (!string.IsNullOrEmpty(MyEntity.Id))
            //{
            //    temp = query.GetSinglePM(MyEntity.Id, Tenant);
            //}
            //if (string.IsNullOrEmpty(temp.Id))
            //{
            //    temp.Id = MyEntity.Id;
            //}
            temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;
            temp.Tenant = Tenant; 
            temp.CommodityNumber = MyCommodity;
            myList.Add(temp);
            return myList;

        }

    }
}