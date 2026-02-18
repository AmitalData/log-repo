using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Model.EntityClasses ;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;

namespace AmitalCloud.Infrastructure.Web.Controllers
{
    [RoutePrefix("api/ngMetaData")]
    public class MetaDataController : ApiController
    {
        [OperationContract]
        [Route("GetAdvanceQueryFiltersPMs")]
        [WebGet(UriTemplate = "getadvancequeryfilterspms/{tenant}")]
        public List<AdvancedQueryFilterPM> GetAdvanceQueryFiltersPMs(int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var advanceQueryFilters = new AdvancedQueryFilterQueryService(tenant).GetMultiFromCache($"GetAdvanceQueryFiltersPMs{tenant}IsPredefined", a => (a.Tenant == tenant || a.Tenant == 0) && a.IsPredefined == true, "ObjectField,Query,Query.ObjectTable", a => new AdvancedQueryFilterPM(a)
            {
                DisplayInList = a.ObjectField.DisplayInList,
                IsCustomFilter = a.ObjectField.IsCustomFilter,
                ObjectFieldName = a.ObjectField.FieldName,
                DataTypeCode = a.ObjectField.DataTypeCode,
                ObjectFieldOperator = a.ObjectField.Operator,
                QueryObjectTableName = a.Query.ObjectTable.Name,
                QueryUserId = a.Query.UserId,
            });
            return advanceQueryFilters;
        }

        [OperationContract]
        [Route("GetTenantLanguageTranslations")]
        [WebGet(UriTemplate = "GetTenantLanguageTranslations/{tenant}")]
        public List<Translation> GetTenantLanguageTranslations(int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            List<Translation> AllTranslations = new List<Translation>();
            TenantPM tenantPM = new TenantQueryService(tenant).GetSingle(tenant, false, true);
            if (tenantPM != null && !string.IsNullOrEmpty(tenantPM.Language) && tenantPM.Language.ToLower() != "en" && tenantPM.Language != "english")
            {
				var url = AmitalCloudSecurityUtility.getLoggedDomain();
                AllTranslations = new TranslationQuery(tenant).GetTenantLanguageTranslations(tenantPM.Language, url);
            }
            return AllTranslations;
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "gettranslations/{translationTenant}")]
        public List<Translation> GetTranslations(int translationTenant)
        {
            translationTenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var url = AmitalCloudSecurityUtility.getLoggedDomain();
            List<Translation> AllTranslations = new TranslationQuery(translationTenant).GetTenantTranslations(url);
            return AllTranslations;
        }

        [OperationContract]
        [Route("GetTenantObjectFields")]
        [WebGet(UriTemplate = "GetTenantObjectFields/{loggedTenant}")]
        public List<ObjectFieldPM> GetTenantObjectFields(int loggedTenant)
        {
            loggedTenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            if (loggedTenant == 0)
            {
                return null;
            }
            var objectFields = new ObjectFieldQueryService(loggedTenant).GetMultiFromCache($"objectFields{loggedTenant}", a => a.Tenant == loggedTenant, "ObjectTable_LookUpTable,FullNameTextCode,ShortNameTextCode,ListTextCode,HelpTextCode,ObjectTable,ObjectTable_MultiTable", a => new ObjectFieldPM(a)
            {
                ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
            });
            return objectFields;
        }

        [Route("GetTenantTextCodes")]
        public List<TextCodePM> GetTenantTextCodes(int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var textCodes = new TextCodeQueryService(tenant).GetMultiFromCache($"textCode{tenant}", a => a.Tenant == tenant, "ObjectTable,SpellCheckedByUser.Contact", a => new TextCodePM(a)
            {
                ObjectTableName = a.ObjectTable.Name,
                SpellCheckedByUserName = a.SpellCheckedByUser == null ? null : a.SpellCheckedByUser.Contact.EnglishName,
            });
            return textCodes;
        }

