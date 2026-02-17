using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;

using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class VehicleController : ApiController
    {

        public HttpResponseMessage GetVehicleByVehicleChassisNumberOrRichbitFileNumber(string vehicleChassisNumber, string richbitFileNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                VehicleQueryService vehicleQuery = new VehicleQueryService(customContext);
                VehiclePM Vehicle = vehicleQuery.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(vehicleChassisNumber, richbitFileNumber, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, Vehicle);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckIfVehicleExistByChassisNumber(string vehicleChassisNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                VehicleQueryService vehicleQuery = new VehicleQueryService(customContext);
                string Vehicle = vehicleQuery.GetVehicleIdByChassisNumber(vehicleChassisNumber, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, Vehicle);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetVehiclesForSelection()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                VehicleQueryService vehicleQuery = new VehicleQueryService(customContext);
                List<VehicleList> vehicles = vehicleQuery.GetVehiclesForSelection(tenant);


                return Request.CreateResponse(HttpStatusCode.OK, vehicles);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //public HttpResponseMessage GetDuplicatedVehInSameDeclaration_old(string declarationId, string richbitNumbersString, string chassissNumbersString) //CheckIfVehUsedInAnotherItemsInTheSameDeclaration
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        string loggedUserEmail = authToken.Email;
        //        SecurityUtility.AuthenticationOnTenant(tenant);


        //        if (!string.IsNullOrWhiteSpace(richbitNumbersString))
        //        {
        //           //return;
        //        }

        //        string[] richbitFileNumbers = richbitNumbersString.Split(',');
        //        string[] chassissNumbers = chassissNumbersString.Split(',');

        //        ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
        //        VehicleQueryService vehicleQuery = new VehicleQueryService(customContext);

        //        List<VehiclePM> vehicles = vehicleQuery.GetVehiclesByRichbitFileNumber(declarationId,richbitFileNumbers, chassissNumbers, tenant);


        //        return Request.CreateResponse(HttpStatusCode.OK, vehicles);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

        public HttpResponseMessage GetCheckRichbitNumbersError(string richbitNumbersString) 
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);


                string[] richbitFileNumbers = richbitNumbersString.Split(',');

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                VehicleQueryService vehicleQuery = new VehicleQueryService(customContext);
                VehicleRepository vehicleRepo = new VehicleRepository(customContext);
                SupplierInvoiceItemVehicleRepository itemVehicleRepo = new SupplierInvoiceItemVehicleRepository(customContext);
                DeclarationRepository declarationRepo = new DeclarationRepository(customContext);

                List<VehicleValidationError> errors = new List<VehicleValidationError>();

                // get vehicles by richbit
                List<VehiclePM> vehicles = vehicleQuery.GetVehiclesByRichbitFileNumbers(richbitFileNumbers, tenant);

                //if (vehicles.Count > 0)
                //{
                foreach (VehiclePM veh in vehicles)
                {
                    if (veh.DeclarationId != null)
                    {
                        //get customfilenumber and invoicecounterkey, put it in declaration error
                        SupplierInvoiceItemVehicle existItemVeh = itemVehicleRepo.GetSupplierInvoiceItemVehicleByRichbitNumber(veh.RichbitFileNumber, veh.DeclarationId, tenant);

                        //build error
                        if (existItemVeh != null)
                        {
                            VehicleValidationError error = new VehicleValidationError()
                            {
                                DeclarationId = veh.DeclarationId,
                                CustomFileNumber = veh.CustomFileNumber,
                                InvoiceCounterKey = existItemVeh.InvoiceCounterKey,
                                InvoiceItemLineNumber = existItemVeh.InvoiceItemLineNumber,
                                RichbitFileNumber = veh.RichbitFileNumber,
                                //ValidationText = "vehicle (" + veh.RichbitFileNumber + ") used in file " + veh.CustomFileNumber,
                                ValidationText = "שילדה (" + veh.RichbitFileNumber + ") קיימת בהצהרה מספר " + veh.CustomFileNumber,
                                IsVehicle = true,

                            };

                            //delete invalid richbit number
                            richbitNumbersString = richbitNumbersString.Replace(veh.RichbitFileNumber + ",", "");
                            richbitFileNumbers = richbitNumbersString.Split(',');

                            errors.Add(error);
                        }
                    }
                }
                //}
                //else
                //{   //no vehicle with these richbits
                //get itemsVehs with these richbits
                List<SupplierInvoiceItemVehicle> itemVehsList = itemVehicleRepo.GetSupplierInvoiceItemVehiclesByRichbitNumbers(richbitFileNumbers, tenant);

                if (itemVehsList.Count > 0)
                {
                    foreach (SupplierInvoiceItemVehicle itemVeh in itemVehsList)
                    {
                        //get customfilenumber put it in declaration error
                        string customFileNo = declarationRepo.GetCusomFileNoForDeclaration(itemVeh.DeclarationId, tenant);

                        //build error
                        VehicleValidationError error = new VehicleValidationError()
                        {
                            DeclarationId = itemVeh.DeclarationId,
                            CustomFileNumber = customFileNo,
                            InvoiceCounterKey = itemVeh.InvoiceCounterKey,
                            InvoiceItemLineNumber = itemVeh.InvoiceItemLineNumber,
                            RichbitFileNumber = itemVeh.RichbitFileNumber,
                            ValidationText = "ריכיבית (" + itemVeh.RichbitFileNumber + ") קיימת בהצהרה מספר " + customFileNo,
                        };
                        errors.Add(error);

                    }
                }
                else
                {
                    // valid

                }


                //}

                return Request.CreateResponse(HttpStatusCode.OK, errors);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetVehiclesByRichbitFileNumbers(string richbitNumbersString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);


                string[] richbitFileNumbers = richbitNumbersString.Split(',');

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                VehicleQueryService vehicleQuery = new VehicleQueryService(customContext);
                VehicleRepository vehicleRepo = new VehicleRepository(customContext);
                SupplierInvoiceItemVehicleRepository itemVehicleRepo = new SupplierInvoiceItemVehicleRepository(customContext);
                DeclarationRepository declarationRepo = new DeclarationRepository(customContext);
                
                // get vehicles by richbit
                List<VehiclePM> vehicles = vehicleQuery.GetVehiclesByRichbitFileNumbers(richbitFileNumbers, tenant);

               

                return Request.CreateResponse(HttpStatusCode.OK, vehicles);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }

    public class VehicleValidationError
    {
        public string ValidationText { get; set; }
        public string DeclarationId { get; set; }
        public string CustomFileNumber { get; set; }
        public string RichbitFileNumber { get; set; }
        public int InvoiceCounterKey { get; set; }
        public int InvoiceItemLineNumber { get; set; }
        public bool IsVehicle { get; set; } = false;

    }
}