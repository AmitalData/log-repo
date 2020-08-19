using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        internal static void MapQuotePackage(QuotePackagePM itemPM, QuotePackage itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.QuoteId = itemPM.QuoteId;
            }

            itemPoco.Height = itemPM.Height;
            itemPoco.Length = itemPM.Length;
            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.GrossWeight = itemPM.GrossWeight;
            itemPoco.Width = itemPM.Width;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
        }
    }
}