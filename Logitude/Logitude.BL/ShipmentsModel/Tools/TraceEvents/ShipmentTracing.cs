using System;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        private int tenant;
        private bool isNewEntity;
        private string loggedContactId;
        private ShipmentPM entityPM;
        private Shipment entityPoco;
        private ShipmentMasterData entityMasterData;
        private string objectTableId;
        private string objectTableName = "Shipment";
        private DateTime todayDateTime;
        private List<EventType> allEventTypes;
        private List<EntityStatus> allEntityStatuses;
        private IWebFreightContext objectContext;
        private EventTypeRepository eventTypeRepository;
        private TraceEventRepository traceEventRepository;
        private EntityStatusRepository entityStatusRepository;
        private ObjectTableRepository objectTabelRepository;
        string myUserId = null;
        string myCustomerCareUserEmail = null;
        public ShipmentTracing(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, string loggedContactId, bool isNewEntity)
        {
            this.tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.entityPoco = entityPoco;
            this.entityMasterData = entityMasterData;
            this.loggedContactId = loggedContactId;
            this.isNewEntity = isNewEntity;
            this.todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            this.objectContext = WebFreightContext.GetContext(tenant);
            this.objectTabelRepository = new ObjectTableRepository(objectContext);
            this.traceEventRepository = new TraceEventRepository(objectContext);
            this.eventTypeRepository = new EventTypeRepository(objectContext);
            this.entityStatusRepository = new EntityStatusRepository(objectContext);

            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            this.objectTableId = objectTable.Id;
            this.allEventTypes = eventTypeRepository.GetEventTypesByTenantAndObjectTableId(tenant, objectTableId).ToList();
            this.allEntityStatuses = entityStatusRepository.GetEntityStatusByTenantAndObjectTableId(tenant, objectTableId).ToList();
        }

        public void BeginTracing()
        {
            if (!entityPM.IsHybrid)
            {
                if (isNewEntity)
                {
                    this.CreateTraceEvent("ORDR");

                    if (entityPM.IsCopyFromShipment)
                    {
                        this.CreateTraceEvent("CFAS", "Copied from shipment number: " + entityPM.BaseShipmentNumber);

                        if (entityPM.ShipmentPayables.Count > 0 || entityPM.ShipmentReceivables.Count > 0)
                        {
                            this.CreateTraceEvent("RPCP", "From Shipment: " + entityPM.BaseShipmentNumber);
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPM.SplitFromShipmentNo))
                    {
                        this.CreateTraceEvent("SPLT", "Split From Shipment: " + entityPM.SplitFromShipmentNo);
                    }
                }

                else
                {
                    if (entityPM.ConvertFromDirectToHouse)
                    {
                        entityPM.ConvertFromDirectToHouse = false;
                        entityPM.ConvertFromHouseToDirect = false;

                        if (entityMasterData != null)
                        {
                            this.CreateTraceEvent("CSDH", entityPM.EventNote);
                        }
                    }

                    if (entityPM.ConvertFromHouseToDirect)
                    {
                        entityPM.ConvertFromDirectToHouse = false;
                        entityPM.ConvertFromHouseToDirect = false;

                        this.CreateTraceEvent("CSHD", entityPM.EventNote);
                    }

                    if (!entityPM.MarkFollowUpsAsDone)
                    {
                        this.CreateTraceEvent("USHI");
                    }

                    if (entityPM.SalesmanUserId != entityPoco.SalesmanUserId)
                    {
                        UserRepository myUserRepository = new UserRepository(tenant);
                        string oldSalesman = "empty";
                        string newSalesman = "empty";

                        if (entityPM.SalesmanUserId != null)
                        {
                            Contact myContact = ContactRepository.GetSingleContact(entityPM.SalesmanUserId, tenant, true);
                            if (myContact != null)
                            {
                                newSalesman = myContact.EnglishName;
                            }
                        }

                        if (entityPoco.SalesmanUserId != null)
                        {
                            Contact myContact = ContactRepository.GetSingleContact(entityPoco.SalesmanUserId, tenant, true);
                            if (myContact != null)
                            {
                                oldSalesman = myContact.EnglishName;
                            }
                        }

                        string remarks = "Salesman changed from " + oldSalesman + " to " + newSalesman;
                        this.CreateTraceEvent("SLCN", remarks);
                    }
                }

                this.TraceOtherData();
                this.TraceMasterData();
                this.TraceRoutingData();
                this.TraceCustomsData();
                this.TraceTerminalData();
            }
        }

        private void TraceCustomsData()
        {
            if ((entityPoco.CustomsClearanceDate == null || entityPoco.FreightRelease == null || entityPoco.TerminalAvailable == null) && (entityPM.CustomsClearanceDate != null && entityPM.FreightRelease != null && entityPM.TerminalAvailable != null))
            {
                this.CreateTraceEvent("AFD");
            }
            else if ((entityPoco.CustomsClearanceDate != null && entityPoco.FreightRelease != null && entityPoco.TerminalAvailable != null) && (entityPM.CustomsClearanceDate == null || entityPM.FreightRelease == null || entityPM.TerminalAvailable == null))
            {
                this.DeleteTraceEvent("AFD");
            }
            else if (entityPoco.CustomsClearanceDate == null && entityPM.CustomsClearanceDate != null)
            {
                if (entityPM.DirectionId == "I")
                {
                    this.CreateTraceEvent("CUCD", entityPM.CustomsClearanceDate);
                }
            }

        }

        private void TraceOtherData()
        {
            if (entityPM.QuoteId != null)
            {
                if (this.isNewEntity)
                {
                    this.CreateTraceEvent("OFQT", "Built from quote number: " + new QuoteRepository(entityPM.Tenant).GetQuoteNumber(entityPM.QuoteId));
                }

                else if (entityPoco.QuoteId == null)
                {
                    this.CreateTraceEvent("CTQT", "Connected to quote number: " + new QuoteRepository(entityPM.Tenant).GetQuoteNumber(entityPM.QuoteId));
                }
            }

            if (entityPM.IsAddingStackEvents)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "AWBF",
                    UserId = loggedContactId,
                    EntityId = entityPM.StackAirlineId,
                    ObjectTableName = "Airline",
                });

                this.CreateTraceEvent("AWBX");
            }

            else if (entityPM.IsRemovingStackEvents)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "AWBR",
                    UserId = loggedContactId,
                    EntityId = entityPM.StackAirlineId,
                    ObjectTableName = "Airline",
                });

                this.CreateTraceEvent("AWBS");
            }

            if (entityPoco.IsOperationalClosed && !entityPM.IsOperationalClosed)
            {
                this.CreateTraceEvent("OPOP", entityPM.EventNote);
            }

            if (!entityPoco.IsOperationalClosed && entityPM.IsOperationalClosed)
            {
                this.CreateTraceEvent("OPCL", entityPM.EventNote);
            }

            if (entityPoco.IsAccountingClosed && !entityPM.IsAccountingClosed)
            {
                this.CreateTraceEvent("ACOP", entityPM.EventNote);
            }

            if (!entityPoco.IsAccountingClosed && entityPM.IsAccountingClosed)
            {
                this.CreateTraceEvent("ACCL", entityPM.EventNote);
            }

            if (entityPoco.IsCancelled && !entityPM.IsCancelled)
            {
                this.CreateTraceEvent("SRAC", entityPM.EventNote);
            }

            if (entityPoco.HasException && entityPM.IsExceptionResolved)
            {
                this.CreateTraceEvent("EXRE", entityPM.EventNote);
            }

            if (!entityPoco.IsCancelled && entityPM.IsCancelled)
            {
                this.CreateTraceEvent("SCNL", entityPM.EventNote);
            }

            if (entityPoco.CustomsClearanceDate == null && entityPM.CustomsClearanceDate != null)
            {
                if (entityPM.DirectionId == "I")
                {
                    this.CreateTraceEvent("ICUC", entityPM.CustomsClearanceDate);
                }

                else if (entityPM.DirectionId == "E")
                {
                    this.CreateTraceEvent("ECUC", entityPM.CustomsClearanceDate);
                }
            }

            if (entityPoco.CutoffDate == null && entityPM.CutoffDate != null)
            {
                this.CreateTraceEvent("CUTO", entityPM.CutoffDate);                
            }
        }
        private void TraceMasterData()
        {
            if ((entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C") && entityMasterData != null)
            {
                if (entityPM.MAWBOBLDate != null && entityMasterData.MAWBOBLDate == null)
                {
                    this.CreateTraceEvent("OBLD", entityPM.MAWBOBLDate);
                }

                this.TraceMasterDataMain();
                this.TraceMasterDataTR1();
                this.TraceMasterDataTR2();
                this.TraceMasterDataTR3();

                DateTime? finalETA = this.GetFinalETA();
                if (finalETA != null && entityMasterData.MainCarriageFinalDestinationETA == null)
                {
                    this.CreateTraceEvent("ETA", finalETA);
                }
                else if (finalETA == null && entityMasterData.MainCarriageFinalDestinationETA != null)
                {
                    this.DeleteTraceEvent("ETA");
                }
            }
        }
        private void TraceMasterDataMain()
        {
            if (entityPM.MainCarriageETD != null && entityMasterData.MainCarriageETD == null)
            {
                this.CreateTraceEvent("ETD", entityPM.MainCarriageETD);
            }
            else if (entityPM.MainCarriageETD == null && entityMasterData.MainCarriageETD != null)
            {
                this.DeleteTraceEvent("ETD");
            }

            string DepartedCode = "DEP";
            if (entityPM.MainCarriageATD != null && entityMasterData.MainCarriageATD == null)
            {
                this.CreateTraceEvent(DepartedCode, entityPM.MainCarriageATD);
            }

            else if (entityPM.MainCarriageATD == null && entityMasterData.MainCarriageATD != null)
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else if (entityPM.MainCarriageFromPortId != entityMasterData.MainCarriageFromPortId)
            {
                if (entityPM.MainCarriageATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(DepartedCode);
                        //this.CreateTraceEvent(DepartedCode, entityPM.MainCarriageATD);
                    }
                }
            }

            string ArrivedCode = "ARR";
            if (entityPM.MainCarriageATA != null && entityMasterData.MainCarriageATA == null)
            {
                this.CreateTraceEvent(ArrivedCode, entityPM.MainCarriageATA);
            }

            else if (entityPM.MainCarriageATA == null && entityMasterData.MainCarriageATA != null)
            {
                this.DeleteTraceEvent(ArrivedCode);
            }

            else if (entityPM.MainCarriageToPortId != entityMasterData.MainCarriageToPortId)
            {
                if (entityPM.MainCarriageATA != null)
                {
                    if (IsCurrentStatus(ArrivedCode))
                    {
                        this.UpdateLocation(ArrivedCode);
                        //this.CreateTraceEvent(ArrivedCode, entityPM.MainCarriageATA);
                    }
                }
            }
        }
        private void TraceMasterDataTR1()
        {
            string DepartedCode = "T1DP";
            if (entityPM.Transshipment1ATD != null && entityMasterData.Transshipment1ATD == null)
            {
                this.CreateTraceEvent(DepartedCode, entityPM.Transshipment1ATD);
            }

            else if (entityPM.Transshipment1ATD == null && entityMasterData.Transshipment1ATD != null)
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else if (entityPM.Transshipment1FromPortId != entityMasterData.Transshipment1FromPortId)
            {
                if (entityPM.Transshipment1ATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(DepartedCode);
                        //this.CreateTraceEvent(DepartedCode, entityPM.Transshipment1ATD);
                    }
                }
            }

            string ArrivedCode = "T1AR";
            if (entityPM.Transshipment1ATA != null && entityMasterData.Transshipment1ATA == null)
            {
                this.CreateTraceEvent(ArrivedCode, entityPM.Transshipment1ATA);
            }

            else if (entityPM.Transshipment1ATA == null && entityMasterData.Transshipment1ATA != null)
            {
                this.DeleteTraceEvent(ArrivedCode);
            }

            else if (entityPM.Transshipment1ToPortId != entityMasterData.Transshipment1ToPortId)
            {
                if (entityPM.Transshipment1ATA != null)
                {
                    if (IsCurrentStatus(ArrivedCode))
                    {
                        this.UpdateLocation(ArrivedCode);
                        //this.CreateTraceEvent(ArrivedCode, entityPM.Transshipment1ATA);
                    }
                }
            }
        }
        private void TraceMasterDataTR2()
        {
            string DepartedCode = "T2DP";
            if (entityPM.Transshipment2ATD != null && entityMasterData.Transshipment2ATD == null)
            {
                this.CreateTraceEvent(DepartedCode, entityPM.Transshipment2ATD);
            }

            else if (entityPM.Transshipment2ATD == null && entityMasterData.Transshipment2ATD != null)
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else if (entityPM.Transshipment2FromPortId != entityMasterData.Transshipment2FromPortId)
            {
                if (entityPM.Transshipment2ATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(DepartedCode);
                        //this.CreateTraceEvent(DepartedCode, entityPM.Transshipment2ATD);
                    }
                }
            }

            string ArrivedCode = "T2AR";
            if (entityPM.Transshipment2ATA != null && entityMasterData.Transshipment2ATA == null)
            {
                this.CreateTraceEvent(ArrivedCode, entityPM.Transshipment2ATA);
            }

            else if (entityPM.Transshipment2ATA == null && entityMasterData.Transshipment2ATA != null)
            {
                this.DeleteTraceEvent(ArrivedCode);
            }

            else if (entityPM.Transshipment2ToPortId != entityMasterData.Transshipment2ToPortId)
            {
                if (entityPM.Transshipment2ATA != null)
                {
                    if (IsCurrentStatus(ArrivedCode))
                    {
                        this.UpdateLocation(ArrivedCode);
                        //this.CreateTraceEvent(ArrivedCode, entityPM.Transshipment2ATA);
                    }
                }
            }
        }
        private void TraceMasterDataTR3()
        {
            string DepartedCode = "T3DP";
            if (entityPM.Transshipment3ATD != null && entityMasterData.Transshipment3ATD == null)
            {
                this.CreateTraceEvent(DepartedCode, entityPM.Transshipment3ATD);
            }

            else if (entityPM.Transshipment3ATD == null && entityMasterData.Transshipment3ATD != null)
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else if (entityPM.Transshipment3FromPortId != entityMasterData.Transshipment3FromPortId)
            {
                if (entityPM.Transshipment3ATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(DepartedCode);
                        //this.CreateTraceEvent(DepartedCode, entityPM.Transshipment3ATD);
                    }
                }
            }

            string ArrivedCode = "T3AR";
            if (entityPM.Transshipment3ATA != null && entityMasterData.Transshipment3ATA == null)
            {
                this.CreateTraceEvent(ArrivedCode, entityPM.Transshipment3ATA);
            }

            else if (entityPM.Transshipment3ATA == null && entityMasterData.Transshipment3ATA != null)
            {
                this.DeleteTraceEvent(ArrivedCode);
            }

            else if (entityPM.Transshipment3ToPortId != entityMasterData.Transshipment3ToPortId)
            {
                if (entityPM.Transshipment3ATA != null)
                {
                    if (IsCurrentStatus(ArrivedCode))
                    {
                        this.UpdateLocation(ArrivedCode);
                        //this.CreateTraceEvent(ArrivedCode, entityPM.Transshipment3ATA);
                    }
                }
            }
        }
        private void TraceRoutingData()
        {
            this.TraceRoutingDataPreCarriage();
            this.TraceRoutingDataOnCarriage();
        }
        private void TraceRoutingDataPreCarriage()
        {
            string DepartedCode = "PRCD";
            if (entityPoco.PreCarriageATD == null && entityPM.PreCarriageATD != null)
            {
                this.CreateTraceEvent(DepartedCode, entityPM.PreCarriageATD, "From " + entityPM.PreCarriageFromPortName);
            }

            else if (entityPoco.PreCarriageATD != null && entityPM.PreCarriageATD == null)
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else if (entityPM.PreCarriageFromPortId != entityPoco.PreCarriageFromPortId)
            {
                if (entityPM.PreCarriageATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(DepartedCode);
                        //this.CreateTraceEvent(DepartedCode, entityPM.PreCarriageATD, "From " + entityPM.PreCarriageFromPortName);
                    }
                }
            }

            string ArrivedCode = "PRCA";
            if (entityPoco.PreCarriageATA == null && entityPM.PreCarriageATA != null)
            {
                this.CreateTraceEvent(ArrivedCode, entityPM.PreCarriageATA, "To " + entityPM.PreCarriageToPortName);
            }

            else if (entityPoco.PreCarriageATA != null && entityPM.PreCarriageATA == null)
            {
                this.DeleteTraceEvent(ArrivedCode);
            }

            else if (entityPM.PreCarriageToPortId != entityPoco.PreCarriageToPortId)
            {
                if (entityPM.PreCarriageATA != null)
                {
                    if (IsCurrentStatus(ArrivedCode))
                    {
                        this.UpdateLocation(ArrivedCode);
                        //this.CreateTraceEvent(ArrivedCode, entityPM.PreCarriageATA, "To " + entityPM.PreCarriageToPortName);
                    }
                }
            }
        }
        private void TraceRoutingDataOnCarriage()
        {
            string DepartedCode = "ONCD";
            if (entityPoco.OnCarriageATD == null && entityPM.OnCarriageATD != null)
            {
                this.CreateTraceEvent(DepartedCode, entityPM.OnCarriageATD, "From " + entityPM.OnCarriageFromPortName);
            }

            else if (entityPoco.OnCarriageATD != null && entityPM.OnCarriageATD == null)
            {
                this.DeleteTraceEvent(DepartedCode);
            }

            else if (entityPM.OnCarriageFromPortId != entityPoco.OnCarriageFromPortId)
            {
                if (entityPM.OnCarriageATD != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(DepartedCode);
                        //this.CreateTraceEvent(DepartedCode, entityPM.OnCarriageATD, "From " + entityPM.OnCarriageFromPortName);
                    }
                }
            }

            string ArrivedCode = "ONCA";
            if (entityPoco.OnCarriageATA == null && entityPM.OnCarriageATA != null)
            {
                this.CreateTraceEvent(ArrivedCode, entityPM.OnCarriageATA, "To " + entityPM.OnCarriageToPortName);
            }

            else if (entityPoco.OnCarriageATA != null && entityPM.OnCarriageATA == null)
            {
                this.DeleteTraceEvent(ArrivedCode);
            }

            else if (entityPM.OnCarriageToPortId != entityPoco.OnCarriageToPortId)
            {
                if (entityPM.OnCarriageATA != null)
                {
                    if (IsCurrentStatus(DepartedCode))
                    {
                        this.UpdateLocation(ArrivedCode);
                        //this.CreateTraceEvent(ArrivedCode, entityPM.OnCarriageATA, "To " + entityPM.OnCarriageToPortName);
                    }
                }
            }
        }

        private DateTime? GetFinalETA()
        {
            DateTime? myResult = entityPM.MainCarriageETA;

            if (entityPM.Transshipment1ToPortId != null)
            {
                if (entityPM.Transshipment1ETA != null)
                {
                    myResult = entityPM.Transshipment1ETA;
                }
            }

            if (entityPM.Transshipment2ToPortId != null)
            {
                if (entityPM.Transshipment2ETA != null)
                {
                    myResult = entityPM.Transshipment2ETA;
                }
            }

            if (entityPM.Transshipment3ToPortId != null)
            {
                if (entityPM.Transshipment3ETA != null)
                {
                    myResult = entityPM.Transshipment3ETA;
                }
            }

            return myResult;
        }

        public void TracePickUp(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (!entityPM.IsHybrid)
            {
                string DepartedCode = "PICD";
                if (itemPOCO.ATD == null && itemPM.ATD != null)
                {
                    this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                }

                else if (itemPOCO.ATD != null && itemPM.ATD == null)
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

                string ArrivedCode = "RCS";
                if (itemPOCO.ATA == null && itemPM.ATA != null)
                {
                    this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                }

                else if (itemPOCO.ATA != null && itemPM.ATA == null)
                {
                    this.DeleteTraceEvent(ArrivedCode);
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
                                //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
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
                                                //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                                            }

                                            break;
                                        }

                                    case "PORT":
                                        {
                                            if (itemPM.ToPortId != itemPOCO.ToPortId)
                                            {
                                                this.UpdateLocation(ArrivedCode);
                                                //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                                            }

                                            break;
                                        }

                                    case "CASL":
                                        {
                                            if (itemPM.ToAddressCity != itemPOCO.ToAddressCity)
                                            {
                                                this.UpdateLocation(ArrivedCode);
                                                //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                                            }

                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void TraceDelivery(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (!entityPM.IsHybrid)
            {
                if (itemPM.PickUpDeliveryTypeCode == "DELV")
                {
                    string DepartedCode = "DELD";
                    if (itemPOCO.ATD == null && itemPM.ATD != null)
                    {
                        this.CreateTraceEvent(DepartedCode, itemPM.ATD, itemPM);
                    }

                    else if (itemPOCO.ATD != null && itemPM.ATD == null)
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
                    if (itemPOCO.ATA == null && itemPM.ATA != null)
                    {
                        this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                    }

                    else if (itemPOCO.ATA != null && itemPM.ATA == null)
                    {
                        this.DeleteTraceEvent(ArrivedCode);
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
                                    //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
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
                                                    //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                                                }

                                                break;
                                            }

                                        case "PORT":
                                            {
                                                if (itemPM.ToPortId != itemPOCO.ToPortId)
                                                {
                                                    this.UpdateLocation(ArrivedCode);
                                                    //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                                                }

                                                break;
                                            }

                                        case "CASL":
                                            {
                                                if (itemPM.ToAddressCity != itemPOCO.ToAddressCity)
                                                {
                                                    this.UpdateLocation(ArrivedCode);
                                                    //this.CreateTraceEvent(ArrivedCode, itemPM.ATA, itemPM);
                                                }

                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void TraceDeletedPickUp(ShipmentPickUpPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (!entityPM.IsHybrid)
            {
                if (itemPOCO.PickUpDeliveryTypeCode == "PICK")
                {
                    if (itemPOCO.ATA != null)
                    {
                        this.DeleteTraceEvent("RCS");
                    }

                    if (itemPOCO.ATD != null)
                    {
                        this.DeleteTraceEvent("PICD");
                    }
                }

                else
                {
                    if (itemPOCO.ATA != null)
                    {
                        this.DeleteTraceEvent("PIOD");
                    }

                    if (itemPOCO.ATD != null)
                    {
                        this.DeleteTraceEvent("DELD");
                    }
                }
            }
        }
        public void TraceDeletedDelivery(ShipmentDeliveryPM itemPM, ShipmentPickUpDelivery itemPOCO)
        {
            if (!entityPM.IsHybrid)
            {
                if (itemPM.PickUpDeliveryTypeCode == "DELV")
                {
                    if (itemPOCO.PickUpDeliveryTypeCode == "PICK")
                    {
                        if (itemPOCO.ATA != null)
                        {
                            this.DeleteTraceEvent("RCS");
                        }

                        if (itemPOCO.ATD != null)
                        {
                            this.DeleteTraceEvent("PICD");
                        }
                    }

                    else
                    {
                        if (itemPOCO.ATA != null)
                        {
                            this.DeleteTraceEvent("PIOD");
                        }

                        if (itemPOCO.ATD != null)
                        {
                            this.DeleteTraceEvent("DELD");
                        }
                    }
                }
            }
        }
        public void TraceShipmentOnCreateDoneFollowUp(FollowUp followUp)
        {
            if (!entityPM.IsHybrid)
            {
                EventType eventType = allEventTypes.Where(d => d.Id == followUp.EventTypeId).FirstOrDefault();

                if (eventType != null)
                {
                    if (eventType.ManualActivatedFollowUp)
                    {
                        this.CreateTraceEvent(eventType.Code, followUp.DoneNote);
                    }
                }
            }
        }
        public void TraceShipmentOnUpdateDoneFollowUp(ShipmentFollowUpPM followUp)
        {
            if (!entityPM.IsHybrid)
            {
                EventType eventType = allEventTypes.Where(d => d.Id == followUp.EventTypeId).FirstOrDefault();

                if (eventType != null)
                {
                    this.CreateTraceEvent("USHI");

                    if (eventType.ManualActivatedFollowUp)
                    {
                        this.CreateTraceEvent(eventType.Code, followUp.DoneDateTime, followUp.DoneNote);
                    }
                }
            }
        }

        private void CreateTraceEvent(string eventTypeCode)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = entityPoco.StatusId,
                EventTypeCode = eventTypeCode,
            });
        }
        private void CreateTraceEvent(string eventTypeCode, string eventNotes)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = entityPoco.StatusId,
                EventTypeCode = eventTypeCode,
                Notes = eventNotes,
            });
        }
        private void CreateTraceEvent(string eventTypeCode, DateTime? eventDateTime)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = entityPoco.StatusId,
                EventTypeCode = eventTypeCode,
                EventDateTime = eventDateTime,
            });
        }
        private void CreateTraceEvent(string eventTypeCode, DateTime? eventDateTime, string eventNotes)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = entityPoco.StatusId,
                EventTypeCode = eventTypeCode,
                EventDateTime = eventDateTime,
                Notes = eventNotes,
            });
        }
        private void CreateTraceEvent(string eventTypeCode, DateTime? eventDateTime, ShipmentPickUpPM myPickUp)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = entityPoco.StatusId,
                EventTypeCode = eventTypeCode,
                EventDateTime = eventDateTime,
                PickUp = myPickUp,
            });
        }
        private void CreateTraceEvent(string eventTypeCode, DateTime? eventDateTime, ShipmentDeliveryPM myDelivery)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = entityPoco.StatusId,
                EventTypeCode = eventTypeCode,
                EventDateTime = eventDateTime,
                Delivery = myDelivery,
            });
        }
        private void CreateTraceEvent(EventStatusTracerArgs args)
        {
            if (!string.IsNullOrEmpty(args.EventTypeCode))
            {
                if (args.ObjectTableName != this.objectTableName)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = args.EventTypeCode,
                        UserId = loggedContactId,
                        EntityId = args.EntityId,
                        ObjectTableName = args.ObjectTableName,
                        Notes = args.Notes,
                    });
                }

                else
                {
                    EventType eventType = allEventTypes.Where(d => d.Code == args.EventTypeCode).FirstOrDefault();

                    if (eventType == null)
                    {
                        throw new Exception("Event Type is not recognized:" + args.EventTypeCode);
                    }

                    else
                    {
                        #region User
                        if (myUserId == null)
                        {
                            if (!string.IsNullOrEmpty(args.UserId))
                            {
                                myUserId = args.UserId;

                                if (tenant != 0)
                                {
                                    UserRepository userRepository = new UserRepository(0);
                                    User user = userRepository.GetSingleUser(myUserId, 0, true);
                                    if (user != null)
                                    {
                                        User systemUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, true);
                                        if (systemUser != null)
                                        {
                                            myUserId = systemUser.Id;
                                        }

                                        myCustomerCareUserEmail = user.Contact.Email;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Dates
                        if (args.LogDateTime == null)
                        {
                            args.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        else if (args.LogDateTime.Value.Year == 1)
                        {
                            args.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        if (args.EventDateTime == null)
                        {
                            args.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        else if (args.EventDateTime.Value.Year == 1)
                        {
                            args.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }
                        #endregion

                        #region Status
                        if (args.IsAddedManually)
                        {
                            if (!string.IsNullOrEmpty(args.NewStatusId) && !string.IsNullOrEmpty(args.OldStatusId))
                            {
                                EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(args.NewStatusId, tenant, true);
                                EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(args.OldStatusId, tenant, true);

                                Contact user = ContactRepository.GetSingleContact(myUserId, tenant, true);
                                if (newStatus != null)
                                {
                                    args.Notes = "Status was changed manually from " + oldStatus.Name + " to " + newStatus.Name + " by " + (user != null ? user.EnglishName : "");
                                }
                            }
                        }

                        else
                        {
                            if (eventType.EntityStatusId != null)
                            {
                                ComputeEventStatus(args);
                            }
                        }
                        #endregion

                        TraceEvent myTraceEvent = new TraceEvent()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Tenant = tenant,
                            EntityId = args.EntityId,
                            EventTypeId = eventType.Id,
                            ObjectTableId = this.objectTableId,
                            LogDateTime = args.LogDateTime.Value,
                            EventDateTime = args.EventDateTime.Value,
                            ExternalId = args.ExternalId,
                            IsAddedManually = args.IsAddedManually,
                            UserId = myUserId,
                            CustomerCareUserEmail = myCustomerCareUserEmail,
                            Notes = args.Notes,
                            Location = args.StatusLocation,
                        };

                        this.traceEventRepository.Add(myTraceEvent);
                        this.traceEventRepository.SubmitChanges();
                        objectContext.SaveChanges();

                        if (eventType.IsCustomerView)
                        {
                            ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(myTraceEvent.Id, tenant);

                            ComputeLastSharedEvent(entityPM);
                        }
                    }
                }
            }
        }
        private void DeleteTraceEvent(string eventTypeCode)
        {
            if (!string.IsNullOrEmpty(eventTypeCode))
            {
                EventType eventType = allEventTypes.Where(d => d.Code == eventTypeCode).FirstOrDefault();

                if (eventType != null)
                {
                    List<TraceEvent> AllEventTraces = this.traceEventRepository.GetAllTraceEventsByEventType(entityPM.Id, eventType.Id, tenant).ToList();

                    if (AllEventTraces.Count > 0)
                    {
                        foreach (TraceEvent iTraceEvent in AllEventTraces)
                        {
                            iTraceEvent.Deleted = true;
                            traceEventRepository.Update(iTraceEvent);
                        }

                        traceEventRepository.SubmitChanges();

                        if (!string.IsNullOrEmpty(eventType.EntityStatusId))
                        {
                            TraceEvent previousEvent = null;

                            List<TraceEvent> iTraceEventList = (from a in objectContext.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                                                 where a.Tenant == tenant
                                                                 && a.EntityId == entityPM.Id
                                                                 && a.ObjectTableId == objectTableId
                                                                 && a.EventType.EntityStatus != null
                                                                 && a.Deleted == false
                                                                 select a).ToList();

                            foreach (TraceEvent e in iTraceEventList)
                            {
                                if (!e.Deleted)
                                {
                                    if (e.EventType.EntityStatus != null)
                                    {
                                        if (previousEvent == null)
                                        {
                                            previousEvent = e;
                                        }

                                        else
                                        {
                                            if (e.EventType.EntityStatus.StatusWeight > previousEvent.EventType.EntityStatus.StatusWeight)
                                            {
                                                previousEvent = e;
                                            }
                                        }
                                    }
                                }
                            }

                            if (previousEvent != null)
                            {
                                entityPM.StatusId = previousEvent.EventType.EntityStatusId;
                                entityPM.StatusDate = previousEvent.EventDateTime;
                                entityPM.LastStatusLogDate = previousEvent.EventDateTime;

                                switch (previousEvent.EventType.Code)
                                {
                                    case "PICD":
                                    case "PCAR":
                                    case "DELD":
                                    case "DLAR":
                                    case "RCS":
                                    case "PIOD":
                                        {
                                            entityPM.StatusLocation = previousEvent.Location;
                                            break;
                                        }

                                    default:
                                        {
                                            entityPM.StatusLocation = GetStatusLocation(new EventStatusTracerArgs() { EventTypeCode = previousEvent.EventType.Code });
                                            break;
                                        }
                                }
                            }

                            else
                            {
                                EventType firstEventType = allEventTypes.Where(d => d.Code == "ORDR").FirstOrDefault();
                                entityPM.StatusId = firstEventType.EntityStatusId;
                                entityPM.StatusDate = entityPM.CreateDateTime;
                                entityPM.LastStatusLogDate = entityPM.CreateDateTime;
                                entityPM.StatusLocation = null;
                            }

                            entityPoco.StatusId = entityPM.StatusId;
                            entityPoco.StatusDate = entityPM.StatusDate;
                            entityPoco.StatusLocation = entityPM.StatusLocation;
                            entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;

                            if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                            {
                                entityMasterData.StatusId = entityPM.StatusId;
                                entityMasterData.StatusDate = entityPM.StatusDate;
                                entityMasterData.StatusLocation = entityPM.StatusLocation;
                            }
                        }
                    }
                }
            }
        }
        private void ComputeEventStatus(EventStatusTracerArgs args)
        {
            EventType newEventType = allEventTypes.Where(d => d.Code == args.EventTypeCode).FirstOrDefault();

            if (newEventType.EntityStatusId != null)
            {
                if (string.IsNullOrEmpty(args.OldStatusId))
                {
                    args.StatusLocation = GetStatusLocation(args);

                    entityPM.StatusId = newEventType.EntityStatusId;
                    entityPM.StatusDate = args.EventDateTime;
                    entityPM.StatusLocation = args.StatusLocation;
                    entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                    entityPoco.StatusId = entityPM.StatusId;
                    entityPoco.StatusDate = entityPM.StatusDate;
                    entityPoco.StatusLocation = entityPM.StatusLocation;
                    entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;

                    if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                    {
                        entityMasterData.StatusId = entityPM.StatusId;
                        entityMasterData.StatusDate = entityPM.StatusDate;
                        entityMasterData.StatusLocation = entityPM.StatusLocation;
                    }
                }

                else
                {
                    EntityStatus newEntityStatus = allEntityStatuses.Where(d => d.Id == newEventType.EntityStatusId).FirstOrDefault();
                    EntityStatus oldEntityStatus = allEntityStatuses.Where(d => d.Id == args.OldStatusId).FirstOrDefault();

                    if (newEntityStatus.StatusWeight >= oldEntityStatus.StatusWeight)
                    {
                        args.StatusLocation = GetStatusLocation(args);

                        entityPM.StatusId = newEventType.EntityStatusId;
                        entityPM.StatusDate = args.EventDateTime;
                        entityPM.StatusLocation = args.StatusLocation;
                        entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                        entityPoco.StatusId = entityPM.StatusId;
                        entityPoco.StatusDate = entityPM.StatusDate;
                        entityPoco.StatusLocation = entityPM.StatusLocation;
                        entityPoco.LastStatusLogDate = entityPM.LastStatusLogDate;

                        if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                        {
                            entityMasterData.StatusId = entityPM.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;
                            entityMasterData.StatusLocation = entityPM.StatusLocation;
                        }
                    }
                }
            }
        }
        private string GetStatusLocation(EventStatusTracerArgs args)
        {
            string myResult = null;

            switch (args.EventTypeCode)
            {
                case "ORDR":
                case "DDCU":
                case "CCD":
                    {
                        myResult = null;
                        break;
                    }

                case "ARR":
                    {
                        myResult = entityPM.MainCarriageToPortCode;
                        break;
                    }

                case "T1AR":
                    {
                        myResult = entityPM.Transshipment1ToPortCode;
                        break;
                    }

                case "T2AR":
                    {
                        myResult = entityPM.Transshipment2ToPortCode;
                        break;
                    }

                case "T3AR":
                    {
                        myResult = entityPM.Transshipment3ToPortCode;
                        break;
                    }

                case "ONCD":
                    {
                        myResult = entityPM.OnCarriageFromPortCode;
                        break;
                    }

                case "ONCA":
                    {
                        myResult = entityPM.OnCarriageToPortCode;
                        break;
                    }

                case "DEP":
                    {
                        myResult = entityPM.MainCarriageFromPortCode;
                        break;
                    }

                case "T1DP":
                    {
                        myResult = entityPM.Transshipment1FromPortCode;
                        break;
                    }

                case "T2DP":
                    {
                        myResult = entityPM.Transshipment2FromPortCode;
                        break;
                    }

                case "T3DP":
                    {
                        myResult = entityPM.Transshipment3FromPortCode;
                        break;
                    }

                case "PRCD":
                    {
                        myResult = entityPM.PreCarriageFromPortCode;
                        break;
                    }

                case "PRCA":
                    {
                        myResult = entityPM.PreCarriageToPortCode;
                        break;
                    }

                case "PICD":
                case "PCAR":
                case "DELD":
                case "DLAR":
                    {
                        if (args.PickUp != null)
                        {
                            switch (args.PickUp.PickUpDeliveryFromTypeCode)
                            {
                                case "PART":
                                    {
                                        if (!string.IsNullOrEmpty(args.PickUp.FromPartnerCardId))
                                        {
                                            AddressRepository addressRepository = new AddressRepository(tenant);
                                            Address myPartnerAddress = addressRepository.GetSingleAddress(args.PickUp.FromAddressId, tenant);
                                            if (myPartnerAddress != null)
                                            {
                                                myResult = myPartnerAddress.City;
                                            }
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        myResult = args.PickUp.FromPortCode;
                                        break;
                                    }

                                case "CASL":
                                    {
                                        myResult = args.PickUp.FromAddressCity;
                                        break;
                                    }
                            }
                        }

                        if (args.Delivery != null)
                        {
                            switch (args.Delivery.PickUpDeliveryFromTypeCode)
                            {
                                case "PART":
                                    {
                                        if (!string.IsNullOrEmpty(args.Delivery.FromPartnerCardId))
                                        {
                                            AddressRepository addressRepository = new AddressRepository(tenant);
                                            Address myPartnerAddress = addressRepository.GetSingleAddress(args.Delivery.FromAddressId, tenant);
                                            if (myPartnerAddress != null)
                                            {
                                                myResult = myPartnerAddress.City;
                                            }
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        myResult = args.Delivery.FromPortCode;
                                        break;
                                    }

                                case "CASL":
                                    {
                                        myResult = args.Delivery.FromAddressCity;
                                        break;
                                    }
                            }
                        }

                        break;
                    }

                case "RCS":
                case "PIOD":
                    {
                        if (args.PickUp != null)
                        {
                            switch (args.PickUp.PickUpDeliveryToTypeCode)
                            {
                                case "PART":
                                    {
                                        if (!string.IsNullOrEmpty(args.PickUp.ToPartnerCardId))
                                        {
                                            AddressRepository addressRepository = new AddressRepository(tenant);
                                            Address myPartnerAddress = addressRepository.GetSingleAddress(args.PickUp.ToAddressId, tenant);
                                            if (myPartnerAddress != null)
                                            {
                                                myResult = myPartnerAddress.City;
                                            }
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        myResult = args.PickUp.ToPortCode;
                                        break;
                                    }

                                case "CASL":
                                    {
                                        myResult = args.PickUp.ToAddressCity;
                                        break;
                                    }
                            }
                        }

                        if (args.Delivery != null)
                        {
                            switch (args.Delivery.PickUpDeliveryToTypeCode)
                            {
                                case "PART":
                                    {
                                        if (!string.IsNullOrEmpty(args.Delivery.ToPartnerCardId))
                                        {
                                            AddressRepository addressRepository = new AddressRepository(tenant);
                                            Address myPartnerAddress = addressRepository.GetSingleAddress(args.Delivery.ToAddressId, tenant);
                                            if (myPartnerAddress != null)
                                            {
                                                myResult = myPartnerAddress.City;
                                            }
                                        }

                                        break;
                                    }

                                case "PORT":
                                    {
                                        myResult = args.Delivery.ToPortCode;
                                        break;
                                    }

                                case "CASL":
                                    {
                                        myResult = args.Delivery.ToAddressCity;
                                        break;
                                    }
                            }
                        }

                        break;
                    }
            }

            return myResult;
        }
        public static void DeleteShipmentTraceEvent(ShipmentPM entityPM, string traceEventId, int tenant, bool external)
        {
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            if (traceEvent != null)
            {
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                Shipment shipment = shipmentRepository.GetSingleShipment(entityPM.Id, entityPM.Tenant);

                traceEvent.Deleted = true;
                traceEventRep.Update(traceEvent);
                traceEventRep.SubmitChanges();

                if (traceEvent.EventType != null && traceEvent.EventType.Code == "EXCE")
                {
                    TraceEvent lastExceptiontraceEvent = traceEventRep.GetLastExceptionTraceEventByShipmentId(entityPM.Id, entityPM.Tenant);

                    if (lastExceptiontraceEvent != null && lastExceptiontraceEvent.Id == traceEvent.Id)
                    {
                        shipment.ExceptionDescription = null;
                        shipment.ExceptionDate = null;
                        shipment.HasException = false;                        
                    }
                }

                List<TraceEvent> traceEventList = new List<TraceEvent>();
                if (traceEvent.EventType.IsCustomerView || !string.IsNullOrEmpty(traceEvent.EventType.EntityStatusId))
                {
                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                    ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
                    string objectTableId = objectTable.Id;

                    traceEventList = traceEventRep.GetTraceEvents(tenant, entityPM.Id, objectTableId).ToList();
                }
                
                if (traceEvent.EventType.IsCustomerView)
                {
                    ComputeLastSharedEvent(entityPM);
                    
                    shipment.LastSharedEventId = entityPM.LastSharedEventId;
                    shipment.LastSharedEventLocation = entityPM.LastSharedEventLocation;
                    shipment.LastSharedEventNotes = entityPM.LastSharedEventNotes;
                    shipment.LastSharedEventDate = entityPM.LastSharedEventDate;
                }

                if (!string.IsNullOrEmpty(traceEvent.EventType.EntityStatusId))
                {
                    if (entityPM.StatusId == traceEvent.EventType.EntityStatusId)
                    {
                        TraceEvent previousEvent = null;
                        
                        foreach (TraceEvent e in traceEventList)
                        {
                            if (!e.Deleted)
                            {
                                if (e.EventType.EntityStatus != null)
                                {
                                    if (previousEvent == null)
                                    {
                                        previousEvent = e;
                                    }

                                    else
                                    {
                                        if (e.EventType.EntityStatus.StatusWeight > previousEvent.EventType.EntityStatus.StatusWeight)
                                        {
                                            previousEvent = e;
                                        }
                                    }
                                }
                            }
                        }

                        if (previousEvent != null)
                        {
                            entityPM.StatusId = previousEvent.EventType.EntityStatusId;
                            entityPM.StatusDate = previousEvent.EventDateTime;
                            entityPM.StatusLocation = previousEvent.Location;
                            entityPM.LastStatusLogDate = entityPM.StatusDate;
                        }

                        else
                        {
                            EntityStatusRepository entityStatusRep = new EntityStatusRepository(tenant);
                            EntityStatus orderStatus = entityStatusRep.GetSingleEntityStatusByCode("SHOR", tenant);
                            entityPM.StatusId = orderStatus.Id;
                            entityPM.StatusDate = entityPM.CreateDateTime;
                            entityPM.StatusLocation = null;
                            entityPM.LastStatusLogDate = entityPM.CreateDateTime;
                        }

                        if (external)
                        {
                            shipment.StatusId = entityPM.StatusId;
                            shipment.StatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            shipment.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            shipment.StatusLocation = entityPM.StatusLocation;

                            if (shipment.ShipmentLevelCode != "H")
                            {
                                ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);
                                ShipmentMasterData masterData = shipmentMasterDataRepository.GetSingleMasterData(shipment.Id);
                                if (masterData != null)
                                {
                                    masterData.StatusId = shipment.StatusId;
                                    masterData.StatusDate = shipment.StatusDate;
                                    masterData.StatusLocation = shipment.StatusLocation;
                                    shipmentMasterDataRepository.Update(masterData);
                                    shipmentMasterDataRepository.SubmitChanges();
                                }
                            }
                        }
                    }
                }

                shipmentRepository.Update(shipment);
                shipmentRepository.SubmitChanges();
            }
        }
        public static void DeleteShipmentTraceEventForHybrid(ShipmentPM entityPM, string traceEventId, int tenant)
        {
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            if (traceEvent != null)
            {
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                Shipment shipment = shipmentRepository.GetSingleShipment(entityPM.Id, entityPM.Tenant);

                traceEvent.Deleted = true;
                traceEventRep.Update(traceEvent);
                traceEventRep.SubmitChanges();

                if (traceEvent.EventType != null && traceEvent.EventType.Code == "EXCE")
                {
                    TraceEvent lastExceptiontraceEvent = traceEventRep.GetLastExceptionTraceEventByShipmentId(entityPM.Id, entityPM.Tenant);

                    if (lastExceptiontraceEvent != null && lastExceptiontraceEvent.Id == traceEvent.Id)
                    {
                        shipment.ExceptionDescription = null;
                        shipment.ExceptionDate = null;
                        shipment.HasException = false;
                    }
                }

                List<TraceEvent> traceEventList = new List<TraceEvent>();
                if (traceEvent.EventType.IsCustomerView || !string.IsNullOrEmpty(traceEvent.EventType.EntityStatusId))
                {
                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                    ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
                    string objectTableId = objectTable.Id;

                    traceEventList = traceEventRep.GetTraceEvents(tenant, entityPM.Id, objectTableId).ToList();
                }

                if (traceEvent.EventType.IsCustomerView)
                {
                    ComputeLastSharedEvent(entityPM);

                    shipment.LastSharedEventId = entityPM.LastSharedEventId;
                    shipment.LastSharedEventLocation = entityPM.LastSharedEventLocation;
                    shipment.LastSharedEventNotes = entityPM.LastSharedEventNotes;
                    shipment.LastSharedEventDate = entityPM.LastSharedEventDate;
                }

                if (!string.IsNullOrEmpty(traceEvent.EventType.EntityStatusId))
                {
                    if (entityPM.StatusId == traceEvent.EventType.EntityStatusId)
                    {
                        TraceEvent previousEvent = null;

                        foreach (TraceEvent e in traceEventList)
                        {
                            if (!e.Deleted)
                            {
                                if (e.EventType.EntityStatus != null)
                                {
                                    if (previousEvent == null)
                                    {
                                        previousEvent = e;
                                    }

                                    else
                                    {
                                        if (e.EventType.EntityStatus.StatusWeight > previousEvent.EventType.EntityStatus.StatusWeight)
                                        {
                                            previousEvent = e;
                                        }
                                    }
                                }
                            }
                        }

                        if (previousEvent != null)
                        {
                            entityPM.StatusId = previousEvent.EventType.EntityStatusId;
                            entityPM.StatusDate = previousEvent.EventDateTime;
                            entityPM.StatusLocation = previousEvent.Location;
                            entityPM.LastStatusLogDate = entityPM.StatusDate;
                        }

                        else
                        {
                            EntityStatusRepository entityStatusRep = new EntityStatusRepository(tenant);
                            EntityStatus orderStatus = entityStatusRep.GetSingleEntityStatusByCode("SHOR", tenant);
                            entityPM.StatusId = orderStatus.Id;
                            entityPM.StatusDate = entityPM.CreateDateTime;
                            entityPM.StatusLocation = null;
                            entityPM.LastStatusLogDate = entityPM.CreateDateTime;
                        }


                        shipment.StatusId = entityPM.StatusId;
                        shipment.StatusDate = entityPM.StatusDate;
                        shipment.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        shipment.StatusLocation = entityPM.StatusLocation;

                        if (shipment.ShipmentLevelCode != "H")
                        {
                            ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);
                            ShipmentMasterData masterData = shipmentMasterDataRepository.GetSingleMasterData(shipment.Id);
                            if (masterData != null)
                            {
                                masterData.StatusId = shipment.StatusId;
                                masterData.StatusDate = shipment.StatusDate;
                                masterData.StatusLocation = shipment.StatusLocation;
                                shipmentMasterDataRepository.Update(masterData);
                                shipmentMasterDataRepository.SubmitChanges();
                            }
                        }
                    }

                }

                shipmentRepository.Update(shipment);
                shipmentRepository.SubmitChanges();
            }
        }
        private void TraceTerminalData()
        {
            if (this.entityPM.DirectionId == "I")
            {
                if (entityPoco.WarehouseLegActualEntryDate == null && entityPM.WarehouseLegActualEntryDate != null)
                {
                    this.CreateTraceEvent("WHED", entityPM.WarehouseLegActualEntryDate);
                }

                else if (entityPoco.WarehouseLegActualEntryDate != null && entityPM.WarehouseLegActualEntryDate == null)
                {
                    this.DeleteTraceEvent("WHED");
                }

                if (entityPoco.WarehouseLegActualReleaseDate == null && entityPM.WarehouseLegActualReleaseDate != null)
                {
                    this.CreateTraceEvent("WHRD", entityPM.WarehouseLegActualReleaseDate);
                }

                else if (entityPoco.WarehouseLegActualReleaseDate != null && entityPM.WarehouseLegActualReleaseDate == null)
                {
                    this.DeleteTraceEvent("WHRD");
                }
            }
        }
        private static void ComputeLastSharedEvent(ShipmentPM entityPM)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            string objectTableId = objectTable.Id;

            TraceEventRepository traceEventRep = new TraceEventRepository(entityPM.Tenant);
            List<TraceEvent> myEventList = traceEventRep.GetTraceEvents(entityPM.Tenant, entityPM.Id, objectTableId).ToList();
            myEventList = myEventList.Where(d => d.EventType.IsCustomerView && !d.Deleted).ToList();

            if (myEventList.Count > 0)
            {
                TraceEvent myHigherEvent = myEventList.OrderByDescending(d => d.EventDateTime).FirstOrDefault();
                if (myHigherEvent != null)
                {
                    entityPM.LastSharedEventId = myHigherEvent.EventTypeId;
                    entityPM.LastSharedEventLocation = myHigherEvent.Location;
                    entityPM.LastSharedEventNotes = myHigherEvent.Notes;
                    entityPM.LastSharedEventDate = myHigherEvent.EventDateTime;
                    entityPM.LastSharedEventName = myHigherEvent.EventType.EnglishName;
                }
            }

            else
            {
                entityPM.LastSharedEventId = null;
                entityPM.LastSharedEventLocation = null;
                entityPM.LastSharedEventNotes = null;
                entityPM.LastSharedEventDate = null;
                entityPM.LastSharedEventName = null;
            }
        }

        private void UpdateLocation(string eventTypeCode)
        {
            if (!string.IsNullOrEmpty(eventTypeCode))
            {
                EventType eventType = allEventTypes.Where(d => d.Code == eventTypeCode).FirstOrDefault();

                if (eventType != null)
                {
                    entityPM.StatusLocation = GetStatusLocation(new EventStatusTracerArgs() { EventTypeCode = eventTypeCode });

                    entityPoco.StatusLocation = entityPM.StatusLocation;

                    if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
                    {
                        entityMasterData.StatusLocation = entityPM.StatusLocation;
                    }

                    List<TraceEvent> AllEventTraces = this.traceEventRepository.GetAllTraceEventsByEventType(entityPM.Id, eventType.Id, tenant).Where(d => d.Deleted == false).ToList();

                    if (AllEventTraces.Count > 0)
                    {
                        foreach (TraceEvent iTraceEvent in AllEventTraces)
                        {
                            iTraceEvent.Location = entityPM.StatusLocation;
                            traceEventRepository.Update(iTraceEvent);
                        }

                        traceEventRepository.SubmitChanges();
                    }
                }
            }
        }
        private bool IsCurrentStatus(string eventTypeCode)
        {
            bool iResult = false;

            if (!string.IsNullOrEmpty(eventTypeCode))
            {
                EventType eventType = allEventTypes.Where(d => d.Code == eventTypeCode).FirstOrDefault();
                if (eventType != null)
                {
                    if (eventType.EntityStatusId != null && this.entityPM.StatusId != null)
                    {
                        if (eventType.EntityStatusId == this.entityPM.StatusId)
                        {
                            iResult = true;
                        }
                    }
                }
            }

            return iResult;
        }
    }

    public class EventStatusTracerArgs
    {
        public int Tenant { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public string EventTypeCode { get; set; }
        public string ObjectTableName { get; set; }
        public bool IsAddedManually { get; set; }
        public DateTime? LogDateTime { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string ExternalId { get; set; }
        public string StatusLocation { get; set; }
        public string NewStatusId { get; set; }
        public string OldStatusId { get; set; }
        public ShipmentPickUpPM PickUp { get; set; }
        public ShipmentDeliveryPM Delivery { get; set; }
    }
}
