using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class ChargesTypeByProductsController : ApiController
    {
        public HttpResponseMessage GetChargesTypeExternalAccountsByProducts(string myChargesTypeId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                ChargesExternalAccountsByProductQuery entityQuery = new ChargesExternalAccountsByProductQuery(tenant);
                IQueryable<ChargesExternalAccountsByProductPM> myResult = entityQuery.GetChargesExternalAccountsByProductsByChargesTypeId(myChargesTypeId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(ChargesTypeByProductsControllerHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (args != null)
                    {
                        ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                        string loggedUserId = null;
                        ContactRepository myContactRepository = new ContactRepository(objectContext);
                        ContactQuery myContactQuery = new ContactQuery(myContactRepository);
                        ContactPM myContactPM = myContactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                        if (myContactPM != null)
                        {
                            loggedUserId = myContactPM.Id;
                        }

                        ChargesExternalAccountsByProductRepository entityRepository = new ChargesExternalAccountsByProductRepository(objectContext);
                        List<ChargesExternalAccountsByProduct> allEntities = entityRepository.GetChargesExternalAccountsByProductsByChargesTypeId(args.ChargesTypeId, tenant).ToList();

                        ChargesExternalAccountsByProductService myService = new ChargesExternalAccountsByProductService(objectContext, tenant);
                        foreach (ChargesExternalAccountsByProductPM itemPM in args.Items)
                        {
                            if (string.IsNullOrEmpty(itemPM.Id))
                            {
                                myService.Create(itemPM);
                            }

                            else
                            {
                                ChargesExternalAccountsByProduct myEntity = allEntities.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                                if (myEntity != null)
                                {
                                    bool isUpdatingEntity = false;

                                    if (myEntity.PayablesGLAccount != itemPM.PayablesGLAccount)
                                    {
                                        isUpdatingEntity = true;
                                    }

                                    else if (myEntity.PayablesCostCenter != itemPM.PayablesCostCenter)
                                    {
                                        isUpdatingEntity = true;
                                    }

                                    else if (myEntity.ReceivablesGLAccount != itemPM.ReceivablesGLAccount)
                                    {
                                        isUpdatingEntity = true;
                                    }

                                    else if (myEntity.ReceivablesCostCenter != itemPM.ReceivablesCostCenter)
                                    {
                                        isUpdatingEntity = true;
                                    }

                                    if (isUpdatingEntity)
                                    {
                                        myService.Update(itemPM);
                                    }
                                }
                            }
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class ChargesTypeByProductsControllerHelper
    {
        [Key]
        public int Tenant { get; set; }
        public string ChargesTypeId { get; set; }
        public List<ChargesExternalAccountsByProductPM> Items { get; set; }
    }
}