using Intuit.Ipp.Core.Configuration;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class SupplierInvoiceController : ApiController
    {

        public HttpResponseMessage GetSupplierInvoiceItemsClasifiedRemarks(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceItemListQueryService queryService = new SupplierInvoiceItemListQueryService(customContext);
                List<SupplierInvoiceItemList> items = queryService.GetSupplierInvoiceItemsClasifiedRemarks(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, items);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSupplierInvoiceItemsForInvoice(string declarationId, int counterkey)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceItemListQueryService queryService = new SupplierInvoiceItemListQueryService(customContext);
                List<SupplierInvoiceItemList> items=  queryService.GetSupplierInvoiceItemsForInvoice(declarationId, counterkey, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, items);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSupplierInvoiceItemsForInvoices(string declarationId, string supplierInvoiceCounterKeys,int skip,int take,bool getCount)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceItemListQueryService queryService = new SupplierInvoiceItemListQueryService(customContext);
                SupplierInvoiceRepository supplierInvoiceRepository = new SupplierInvoiceRepository(customContext);
                IQueryable<SupplierInvoiceItemList> items = queryService.GetSupplierInvoiceItemsForInvoices(declarationId, supplierInvoiceCounterKeys, tenant,skip,take);

                //List<SupplierInvoice> supplierInvoices = supplierInvoiceRepository.GetSupplierInvoicesByCounterKeys(declarationId, supplierInvoiceCounterKeys, tenant);
                //List<SupplierInvoiceList> invoices = (from a in supplierInvoices
                //        select new SupplierInvoiceList()
                //        {
                //            DeclarationId = a.DeclarationId,
                //            InvoiceCounterKey = a.InvoiceCounterKey,
                //            SequenceNumeric = a.SequenceNumeric,
                //        }).OrderBy(d => d.SequenceNumeric).ToList();
                //invoices = invoices.Where(d => d.IsAccumalated).ToList();
                //foreach (SupplierInvoiceList item in invoices.Where(d=> d.IsAccumalated))
                //{
                //    List<SupplierInvoiceItemList> parents = items.Where(d => d.DeclarationId == item.DeclarationId && d.CounterKey == item.InvoiceCounterKey && d.IsParent).ToList();
                //}
            //    items= from a in items where declarationId
                ServiceResponse response = new ServiceResponse();
                if (getCount)
                {
                    response.Count = items != null ? items.Count() : 0;
                }
                if (items != null)
                {
                   
                    items = items.OrderBy(d => d.SequenceNumeric).Skip(skip).Take(take);
                  
                    
                }
                response.Result = items;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSelectedSupplierInvoiceItems(string declarationId,string supplierInvoiceCounterKeys,string lineNubmers)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                SupplierInvoiceItemListQueryService queryService = new SupplierInvoiceItemListQueryService(customContext);
                SupplierInvoiceRepository supplierInvoiceRepository = new SupplierInvoiceRepository(customContext);
                IQueryable<SupplierInvoiceItemList> items = queryService.GetSelectedSupplierInvoiceItems(declarationId, supplierInvoiceCounterKeys, lineNubmers,tenant);

              
                ServiceResponse response = new ServiceResponse();
                response.Result = items;
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}