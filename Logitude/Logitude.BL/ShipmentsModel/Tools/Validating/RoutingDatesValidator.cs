using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class RoutingDatesValidator
    {
        private ShipmentPM shipmentPM;
        public RoutingDatesValidator(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.SetLegsExistsFlags();            
        }

        private bool isPickupsExists;
        private bool isDeliveriesExists;
        private bool isPreCarriageExists;
        private bool isPreForwardingExists;
        private bool isTransshipment1Exists;
        private bool isTransshipment2Exists;
        private bool isTransshipment3Exists;
        private bool isOnCarriageExists;
        private bool isOnForwardingExists;
        private bool isWarehouseLegExists;
        private bool isWarehouse1LegExists;
        private bool isDestinationWarehouseLegExists;
        private bool isWarehouseLeg2Exists;
        private bool isDestinationWarehouse2LegExists;
        private void SetLegsExistsFlags()
        {
            this.isPickupsExists = shipmentPM.ShipmentPickUps.Count > 0;
            this.isDeliveriesExists = shipmentPM.ShipmentDeliveries.Count > 0;
            this.isPreCarriageExists = shipmentPM.PreCarriageFromPortId != null && shipmentPM.PreCarriageToPortId != null;
            this.isPreForwardingExists = shipmentPM.PreForwardingFromPortId != null && shipmentPM.PreForwardingToPortId != null;
            this.isTransshipment1Exists = shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null;
            this.isTransshipment2Exists = shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null;
            this.isTransshipment3Exists = shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null;
            this.isOnCarriageExists = shipmentPM.OnCarriageFromPortId != null && shipmentPM.OnCarriageToPortId != null;
            this.isOnForwardingExists = shipmentPM.OnForwardingFromPortId != null && shipmentPM.OnForwardingToPortId != null;
            this.isWarehouseLegExists = shipmentPM.WarehouseLegWarehouseId != null;
            this.isWarehouse1LegExists = isWarehouseLegExists == true && shipmentPM.DirectionId != "I";
            this.isDestinationWarehouseLegExists = isWarehouseLegExists == true && shipmentPM.DirectionId == "I";
            this.isWarehouseLeg2Exists = shipmentPM.WarehouseLeg2WarehouseId != null;
            this.isDestinationWarehouse2LegExists = isWarehouseLeg2Exists == true && shipmentPM.DirectionId == "R";
        }

        public void Validate()
        {
            this.ValidatePickups();
            this.ValidateDeliveries();
            this.ValidatePreCarriage();
            this.ValidatePreForwarding();
            this.ValidateMainCarriage();
            this.ValidateTransshipment1();
            this.ValidateTransshipment2();
            this.ValidateTransshipment3();
            this.ValidateWarehouse();
            this.ValidateDestinationWarehouse();
            this.ValidateOnCarriage();
            this.ValidateOnForwarding();            
        }

        private void ValidatePickups()
        {
            if (!isPickupsExists) return;
            foreach (ShipmentPickUpPM shipmentPickUp in  shipmentPM.ShipmentPickUps)
            {
                ShipmentPickUpDeliveryValidator.ValidatePickup(shipmentPickUp, shipmentPM);                
            }
        }
        private void ValidateDeliveries()
        {
            if (!isDeliveriesExists) return;
            foreach (ShipmentDeliveryPM shipmentDelivery in shipmentPM.ShipmentDeliveries)
            {
                ShipmentPickUpDeliveryValidator.ValidateDelivery(shipmentDelivery, shipmentPM);
            }
        }
        private void ValidatePreCarriage()
        {
            if (!isPreCarriageExists) return;

            this.ValidateEstimatedLegDates("Pre-Carriage", shipmentPM.PreCarriageETD, shipmentPM.PreCarriageETA);
            this.ValidateActualLegDates("Pre-Carriage", shipmentPM.PreCarriageATD, shipmentPM.PreCarriageATA);
            this.ValidatePreCarriage_PreviousLeg();
            this.ValidatePreCarriage_NextLeg();
        }
        private void ValidatePreCarriage_PreviousLeg()
        {
            if (isPreForwardingExists)            
                this.ValidatePreCarriage_Previous_PreForwarding();               
            
            else if (isWarehouse1LegExists)            
                this.ValidatePreCarriage_Previous_Warehouse1();    
        }
        private void ValidatePreCarriage_Previous_PreForwarding()
        {
            if (IsDateSmaller(shipmentPM.PreCarriageETD, shipmentPM.PreForwardingETA))            
                throw new ApplicationException("Pre-Carriage expected departure must be bigger than Pre-Forwarding expected arrival");            

            if (IsDateSmaller(shipmentPM.PreCarriageATD, shipmentPM.PreForwardingATA))            
                throw new ApplicationException("Pre-Carriage actual departure must be bigger than Pre-Forwarding actual arrival");            
        }
        private void ValidatePreCarriage_Previous_Warehouse1()
        {
            if (IsDateSmaller(shipmentPM.PreCarriageETD, shipmentPM.WarehouseLegExpectedReleaseDate))            
                throw new ApplicationException("Pre-Carriage expected departure must be bigger than Warehouse expected release");            

            if (IsDateSmaller(shipmentPM.PreCarriageATD, shipmentPM.WarehouseLegActualReleaseDate))            
                throw new ApplicationException("Pre-Carriage actual departure must be bigger than Warehouse actual release");
            
        }
        private void ValidatePreCarriage_NextLeg()
        {
            if (IsDateBigger(shipmentPM.PreCarriageETA, shipmentPM.MainCarriageETD))            
                throw new ApplicationException("Pre-Carriage expected arrival must be less than Main-Carriage expected departure");            

            if (IsDateBigger(shipmentPM.PreCarriageATA, shipmentPM.MainCarriageATD))            
                throw new ApplicationException("Pre-Carriage actual arrival must be less than Main-Carriage actual departure");            
        }
        private void ValidatePreForwarding()
        {
            if (!isPreForwardingExists) return;
            this.ValidateEstimatedLegDates("Pre-Forwarding", shipmentPM.PreForwardingETD, shipmentPM.PreForwardingETA);
            this.ValidateActualLegDates("Pre-Forwarding", shipmentPM.PreForwardingATD, shipmentPM.PreForwardingATA);
            this.ValidatePreForwarding_PreviousLeg();
            this.ValidatePreForwarding_NextLeg();
        }
        private void ValidatePreForwarding_PreviousLeg()
        {
            if (isWarehouse1LegExists)
                this.ValidatePreForwarding_Previous_Warehouse1();
        }
        private void ValidatePreForwarding_Previous_Warehouse1()
        {
            if (IsDateSmaller(shipmentPM.PreForwardingETD, shipmentPM.WarehouseLegExpectedReleaseDate))            
                throw new ApplicationException("Pre-Forwarding expected departure must be bigger than Warehouse expected release");            

            if (IsDateSmaller(shipmentPM.PreForwardingATD, shipmentPM.WarehouseLegActualReleaseDate))            
                throw new ApplicationException("Pre-Forwarding actual departure must be bigger than Warehouse actual release");            
        }
        
        private void ValidatePreForwarding_NextLeg()
        {
            if (IsDateBigger(shipmentPM.PreForwardingETA, shipmentPM.MainCarriageETD))
                throw new ApplicationException("Pre-Forwarding expected arrival must be less than Main-Forwarding expected departure");

            if (IsDateBigger(shipmentPM.PreForwardingATA, shipmentPM.MainCarriageATD))
                throw new ApplicationException("Pre-Forwarding actual arrival must be less than Main-Forwarding actual departure");
        }
        private void ValidateMainCarriage()
        {
            this.ValidateEstimatedLegDates("Main-Carriage", shipmentPM.MainCarriageETD, shipmentPM.MainCarriageETA);
            this.ValidateActualLegDates("Main-Carriage", shipmentPM.MainCarriageATD, shipmentPM.MainCarriageATA);
            this.ValidateMainCarriage_PreviousLeg();
            this.ValidateMainCarriage_NextLeg();
        }
        private void ValidateMainCarriage_PreviousLeg()
        {
            if (isWarehouse1LegExists)
                this.ValidateMainCarriage_Previous_Warehouse1();
        }
       
        private void ValidateMainCarriage_Previous_Warehouse1()
        {
            if (IsDateSmaller(shipmentPM.MainCarriageETD, shipmentPM.WarehouseLegExpectedReleaseDate))            
                throw new ApplicationException("Main-Carriage expected departure must be bigger than Warehouse expected release");            

            if (IsDateSmaller(shipmentPM.MainCarriageATD, shipmentPM.WarehouseLegActualReleaseDate))            
                throw new ApplicationException("Main-Carriage actual departure must be bigger than Warehouse actual release");            
        }
        private void ValidateMainCarriage_NextLeg()
        {
            if (isTransshipment1Exists)
                this.ValidateMainCarriage_Next_Trans1();

            else if (isTransshipment2Exists)
                this.ValidateMainCarriage_Next_Trans2();

            else if (isTransshipment3Exists)
                this.ValidateMainCarriage_Next_Trans3();

            else if (isOnCarriageExists)
                this.ValidateMainCarriage_Next_OnCarriage();

            else if (isOnForwardingExists)
                this.ValidateMainCarriage_Next_OnForwarding();

            else if (isDestinationWarehouseLegExists)
                this.ValidateMainCarriage_Next_DestinationWarehouse();

            else if (isDestinationWarehouse2LegExists)
                this.ValidateMainCarriage_Next_DestinationWarehouse2();
        }
        private void ValidateMainCarriage_Next_Trans1()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.Transshipment1ETD))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than Via1 expected departure");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.Transshipment1ATD))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than Via1 actual departure");            
        }
        private void ValidateMainCarriage_Next_Trans2()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.Transshipment2ETD))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than Via2 expected departure");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.Transshipment2ATD))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than Via2 actual departure");            
        }
        private void ValidateMainCarriage_Next_Trans3()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.Transshipment3ETD))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than Via3 expected departure");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.Transshipment3ATD))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than Via3 actual departure");            
        }
        private void ValidateMainCarriage_Next_OnCarriage()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.OnCarriageETD))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than On-Carriage expected departure");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.OnCarriageATD))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than On-Carriage actual departure");            
        }
        private void ValidateMainCarriage_Next_OnForwarding()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.OnForwardingETD))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than On-Carriage expected departure");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.OnForwardingATD))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than On-Carriage actual departure");            
        }
        private void ValidateMainCarriage_Next_DestinationWarehouse()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.WarehouseLegExpectedEntryDate))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than Warehouse expected entry");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.WarehouseLegActualEntryDate))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than Warehouse actual entry");            
        }
        private void ValidateMainCarriage_Next_DestinationWarehouse2()
        {
            if (IsDateBigger(shipmentPM.MainCarriageETA, shipmentPM.WarehouseLeg2ExpectedEntryDate))            
                throw new ApplicationException("Main-Carriage expected arrival must be less than Destination Warehouse expected entry");            

            if (IsDateBigger(shipmentPM.MainCarriageATA, shipmentPM.WarehouseLegActualEntryDate))            
                throw new ApplicationException("Main-Carriage actual arrival must be less than Warehouse actual entry");            
        }
        
        private void ValidateTransshipment1()
        {
            if (!isTransshipment1Exists) return;
            this.ValidateEstimatedLegDates("Via1", shipmentPM.Transshipment1ETD, shipmentPM.Transshipment1ETA);
            this.ValidateActualLegDates("Via1", shipmentPM.Transshipment1ATD, shipmentPM.Transshipment1ATA);
            this.ValidateMainTransshipment1_NextLeg();
        }
        private void ValidateMainTransshipment1_NextLeg()
        {
            if (isTransshipment2Exists)
                this.ValidateTransshipment1_Next_Trans2();

            else if (isTransshipment3Exists)            
                this.ValidateTransshipment1_Next_Trans3();                
            
            else if (isOnCarriageExists)            
                this.ValidateTransshipment1_Next_OnCarriage();               
            
            else if (isOnForwardingExists)            
                this.ValidateTransshipment1_Next_OnForwarding();                
            
            else if (isDestinationWarehouseLegExists)
                this.ValidateTransshipment1_Next_DestinationWarehouse();

            else if (isDestinationWarehouse2LegExists)            
                this.ValidateTransshipment1_Next_DestinationWarehouse2();             
        }
        private void ValidateTransshipment1_Next_Trans2()
        {
            if (IsDateBigger(shipmentPM.Transshipment1ETA, shipmentPM.Transshipment2ETD))            
                throw new ApplicationException("Via1 expected arrival must be less than Via2 expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment1ATA, shipmentPM.Transshipment2ATD))            
                throw new ApplicationException("Via1 actual arrival must be less than Via2 actual departure");            
        }
        private void ValidateTransshipment1_Next_Trans3()
        {
            if (IsDateBigger(shipmentPM.Transshipment1ETA, shipmentPM.Transshipment3ETD))            
                throw new ApplicationException("Via1 expected arrival must be less than Via3 expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment1ATA, shipmentPM.Transshipment3ATD))            
                throw new ApplicationException("Via1 actual arrival must be less than Via3 actual departure");            
        }
        private void ValidateTransshipment1_Next_OnCarriage()
        {
            if (IsDateBigger(shipmentPM.Transshipment1ETA, shipmentPM.OnCarriageETD))            
                throw new ApplicationException("Via1 expected arrival must be less than On-Carriage expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment1ATA, shipmentPM.OnCarriageATD))            
                throw new ApplicationException("Via1 actual arrival must be less than On-Carriage actual departure");            
        }
        private void ValidateTransshipment1_Next_OnForwarding()
        {
            if (IsDateBigger(shipmentPM.Transshipment1ETA, shipmentPM.OnForwardingETD))            
                throw new ApplicationException("Via1 expected arrival must be less than On-Forwarding expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment1ATA, shipmentPM.OnForwardingATD))           
                throw new ApplicationException("Via1 actual arrival must be less than On-Forwarding actual departure");            
        }
        private void ValidateTransshipment1_Next_DestinationWarehouse()
        {
            if (IsDateBigger(shipmentPM.Transshipment1ETA, shipmentPM.WarehouseLegExpectedEntryDate))            
                throw new ApplicationException("Via1 expected arrival must be less than Warehouse expected entry");            

            if (IsDateBigger(shipmentPM.Transshipment1ATA, shipmentPM.WarehouseLegActualEntryDate))            
                throw new ApplicationException("Via1 actual arrival must be less than Warehouse actual entry");            
        }
        private void ValidateTransshipment1_Next_DestinationWarehouse2()
        {
            if (IsDateBigger(shipmentPM.Transshipment1ETA, shipmentPM.WarehouseLeg2ExpectedEntryDate))            
                throw new ApplicationException("Via1 expected arrival must be less than Destination Warehouse expected entry");            

            if (IsDateBigger(shipmentPM.Transshipment1ATA, shipmentPM.WarehouseLeg2ActualEntryDate))            
                throw new ApplicationException("Via1 actual arrival must be less than Destination Warehouse actual entry");            
        }
       
        private void ValidateTransshipment2()
        {
            if (!isTransshipment2Exists) return;
            this.ValidateEstimatedLegDates("Via2", shipmentPM.Transshipment2ETD, shipmentPM.Transshipment2ETA);
            this.ValidateActualLegDates("Via2", shipmentPM.Transshipment2ATD, shipmentPM.Transshipment2ATA);
            this.ValidateTransshipment2_NextLeg();            
        }
        private void ValidateTransshipment2_NextLeg()
        {
            if (isTransshipment3Exists)            
                this.ValidateTransshipment2_Next_Trans3();                
            
            else if (isOnCarriageExists)
                this.ValidateTransshipment2_Next_OnCarriage();

            else if (isOnForwardingExists)
                this.ValidateTransshipment2_Next_OnForwarding();

            else if (isDestinationWarehouseLegExists)            
                this.ValidateTransshipment2_Next_DestinationWarehouse();                
            
            else if (isDestinationWarehouse2LegExists)            
                this.ValidateTransshipment2_Next_DestinationWarehouse2();
        }
        private void ValidateTransshipment2_Next_Trans3()
        {
            if (IsDateBigger(shipmentPM.Transshipment2ETA, shipmentPM.Transshipment3ETD))            
                throw new ApplicationException("Via2 expected arrival must be less than Via3 expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment2ATA, shipmentPM.Transshipment3ATD))            
                throw new ApplicationException("Via2 actual arrival must be less than Via3 actual departure");            
        }
        private void ValidateTransshipment2_Next_OnCarriage()
        {
            if (IsDateBigger(shipmentPM.Transshipment2ETA, shipmentPM.OnCarriageETD))            
                throw new ApplicationException("Via2 expected arrival must be less than On-Carriage expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment2ATA, shipmentPM.OnCarriageATD))            
                throw new ApplicationException("Via2 actual arrival must be less than On-Carriage actual departure");            
        }
        private void ValidateTransshipment2_Next_OnForwarding()
        {
            if (IsDateBigger(shipmentPM.Transshipment2ETA, shipmentPM.OnForwardingETD))            
                throw new ApplicationException("Via2 expected arrival must be less than On-Forwarding expected departure");            

            if (IsDateBigger(shipmentPM.Transshipment2ATA, shipmentPM.OnForwardingATD))            
                throw new ApplicationException("Via2 actual arrival must be less than On-Forwarding actual departure");            
        }
        private void ValidateTransshipment2_Next_DestinationWarehouse()
        {
            if (IsDateBigger(shipmentPM.Transshipment2ETA, shipmentPM.WarehouseLegExpectedEntryDate))            
                throw new ApplicationException("Via2 expected arrival must be less than Warehouse expected entry");            

            if (IsDateBigger(shipmentPM.Transshipment2ATA, shipmentPM.WarehouseLegActualEntryDate))            
                throw new ApplicationException("Via2 actual arrival must be less than Warehouse actual entry");            
        }
        private void ValidateTransshipment2_Next_DestinationWarehouse2()
        {
            if (IsDateBigger(shipmentPM.Transshipment2ETA, shipmentPM.WarehouseLeg2ExpectedEntryDate))            
                throw new ApplicationException("Via2 expected arrival must be less than Destination Warehouse expected entry");            

            if (IsDateBigger(shipmentPM.Transshipment2ATA, shipmentPM.WarehouseLeg2ActualEntryDate))            
                throw new ApplicationException("Via2 actual arrival must be less than Destination Warehouse actual entry");            
        }        
        private void ValidateTransshipment3()
        {
            if (!isTransshipment3Exists) return;
            this.ValidateEstimatedLegDates("Via3", shipmentPM.Transshipment3ETD, shipmentPM.Transshipment3ETA);
            this.ValidateActualLegDates("Via3", shipmentPM.Transshipment3ATD, shipmentPM.Transshipment3ATA);
            this.ValidateTransshipment3_NextLeg(); 
        }
        private void ValidateTransshipment3_NextLeg()
        {
            if (isOnCarriageExists)            
                this.ValidateTransshipment3_Next_OnCarriage();               

            else if (isOnForwardingExists)            
                this.ValidateTransshipment3_Next_OnForwarding();
            
            else if (isDestinationWarehouseLegExists)            
                this.ValidateTransshipment3_Next_DestinationWarehouse();
            
            else if (isDestinationWarehouse2LegExists)
                this.ValidateTransshipment3_Next_DestinationWarehouse2();             
        }
        private void ValidateTransshipment3_Next_OnCarriage()
        {
            if (IsDateBigger(shipmentPM.Transshipment3ETA, shipmentPM.OnCarriageETD))
                throw new ApplicationException("Via3 expected arrival must be less than On-Carriage expected departure");

            if (IsDateBigger(shipmentPM.Transshipment3ATA, shipmentPM.OnCarriageATD))
                throw new ApplicationException("Via3 actual arrival must be less than On-Carriage actual departure");
        }
        private void ValidateTransshipment3_Next_OnForwarding()
        {
            if (IsDateBigger(shipmentPM.Transshipment3ETA, shipmentPM.OnForwardingETD))
                throw new ApplicationException("Via3 expected arrival must be less than On-Forwarding expected departure");

            if (IsDateBigger(shipmentPM.Transshipment3ATA, shipmentPM.OnForwardingATD))
                throw new ApplicationException("Via3 actual arrival must be less than On-Forwarding actual departure");
        }
        private void ValidateTransshipment3_Next_DestinationWarehouse()
        {
            if (IsDateBigger(shipmentPM.Transshipment3ETA, shipmentPM.WarehouseLegExpectedEntryDate))
                throw new ApplicationException("Via3 expected arrival must be less than Warehouse expected entry");

            if (IsDateBigger(shipmentPM.Transshipment3ATA, shipmentPM.WarehouseLegActualEntryDate))
                throw new ApplicationException("Via3 actual arrival must be less than Warehouse actual entry");
        }
        private void ValidateTransshipment3_Next_DestinationWarehouse2()
        {
            if (IsDateBigger(shipmentPM.Transshipment3ETA, shipmentPM.WarehouseLeg2ExpectedEntryDate))
                throw new ApplicationException("Via3 expected arrival must be less than Destination Warehouse expected entry");

            if (IsDateBigger(shipmentPM.Transshipment3ATA, shipmentPM.WarehouseLeg2ActualEntryDate))
                throw new ApplicationException("Via3 actual arrival must be less than Destination Warehouse actual entry");
        }
        private void ValidateOnCarriage()
        {
            if (!isOnCarriageExists) return;
            this.ValidateEstimatedLegDates("On-Carriage", shipmentPM.OnCarriageETD, shipmentPM.OnCarriageETA);
            this.ValidateActualLegDates("On-Carriage", shipmentPM.OnCarriageATD, shipmentPM.OnCarriageATA);
            this.ValidateOnCarriage_NextLeg();
        }
        
        private void ValidateOnCarriage_NextLeg()
        {
            if (isOnForwardingExists)            
                this.ValidateOnCarriage_Next_OnForwarding();
            
            else if (isDestinationWarehouseLegExists)            
                this.ValidateOnCarriage_Next_DestinationWarehouse();

            else if (isDestinationWarehouse2LegExists)            
                this.ValidateOnCarriage_Next_DestinationWarehouse2();            
        }
        private void ValidateOnCarriage_Next_OnForwarding()
        {
            if (IsDateBigger(shipmentPM.OnCarriageETA, shipmentPM.OnForwardingETD))
                throw new ApplicationException("On-Carriage expected arrival must be less than On-Forwarding expected departure");

            if (IsDateBigger(shipmentPM.OnCarriageATA, shipmentPM.OnForwardingATD))
                throw new ApplicationException("On-Carriage actual arrival must be less than On-Forwarding actual departure");
        }
        private void ValidateOnCarriage_Next_DestinationWarehouse()
        {
            if (IsDateBigger(shipmentPM.OnCarriageETA, shipmentPM.WarehouseLegExpectedEntryDate))
                throw new ApplicationException("On-Carriage expected arrival must be less than Warehouse expected entry");

            if (IsDateBigger(shipmentPM.OnCarriageATA, shipmentPM.WarehouseLegActualEntryDate))
                throw new ApplicationException("On-Carriage actual arrival must be less than Warehouse actual entry");
        }
        private void ValidateOnCarriage_Next_DestinationWarehouse2()
        {
            if (IsDateBigger(shipmentPM.OnCarriageETA, shipmentPM.WarehouseLeg2ExpectedEntryDate))
                throw new ApplicationException("On-Carriage expected arrival must be less than Destination Warehouse expected entry");

            if (IsDateBigger(shipmentPM.OnCarriageATA, shipmentPM.WarehouseLeg2ActualEntryDate))
                throw new ApplicationException("On-Carriage actual arrival must be less than Destination Warehouse actual entry");
        }
        
        private void ValidateOnForwarding()
        {
            if (!isOnForwardingExists) return;
            this.ValidateEstimatedLegDates("On-Forwarding", shipmentPM.OnForwardingETD, shipmentPM.OnForwardingETA);
            this.ValidateActualLegDates("On-Forwarding", shipmentPM.OnForwardingATD, shipmentPM.OnForwardingATA);
            this.ValidateOnForwarding_NextLeg(); 
        }
        
        private void ValidateOnForwarding_NextLeg()
        {
            if (isDestinationWarehouseLegExists)
                this.ValidateOnForwarding_Next_DestinationWarehouse();

            if (isDestinationWarehouse2LegExists)
                this.ValidateOnForwarding_Next_DestinationWarehouse2();
        }
        private void ValidateOnForwarding_Next_DestinationWarehouse()
        {
            if (IsDateBigger(shipmentPM.OnForwardingETA, shipmentPM.WarehouseLegExpectedEntryDate))
                throw new ApplicationException("On-Forwarding expected arrival must be less than Warehouse expected entry");

            if (IsDateBigger(shipmentPM.OnForwardingATA, shipmentPM.WarehouseLegActualEntryDate))
                throw new ApplicationException("On-Forwarding actual arrival must be less than Warehouse actual entry");
        }
        private void ValidateOnForwarding_Next_DestinationWarehouse2()
        {
            if (IsDateBigger(shipmentPM.OnForwardingETA, shipmentPM.WarehouseLeg2ExpectedEntryDate))
                throw new ApplicationException("On-Forwarding expected arrival must be less than Destination Warehouse expected entry");

            if (IsDateBigger(shipmentPM.OnForwardingATA, shipmentPM.WarehouseLeg2ActualEntryDate))
                throw new ApplicationException("On-Forwarding actual arrival must be less than Destination Warehouse actual entry");
        }
        
        private void ValidateWarehouse()
        {
            if (!isWarehouseLegExists) return;
            this.ValidateEstimatedLegDates("Warehouse", shipmentPM.WarehouseLegExpectedEntryDate, shipmentPM.WarehouseLegExpectedReleaseDate);
            this.ValidateActualLegDates("Warehouse", shipmentPM.WarehouseLegActualEntryDate, shipmentPM.WarehouseLegActualReleaseDate);
        }
        private void ValidateDestinationWarehouse()
        {
            if (!isWarehouseLeg2Exists) return;
            this.ValidateEstimatedLegDates("Destination Warehouse", shipmentPM.WarehouseLeg2ExpectedEntryDate, shipmentPM.WarehouseLeg2ExpectedReleaseDate);
            this.ValidateActualLegDates("Destination Warehouse", shipmentPM.WarehouseLeg2ActualEntryDate, shipmentPM.WarehouseLeg2ActualReleaseDate);
        }
        
        private void ValidateEstimatedLegDates(string legCode, DateTime? ETD, DateTime? ETA)
        {
            if (!IsRoutingLegDatesValid(ETD, ETA))
            {
                throw new ApplicationException(legCode + " expected departure must be less than " + legCode + " expected arrival");
            }
        }
        private void ValidateActualLegDates(string legCode, DateTime? ATD, DateTime? ATA)
        {
            if (!IsRoutingLegDatesValid(ATD, ATA))
            {
                throw new ApplicationException(legCode + " actual departure must be less than " + legCode + " actual arrival");
            }
        }
        public static bool IsActualDateValid(DateTime? myDate, int tenant)
        {
            if (myDate == null) return true;

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddHours(24);
            if (myDate > todayDate)
            {
                return false;
            }

            return true;
        }
        public static bool IsRoutingLegDatesValid(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return true;

            if (date1 > date2.Value.AddHours(24))
            {
                return false;
            }

            return true;
        }
        public static bool IsDateSeriesBiggerNotEqual(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return false;

            if (date1 > date2)
            {
                return true;
            }

            return false;
        }
        public static bool IsDateBigger(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return false;

            if (date1 >= date2)
            {
                return true;
            }

            return false;
        }
        public static bool IsDateSmaller(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return false;

            if (date1 <= date2)
            {
                return true;
            }

            return false;
        }
    }
}
