using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class ContainersFUDomainController : ApiController
    {
        public HttpResponseMessage GetQueriesCounts()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("ContainerFollowUp", "READ", tenant);

                ContainersFUSummary myResult = new ContainersFUSummary();
                myResult.Id = tenant;

                IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
                IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);
                List<string> allStatusedCodes_DEP = new List<string>();
                allStatusedCodes_DEP.Add("SDEP");
                allStatusedCodes_DEP.Add("SDE2");
                allStatusedCodes_DEP.Add("SDE3");
                allStatusedCodes_DEP.Add("SDE4");
                allStatusedCodes_DEP.Add("ONCD");

                //List<string> allStatusedCodes_ARR = new List<string>();
                //allStatusedCodes_ARR.Add("SARR");
                //allStatusedCodes_ARR.Add("SAR2");
                //allStatusedCodes_ARR.Add("SAR3");
                //allStatusedCodes_ARR.Add("SAR4");
                //allStatusedCodes_ARR.Add("SAR5");

                List<string> allStatusedCodes_ARR_UP = new List<string>();
                allStatusedCodes_ARR_UP.Add("SARR");
                allStatusedCodes_ARR_UP.Add("SAR2");
                allStatusedCodes_ARR_UP.Add("SAR3");
                allStatusedCodes_ARR_UP.Add("SAR4");
                allStatusedCodes_ARR_UP.Add("SAR5");
                allStatusedCodes_ARR_UP.Add("CERT");
                allStatusedCodes_ARR_UP.Add("DTCA");
                allStatusedCodes_ARR_UP.Add("ICCL");
                allStatusedCodes_ARR_UP.Add("SHCL");
                allStatusedCodes_ARR_UP.Add("SDL2");
                allStatusedCodes_ARR_UP.Add("SDLY");
                allStatusedCodes_ARR_UP.Add("SDLD");
                allStatusedCodes_ARR_UP.Add("INPR");
                allStatusedCodes_ARR_UP.Add("DDAP");
                allStatusedCodes_ARR_UP.Add("DDDE");

                string myObjectTableId = (from d in myFreightContext.ObjectTables where d.Name == "Shipment" select d.Id).FirstOrDefault();
                List<string> allStatusedIds_DEP = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && d.ObjectTableId == myObjectTableId && allStatusedCodes_DEP.Contains(d.Code) select d.Id).ToList();
                //List<string> allStatusedIds_ARR = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && d.ObjectTableId == myObjectTableId && allStatusedCodes_ARR.Contains(d.Code) select d.Id).ToList();
                List<string> allStatusedIds_ARR_UP = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && d.ObjectTableId == myObjectTableId && allStatusedCodes_ARR_UP.Contains(d.Code) select d.Id).ToList();

                myResult.ArrivedNotDelivered = (from myPackage in MyContext.ShipmentPackages
                                                join db_Shipments in MyContext.Shipments on myPackage.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                from myShipment in PackagesShipments
                                                where myPackage.Tenant == tenant && myShipment.Tenant == tenant
                                                && myShipment.IsCancelled == false
                                                && allStatusedIds_ARR_UP.Contains(myShipment.StatusId)
                                                &&
                                                (
                                                myPackage.IsDeliveryFU && myPackage.DeliveryATA == null
                                                //||
                                                //myPackage.IsEmptyContainerReturnFU && myPackage.EmptyContainerReturnATA == null
                                                )
                                                select myPackage).Count();

                myResult.DeliveredNotReturned = (from myPackage in MyContext.ShipmentPackages
                                                 join db_Shipments in MyContext.Shipments on myPackage.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                 from myShipment in PackagesShipments
                                                 where myPackage.Tenant == tenant && myShipment.Tenant == tenant
                                                 && myShipment.IsCancelled == false
                                                 && (myPackage.IsDeliveryFU && myPackage.DeliveryATA != null)
                                                 && (myPackage.IsEmptyContainerReturnFU && myPackage.EmptyContainerReturnATA == null)
                                                 select myPackage).Count();


                myResult.InTransit = (from myPackage in MyContext.ShipmentPackages
                                      join db_Shipments in MyContext.Shipments on myPackage.ShipmentId equals db_Shipments.Id into PackagesShipments
                                      from myShipment in PackagesShipments
                                      where
                                      myPackage.Tenant == tenant
                                      && myShipment.Tenant == tenant
                                      && myShipment.IsCancelled == false
                                      && myShipment.DirectionId == "I"
                                      &&
                                          (
                                          myShipment.TransportModeId == "O" && (myShipment.ShipmentTypeId == "FCLD" || myShipment.ShipmentTypeId == "MYGO")
                                          ||
                                          myShipment.TransportModeId == "I" && (myShipment.ShipmentTypeId == "FTL" || myShipment.ShipmentTypeId == "MYGI")
                                          )
                                      && allStatusedIds_DEP.Contains(myShipment.StatusId)

                                      // Task 44634: In Transit Query | follow up is not required
                                      //&&
                                      //(
                                      //myPackage.IsDeliveryFU
                                      //||
                                      //myPackage.IsEmptyContainerReturnFU
                                      //)
                                      select myPackage).Count();

                myResult.ContainersCount = (from container in MyContext.Containers
                                            where container.Tenant == tenant 
                                            && container.IsCancelled == false && container.IsClosed == false
                                            select container).Count();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        // StatusCode  StatusName   StatusWeight    EventTypeCode     ActionField        
        // SARR        Arrived      7               ARR               MainCarriageATA
        // SAR2        Arrived      9               T1AR              Transshipment1ATA
        // SAR3        Arrived      11              T2AR              Transshipment2ATA
        // SAR4        Arrived      13              T3AR              Transshipment3ATA
        // SAR5        Arrived      15              ONCA              OnCarriageATA
    }

    public class ContainersFUSummary
    {
        [Key]
        public int Id { get; set; }
        public int InTransit { get; set; }
        public int ArrivedNotDelivered { get; set; }
        public int DeliveredNotReturned { get; set; }
        public int ContainersCount { get; set; }
    }
}