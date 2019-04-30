using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class HouseController : ApiController
    {
        public HttpResponseMessage GetSingleHouse(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

				SecurityUtility.AuthenticateAPICall(tenant);

				HouseQueryService Service = new HouseQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetHouseById(id, tenant);
                //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleHouseByNumber(string number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

				SecurityUtility.AuthenticateAPICall(tenant);

				HouseQueryService Service = new HouseQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetHouseByShipmentNumber(number, tenant);
                //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(House entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

					SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                    IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);

                    ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                    string computingPartnerCode = "";
                    if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                    {
                        computingPartnerCode = entity.ComputingPartnerCode;
                    }

                    if (!string.IsNullOrEmpty(entity.HouseNo))
                    {
                        bool exist = (from a in MyContext.Shipments
                                      where a.Tenant == authToken.Tenant
                                      && a.ShipmentLevelCode == "H"
                                      && !string.IsNullOrEmpty(a.House)
                                      && a.House == entity.HouseNo
                                      select a).Any();

                        if (exist)
                        {
                            throw new ApplicationException("A House with the given house number already exists");
                        }
                    }

                    if (entity.TransportMode != null && entity.ShipmentType != null)
                    {
                        if (entity.TransportMode.Code != "A")
                        {
                            if (entity.OceanOrInlandPackages != null && entity.OceanOrInlandPackages.Count > 0)
                            {
                                foreach (OceanOrInlandPackage item in entity.OceanOrInlandPackages)
                                {
                                    if (item.PackageType == null)
                                    {
                                        string message = entity.ShipmentType.Code.Contains("LCL") ? "Package Type is required" : "Container Type is required";
                                        throw new ApplicationException(message);
                                    }

                                    else
                                    {
                                        if (entity.ShipmentType.Code.Contains("FCL"))
                                        {
                                            if (item.Pieces == null || item.Pieces == 0)
                                            {
                                                item.Pieces = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (entity.TransportMode != null && entity.TransportMode.Code != "A")
                    {
                        if (entity.ShipmentType == null || (entity.ShipmentType != null && string.IsNullOrEmpty(entity.ShipmentType.Code)))
                        {
                            throw new ApplicationException("Missing Shipment Type");
                        }
                    }

                    if (entity.Receivables != null && entity.Receivables.Count > 0)
                    {
                        foreach (Receivable item in entity.Receivables)
                        {
                            if(item.ChargesType == null)
                            {
                                throw new ApplicationException("Receivable Charges Type is required");
                            }
                            
                            if (item.Currency == null)
                            {
                                throw new ApplicationException("Receivable Currency is required");
                            }
                        }
                    }

                    if (entity.Payables != null && entity.Payables.Count > 0)
                    {
                        foreach (Payable item in entity.Payables)
                        {
                            if (item.ChargesType == null)
                            {
                                throw new ApplicationException("Payable Charges Type is required");
                            }
                            
                            if (item.Currency == null)
                            {
                                throw new ApplicationException("Payable Currency is required");
                            }
                        }
                    }
                    
                    HouseQueryService mappingService = new HouseQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.HouseCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        switch(entityPM.DirectionId)
                        {
                            case "I":
                                {
                                    if(string.IsNullOrEmpty(entityPM.ConsigneeId))
                                    {
                                        throw new ApplicationException("Consignee is required for import houses");
                                    }

                                    break;
                                }

                            case "E":
                                {
                                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                                    {
                                        throw new ApplicationException("Shipper is required for export houses");
                                    }

                                    break;
                                }

                            case "D":
                                {
                                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                                    {
                                        throw new ApplicationException("Shipper is required for domestic houses");
                                    }

                                    break;
                                }

                            case "R":
                                {
                                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                                    {
                                        throw new ApplicationException("Shipper is required for drop houses");
                                    }

                                    break;
                                }
                        }

                        if (string.IsNullOrEmpty(entityPM.VolumeUnitCode))
                        {
                            throw new ApplicationException("Missing volume unit code");
                        }

                        if (string.IsNullOrEmpty(entityPM.DimensionsUnitCode))
                        {
                            throw new ApplicationException("Missing dimensions unit code");
                        }

                        if (string.IsNullOrEmpty(entityPM.GrossWeightUnitCode))
                        {
                            throw new ApplicationException("Missing gross weight unit code");
                        }

                        if (string.IsNullOrEmpty(entityPM.ChargeableWeightUnitCode))
                        {
                            throw new ApplicationException("Missing chargeable weight unit code");
                        }

                        switch (entityPM.VolumeUnitCode)
                        {
                            case "CBF":
                                {
                                    if (entityPM.DimensionsUnitCode == "Cm")
                                    {
                                        throw new ApplicationException("When volume unit is CBF, dimensions unit should be Inch or Cm");
                                    }
                                    break;
                                }

                            case "CBI":
                                {
                                    if (entityPM.DimensionsUnitCode != "Inc")
                                    {
                                        throw new ApplicationException("When volume unit is CBI, dimensions unit should be Inch");
                                    }
                                    break;
                                }

                            case "CBM":
                                {
                                    if (entityPM.DimensionsUnitCode != "Cm")
                                    {
                                        throw new ApplicationException("When volume unit is CBM, dimensions unit should be Cm");
                                    }
                                    break;
                                }
                        }

                        if (!string.IsNullOrEmpty(entityPM.IncotermId))
                        {
                            IncotermRepository myIncotermRepository = new IncotermRepository(entityPM.Tenant);
                            Incoterm myIncoterm = myIncotermRepository.GetSingleIncoterm(entityPM.IncotermId, entityPM.Tenant);
                            if (myIncoterm != null)
                            {
                                entityPM.FreightPrepaidCollectId = myIncoterm.Freight;
                                entityPM.OtherPrepaidCollectId = myIncoterm.OtherCharges;
                            }
                        }

                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        if (!string.IsNullOrEmpty(entityPM.CustomerId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.CustomerId, authToken.Tenant);
                            if (address != null)
                            {
                                entityPM.CustomerAddressId = address.Id;
                            }

                            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                            Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                            if (customer != null)
                            {
                                entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
                                entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
                            }
                        }

                        else
                        {
                            throw new ApplicationException("Customer is missing");
                        }
                        
                        if (!string.IsNullOrEmpty(entityPM.ShipperId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, authToken.Tenant);
                            if(address != null)
                            {
                                entityPM.ShipperAddressId = address.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.ConsigneeId, authToken.Tenant);
                            if (address != null)
                            {
                                entityPM.ConsigneeAddressId = address.Id;
                            }
                        }
                        
                        if (entityPM.ShipmentPackages.Count > 0)
                        {
                            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                            {
                                if (entityPM.ShipmentTypeId == "FCLD" || entityPM.ShipmentTypeId == "FTL")
                                {
                                    item.IsContainer = true;
                                }

                                item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                                item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                            }
                        }

                        ComputeHelper.ComputeTotals(entityPM);
                        APIReceivablePayableHelper receivablePayableHelper = new APIReceivablePayableHelper(entityPM, authToken.Tenant);
                        receivablePayableHelper.ValidateReceivablesAndPayables();
                        receivablePayableHelper.ComputeReceivablesPayablesTotals();

                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        scope.Complete();
                    }


                    var result = mappingService.GetHouseById(entityPM.Id, authToken.Tenant);
                    APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "House API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        
        public HttpResponseMessage Put(House entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        string token = HttpContext.Current.Request.Headers["Token"];
            //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            //        ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
            //        string computingPartnerCode = "";
            //        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode)) computingPartnerCode = entity.ComputingPartnerCode;//loggedContactInfo.ComputingPartnerCode;

            //        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
            //        HouseQueryService mappingService = new HouseQueryService(authToken.Tenant);
            //        ShipmentPM entityPM = mappingService.HouseDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

            //        using (TransactionScope scope = TransactionFactory.GetTransaction())
            //        {
            //            ShipmentRepository entityRepository = new ShipmentRepository(MyContext);
            //            Shipment entityPoco = null;
            //            entityPoco = entityRepository.GetSingleShipment(entityPM.Id, authToken.Tenant);

            //            if (entityPoco == null)
            //            {
            //                throw new ApplicationException("No shipment found");
            //            }

            //            else
            //            {
            //                if (!string.IsNullOrEmpty(entityPM.MasterShipmentDataId) || entityPM.IsOperationalClosed || entityPM.IsAccountingClosed || entityPM.IsCancelled)
            //                {
            //                    this.CheckEntityChanges(entityPM, entityPoco, MyContext, authToken.Tenant);
            //                }

            //                else
            //                {
            //                    this.DoUpdate(entityPM, MyContext, authToken.Tenant);
            //                }
            //            }


            //            scope.Complete();
            //        }

            //        var result = mappingService.GetHouseById(entityPM.Id, authToken.Tenant);
            //        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "House API", authToken.Tenant);
            //        return Request.CreateResponse(HttpStatusCode.OK, result);
            //    }

            //    catch (Exception ex)
            //    {
            //        var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
            //        APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
            //        return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            //    }
            //}

            //else
            //{
            //    var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
            //    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
            //    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);

            //}
        }

        private void CheckEntityChanges(ShipmentPM entityPM, Shipment entityPoco, IShipmentsContext context, int tenant)
        {
            if (!string.IsNullOrEmpty(entityPM.MasterShipmentDataId) && !entityPM.IsOperationalClosed && !entityPM.IsAccountingClosed && !entityPM.IsCancelled)
            {
                if (entityPM.FromPortId != entityPoco.FromPortId)
                {
                    throw new ApplicationException("You can't change from port, because this house is connected to master");
                }

                else if (entityPM.ToPortId != entityPoco.ToPortId)
                {
                    throw new ApplicationException("You can't change to port, because this house is connected to master");
                }

                else
                {
                    this.DoUpdate(entityPM, context, tenant);
                }
            }

            else if (entityPM.IsOperationalClosed || entityPM.IsAccountingClosed || entityPM.IsCancelled)
            {
                string message = "closed";
                if (entityPM.IsCancelled)
                {
                    message = "cancelled";
                }
                else if(entityPM.IsAccountingClosed)
                {
                    message = "accounting closed";
                }
                else if (entityPM.IsOperationalClosed)
                {
                    message = "operational closed";
                }

                if (entityPM.ShipperId != entityPoco.ShipperId)
                {
                    throw new ApplicationException("You can't change shipper, because this house is " + message);
                }

                else if (entityPM.ConsigneeId != entityPoco.ConsigneeId)
                {
                    throw new ApplicationException("You can't change consignee, because this house is " + message);
                }

                else if (entityPM.CustomerId != entityPoco.CustomerId)
                {
                    throw new ApplicationException("You can't change customer, because this house is " + message);
                }

                else if (entityPM.FromPortId != entityPoco.FromPortId)
                {
                    throw new ApplicationException("You can't change from port, because this house is " + message);
                }

                else if (entityPM.House != entityPoco.House)
                {
                    throw new ApplicationException("You can't change house, because this house is " + message);
                }

                else if (entityPM.IncotermId != entityPoco.IncotermId)
                {
                    throw new ApplicationException("You can't change incoterm, because this house is " + message);
                }

                else if (entityPM.ToPortId != entityPoco.ToPortId)
                {
                    throw new ApplicationException("You can't change to port, because this house is " + message);
                }

                else if (entityPM.ChargeableWeightUnitCode != entityPoco.ChargeableWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Chargeable Weight Unit, because this house is " + message);
                }

                else if (entityPM.GrossWeightUnitCode != entityPoco.GrossWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Gross Weight Unit, because this house is " + message);
                }

                else if (entityPM.VolumeUnitCode != entityPoco.VolumeUnitCode)
                {
                    throw new ApplicationException("You can't change Volume Unit, because this house is " + message);
                }

                else if (entityPM.HAWBDate != entityPoco.HAWBDate)
                {
                    throw new ApplicationException("You can't change HAWB Date, because this house is " + message);
                }

                else if (entityPM.DescriptionOfGoods != entityPoco.DescriptionOfGoods)
                {
                    throw new ApplicationException("You can't change Description Of Goods, because this house is " + message);
                }

                else if (entityPM.BranchId != entityPoco.BranchId)
                {
                    throw new ApplicationException("You can't change Branch, because this house is " + message);
                }

                else if (entityPM.DepartmentId != entityPoco.DepartmentId)
                {
                    throw new ApplicationException("You can't change Department, because this house is " + message);
                }

                else
                {
                    ShipmentPackageRepository packageRepository = new ShipmentPackageRepository(entityPM.Tenant);
                    List<ShipmentPackage> packages = packageRepository.GetShipmentPackagesForShipmentTenant(entityPM.Id, entityPM.Tenant).ToList();

                    if (packages.Count < entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't add packages, because this house is " + message);
                    }

                    else if (packages.Count > entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't delete packages, because this house is " + message);
                    }

                    else
                    {
                        foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
                        {
                            ShipmentPackage item = packages.Where(d => d.Id == itemPM.Id).FirstOrDefault();

                            if (itemPM.PackageTypeId != item.PackageTypeId)
                            {
                                throw new ApplicationException("You can't change Package Type, because this house is " + message);
                            }

                            else if (itemPM.Length != item.Length)
                            {
                                throw new ApplicationException("You can't change Length, because this house is " + message);
                            }

                            else if (itemPM.Width != item.Width)
                            {
                                throw new ApplicationException("You can't change Width, because this house is " + message);
                            }

                            else if (itemPM.Height != item.Height)
                            {
                                throw new ApplicationException("You can't change Height, because this house is " + message);
                            }

                            else if (itemPM.Quantity != item.Quantity)
                            {
                                throw new ApplicationException("You can't change Quantity, because this house is " + message);
                            }

                            else if (itemPM.Volume != item.Volume)
                            {
                                throw new ApplicationException("You can't change Volume, because this house is " + message);
                            }

                            else if (itemPM.Weight != item.Weight)
                            {
                                throw new ApplicationException("You can't change Weight, because this house is " + message);
                            }

                            else if (itemPM.Reference1 != item.Reference1)
                            {
                                throw new ApplicationException("You can't change Reference 1, because this house is " + message);
                            }

                            else if (itemPM.Reference2 != item.Reference2)
                            {
                                throw new ApplicationException("You can't change Reference 2, because this house is " + message);
                            }

                            else if (itemPM.Reference3 != item.Reference3)
                            {
                                throw new ApplicationException("You can't change Reference 3, because this house is " + message);
                            }

                            else if (itemPM.CommodityNumber != item.CommodityNumber)
                            {
                                throw new ApplicationException("You can't change Commodity Number, because this house is " + message);
                            }

                            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
                            {
                                if (itemPM.ShipperSeal != item.ShipperSeal)
                                {
                                    throw new ApplicationException("You can't change Shipper Seal, because this house is " + message);
                                }

                                else if (itemPM.CarrierSeal != item.CarrierSeal)
                                {
                                    throw new ApplicationException("You can't change Carrier Seal, because this house is " + message);
                                }
                            }

                            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId) && entityPM.ShipmentTypeId != "FCLD")
                            {
                                if (itemPM.Harmonize != item.Harmonize)
                                {
                                    throw new ApplicationException("You can't change Length, because this house is " + message);
                                }

                                else if (itemPM.Temperature != item.Temperature)
                                {
                                    throw new ApplicationException("You can't change Temperature, because this house is " + message);
                                }

                                else if (itemPM.Ventilation != item.Ventilation)
                                {
                                    throw new ApplicationException("You can't change Ventilation, because this house is " + message);
                                }

                                else if (itemPM.IsDangerous != item.IsDangerous)
                                {
                                    throw new ApplicationException("You can't change Is Dangerous, because this house is " + message);
                                }

                                else if (itemPM.ClassNumber != item.ClassNumber)
                                {
                                    throw new ApplicationException("You can't change Class Number, because this house is " + message);
                                }

                                else if (itemPM.UnNumber != item.UnNumber)
                                {
                                    throw new ApplicationException("You can't change Un Number, because this house is " + message);
                                }

                                else if (itemPM.PackagingGroup != item.PackagingGroup)
                                {
                                    throw new ApplicationException("You can't change Packaging Group, because this house is " + message);
                                }

                                else if (itemPM.IMDGCode != item.IMDGCode)
                                {
                                    throw new ApplicationException("You can't change IMDG Code, because this house is " + message);
                                }

                                else if (itemPM.FlashPoint != item.FlashPoint)
                                {
                                    throw new ApplicationException("You can't change Flash Point, because this house is " + message);
                                }

                                else if (itemPM.MaterialDescription != item.MaterialDescription)
                                {
                                    throw new ApplicationException("You can't change Material Description, because this house is " + message);
                                }
                            }

                            if (entityPM.ShipmentTypeId == "FCLD")
                            {
                                if (itemPM.Tare != item.Tare)
                                {
                                    throw new ApplicationException("You can't change Tare, because this house is " + message);
                                }

                                else if (itemPM.MarksAndNumbers != item.MarksAndNumbers)
                                {
                                    throw new ApplicationException("You can't change Marks And Numbers, because this house is " + message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DoUpdate(ShipmentPM entityPM, IShipmentsContext MyContext, int tenant)
        {
            if (!string.IsNullOrEmpty(entityPM.IncotermId))
            {
                IncotermRepository myIncotermRepository = new IncotermRepository(entityPM.Tenant);
                Incoterm myIncoterm = myIncotermRepository.GetSingleIncoterm(entityPM.IncotermId, entityPM.Tenant);
                if (myIncoterm != null)
                {
                    entityPM.FreightPrepaidCollectId = myIncoterm.Freight;
                    entityPM.OtherPrepaidCollectId = myIncoterm.OtherCharges;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                if (customer != null)
                {
                    entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
                    entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
                }
            }

            if (entityPM.ShipmentPackages.Count > 0)
            {
                foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                {
                    item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                    item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                }
            }

            ComputeHelper.ComputeTotals(entityPM);

            ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
            ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(MyContext));
            List<ShipmentPackagePM> shipmentPackages = shipPackageQuery.GetShipmentPackages(entityPM.Id, entityPM.ShipmentNumber, tenant);
            foreach (ShipmentPackagePM package in shipmentPackages)
            {
                package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                entityPM.ShipmentPackages.Add(package);
            }

            service.Update(true);
        }
    }
}