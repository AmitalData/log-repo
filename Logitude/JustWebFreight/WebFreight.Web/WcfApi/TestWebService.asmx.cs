using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebFreight.Web.CommonDataModel.EntityPMs;
using WebFreight.Web.ShipmentsModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    /// <summary>
    /// Summary description for TestWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TestWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public void Test()
        {
           AddressWcfService addressService = new AddressWcfService();
            AddressPM address = new AddressPM()
            {
                ExternalId = "E00001",
                Tenant = 1,
                Address1 = "Al Beireh",
                Address2 = "Jawwal",
                AddressTypeId = "M", // M: main address, B: billing // O:Other
                ATTN = "11111",
                CardId = "70002",
                City = "Ramallah",
                CountryId = "PS",
                Name = "Main Address",
                ZipCode = "0972",

            };

            addressService.Upsert(address, false);

            //ShipmentWcfService service = new ShipmentWcfService();
            //ShipmentPM shipment = new ShipmentPM()
            //{
            //    ShipperId = "70000",
            //    ConsigneeId = "70002",
            //    FromPortId = "FRA",
            //    ToPortId = "FTU",
            //    DirectionId = "E",
            //    TransportModeId = "A",
            //    Tenant = 1,
            //    //StatusId = "SHOR",
            //    ShipmentLevelCode = "D",
            //    MainCarriageFromPortId = "FRA",
            //    MainCarriageToPortId = "FTU",
            //    FreightPrepaidCollectId = "P",
            //    OtherPrepaidCollectId = "P",
            //    BranchId = "HybridB1",
            //    DepartmentId = "HybridD1",
            //    AWBCurrencyId = "USD",
            //    ChargeableWeightUnitCode = "KG",
            //    DimensionsUnitCode = "Cm",
            //    //FHLStatusCode = "NSEN",
            //    // SalesmanUserId = "HybridU1",
            //    ProfitCurrencyId = "USD",
            //    ShipmentCustomerTypeCode = "SHI",
            //    VolumeUnitCode = "CBM",
            //    ShipmentNumber = "555555",
            //    CreatedByUserId = "HybridSystemUser",

            //    //Notes="updated 111",
            //};
          

            //service.Upsert(shipment, false);
        }
    }
}
