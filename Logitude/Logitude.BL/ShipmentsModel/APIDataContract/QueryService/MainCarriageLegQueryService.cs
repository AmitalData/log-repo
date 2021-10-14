using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public class MainCarriageLegQueryService
    {
        public MainCarriageLegQueryService(int tenant)
        {

        }

        public List<MainCarriageLeg> MainCarriageLegDataMapping(List<TransshipmentLeg> MyEntityPMs, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var MyList = new List<MainCarriageLeg>();

                foreach (TransshipmentLeg item in MyEntityPMs)
                {
                    MainCarriageLeg temp = new MainCarriageLeg();
                    temp.Id = item.Id;
                    temp.LegIndex = item.LegIndex;
                    temp.ETD = item.ETD;
                    temp.ETA = item.ETA;
                    temp.ATD = item.ATD;
                    temp.ATA = item.ATA;
                    temp.CarrierNumber = item.CarrierNumber;
                    temp.MasterNumber = item.MasterNumber;
                    
                    if (item.CarrierId != null)
                    {
                        CardQueryService Service = new CardQueryService(Tenant);
                        temp.Carrier = Service.GetCardById(item.CarrierId, Tenant, ComputingPartnerName);
                    }

                    if (item.FromPortId != null)
                    {
                        PortQueryService Service = new PortQueryService(Tenant);
                        temp.FromPort = Service.GetPortById(item.FromPortId, Tenant, ComputingPartnerName);
                    }

                    if (item.ToPortId != null)
                    {
                        PortQueryService Service = new PortQueryService(Tenant);
                        temp.ToPort = Service.GetPortById(item.ToPortId, Tenant, ComputingPartnerName);
                    }

                    if (item.VesselId != null)
                    {
                        VesselQueryService Service = new VesselQueryService(Tenant);
                        temp.Vessel = Service.GetVesselById(item.VesselId, Tenant, ComputingPartnerName);
                    }

                    MyList.Add(temp);
                }

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<TransshipmentLeg> MainCarriageLegDataMappingAndValidatin(List<MainCarriageLeg> MyEntity, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            try
            {
                List<TransshipmentLeg> MyList = new List<TransshipmentLeg>();

                CardQueryService CardService = new CardQueryService(Tenant);
                PortQueryService PortService = new PortQueryService(Tenant);
                VesselQueryService VesselService = new VesselQueryService(Tenant);

                foreach (MainCarriageLeg item in MyEntity)
                {
                    TransshipmentLeg temp = new TransshipmentLeg();
                    temp.LegIndex = item.LegIndex;

                    //if (!IsUpdate)
                    //{
                    temp.MasterNumber = item.MasterNumber;
                    //}

                    //if (!IsUpdate)
                    //{
                    temp.CarrierNumber = item.CarrierNumber;
                    //}

                    temp.ETD = item.ETD;
                    temp.ETA = item.ETA;
                    temp.ATD = item.ATD;
                    temp.ATA = item.ATA;

                    //if (!IsUpdate)
                    //{
                    //    temp.ETD = item.ETD;
                    //}

                    //if (!IsUpdate)
                    //{
                    //    temp.ETA = item.ETA;
                    //}                   

                    //if (!IsUpdate)
                    //{
                    //    temp.ATD = item.ATD;
                    //}                    

                    //if (!IsUpdate)
                    //{
                    //    temp.ATA = item.ATA;
                    //}                    

                    if (item.Carrier != null)
                    {
                        var myCarrierPM = CardService.CardDataMappingAndValidatin(item.Carrier, Tenant, ComputingPartnerName);
                        if (myCarrierPM != null)
                        {
                            //if (!IsUpdate)
                            //{  
                            temp.CarrierId = myCarrierPM.Id;
                            //}
                        }
                    }

                    if (item.FromPort != null)
                    {
                        var myPortPM = PortService.PortDataMappingAndValidatin(item.FromPort, Tenant, ComputingPartnerName);
                        if (myPortPM != null)
                        {
                            //if (!IsUpdate)
                            //{
                                temp.FromPortId = myPortPM.Id;
                            //}
                        }
                    }

                    if (item.ToPort != null)
                    {
                        var myPortPM = PortService.PortDataMappingAndValidatin(item.ToPort, Tenant, ComputingPartnerName);
                        if (myPortPM != null)
                        {
                            //if (!IsUpdate)
                            //{
                                temp.ToPortId = myPortPM.Id;
                            //}
                        }
                    }

                    if (item.Vessel != null)
                    {
                        var myVesselPM = VesselService.VesselDataMappingAndValidatin(item.Vessel, Tenant, ComputingPartnerName);
                        if (myVesselPM != null)
                        {
                            //if (!IsUpdate)
                            //{
                            temp.VesselId = myVesselPM.Id;
                            //}
                        }
                    }

                    MyList.Add(temp);
                }

                return MyList;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
