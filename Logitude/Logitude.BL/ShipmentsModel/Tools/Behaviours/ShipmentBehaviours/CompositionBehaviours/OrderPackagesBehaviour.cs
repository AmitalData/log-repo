using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.CompositionBehaviours
{
    public class OrderPackagesBehaviour: IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;
        private ShipmentOrderPackageRepository entityRepository;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.entityRepository = new ShipmentOrderPackageRepository(this.initializer.ShipmentContext);

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                foreach (ShipmentOrderPackagePM itemPM in entityPM.ShipmentOrderPackages)
                {
                    itemPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
            }

            foreach (ShipmentOrderPackagePM itemPM in entityPM.ShipmentOrderPackages)
            {
                switch (itemPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            this.CreateShipmentOrderPackage(itemPM);
                            break;
                        }

                    case ChangeSetOperation.Update:
                        {
                            this.UpdateShipmentOrderPackage(itemPM);
                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            this.DeleteShipmentOrderPackage(itemPM);
                            break;
                        }

                    default: { break; }
                }
            }
        }

        private void CreateShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentOrderPackage", initializer.Tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = initializer.Tenant;

            ShipmentOrderPackage itemPoco = new ShipmentOrderPackage()
            {
                Id = itemPM.Id,
            };

            MapEntity(itemPM, itemPoco);
            entityRepository.Add(itemPoco);
        }

        private void UpdateShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            ShipmentOrderPackage itemPoco = entityRepository.GetSingleShipmentOrderPackage(itemPM.Id);

            if (itemPoco != null)
            {
                MapEntity(itemPM, itemPoco);
                entityRepository.Update(itemPoco);
            }
        }

        private void DeleteShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            ShipmentOrderPackage itemPoco = entityRepository.GetSingleShipmentOrderPackage(itemPM.Id);

            if (itemPoco != null)
            {
                entityRepository.Remove(itemPoco);
            }
        }

        private void MapEntity(ShipmentOrderPackagePM itemPM, ShipmentOrderPackage itemPoco)
        {
            if (itemPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ShipmentId = itemPM.ShipmentId;
            }

            itemPoco.PackageTypeId = itemPM.PackageTypeId;
            itemPoco.Quantity = itemPM.Quantity;
            itemPoco.IsContainer = itemPM.IsContainer;
            itemPoco.Height = itemPM.Height;
            itemPoco.Length = itemPM.Length;
            itemPoco.Volume = itemPM.Volume;
            itemPoco.GrossWeight = itemPM.GrossWeight;
            itemPoco.Width = itemPM.Width;
            itemPoco.VolumetricWeight = itemPM.VolumetricWeight;
            itemPoco.ContainerNumber = itemPM.ContainerNumber;
        }
    }
}
