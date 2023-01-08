using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Data.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Collections.Generic;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.SystemLogs;
using System.Net.Http;
using System.Net;
using Logitude.BL.InvoiceModel.EntityLists;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDashboardController : ApiController
    {
        [HttpPost]
        [Route("DigitalDashboard/GetInvoicesGroupedByPaidStatus")]
        public HttpResponseMessage GetInvoicesGroupedByPaidStatus(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var invoices = aRInvoiceQuery.GetByFilters(newFilters);

                var res = invoices.Where(r => !string.IsNullOrEmpty(r.PaidStatus))
                                                      .GroupBy(r => r.PaidStatus)
                                                      .ToDictionary(t => t.Key, t => t.Count());

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalDashboard/GetInvoicesGroupedByDate")]
        public HttpResponseMessage GetInvoicesGroupedByDate(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var invoices = aRInvoiceQuery.GetByFiltersForDashBoard(newFilters);

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                var lastMonth = todayDate.AddMonths(-1);
                var last2Month = todayDate.AddMonths(-2);
                var nextMonth = todayDate.AddMonths(1);

                var firstDayOfMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

                var overDueCounters = invoices.Where(a => a.DueDate != null
                                                          && a.DueDate < firstDayOfMonth
                                                          && a.PaidStatus == "Unpaid" || a.PaidStatus == "Partially Paid")
                                              .GroupBy(a => a.DueDate.Value.Month == todayDate.Month && a.DueDate.Value.Year == todayDate.Year
                                                         ? "Current"
                                                      : a.DueDate.Value.Month == lastMonth.Month && a.DueDate.Value.Year == lastMonth.Year
                                                        ? "Last Month"
                                                        : a.DueDate.Value.Month == last2Month.Month && a.DueDate.Value.Year == last2Month.Year
                                                          ? "Last 2 Month"
                                                          : "Less than 2 Month")
                                              .Select(a => new
                                              {
                                                  Lable = a.Key,
                                                  OverDueCount = a.Count()
                                              })
                                              .ToList();

                var partiallyPaidCounter = invoices.Where(a => a.DueDate != null
                                                                && a.DueDate < firstDayOfMonth
                                                                && a.PaidStatus == "Partially Paid")
                                                   .GroupBy(a => a.DueDate.Value.Month == todayDate.Month && a.DueDate.Value.Year == todayDate.Year
                                                              ? "Current"
                                                           : a.DueDate.Value.Month == lastMonth.Month && a.DueDate.Value.Year == lastMonth.Year
                                                             ? "Last Month"
                                                             : a.DueDate.Value.Month == last2Month.Month && a.DueDate.Value.Year == last2Month.Year
                                                               ? "Last 2 Month"
                                                               : "Less than 2 Month")
                                                   .Select(a => new
                                                   {
                                                       Lable = a.Key,
                                                       PartiallyPaidCount = a.Count()
                                                   })
                                                   .ToList();

                var response = new Dictionary<string, object>();

                foreach (var item in partiallyPaidCounter)
                {
                    var data = overDueCounters.FirstOrDefault(a => a.Lable.Equals(item.Lable));
                    response.Add(item.Lable, new
                    {
                        item.PartiallyPaidCount,
                        data.OverDueCount
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalDashboard/GetShipmentsGroupedByStatus")]
        public HttpResponseMessage GetShipmentsGroupedByStatus(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipments = shipmentQuery.GetByFilters(newFilters)
                                             .Where(r => !string.IsNullOrEmpty(r.StatusCode));

                var res = GetDigitalStatusesWithCount(shipments, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalDashboard/GetShipmentsGroupedByStatusWeight")]
        public HttpResponseMessage GetShipmentsGroupedByStatusweight(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipmentsQuery = shipmentQuery.GetByFilters(newFilters)
                                                  .Where(r => !string.IsNullOrEmpty(r.StatusCode))
                                                  .Select(a => new DashboardModelObject() {
                                                      StatusCode = a.StatusCode, 
                                                      StatusWeight = a.StatusWeight,
                                                      TransportModeId = a.TransportModeId
                                                  });

                var res = GetDigitalStatusesWeightWithCount(shipmentsQuery, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        [Route("DigitalDashboardController/GetDashboardSummary")]
        public HttpResponseMessage GetDashboardSummary(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                var dashboardSummary = new DigitalDashboardSummary();
                var currentDateTime = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant).Date;
                var currentWeek = currentDateTime.AddDays(7).Date;
                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipments = shipmentQuery.GetByFilters(newFilters);

                dashboardSummary.TotalShipmentsByETACount = shipments.Where(d => d.MainCarriageETA >= currentDateTime
                                                                                && d.MainCarriageETA <= currentWeek)
                                                                     .Count();

                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var invoices = aRInvoiceQuery.GetByFilters(newFilters);

                dashboardSummary.TotalInvoicesByDueDateCount = invoices.Where(d => d.DueDate >= currentDateTime
                                                                                    && d.DueDate <= currentWeek)
                                                                       .Count();

                return Request.CreateResponse(HttpStatusCode.OK, dashboardSummary);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        #region Private Methods 

        private Dictionary<string, object> GetDigitalStatusesWithCount(IQueryable<DigitalShipmentList> shipments, int tenant)
        {
            var shipmentsGroupedByStatus = new Dictionary<string, object>();
            var entityStatusQuery = new EntityStatusQuery(tenant);
            var blockedStatus = new List<string> { "PSDL", "PODR" };
            var allStatuses = entityStatusQuery.GetEntityStatusPMsByTenant(tenant)
                                               .ToList();

            var allDigitalEntityStatus = allStatuses.Where(a => a.IsDigitalPortal
                                                                && !blockedStatus.Contains(a.Code))
                                                    .ToList();

            var digitalEntityStatusCodes = allStatuses.Where(a => a.IsDigitalPortal 
                                                                  && !blockedStatus.Contains(a.Code))
                                                      .Select(a => a.Code)
                                                      .ToList();

            var allStatusesCodes = allStatuses.Select(a => a.Code);
            var othersStatuses = allStatusesCodes.Except(digitalEntityStatusCodes).ToList();

            allDigitalEntityStatus.ForEach(item =>
            {
                var count = shipments.Where(r => r.StatusCode.Equals(item.Code, StringComparison.InvariantCultureIgnoreCase)).Count();
                shipmentsGroupedByStatus.Add(GetDigitalStatusName(item.Code, item.DisplayName), new { Count = count, Statuses = item.Id });
            });

            var others = shipments.Where(r => othersStatuses.Contains(r.StatusCode))
                                  .Select(a => a.Id)
                                  .ToList();

            var othersStatusesIds = string.Join(",", others);
            shipmentsGroupedByStatus.Add("Others", new { Count = others.Count(), Statuses = othersStatusesIds });
            return shipmentsGroupedByStatus;
        }

        private Dictionary<string, object> GetDigitalStatusesWeightWithCount(IQueryable<DashboardModelObject> shipments, int tenant)
        {
            var entityStatusQuery = new EntityStatusQuery(tenant);

            var blockedStatus = new List<string> { "PSDL", "PODR" };

            var allStatuses = entityStatusQuery.GetEntityStatusPMsByTenant(tenant)
                                               .Where(a => !blockedStatus.Contains(a.Code) 
                                                           && a.IsDigitalPortal)
                                               .OrderBy(a => a.StatusWeight)
                                               .Select(a => new 
                                               {
                                                    a.Code,
                                                    a.StatusWeight
                                               })
                                               .ToList();

            if (!allStatuses.Any())
            {
                return new Dictionary<string, object>
                {
                    { "Origin", 0 },
                    { "InTransit", 0 },
                    { "dataAtDestination", 0 }
                };
            }

            var allowedStatusCode = allStatuses.Select(a => a.Code).ToList();

            var departedCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SDEP").StatusWeight;

            var arrivedAtDestinationCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SARR").StatusWeight;

            var result = shipments.Where(r => allowedStatusCode.Contains(r.StatusCode)
                                              && (r.StatusWeight < departedCodeWeight
                                                  || (r.StatusWeight >= departedCodeWeight
                                                      && r.StatusWeight < arrivedAtDestinationCodeWeight)
                                                  || (r.StatusWeight >= arrivedAtDestinationCodeWeight)))
                                   .Select(a => new
                                   {
                                       a.TransportModeId,
                                       Code = a.StatusWeight < departedCodeWeight
                                               ? "Origin"
                                               : (a.StatusWeight >= departedCodeWeight
                                                  && a.StatusWeight < arrivedAtDestinationCodeWeight)
                                                  ? "InTransit"
                                                  : "dataAtDestination"
                                   })
                                   .GroupBy(a => a.Code)
                                   .ToDictionary(a => a.Key, y => (object)y.GroupBy(x => x.TransportModeId)
                                                                           .ToDictionary(b => b.Key, xx => xx.Count())); ;
            return result;
        }

        private string GetDigitalStatusName(string code, string exactStatusName)
        {
            if (code.Equals("SDLY", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Delivered";
            }

            if (code.Equals("SDL2", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Out for Delivery";
            }

            if (code.Equals("SHOR", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Created";
            }

            return exactStatusName;
        }

        #endregion Private Methods 
    }
}