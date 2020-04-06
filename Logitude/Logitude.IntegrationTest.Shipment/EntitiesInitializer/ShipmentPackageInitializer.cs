using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.EntitiesInitializer
{
    public class ShipmentPackageInitializer : IEntityInitializer
    {
        public object Create(EntityInitializerArguments args)
        {
            ShipmentPackagePM entityPM = new ShipmentPackagePM()
            {
                Quantity = args.PackageQuantity,
                Weight = args.PackageWeight,
                Length = args.PackageLength,
                Width = args.PackageWidth,
                Height = args.PackageHeight,
                Tenant = IntegrationTestLoginParameters.Tenant,               
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            this.ComouteVolume(entityPM);

            return entityPM;
        }

        private void ComouteVolume(ShipmentPackagePM entityPM)
        {
            if (entityPM.Length != null && entityPM.Width != null && entityPM.Height != null)
            {
                entityPM.Volume = entityPM.Length * entityPM.Width * entityPM.Height;
                //entityPM.VolumetricWeight = entityPM.Quantity * entityPM.UnitPrice;
            }
        }
    }
}
