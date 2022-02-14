using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping    {
        public static void MapProductItem(ShipmentProductItemPM itemPM, ShipmentProductItem itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.ProductItemId = itemPM.ProductItemId;
            itemPoco.Description = itemPM.Description;
            itemPoco.HTSCode = itemPM.HTSCode;
            itemPoco.SKU = itemPM.SKU;
            itemPoco.ApprovedByCustomer = itemPM.ApprovedByCustomer;
            itemPoco.Name = itemPM.Name;
            itemPoco.Brand = itemPM.Brand;
            itemPoco.ASIN = itemPM.ASIN;
            itemPoco.UPC = itemPM.UPC;
            itemPoco.OriginCountryId = itemPM.OriginCountryId;
            itemPoco.VATPercentage = itemPM.VATPercentage;
            itemPoco.DutiesPercentage = itemPM.DutiesPercentage;
            itemPoco.OtherDuties = itemPM.OtherDuties;
            itemPoco.Remarks = itemPM.Remarks;
            itemPoco.ShipperId = itemPM.ShipperId;
        }
    }
}
