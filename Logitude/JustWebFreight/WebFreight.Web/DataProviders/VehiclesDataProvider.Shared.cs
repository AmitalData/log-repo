using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class VehiclesDataProvider : BaseDataProvider
    {

        public List<VehiclePackageRecord> ShipmentPackages { get; set; }

    }

    public class VehiclePackageRecord
    {
        public string ShipmentNumber { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate  { get; set; }
        public string Carrier { get; set; }
        public string CarrierNumber { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryofManufacture { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public string VehicleType  { get; set; }
        
    }



}