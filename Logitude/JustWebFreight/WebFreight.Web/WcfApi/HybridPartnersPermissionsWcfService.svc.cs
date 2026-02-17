using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HybridPartnersPermissionsWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HybridPartnersPermissionsWcfService.svc or HybridPartnersPermissionsWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HybridPartnersPermissionsWcfService : IHybridPartnersPermissionsWcfService
    {
        public Response Upsert(int allowedTenant, int allowedByPartnerTenant, bool inActive)
        {

            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    //SecurityUtility.AuthenticationOnTenant(allowedByPartnerTenant);

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(0);
                    HybridPartnersPermissionRepository hybridPartnersPermissionRepository = new HybridPartnersPermissionRepository(commoncontext);
                    HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);

                    HybridPartner allowedByPartner = hybridPartnerRepository.GetHybridPartnersByPartnerTenant(allowedByPartnerTenant).FirstOrDefault();
                    HybridPartner allowedPartner = hybridPartnerRepository.GetHybridPartnersByPartnerTenant(allowedTenant).FirstOrDefault();
                    if (allowedByPartner != null && allowedPartner != null)
                    {
                        HybridPartnersPermission entity = hybridPartnersPermissionRepository.GetSingleHybridPartnersPermission(allowedPartner.Id, allowedByPartner.Id);
                        if (entity == null)
                        {
                            entity = new HybridPartnersPermission()
                            {
                                AllowedByHybridPartnerId = allowedByPartner.Id,
                                HybridPartnerId = allowedPartner.Id,
                                InActive = inActive,
                            };

                            hybridPartnersPermissionRepository.Add(entity);
                        }
                        else
                        {
                            entity.InActive = inActive;
                            hybridPartnersPermissionRepository.Update(entity);
                        }

                        hybridPartnersPermissionRepository.SubmitChanges();

                        response.Result = entity.HybridPartnerId.ToString();
                    }
                    else
                    {
                        response.HasError = true;
                        if (allowedByPartner == null)
                            response.ErrorMessage = "Couldn't find a hybrid partner for tenant " + allowedByPartnerTenant;

                        if (allowedPartner == null)
                            response.ErrorMessage = (!string.IsNullOrEmpty(response.ErrorMessage) ? Environment.NewLine : "") + "Couldn't find a hybrid partner for tenant " + allowedTenant;
                    }
                    scope.Complete();
                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }
    

        public List<HybridPartnerList> GetAllowedPartners(int hybridPartnerTenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            response = new Response();
            try
            {
                //SecurityUtility.AuthenticationOnTenant(hybridPartnerTenant);
                List<HybridPartnerList> result = new List<HybridPartnerList>();
                ICommonDataContext commoncontext = CommonDataContext.GetContext(0);
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
                HybridPartnersPermissionRepository hybridPartnersPermissionRepository = new HybridPartnersPermissionRepository(commoncontext);

                HybridPartner hybridPartner = hybridPartnerRepository.GetHybridPartnersByPartnerTenant(hybridPartnerTenant).FirstOrDefault();
                if (hybridPartner != null)
                {
                    List<string> allowedPartners = hybridPartnersPermissionRepository.GetHybridPartnersPermissionByPartnerId(hybridPartner.Id).Select(p=>p.AllowedByHybridPartnerId).ToList();

                    result = (from b in hybridPartnerRepository.context.HybridPartners
                              where allowedPartners.Contains(b.Id)
                              select new HybridPartnerList
                              {
                                  Id = b.Id,
                                  LocalName = b.LocalName,
                                  LogoId = b.LogoId,
                                  Name = b.Name,
                                  PartnerTenant = b.PartnerTenant,
                                  SearchFields = b.SearchFields,
                                  SmallLogoId = b.SmallLogoId,
                              }).ToList();

                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessage = "Could not find a hybrid partner for this tenant";
                }

                return result;

            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

      
    }
}
