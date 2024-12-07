using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.WebServices;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.CustomFilters;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.IdentityModel.Protocols.WSTrust;
using Logitude.SystemLogs;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using WebFreight.Web.Helpers.APIHelpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    //[ApiExceptionFilter]
    public class ngMetaDataController : ApiController
    {

        public ngMetaDataController()
        {

        }

      
        public UserPM GetAuthenticatedUserDetails(string userid, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(tenant);
                contact = contactrep.GetSingleContactByEmail(email, tenant);
            }
            UserRepository userrep = new UserRepository(tenant);
            UserQuery userquery = new UserQuery(userrep);
            var userPM = userquery.GetSingleUserPMByEmail(email, tenant, false);

            return userPM;
        }

        public TenantPM GetAuthenticatedTenantDetails(int tenantid)
        {

            SecurityUtility.AuthenticationOnTenant(tenantid);
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(tenantid);
                contact = contactrep.GetSingleContactByEmail(email, tenantid);
            }

            TenantRepository tenantRepository = new TenantRepository(tenantid);
            TenantQuery tenantQuery = new TenantQuery(tenantRepository);
            var tenantPM = tenantQuery.GetSinglePM(tenantid);

            return tenantPM;
        }

        public List<ScreenFieldPM> GetAllScreenFieldsByTenant(int tenant, string screenfields)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            ScreenFieldsRepository repository = new ScreenFieldsRepository(tenant);
            ScreenFieldsQuery query = new ScreenFieldsQuery(repository);
            var screenfieldsList = query.GetScreenFieldPMsByTenant(tenant);

            return screenfieldsList;
        }

        public List<ScreenPM> GetAllScreensByTenant(int tenant, string screens)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            ScreensRepository repository = new ScreensRepository(tenant);
            ScreensQuery query = new ScreensQuery(repository);
            var screensList = query.GetScreenPMsByTenant(tenant);

            return screensList;
        }

        public List<ObjectTableTabPM> GetAllObjectTableTabsByTenant(int tenant, string objecttabletabs)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            ObjectTableTabRepository repository = new ObjectTableTabRepository(tenant);
            ObjectTableTabQuery query = new ObjectTableTabQuery(repository);
            var objectTableTabs = query.GetObjectTableTabPMsByTenant(tenant).ToList();

            return objectTableTabs;
        }



        public List<ObjectTablePM> GetAllObjectTables(int tenant, string objecttables)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            ObjectTableRepository repository = new ObjectTableRepository(tenant);
            ObjectTableQuery query = new ObjectTableQuery(repository);
            var objectTables = query.GetObjectPMsByTenant(tenant).ToList();

            return objectTables;
        }

        [WebInvoke(
            UriTemplate = "api/ngMetaData/menustables",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "GET"
        )]
        public List<MenusTablePM> GetAllMenusTablesByTenant(int tenant, string menustables)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

            MenusTableRepository menuRepository = new MenusTableRepository(tenant);
            MenusTableQuery menuQuery = new MenusTableQuery(menuRepository);
            var menus = menuQuery.GetMenusTablePMsByTenant(tenant).ToList();
            //menus = menus.Where(x => x.MenuTypeCode == "Main").ToList();
            return menus;
        }

        [WebInvoke(
            UriTemplate = "api/ngMetaData/textcodetranslations",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "GET"
        )]
        public List<FieldsTranslations> GetAllTextCodeTranslations(int tenant, string textcodetranslations)
        {
             string token = HttpContext.Current.Request.Headers["Token"];
           
            GeneralDomainService service = new GeneralDomainService();
            var textCodeList = service.GetAllFieldsTranslations(tenant, "EN", 0, 220000).ToList();
            
            return textCodeList;
        }


        public List<TextCodePM> GetTenantTextCodes(int tenant)
        {
             string token = HttpContext.Current.Request.Headers["Token"];
            
            GeneralDomainService service = new GeneralDomainService();
            var textCodeList = service.GetTextCodesByTenant(tenant).ToList();
           
            return textCodeList;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallcards/{tenant}/{inActive}/{dumb}")]
        public List<CardPM> GetAllCardsByTenant(int tenant, bool inActive, string dumb)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            CardRepository cardRepository = new CardRepository(tenant);
            List<CardPM> cards = (from a in cardRepository.context.Cards
                                  where a.InActive == inActive
                                  && a.Tenant == tenant
                                  select new CardPM()
                                  {
                                      Id = a.Id,
                                      EnglishName = a.EnglishName,
                                      Code = a.Code,
                                      LocalName = a.LocalName,
                                      PartnerTypeId = a.PartnerTypeId,
                                      PartnerTypeName = a.PartnerType.Name,
                                      InActive = a.InActive,
                                      CityName = a.CityName,
                                      CountryCode = a.CountryCode,
                                      CountryId = a.CountryId,
                                      CountryName = a.CountryName
                                  }).ToList();
            return cards;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallports/{tenant}/{inActive}/{inland}/{air}/{ocean}")]
        public List<PortPM> GetAllPortsByTenant(int tenant, bool inActive, bool inland, bool air, bool ocean)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            PortRepository portsRepository = new PortRepository(tenant);
            PortQuery query = new PortQuery(portsRepository);
            var ports = query.GetPortPMsByTenant(tenant).ToList();
            //List<PortPM> ports = (from a in portsRepository.context.Ports
            //                      where a.Tenant == tenant
            //                      && a.InActive == inActive
            //                      //&& a.IsAir == air || a.IsInland == inland || a.IsOcean == ocean
            //                      select new PortPM()
            //                      {
            //                          Id = a.Id,
            //                          EnglishName = a.EnglishName,
            //                          Code = a.Code,
            //                          IsOcean = a.IsOcean,
            //                          IsAir = a.IsAir,
            //                          IsInland = a.IsInland
            //                      }).ToList();
            return ports;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallstatuses/{tenant}/{inActive}/{dumb2}")]
        public HttpResponseMessage GetAllStatusesByTenant(int tenant, bool inActive, string dumb2)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //EntityStatusRepository statusRepository = new EntityStatusRepository(tenant);
            //List<EntityStatusPM> statuses = (from a in statusRepository.context.EntityStatus
            //                                 where a.Tenant == tenant
            //                                 && a.InActive == inActive
            //                                 select new EntityStatusPM()
            //                                 {
            //                                     Id = a.Id,
            //                                     Name = a.Name,
            //                                     Code = a.Code,
            //                                     Tenant = a.Tenant,
            //                                     InActive = a.InActive,
            //                                     ObjectTableId = a.ObjectTableId,
            //                                     ObjectTableName = a.ObjectTable.Name,
            //                                     StatusWeight = a.StatusWeight
            //                                 }).ToList();
            //return statuses;
            try
            {
                //SecurityUtility.AuthenticationOnTenant(tenant);
                //SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", tenant);
                EntityStatusRepository statusRepository = new EntityStatusRepository(tenant);
                List<EntityStatusPM> statuses = (from a in statusRepository.context.EntityStatus
                                                 where a.Tenant == tenant
                                                 && a.InActive == inActive
                                                 select new EntityStatusPM()
                                                 {
                                                     Id = a.Id,
                                                     Name = a.Name,
                                                     Code = a.Code,
                                                     Tenant = a.Tenant,
                                                     InActive = a.InActive,
                                                     ObjectTableId = a.ObjectTableId,
                                                     ObjectTableName = a.ObjectTable.Name,
                                                     StatusWeight = a.StatusWeight
                                                 }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, statuses);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [OperationContract]
        [WebGet(UriTemplate = "getalltransportmodes/{tenant}/{dummy}")]
        public List<TransportModePM> GetAllTransportModes(int tenant, string dummy)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            TransportModeRepository transportRepository = new TransportModeRepository(tenant);
            List<TransportModePM> transportModes = (from a in transportRepository.context.TransportModes
                                                        //where a.Id == "I"
                                                    select new TransportModePM()
                                                    {
                                                        Id = a.Id,
                                                        Name = a.Name
                                                    }).ToList();
            return transportModes;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getalldirections/{tenant}/{dummy2}")]
        public List<DirectionPM> GetAllDirections(int tenant, string dummy2)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            DirectionRepository directionRepository = new DirectionRepository(tenant);
            List<DirectionPM> directions = (from a in directionRepository.context.Directions
                                                //where a.Id == "E"
                                            select new DirectionPM()
                                            {
                                                Id = a.Id,
                                                Name = a.Name
                                            }).ToList();
            return directions;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallobjectfields/{tenant}/{objectTableName}/{inActive}")]
        public List<ObjectFieldPM> GetAllObjectFieldsByTenantByObjectTable(int tenant, string objectTableName, bool inActive)
        {
            int tabletenant = 0;
            //SecurityUtility.AuthenticationOnTenant(tenant);
            ObjectFieldRepository repository = new ObjectFieldRepository(tenant);
            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            ObjectFieldQuery query = new ObjectFieldQuery(repository);

            var tableId = tableRepository.GetObjectTableByName(objectTableName, tenant, false).Id;

            List<string> list = new List<string>();

            var objectFields = query.GetObjectFieldPMsByTenant(tabletenant, tenant).ToList();
            //.Where(x => x.ObjectTableId == "1-1" || x.ObjectTableId == "1-4" || x.ObjectTableId == "1-5" || x.ObjectTableId == "1-14" || x.ObjectTableId == "1-11" || x.ObjectTableId == "1-6")
            //.ToList();
            return objectFields;
        }

        [OperationContract]
        [WebGet(UriTemplate = "GetTenantObjectFields/{loggedTenant}")]
        public List<ObjectFieldPM> GetTenantObjectFields(int loggedTenant)
        {
            if (loggedTenant != 0)
            {
                ObjectFieldRepository repository = new ObjectFieldRepository(loggedTenant);
                ObjectTableRepository tableRepository = new ObjectTableRepository(loggedTenant);
                ObjectFieldQuery query = new ObjectFieldQuery(repository);


                var objectFields = query.GetObjectFieldPMsByTenant(loggedTenant, loggedTenant);
                //.Where(x => x.ObjectTableId == "1-1" || x.ObjectTableId == "1-4" || x.ObjectTableId == "1-5" || x.ObjectTableId == "1-14" || x.ObjectTableId == "1-11" || x.ObjectTableId == "1-6")
                //.ToList();
                return objectFields;
            }
            else
            {
                return null;
            }
        }


        [OperationContract]
        [WebGet(UriTemplate = "getportssearchresults/{tenant}/{searchfields}/{inActive}/{inland}/{air}/{ocean}")]
        public List<PortPM> GetPortsSearchResults(int tenant, string searchfields, bool inActive, bool inland, bool air, bool ocean)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            PortRepository repo = new PortRepository(tenant);
            List<PortPM> ports = (from a in repo.context.Ports
                                  where a.Tenant == tenant
                                  && a.InActive == inActive
                                  && a.IsAir == air && a.IsInland == inland && a.IsOcean == ocean
                                  && a.SearchFields.Contains(searchfields)
                                  select new PortPM()
                                  {
                                      Id = a.Id,
                                      EnglishName = a.EnglishName,
                                      Code = a.Code,
                                      IsOcean = a.IsOcean,
                                      IsAir = a.IsAir,
                                      IsInland = a.IsInland,
                                      CountryName = a.Country.EnglishName,
                                      CountryCode = a.Country.Code
                                  }).ToList();
            return ports;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getdirectionssearchresults/{tenant}/{searchfields}")]
        public List<DirectionPM> GetDirectionsSearchResults(int tenant, string searchfields)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.AuthenticationOnTenant(tenant);
            DirectionRepository repo = new DirectionRepository(tenant);
            List<DirectionPM> directionsList = (from a in repo.context.Directions
                                                where a.SearchFields.Contains(searchfields)
                                                select new DirectionPM()
                                                {
                                                    Id = a.Id,
                                                    Name = a.Name,
                                                }).ToList();
            return directionsList;
        }

        [OperationContract]
        [WebGet(UriTemplate = "gettransportmodessearchresults/{tenant}/{searchfields}")]
        public List<TransportModePM> GetTransportModesSearchResults(int tenant, string searchfields)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.AuthenticationOnTenant(tenant);
            TransportModeRepository repo = new TransportModeRepository(tenant);
            List<TransportModePM> transportModesList = (from a in repo.context.TransportModes
                                                        where a.SearchFields.Contains(searchfields)
                                                        select new TransportModePM()
                                                        {
                                                            Id = a.Id,
                                                            Name = a.Name,
                                                        }).ToList();
            return transportModesList;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getcardssearchresults/{tenant}/{searchfields}/{inActive}")]
        public List<CardPM> GetCardsSearchResults(int tenant, string searchfields, bool inActive)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            CardRepository repo = new CardRepository(tenant);
            List<CardPM> cards = (from a in repo.context.Cards
                                  where a.Tenant == tenant
                                  && a.InActive == inActive
                                  && a.SearchFields.Contains(searchfields)
                                  select new CardPM()
                                  {
                                      Id = a.Id,
                                      EnglishName = a.EnglishName,
                                      Code = a.Code,
                                      LocalName = a.LocalName,
                                      PartnerTypeId = a.PartnerTypeId,
                                      PartnerTypeName = a.PartnerType.Name,
                                      InActive = a.InActive,
                                      CityName = a.CityName,
                                      CountryCode = a.CountryCode,
                                      CountryId = a.CountryId,
                                      CountryName = a.CountryName
                                  }).Take(10).ToList();
            return cards;
        }


        [OperationContract]
        [WebGet(UriTemplate = "getallbranchesbytenant/{tenant}/{inActive}/{orenodummydesu}")]
        public List<BranchPM> GetAllBranchesByTenant(int tenant, bool inActive, bool orenodummydesu)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            BranchRepository branchRepository = new BranchRepository(tenant);
            BranchQuery query = new BranchQuery(branchRepository);
            var branches = query.GetBranchPMsByTenant(tenant).ToList();
            return branches;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getalldepartmentsbytenant/{tenant}/{inActive}/{orenodummydesu}")]
        public List<DepartmentPM> GetAllDepartmentsByTenant(int tenant, bool inActive, bool orenodummy)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            DepartmentRepository departmentRepository = new DepartmentRepository(tenant);
            DepartmentQuery query = new DepartmentQuery(departmentRepository);
            var departments = query.GetDepartmentPMsByTenant(tenant).ToList();
            return departments;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallincotermsbytenant/{tenant}/{inActive}/{incotermsy}")]
        public List<IncotermPM> GetAllIncotermsByTenant(int tenant, bool inActive, bool incotermsy)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            IncotermRepository incotermRepository = new IncotermRepository(tenant);
            IncotermQuery query = new IncotermQuery(incotermRepository);
            var incoterms = query.GetIncotermPMsByTenant(tenant).ToList();
            return incoterms;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallusersbytenant/{tenant}/{inActive}/{usersy}")]
        public List<UserPM> GetAllUsersByTenant(int tenant, bool inActive, bool usersy)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            UserQuery query = new UserQuery(userRepository);
            var users = query.GetUserPMsByTenant(tenant).ToList();
            return users;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallprepaidcollectsbytenant/{tenant}/{inActive}/{prepaidcollect}")]
        public List<PrepaidCollectPM> GetAllPrepaidCollectsByTenant(int tenant, bool inActive, bool prepaidcollect)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            PrepaidCollectRepository prepaidcollectRepository = new PrepaidCollectRepository(tenant);
            PrepaidCollectQuery query = new PrepaidCollectQuery(prepaidcollectRepository);
            var prepaidcollects = query.GetPrepaidCollectPMs().ToList();
            return prepaidcollects;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallmovetypesbytenant/{tenant}/{inActive}/{movetypesy}")]
        public List<MoveTypePM> GetAllMoveTypesByTenant(int tenant, bool inActive, bool movetypesy)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MoveTypeRepository movetypeRepository = new MoveTypeRepository(tenant);
            MoveTypeQuery query = new MoveTypeQuery(movetypeRepository);
            var movetypes = query.GetMoveTypePMs(tenant).ToList();
            return movetypes;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallspecialservicestypesbytenant/{tenant}/{inActive}/{specialservicestyp}")]
        public List<SpecialServicesTypePM> GetAllSpecialServicesTypesByTenant(int tenant, bool inActive, bool specialservicestyp)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            SpecialServicesTypeRepository specialservicestypeRepository = new SpecialServicesTypeRepository(tenant);
            SpecialServicesTypeQuery query = new SpecialServicesTypeQuery(specialservicestypeRepository);
            var specialservicestypes = query.GetSpecialServicesTypePMsByTenant(tenant).ToList();
            return specialservicestypes;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getallmenubuttonssbyobjecttable/{tenant}/{objecttableid}/{menubuttons}")]
        public List<MenuButtonPM> GetAllSpecialServicesTypesByTenant(int tenant, string objecttableid, bool menubuttons)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            MenuButtonRepository menubuttonsRepository = new MenuButtonRepository(tenant);
            MenuButtonQuery query = new MenuButtonQuery(menubuttonsRepository);
            //var buttons = query.GetSpecialServicesTypePMsByTenant(tenant).ToList();
            List<MenuButtonPM> buttons = (from a in menubuttonsRepository.context.MenuButtons
                                          where a.MenuButtonGroupId == objecttableid
                                          select new MenuButtonPM()
                                          {
                                              Id = a.Id,
                                              ControlPath = a.ControlPath,
                                              DropDownControl = a.DropDownControl,
                                              EventCode = a.EventCode,
                                              FeatureId = a.FeatureId,
                                              Index = a.Index,
                                              IsActive = a.IsActive,
                                              //IsDisabled
                                              LabelTextCodeId = a.LabelTextCodeId,
                                              LabelTextCodeCode = a.LabelTextCodeCode,
                                              MenuButtonGroupId = a.MenuButtonGroupId,
                                              MenuButtonType = a.MenuButtonType,
                                              ParentMenuButtonId = a.ParentMenuButtonId,
                                              Style = a.Style,
                                              Width = a.Width,
                                              HtmlComponentPath = a.HtmlComponentPath,
                                          }).ToList();
            return buttons;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getloggeduserpm/{tenant}/{useremail}/{getloggeduser}")]
        public UserPM GetLoggedUserPM(int tenant, string useremail, bool getloggeduser)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // GetGlobalContactByEmailAndTenant2
            var objectContext = GlobalContext.GetContext();
            GlobalContactRepository globalContactsRepository = new GlobalContactRepository(objectContext);

            bool sameEmail = false;
            ContactPM contact = LoggedContactResolver.GetLoggedContact(tenant);
            if (contact != null)
            {
                string email = contact.Email;
                if (!String.IsNullOrWhiteSpace(useremail) && useremail == email)
                {
                    sameEmail = true;
                }
            }
            UserPM loggeduser = null;
            if (sameEmail)
            {
                GlobalContact globalContact = globalContactsRepository.GetGlobalContactByEmailAndTenant(useremail, tenant);

                if (globalContact != null)
                {
                    UserRepository userRepository = new UserRepository(tenant);
                    UserQuery query = new UserQuery(userRepository);
                    loggeduser = query.GetSingleUserPMByEmail(useremail, globalContact.GlobalTenantId, false);
                    if (globalContact.GlobalTenantId == 0 && LogitudeSettings.IsCostomsDeploy && loggeduser == null)// in custom allowed sysdamin login to the tenant 
                    {
                        loggeduser = query.GetSingleUserPMByEmail(useremail, tenant, false);
                    }
                }
                if (loggeduser != null)
                {
                    loggeduser.DisableCachedData = FeatureToggleHelper.HasFeatureToggle("DCS", tenant);
                }
            }
            return loggeduser;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getloggedtenantpm/{tenant}/{getloggedtenant}")]
        public TenantPM GetLoggedTenantPM(int tenant, bool getloggedtenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            TenantRepository tenantRepository = new TenantRepository(tenant);
            TenantQuery query = new TenantQuery(tenantRepository);
            var loggedtenant = query.GetSinglePM(tenant);
            return loggedtenant;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getquerypm/{tenant}/{query}/{objecttableid}")]
        public List<QueryPM> GetQueryPM(int tenant, string query, string objecttableid)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            QueryRepository queryRepository = new QueryRepository(tenant);
            QueryQuery queryQuery = new QueryQuery(queryRepository);
            var queryFirst = queryQuery.GetQueryPMsByTenant(tenant)
                //.Where(x => x.ObjectTableId == objecttableid)
                .OrderBy(d => d.IndexOrder)
                .ToList(); // .FirstOrDefault();
            return queryFirst;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getquerypms/{tenant}/{userid}/{objecttableid}")]
        public List<QueryPM> GetQueryPMs(int tenant, string UserId, string objecttableid)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            QueryRepository queryRepository = new QueryRepository(tenant);
            QueryQuery queryQuery = new QueryQuery(queryRepository);
            var queryFirst = queryQuery.GetQueries_Login(tenant, UserId)
                //.Where(x => x.ObjectTableId == objecttableid)
                .OrderBy(d => d.IndexOrder)
                .ToList(); // .FirstOrDefault();
            return queryFirst;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getquerycolumnpms/{tenant}/{queryCode}/{objecttableid}/{userid}/{getfromsystemlevel}")]
        public List<QueryColumnPM> GetQueryColumnPMs(int tenant, string queryCode, string objecttableid, string userid, bool getfromsystemlevel)
        {
            QueryColumnRepository queryColumnRepository = new QueryColumnRepository(tenant);
            QueryColumnQuery queryColumnQuery = new QueryColumnQuery(queryColumnRepository);
            var queryColumns = getfromsystemlevel ? null : queryColumnQuery.GetQueryColumnsByQueryCodeAndUser(tenant, userid, queryCode).ToList();

            
            if (queryColumns != null && queryColumns.Count() > 0)
            {
                if(LogitudeSettings.WorkEnvironment != "cloud")
                {
                    ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
                    ObjectTable objectTable = tableRepository.GetSingleObjectTable(objecttableid, tenant, true);

                    if (queryCode == "ARInvoice.All Invoices" && objectTable != null && objectTable.Name == "ARInvoice")
                    {
                        queryColumns.RemoveAll(q =>
                            q.ObjectFieldName == "TotalVAT" ||
                            q.ObjectFieldName == "TotalExamptFortaxReport" ||
                            q.ObjectFieldName == "TotalAmountNotForTaxReport" ||
                            q.ObjectFieldName == "TotalAmountForTaxReport" ||
                            q.ObjectFieldName == "TotaVatableAmountForTaxReport"
                        );
                    }
                }
                return queryColumns.OrderBy(a => a.IndexOrder).ToList();
            }

            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            QueryColumnService queryColumnService = new QueryColumnService(webFreightContext, tenant);
            return queryColumnService.GetSystemMetaDataQueryColumns(objecttableid, tenant, queryCode);
        }

        [OperationContract]
        [WebGet(UriTemplate = "getmenubuttongrouppms/{tenant}/{objecttableid}")]
        public List<MenuButtonGroupPM> GetMenuButtonGroupPMs(int tenant, string objecttableid)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            MenuButtonGroupRepository menuButtonGroupRepository = new MenuButtonGroupRepository(tenant);
            MenuButtonGroupQuery menuButtonGroupQuery = new MenuButtonGroupQuery(menuButtonGroupRepository);
            var menuButtons = menuButtonGroupQuery.GetMenuButtonGroupsByObjectTable(objecttableid, tenant);

            return menuButtons;

        }

        [OperationContract]
        [WebGet(UriTemplate = "getadvancequeryfilterspms/{tenant}")]
        public List<AdvancedQueryFilterPM> GetAdvanceQueryFiltersPMs(int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            AdvancedQueryFilterRepository advancedQueryFilterRepository = new AdvancedQueryFilterRepository(tenant);
            AdvancedQueryFilterQuery advancedQueryFilterQuery = new AdvancedQueryFilterQuery(advancedQueryFilterRepository);
            var Filters = advancedQueryFilterQuery.GetPredefinedQueryFilters(tenant);

            return Filters.ToList();

        }

        [OperationContract]
        [WebGet(UriTemplate = "gettranslations/{translationTenant}")]
        public List<Translation> GetTranslations(int translationTenant)
        {
            TextCodeRepository txtCodeRep = new TextCodeRepository(translationTenant);
            TranslationRepository translationRep = new TranslationRepository(translationTenant);

            List<Translation> AllTranslations = translationRep.GetTranslationsByTenantList(translationTenant);
            TenantRepository tenantRep = new TenantRepository(translationTenant);
            Tenant tenantPoco = tenantRep.GetSingleByTenant(translationTenant);
            TenantManagmentPrivateLabelsPM privatelabel = null;
            var url = SecurityUtility.getLoggedDomain();
            if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
            {
                TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                privatelabel = query.GetSingleActivePMByUrl_Cache(url);
            }
            if (privatelabel != null)
            {
                TextCode textCode = txtCodeRep.GetTextCodeByTenantAndCode("General.MH.Importers", translationTenant);
                Translation tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                if (tra != null)
                {
                    tra.TranslatedText = privatelabel.PrivateLabelName;
                }
                else
                {
                    tra = new Translation()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TextCodeId = textCode.Id,
                        TextCode = textCode,
                        Tenant = translationTenant,
                        TranslatedText = privatelabel.PrivateLabelName,
                        TranslationHeaderCode = tenantPoco.Language,
                        TextCodeCode = textCode.Code,

                    };

                    AllTranslations.Add(tra);
                }
                textCode = txtCodeRep.GetTextCodeByTenantAndCode("General.MH.ActivationWizard", translationTenant);
                tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                if (tra != null)
                {
                    tra.TranslatedText = privatelabel.PrivateLabelShortName + " Services";
                }
                else
                {
                    tra = new Translation()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TextCodeId = textCode.Id,
                        TextCode = textCode,
                        Tenant = translationTenant,
                        TranslatedText = privatelabel.PrivateLabelShortName + " Services",
                        TranslationHeaderCode = tenantPoco.Language,
                        TextCodeCode = textCode.Code,
                    };

                    AllTranslations.Add(tra);
                }

                //var TextCodeId = service.GetTextCodesByTenant(tenant).Where(a => a.Code == "General.MH.Importers").First().Id;
                //textCodeList.Where(a => a.TextCodeId == TextCodeId).First().DefaultText = privatelabel.PrivateLabelName;
            }
            return AllTranslations;
        }



        [OperationContract]
        [WebGet(UriTemplate = "GetTenantLanguageTranslations/{tenant}")]
        public List<Translation> GetTenantLanguageTranslations(int tenant)
        {

            List<Translation> AllTranslations = new List<Translation>();
            TextCodeRepository txtCodeRep = new TextCodeRepository(tenant);
            TranslationRepository translationRep = new TranslationRepository(tenant);
            TenantRepository tenantRep = new TenantRepository(tenant);
            Tenant tenantPoco = tenantRep.GetSingleByTenant(tenant);
            if (!string.IsNullOrEmpty(tenantPoco.Language) && tenantPoco.Language.ToLower() != "en" && tenantPoco.Language != "english")
            {
                AllTranslations = translationRep.GetTranslationsByLanguageCode(tenantPoco.Language, tenant);

                TenantManagmentPrivateLabelsPM privatelabel = null;
                var url = SecurityUtility.getLoggedDomain();
                if (!url.Contains("system.logitudeworld.com") && !url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
                {
                    TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(tenant);
                    privatelabel = query.GetSingleActivePMByUrl_Cache(url);
                }
                if (privatelabel != null)
                {
                    TextCode textCode = txtCodeRep.GetTextCodeByTenantAndCode("General.MH.Importers", tenant);
                    Translation tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                    if (tra != null)
                    {
                        tra.TranslatedText = privatelabel.PrivateLabelName;
                    }
                    else
                    {
                        tra = new Translation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TextCodeId = textCode.Id,
                            TextCode = textCode,
                            Tenant = tenant,
                            TranslatedText = privatelabel.PrivateLabelName,
                            TranslationHeaderCode = tenantPoco.Language,
                            TextCodeCode = textCode.Code,

                        };

                        AllTranslations.Add(tra);
                    }
                    textCode = txtCodeRep.GetTextCodeByTenantAndCode("General.MH.ActivationWizard", tenant);
                    tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                    if (tra != null)
                    {
                        tra.TranslatedText = privatelabel.PrivateLabelShortName + " Services";
                    }
                    else
                    {
                        tra = new Translation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TextCodeId = textCode.Id,
                            TextCode = textCode,
                            Tenant = tenant,
                            TranslatedText = privatelabel.PrivateLabelShortName + " Services",
                            TranslationHeaderCode = tenantPoco.Language,
                            TextCodeCode = textCode.Code,
                        };

                        AllTranslations.Add(tra);
                    }

                    //var TextCodeId = service.GetTextCodesByTenant(tenant).Where(a => a.Code == "General.MH.Importers").First().Id;
                    //textCodeList.Where(a => a.TextCodeId == TextCodeId).First().DefaultText = privatelabel.PrivateLabelName;
                }
            }
            return AllTranslations;
        }

        [OperationContract]
        [WebGet(UriTemplate = "accountingsettingpm/{id}")]
        public AccountingSettingPM GetAccountingSettingPM(int id)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if (authToken != null)
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(id);
            AccountingSettingQuery accountingSettingQuery = new AccountingSettingQuery(accountingSettingRepository);
            var accountingSettingPM = accountingSettingQuery.GetSinglePM(id);
            return accountingSettingPM;
        }

        [OperationContract]
        [WebGet(UriTemplate = "customsinterfacesettingpm/{InterfaceId}")]
        public CustomsInterfaceSettingPM GetCustomsInterfaceSettingPM(int InterfaceId)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if (authToken != null)
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            CustomsInterfaceSettingRepository repository = new CustomsInterfaceSettingRepository(InterfaceId);
            CustomsInterfaceSettingQuery query = new CustomsInterfaceSettingQuery(repository);
            var setting = query.GetSinglePM(InterfaceId, 0);
            return setting;
        }

        [OperationContract]
        [WebGet(UriTemplate = "shaerdlogisticssettingpm/{settingId}")]
        public SharedLogisticsSettingPM GetSharedLogisticsSettingM(int settingId)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if (authToken != null)
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            SharedLogisticsSettingQueryService query = new SharedLogisticsSettingQueryService(settingId);
            SharedLogisticsSettingPM setting = query.GetSingle(settingId.ToString(), false, false);
            return setting;
        }

        [OperationContract]
        [WebGet(UriTemplate = "getObjectFieldsByObjectTable/{objectTableName}")]
        public List<ObjectFieldPM> GetObjectFieldsByObjectTable(string objectTableName)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            ObjectTableRepository tableRepository = new ObjectTableRepository(authToken.Tenant);
            var tableId = tableRepository.GetObjectTableByName(objectTableName, authToken.Tenant, false).Id;
            ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(authToken.Tenant);
            List<ObjectFieldPM> objectFields = objectFieldQuery.GetObjectFieldsByTenantAndObjectTableId(authToken.Tenant, tableId);
            return objectFields;
        }
    }
}