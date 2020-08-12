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
    public class TransshipmentQueryService
    {
        public TransshipmentQueryService(int tenant)
        {
            
        }

        public List<Transshipment> TransshipmentDataMapping(List<TransshipmentLeg> MyEntityPMs, int Tenant)
        {
            try
            {

                var MyList = new List<Transshipment>();

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<TransshipmentLeg> TransshipmentDataMappingAndValidatin(List<Transshipment> MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                List<TransshipmentLeg> MyList = new List<TransshipmentLeg>();

                CardQueryService CardService = new CardQueryService(Tenant);
                PortQueryService PortService = new PortQueryService(Tenant);
                VesselQueryService VesselService = new VesselQueryService(Tenant);

                foreach (Transshipment item in MyEntity)
                {
                    TransshipmentLeg temp = new TransshipmentLeg();
                    temp.LegIndex = item.LegIndex;
                    temp.MasterNumber = item.MasterNumber;
                    temp.CarrierNumber = item.CarrierNumber;
                    
                    if (item.Carrier != null)
                    {
                        var myCarrierPM = CardService.CardDataMappingAndValidatin(item.Carrier, Tenant, ComputingPartnerName);
                        if (myCarrierPM != null)
                        {
                            temp.CarrierId = myCarrierPM.Id;
                        }
                    }
                                        
                    if (item.Port != null)
                    {
                        var myPortPM = PortService.PortDataMappingAndValidatin(item.Port, Tenant, ComputingPartnerName);
                        if (myPortPM != null)
                        {
                            temp.PortId = myPortPM.Id;
                        }
                    }
                    
                    if (item.Vessel != null)
                    {
                        var myVesselPM = VesselService.VesselDataMappingAndValidatin(item.Vessel, Tenant, ComputingPartnerName);
                        if (myVesselPM != null)
                        {
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
