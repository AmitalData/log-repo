using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebFreight.Web.DataContracts;
using System.Net;
using System.Web.Http;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using WebFreight.Web.Helpers.APIHelpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Logitude.Server.Tools.Helpers;
using System.Reflection;
using SilverlightExpressions;
using WebFreight.Web.Validators;
using Simplog.Data.Helpers;
using WebFreight.Web.ShipmentsModel.DomainServices;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class MasterController : ApiController
    {
        public HttpResponseMessage GetSingleMaster(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                MasterQueryService Service = new MasterQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetMasterById(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        //public HttpResponseMessage GetSingleMasterByNumber(string number)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        MasterQueryService Service = new MasterQueryService(tenant);
        //        ServiceResponse response = new ServiceResponse();
        //        var Result = Service.GetMasterByMaster(number, tenant);
        //        //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
        //        return Request.CreateResponse(HttpStatusCode.OK, Result);
        //    }
        //    catch (Exception ex)
        //    {
        //        var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
        //        return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        //    }
        //}
        public HttpResponseMessage Post(Master entity)
        {
            if (ModelState.IsValid)
            {
                try
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
                    MasterQueryService mappingService = new MasterQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.MasterCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        if (entity.IsOperationalClosed)
                        {
                            string errorMessage = "";

                            RulesValidator validator = new RulesValidator();
                            validator.Initialize(authToken.Tenant);
                            List<ObjectTableRuleField> requiredFields = validator.ValidateAllRequiredFieldRules(entityPM, "Shipment", authToken.Tenant);

                            IWebFreightContext webFreightContext = WebFreightContext.GetContext(authToken.Tenant);
                            ObjectFieldRepository ObjectFieldRepository = new ObjectFieldRepository(webFreightContext);
                            if (requiredFields.Count > 0)
                            {
                                foreach (ObjectTableRuleField field in requiredFields)
                                {
                                    ObjectField f = ObjectFieldRepository.GetSingleObjectFieldById(field.ObjectFieldId, authToken.Tenant);
                                    errorMessage = errorMessage + ", " + TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", f.FullNameTextCode.Code, null, null, field.Tenant);
                                }
                            }

                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = errorMessage.TrimStart(',');
                                throw new ApplicationException("Due to operational closed: " + errorMessage);
                            }

                            entityPM.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                        }

                        if (entity.IsAccountingClosed)
                        {
                            if (!entity.IsOperationalClosed)
                            {
                                throw new ApplicationException("Shipment shoud be closed operationally");
                            }

                            else
                            {
                                entityPM.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                            }
                        }

                        if (entity.Houses.Count > 0)
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(authToken.Tenant);
                            foreach (House item in entity.Houses)
                            {
                                bool isValid = true;
                                Shipment myDataBaseShipment = null;

                                if (!string.IsNullOrEmpty(item.Id))
                                {
                                    myDataBaseShipment = shipmentRepository.GetSingleShipment(item.Id, authToken.Tenant);
                                }

                                if (myDataBaseShipment == null)
                                {
                                    if (!string.IsNullOrEmpty(item.ShipmentNumber))
                                    {
                                        myDataBaseShipment = shipmentRepository.GetSingleShipmentByShipmentNumber(item.ShipmentNumber, authToken.Tenant);
                                    }
                                }

                                if (myDataBaseShipment == null)
                                {
                                    isValid = false;
                                    throw new ApplicationException("Shipment with ShipmentNumber " + item.ShipmentNumber + " doesn't exist");
                                }

                                if (!string.IsNullOrEmpty(item.Id) && !string.IsNullOrEmpty(item.ShipmentNumber))
                                {
                                    if (myDataBaseShipment != null)
                                    {
                                        if (myDataBaseShipment.ShipmentNumber != item.ShipmentNumber)
                                        {
                                            isValid = false;
                                            throw new ApplicationException("The sent Id and Shipment Number are not matching");
                                        }
                                    }
                                }

                                else if (myDataBaseShipment.ShipmentLevelCode != "H")
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment is not house");
                                }

                                else if (!string.IsNullOrEmpty(myDataBaseShipment.MasterShipmentDataId))
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment connected to another master");
                                }

                                else if (myDataBaseShipment.DirectionId != entityPM.DirectionId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment direction not matches the master direction");
                                }

                                else if (myDataBaseShipment.TransportModeId != entityPM.TransportModeId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment transport mode not matches the master transport mode");
                                }

                                else if (myDataBaseShipment.FromPortId != entityPM.FromPortId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment from port not matches the master from port");
                                }

                                else if (myDataBaseShipment.ToPortId != entityPM.ToPortId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment to port not matches the master to port");
                                }

                                else if (myDataBaseShipment.BranchId != entityPM.BranchId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment branch not matches the master branch");
                                }

                                else if (myDataBaseShipment.IsCancelled)
                                {
                                    isValid = false;
                                    throw new ApplicationException("You can't connect cancelled house");
                                }

                                else if (myDataBaseShipment.IsOperationalClosed)
                                {
                                    isValid = false;
                                    throw new ApplicationException("You can't connect operational closed house");
                                }

                                else if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
                                {
                                    switch (entityPM.ShipmentTypeId.ToUpper())
                                    {
                                        case "MYGO":
                                            {
                                                if (myDataBaseShipment.ShipmentTypeId != "LCLD")
                                                {
                                                    isValid = false;
                                                    throw new ApplicationException("The sent shipment type should be LCL");
                                                }
                                                break;
                                            }

                                        case "MYGI":
                                            {
                                                if (myDataBaseShipment.ShipmentTypeId != "LTL")
                                                {
                                                    isValid = false;
                                                    throw new ApplicationException("The sent shipment type should be LTL");
                                                }
                                                break;
                                            }
                                    }
                                }

                                bool hasOpenPayables = false;
                                bool hasOpenReceivables = false;
                                if (entity.IsAccountingClosed && entity.IsOperationalClosed)
                                {
                                    List<ShipmentReceivable> houseReceivables = MyContext.ShipmentReceivables.Where(d => d.ShipmentId == myDataBaseShipment.Id).ToList();
                                    List<ShipmentPayable> housePayables = MyContext.ShipmentPayables.Where(d => d.ShipmentId == myDataBaseShipment.Id).ToList();

                                    if (!hasOpenReceivables)
                                    {
                                        #region
                                        if (houseReceivables.Count > 0)
                                        {
                                            foreach (ShipmentReceivable recitem in houseReceivables)
                                            {
                                                if (recitem.ShipmentReceivableLineStatusCode != "ACCT" && recitem.ShipmentReceivableLineStatusCode != "EMPT")
                                                {
                                                    if (recitem.TotalAmount != null && recitem.TotalAmount != 0)
                                                    {
                                                        hasOpenReceivables = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }

                                    if (!hasOpenPayables)
                                    {
                                        #region
                                        if (housePayables.Count > 0)
                                        {
                                            foreach (ShipmentPayable payaitem in housePayables)
                                            {
                                                if (payaitem.ShipmentPayableLineStatusCode != "ACCT" && payaitem.ShipmentPayableLineStatusCode != "EMPT" && payaitem.ShipmentPayableParentId == null)
                                                {
                                                    if (payaitem.ShipmentPayableAmountTypeCode == "NEXP")
                                                    {
                                                        if (payaitem.AccountedAmount != null && payaitem.AccountedAmount != 0)
                                                        {
                                                            hasOpenPayables = true;
                                                            break;
                                                        }
                                                    }

                                                    else
                                                    {
                                                        if (payaitem.ExpectedAmount != null && payaitem.ExpectedAmount != 0)
                                                        {
                                                            hasOpenPayables = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }

                                if (hasOpenPayables || hasOpenReceivables)
                                {
                                    isValid = false;
                                    throw new ApplicationException("Can’t close for accounting: House #" + myDataBaseShipment.ShipmentNumber + " has open receivables/ payables");
                                }

                                if (isValid)
                                {
                                    // connect to master
                                    ConsoleShipmentPM myConsole = new ConsoleShipmentPM()
                                    {
                                        Id = myDataBaseShipment.Id,
                                    };

                                    entityPM.ShipmentConsoleShipments.Add(myConsole);
                                }
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
                        service.Create();

                        ShipmentRepository entityRepository = new ShipmentRepository(MyContext);
                        List<Shipment> allHouses = entityRepository.GetHouseShipmentsForMaster(entityPM.Id, authToken.Tenant);

                        if (allHouses.Count > 0)
                        {
                            foreach (Shipment item in allHouses)
                            {
                                if (entityPM.IsOperationalClosed)
                                {
                                    item.IsOperationalClosed = true;
                                    item.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);

                                    if (item.FirstOperationalCloseDate == null)
                                    {
                                        item.FirstOperationalCloseDate = item.OperationalCloseDate;
                                    }
                                }

                                if (entityPM.IsAccountingClosed)
                                {
                                    item.IsAccountingClosed = true;
                                    item.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                                }

                                entityRepository.Update(item);
                            }

                            entityRepository.SubmitChanges();
                        }
                        scope.Complete();
                    }

                    var result = mappingService.GetMasterById(entityPM.Id, authToken.Tenant);
                    APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Master API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Master entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        }
    }
}