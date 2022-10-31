using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Server.Infrastructure.DataContracts.Models.SearchModel;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalGlobalSearchController : ApiController
    {
        [HttpPost]
        [Route("DigitalGlobalSearch/GetDigitalGlobalSearchResults")]
        public IHttpActionResult GetDigitalGlobalSearchResults(GeneralFilters newFilters)
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
                var searchFields = newFilters.SearchText;
                newFilters.Tenant = authToken.Tenant;
                newFilters.SortBy = "StatusDate";
                newFilters.SortDirection = "Descending";
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipments = shipmentQuery.GetByFilters(newFilters);
                if (!string.IsNullOrWhiteSpace(searchFields))
                {
                    shipments = shipments.Where(d => d.ShipmentNumber.Contains(searchFields))
                                         .Take(10);
                }

                newFilters.SortBy = "InvoiceDate";
                newFilters.SortDirection = "Descending";
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var aRInvoices = aRInvoiceQuery.GetByFilters(newFilters);
                if (!string.IsNullOrWhiteSpace(searchFields))
                {
                    aRInvoices = aRInvoices.Where(d => d.InvoiceNumber.Contains(searchFields))
                                           .Take(10);
                }

                var invoicesCount = aRInvoices.Count();
                var shipmentsCount = shipments.Count();

                if(invoicesCount >= 5 && shipmentsCount >= 5)
                {
                    aRInvoices = aRInvoices.Take(5);
                    shipments = shipments.Take(5);
                }

                else if (invoicesCount >= shipmentsCount)
                {
                    shipments = shipments.Take(shipmentsCount);
                    aRInvoices = aRInvoices.Take(10 - shipmentsCount);
                } 

                else if (shipmentsCount >= invoicesCount)
                {
                    aRInvoices = aRInvoices.Take(invoicesCount);
                    shipments  = shipments.Take(10 - invoicesCount);
                }

                var dictionary = new Dictionary<string, List<GlobalSearchResult>> {
                    { "Shipments", GetShipmentsGlobalSearch(shipments.ToList())},
                    { "Invoicing", GetInovicesGlobalSearch(aRInvoices.ToList())}
                };

                return Ok(dictionary);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        #region Private Methods 
        private List<GlobalSearchResult> GetShipmentsGlobalSearch(List<DigitalShipmentList> shipments)
        {
            var data = new List<GlobalSearchResult>();

            shipments.ForEach (item => {
                data.Add(new ShipmentSearchResult
                {
                    Id = item.Id,
                    ShipmentNumber = item.ShipmentNumber,
                    Direction = item.DirectionId,
                    TransportMode = item.TransportModeId,
                    Reference = item.DirectionId == "I" ? item.ShipperName : item.ConsigneeName
                });
            });

            return data;
        }

        private List<GlobalSearchResult> GetInovicesGlobalSearch(List<ARInvoiceList> aRInvoices)
        {
            var data = new List<GlobalSearchResult>();

            aRInvoices.ForEach(item => {
                data.Add(new InvoiceSearchResult
                {
                    Id = item.Id,
                    InvoiceNumber = item.InvoiceNumber,
                    Reference = item.MainEntityReference,
                    Type = item.ARInvoiceTypeName,
                    IsConsolidationInvoice = item.IsConsolidationInvoice
                });
            });

            return data;
        }

        #endregion Private Methods 
    }
}