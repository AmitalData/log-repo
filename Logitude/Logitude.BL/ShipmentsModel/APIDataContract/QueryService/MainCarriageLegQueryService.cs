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

        public List<MainCarriageLeg> MainCarriageLegDataMapping(List<TransshipmentLeg> MyEntityPMs, int Tenant)
        {
            try
            {

                var MyList = new List<MainCarriageLeg>();

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

                    //if (IsUpdate)
                    //{
                    //    throw new ApplicationException("MasterNumber Can't be update");
                    //}
                    temp.MasterNumber = item.MasterNumber;

                    if (IsUpdate)
                    {
                        throw new ApplicationException("CarrierNumber Can't be update");
                    }
                    temp.CarrierNumber = item.CarrierNumber;

                    //if (IsUpdate)
                    //{
                    //    throw new ApplicationException("ETD Can't be update");
                    //}
                    temp.ETD = item.ETD;

                    //if (IsUpdate)
                    //{
                    //    throw new ApplicationException("ETA Can't be update");
                    //}
                    temp.ETA = item.ETA;

                    //if (IsUpdate)
                    //{
                    //    throw new ApplicationException("ATD Can't be update");
                    //}
                    temp.ATD = item.ATD;

                    //if (IsUpdate)
                    //{
                    //    throw new ApplicationException("ATA Can't be update");
                    //}
                    temp.ATA = item.ATA;

                    if (item.Carrier != null)
                    {
                        var myCarrierPM = CardService.CardDataMappingAndValidatin(item.Carrier, Tenant, ComputingPartnerName);
                        if (myCarrierPM != null)
                        {
                            //if (IsUpdate)
                            //{
                            //    throw new ApplicationException("Carrier Can't be update");
                            //}

                            temp.CarrierId = myCarrierPM.Id;
                        }
                    }
                                        
                    if (item.FromPort != null)
                    {
                        var myPortPM = PortService.PortDataMappingAndValidatin(item.FromPort, Tenant, ComputingPartnerName);
                        if (myPortPM != null)
                        {
                            if (IsUpdate)
                            {
                                throw new ApplicationException("FromPort Can't be update");
                            }

                            temp.FromPortId = myPortPM.Id;
                        }
                    }

                    if (item.ToPort != null)
                    {
                        var myPortPM = PortService.PortDataMappingAndValidatin(item.ToPort, Tenant, ComputingPartnerName);
                        if (myPortPM != null)
                        {
                            if (IsUpdate)
                            {
                                throw new ApplicationException("ToPort Can't be update");
                            }
                            
                            temp.ToPortId = myPortPM.Id;
                        }
                    }

                    if (item.Vessel != null)
                    {
                        var myVesselPM = VesselService.VesselDataMappingAndValidatin(item.Vessel, Tenant, ComputingPartnerName);
                        if (myVesselPM != null)
                        {
                            //if (IsUpdate)
                            //{
                            //    throw new ApplicationException("Vessel Can't be update");
                            //}

                            temp.VesselId = myVesselPM.Id;
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
