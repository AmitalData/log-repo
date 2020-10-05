using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentNumberCounterBehaviour: IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            this.InitializeFlags();
            this.UpdateShipmentNumber();
        }

        bool isTakenCounter = false;
        private void InitializeFlags()
        {
            if (initializer.IsNewEntity && initializer.EntityPM.ShipmentNumber == null)
            {
                isTakenCounter = true;
            }

            else if (initializer.EntityPM.ShipmentDirectionConverted && initializer.EntityPM.ShipmentConvertedNewNumber)
            {
                isTakenCounter = true;

                // Save Old number in service ini
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = initializer.Tenant,
                    EventTypeCode = "SNOC",
                    UserId = initializer.LoggedContact.Id,
                    EntityId = initializer.EntityPM.Id,
                    ObjectTableName = "Shipment",
                    Notes = "Old Number: " + initializer.EntityPM.ShipmentNumber,
                });
            }
        }

        private void UpdateShipmentNumber()
        {
            if (isTakenCounter)
            {
                Dictionary<string, string> counterAdditionalParameters = new Dictionary<string, string>() { { "[B]", "" } };
                if (!string.IsNullOrEmpty(initializer.EntityPM.BranchId))
                {
                    Branch myBranch = (from d in initializer.CommonContext.Branches
                                       where d.Tenant == initializer.Tenant
                                       && d.Id == initializer.EntityPM.BranchId
                                       select d).FirstOrDefault();

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
