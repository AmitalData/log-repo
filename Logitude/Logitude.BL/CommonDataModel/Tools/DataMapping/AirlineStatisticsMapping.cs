using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AirlineStatisticsMapping
    {
        public static void MapEntity(AirlineStatisticsPM entityPM, AirlineStatistics poco, bool isNewState)
        {
            poco.Tenant = entityPM.Tenant;
            poco.SourceTenant = entityPM.SourceTenant;
            poco.SourceTenantName = entityPM.SourceTenantName;
            poco.ShipmentId = entityPM.ShipmentId;
            poco.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            poco.BookingId = entityPM.BookingId;
            poco.EntityReference = entityPM.EntityReference;
            poco.AWBNumber = entityPM.AWBNumber;
            poco.HWBNumber = entityPM.HWBNumber;
            poco.AirlineCode = entityPM.AirlineCode;
            poco.CreateDate = entityPM.CreateDate;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.EntitiyCreateDate = entityPM.EntitiyCreateDate;
            poco.EntitiyUpdateDate = entityPM.EntitiyUpdateDate;
            poco.EntityCreatedByUserName = entityPM.EntityCreatedByUserName;
            poco.MessageType = entityPM.MessageType;
            poco.LastSentDate = entityPM.LastSentDate;
            poco.EntityStatus = entityPM.EntityStatus;
            poco.NumberOfPackages = entityPM.NumberOfPackages;
            poco.ChargeableWeight = entityPM.ChargeableWeight;
            poco.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            poco.GrossWeight = entityPM.GrossWeight;
            poco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            poco.Volume = entityPM.Volume;
            poco.VolumeUnitCode = entityPM.VolumeUnitCode;
            poco.OriginCode = entityPM.OriginCode;
            poco.DestinationCode = entityPM.DestinationCode;
            poco.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            poco.ShipperName = entityPM.ShipperName;
            poco.ConsigneeName = entityPM.ConsigneeName;
            poco.Flight1 = entityPM.Flight1;
            poco.Flight1Date = entityPM.Flight1Date;
            poco.Flight2 = entityPM.Flight2;
            poco.Flight2Date = entityPM.Flight2Date;
            poco.Flight3 = entityPM.Flight3;
            poco.Flight3Date = entityPM.Flight3Date;
            poco.OnCarriageTo = entityPM.OnCarriageTo;
            poco.OnCarriageDate = entityPM.OnCarriageDate;
            poco.PreCarriageFrom = entityPM.PreCarriageFrom;
            poco.PreCarriageDate = entityPM.PreCarriageDate;
            poco.Allotment = entityPM.Allotment;

            BuildSearchFields(entityPM, poco);            
        }

        private static void BuildSearchFields(AirlineStatisticsPM entityPM, AirlineStatistics poco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.SourceTenant.ToString());
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.SourceTenantName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EntityReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.AWBNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.MessageType);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EntityCreatedByUserName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            poco.SearchFields = mySearchFields;
        }
    }
}
