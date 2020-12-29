using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentMasterEntityBehaviour: IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;

            this.HandleBehaviour();
            this.HandleEntityFlags();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                if (entityPM.ShipmentLevelCode != "H")
                {
                    initializer.EntityMasterData = new ShipmentMasterData();
                    initializer.EntityMasterData.Id = initializer.EntityPM.Id;
                    initializer.EntityMasterData.MasterShipmentNumber = initializer.EntityPM.ShipmentNumber;
                    initializer.EntityPM.MasterShipmentDataId = initializer.EntityPM.Id;
                    initializer.MasterDataRepository.Add(initializer.EntityMasterData);
                }

                else if (entityPM.ShipmentLevelCode == "H" && entityPM.MasterShipmentDataId != null)
                {
                    // this case is when create house from master sceen
                    // need to get the master, some fields need to be calculated from the master
                    // but we dont want to map the master it self
                    initializer.EntityMasterData = initializer.MasterDataRepository.GetSingleMasterData(entityPM.MasterShipmentDataId);

                    if (initializer.EntityMasterData != null)
                    {
                        entityPM.ComputedShipmentNumber = initializer.EntityMasterData.MasterShipmentNumber;

                        if (initializer.EntityMasterData.ProrateReceivables)
                        {
                            initializer.IsUpdatingRegistryDate = true;
                            initializer.IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }
            }

            else
            {
                string masterDataId = null;

                if (entityPM.ShipmentLevelCode == "H")
                {
                    masterDataId = entityPM.MasterShipmentDataId;
                }

                else
                {
                    masterDataId = initializer.EntityPOCO.MasterShipmentDataId;
                }

                if (masterDataId != null)
                {
                    initializer.EntityMasterData = initializer.MasterDataRepository.GetSingleMasterData(masterDataId);
                }

                if (initializer.EntityPM.IsHybrid)
                {
                    if (initializer.EntityMasterData == null)
                    {
                        if (initializer.EntityPM.ShipmentLevelCode != "H")
                        {
                            if (!initializer.EntityPM.ConvertFromDirectToHouse && !initializer.EntityPM.ConvertFromHouseToDirect)
                            {
                                initializer.EntityMasterData = new ShipmentMasterData();
                                initializer.EntityMasterData.Id = initializer.EntityPM.Id;
                                initializer.EntityPM.MasterShipmentDataId = initializer.EntityPM.Id;
                                initializer.EntityMasterData.MasterShipmentNumber = initializer.EntityPM.ShipmentNumber;
                                initializer.MasterDataRepository.Add(initializer.EntityMasterData);
                            }
                        }
                    }
                }
            }
        }
        private void HandleEntityFlags()
        {
            if (initializer.EntityMasterData != null)
            {
                if (initializer.IsNewEntity)
                {
                    if (initializer.EntityPM.ShipmentLevelCode == "H")
                    {
                        if (initializer.EntityMasterData.ProrateReceivables)
                        {
                            initializer.IsUpdatingRegistryDate = true;
                            initializer.IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }

                else
                {
                    if (initializer.EntityPM.ShipmentLevelCode == "C")
                    {
                        if (initializer.EntityPM.ProrateReceivables != initializer.EntityMasterData.ProrateReceivables)
                        {
                            initializer.IsProratingChanged = true;
                            initializer.IsUpdatingRegistryDate = true;
                            initializer.IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }
            }

        }
    }
}
