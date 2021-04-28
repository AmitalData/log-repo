using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentOperationalDateBehaviour : IServiceBehaviour
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
            CalculateOperationalDate();
        }

        private void CalculateOperationalDate()
        {
            DateTime? output = null;

            if (entityPM.ShipmentLevelCode == "H")
            {
                output = CalculateHouseShipmentOperationalDate();
            }

            else
            {
                if (entityPM.DirectionId == "I")
                {
                    output = CalculateArrivalShipmentOperationalDate();

                }

                else
                {
                    output = CalculateDepartureShipmentOperationalDate();
                }
            }

            entityPM.OperationalDate = output;
        }

        private DateTime? CalculateHouseShipmentOperationalDate()
        {
            DateTime? output = null;

            if (entityPM.MasterShipmentDataId != null)
            {
                output = entityPM.OperationalDate;
            }

            if (output == null)
            {
                output = entityPM.CreateDateTime;
            }

            return output;
        }

        private DateTime? CalculateArrivalShipmentOperationalDate()
        {
            DateTime? output = null;

            if (entityPM.Transshipment3ATA != null)
            {
                output = entityPM.Transshipment3ATA;
            }

            else if (entityPM.Transshipment2ATA != null)
            {
                output = entityPM.Transshipment2ATA;
            }

            else if (entityPM.Transshipment1ATA != null)
            {
                output = entityPM.Transshipment1ATA;
            }

            else if (entityPM.MainCarriageATA != null)
            {
                output = entityPM.MainCarriageATA;
            }

            else if (entityPM.Transshipment3ETA != null)
            {
                output = entityPM.Transshipment3ETA;
            }

            else if (entityPM.Transshipment2ETA != null)
            {
                output = entityPM.Transshipment2ETA;
            }

            else if (entityPM.Transshipment1ETA != null)
            {
                output = entityPM.Transshipment1ETA;
            }

            else if (entityPM.MainCarriageETA != null)
            {
                output = entityPM.MainCarriageETA;
            }

            else
            {
                output = entityPM.CreateDateTime;
            }

            return output;
        }

        private DateTime? CalculateDepartureShipmentOperationalDate()
        {
            DateTime? output = null;

            if (entityPM.MainCarriageATD != null)
            {
                output = entityPM.MainCarriageATD;
            }

            else if (entityPM.Transshipment1ATD != null)
            {
                output = entityPM.Transshipment1ATD;
            }

            else if (entityPM.Transshipment2ATD != null)
            {
                output = entityPM.Transshipment2ATD;
            }

            else if (entityPM.Transshipment3ATD != null)
            {
                output = entityPM.Transshipment3ATD;
            }

            else if (entityPM.MainCarriageETD != null)
            {
                output = entityPM.MainCarriageETD;
            }

            else if (entityPM.Transshipment1ETD != null)
            {
                output = entityPM.Transshipment1ETD;
            }

            else if (entityPM.Transshipment2ETD != null)
            {
                output = entityPM.Transshipment2ETD;
            }

            else if (entityPM.Transshipment3ETD != null)
            {
                output = entityPM.Transshipment3ETD;
            }

            else
            {
                output = entityPM.CreateDateTime;
            }

            return output;
        }
    }
}
