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
                var invoices = aRInvoiceQuery.GetByFilters(newFilters);

                var res = invoices.Where(r => r.DueDate != null
                                              && r.PaidStatus != "Paid")
                                  .ToList();

                var response = GetInvoicesSummaries(tenant, res);
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
                var shipments = shipmentQuery.GetByFilters(newFilters).Where(r => !string.IsNullOrEmpty(r.StatusCode)).ToList();
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
                var shipments = shipmentQuery.GetByFilters(newFilters).Where(r => !string.IsNullOrEmpty(r.StatusCode)).ToList();
                var res = GetDigitalStatusesWeightWithCount(shipments, authToken.Tenant);

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

        private Dictionary<string, object> GetInvoicesSummaries(int tenant, List<ARInvoiceList> res)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            var currentMonthInvocies = res.Where(a => a.DueDate.Value.Month == todayDate.Month)
                              .GroupBy(a => a.DueDate.Value.Month)
                              .ToDictionary(t => t.Key,
                                                 t => new
                                                 {
                                                     PartiallyPaidCount = t.Where(a => a.PaidStatus == "Partially Paid").Count(),
                                                     OverDueCount = t.Where(a => a.DueDate < todayDate).Count()
                                                 });

            var lastMonthInvocies = res.Where(a => a.DueDate.Value.Month == todayDate.AddMonths(-1).Month)
                                       .GroupBy(a => a.DueDate.Value.Month)
                                       .ToDictionary(t => t.Key,
                                                          t => new
                                                          {
                                                              PartiallyPaidCount = t.Where(a => a.PaidStatus == "Partially Paid").Count(),
                                                              OverDueCount = t.Where(a => a.DueDate < todayDate).Count()
                                                          });

            var last2MonthInvocies = res.Where(a => a.DueDate.Value.Month == todayDate.AddMonths(-2).Month)
                                        .GroupBy(a => a.DueDate.Value.Month)
                                        .ToDictionary(t => t.Key,
                                                           t => new
                                                           {
                                                               PartiallyPaidCount = t.Where(a => a.PaidStatus == "Partially Paid").Count(),
                                                               OverDueCount = t.Where(a => a.DueDate < todayDate).Count()
                                                           });

            var lessThan2MonthInvocies = res.Where(a => a.DueDate.Value.Month < todayDate.AddMonths(-2).Month)
                                            .GroupBy(a => a.DueDate.Value.Month)
                                            .ToDictionary(t => t.Key,
                                                               t => new
                                                               {
                                                                   PartiallyPaidCount = t.Where(a => a.PaidStatus == "Partially Paid").Count(),
                                                                   OverDueCount = t.Where(a => a.DueDate < todayDate).Count()
                                                               });

            var tempOverDueCount = lessThan2MonthInvocies.Values.Sum(a => a.OverDueCount);
            var tempPartiallyPaidCount = lessThan2MonthInvocies.Values.Sum(a => a.PartiallyPaidCount);

            var response = new Dictionary<string, object>
            {
                {"Current",  currentMonthInvocies.Values},
                {"Last Month",  lastMonthInvocies.Values},
                {"Last 2 Month",  last2MonthInvocies.Values},
                {"Less than 2 Month", 
                    new
                    {
                        PartiallyPaidCount = tempOverDueCount,
                        OverDueCount = tempPartiallyPaidCount
                    }
                }
            };

            return response;
        }

        private Dictionary<string, object> GetDigitalStatusesWithCount(List<DigitalShipmentList> shipments, int tenant)
        {
            var shipmentsGroupedByStatus = new Dictionary<string, object>();
            var entityStatusQuery = new EntityStatusQuery(tenant);
            var blockedStatus = new List<string> { "PSDL", "PODR" };
            var allStatuses = entityStatusQuery.GetEntityStatusPMsByTenant(tenant).ToList();
            var allDigitalEntityStatus = allStatuses.Where(a => a.IsDigitalPortal && !blockedStatus.Contains(a.Code)).ToList();
            var digitalEntityStatusCodes = allStatuses.Where(a => a.IsDigitalPortal && !blockedStatus.Contains(a.Code)).Select(a => a.Code).ToList();
            var allStatusesCodes = allStatuses.Select(a => a.Code);
            var othersStatuses = allStatusesCodes.Except(digitalEntityStatusCodes).ToList();

            allDigitalEntityStatus.ForEach(item =>
            {
                var count = shipments.Where(r => r.StatusCode.Equals(item.Code, StringComparison.InvariantCultureIgnoreCase)).Count();
                shipmentsGroupedByStatus.Add(GetDigitalStatusName(item.Code, item.DisplayName), new { Count = count, Statuses = item.Id });
            });

            var others = shipments.Where(r => othersStatuses.Contains(r.StatusCode));
            var othersStatusesIds = string.Join(",", others.Select(x => x.Id));
            shipmentsGroupedByStatus.Add("Others", new { Count = others.Count(), Statuses = othersStatusesIds });

            return shipmentsGroupedByStatus;
        }

        private Dictionary<string, object> GetDigitalStatusesWeightWithCount(List<DigitalShipmentList> shipments, int tenant)
        {
            var shipmentsGroupedByStatus = new Dictionary<string, object>();
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

            var departedCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SDEP").StatusWeight;

            var digitalStatusesOrigin = allStatuses.Where(a => a.StatusWeight < departedCodeWeight)
                                                   .Select(a => a.Code)
                                                   .ToList();  //new List<string> { "SHOR", "SHP2" };

            var dataOrigin = shipments.Where(r => digitalStatusesOrigin.Contains(r.StatusCode))
                                .GroupBy(a => a.TransportModeId)
                                .ToDictionary(x => x.Key, y => y.Count());

            var arrivedAtDestinationCodeWeight = allStatuses.FirstOrDefault(a => a.Code == "SARR").StatusWeight;

            var digitalStatusesInTransit = allStatuses.Where(a => a.StatusWeight >= departedCodeWeight && a.StatusWeight < arrivedAtDestinationCodeWeight)
                                                      .Select(a => a.Code)
                                                      .ToList(); //new List<string> { "SDEP" };
            var dataInTransit = allStatuses.Where(a => a.StatusWeight >= departedCodeWeight 
                                                       && a.StatusWeight < arrivedAtDestinationCodeWeight)
                                           .Select(a => a.Code)
                                           .ToList();

            var digitalStatusesAtDestination = allStatuses.Where(a => a.StatusWeight >= arrivedAtDestinationCodeWeight)
                                                          .Select(a => a.Code)
                                                          .ToList(); //new List<string> { "SARR", "SDL2", "SDLY"};

            var dataAtDestination = shipments.Where(r => digitalStatusesInTransit.Contains(r.StatusCode))
                                             .GroupBy(a => a.TransportModeId)
                                             .ToDictionary(x => x.Key, y => y.Count());

            var result = new Dictionary<string, object>
            {
                { "Origin", dataOrigin },
                { "InTransit", dataInTransit },
                { "dataAtDestination", dataAtDestination }
            };

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