using Logitude.CargoTracking.BL.CargoTrackingServices.Services.MainService;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.BL.Enums;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CoreBL
{
    public class CargoTrackingMilestoneBuilder
    {
        public List<Milestone> BuildShipmentMilstones(CargoTrackingShipmentPM shipment, Dictionary<string, CargoTrackingMilestoneList> milestonesDictionaryByCode)
        {
            return BuildShipmentMilstonesFromPM(shipment, milestonesDictionaryByCode);
        }
        public List<Milestone> BuildShipmentMilstones(CargoTrackingShipmentList shipment, Dictionary<string, CargoTrackingMilestoneList> milestonesDictionaryByCode, bool ignoreCustomMilestones =false)
        {
            var shipmentPM = GetShipmentPM(shipment);
            return BuildShipmentMilstonesFromPM(shipmentPM, milestonesDictionaryByCode, ignoreCustomMilestones);
        }

        private CargoTrackingShipmentPM GetShipmentPM(CargoTrackingShipmentList shipment)
        {
            var shipmentPM = new CargoTrackingShipmentPM();
            var listPropertiesDictionary = shipment.GetType().GetProperties().ToDictionary(e => e.Name, e => e);
            foreach (var Property in shipmentPM.GetType().GetProperties())
            {
                if (!listPropertiesDictionary.ContainsKey(Property.Name))
                    continue;
                var listProperty = listPropertiesDictionary[Property.Name];
                if (listProperty.PropertyType != Property.PropertyType)
                    continue;
                Property.SetValue(shipmentPM, listProperty.GetValue(shipment));

            }
            return shipmentPM;
        }

        private List<Milestone> BuildShipmentMilstonesFromPM(CargoTrackingShipmentPM shipment, Dictionary<string, CargoTrackingMilestoneList> milestonesDictionaryByCode, bool ignoreCustomMilestones= false)
        {
            List<Milestone> milestones = new List<Milestone>();

            var tenantMilestones = milestonesDictionaryByCode.Where(x => x.Value.TenantId == shipment.Tenant)
                                                             .ToDictionary(x=>x.Key, x=>x.Value);
            if (!ignoreCustomMilestones && tenantMilestones.Count > 0)
            {
                var eventsBuilder = new CargoTrackingEventsBuilder();
                var events = eventsBuilder.BuildShipmentEvents(shipment.EntityId, shipment.Tenant, shipment.ForwardingShipmentHeaderId);

                foreach(var item in tenantMilestones)
                {
                    var mile = item.Value;
                    var milestone = new Milestone
                    {
                        Code = mile.Code,
                        Name = mile.EnglishName,
                        LocalName = mile.LocalName,
                        Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? (mile.Weight ?? 0)
                            : shipment.DirectionId == "E" ? (mile.ExportWeight ?? 0) : 0,
                        Date = null,
                        EstimationDate = null,
                        Done = false,
                        Notes = mile.Notes,
                        IsCurrent = false,
                        IsEstimation = false,
                        InActive = mile.Inactive,
                        TenantId = shipment.Tenant
                    };
                    
                    var eventMatch = events.FirstOrDefault(e => e.EventTypeId == mile.EventTypeId);
                    if(eventMatch != null)
                    {
                        milestone.Date = eventMatch.EventDatetime;
                        milestone.Done = true;
                    }

                    milestones.Add(milestone);

                }

            }

            else
            {
                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Created))
                {
                    throw new Exception("The created milestone code: " + CargoTrackingMilestoneValues.Created + "not exist in data base");
                }
                var created = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Created];
                milestones.Add(new Milestone()
                {

                    Code = CargoTrackingMilestoneValues.Created,
                    Name = created.EnglishName,
                    LocalName = created.LocalName,
                    Weight = (shipment.DirectionId == "I"|| shipment.DirectionId == "C") ? created.Weight.HasValue ? created.Weight.Value : 0
                    : shipment.DirectionId == "E" ? created.ExportWeight.HasValue ? created.ExportWeight.Value : 0 : 0,
                    Date = shipment.CreateDate,
                    EstimationDate = null,
                    Done = true,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = false,
                    InActive= created.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Booking))
                {
                    throw new Exception("The booking milestone code: " + CargoTrackingMilestoneValues.Booking + "not exist in data base");
                }
                var booking = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Booking];
                milestones.Add(new Milestone()
                {

                    Code = CargoTrackingMilestoneValues.Booking,
                    Name = booking.EnglishName,
                    LocalName = booking.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? booking.Weight.HasValue ? booking.Weight.Value : 0
                    : shipment.DirectionId == "E" ? booking.ExportWeight.HasValue ? booking.ExportWeight.Value : 0 : 0,
                    Date = shipment.BookingDate,
                    EstimationDate = shipment.BookingEstimationDate,
                    Done = shipment.BookingDone,
                    Notes = shipment.BookingNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.BookingDate == null && shipment.BookingEstimationDate != null,
                    InActive = booking.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Pickup))
                {
                    throw new Exception("The pickup milestone code: " + CargoTrackingMilestoneValues.Pickup + "not exist in data base");
                }
                var pickup = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Pickup];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.Pickup,
                    Name = pickup.EnglishName,
                    LocalName = pickup.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? pickup.Weight.HasValue ? pickup.Weight.Value : 0
                    : shipment.DirectionId == "E" ? pickup.ExportWeight.HasValue ? pickup.ExportWeight.Value : 0 : 0,
                    Date = shipment.PickupDate,
                    EstimationDate = shipment.PickupEstimationDate,
                    Done = shipment.PickupDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.PickupDate == null && shipment.PickupEstimationDate != null,
                    InActive = pickup.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.OriginWarehouse))
                {
                    throw new Exception("The from warehouse milestone code: " + CargoTrackingMilestoneValues.OriginWarehouse + "not exist in data base");
                }
                var fromwarehouse = milestonesDictionaryByCode[CargoTrackingMilestoneValues.OriginWarehouse];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.OriginWarehouse,
                    Name = fromwarehouse.EnglishName,
                    LocalName = fromwarehouse.LocalName,
                    Weight = shipment.DirectionId == "I" ? fromwarehouse.Weight.HasValue ? fromwarehouse.Weight.Value : 0
                    : shipment.DirectionId == "E" ? fromwarehouse.ExportWeight.HasValue ? fromwarehouse.ExportWeight.Value : 0 : 0,
                    Date = shipment.FromWarehouseDate,
                    EstimationDate = shipment.FromWarehouseEstimationDate,
                    Done = shipment.FromWarehouseDone,
                    Notes = shipment.FromWarehouseNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.FromWarehouseDate == null && shipment.FromWarehouseEstimationDate != null,
                    InActive = fromwarehouse.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Departure))
                {
                    throw new Exception("The departure milestone code: " + CargoTrackingMilestoneValues.Departure + "not exist in data base");
                }
                var departure = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Departure];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.Departure,
                    Name = departure.EnglishName,
                    LocalName = departure.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? departure.Weight.HasValue ? departure.Weight.Value : 0
                    : shipment.DirectionId == "E" ? departure.ExportWeight.HasValue ? departure.ExportWeight.Value : 0 : 0,
                    Date = shipment.DepartureDate,
                    EstimationDate = shipment.DepartureEstimationDate,
                    Done = shipment.DepartureDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.DepartureDate == null && shipment.DepartureEstimationDate != null,
                    InActive = departure.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Arrival))
                {
                    throw new Exception("The arrival milestone code: " + CargoTrackingMilestoneValues.Arrival + "not exist in data base");
                }
                var arrival = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Arrival];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.Arrival,
                    Name = arrival.EnglishName,
                    LocalName = arrival.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? arrival.Weight.HasValue ? arrival.Weight.Value : 0
                    : shipment.DirectionId == "E" ? arrival.ExportWeight.HasValue ? arrival.ExportWeight.Value : 0 : 0,
                    Date = shipment.ArrivalDate,
                    EstimationDate = shipment.ArrivalEstimationDate,
                    Done = shipment.ArrivalDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.ArrivalDate == null && shipment.ArrivalEstimationDate != null,
                    InActive = arrival.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.DestinationWarehouse))
                {
                    throw new Exception("The to warehouse milestone code: " + CargoTrackingMilestoneValues.DestinationWarehouse + "not exist in data base");
                }
                var towarehouse = milestonesDictionaryByCode[CargoTrackingMilestoneValues.DestinationWarehouse];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.DestinationWarehouse,
                    Name = towarehouse.EnglishName,
                    LocalName = towarehouse.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? towarehouse.Weight.HasValue ? towarehouse.Weight.Value : 0
                    : shipment.DirectionId == "E" ? towarehouse.ExportWeight.HasValue ? towarehouse.ExportWeight.Value : 0 : 0,
                    Date = shipment.ToWarehouseDate,
                    EstimationDate = shipment.ToWarehouseEstimationDate,
                    Done = shipment.ToWarehouseDone,
                    Notes = shipment.ToWarehouseNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.ToWarehouseDate == null && shipment.ToWarehouseEstimationDate != null,
                    InActive = towarehouse.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.AssignedToCustomsBroker))
                {
                    throw new Exception("The assigned to customs broker milestone code: " + CargoTrackingMilestoneValues.AssignedToCustomsBroker + "not exist in data base");
                }
                var assignedToCustomsBroker = milestonesDictionaryByCode[CargoTrackingMilestoneValues.AssignedToCustomsBroker];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.AssignedToCustomsBroker,
                    Name = assignedToCustomsBroker.EnglishName,
                    LocalName = assignedToCustomsBroker.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? assignedToCustomsBroker.Weight.HasValue ? assignedToCustomsBroker.Weight.Value : 0
                    : shipment.DirectionId == "E" ? assignedToCustomsBroker.ExportWeight.HasValue ? assignedToCustomsBroker.ExportWeight.Value : 0 : 0,
                    Date = shipment.AssignedCustomsAgentDate,
                    EstimationDate = shipment.AssignedCustomsAgentEstDate,
                    Done = shipment.AssignedCustomsAgentDone,
                    Notes = shipment.AssignedCustomsAgentNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.AssignedCustomsAgentDate == null && shipment.AssignedCustomsAgentEstDate != null,
                    InActive = assignedToCustomsBroker.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.CustomsProcess))
                {
                    throw new Exception("The customs process milestone code: " + CargoTrackingMilestoneValues.CustomsProcess + "not exist in data base");
                }
                var customsProcess = milestonesDictionaryByCode[CargoTrackingMilestoneValues.CustomsProcess];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.CustomsProcess,
                    Name = customsProcess.EnglishName,
                    LocalName = customsProcess.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? customsProcess.Weight.HasValue ? customsProcess.Weight.Value : 0
                    : shipment.DirectionId == "E" ? customsProcess.ExportWeight.HasValue ? customsProcess.ExportWeight.Value : 0 : 0,
                    //Date = Shipment.process,
                    EstimationDate = null,
                    //Done = Shipment.CustomsPaymentDone,
                    Notes = null,
                    IsCurrent = false,
                    //IsEstimation = !Shipment.CustomsPaymentDone,
                    InActive = customsProcess.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.GoodsClassification))
                {
                    throw new Exception("The goods classification milestone code: " + CargoTrackingMilestoneValues.GoodsClassification + "not exist in data base");
                }
                var goodsClassification = milestonesDictionaryByCode[CargoTrackingMilestoneValues.GoodsClassification];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.GoodsClassification,
                    Name = goodsClassification.EnglishName,
                    LocalName = goodsClassification.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? goodsClassification.Weight.HasValue ? goodsClassification.Weight.Value : 0
                    : shipment.DirectionId == "E" ? goodsClassification.ExportWeight.HasValue ? goodsClassification.ExportWeight.Value : 0 : 0,
                    Date = shipment.GoodsClassificationDate,
                    EstimationDate = shipment.GoodsClassificationEstDate,
                    Done = shipment.GoodsClassificationDone,
                    Notes = shipment.GoodsClassificationNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.GoodsClassificationDate == null && shipment.GoodsClassificationEstDate != null,
                    InActive = goodsClassification.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.DocumentInspection))
                {
                    throw new Exception("The document inspection milestone code: " + CargoTrackingMilestoneValues.DocumentInspection + "not exist in data base");
                }
                var documentInspection = milestonesDictionaryByCode[CargoTrackingMilestoneValues.DocumentInspection];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.DocumentInspection,
                    Name = documentInspection.EnglishName,
                    LocalName = documentInspection.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? documentInspection.Weight.HasValue ? documentInspection.Weight.Value : 0
                    : shipment.DirectionId == "E" ? documentInspection.ExportWeight.HasValue ? documentInspection.ExportWeight.Value : 0 : 0,
                    Date = shipment.DocumentInspectionDate,
                    EstimationDate = shipment.DocumentInspectionEstDate,
                    Done = shipment.DocumentInspectionDone,
                    Notes = shipment.DocumentInspectionNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.DocumentInspectionDate == null && shipment.DocumentInspectionEstDate != null,
                    InActive = documentInspection.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.PaymentRequested))
                {
                    throw new Exception("The payment requested milestone code: " + CargoTrackingMilestoneValues.PaymentRequested + "not exist in data base");
                }
                var paymentRequested = milestonesDictionaryByCode[CargoTrackingMilestoneValues.PaymentRequested];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.PaymentRequested,
                    Name = paymentRequested.EnglishName,
                    LocalName = paymentRequested.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? paymentRequested.Weight.HasValue ? paymentRequested.Weight.Value : 0
                    : shipment.DirectionId == "E" ? paymentRequested.ExportWeight.HasValue ? paymentRequested.ExportWeight.Value : 0 : 0,
                    Date = shipment.PaymentRequiredDate,
                    EstimationDate = shipment.PaymentRequiredEstimationDate,
                    Done = shipment.PaymentRequiredDone,
                    Notes = shipment.PaymentRequiredNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.PaymentRequiredDate == null && shipment.PaymentRequiredEstimationDate != null,
                    InActive = paymentRequested.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.PaymentReceived))
                {
                    throw new Exception("The payment received milestone code: " + CargoTrackingMilestoneValues.PaymentReceived + "not exist in data base");
                }
                var paymentReceived = milestonesDictionaryByCode[CargoTrackingMilestoneValues.PaymentReceived];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.PaymentReceived,
                    Name = paymentReceived.EnglishName,
                    LocalName = paymentReceived.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? paymentReceived.Weight.HasValue ? paymentReceived.Weight.Value : 0
                    : shipment.DirectionId == "E" ? paymentReceived.ExportWeight.HasValue ? paymentReceived.ExportWeight.Value : 0 : 0,
                    Date = shipment.PaymentReceivedDate,
                    EstimationDate = shipment.PaymentReceivedEstomationDate,
                    Done = shipment.PaymentReceivedDone,
                    Notes = shipment.PaymentReceivedNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.PaymentReceivedDate == null && shipment.PaymentReceivedEstomationDate != null,
                    InActive = paymentReceived.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.CustomsPayment))
                {
                    throw new Exception("The customs payment milestone code: " + CargoTrackingMilestoneValues.CustomsPayment + "not exist in data base");
                }
                var customsPayment = milestonesDictionaryByCode[CargoTrackingMilestoneValues.CustomsPayment];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.CustomsPayment,
                    Name = customsPayment.EnglishName,
                    LocalName = customsPayment.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? customsPayment.Weight.HasValue ? customsPayment.Weight.Value : 0
                    : shipment.DirectionId == "E" ? customsPayment.ExportWeight.HasValue ? customsPayment.ExportWeight.Value : 0 : 0,
                    Date = shipment.CustomsPaymentDate,
                    EstimationDate = null,
                    Done = shipment.CustomsPaymentDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.CustomsPaymentDone != true,
                    InActive = customsPayment.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Clearance))
                {
                    throw new Exception("The clearance milestone code: " + CargoTrackingMilestoneValues.Clearance + "not exist in data base");
                }
                var clearance = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Clearance];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.Clearance,
                    Name = clearance.EnglishName,
                    LocalName = clearance.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? clearance.Weight.HasValue ? clearance.Weight.Value : 0
                    : shipment.DirectionId == "E" ? clearance.ExportWeight.HasValue ? clearance.ExportWeight.Value : 0 : 0,
                    Date = shipment.ClearanceDate,
                    EstimationDate = null,
                    Done = shipment.ClearanceDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.ClearanceDone != true,
                    InActive = clearance.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.GatepassArrived))
                {
                    throw new Exception("The gate pass arrived code: " + CargoTrackingMilestoneValues.GatepassArrived + "not exist in data base");
                }
                var gatepassArrived = milestonesDictionaryByCode[CargoTrackingMilestoneValues.GatepassArrived];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.GatepassArrived,
                    Name = gatepassArrived.EnglishName,
                    LocalName = gatepassArrived.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? gatepassArrived.Weight.HasValue ? gatepassArrived.Weight.Value : 0
                    : shipment.DirectionId == "E" ? gatepassArrived.ExportWeight.HasValue ? gatepassArrived.ExportWeight.Value : 0 : 0,
                    Date = shipment.GatepassArrivedDate,
                    EstimationDate = shipment.GatepassArrivedEstDate,
                    Done = shipment.GatepassArrivedDone,
                    Notes = shipment.GatepassArrivedNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.GatepassArrivedDate == null && shipment.GatepassArrivedEstDate != null,
                    InActive = gatepassArrived.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.AssignedToTrucker))
                {
                    throw new Exception("The assigned to trucker code: " + CargoTrackingMilestoneValues.AssignedToTrucker + "not exist in data base");
                }
                var assignedToTrucker = milestonesDictionaryByCode[CargoTrackingMilestoneValues.AssignedToTrucker];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.AssignedToTrucker,
                    Name = assignedToTrucker.EnglishName,
                    LocalName = assignedToTrucker.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? assignedToTrucker.Weight.HasValue ? assignedToTrucker.Weight.Value : 0
                    : shipment.DirectionId == "E" ? assignedToTrucker.ExportWeight.HasValue ? assignedToTrucker.ExportWeight.Value : 0 : 0,
                    Date = shipment.AssignedTruckerDate,
                    EstimationDate = shipment.AssignedTruckerEstimationDate,
                    Done = shipment.AssignedTruckerDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.AssignedTruckerDate == null && shipment.AssignedTruckerEstimationDate != null,
                    InActive = assignedToTrucker.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.DeliveryOnTheWay))
                {
                    throw new Exception("The delivery out code: " + CargoTrackingMilestoneValues.DeliveryOnTheWay + "not exist in data base");
                }
                var deliveryOut = milestonesDictionaryByCode[CargoTrackingMilestoneValues.DeliveryOnTheWay];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.DeliveryOnTheWay,
                    Name = deliveryOut.EnglishName,
                    LocalName = deliveryOut.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? deliveryOut.Weight.HasValue ? deliveryOut.Weight.Value : 0
                    : shipment.DirectionId == "E" ? deliveryOut.ExportWeight.HasValue ? deliveryOut.ExportWeight.Value : 0 : 0,
                    Date = shipment.DeliveryDate,
                    EstimationDate = shipment.DeliveryEstimationDate,
                    Done = shipment.DeliveryDone,
                    Notes = shipment.DeliveryNotes,
                    IsCurrent = false,
                    IsEstimation = shipment.DeliveryDate == null && shipment.DeliveryEstimationDate != null,
                    InActive = deliveryOut.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.Delivered))
                {
                    throw new Exception("The delivered code: " + CargoTrackingMilestoneValues.Delivered + "not exist in data base");
                }
                var delivered = milestonesDictionaryByCode[CargoTrackingMilestoneValues.Delivered];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.Delivered,
                    Name = delivered.EnglishName,
                    LocalName = delivered.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? delivered.Weight.HasValue ? delivered.Weight.Value : 0
                    : shipment.DirectionId == "E" ? delivered.ExportWeight.HasValue ? delivered.ExportWeight.Value : 0 : 0,
                    Date = shipment.DeliveredDate,
                    EstimationDate = shipment.DeliveredEstimationDate,
                    Done = shipment.DeliveredDone,
                    Notes = null,
                    IsCurrent = false,
                    IsEstimation = shipment.DeliveredDate == null && shipment.DeliveredEstimationDate != null,
                    InActive = delivered.Inactive
                });

                if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.InvoiceIssued))
                {
                    throw new Exception("The invoiced code: " + CargoTrackingMilestoneValues.InvoiceIssued + "not exist in data base");
                }
                var invoiced = milestonesDictionaryByCode[CargoTrackingMilestoneValues.InvoiceIssued];
                milestones.Add(new Milestone()
                {
                    Code = CargoTrackingMilestoneValues.InvoiceIssued,
                    Name = invoiced.EnglishName,
                    LocalName = invoiced.LocalName,
                    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? invoiced.Weight.HasValue ? invoiced.Weight.Value : 0 
                    : shipment.DirectionId == "E" ? invoiced.ExportWeight.HasValue ? invoiced.ExportWeight.Value : 0 : 0,
                    Date = shipment.InvoicedDate,
                    Done = shipment.InvoicedDone,
                    Notes = null,
                    IsCurrent = false,
                    InActive = invoiced.Inactive
                });
			    if (!milestonesDictionaryByCode.ContainsKey(CargoTrackingMilestoneValues.ArrivedAtDistributionPoint))
			    {
				    throw new Exception("The deliveryArrived code: " + CargoTrackingMilestoneValues.ArrivedAtDistributionPoint + "not exist in data base");
			    }



			
			    var deliveryArrived = milestonesDictionaryByCode[CargoTrackingMilestoneValues.ArrivedAtDistributionPoint];
                var eventMilestoneResult = CargoTrackingShipmentsService.GetDefaultEventMilstone(shipment.Tenant, shipment.EntityId, shipment.ForwardingShipmentHeaderId);

			    milestones.Add(new Milestone()
			    {
				    Code = CargoTrackingMilestoneValues.ArrivedAtDistributionPoint,
				    Name = deliveryArrived.EnglishName,
				    LocalName = deliveryArrived.LocalName,
				    Weight = (shipment.DirectionId == "I" || shipment.DirectionId == "C") ? deliveryArrived.Weight.HasValue ? deliveryArrived.Weight.Value : 0
				    : shipment.DirectionId == "E" ? deliveryArrived.ExportWeight.HasValue ? deliveryArrived.ExportWeight.Value : 0 : 0,
				    Date = eventMilestoneResult?.EventDateTime,
				    Done = eventMilestoneResult?.EventDateTime == null ? false : true,
				    Notes = eventMilestoneResult?.Notes,
				    IsCurrent = false,
				    InActive = deliveryArrived.Inactive
			    });
            }

            milestones = milestones.OrderByDescending(d => d.Weight).ToList();
            milestones = SetMilsetoneIsEstimation(milestones);
            return milestones;
        }

        private List<Milestone> SetMilsetoneIsEstimation(List<Milestone> milestones) {
            foreach (var milestone in milestones)
            {
                if (milestone.Date != null)
                {
                    milestone.IsEstimation = milestone.Date.Value.Date > DateTime.Now.Date;
                }
            }
            return milestones;
        }
    }
}
