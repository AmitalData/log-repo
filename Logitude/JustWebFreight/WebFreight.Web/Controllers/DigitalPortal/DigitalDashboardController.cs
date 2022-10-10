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

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDashboardController : ApiController
    {
        [HttpPost]
        [Route("DigitalDashboardController/GetInvoicesGroupedByPaidStatus")]
        public IHttpActionResult GetInvoicesGroupedByPaidStatus(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var invoices = aRInvoiceQuery.GetByFilters(newFilters);

                var invoicesGroupedByStatus = invoices.Where(r => !string.IsNullOrEmpty(r.PaidStatus))
                                                      .GroupBy(r => r.PaidStatus)
                                                      .ToDictionary(t => t.Key, t => t.Count());

                return Ok(invoicesGroupedByStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalDashboardController/GetShipmentsGroupedByStatus")]
        public IHttpActionResult GetShipmentsGroupedByStatus(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipments = shipmentQuery.GetByFilters(newFilters);

                var shipmentsGroupedByStatus = shipments.Where(r => !string.IsNullOrEmpty(r.StatusCode))
                                                        .Select(a => new {
                                                            ExactStatusName = a.StatusCode.Equals("SDLY") 
                                                                              ? "Out for Delivery" 
                                                                              : a.StatusCode.Equals("SHOR") 
                                                                                ? "Created" 
                                                                                : a.ExactStatusName
                                                        })
                                                        .GroupBy(r => r.ExactStatusName)
                                                        .ToDictionary(t => t.Key, t => t.Count());

                return Ok(shipmentsGroupedByStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalDashboardController/GetDashboardSummary")]
        public IHttpActionResult GetDashboardSummary(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                var dashboardSummary = new DigitalDashboardSummary();
                var currentDateTime = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant).Date;
                var currentWeek = currentDateTime.AddDays(7).Date;
                newFilters.Tenant = authToken.Tenant;
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipments = shipmentQuery.GetByFilters(newFilters);

                dashboardSummary.TotalShipmentsByETACount = shipments.Where(d=> d.MainCarriageETA >= currentDateTime
                                                                                && d.MainCarriageETA <= currentWeek)
                                                                     .Count();

                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var invoices = aRInvoiceQuery.GetByFilters(newFilters);

                dashboardSummary.TotalInvoicesByDueDateCount = invoices.Where(d => d.DueDate >= currentDateTime 
                                                                                    && d.DueDate <= currentWeek)
                                                                       .Count();
               
                return Ok(dashboardSummary);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }
    }
}