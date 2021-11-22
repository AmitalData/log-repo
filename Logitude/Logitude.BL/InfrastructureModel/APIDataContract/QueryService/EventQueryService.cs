using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
    public partial class EventQueryService
    {
        public List<Event> EventCustomDataMapping(ShipmentPM shipmentPM, List<TraceEventPM> traceEventPMs, int tenant, string computingPartnerName = "")
        {
            try
            {
                var MyList = new List<Event>();

                foreach (TraceEventPM traceEventPM in traceEventPMs)
                {
                    MyList.Add(this.CreateAPIEvent(traceEventPM, computingPartnerName));
                }

                return MyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Event CreateAPIEvent(TraceEventPM traceEventPM, string computingPartnerName)
        {
            Event traceEvent = new Event()
            {
                Id = traceEventPM.Id,
                EventDateTime = traceEventPM.EventDateTime,
                LogDateTime = traceEventPM.LogDateTime,
                Notes = traceEventPM.Notes,
                IsManualEntry = traceEventPM.IsManualEntry,
            };

            if (traceEventPM.UserId != null)
            {
                UserQueryService Service = new UserQueryService(traceEventPM.Tenant);
                traceEvent.CreatedBy = Service.GetUserById(traceEventPM.UserId, traceEventPM.Tenant, computingPartnerName);
            }

            if (traceEventPM.EventTypeId != null)
            {
                EventTypeQueryService Service = new EventTypeQueryService(traceEventPM.Tenant);
                traceEvent.EventType = Service.GetEventTypeById(traceEventPM.EventTypeId, traceEventPM.Tenant, computingPartnerName);
            }

            return traceEvent;
        }
        public List<TraceEventPM> EventCustomDataMappingAndValidatin(Direct myEntity, List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                List<TraceEventPM> MyList = new List<TraceEventPM>();

                //CardQueryService CardService = new CardQueryService(Tenant);
                //PortQueryService PortService = new PortQueryService(Tenant);
                //VesselQueryService VesselService = new VesselQueryService(Tenant);

                //foreach (MainCarriageLeg item in MyEntity)
                //{
                //    TransshipmentLeg temp = new TransshipmentLeg();
                //    temp.LegIndex = item.LegIndex;

                //    //if (!IsUpdate)
                //    //{
                //    temp.MasterNumber = item.MasterNumber;
                //    //}

                //    //if (!IsUpdate)
                //    //{
                //    temp.CarrierNumber = item.CarrierNumber;
                //    //}

                //    temp.ETD = item.ETD;
                //    temp.ETA = item.ETA;
                //    temp.ATD = item.ATD;
                //    temp.ATA = item.ATA;

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ETD = item.ETD;
                //    //}

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ETA = item.ETA;
                //    //}                   

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ATD = item.ATD;
                //    //}                    

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ATA = item.ATA;
                //    //}                    

                //    if (item.Carrier != null)
                //    {
                //        var myCarrierPM = CardService.CardDataMappingAndValidatin(item.Carrier, Tenant, ComputingPartnerName);
                //        if (myCarrierPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{  
                //            temp.CarrierId = myCarrierPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.FromPort != null)
                //    {
                //        var myPortPM = PortService.PortDataMappingAndValidatin(item.FromPort, Tenant, ComputingPartnerName);
                //        if (myPortPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.FromPortId = myPortPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.ToPort != null)
                //    {
                //        var myPortPM = PortService.PortDataMappingAndValidatin(item.ToPort, Tenant, ComputingPartnerName);
                //        if (myPortPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.ToPortId = myPortPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.Vessel != null)
                //    {
                //        var myVesselPM = VesselService.VesselDataMappingAndValidatin(item.Vessel, Tenant, ComputingPartnerName);
                //        if (myVesselPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.VesselId = myVesselPM.Id;
                //            //}
                //        }
                //    }

                //    MyList.Add(temp);
                //}

                return MyList;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<TraceEventPM> EventCustomDataMappingAndValidatin(House myEntity, List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                List<TraceEventPM> MyList = new List<TraceEventPM>();

                //CardQueryService CardService = new CardQueryService(Tenant);
                //PortQueryService PortService = new PortQueryService(Tenant);
                //VesselQueryService VesselService = new VesselQueryService(Tenant);

                //foreach (MainCarriageLeg item in MyEntity)
                //{
                //    TransshipmentLeg temp = new TransshipmentLeg();
                //    temp.LegIndex = item.LegIndex;

                //    //if (!IsUpdate)
                //    //{
                //    temp.MasterNumber = item.MasterNumber;
                //    //}

                //    //if (!IsUpdate)
                //    //{
                //    temp.CarrierNumber = item.CarrierNumber;
                //    //}

                //    temp.ETD = item.ETD;
                //    temp.ETA = item.ETA;
                //    temp.ATD = item.ATD;
                //    temp.ATA = item.ATA;

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ETD = item.ETD;
                //    //}

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ETA = item.ETA;
                //    //}                   

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ATD = item.ATD;
                //    //}                    

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ATA = item.ATA;
                //    //}                    

                //    if (item.Carrier != null)
                //    {
                //        var myCarrierPM = CardService.CardDataMappingAndValidatin(item.Carrier, Tenant, ComputingPartnerName);
                //        if (myCarrierPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{  
                //            temp.CarrierId = myCarrierPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.FromPort != null)
                //    {
                //        var myPortPM = PortService.PortDataMappingAndValidatin(item.FromPort, Tenant, ComputingPartnerName);
                //        if (myPortPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.FromPortId = myPortPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.ToPort != null)
                //    {
                //        var myPortPM = PortService.PortDataMappingAndValidatin(item.ToPort, Tenant, ComputingPartnerName);
                //        if (myPortPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.ToPortId = myPortPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.Vessel != null)
                //    {
                //        var myVesselPM = VesselService.VesselDataMappingAndValidatin(item.Vessel, Tenant, ComputingPartnerName);
                //        if (myVesselPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.VesselId = myVesselPM.Id;
                //            //}
                //        }
                //    }

                //    MyList.Add(temp);
                //}

                return MyList;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<TraceEventPM> EventCustomDataMappingAndValidatin(Master myEntity, List<Event> eventList, int tenant, string computingPartnerName)
        {
            try
            {
                List<TraceEventPM> MyList = new List<TraceEventPM>();

                //CardQueryService CardService = new CardQueryService(Tenant);
                //PortQueryService PortService = new PortQueryService(Tenant);
                //VesselQueryService VesselService = new VesselQueryService(Tenant);

                //foreach (MainCarriageLeg item in MyEntity)
                //{
                //    TransshipmentLeg temp = new TransshipmentLeg();
                //    temp.LegIndex = item.LegIndex;

                //    //if (!IsUpdate)
                //    //{
                //    temp.MasterNumber = item.MasterNumber;
                //    //}

                //    //if (!IsUpdate)
                //    //{
                //    temp.CarrierNumber = item.CarrierNumber;
                //    //}

                //    temp.ETD = item.ETD;
                //    temp.ETA = item.ETA;
                //    temp.ATD = item.ATD;
                //    temp.ATA = item.ATA;

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ETD = item.ETD;
                //    //}

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ETA = item.ETA;
                //    //}                   

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ATD = item.ATD;
                //    //}                    

                //    //if (!IsUpdate)
                //    //{
                //    //    temp.ATA = item.ATA;
                //    //}                    

                //    if (item.Carrier != null)
                //    {
                //        var myCarrierPM = CardService.CardDataMappingAndValidatin(item.Carrier, Tenant, ComputingPartnerName);
                //        if (myCarrierPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{  
                //            temp.CarrierId = myCarrierPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.FromPort != null)
                //    {
                //        var myPortPM = PortService.PortDataMappingAndValidatin(item.FromPort, Tenant, ComputingPartnerName);
                //        if (myPortPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.FromPortId = myPortPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.ToPort != null)
                //    {
                //        var myPortPM = PortService.PortDataMappingAndValidatin(item.ToPort, Tenant, ComputingPartnerName);
                //        if (myPortPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.ToPortId = myPortPM.Id;
                //            //}
                //        }
                //    }

                //    if (item.Vessel != null)
                //    {
                //        var myVesselPM = VesselService.VesselDataMappingAndValidatin(item.Vessel, Tenant, ComputingPartnerName);
                //        if (myVesselPM != null)
                //        {
                //            //if (!IsUpdate)
                //            //{
                //            temp.VesselId = myVesselPM.Id;
                //            //}
                //        }
                //    }

                //    MyList.Add(temp);
                //}

                return MyList;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
