using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

namespace WebFreight.Web.Controllers.ShipmentsModel.Generated.PMControllers
{
    public class ContainerFollowUpController : ApiController
    {
        public HttpResponseMessage GetSingle(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("ContainerFollowUp", "READ", tenant);

                IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);

                ContainerFollowUpPM entityPM = (from f in myContext.ShipmentPackages.Include("PackageType")
                                                join db_Shipments in myContext.Shipments.Include("Direction").Include("TransportMode").Include("ShipmentLevel").Include("ShipmentType").Include("ShipperCard").Include("ConsigneeCard").Include("CustomerCard").Include("CustomerCard.PrimaryContact").Include("ShipmentMasterData").Include("ShipmentMasterData.MainCarriageCarrierCard").Include("ShipmentMasterData.MainCarriageVessel")
                                                on f.ShipmentId equals db_Shipments.Id into PackagesShipments
                                                from myShipment in PackagesShipments
                                                where f.Tenant == tenant && myShipment.Tenant == tenant
                                                && f.Id == id
                                                select new ContainerFollowUpPM()
                                                {
                                                    Id = f.Id,
                                                    Tenant = f.Tenant,
                                                    ShipmentId = f.ShipmentId,
                                                    ShipperSeal = f.ShipperSeal,
                                                    Volume = f.Volume,
                                                    ContainerNumber = f.ContainerNumber,
                                                    IsDangerous = f.IsDangerous,
                                                    Description = f.Description,
                                                    MarksAndNumbers = f.MarksAndNumbers,
                                                    ContainerTypeName = f.PackageType == null ? null : f.PackageType.EnglishName,
                                                    IsDeliveryFU = f.IsDeliveryFU,
                                                    DeliveryId = f.DeliveryId,
                                                    DeliveryETD = f.DeliveryETD,
                                                    DeliveryATD = f.DeliveryATD,
                                                    DeliveryATA = f.DeliveryATA,
                                                    DeliveryETA = f.DeliveryETA,
                                                    DeliveryFrom = f.DeliveryFrom,
                                                    DeliveryTo = f.DeliveryTo,
                                                    DeliveryDeparture = f.DeliveryATD != null ? f.DeliveryATD : f.DeliveryETD,
                                                    DeliveryArrival = f.DeliveryATA != null ? f.DeliveryATA : f.DeliveryETA,
                                                    IsEmptyContainerReturnFU = f.IsEmptyContainerReturnFU,
                                                    EmptyContainerReturnId = f.EmptyContainerReturnId,
                                                    EmptyContainerReturnETD = f.EmptyContainerReturnETD,
                                                    EmptyContainerReturnATD = f.EmptyContainerReturnATD,
                                                    EmptyContainerReturnETA = f.EmptyContainerReturnETA,
                                                    EmptyContainerReturnATA = f.EmptyContainerReturnATA,
                                                    EmptyContainerReturnFrom = f.EmptyContainerReturnFrom,
                                                    EmptyContainerReturnTo = f.EmptyContainerReturnTo,
                                                    ReturnDeparture = f.EmptyContainerReturnATD != null ? f.EmptyContainerReturnATD : f.EmptyContainerReturnETD,
                                                    ReturnArrival = f.EmptyContainerReturnATA != null ? f.EmptyContainerReturnATA : f.EmptyContainerReturnETA,

                                                    DirectionId = myShipment.DirectionId,
                                                    TransportModeId = myShipment.TransportModeId,
                                                    ShipmentNumber = myShipment.ShipmentNumber,
                                                    House = myShipment.House,
                                                    ShipmentLevelCode = myShipment.ShipmentLevelCode,
                                                    StatusId = myShipment.StatusId,
                                                    ConsigneeReference = (myShipment.ConsigneeReference1 == null || myShipment.ConsigneeReference1 == "") ? myShipment.ConsigneeReference2 : ((myShipment.ConsigneeReference2 == null || myShipment.ConsigneeReference2 == "") ? myShipment.ConsigneeReference1 : myShipment.ConsigneeReference1 + "," + myShipment.ConsigneeReference2),
                                                    DirectionName = myShipment.Direction == null ? null : myShipment.Direction.Name,
                                                    TransportModeName = myShipment.TransportMode == null ? null : myShipment.TransportMode.Name,
                                                    ShipmentLevelName = myShipment.ShipmentLevel == null ? null : myShipment.ShipmentLevel.Name,
                                                    ShipmentType = myShipment.ShipmentType == null ? null : myShipment.ShipmentType.Name,
                                                    ShipperName = myShipment.ShipperCard == null ? null : myShipment.ShipperCard.EnglishName,
                                                    ConsigneeName = myShipment.ConsigneeCard == null ? null : myShipment.ConsigneeCard.EnglishName,
                                                    CustomerName = myShipment.CustomerCard == null ? null : myShipment.CustomerCard.EnglishName,
                                                    LongMaster = myShipment.ShipmentMasterData == null ? null : (myShipment.TransportModeId == "A" ? (!string.IsNullOrEmpty(myShipment.ShipmentMasterData.AirlinePrefix) && !string.IsNullOrEmpty(myShipment.ShipmentMasterData.Master) ? myShipment.ShipmentMasterData.AirlinePrefix + "-" + myShipment.ShipmentMasterData.Master : "") : myShipment.ShipmentMasterData.Master),
                                                    CarrierName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageCarrierCard == null ? null : myShipment.ShipmentMasterData.MainCarriageCarrierCard.EnglishName),
                                                    CustomerContactName = myShipment.CustomerCard == null ? null : (myShipment.CustomerCard.PrimaryContact == null ? null : myShipment.CustomerCard.PrimaryContact.EnglishName),

                                                    ShipmentNotes = myShipment.Notes,
                                                    VesselName = myShipment.ShipmentMasterData == null ? null : (myShipment.ShipmentMasterData.MainCarriageVessel == null ? null : myShipment.ShipmentMasterData.MainCarriageVessel.EnglishName),

                                                }).FirstOrDefault();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(ContainerFollowUpPM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);
                        //SecurityUtility.CheckContactFeature("ContainerFollowUp", "UPDATE", tenant);

