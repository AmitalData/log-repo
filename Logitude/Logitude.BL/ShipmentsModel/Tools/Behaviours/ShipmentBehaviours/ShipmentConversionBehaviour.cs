using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentConversionBehaviour : IServiceBehaviour
    {
        private int tenant;
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;
        private ShipmentPackageRepository shipmentPackageRepository;
        private ShipmentContainerStatusRepository shipmentContainerStatusRepository;
        private InsideShipmentPackageRepository insideShipmentPackageRepository;
        private ShipmentPackageItemRepository shipmentPackageItemRepository;
        private ShipmentPackageHarmonizeRepository shipmentPackageHarmonizeRepository;
        private ShipmentOrderPackageRepository shipmentOrderPackageRepository;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.tenant = this.initializer.Tenant;
            this.shipmentPackageRepository = this.initializer.ShipmentPackageRepository;
            this.shipmentContainerStatusRepository = this.initializer.ShipmentContainerStatusRepository;
            this.insideShipmentPackageRepository = this.initializer.InsideShipmentPackageRepository;
            this.shipmentPackageItemRepository = this.initializer.ShipmentPackageItemRepository;
            this.shipmentPackageHarmonizeRepository = this.initializer.ShipmentPackageHarmonizeRepository;
            this.shipmentOrderPackageRepository = this.initializer.ShipmentOrderPackageRepository;

            this.HandleBehaviour();
        }
        private void HandleBehaviour()
        {
            if (entityPM.ConvertShipmentToLCL || entityPM.ConvertShipmentToFCL || entityPM.ConvertShipmentToLTL || entityPM.ConvertShipmentToFTL)
            {
                if (entityPM.ShipmentPackages != null && entityPM.ShipmentPackages.Count > 0)
                {
                    this.DeleteShipmentPackages();                    
                }

                if (entityPM.ShipmentOrderPackages != null && entityPM.ShipmentOrderPackages.Count > 0)
                {
                    this.DeleteShipmentOrderPackages();
                }

                entityPM.BookingVolume = null;
                entityPM.BookingNumberOfPackages = null;
                entityPM.OrderChargeableWeight = null;
                entityPM.OrderGrossWeight = null;
                entityPM.OrderVolumetricWeight = null;
                entityPM.TEU = null;
                entityPM.NumberOfPackages = null;
                entityPM.NumberOfContainers = null;
                entityPM.GrossWeight = null;
                entityPM.ChargeableWeight = null;
                entityPM.VolumetricWeight = null;
                entityPM.Volume = null;

                if (entityPM.ConvertShipmentToLCL)
                {
                    entityPM.ShipmentTypeId = "LCLD";
                }

                else if (entityPM.ConvertShipmentToFCL)
                {
                    entityPM.ShipmentTypeId = "FCLD";
                }

                else if (entityPM.ConvertShipmentToLTL)
                {
                    entityPM.ShipmentTypeId = "LTL";
                }

                else if (entityPM.ConvertShipmentToFTL)
                {
                    entityPM.ShipmentTypeId = "FTL";
                }
                                
                this.initializer.IsUpdatingSubType = true;
                this.initializer.IsUpdatingProfitFromConversion = true;
            }
        }

        private void DeleteShipmentPackages()
        {
            foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
            {
                ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.Id, tenant);

                if (itemPoco != null)
                {
                    List<ShipmentContainerStatus> shipmentContainerStatuses = shipmentContainerStatusRepository.GetShipmentContainerStatusByContainerId(itemPoco.Id, tenant).ToList();
                    if (shipmentContainerStatuses != null)
                    {
                        foreach (ShipmentContainerStatus item in shipmentContainerStatuses)
                        {
                            shipmentContainerStatusRepository.Remove(item);
                        }
                    }

                    List<InsideShipmentPackage> list = insideShipmentPackageRepository.GetInsidePackagesByShipmentPackageId(itemPoco.Id, tenant).ToList();
                    if (list != null)
                    {
                        foreach (InsideShipmentPackage item in list)
                        {
                            insideShipmentPackageRepository.Remove(item);
                        }
                    }

                    List<ShipmentPackageItem> list2 = shipmentPackageItemRepository.GetShipmentPackageItemsbyPackageId(itemPoco.Id, tenant).ToList();
                    if (list2 != null)
                    {
                        foreach (ShipmentPackageItem item in list2)
                        {
                            shipmentPackageItemRepository.Remove(item);
                        }
                    }

                    List<ShipmentPackageHarmonize> list3 = shipmentPackageHarmonizeRepository.GetShipmentPackageHarmonizesByShipmentPackageId(itemPoco.Id, tenant).ToList();
                    if (list3 != null)
                    {
                        foreach (ShipmentPackageHarmonize item in list3)
                        {
                            shipmentPackageHarmonizeRepository.Remove(item);
                        }
                    }

                    shipmentPackageRepository.Remove(itemPoco);
                }
            }
        }

        private void DeleteShipmentOrderPackages()
        {
            foreach (ShipmentOrderPackagePM itemPM in entityPM.ShipmentOrderPackages)
            {
                ShipmentOrderPackage itemPoco = shipmentOrderPackageRepository.GetSingleShipmentOrderPackage(itemPM.Id);

                if (itemPoco != null)
                {
                    shipmentOrderPackageRepository.Remove(itemPoco);
                }
            }
        }
    }
}
