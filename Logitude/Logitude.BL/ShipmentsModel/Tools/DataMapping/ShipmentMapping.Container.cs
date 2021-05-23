using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        public static void MapContainer(ContainerPM containerPM, Container container, bool isNewEntity)
        {
            if (isNewEntity)
            {
                containerPM.Id = container.Id;
                containerPM.Tenant = container.Tenant;               
            }

            containerPM.CreateDate = container.CreateDate;
            containerPM.CreatedByUserId = container.CreatedByUserId;
            containerPM.UpdateDate = container.UpdateDate;
            containerPM.UpdatedByUserId = container.UpdatedByUserId;
            containerPM.MainCarriageCarrierId = container.MainCarriageCarrierId;
            containerPM.MainCarriageCarrierNumber = container.MainCarriageCarrierNumber;
            containerPM.MainCarriageATA = container.MainCarriageATA;
            containerPM.MainCarriageATD = container.MainCarriageATD;
            containerPM.MainCarriageETA = container.MainCarriageETA;
            containerPM.MainCarriageETD = container.MainCarriageETD;
            containerPM.MainCarriageVesselId = container.MainCarriageVesselId;
            containerPM.DischargeDate = container.DischargeDate;
            containerPM.Master = container.Master;
            containerPM.SearchFields = container.SearchFields;
            containerPM.ShipmentPackagesId = container.ShipmentPackagesId;
            containerPM.ContainerNumber = container.ContainerNumber;
        }
    }
}
