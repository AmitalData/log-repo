using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.EntitiesInitializer
{
    public class ShipmentOrderPackageInitializer : IEntityInitializer
    {
        public object Create(EntityInitializerArguments args)
        {
            ShipmentOrderPackagePM entityPM = new ShipmentOrderPackagePM()
            {
                Tenant = IntegrationTestLoginParameters.Tenant,


            };

            return entityPM;
        }
    }
}

