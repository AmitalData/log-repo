import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ContainerPM } from '../EntityPMs/ContainerPM';
import { DateTool } from '../../Infrastructure/Tools';

export class ContainerPMInitService {

    public static InitValues(entityPM: ContainerPM, isNew: boolean) {
    
    }

    public static ApplyUIPoperties(entityPM: ContainerPM, isNew: boolean) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("MainCarriageCarrierId", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageVesselId", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageETA", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageETD", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageATA", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageATD", "Container", false);
            entityPM.UIProperties.SetEnabled("Master", "Container", false);
            entityPM.UIProperties.SetEnabled("TerminalAddress", "Container", false);
            entityPM.UIProperties.SetEnabled("TerminalPhone", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPickupETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPickupETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPickupATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPickupATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1ETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1ETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1ATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1ATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2ETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2ETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2ATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2ATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3ETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3ETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3ATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3ATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentDeliveryETA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentDeliveryETD", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentDeliveryATA", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentDeliveryATD", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentOriginAgentId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentDestinationAgentId", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageFromId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageToId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageFromId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageToId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1FromId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1ToId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2FromId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2ToId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3FromId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3ToId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageFromId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageToId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentFirstPickupFrom", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentFirstPickupTo", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageFrom", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentPreCarriageTo", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageFrom", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentMainCarriageTo", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1From", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment1To", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2From", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment2To", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3From", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTransshipment3To", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageFrom", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentOnCarriageTo", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentLastDeliveryFrom", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentLastDeliveryTo", "Container", false);

            entityPM.UIProperties.SetEnabled("ShipmentNumber", "Container", false);
            entityPM.UIProperties.SetEnabled("ContainersCount", "Container", false);
            entityPM.UIProperties.SetEnabled("HandlerId", "Container", false);
            entityPM.UIProperties.SetEnabled("HandlerName", "Container", false);
            entityPM.UIProperties.SetEnabled("CustomerId", "Container", false);
            entityPM.UIProperties.SetEnabled("OPClosed", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTypeId", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentTypeName", "Container", false);
            entityPM.UIProperties.SetEnabled("ShipmentCreateDate", "Container", false);
            entityPM.UIProperties.SetEnabled("PODReceivedOnDate", "Container", false);

        }
    }
}