                        IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
                        ShipmentPackageRepository myRepository = new ShipmentPackageRepository(myContext);

                        ShipmentPackage myPackage = myRepository.GetSingleShipmentPackage(entityPM.Id, entityPM.Tenant);
                        if(myPackage != null)
                        {
                            myPackage.IsDeliveryFU = entityPM.IsDeliveryFU;
                            myPackage.IsEmptyContainerReturnFU = entityPM.IsEmptyContainerReturnFU;

                            if (myPackage.DeliveryId == null)
                            {
                                myPackage.DeliveryETD = entityPM.DeliveryETD;
                                myPackage.DeliveryATD = entityPM.DeliveryATD;
                                myPackage.DeliveryETA = entityPM.DeliveryETA;
                                myPackage.DeliveryATA = entityPM.DeliveryATA;
                                myPackage.DeliveryFrom = entityPM.DeliveryFrom;
                                myPackage.DeliveryTo = entityPM.DeliveryTo;
                            }

                            if (myPackage.EmptyContainerReturnId == null)
                            {
                                myPackage.EmptyContainerReturnETD = entityPM.EmptyContainerReturnETD;
                                myPackage.EmptyContainerReturnATD = entityPM.EmptyContainerReturnATD;
                                myPackage.EmptyContainerReturnETA = entityPM.EmptyContainerReturnETA;
                                myPackage.EmptyContainerReturnATA = entityPM.EmptyContainerReturnATA;
                                myPackage.EmptyContainerReturnFrom = entityPM.EmptyContainerReturnFrom;
                                myPackage.EmptyContainerReturnTo = entityPM.EmptyContainerReturnTo;
                            }
                        }

                        myRepository.Update(myPackage);
                        myRepository.SubmitChanges();

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

    }
}