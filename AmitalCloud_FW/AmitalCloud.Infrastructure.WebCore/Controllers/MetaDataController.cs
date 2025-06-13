using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetaDataController : ControllerBase
    {
        [OperationContract]
        [WebGet(UriTemplate = "getadvancequeryfilterspms/{tenant}")]
        [HttpGet("{tenant?}")]
        public List<AdvancedQueryFilterPM> GetAdvanceQueryFiltersPMs(int tenant)
        => new AdvancedQueryFilterQueryService(tenant).GetMulti(a => (a.Tenant == tenant || a.Tenant == 0) && a.IsPredefined == true
                , "ObjectField,Query,Query.ObjectTable").ToList();

        [OperationContract]
        [WebGet(UriTemplate = "GetTenantLanguageTranslations/{tenant}")]
        [HttpGet("{tenant?}")]
        public List<Translation> GetTenantLanguageTranslations(int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            List<Translation> AllTranslations = new List<Translation>();
            TenantPM tenantPM = new TenantQueryService(tenant).GetSingle(tenant, true, true);
            if (!string.IsNullOrEmpty(tenantPM.Language) && tenantPM.Language.ToLower() != "en" && tenantPM.Language != "english")
            {
                AllTranslations = new TranslationRepository(tenant).GetTranslationsByLanguageCode(tenantPM.Language, tenant);
                //todo: check if this is needed
                //TenantManagmentPrivateLabelsPM privatelabel = null;
                //var url = AmitalCloudSecurityUtility.getLoggedDomain();
                //if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                //{
                //    TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(tenant);
                //    privatelabel = query.GetSingleActivePMByUrl_Cache(url);
                //}

                //    if (privatelabel != null)
                //        {
                //            TextCodePM textCode = service.GetMulti(a => a.Code == "General.MH.Importers" && a.Tenant == tenant, "").FirstOrDefault();
                //            Translation tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                //            if (tra != null)
                //            {
                //                tra.TranslatedText = privatelabel.PrivateLabelName;
                //            }
                //            else
                //            {
                //                tra = new Translation()
                //                {
                //                    Id = Guid.NewGuid().ToString(),
                //                    TextCodeId = textCode.Id,
                //                    TextCode = textCode,
                //                    Tenant = tenant,
                //                    TranslatedText = privatelabel.PrivateLabelName,
                //                    TranslationHeaderCode = tenantPM.Language,
                //                    TextCodeCode = textCode.Code,
                //                };

                //                AllTranslations.Add(tra);
                //            }
                //            textCode = service.GetMulti(a => a.Code == "General.MH.ActivationWizard" && a.Tenant == tenant, "").FirstOrDefault();
                //            tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                //            if (tra != null)
                //            {
                //                tra.TranslatedText = privatelabel.PrivateLabelShortName + " Services";
                //            }
                //            else
                //            {
                //                tra = new Translation()
                //                {
                //                    Id = Guid.NewGuid().ToString(),
                //                    TextCodeId = textCode.Id,
                //                    TextCode = textCode,
                //                    Tenant = tenant,
                //                    TranslatedText = privatelabel.PrivateLabelShortName + " Services",
                //                    TranslationHeaderCode = tenantPM.Language,
                //                    TextCodeCode = textCode.Code,
                //                };

                //                AllTranslations.Add(tra);
                //            }
                //        }
            }
            return AllTranslations;
        }

        [OperationContract]
        [WebGet(UriTemplate = "gettranslations/{translationTenant}")]
        [HttpGet("{translationTenant?}")]
        public List<Translation> GetTranslations(int translationTenant) => GetTenantLanguageTranslations(translationTenant);

        [OperationContract]
        [WebGet(UriTemplate = "GetTenantObjectFields/{loggedTenant}")]
        [HttpGet("{loggedTenant?}")]
        public List<ObjectFieldPM> GetTenantObjectFields(int loggedTenant)
        {
            loggedTenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            if (loggedTenant == 0)
            {
                return null;
            }
            return new ObjectFieldQueryService(loggedTenant).GetMulti(a => a.Tenant == loggedTenant, "ObjectTable").ToList();

        }

        [HttpGet("{tenant?}")]
        public List<TextCodePM> GetTenantTextCodes(int tenant)
                => new TextCodeQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetMulti(a => a.Tenant == tenant, "").ToList();

        [OperationContract]
        [WebGet(UriTemplate = "getloggeduserpm/{tenant}/{useremail}/{getloggeduser}")]
        [HttpGet("{tenant?}/{useremail?}/{getloggeduser?}")]
        public UserPM GetLoggedUserPM(int tenant, string useremail, bool getloggeduser)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            ContactPM contact = LoggedContactResolver.GetLoggedContact(tenant);
            if (contact == null)
            {
                return null;
            }
            if (!String.IsNullOrWhiteSpace(useremail) && useremail == contact.Email)
            {
                var service = new UserQueryService(tenant);
                return service.GetMulti(a => a.Contact.Email == contact.Email, "").FirstOrDefault();
            }
            return null;
        }

        [HttpGet("{tenant?}/{screenfields?}")]
        public List<ScreenFieldPM> GetAllScreenFieldsByTenant(int tenant, string screenfields)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new ScreenFieldQueryService(tenant).GetMulti(a => a.Tenant == tenant, "ObjectField,Screen,Screen.ObjectTable").ToList();

        }

        [HttpGet("{tenant?}/{screens?}")]
        public List<ScreenPM> GetAllScreensByTenant(int tenant, string screens)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new ScreenQueryService(tenant).GetMulti(a => a.Tenant == tenant, "ObjectTable");
        }

        [HttpGet("{tenant?}/{objecttabletabs?}")]
        public List<ObjectTableTabPM> GetAllObjectTableTabsByTenant(int tenant, string objecttabletabs)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new ObjectTableTabQueryService(tenant).GetMulti(a => a.Tenant == tenant, "ObjectTable").ToList();
        }

        [HttpGet("{tenant?}/{objecttables?}")]
        public List<ObjectTablePM> GetAllObjectTables(int tenant, string objecttables)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new ObjectTableQueryService(tenant).GetMulti(a => a.Tenant == tenant, "ObjectTableTabs").ToList();
        }

        [WebInvoke(
            UriTemplate = "api/ngMetaData/menustables",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "GET"
        )]
        [HttpGet("{tenant?}/{menustables?}")]
        public List<MenusTablePM> GetAllMenusTablesByTenant(int tenant, string menustables)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new MenusTableQueryService(tenant).GetMulti(a => a.Tenant == tenant, "").ToList();
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallstatuses/{tenant}/{inActive}/{dumb2}")]
        [HttpGet("{tenant?}/{inActive?}/{dumb2?}")]
        public HttpResponseMessage GetAllStatusesByTenant(int tenant, bool inActive, string dumb2)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                return Request.CreateResponse(HttpStatusCode.OK, new EntityStatusQueryService(tenant).GetMulti(a => a.Tenant == tenant && a.InActive == inActive, "").ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "getalldirections/{tenant}/{dummy2}")]
        [HttpGet("{tenant?}/{dummy2?}")]
        public List<DirectionPM> GetAllDirections(int tenant, string dummy2)
        {
            return new DirectionQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetMulti(a => true, "").ToList();
        }

        [OperationContract]
        [WebGet(UriTemplate = "getalltransportmodes/{tenant}/{dummy}")]
        [HttpGet("{tenant?}/{dummy?}")]
        public List<TransportModePM> GetAllTransportModes(int tenant, string dummy)
        => new TransportModeQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetMulti(a => true, "").ToList();

        [WebInvoke(
             UriTemplate = "api/ngMetaData/textcodetranslations",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             Method = "GET"
         )]
        [HttpGet("{tenant?}/{textcodetranslations?}")]
        public List<FieldsTranslationsPM> GetAllTextCodeTranslations(int tenant, string textcodetranslations)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new FieldsTranslationsQueryService(tenant).GetMulti(a => a.Tenant == tenant && a.TranslationLanguageCode == "EN", "").ToList();

        }

        [OperationContract]
        [WebGet(UriTemplate = "customsinterfacesettingpm/{InterfaceId}")]
        [HttpGet("{InterfaceId?}")]
        public CustomsInterfaceSettingPM GetCustomsInterfaceSettingPM(int InterfaceId)
        => new CustomsInterfaceSettingQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetSingle(InterfaceId, true, true);

        [OperationContract]
        [WebGet(UriTemplate = "getallmenubuttonssbyobjecttable/{tenant}/{objecttableid}/{menubuttons}")]
        [HttpGet("{tenant?}/{objecttableid?}/{menubuttons?}")]
        public List<MenuButtonPM> GetAllSpecialServicesTypesByTenant(int tenant, string objecttableid, bool menubuttons)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            var service = new MenuButtonQueryService(tenant);
            return service.GetMulti(a => true, "");
            //MenuButtonRepository menubuttonsRepository = new MenuButtonRepository(tenant);
            //MenuButtonQuery query = new MenuButtonQuery(menubuttonsRepository);
            ////var buttons = query.GetSpecialServicesTypePMsByTenant(tenant).ToList();
            //List<MenuButtonPM> buttons = (from a in menubuttonsRepository.context.MenuButtons
            //                              where a.MenuButtonGroupId == objecttableid
            //                              select new MenuButtonPM(a)
            //                              {
            //                                  Id = a.Id,
            //                                  ControlPath = a.ControlPath,
            //                                  DropDownControl = a.DropDownControl,
            //                                  EventCode = a.EventCode,
            //                                  FeatureId = a.FeatureId,
            //                                  Index = a.Index,
            //                                  IsActive = a.IsActive,
            //                                  //IsDisabled
            //                                  LabelTextCodeId = a.LabelTextCodeId,
            //                                  LabelTextCodeCode = a.LabelTextCodeCode,
            //                                  MenuButtonGroupId = a.MenuButtonGroupId,
            //                                  MenuButtonType = a.MenuButtonType,
            //                                  ParentMenuButtonId = a.ParentMenuButtonId,
            //                                  Style = a.Style,
            //                                  Width = a.Width,
            //                                  HtmlComponentPath = a.HtmlComponentPath,
            //                              }).ToList();
            //return buttons;
        }
    }
}