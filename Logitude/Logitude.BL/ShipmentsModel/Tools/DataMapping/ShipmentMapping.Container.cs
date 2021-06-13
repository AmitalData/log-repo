using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
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
                container.Id = containerPM.Id;
                container.Tenant = containerPM.Tenant;
                container.CreateDate = containerPM.CreateDate;
                container.CreatedByUserId = containerPM.CreatedByUserId;
            }

            container.UpdateDate = containerPM.UpdateDate;
            container.UpdatedByUserId = containerPM.UpdatedByUserId;
            container.MainCarriageCarrierId = containerPM.MainCarriageCarrierId;
            container.MainCarriageCarrierNumber = containerPM.MainCarriageCarrierNumber;
            container.MainCarriageATA = containerPM.MainCarriageATA;
            container.MainCarriageATD = containerPM.MainCarriageATD;
            container.MainCarriageETA = containerPM.MainCarriageETA;
            container.MainCarriageETD = containerPM.MainCarriageETD;
            container.MainCarriageVesselId = containerPM.MainCarriageVesselId;
            container.DischargeDate = containerPM.DischargeDate;
            container.Master = containerPM.Master;
            container.ShipmentPackagesId = containerPM.ShipmentPackagesId;
            container.ContainerNumber = containerPM.ContainerNumber;
            container.ShipmentId = containerPM.ShipmentId;
            BuildSearchField(containerPM, container);
        }

        public static void BuildSearchField(ContainerPM containerPM, Container container)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.ContainerNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.MainCarriageCarrierNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerPM.CarrierName);

            containerPM.SearchFields = mySearchFields;
            container.SearchFields = mySearchFields;
        }
    }
}
