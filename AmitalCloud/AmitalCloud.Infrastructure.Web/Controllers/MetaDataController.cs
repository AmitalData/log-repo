using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
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
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new AdvancedQueryFilterQueryService(tenant).GetMulti(a => (a.Tenant == tenant || a.Tenant == 0) && a.IsPredefined == true, a => new AdvancedQueryFilterPM(a)
            {
                DisplayInList = a.ObjectField.DisplayInList,
                IsCustomFilter = a.ObjectField.IsCustomFilter,
                ObjectFieldName = a.ObjectField.FieldName,
                DataTypeCode = a.ObjectField.DataTypeCode,
                ObjectFieldOperator = a.ObjectField.Operator,
                QueryObjectTableName = a.Query.ObjectTable.Name,
                QueryUserId = a.Query.UserId,
            },
            "ObjectField,Query,Query.ObjectTable").ToList();
        }

        [OperationContract]
        [Route("GetTenantLanguageTranslations")]
        [WebGet(UriTemplate = "GetTenantLanguageTranslations/{tenant}")]
        public List<Translation> GetTenantLanguageTranslations(int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            List<Translation> AllTranslations = new List<Translation>();
            TenantPM tenantPM = new TenantQueryService(tenant).GetSingle(tenant, false, true);
            if (tenantPM != null && !string.IsNullOrEmpty(tenantPM.Language) && tenantPM.Language.ToLower() != "en" && tenantPM.Language != "english")
            {
                TranslationQuery service = new TranslationQuery(tenant);
                AllTranslations = service.GetTenantLanguageTranslations(tenant, tenantPM.Language);
            }
            return AllTranslations;
        }
        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "gettranslations/{translationTenant}")]
        public List<Translation> GetTranslations(int translationTenant)
        {
            translationTenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            TranslationQuery service = new TranslationQuery(translationTenant);
            List<Translation> AllTranslations = service.GetTenantTranslations(translationTenant);
            return AllTranslations;
        }
        [OperationContract]
        [Route("GetTenantObjectFields")]
        [WebGet(UriTemplate = "GetTenantObjectFields/{loggedTenant}")]
        public List<ObjectFieldPM> GetTenantObjectFields(int loggedTenant)
        {
            loggedTenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            if (loggedTenant == 0)
            {
                return null;
            }
            return new ObjectFieldQueryService(loggedTenant).GetMulti(a => a.Tenant == loggedTenant, a => new ObjectFieldPM(a)
            {
                ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
            }, "ObjectTable_LookUpTable,FullNameTextCode,ShortNameTextCode,ListTextCode,HelpTextCode,ObjectTable,ObjectTable_MultiTable").ToList();
        }

        [Route("GetTenantTextCodes")]
        public List<TextCodePM> GetTenantTextCodes(int tenant)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new TextCodeQueryService(tenant).GetMulti(a => a.Tenant == tenant, a => new TextCodePM(a)
            {
                ObjectTableName = a.ObjectTable.Name,
                SpellCheckedByUserName = a.SpellCheckedByUser == null ? null : a.SpellCheckedByUser.Contact.EnglishName,
            }, "ObjectTable,SpellCheckedByUser.Contact").ToList();
        }

        [HttpGet]
        [Route("")]
        [OperationContract]
        [WebGet(UriTemplate = "getloggeduserpm/{tenant}/{useremail}/{getloggeduser}")]
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
                return service.GetMulti(a => a.Contact.Email == contact.Email).FirstOrDefault();
            }
            return null;
        }
        [Route("")]
        public List<ScreenFieldPM> GetAllScreenFieldsByTenant(int tenant, string screenfields)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            ScreenFieldQuery query = new ScreenFieldQuery(tenant);
            return query.GetScreenFieldPMsByTenant(tenant);
        }
        [Route("")]
        public List<ScreenPM> GetAllScreensByTenant(int tenant, string screens)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            ScreenQuery query = new ScreenQuery(tenant);
            return query.GetScreenPMsByTenant(tenant);            
        }
        [Route("")]
        public List<ObjectTableTabPM> GetAllObjectTableTabsByTenant(int tenant, string objecttabletabs)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            ObjectTableTabQuery query = new ObjectTableTabQuery(tenant);
            var objectTableTabs = query.GetObjectTableTabPMsByTenant(tenant);
            return objectTableTabs;
        }
        [Route("")]
        public List<ObjectTablePM> GetAllObjectTables(int tenant, string objecttables)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
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
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new MenusTableQueryService(tenant).GetMulti(a => a.Tenant == tenant || a.Tenant == 0, a => new MenusTablePM(a)
            {
                ObjectTableName = a.ObjectTable.Name,
                FeatureCode = a.Feature.Code,
            }, "ObjectTable,Feature").ToList();
        }
        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getallstatuses/{tenant}/{inActive}/{dumb2}")]
        public HttpResponseMessage GetAllStatusesByTenant(int tenant, bool inActive, string dumb2)
        {
            try
            {
                tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
                return Request.CreateResponse(HttpStatusCode.OK, new EntityStatusQueryService(tenant).GetMulti(a => a.Tenant == tenant && a.InActive == inActive, a => new EntityStatusPM(a)
                {
                    ObjectTableName = a.ObjectTable.Name,
                }, "ObjectTable").ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, AmitalCloudApiExceptionBuilder.BuildException(ex));
            }
        }
        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getalldirections/{tenant}/{dummy2}")]
        public List<DirectionPM> GetAllDirections(int tenant, string dummy2)
        {
            return new DirectionQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetMulti(a => true).ToList();
        }
        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getalltransportmodes/{tenant}/{dummy}")]
        public List<TransportModePM> GetAllTransportModes(int tenant, string dummy)
        => new TransportModeQueryService(AmitalCloudSecurityUtility.AuthenticationOnTenant()).GetMulti(a => true).ToList();

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "accountingsettingpm/{id}")]
        public AccountingSettingPM GetAccountingSettingPM(int id)
        {
            id = AmitalCloudSecurityUtility.AuthenticationOnTenant();
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
            InterfaceId = AmitalCloudSecurityUtility.AuthenticationOnTenant();
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
            int tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();
            return new SharedLogisticsSettingQueryService(tenant).GetSingle(settingId.ToString(), false, false);
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallmenubuttonssbyobjecttable/{tenant}/{objecttableid}/{menubuttons}")]
        public List<MenuButtonPM> GetAllSpecialServicesTypesByTenant(int tenant, string objecttableid, bool menubuttons)
        {
            tenant =  AmitalCloudSecurityUtility.AuthenticationOnTenant();
            var service = new MenuButtonQueryService(tenant);
            return service.GetMulti(a => a.MenuButtonGroupId == objecttableid);
        }

        [OperationContract]
        [Route("")]
        [WebGet(UriTemplate = "getquerypms/{tenant}/{userid}/{objecttableid}")]
        public List<QueryPM> GetQueryPMs(int tenant, string UserId, string objecttableid)
        {
            tenant = AmitalCloudSecurityUtility.AuthenticationOnTenant();

            QueryQueryService queryQueryService = new QueryQueryService(tenant);
            List<QueryPM> queries = queryQueryService.GetMulti(a => (a.Tenant == tenant && (a.UserId == UserId || a.UserId == null)) || a.Tenant == 0 || a.SharedWithAll || a.SharedWithSpecificUsers, a => new QueryPM(a) 
            {
                ObjectTableName = a.ObjectTable.Name,
                ObjectTableIsNewWizard = a.ObjectTable.IsNewWizard,
                ObjectTableNewWizardControlName = a.ObjectTable.NewWizardControlName,
                QueryGroupIndexOrder = a.QueryGroup != null ? a.QueryGroup.IndexOrder : 0,
                NewViewName = a.NameTextCode == null ? null : a.NameTextCode.DefaultText,
            }, "ObjectTable,QueryGroup,NameTextCode");

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