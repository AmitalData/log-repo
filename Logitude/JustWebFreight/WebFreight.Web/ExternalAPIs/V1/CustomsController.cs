using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
    public class CustomsController : ApiController
    {
        public HttpResponseMessage GetSingleCustoms(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				CustomsQueryService Service = new CustomsQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetCustomsById(id, tenant);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleCustomsByNumber(string number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				CustomsQueryService Service = new CustomsQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetCustomsByShipmentNumber(number, tenant);
                //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(Customs entity)
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
                    CustomsQueryService mappingService = new CustomsQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.CustomsCustomDataMappingAndValidatin(entity, authToken.Tenant);
                    entityPM.IsExternalAPI = true;
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
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
   
                        if (!string.IsNullOrEmpty(entityPM.ShipperId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, authToken.Tenant);
                            if (address != null)
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
                                item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                                item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                            }
                        }

                        ComputeHelper.ComputeTotals(entityPM);
                       

                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        if (!string.IsNullOrEmpty(entityPM.ShipmentNumber))
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(MyContext);
                            ShipmentQuery myShipmentQuery = new ShipmentQuery(shipmentRepository);
                            ShipmentPM shipment= myShipmentQuery.GetSingleShipmentPMByNumber(entityPM.ShipmentNumber, authToken.Tenant);
                            if (shipment != null)
                                throw new ApplicationException("The Shipment Number already exists");
                        }


                        service.Create();
                        scope.Complete();



                    }
                    var result = mappingService.GetCustomsById(entityPM.Id, authToken.Tenant);
                    APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Customs API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment",null, "Customs API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Customs API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Customs entity)
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
                    CustomsQueryService mappingService = new CustomsQueryService(authToken.Tenant);
                    ShipmentQuery query = new ShipmentQuery(authToken.Tenant);
                    ShipmentPM entityPM = query.GetSinglePMByShipmentNumber(entity.ShipmentNumber, authToken.Tenant);
                    

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        ShipmentRepository entityRepository = new ShipmentRepository(MyContext);
                        if (string.IsNullOrEmpty(entity.ShipmentNumber))
                        {
                            throw new ApplicationException("No shipment number found");
                        }
                        else
                        {
                            Shipment entityPoco = entityRepository.GetSingleShipmentByShipmentNumber(entity.ShipmentNumber, authToken.Tenant);
                            if (entityPoco == null)
                            {
                                throw new ApplicationException("No shipment with such shipment number");
                            }
                            else
                            {
                                ShipmentPM directPM = mappingService.CustomsCustomDataMappingAndValidatin(entity, authToken.Tenant);
                                directPM.IsExternalAPI = true;
                                directPM.VolumeUnitCode = entity.VolumeUnit!=null? entity.VolumeUnit.Code:null;
                                directPM.GrossWeightUnitCode = entity.GrossWeightUnit!=null? entity.GrossWeightUnit.Code:null;
                                directPM.ChargeableWeightUnitCode = entity.ChargeableWeightUnit!=null? entity.ChargeableWeightUnit.Code:null;

                                this.MapCustomsToEntityPM(directPM, entityPM, MyContext);


                                if (entityPM.IsOperationalClosed || entityPM.IsAccountingClosed || entityPM.IsCancelled)
                                {
                                    this.CheckEntityChanges(entityPM, entityPoco);
                                    this.RemovePackages(directPM, entityPM, MyContext);
                                }
                                else
                                {
                                    this.RemovePackages(directPM, entityPM, MyContext);
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
                                        }
                                    }

                                    ComputeHelper.ComputeTotals(entityPM);

                                    ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());

                                    service.Update(true);
                                }
                            }
                        }

                       
                        scope.Complete();
                    }

                    var result = mappingService.GetCustomsById(entityPM.Id, authToken.Tenant);

                    APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Customs API", authToken.Tenant);

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }


                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Customs API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Customs API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private void CheckEntityChanges(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (entityPM.IsOperationalClosed)
            {
                if (entityPM.ShipperId != entityPoco.ShipperId)
                {
                    throw new ApplicationException("You can't change shipper, because this direct is closed");
                }

                else if (entityPM.ConsigneeId != entityPoco.ConsigneeId)
                {
                    throw new ApplicationException("You can't change consignee, because this direct is closed");
                }

                else if (entityPM.CustomerId != entityPoco.CustomerId)
                {
                    throw new ApplicationException("You can't change customer, because this direct is closed");
                }

                else if (entityPM.FromPortId != entityPoco.FromPortId)
                {
                    throw new ApplicationException("You can't change from port, because this direct is closed");
                }

                else if (entityPM.ToPortId != entityPoco.ToPortId)
                {
                    throw new ApplicationException("You can't change to port, because this direct is closed");
                }

                else if (entityPM.ChargeableWeightUnitCode != entityPoco.ChargeableWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Chargeable Weight Unit, because this direct is closed");
                }

                else if (entityPM.GrossWeightUnitCode != entityPoco.GrossWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Gross Weight Unit, because this direct is closed");
                }

                else if (entityPM.VolumeUnitCode != entityPoco.VolumeUnitCode)
                {
                    throw new ApplicationException("You can't change Volume Unit, because this direct is closed");
                }

                else if (entityPM.HAWBDate != entityPoco.HAWBDate)
                {
                    throw new ApplicationException("You can't change HAWB Date, because this direct is closed");
                }

                else if (entityPM.DescriptionOfGoods != entityPoco.DescriptionOfGoods)
                {
                    throw new ApplicationException("You can't change Description Of Goods, because this direct is closed");
                }

                else if (entityPM.BranchId != entityPoco.BranchId)
                {
                    throw new ApplicationException("You can't change Branch, because this direct is closed");
                }

                else if (entityPM.DepartmentId != entityPoco.DepartmentId)
                {
                    throw new ApplicationException("You can't change Department, because this direct is closed");
                }

                else if (entityPM.ShipmentPackages.Count > 0)
                {
                    ShipmentPackageRepository packageRepository = new ShipmentPackageRepository(entityPM.Tenant);
                    List<ShipmentPackage> packages = packageRepository.GetShipmentPackagesForShipmentTenant(entityPM.Id, entityPM.Tenant).ToList();

                    if (packages.Count < entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't add packages, because this direct is closed");
                    }

                    else if (packages.Count > entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't delete packages, because this direct is closed");
                    }

                    else
                    {
                        foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
                        {
                            ShipmentPackage item = packages.Where(d => d.Id == itemPM.Id).FirstOrDefault();

                            if (itemPM.PackageTypeId != item.PackageTypeId)
                            {
                                throw new ApplicationException("You can't change Package Type, because this direct is closed");
                            }

                            else if (itemPM.Length != item.Length)
                            {
                                throw new ApplicationException("You can't change Length, because this direct is closed");
                            }

                            else if (itemPM.Width != item.Width)
                            {
                                throw new ApplicationException("You can't change Width, because this direct is closed");
                            }

                            else if (itemPM.Height != item.Height)
                            {
                                throw new ApplicationException("You can't change Height, because this direct is closed");
                            }

                            else if (itemPM.Quantity != item.Quantity)
                            {
                                throw new ApplicationException("You can't change Quantity, because this direct is closed");
                            }

                            else if (itemPM.Volume != item.Volume)
                            {
                                throw new ApplicationException("You can't change Volume, because this direct is closed");
                            }

                            else if (itemPM.Weight != item.Weight)
                            {
                                throw new ApplicationException("You can't change Weight, because this direct is closed");
                            }

                            else if (itemPM.Reference1 != item.Reference1)
                            {
                                throw new ApplicationException("You can't change Reference 1, because this direct is closed");
                            }

                            else if (itemPM.Reference2 != item.Reference2)
                            {
                                throw new ApplicationException("You can't change Reference 2, because this direct is closed");
                            }

                            else if (itemPM.Reference3 != item.Reference3)
                            {
                                throw new ApplicationException("You can't change Reference 3, because this direct is closed");
                            }

                            else if (itemPM.CommodityNumber != item.CommodityNumber)
                            {
                                throw new ApplicationException("You can't change Commodity Number, because this direct is closed");
                            }

                            if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode))
                            {
                                if (itemPM.ShipperSeal != item.ShipperSeal)
                                {
                                    throw new ApplicationException("You can't change Shipper Seal, because this direct is closed");
                                }

                                else if (itemPM.CarrierSeal != item.CarrierSeal)
                                {
                                    throw new ApplicationException("You can't change Carrier Seal, because this direct is closed");
                                }
                            }

                            if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode) && entityPM.ShipmentLevelCode != "FCLD")
                            {
                                if (itemPM.Harmonize != item.Harmonize)
                                {
                                    throw new ApplicationException("You can't change Length, because this direct is closed");
                                }

                                else if (itemPM.Temperature != item.Temperature)
                                {
                                    throw new ApplicationException("You can't change Temperature, because this direct is closed");
                                }

                                else if (itemPM.Ventilation != item.Ventilation)
                                {
                                    throw new ApplicationException("You can't change Ventilation, because this direct is closed");
                                }

                                else if (itemPM.IsDangerous != item.IsDangerous)
                                {
                                    throw new ApplicationException("You can't change Is Dangerous, because this direct is closed");
                                }

                                else if (itemPM.ClassNumber != item.ClassNumber)
                                {
                                    throw new ApplicationException("You can't change Class Number, because this direct is closed");
                                }

                                else if (itemPM.UnNumber != item.UnNumber)
                                {
                                    throw new ApplicationException("You can't change Un Number, because this direct is closed");
                                }

                                else if (itemPM.PackagingGroup != item.PackagingGroup)
                                {
                                    throw new ApplicationException("You can't change Packaging Group, because this direct is closed");
                                }

                                else if (itemPM.IMDGCode != item.IMDGCode)
                                {
                                    throw new ApplicationException("You can't change IMDG Code, because this direct is closed");
                                }

                                else if (itemPM.FlashPoint != item.FlashPoint)
                                {
                                    throw new ApplicationException("You can't change Flash Point, because this direct is closed");
                                }

                                else if (itemPM.MaterialDescription != item.MaterialDescription)
                                {
                                    throw new ApplicationException("You can't change Material Description, because this direct is closed");
                                }
                            }

                            if (entityPM.ShipmentLevelCode == "FCLD")
                            {
                                if (itemPM.Tare != item.Tare)
                                {
                                    throw new ApplicationException("You can't change Tare, because this direct is closed");
                                }

                                else if (itemPM.MarksAndNumbers != item.MarksAndNumbers)
                                {
                                    throw new ApplicationException("You can't change Marks And Numbers, because this direct is closed");
                                }
                            }
                        }
                    }
                }
            }

            else if (entityPM.IsAccountingClosed)
            {
                throw new ApplicationException("Shipment Is Accounting closed, you can't do any change");
            }

            else if (entityPM.IsCancelled)
            {
                throw new ApplicationException("Shipment Is cancelled, you can't do any change");
            }
        }

        private void RemovePackages(ShipmentPM directPM, ShipmentPM entityPM, IShipmentsContext MyContext)
        {
            ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(MyContext));
            entityPM.ShipmentPackages = shipPackageQuery.GetShipmentPackages(entityPM.Id, entityPM.ShipmentNumber, entityPM.Tenant);
            foreach (ShipmentPackagePM package in entityPM.ShipmentPackages)
            {
                package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }

            if (directPM.ShipmentPackages.Count > 0)
            {
                foreach (ShipmentPackagePM package in directPM.ShipmentPackages)
                {
                    package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    entityPM.ShipmentPackages.Add(package);
                }
            }

        }
        
        private void MapCustomsToEntityPM(ShipmentPM customsPM, ShipmentPM entityPM, IShipmentsContext MyContext)
        {
            entityPM.ShipmentLevelCode = customsPM.ShipmentLevelCode;
            entityPM.DirectionId = customsPM.DirectionId;
            entityPM.TransportModeId = customsPM.TransportModeId;
            entityPM.ShipperId = customsPM.ShipperId;
            entityPM.ShipperReference1 = customsPM.ShipperReference1;
            entityPM.ShipperReference2 = customsPM.ShipperReference2;
            entityPM.ConsigneeId = customsPM.ConsigneeId;
            entityPM.ConsigneeReference1 = customsPM.ConsigneeReference1;
            entityPM.ConsigneeReference2 = customsPM.ConsigneeReference2;
            entityPM.House = customsPM.House;
            entityPM.HAWBDate = customsPM.HAWBDate;
            entityPM.CustomsClearanceDate = customsPM.CustomsClearanceDate;
            entityPM.CustomerId = customsPM.CustomerId;
            entityPM.FromPortId = customsPM.FromPortId;
            entityPM.ToPortId = customsPM.ToPortId;
            entityPM.GrossWeightUnitCode = customsPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = customsPM.ChargeableWeightUnitCode;
            entityPM.VolumeUnitCode = customsPM.VolumeUnitCode;
            entityPM.IncotermCode = customsPM.IncotermCode;
            entityPM.IncotermId = customsPM.IncotermId;
            entityPM.DescriptionOfGoods = customsPM.DescriptionOfGoods;
            entityPM.ShipmentPackages = customsPM.ShipmentPackages;
            entityPM.BranchId = customsPM.BranchId;
            entityPM.BranchId = customsPM.BranchId;
            entityPM.DepartmentId = customsPM.DepartmentId;
            entityPM.TEU = customsPM.TEU;
            entityPM.NumberOfPackages = customsPM.NumberOfPackages;
            entityPM.GrossWeight = customsPM.GrossWeight;
            entityPM.Volume = customsPM.Volume;
            entityPM.VolumetricWeight = customsPM.VolumetricWeight;
            entityPM.ChargeableWeight = customsPM.ChargeableWeight;
            entityPM.Master = customsPM.Master;
            entityPM.MainCarriageCarrierCode = customsPM.MainCarriageCarrierCode;
            entityPM.MainCarriageCarrierId = customsPM.MainCarriageCarrierId;
            entityPM.MainCarriageFromPortId = customsPM.MainCarriageFromPortId;
            entityPM.MainCarriageToPortId = customsPM.MainCarriageToPortId;
            entityPM.DeclarationNumber = customsPM.DeclarationNumber;
            entityPM.DeclarationXMLData = customsPM.DeclarationXMLData;
        }
    }
}
