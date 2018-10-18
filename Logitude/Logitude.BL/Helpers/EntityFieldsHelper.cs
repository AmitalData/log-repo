using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class EntityFieldsHelper
    {
        public static string GetLongMasterField(ShipmentDataView entity)
        {
            string myField = null;

            if (entity != null)
            {
                if (entity.TransportModeId == "A")
                {
                    if (!string.IsNullOrEmpty(entity.AirlinePrefix) && !string.IsNullOrEmpty(entity.Master))
                    {
                        myField = entity.AirlinePrefix + "-" + entity.Master;
                    }
                }

                else
                {
                    myField = entity.Master;
                }
            }

            return myField;
        }

        public static string GetLongMasterField(Shipment entity, ShipmentMasterData entityMasterData)
        {
            string myField = null;

            if (entity != null && entityMasterData != null)
            {
                if (entity.TransportModeId == "A")
                {
                    if (!string.IsNullOrEmpty(entityMasterData.AirlinePrefix) && !string.IsNullOrEmpty(entityMasterData.Master))
                    {
                        myField = entityMasterData.AirlinePrefix + "-" + entityMasterData.Master;
                    }
                }

                else
                {
                    myField = entityMasterData.Master;
                }
            }

            return myField;
        }

        public static string GetLongMasterField(string myTransportModeId, string myAirlinePrefix, string myMaster)
        {
            string myField = null;

            if (myTransportModeId == "A")
            {
                if (!string.IsNullOrEmpty(myAirlinePrefix) && !string.IsNullOrEmpty(myMaster))
                {
                    myField = myAirlinePrefix + "-" + myMaster;
                }
            }

            else
            {
                myField = myMaster;
            }

            return myField;
        }
    }
}
