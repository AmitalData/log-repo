
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class FlightsSchedulesResponseDataMapping: IMapping<FlightsSchedulesResponsePM, FlightsSchedulesResponse>
   {

        public void CustomPMToPOCO(FlightsSchedulesResponsePM entityPM, FlightsSchedulesResponse entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(FlightsSchedulesResponsePM entityPM, FlightsSchedulesResponse entityPOCO)
        {
            if (!string.IsNullOrEmpty(entityPOCO.AirlineId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPOCO.AirlineId, entityPOCO.Tenant, true);
                if (myCard != null)
                {
                    entityPM.AirlineCode = myCard.Code;
                    entityPM.AirlineName = myCard.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.FromPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(entityPOCO.Tenant, entityPOCO.FromPortId, true);
                if (myPort != null)
                {
                    //entityPM.FromPortCode = myPort.Code;
                    //entityPM.FromPortName = myPort.EnglishName;

                    if (!string.IsNullOrEmpty(myPort.CountryId))
                    {
                        Country myCountry = CountryRepository.GetSingleCountry(myPort.CountryId, entityPOCO.Tenant, true);
                        if (myCountry != null)
                        {
                            entityPM.FromPortCountryCode = myCountry.Code;
                            entityPM.FromPortCountryName = myCountry.EnglishName;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.ToPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(entityPOCO.Tenant, entityPOCO.ToPortId, true);
                if (myPort != null)
                {
                    //entityPM.ToPortCode = myPort.Code;
                    //entityPM.ToPortName = myPort.EnglishName;

                    if (!string.IsNullOrEmpty(myPort.CountryId))
                    {
                        Country myCountry = CountryRepository.GetSingleCountry(myPort.CountryId, entityPOCO.Tenant, true);
                        if (myCountry != null)
                        {
                            entityPM.ToPortCountryCode = myCountry.Code;
                            entityPM.ToPortCountryName = myCountry.EnglishName;
                        }
                    }
                }
            }
        }
   }


}
   