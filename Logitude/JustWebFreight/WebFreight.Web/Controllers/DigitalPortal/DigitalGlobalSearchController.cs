using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Simplog.Server.Infrastructure.DataContracts.Models.SearchModel;
using Logitude.SystemLogs;
using System.Net;
using System.Net.Http;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalGlobalSearchController : ApiController
    {
        [HttpPost]
        [Route("DigitalGlobalSearch/GetDigitalGlobalSearchResults")]
        public HttpResponseMessage GetDigitalGlobalSearchResults(GeneralFilters newFilters, bool includeQoutes = false)
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

                var dictionary = new Dictionary<string, List<GlobalSearchResult>>();

                if (string.IsNullOrWhiteSpace(newFilters.SearchText))
                {
                    dictionary =  new Dictionary<string, List<GlobalSearchResult>>
                    {
                        { "Menu.G.Shipment", new List<GlobalSearchResult>()},
                        { "Menu.G.Invoice", new List<GlobalSearchResult>()},
                        { "Menu.G.Quotes", new List<GlobalSearchResult>()},
                    };
                    
                    return Request.CreateResponse(HttpStatusCode.OK, dictionary);
                }

                newFilters.Tenant = authToken.Tenant;
                newFilters.SortBy = "StatusDate";
                newFilters.SortDirection = "Descending";
                var shipmentQuery = new ShipmentQuery(authToken.Tenant);
                var shipments = shipmentQuery.GetByFilters(newFilters);
                if (!string.IsNullOrWhiteSpace(searchFields))
                {
                    shipments = shipments.Where(d => d.ShipmentNumber.Contains(searchFields))
                                         .Take(3);
                }

                newFilters.SortBy = "UpdateDate";
                newFilters.SortDirection = "Descending";
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var aRInvoices = aRInvoiceQuery.GetByFilters(newFilters);
                if (!string.IsNullOrWhiteSpace(searchFields))
                {
                    aRInvoices = aRInvoices.Where(d => d.InvoiceNumber.Contains(searchFields))
                                           .Take(3);
                }

                dictionary = new Dictionary<string, List<GlobalSearchResult>> 
                {
                    { "Menu.G.Shipment", GetShipmentsGlobalSearch(shipments.ToList())},
                    { "Menu.G.Invoice", GetInovicesGlobalSearch(aRInvoices.ToList())},
                };

                if (includeQoutes)
                {
                    newFilters.SortBy = "LastUpdate";
                    newFilters.SortDirection = "Descending";

                    var quoteQuery = new QuoteQuery(authToken.Tenant);
                    var quotesQuery = quoteQuery.GetByFilters(newFilters);
                    if (!string.IsNullOrWhiteSpace(searchFields))
                    {
                        quotesQuery = quotesQuery.Where(d => d.QuoteNumber.Contains(searchFields))
                                                 .Take(3);
                    }

                    dictionary.Add("Menu.G.Quotes", GetQoutesGlobalSearch(quotesQuery.ToList()));
                }

                return Request.CreateResponse(HttpStatusCode.OK, dictionary);
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
                    Reference = item.CustomerName
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
        
        private List<GlobalSearchResult> GetQoutesGlobalSearch(List<QuoteList> quoteList)
        {
            var data = new List<GlobalSearchResult>();

            quoteList.ForEach(item => {
                data.Add(new QuoteSearchResult
                {
                    Id = item.Id,
                    QuoteNumber = item.QuoteNumber,
                    Direction = item.DirectionName,
                    TransportMode = item.TransportModeId,
                    Reference = item.DirectionId == "I" ? item.ShipperName : item.ConsigneeName
                });
            });

            return data;
        }

        #endregion Private Methods 
    }
}