        [HttpGet]
        [Route("")]
        [OperationContract]
        [WebGet(UriTemplate = "getloggeduserpm/{tenant}/{useremail}/{getloggeduser}")]
        public UserPM GetLoggedUserPM(int tenant, string useremail, bool getloggeduser)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            ContactPM contact = LoggedContactResolver.GetLoggedContact(tenant);
            if (contact == null)
            {
                return null;
            }
            if (!String.IsNullOrWhiteSpace(useremail) && useremail == contact.Email)
            {
                var service = new UserQueryService(tenant);
                return service.GetMulti(a => a.Contact.Email == contact.Email).FirstOrDefault();
            }
            return null;
        }

        [Route("")]
        public List<ScreenFieldPM> GetAllScreenFieldsByTenant(int tenant, string screenfields)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var screenFields = new ScreenFieldQuery(tenant).GetScreenFieldPMsByTenant();
            return screenFields;
        }

        [Route("")]
        public List<ScreenPM> GetAllScreensByTenant(int tenant, string screens)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var allScreens = new ScreenQuery(tenant).GetScreenPMsByTenant();
            return allScreens;
        }

        [Route("")]
        public List<ObjectTableTabPM> GetAllObjectTableTabsByTenant(int tenant, string objecttabletabs)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var objectTableTabs = new ObjectTableTabQuery(tenant).GetObjectTableTabPMsByTenant();
            return objectTableTabs;
        }

        [Route("")]
        public List<ObjectTablePM> GetAllObjectTables(int tenant, string objecttables)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            ObjectTableQuery query = new ObjectTableQuery(tenant);
            var objectTables = query.GetObjectPMsByTenant(tenant).ToList();
            return objectTables;
        }

        [WebInvoke(
            UriTemplate = "api/ngMetaData/menustables",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "GET"
        )]
        [Route("")]
        public List<MenusTablePM> GetAllMenusTablesByTenant(int tenant, string menustables)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var menuTables = new MenusTableQueryService(tenant).GetMultiFromCache($"GetAllMenusTablesByTenant{tenant}", a => a.Tenant == tenant || a.Tenant == 0, "ObjectTable,Feature", a => new MenusTablePM(a)
            {
                ObjectTableName = a.ObjectTable?.Name,
                FeatureCode = a.Feature?.Code,
            });
            return menuTables;
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getallstatuses/{tenant}/{inActive}/{dumb2}")]
        public HttpResponseMessage GetAllStatusesByTenant(int tenant, bool inActive, string dumb2)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
                var statuses = new EntityStatusQueryService(tenant).GetMultiFromCache($"entityStatus{tenant}-{inActive}", a => a.Tenant == tenant && a.InActive == inActive, "ObjectTable", a => new EntityStatusPM(a)
                {
                    ObjectTableName = a.ObjectTable.Name,
                });
                return Request.CreateResponse(HttpStatusCode.OK, statuses);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getalldirections/{tenant}/{dummy2}")]
        public List<DirectionPM> GetAllDirections(int tenant, string dummy2)
        {
            var directions = new DirectionQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetMulti(a => true).ToList();
            return directions;
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getalltransportmodes/{tenant}/{dummy}")]
        public List<TransportModePM> GetAllTransportModes(int tenant, string dummy)
        {
            var transportModes = new TransportModeQueryService(AmitalCloudSecurityUtility.AuthenticateTenant()).GetMulti(a => true).ToList();
            return transportModes;
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "accountingsettingpm/{id}")]
        public AccountingSettingPM GetAccountingSettingPM(int id)
        {
            id = AmitalCloudSecurityUtility.AuthenticateTenant();
            return new AccountingSettingQueryService(id).GetMulti(a => a.Id == id, a => new AccountingSettingPM(a)
            {
                TransferFTPDetailHost = a.TransferFTPDetail == null ? null : a.TransferFTPDetail.Host,
            }, "TransferFTPDetail").FirstOrDefault();
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "customsinterfacesettingpm/{InterfaceId}")]
        public CustomsInterfaceSettingPM GetCustomsInterfaceSettingPM(int InterfaceId)
        {
            InterfaceId = AmitalCloudSecurityUtility.AuthenticateTenant();
            return new CustomsInterfaceSettingQueryService(InterfaceId).GetMulti(a => a.Tenant == InterfaceId, a => new CustomsInterfaceSettingPM(a)
            {
                ArtemusOutSettingsHost = a.ArtemusOutSettings == null ? null : a.ArtemusOutSettings.Host,
                ArtemusInSettingsHost = a.ArtemusInSettings == null ? null : a.ArtemusInSettings.Host,
                LocalCustomsInterfaceName = a.LocalCustomsInterface == null ? null : a.LocalCustomsInterface.Name,
            }, "ArtemusOutSettings,ArtemusInSettings,LocalCustomsInterface").FirstOrDefault();
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "shaerdlogisticssettingpm/{settingId}")]
        public SharedLogisticsSettingPM GetSharedLogisticsSettingM(int settingId)
        {
            int tenant = AmitalCloudSecurityUtility.AuthenticateTenant();
            var sharedLogisticsSetting = new SharedLogisticsSettingQueryService(tenant).GetSingle(settingId.ToString(), false, true);
            return sharedLogisticsSetting;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallmenubuttonssbyobjecttable/{tenant}/{objecttableid}/{menubuttons}")]
        public List<MenuButtonPM> GetAllSpecialServicesTypesByTenant(int tenant, string objecttableid, bool menubuttons)
        {
            tenant =  AmitalCloudSecurityUtility.AuthenticateTenant();
            var service = new MenuButtonQueryService(tenant);
            return service.GetMulti(a => a.MenuButtonGroupId == objecttableid);
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getquerypms/{tenant}/{userid}/{objecttableid}")]
        public List<QueryPM> GetQueryPMs(int tenant, string UserId, string objecttableid)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticateTenant();

            QueryQueryService queryQueryService = new QueryQueryService(tenant);
            List<QueryPM> queries = queryQueryService.GetMultiFromCache($"GetQueryPMs{tenant}-{UserId}", a => (a.Tenant == tenant && (a.UserId == UserId || a.UserId == null)) || a.Tenant == 0 || a.SharedWithAll || a.SharedWithSpecificUsers, "ObjectTable,QueryGroup,NameTextCode", a => new QueryPM(a) 
            {
                ObjectTableName = a.ObjectTable.Name,
                ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
            });

            List<QueryPM> myResult = new List<QueryPM>();
            SharedUserQueryQueryService sharedUserQueryQueryService = new SharedUserQueryQueryService(tenant);
            List<SharedUserQueryPM> sharedUserQueries = sharedUserQueryQueryService.GetMulti(a => a.Tenant == tenant);
            foreach (QueryPM item in queries)
            {
                if (!item.SharedWithSpecificUsers || (item.SharedWithSpecificUsers && (sharedUserQueries.Where(d => d.QueryCode == item.UniqueCode && d.UserId == UserId).Any() || item.SharedByUserId == UserId)))
                {
                    myResult.Add(item);
                }
            }
            myResult = myResult.OrderBy(d => d.IndexOrder).ToList();
            return myResult;
        }
    }
}