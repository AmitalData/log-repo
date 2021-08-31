using System;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.ShipmentsModel.Repositories;


namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        public void TracePickUp(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO, ShipmentPM shipmentPM)
        {
            //if (!entityPM.IsHybrid)
            //{
            string DepartedCode = "PICD";
            if (RoutingDate.IsDateAddedOrModified(itemPM.ATD, itemPOCO.ATD))
            {
                this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
            }

            else if (RoutingDate.IsDateRemoved(itemPM.ATD, itemPOCO.ATD))
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else
            {
                if (itemPM.ATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        if (itemPM.PickUpDeliveryFromTypeCode != itemPOCO.PickUpDeliveryFromTypeCode)
                        {
                            this.UpdateLocation(DepartedCode);
                        }

                        else
                        {
                            switch (itemPM.PickUpDeliveryFromTypeCode)
                            {
                                case "PART":
                                    {
                                        if (itemPM.FromAddressId != itemPOCO.FromAddressId)
                                        {
                                            this.UpdateLocation(DepartedCode);
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        if (itemPM.FromPortId != itemPOCO.FromPortId)
                                        {
                                            this.UpdateLocation(DepartedCode);
                                        }

                                        break;
                                    }

                                case "CASL":
                                    {
                                        if (itemPM.FromAddressCity != itemPOCO.FromAddressCity)
                                        {
                                            this.UpdateLocation(DepartedCode);
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            string ArrivedCode = "RCS";
            string ArrivedCode_New = "PIAR";
            if (RoutingDate.IsDateAddedOrModified(itemPM.ATA, itemPOCO.ATA))
            {
                this.DeleteTraceEvent(ArrivedCode);
                this.CreateTraceEvent(ArrivedCode_New, itemPM.ATA, itemPM);

                ShipmentPickUpPM lastPickup = entityPM.ShipmentPickUps.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                if (RoutingDate.IsAllPickupsHaveDates(entityPM) && lastPickup != null)
                {
                    this.CreateTraceEvent(ArrivedCode, lastPickup.ATA, lastPickup);
                }
            }

            else if (RoutingDate.IsDateRemoved(itemPM.ATA, itemPOCO.ATA))
            {
                this.DeleteTraceEvent(ArrivedCode);
                this.DeleteTraceEvent(ArrivedCode_New, itemPM.PickUpDeliveryNumber);
            }

            else
            {
                if (itemPM.ATA != null)
                {
                    if (IsCurrentStatus(ArrivedCode))
                    {
                        if (itemPM.PickUpDeliveryToTypeCode != itemPOCO.PickUpDeliveryToTypeCode)
                        {
                            this.UpdateLocation(ArrivedCode);
                        }

                        else
                        {
                            switch (itemPM.PickUpDeliveryToTypeCode)
                            {
                                case "PART":
                                    {
                                        if (itemPM.ToAddressId != itemPOCO.ToAddressId)
                                        {
                                            this.UpdateLocation(ArrivedCode);
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        if (itemPM.ToPortId != itemPOCO.ToPortId)
                                        {
                                            this.UpdateLocation(ArrivedCode);
                                        }

                                        break;
                                    }

                                case "CASL":
                                    {
                                        if (itemPM.ToAddressCity != itemPOCO.ToAddressCity)
                                        {
                                            this.UpdateLocation(ArrivedCode);
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }

                else
                {
                    this.DeleteTraceEvent(ArrivedCode);
                }
            }
            //}

            this.TracePickUpArrangedEvent(itemPM, itemPOCO, shipmentPM);
        }
        public void TraceDelivery(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO, ShipmentPM shipmentPM)
        {
            //if (!entityPM.IsHybrid)
            //{
            if (itemPM.PickUpDeliveryTypeCode == "DELV")
            {
                string DepartedCode = "DELD";
                if (RoutingDate.IsDateAddedOrModified(itemPM.ATD, itemPOCO.ATD))
                {
                    this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                }

                else if (RoutingDate.IsDateRemoved(itemPM.ATD, itemPOCO.ATD))
                {
                    this.DeleteTraceEvent(DepartedCode);
                }

                else
                {
                    if (itemPM.ATD != null)
                    {
                        if (IsCurrentStatus(DepartedCode))
                        {
                            if (itemPM.PickUpDeliveryFromTypeCode != itemPOCO.PickUpDeliveryFromTypeCode)
                            {
                                this.UpdateLocation(DepartedCode);
                                //this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                            }

                            else
                            {
                                switch (itemPM.PickUpDeliveryFromTypeCode)
                                {
                                    case "PART":
                                        {
                                            if (itemPM.FromAddressId != itemPOCO.FromAddressId)
                                            {
                                                this.UpdateLocation(DepartedCode);
                                                //this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                                            }

                                            break;
                                        }

                                    case "PORT":
                                        {
                                            if (itemPM.FromPortId != itemPOCO.FromPortId)
                                            {
                                                this.UpdateLocation(DepartedCode);
                                                //this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                                            }

                                            break;
                                        }

                                    case "CASL":
                                        {
                                            if (itemPM.FromAddressCity != itemPOCO.FromAddressCity)
                                            {
                                                this.UpdateLocation(DepartedCode);
                                                //this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                                            }

                                            break;
                                        }
                                }
                            }
                        }
                    }
                }

                string ArrivedCode = "PIOD";
                string ArrivedCode_New = "DEAR";
                if (RoutingDate.IsDateAddedOrModified(itemPM.ATA, itemPOCO.ATA))
                {
                    this.DeleteTraceEvent(ArrivedCode);
                    this.CreateTraceEvent(ArrivedCode_New, itemPM.ATA, itemPM);

                    ShipmentDeliveryPM lastDelivery = entityPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    if (RoutingDate.IsAllDeliveriesHaveDates(entityPM) && lastDelivery != null)
                    {
                        this.CreateTraceEvent(ArrivedCode, lastDelivery.ATA, lastDelivery);
                    }
                }

                else if (RoutingDate.IsDateRemoved(itemPM.ATA, itemPOCO.ATA))
                {
                    this.DeleteTraceEvent(ArrivedCode);
                    this.DeleteTraceEvent(ArrivedCode_New, itemPM.PickUpDeliveryNumber);
                }

                else
                {
                    if (itemPM.ATA != null)
                    {
                        if (IsCurrentStatus(ArrivedCode))
                        {
                            if (itemPM.PickUpDeliveryToTypeCode != itemPOCO.PickUpDeliveryToTypeCode)
                            {
                                this.UpdateLocation(ArrivedCode);
                            }

                            else
                            {
                                switch (itemPM.PickUpDeliveryToTypeCode)
                                {
                                    case "PART":
                                        {
                                            if (itemPM.ToAddressId != itemPOCO.ToAddressId)
                                            {
                                                this.UpdateLocation(ArrivedCode);
                                            }

                                            break;
                                        }

                                    case "PORT":
                                        {
                                            if (itemPM.ToPortId != itemPOCO.ToPortId)
                                            {
                                                this.UpdateLocation(ArrivedCode);
                                            }

                                            break;
                                        }

                                    case "CASL":
                                        {
                                            if (itemPM.ToAddressCity != itemPOCO.ToAddressCity)
                                            {
                                                this.UpdateLocation(ArrivedCode);
                                            }

                                            break;
                                        }
                                }
                            }
                        }
                    }

                    else
                    {
                        this.DeleteTraceEvent(ArrivedCode);
                    }
                }

                this.TraceDeliveryArrangedEvent(itemPM, itemPOCO, shipmentPM);
            }
            // }
        }
        public void TraceDeletedPickUp(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO, ShipmentPM shipmentPM)
        {
            //if (!entityPM.IsHybrid)
            //{
            if (itemPOCO.PickUpDeliveryTypeCode == "PICK")
            {
                if (itemPOCO.ATA != null)
                {
                    this.DeleteTraceEvent("RCS");
                    this.DeleteTraceEvent("PIAR", itemPOCO.PickUpDeliveryNumber);

                    ShipmentPickUpPM lastPickup = entityPM.ShipmentPickUps.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    if (RoutingDate.IsAllPickupsHaveDates(entityPM) && lastPickup != null)
                    {
                        this.CreateTraceEvent("RCS", lastPickup.ATA, lastPickup);
                    }
                }

                if (itemPOCO.ATD != null)
                {
                    this.DeleteTraceEvent("PICD");
                }

                if (IsFirstPickup(itemPM, shipmentPM) && (itemPOCO.ETA != null || itemPOCO.ETD != null))
                {
                    this.DeleteTraceEvent("PCAR");
                }
            }
            //}
        }
        public void TraceDeletedDelivery(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO, ShipmentPM shipmentPM)
        {
            //if (!entityPM.IsHybrid)
            //{
            if (itemPM.PickUpDeliveryTypeCode == "DELV")
            {
                if (itemPOCO.ATA != null)
                {
                    this.DeleteTraceEvent("PIOD");
                    this.DeleteTraceEvent("DEAR", itemPOCO.PickUpDeliveryNumber);

                    ShipmentDeliveryPM lastDelivery = entityPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    if (RoutingDate.IsAllDeliveriesHaveDates(entityPM) && lastDelivery != null)
                    {
                        this.CreateTraceEvent("PIOD", lastDelivery.ATA, lastDelivery);
                    }
                }

                if (itemPOCO.ATD != null)
                {
                    this.DeleteTraceEvent("DELD");
                }
                if (IsFirstDelivery(itemPM, shipmentPM) && (itemPOCO.ETA != null || itemPOCO.ETD != null))
                {
                    this.DeleteTraceEvent("DLAR");
                }
            }
            //}
        }
        private string GetConnectedStandaloneShipmentNotes()
        {
            string eventNotes = null;

            if (!string.IsNullOrEmpty(entityPM.StandalonePickupDeliveryId))
            {
                ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(tenant);
                ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, entityPM.StandalonePickupDeliveryId);
                if (shipmentPickUpDelivery != null)
                {
                    eventNotes = "Conncted To " + (shipmentPickUpDelivery.PickUpDeliveryTypeCode == "PICK" ? "pickup: " : "delivery: ") + shipmentPickUpDelivery.PickUpDeliveryNumber;
                }
            }
            return eventNotes;
        }
        private void TraceDeliveryArrangedEvent(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO, ShipmentPM shipmentPM)
        {
            if (!IsFirstDelivery(itemPM, shipmentPM))
            {
                return;
            }

            this.TraceAddDeliveryArrangedEvent(itemPM, itemPOCO);
            this.TraceUpdateDeliveryArrangedEvent(itemPM, itemPOCO);
            this.TraceDeleteDeliveryArrangedEvent(itemPM, itemPOCO);
        }
        private void TracePickUpArrangedEvent(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO, ShipmentPM shipmentPM)
        {
            if (!IsFirstPickup(itemPM, shipmentPM))
            {
                return;
            }
            this.TraceAddPickUpArrangedEvent(itemPM, itemPOCO);
            this.TraceUpdatePickUpArrangedEvent(itemPM, itemPOCO);
            this.TraceDeletePickUpArrangedEvent(itemPM, itemPOCO);
        }
        private bool IsFirstPickup(ShipmentPickUpPM itemPM, ShipmentPM shipmentPM)
        {
            ShipmentPickUpPM firstPickup =
                            (from d in shipmentPM.ShipmentPickUps
                             select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (firstPickup == null)
            {
                return false;
            }
            if (itemPM.PickUpDeliveryNumber == firstPickup.PickUpDeliveryNumber)
            {
                return true;
            }

            return false;
        }
        private bool IsFirstDelivery(ShipmentDeliveryPM itemPM, ShipmentPM shipmentPM)
        {
            ShipmentDeliveryPM firstDelivery =
                            (from d in shipmentPM.ShipmentDeliveries
                             select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
            if (firstDelivery == null)
            {
                return false;
            }
            if (itemPM.PickUpDeliveryNumber == firstDelivery.PickUpDeliveryNumber)
            {
                return true;
            }

            return false;
        }
        private void TraceAddDeliveryArrangedEvent(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsETALegDateAdded(null, itemPM, itemPOCO)
                && !IsEventExistInTraceEvents("DLAR", itemPOCO.ETD) &&
                !IsEventExistInTraceEvents("DLAR", itemPOCO.ETA))
            {
                this.CreateTraceEvent("DLAR", itemPM.ETA, itemPM);
            }
            else if (IsETDLegDateAdded(null, itemPM, itemPOCO)
                && !IsEventExistInTraceEvents("DLAR", itemPOCO.ETD) &&
                !IsEventExistInTraceEvents("DLAR", itemPOCO.ETA))
            {
                this.CreateTraceEvent("DLAR", itemPM.ETD, itemPM);
            }
            else if (IsETAOrETAEditedAfterDeleteFirstDeilvery(itemPM, itemPOCO))
            {
                this.DeleteTraceEvent("DLAR");
                this.CreateTraceEvent("DLAR", itemPM.ETA != null ? itemPM.ETA : itemPM.ETD, itemPM);
            }
        }
        private void TraceAddPickUpArrangedEvent(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsETALegDateAdded(itemPM, null, itemPOCO)
                && !IsEventExistInTraceEvents("PCAR", itemPOCO.ETD) &&
                !IsEventExistInTraceEvents("PCAR", itemPOCO.ETA))
            {
                this.CreateTraceEvent("PCAR", itemPM.ETA, itemPM);
            }
            else if (IsETDLegDateAdded(itemPM, null, itemPOCO)
                && !IsEventExistInTraceEvents("PCAR", itemPOCO.ETD) &&
                !IsEventExistInTraceEvents("PCAR", itemPOCO.ETA))
            {
                this.CreateTraceEvent("PCAR", itemPM.ETD, itemPM);
            }
            else if (IsETAOrETAEditedAfterDeleteFirstPickup(itemPM, itemPOCO))
            {
                this.DeleteTraceEvent("PCAR");
                this.CreateTraceEvent("PCAR", itemPM.ETA != null ? itemPM.ETA : itemPM.ETD, itemPM);
            }
        }
        private void TraceUpdateDeliveryArrangedEvent(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsETALegDateEdited(null, itemPM, itemPOCO) && IsEventExistInTraceEvents("DLAR", itemPOCO.ETA))
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETA);
                this.CreateTraceEvent("DLAR", itemPM.ETA, itemPM);
            }
            else if (IsETDLegDateEdited(null, itemPM, itemPOCO) && IsEventExistInTraceEvents("DLAR", itemPOCO.ETD))
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETD);
                this.CreateTraceEvent("DLAR", itemPM.ETD, itemPM);
            }
        }
        private void TraceUpdatePickUpArrangedEvent(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsETALegDateEdited(itemPM, null, itemPOCO) && IsEventExistInTraceEvents("PCAR", itemPOCO.ETA))
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETA);
                this.CreateTraceEvent("PCAR", itemPM.ETA, itemPM);
            }
            else if (IsETDLegDateEdited(itemPM, null, itemPOCO) && IsEventExistInTraceEvents("PCAR", itemPOCO.ETD))
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETD);
                this.CreateTraceEvent("PCAR", itemPM.ETD, itemPM);
            }
            
        }
        private void TraceDeleteDeliveryArrangedEvent(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsETALegDateDeleted(null, itemPM, itemPOCO) && !IsETDLegDateDeleted(null, itemPM, itemPOCO))
            {
                this.TraceETALegDateDeliveryArrangedEvent(itemPM, itemPOCO);
            }
            else if (IsETDLegDateDeleted(null, itemPM, itemPOCO) && !IsETALegDateDeleted(null, itemPM, itemPOCO))
            {
                this.TraceETDLegDateDeliveryArrangedEvent(itemPM, itemPOCO);
            }
            else if (IsETDLegDateDeleted(null, itemPM, itemPOCO) && IsETALegDateDeleted(null, itemPM, itemPOCO))
            {
                this.DeleteTraceEvent("DLAR");
            }
        }
        private void TraceDeletePickUpArrangedEvent(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsETALegDateDeleted(itemPM, null, itemPOCO) && !IsETDLegDateDeleted(itemPM, null, itemPOCO))
            {
                this.TraceETALegDatePickUpArrangedEvent(itemPM, itemPOCO);
            }
            else if (IsETDLegDateDeleted(itemPM, null, itemPOCO) && !IsETALegDateDeleted(itemPM, null, itemPOCO))
            {
                this.TraceETDLegDatePickUpArrangedEvent(itemPM, itemPOCO);
            }
            else if (IsETDLegDateDeleted(itemPM, null, itemPOCO) && IsETALegDateDeleted(itemPM, null, itemPOCO))
            {
                this.DeleteTraceEvent("PCAR");
            }
        }
        private void TraceETALegDateDeliveryArrangedEvent(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsEventExistInTraceEvents("DLAR", itemPOCO.ETA) && (IsETDLegDateAdded(null, itemPM, itemPOCO) || IsETDLegDateEdited(null, itemPM, itemPOCO)))
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETA);
                this.CreateTraceEvent("DLAR", itemPM.ETD, itemPM);
            }
            else if (IsEventExistInTraceEvents("DLAR", itemPOCO.ETA) && itemPOCO.ETD != null)
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETA);
                this.CreateTraceEvent("DLAR", itemPOCO.ETD, itemPM);
            }
            else
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETA);
            }
        }
        private void TraceETDLegDateDeliveryArrangedEvent(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsEventExistInTraceEvents("DLAR", itemPOCO.ETD) && (IsETALegDateAdded(null, itemPM, itemPOCO) || IsETALegDateEdited(null, itemPM, itemPOCO)))
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETD);
                this.CreateTraceEvent("DLAR", itemPM.ETA, itemPM);
            }
            else if (IsEventExistInTraceEvents("DLAR", itemPOCO.ETD) && itemPOCO.ETA != null)
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETD);
                this.CreateTraceEvent("DLAR", itemPOCO.ETA, itemPM);
            }
            else
            {
                this.DeleteTraceEvent("DLAR", null, itemPOCO.ETD);
            }
        }
        private void TraceETALegDatePickUpArrangedEvent(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsEventExistInTraceEvents("PCAR", itemPOCO.ETA) && (IsETDLegDateAdded(itemPM, null, itemPOCO) || IsETDLegDateEdited(itemPM, null, itemPOCO)))
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETA);
                this.CreateTraceEvent("PCAR", itemPM.ETD, itemPM);
            }
            else if (IsEventExistInTraceEvents("PCAR", itemPOCO.ETA) && itemPOCO.ETD != null)
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETA);
                this.CreateTraceEvent("PCAR", itemPOCO.ETD, itemPM);
            }
            else
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETA);
            }
        }
        private void TraceETDLegDatePickUpArrangedEvent(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (IsEventExistInTraceEvents("PCAR", itemPOCO.ETD) && (IsETALegDateAdded(itemPM, null, itemPOCO) || IsETALegDateEdited(itemPM, null, itemPOCO)))
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETD);
                this.CreateTraceEvent("PCAR", itemPM.ETA, itemPM);
            }
            else if (IsEventExistInTraceEvents("PCAR", itemPOCO.ETD) && itemPOCO.ETA != null)
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETD);
                this.CreateTraceEvent("PCAR", itemPOCO.ETA, itemPM);
            }
            else
            {
                this.DeleteTraceEvent("PCAR", null, itemPOCO.ETD);
            }
        }
        private bool IsETALegDateAdded(ShipmentPickUpPM shipmentPickUpPM, ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (shipmentDeliveryPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateAdded(shipmentDeliveryPM.ETA, shipmentPickUpDelivery.ETA));
            }
            else if (shipmentPickUpPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateAdded(shipmentPickUpPM.ETA, shipmentPickUpDelivery.ETA));
            }

            return false;
        }
        private bool IsETALegDateEdited(ShipmentPickUpPM shipmentPickUpPM, ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (shipmentDeliveryPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateEdited(shipmentDeliveryPM.ETA, shipmentPickUpDelivery.ETA));
            }
            else if (shipmentPickUpPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateEdited(shipmentPickUpPM.ETA, shipmentPickUpDelivery.ETA));
            }

            return false;
        }
        private bool IsETALegDateDeleted(ShipmentPickUpPM shipmentPickUpPM, ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (shipmentDeliveryPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateRemoved(shipmentDeliveryPM.ETA, shipmentPickUpDelivery.ETA));
            }
            else if (shipmentPickUpPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateRemoved(shipmentPickUpPM.ETA, shipmentPickUpDelivery.ETA));
            }

            return false;
        }
        private bool IsETDLegDateAdded(ShipmentPickUpPM shipmentPickUpPM, ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (shipmentDeliveryPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateAdded(shipmentDeliveryPM.ETD, shipmentPickUpDelivery.ETD));
            }
            else if (shipmentPickUpPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateAdded(shipmentPickUpPM.ETD, shipmentPickUpDelivery.ETD));
            }

            return false;
        }
        private bool IsETDLegDateEdited(ShipmentPickUpPM shipmentPickUpPM, ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (shipmentDeliveryPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateEdited(shipmentDeliveryPM.ETD, shipmentPickUpDelivery.ETD));
            }
            else if (shipmentPickUpPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateEdited(shipmentPickUpPM.ETD, shipmentPickUpDelivery.ETD));
            }

            return false;
        }
        private bool IsETDLegDateDeleted(ShipmentPickUpPM shipmentPickUpPM, ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery shipmentPickUpDelivery)
        {
            if (shipmentDeliveryPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateRemoved(shipmentDeliveryPM.ETD, shipmentPickUpDelivery.ETD));
            }
            else if (shipmentPickUpPM != null && shipmentPickUpDelivery != null)
            {
                return (RoutingDate.IsDateRemoved(shipmentPickUpPM.ETD, shipmentPickUpDelivery.ETD));
            }

            return false;
        }
        private List<TraceEvent> GetPickupDeliveryArrangedTraceEventsByEventDate(List<TraceEvent> traceEvents, DateTime? eventDateTime)
        {
            if (traceEvents == null)
            {
                return null;
            }

            return traceEvents.FindAll(d => eventDateTime != null && d.EventDateTime == eventDateTime && d.Deleted == false);
        }
        private bool IsEventExistInTraceEvents(string eventTypeCode, DateTime? eventDateTime)
        {
            if (string.IsNullOrEmpty(eventTypeCode) || eventDateTime == null)
            {
                return false;
            }

            EventType eventType = allEventTypes.Where(d => d.Code == eventTypeCode).FirstOrDefault();
            if (eventType == null)
            {
                return false;
            }

            List<TraceEvent> AllEventTraces = this.traceEventRepository.GetAllTraceEventsByEventType(entityPM.Id, eventType.Id, tenant).ToList();
            return AllEventTraces.Find(d => eventDateTime != null && d.EventDateTime == eventDateTime && d.Deleted == false) != null;
        }
    
        private bool IsETAOrETAEditedAfterDeleteFirstDeilvery(ShipmentDeliveryPM shipmentDeliveryPM, ShipmentPickUpDelivery itemPOCO)
        {
           return  ((IsETDLegDateEdited(null, shipmentDeliveryPM, itemPOCO) || IsETALegDateEdited(null, shipmentDeliveryPM, itemPOCO) ||
                      IsETDLegDateDeleted(null, shipmentDeliveryPM, itemPOCO) || IsETDLegDateDeleted(null, shipmentDeliveryPM, itemPOCO)) &&
                      !IsEventExistInTraceEvents("DLAR", itemPOCO.ETA) && !IsEventExistInTraceEvents("DLAR", itemPOCO.ETD));
        }

        private bool IsETAOrETAEditedAfterDeleteFirstPickup(ShipmentPickUpPM shipmentPickUpPM, ShipmentPickUpDelivery itemPOCO)
        {
            return ((IsETDLegDateEdited(shipmentPickUpPM, null, itemPOCO) || IsETALegDateEdited(shipmentPickUpPM, null, itemPOCO) ||
                       IsETDLegDateDeleted(shipmentPickUpPM, null, itemPOCO) || IsETDLegDateDeleted(shipmentPickUpPM, null, itemPOCO)) &&
                       !IsEventExistInTraceEvents("DLAR", itemPOCO.ETA) && !IsEventExistInTraceEvents("DLAR", itemPOCO.ETD));
        }
    }
}
