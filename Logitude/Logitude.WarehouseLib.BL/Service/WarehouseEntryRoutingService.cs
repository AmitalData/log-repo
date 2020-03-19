using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.WarehouseLib.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.Service
{
  public  class WarehouseEntryRoutingService
    {

        #region Routing

        public WarehouseEntryRouting GetWarehouseEntryRouting(WarehouseEntryRoutingArgs warehouseEntryRoutingArgs)
        {
            WarehouseEntryRouting warehouseEntryRouting = new WarehouseEntryRouting();
            if (warehouseEntryRoutingArgs.DirectionId == "D" && warehouseEntryRoutingArgs.TransportModeId == "I") warehouseEntryRouting = GetWarehouseEntryRoutingForInlandDomestic(warehouseEntryRoutingArgs);
            else warehouseEntryRouting = GetWarehouseEntryRoutingForNotInlandDomestic(warehouseEntryRoutingArgs);
            return warehouseEntryRouting;
        }


        #region Inland Domestic
        private WarehouseEntryRouting GetWarehouseEntryRoutingForInlandDomestic(WarehouseEntryRoutingArgs warehouseEntryRoutingArgs)
        {
            WarehouseEntryRouting warehouseEntryRouting = new WarehouseEntryRouting();
            List<string> addressIds = new List<string>();
            if (!string.IsNullOrEmpty(warehouseEntryRoutingArgs.FromAddressId)) addressIds.Add(warehouseEntryRoutingArgs.FromAddressId);
            if (!string.IsNullOrEmpty(warehouseEntryRoutingArgs.ToAddressId)) addressIds.Add(warehouseEntryRoutingArgs.ToAddressId);

            List<AddressList> addressLists = new List<AddressList>();
            if (addressIds.Count > 0)
            {
                AddressQuery addressQuery = new AddressQuery(warehouseEntryRoutingArgs.Tenant);
                addressLists = addressQuery.GetAddressListsByIds(addressIds, warehouseEntryRoutingArgs.Tenant);
                warehouseEntryRouting.Origin = GetCityNameByAddressId(warehouseEntryRoutingArgs.FromAddressId, addressLists);
                warehouseEntryRouting.Destination = GetCityNameByAddressId(warehouseEntryRoutingArgs.ToAddressId, addressLists);
                warehouseEntryRouting.Routing = (warehouseEntryRouting.Origin + " > " + warehouseEntryRouting.Destination);
            }
            return warehouseEntryRouting;
        }
        private string GetCityNameByAddressId(string addressId, List<AddressList> addressLists)
        {
            string cityName = string.Empty;
            if (!string.IsNullOrEmpty(addressId))
            {
                AddressList addressList = addressLists.Where(d => d.Id == addressId).FirstOrDefault();
                if (addressList != null) cityName = addressList.City;
            }

            return cityName;
        }
        private string GetCountryNameByCountryId(string countryId, int tenant)
        {
            string result = string.Empty;
            CountryQuery countryQuery = new CountryQuery(tenant);
            CountryPM countryPM = countryQuery.GetSinglePM(countryId, tenant);
            if (countryPM != null) result = countryPM.EnglishName;

            return result;
        }
        private string GetPortCodeByPortId(string portId, int tenant)
        {
            string portCode = string.Empty;
            PortQuery portQuery = new PortQuery(tenant);
            if (!string.IsNullOrEmpty(portId))
            {
                PortPM portPM = portQuery.GetSinglePM(portId, tenant);
                if (portPM != null) portCode = portPM.Code;
            }

            return portCode;
        }
        #endregion
        
        #region Not Inland Domestic
        private WarehouseEntryRouting GetWarehouseEntryRoutingForNotInlandDomestic(WarehouseEntryRoutingArgs warehouseEntryRoutingArgs)
        {
            WarehouseEntryRouting warehouseEntryRouting = new WarehouseEntryRouting();
            warehouseEntryRouting.Origin = GetWarehouseEntryOriginForNotInlandDomestic(warehouseEntryRoutingArgs);
            warehouseEntryRouting.Destination = GetWarehouseEntryDestinationForNotInlandDomestic(warehouseEntryRoutingArgs);
            warehouseEntryRouting.Routing = (warehouseEntryRouting.Origin + " > " + warehouseEntryRouting.Destination);
            return warehouseEntryRouting;
        }
        private string GetWarehouseEntryOriginForNotInlandDomestic(WarehouseEntryRoutingArgs warehouseEntryRoutingArgs)
        {
            string origin = string.Empty;
            if (warehouseEntryRoutingArgs.FromTypeCode == "PORT" && !string.IsNullOrEmpty(warehouseEntryRoutingArgs.FromPortId)) origin = GetPortCodeByPortId(warehouseEntryRoutingArgs.FromPortId, warehouseEntryRoutingArgs.Tenant);
            else if (warehouseEntryRoutingArgs.FromTypeCode == "CASL" && !string.IsNullOrEmpty(warehouseEntryRoutingArgs.FromCountryId)) origin = GetCountryNameByCountryId(warehouseEntryRoutingArgs.FromCountryId, warehouseEntryRoutingArgs.Tenant);
            return origin;

        }
        private string GetWarehouseEntryDestinationForNotInlandDomestic(WarehouseEntryRoutingArgs warehouseEntryRoutingArgs)
        {
            string destination = string.Empty;
            if (warehouseEntryRoutingArgs.ToTypeCode == "PORT" && !string.IsNullOrEmpty(warehouseEntryRoutingArgs.ToPortId)) destination = GetPortCodeByPortId(warehouseEntryRoutingArgs.ToPortId, warehouseEntryRoutingArgs.Tenant);
            else if (warehouseEntryRoutingArgs.ToTypeCode == "CASL" && !string.IsNullOrEmpty(warehouseEntryRoutingArgs.ToCountryId)) destination = GetCountryNameByCountryId(warehouseEntryRoutingArgs.ToCountryId, warehouseEntryRoutingArgs.Tenant);
            return destination;

        }

        #endregion

        #endregion
    }




    public class WarehouseEntryRouting
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Routing { get; set; }
    }

    public class WarehouseEntryRoutingArgs
    {
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string FromAddressId { get; set; }
        public string ToAddressId { get; set; }

        public string FromPortId { get; set; }
        public string ToPortId { get; set; }

        public string ToCountryId { get; set; }
        public string FromCountryId { get; set; }

        public string FromTypeCode { get; set; }
        public string ToTypeCode { get; set; }
        public int Tenant { get; set; }
    }
}
