using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
    public class DirectController : ApiController
    {
        public HttpResponseMessage GetSingleDirect(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                DirectQueryService Service = new DirectQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetDirectById(id, tenant);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        
        public HttpResponseMessage Post(Direct entity)
        {
      
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                        string computingPartnerCode = "";
                        if (loggedContactInfo != null)
                        {
                            computingPartnerCode = loggedContactInfo.ComputingPartnerCode;
                        }

                        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                        ShipmentPM entityPM = mappingService.DirectCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

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
                            if (entityPM.CustomerId == entityPM.ShipperId || entityPM.CustomerId == entityPM.ConsigneeId || entityPM.CustomerId == entityPM.AgentId || entityPM.CustomerId == entityPM.IssuingCarrierAgentId || entityPM.CustomerId == entityPM.CustomAgentExportId || entityPM.CustomerId == entityPM.CustomAgentImportId || entityPM.CustomerId == entityPM.Notify1Id || entityPM.CustomerId == entityPM.Notify2Id || entityPM.CustomerId == entityPM.ShipperNotExporterId || entityPM.CustomerId == entityPM.ConsigneeNotImporterId || entityPM.CustomerId == entityPM.FreightForwarderId || entityPM.CustomerId == entityPM.ColoaderId || entityPM.CustomerId == entityPM.CustomClearancePointId || entityPM.CustomerId == entityPM.ConsolidatorId || entityPM.CustomerId == entityPM.ReleasingAgentId)
                            {
                                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                                if (customer != null)
                                {
                                    entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
                                    if (customer.Customer != null)
                                    {
                                        entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
                                    }
                                    else
                                    {
                                        throw new ApplicationException("The Customer Doesn't Exist on the shipment");

                                    }
                                }
                            }
                            else
                            {
                                throw new ApplicationException("The Customer Doesn't Exist on the shipment");
                            }

                        }

                        if (entityPM.ShipmentPackages.Count > 0)
                        {
                            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                            {
                                if (item.Width != null || item.Height != null || item.Length != null)
                                {
                                    item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                                }
                                
                                item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                            }
                        }

                        ComputeHelper.ComputeTotals(entityPM);                        

                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        var result = mappingService.GetDirectById(entityPM.Id, authToken.Tenant);
                        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Direct API", authToken.Tenant);
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Direct entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        string token = HttpContext.Current.Request.Headers["Token"];
            //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            //        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
            //        DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
            //        ShipmentQuery query = new ShipmentQuery(authToken.Tenant);
            //        ShipmentPM entityPM = query.GetSinglePMByShipmentNumber(entity.ShipmentNumber, authToken.Tenant);


            //        using (TransactionScope scope = TransactionFactory.GetTransaction())
            //        {
            //            ShipmentRepository entityRepository = new ShipmentRepository(MyContext);
            //            if (string.IsNullOrEmpty(entity.ShipmentNumber))
            //            {
            //                throw new ApplicationException("No shipment number found");
            //            }
            //            else
            //            {
            //                Shipment entityPoco = entityRepository.GetSingleShipmentByShipmentNumber(entity.ShipmentNumber, authToken.Tenant);
            //                if (entityPoco == null)
            //                {
            //                    throw new ApplicationException("No shipment with such shipment number");
            //                }
            //                else
            //                {
            //                    ShipmentPM directPM = mappingService.DirectCustomDataMappingAndValidatin(entity, authToken.Tenant);
            //                    if (entityPM.DirectionId != directPM.DirectionId)
            //                    {
            //                        throw new ApplicationException("You can't change the shipment direction");
            //                    }
            //                    else if (entityPM.TransportModeId != directPM.TransportModeId)
            //                    {
            //                        throw new ApplicationException("You can't change the shipment Transport Mode");
            //                    }
            //                    directPM.VolumeUnitCode = entity.VolumeUnit.Code;
            //                    directPM.GrossWeightUnitCode = entity.GrossWeightUnit.Code;
            //                    directPM.ChargeableWeightUnitCode = entity.ChargeableWeightUnit.Code;

            //                    this.MapDirectToEntityPM(directPM, entityPM, MyContext);


            //                    if (entityPM.IsOperationalClosed || entityPM.IsAccountingClosed || entityPM.IsCancelled)
            //                    {
            //                        this.CheckEntityChanges(entityPM, entityPoco);
            //                        this.RemovePackages(directPM, entityPM, MyContext);
            //                    }
            //                    else
            //                    {
            //                        this.RemovePackages(directPM, entityPM, MyContext);

            //                        if (!string.IsNullOrEmpty(entityPM.IncotermId))
            //                        {
            //                            IncotermRepository myIncotermRepository = new IncotermRepository(entityPM.Tenant);
            //                            Incoterm myIncoterm = myIncotermRepository.GetSingleIncoterm(entityPM.IncotermId, entityPM.Tenant);
            //                            if (myIncoterm != null)
            //                            {
            //                                entityPM.FreightPrepaidCollectId = myIncoterm.Freight;
            //                                entityPM.OtherPrepaidCollectId = myIncoterm.OtherCharges;
            //                            }
            //                        }
            //                        if (!string.IsNullOrEmpty(entityPM.CustomerId))
            //                        {
            //                            if (entityPM.CustomerId == entityPM.ShipperId || entityPM.CustomerId == entityPM.ConsigneeId || entityPM.CustomerId == entityPM.AgentId || entityPM.CustomerId == entityPM.IssuingCarrierAgentId || entityPM.CustomerId == entityPM.CustomAgentExportId || entityPM.CustomerId == entityPM.CustomAgentImportId || entityPM.CustomerId == entityPM.Notify1Id || entityPM.CustomerId == entityPM.Notify2Id || entityPM.CustomerId == entityPM.ShipperNotExporterId || entityPM.CustomerId == entityPM.ConsigneeNotImporterId || entityPM.CustomerId == entityPM.FreightForwarderId || entityPM.CustomerId == entityPM.ColoaderId || entityPM.CustomerId == entityPM.CustomClearancePointId || entityPM.CustomerId == entityPM.ConsolidatorId || entityPM.CustomerId == entityPM.ReleasingAgentId)
            //                            {
            //                                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            //                                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
            //                                if (customer != null)
            //                                {
            //                                    entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
            //                                    if (customer.Customer != null)
            //                                    {
            //                                        entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
            //                                    }
            //                                    else
            //                                    {
            //                                        throw new ApplicationException("The Customer Doesn't Exist on the shipment");
            //                                    }
            //                                }
            //                            }
            //                            else
            //                            {
            //                                throw new ApplicationException("The Customer Doesn't Exist on the shipment");
            //                            }
            //                        }
            //                        if (entityPM.ShipmentPackages.Count > 0)
            //                        {
            //                            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
            //                            {
            //                                if (item.Width != null || item.Height != null || item.Length != null)
            //                                {
            //                                    item.Volume = this.ComputeVolume(item, entityPM);
            //                                }
            //                                item.VolumetricWeight = this.ComputeVolumetricWeight(item, entityPM);
            //                            }
            //                        }

            //                        this.ComputeTotals(entityPM);

            //                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());

            //                        service.Update(true);
            //                    }
            //                }
            //            }


            //            scope.Complete();
            //        }

            //        var result = mappingService.GetDirectById(entityPM.Id, authToken.Tenant);

            //        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Direct API", authToken.Tenant);

            //        return Request.CreateResponse(HttpStatusCode.OK, result);
            //    }


            //    catch (Exception ex)
            //    {
            //        var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
            //        APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
            //        return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            //    }
            //}
            //else
            //{
            //    var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
            //    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
            //    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            //}
        }

        private void MapDirectToEntityPM(ShipmentPM directPM,ShipmentPM entityPM, IShipmentsContext MyContext)
        {
            entityPM.ShipmentLevelCode = directPM.ShipmentLevelCode;
            entityPM.DirectionId = directPM.DirectionId;
            entityPM.TransportModeId = directPM.TransportModeId;
            entityPM.ShipperId = directPM.ShipperId;
            entityPM.ShipperReference1 = directPM.ShipperReference1;
            entityPM.ShipperReference2 = directPM.ShipperReference2;
            entityPM.ConsigneeId = directPM.ConsigneeId;
            entityPM.ConsigneeReference1 = directPM.ConsigneeReference1;
            entityPM.ConsigneeReference2 = directPM.ConsigneeReference2;
            entityPM.CustomerId = directPM.CustomerId;
            entityPM.FromPortId = directPM.FromPortId;
            entityPM.ToPortId = directPM.ToPortId;
            entityPM.GrossWeightUnitCode = directPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = directPM.ChargeableWeightUnitCode;
            entityPM.VolumeUnitCode = directPM.VolumeUnitCode;
            entityPM.IncotermCode = directPM.IncotermCode;
            entityPM.IncotermId = directPM.IncotermId;
            entityPM.DescriptionOfGoods = directPM.DescriptionOfGoods;
            entityPM.ShipmentPackages = directPM.ShipmentPackages;
            entityPM.BranchId = directPM.BranchId;
            entityPM.BranchId = directPM.BranchId;
            entityPM.DepartmentId = directPM.DepartmentId;
            entityPM.TEU = directPM.TEU;
            entityPM.NumberOfPackages = directPM.NumberOfPackages;
            entityPM.GrossWeight = directPM.GrossWeight;
            entityPM.Volume = directPM.Volume;
            entityPM.VolumetricWeight = directPM.VolumetricWeight;
            entityPM.ChargeableWeight = directPM.ChargeableWeight;
            entityPM.Master = directPM.Master;
            entityPM.MainCarriageCarrierCode = directPM.MainCarriageCarrierCode;
            entityPM.MainCarriageCarrierId = directPM.MainCarriageCarrierId;
            entityPM.MainCarriageFromPortId = directPM.MainCarriageFromPortId;
            entityPM.MainCarriageToPortId = directPM.MainCarriageToPortId;             
             
        }
        
        private void RemovePackages(ShipmentPM directPM, ShipmentPM entityPM, IShipmentsContext MyContext) {
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

                else
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

                    //else
                    //{
                    //    foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
                    //    {
                    //        ShipmentPackage item = packages.Where(d => d.Id == itemPM.Id).FirstOrDefault();

                    //        if (itemPM.PackageTypeId != item.PackageTypeId)
                    //        {
                    //            throw new ApplicationException("You can't change Package Type, because this direct is closed");
                    //        }

                    //        else if (itemPM.Length != item.Length)
                    //        {
                    //            throw new ApplicationException("You can't change Length, because this direct is closed");
                    //        }

                    //        else if (itemPM.Width != item.Width)
                    //        {
                    //            throw new ApplicationException("You can't change Width, because this direct is closed");
                    //        }

                    //        else if (itemPM.Height != item.Height)
                    //        {
                    //            throw new ApplicationException("You can't change Height, because this direct is closed");
                    //        }

                    //        else if (itemPM.Quantity != item.Quantity)
                    //        {
                    //            throw new ApplicationException("You can't change Quantity, because this direct is closed");
                    //        }

                    //        else if (itemPM.Volume != item.Volume)
                    //        {
                    //            throw new ApplicationException("You can't change Volume, because this direct is closed");
                    //        }

                    //        else if (itemPM.Weight != item.Weight)
                    //        {
                    //            throw new ApplicationException("You can't change Weight, because this direct is closed");
                    //        }

                    //        else if (itemPM.Reference1 != item.Reference1)
                    //        {
                    //            throw new ApplicationException("You can't change Reference 1, because this direct is closed");
                    //        }

                    //        else if (itemPM.Reference2 != item.Reference2)
                    //        {
                    //            throw new ApplicationException("You can't change Reference 2, because this direct is closed");
                    //        }

                    //        else if (itemPM.Reference3 != item.Reference3)
                    //        {
                    //            throw new ApplicationException("You can't change Reference 3, because this direct is closed");
                    //        }

                    //        else if (itemPM.CommodityNumber != item.CommodityNumber)
                    //        {
                    //            throw new ApplicationException("You can't change Commodity Number, because this direct is closed");
                    //        }

                    //        if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode))
                    //        {
                    //            if (itemPM.Seal != item.Seal)
                    //            {
                    //                throw new ApplicationException("You can't change Seal, because this direct is closed");
                    //            }

                    //            else if (itemPM.Seal2 != item.Seal2)
                    //            {
                    //                throw new ApplicationException("You can't change Seal 2, because this direct is closed");
                    //            }
                    //        }

                    //        if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode) && entityPM.ShipmentLevelCode != "FCLD")
                    //        {
                    //            if (itemPM.Harmonize != item.Harmonize)
                    //            {
                    //                throw new ApplicationException("You can't change Length, because this direct is closed");
                    //            }

                    //            else if (itemPM.Temperature != item.Temperature)
                    //            {
                    //                throw new ApplicationException("You can't change Temperature, because this direct is closed");
                    //            }

                    //            else if (itemPM.Ventilation != item.Ventilation)
                    //            {
                    //                throw new ApplicationException("You can't change Ventilation, because this direct is closed");
                    //            }

                    //            else if (itemPM.IsDangerous != item.IsDangerous)
                    //            {
                    //                throw new ApplicationException("You can't change Is Dangerous, because this direct is closed");
                    //            }

                    //            else if (itemPM.ClassNumber != item.ClassNumber)
                    //            {
                    //                throw new ApplicationException("You can't change Class Number, because this direct is closed");
                    //            }

                    //            else if (itemPM.UnNumber != item.UnNumber)
                    //            {
                    //                throw new ApplicationException("You can't change Un Number, because this direct is closed");
                    //            }

                    //            else if (itemPM.PackagingGroup != item.PackagingGroup)
                    //            {
                    //                throw new ApplicationException("You can't change Packaging Group, because this direct is closed");
                    //            }

                    //            else if (itemPM.IMDGCode != item.IMDGCode)
                    //            {
                    //                throw new ApplicationException("You can't change IMDG Code, because this direct is closed");
                    //            }

                    //            else if (itemPM.FlashPoint != item.FlashPoint)
                    //            {
                    //                throw new ApplicationException("You can't change Flash Point, because this direct is closed");
                    //            }

                    //            else if (itemPM.MaterialDescription != item.MaterialDescription)
                    //            {
                    //                throw new ApplicationException("You can't change Material Description, because this direct is closed");
                    //            }
                    //        }

                    //        if (entityPM.ShipmentLevelCode == "FCLD")
                    //        {
                    //            if (itemPM.Tare != item.Tare)
                    //            {
                    //                throw new ApplicationException("You can't change Tare, because this direct is closed");
                    //            }

                    //            else if (itemPM.MarksAndNumbers != item.MarksAndNumbers)
                    //            {
                    //                throw new ApplicationException("You can't change Marks And Numbers, because this direct is closed");
                    //            }
                    //        }
                    //    }
                    //}
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
    }
}
