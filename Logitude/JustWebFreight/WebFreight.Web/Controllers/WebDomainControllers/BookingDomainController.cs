using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.BL.EntityUpdateServices.Behaviours.BookingBehaviours.Validators;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityKeys;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.BookingModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class BookingDomainController : ApiController
    {
        public HttpResponseMessage GetBookingsCounts()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

                BookingsDomainService bookingService = new BookingsDomainService();
                BookingsDataCounts myResult = bookingService.GetBookingsCounts(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRecentBookings()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);

                ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objecttableRepository.GetObjectTableByName("Booking", 0, true);

                IBookingContext objectContext = BookingContext.GetContext(tenant);
                BookingListQueryService queryService = new BookingListQueryService(objectContext);
                IQueryable<BookingList> myResult = queryService.GetRecentEntityLists(tenant, contact.Id, objectTable.Id).AsQueryable();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetBookingAnswerPMs(string bookingId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

                List<BookingAnswerPM> myResult = new List<BookingAnswerPM>();
                IBookingContext objectContext = BookingContext.GetContext(tenant);               

                BookingKeys bookingKeys = new BookingKeys()
                {
                    Id = bookingId
                };

                BookingAnswerQueryService mBookingAnswerQueryService = new BookingAnswerQueryService(objectContext);
                myResult = mBookingAnswerQueryService.GetMulti(bookingKeys, true);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostValidateShipmentMasterArgs(ValidateShipmentMasterArgs args)
        {
            try
            {
                string myResult = null;

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            
                int myTenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(myTenant);

                try
                {
                    BookingMasterIsUsedValidator validator = new BookingMasterIsUsedValidator();

                    validator.Validate(new BookingMasterIsUsedValidatorArgs()
                    {
                        Tenant = myTenant,
                        BookingId = args.BookingId,
                        DirectionCode = args.DirectionId,
                        TransportModeCode = args.TransportModeId,
                        Master = args.Master,
                        AirlinePrefix = args.AirlinePrefix,
                        IsCancelled = args.IsCancelled,
                    });
                }

                catch (Exception ex)
                {
                    myResult = ex.Message;
                }

                //if (myTenant != 343 && myTenant != 528)
                //{
                //    if (!string.IsNullOrEmpty(args.Master) && !string.IsNullOrEmpty(args.AirlinePrefix) && !args.IsCancelled)
                //    {
                //        BookingRepository myBookingRepository = new BookingRepository(myTenant);
                //        bool isFieldExists = myBookingRepository.IsMasterFieldUsed(args.Master, args.AirlinePrefix, args.BookingId, myTenant, args.DirectionId, args.TransportModeId);
                //        if (isFieldExists)
                //        {
                //            myResult = "Master field already used in another Booking";
                //        }
                //    }
                //}

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetValidateBookingForSending(string bookingId, bool isCancellationSent)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(myTenant);
                BookingsDomainService bookingService = new BookingsDomainService();
                BookingValidatorResultClass myResult = bookingService.ValidateBookingForSending(bookingId, myTenant, isCancellationSent);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetBookingsDashBoard(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int myTenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(myTenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                BookingsDomainService bookingService = new BookingsDomainService();
                List<ChartingDataClass> myResult = bookingService.GetBookingsDashBoard(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



    }
}