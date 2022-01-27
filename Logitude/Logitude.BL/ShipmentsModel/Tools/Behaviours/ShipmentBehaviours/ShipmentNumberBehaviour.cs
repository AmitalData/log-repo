using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentNumberBehaviour : IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                if (!initializer.EntityPM.IsHybrid)
                {
                    this.GetCounterShipmentNumber();
                }
            }

            else
            {
                if (!initializer.EntityPOCO.IsCancelled || !initializer.EntityPM.IsCancelled)
                {
                    if (initializer.EntityPM.ShipmentDirectionConverted)
                    {
                        if (initializer.EntityPM.ShipmentConvertedNewNumber)
                        {
                            this.GetCounterShipmentNumber();
                        }
                    }
                }
            }
        }

        private void GetCounterShipmentNumber()
        {
            bool isTakenCounter = false;

            if (initializer.IsNewEntity && initializer.EntityPM.ShipmentNumber == null)
            {
                isTakenCounter = true;
            }

            else if (initializer.EntityPM.ShipmentDirectionConverted && initializer.EntityPM.ShipmentConvertedNewNumber)
            {
                this.entityPM.OldShipmentNumber = initializer.EntityPM.ShipmentNumber;
                isTakenCounter = true;

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = initializer.Tenant,
                    EventTypeCode = "SNOC",
                    UserId = initializer.LoggedContactId,
                    EntityId = initializer.EntityPM.Id,
                    ObjectTableName = "Shipment",
                    Notes = "Old Number: " + initializer.EntityPM.ShipmentNumber,
                    Entity = initializer.EntityPM,
                });
            }

            if (isTakenCounter)
            {
                Dictionary<string, string> counterAdditionalParameters = new Dictionary<string, string>() { { "[B]", "" } };
                if (!string.IsNullOrEmpty(initializer.EntityPM.BranchId))
                {
                    BranchRepository branchRepository = new BranchRepository(initializer.CommonContext);
                    Branch myBranch = branchRepository.GetSingleBranch(initializer.EntityPM.BranchId, initializer.EntityPM.Tenant);

                    if (myBranch != null && !string.IsNullOrEmpty(myBranch.CounterCode))
                    {
                        counterAdditionalParameters["[B]"] = myBranch.CounterCode;
                    }
                }

                if (initializer.EntityPM.ShipmentLevelCode == "C")
                {
                    initializer.EntityPM.ShipmentNumber = TableCounter.GetNumber(initializer.Tenant, "MAST", initializer.EntityPM.DirectionId, initializer.EntityPM.TransportModeId, counterAdditionalParameters);
                }

                else
                {
                    if (initializer.EntityPM.DirectionId.ToUpper() == "C")
                    {
                        initializer.EntityPM.ShipmentNumber = TableCounter.GetNumber(initializer.Tenant, "SHIP", "I", initializer.EntityPM.TransportModeId, counterAdditionalParameters);
                    }

                    else
                    {
                        initializer.EntityPM.ShipmentNumber = TableCounter.GetNumber(initializer.Tenant, "SHIP", initializer.EntityPM.DirectionId, initializer.EntityPM.TransportModeId, counterAdditionalParameters);
                    }
                }
            }
        }

    }
}
