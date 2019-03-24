using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.QuoteModel.DomainServices;
using WebFreight.Web.Security;
using WebFreight.Web.ShipmentsModel.DomainServices;
using WebFreight.Web.SystemLogsModel.EntityList;
using WebFreight.Web.SystemLogsModel.Queries;
using Logitude.SystemLogs;
using Logitude.SystemLogs.Repositories;
using Logitude.SystemLogs.POCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using System.Data.SqlClient;
using System.Data;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.InfrastructureModel.EntityLists;
using System.Threading;
using Logitude.Server.Tools.QueueService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data.Repsitories;
using System.IO;
using WebFreight.Web.App_Code.AngularJS_App_Code.Global;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class InfrastructureDomainController : ApiController
    {
        public HttpResponseMessage GetLastFilters()
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

                    ICRMContext crmContext = CRMContext.GetContext(tenant);

                    CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);

                    List<CRMFilterSettingList> myResult = listService.GetList(tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUpdateLastFilter(string myControlName, string myFilterName, string myFilterValue)
        {
            try
            {
                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{

                if (myFilterValue == "null")
                {
                    myFilterValue = null;
                }

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ICRMContext crmContext = CRMContext.GetContext(tenant);

                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    string loggedUserId = contact.Id;

                    UserRepository myUserRepository = new UserRepository(tenant);
                    User loggedUser = myUserRepository.GetSingleUser(loggedUserId, tenant);

                    if (LogitudeSettings.WorkEnvironment == "customs")
                    {
                        if (loggedUser == null && tenant != 0)
                        {
                            loggedUser = myUserRepository.GetSingleUser(loggedUserId, 0);
                        }
                    }

                    if (loggedUser != null)
                    {
                        CRMFilterSettingRepository myRepository = new CRMFilterSettingRepository(crmContext);
                        CRMFilterSetting filter = myRepository.GetFilterByDetails(tenant, loggedUser.Id, myControlName, myFilterName);

                        if (filter != null)
                        {
                            filter.FilterValue = myFilterValue;

                            if (filter.FilterName == "BusinessUnit")
                            {
                                CRMFilterSetting filter_Owner = myRepository.GetFilterByDetails(tenant, loggedUser.Id, myControlName, "Owner");
                                if (filter_Owner != null)
                                {
                                    filter_Owner.FilterValue = null;
                                }
                            }

                            myRepository.Update(filter);
                            myRepository.SubmitChanges();
                        }

                        else
                        {
                            filter = new CRMFilterSetting()
                            {
                                Id = IdCounter.GetNumber("CRMFilterSetting", tenant),
                                Tenant = tenant,
                                UserId = loggedUser.Id,
                                ControlNameSpace = myControlName,
                                FilterName = myFilterName,
                                FilterValue = myFilterValue
                            };

                            myRepository.Add(filter);
                            myRepository.SubmitChanges();
                        }
                    }
                }

                CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);
                List<CRMFilterSettingList> myResult = listService.GetList(tenant);

                //scope.Complete();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                //}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMainMenuFollowups(string objectTableName)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                string loggedUserId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    loggedUserId = contact.Id;
                }

                if (objectTableName == "Shipment")
                {
                    FilterSerializer serializer = new FilterSerializer();
                    QueryOperations queryOperations = new QueryOperations();
                    queryOperations.SetFilter("FollowUpOwnerUserId", loggedUserId, true, "Equals", null, true);
                    queryOperations.PageSize = 1000;
                    queryOperations.PageIndex = 0;
                    byte[] arrayOfBytes = serializer.SerializeFilterItems(queryOperations);

                    ShipmentsDomainService domain = new ShipmentsDomainService();
                    List<ShipmentList> myResult = domain.GetFollowUpsByShipmentsFilter(arrayOfBytes, tenant).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }

                else if (objectTableName == "Quote")
                {
                    QuotesDomainService domain = new QuotesDomainService();
                    IQueryable<QuoteList> iQueryable = domain.GetShipmentFollowUpsForQuote(tenant);

                    if (!string.IsNullOrEmpty(loggedUserId))
                    {
                        iQueryable = iQueryable.Where(d => d.FollowUpOwnerId == loggedUserId);
                    }

                    List<QuoteList> myResult = iQueryable.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSelectedAndUnselectedRoleFeatures(string RoleId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                List<string> allowedPackages = new List<string>();
                string email = HttpContext.Current.User.Identity.Name;
                ContactInfo inf = SecurityUtility.GetContactInfo(email, tenant);
                if (inf != null)
                {
                    allowedPackages = inf.PackagesCodes;
                }

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                FeatureRepository iFeatureRepository = new FeatureRepository(commonDataContext);

                FeatureQuery featureQuery = new FeatureQuery(iFeatureRepository);
                List<FeaturePM> myResult = featureQuery.GetSelectedAndUnSelectedFeatures(RoleId, allowedPackages, tenant);


                Tenant iTenant = (from d in commonDataContext.Tenants where d.Id == tenant select d).FirstOrDefault();
                List<string> allTextCodesIds = myResult.Where(d => d.NameTextCodeId != null).Select(s => s.NameTextCodeId).ToList();
                List<TextCode> allTextCodes = (from d in webFreightContext.TextCodes where allTextCodesIds.Contains(d.Id) select d).ToList();
                List<Translation> allTranslations = new List<Translation>();

                if (iTenant.Language != null)
                {
                    TranslationHeader iTranslationHeader = (from d in webFreightContext.TranslationHeaders where d.Code == iTenant.Language select d).FirstOrDefault();
                    if (iTranslationHeader != null)
                    {
                        allTranslations = (from d in webFreightContext.Translations
                                           where d.TranslationHeaderCode == iTranslationHeader.Code
                                           && d.Tenant == tenant
                                           && allTextCodesIds.Contains(d.TextCodeId)
                                           select d).ToList();
                    }
                }

                foreach (FeaturePM item in myResult)
                {
                    if (!string.IsNullOrEmpty(item.NameTextCodeId))
                    {
                        TextCode iTextCode = allTextCodes.Where(d => d.Id == item.NameTextCodeId).FirstOrDefault();
                        if (iTextCode != null)
                        {
                            item.TranslatedName = iTextCode.DefaultText;

                            Translation iTranslation = allTranslations.Where(d => d.TextCodeId == item.NameTextCodeId).FirstOrDefault();
                            if (iTranslation != null)
                            {
                                item.TranslatedName = iTranslation.TranslatedText;
                            }
                        }
                    }
                }

                //foreach (FeaturePM item in myResult)
                //{
                //    if (!string.IsNullOrEmpty(item.NameTextCodeCode))
                //    {
                //        item.TranslatedName = TranslateTextsClass.Translate(item.NameTextCodeCode, tenant);
                //    }
                //}

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSelectedAndUnselectedPackageFeatures(string PackageCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                FeatureQuery featureQuery = new FeatureQuery(tenant);
                List<FeaturePM> myResult = featureQuery.GetSelectedAndUnselectedPackagesFeatures(PackageCode, tenant);

                foreach (FeaturePM item in myResult)
                {
                    if (!string.IsNullOrEmpty(item.NameTextCodeCode))
                    {
                        item.TranslatedName = TranslateTextsClass.Translate(item.NameTextCodeCode, tenant);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllowedFeaturesForLoggedUser()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                string loggedUserId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    loggedUserId = contact.Id;
                }

                FeatureQuery featureQuery = new FeatureQuery(tenant);
                LoggedUserFeatures loggedUserFeatures = featureQuery.GetAllowedFeaturesForLoggedUser(loggedUserId, tenant);
                List<FeaturePM> myResult1 = loggedUserFeatures.Features;

                List<FeaturePM> myResult = new List<FeaturePM>();
                List<string> toggleCodes = myResult1.Where(d => !string.IsNullOrEmpty(d.ToggleCode)).Select(s => s.ToggleCode).ToList();

                if(toggleCodes.Count == 0)
                {
                    myResult = myResult1;
                }

                else
                {
                    FeatureToggleRepository featureToggleRepository = new FeatureToggleRepository(0);
                    List<FeatureToggle> featureToggles = featureToggleRepository.GetAllByToggleCodeList(toggleCodes, 0).ToList();

                    foreach (FeaturePM item in myResult1)
                    {
                        if (string.IsNullOrEmpty(item.ToggleCode))
                        {
                            myResult.Add(item);
                        }

                        else
                        {
                            FeatureToggle featureToggle = featureToggles.Where(d => d.TenantNumber == tenant).FirstOrDefault();
                            if (featureToggle != null)
                            {
                                myResult.Add(item);
                            }
                        }
                    }
                }


                //if (tenant == 4)
                //{
                //    string fileContentString = "UserId:" + loggedUserId + ", UserEmail:" + loggedUserEmail;
                //    string allowedPackagesString = LogitudeXmlSerializer.SerializeObjectToXmlString<List<string>>(loggedUserFeatures.AllowedPackagesCodes);
                //    string allFeaturesString = LogitudeXmlSerializer.SerializeObjectToXmlString<List<FeaturePM>>(myResult);

                //    fileContentString += Environment.NewLine + "Allowed Packages" + Environment.NewLine;
                //    fileContentString += Environment.NewLine + allowedPackagesString + Environment.NewLine;
                //    fileContentString += Environment.NewLine + "Allowed Features" + Environment.NewLine;
                //    fileContentString += Environment.NewLine + allFeaturesString;

                //    AzureLog.SaveFileToStorage("ICLFeatures" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", fileContentString, tenant);
                //}

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetNewFeaturesList()
        {
            try
            {
                // Ayman: Only for Tenant 0
                int tenant = 0;

                SecurityUtility.AuthenticationOnTenant(tenant);

                FeatureQuery featureQuery = new FeatureQuery(tenant);
                List<FeatureList> myResult = featureQuery.GetNewFeaturesList(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetPackagesBMs()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                PackageQuery packageQuery = new PackageQuery(tenant);
                List<PackagePM> myResult = packageQuery.GetPackagePMs();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutFeatures(FeaturesUpdateHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (args != null)
                    {
                        if (args.Items.Count > 0)
                        {
                            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                            FeatureService service = new FeatureService(objectContext, tenant);

                            List<FeaturePM> featuresAdded = new List<FeaturePM>();
                            List<FeaturePM> featuresRemoved = new List<FeaturePM>();
                            foreach (FeaturePM item in args.Items)
                            {
                                service.Update(item);

                                if (args.RoleId != null)
                                {
                                    if (service.RoleFeatureAdded)
                                    {
                                        featuresAdded.Add(item);
                                    }

                                    else if (service.RoleFeatureRemoved)
                                    {
                                        featuresRemoved.Add(item);
                                    }
                                }


                                else if (args.PackageCode != null)
                                {
                                    if (item.IsAdded)
                                    {
                                        featuresAdded.Add(item);
                                    }

                                    else if (item.IsRemoved)
                                    {
                                        featuresRemoved.Add(item);
                                    }
                                }
                            }

                            string myFeatureChanges = null;
                            this.BuildFeatureChangesAdded(ref myFeatureChanges, featuresAdded);
                            this.BuildFeatureChangesRemoved(ref myFeatureChanges, featuresRemoved);

                            if (myFeatureChanges != null)
                            {
                                string loggedUserEmail = authToken.Email;
                                string loggedUserId = this.GetLoggedUserId(loggedUserEmail, tenant);

                                FeatureChangeRepository myRepository = new FeatureChangeRepository(objectContext);
                                FeatureChange myFeatureChange = new FeatureChange()
                                {
                                    Tenant = tenant,
                                    Id = IdCounter.GetNumber("FeatureChange", tenant).ToString(),
                                    EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                                    Notes = myFeatureChanges,
                                    UserId = loggedUserId,
                                };

                                if (args.RoleId != null)
                                {
                                    myFeatureChange.RoleId = args.RoleId;
                                    myFeatureChange.Name = "Roles features updated";
                                    myFeatureChange.SearchFields = myFeatureChange.Name;
                                }

                                else
                                {
                                    myFeatureChange.PackageCode = args.PackageCode;
                                    myFeatureChange.Name = "Package features Updated";
                                    myFeatureChange.SearchFields = myFeatureChange.Name;
                                }

                                myRepository.Add(myFeatureChange);
                                myRepository.SubmitChanges();
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
        public HttpResponseMessage GetSendEntityToAirlineTenant(string entityId, string objectTableName, string airlineCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                TenantManagement myAirlineTenantManagement = null;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    IQueryable<TenantManagement> airlineTenants = tenantManagementRepository.GetAirlineTenants();
                    myAirlineTenantManagement = airlineTenants.Where(d => d.TenantConnectedToAirlineCode == airlineCode).FirstOrDefault();
                    scope.Complete();
                }

                if (myAirlineTenantManagement == null)
                {
                    throw new Exception("Airline Tenant not exists");
                }

                else
                {
                    ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                    ParticipantRepository participantRepository = new ParticipantRepository(myCommonContext);

                    IQueryable<Participant> allParticipants = participantRepository.GetParticipants(myAirlineTenantManagement.Id);
                    Participant myParticipant = allParticipants.Where(d => d.ForwarderTenant == tenant).FirstOrDefault();
                    if (myParticipant == null)
                    {
                        throw new Exception("Participant not exists");
                    }

                    else
                    {
                        bool isDirect = myParticipant.IsDirect;
                        AirlineStatisticsRepository airlineStatisticsRepository = new AirlineStatisticsRepository(myCommonContext);

                        if (objectTableName == "Shipment")
                        {
                            #region
                            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                            ShipmentDataView shipment = shipmentRepository.GetSingleShipmentDataView(entityId, tenant);

                            bool isExists = (from a in airlineStatisticsRepository.GetAirlineStatistics(myAirlineTenantManagement.Id) where a.ShipmentId == entityId select a).Any();
                            if (isExists)
                            {
                                #region
                                AirlineStatistics statistics = airlineStatisticsRepository.GetSingleAirlineStatisticsByShipmentId(entityId);

                                statistics.AWBNumber = shipment.Master;
                                statistics.HWBNumber = shipment.House;
                                statistics.AirlineCode = shipment.MainCarriageCarrierCode;
                                statistics.UpdateDate = TenantServerConfigration.GetCurrentDateTime(statistics.Tenant);
                                statistics.EntitiyUpdateDate = shipment.LastUpdateDate;
                                statistics.LastSentDate = shipment.ShipmentLevelCode == "H" ? shipment.FHLStatusDate : shipment.FWBStatusDate;
                                statistics.EntityStatus = shipment.ShipmentStatusName;
                                statistics.NumberOfPackages = shipment.NumberOfPackages;
                                statistics.ChargeableWeight = (decimal)shipment.ChargeableWeight;
                                statistics.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
                                statistics.GrossWeight = (decimal)shipment.GrossWeight;
                                statistics.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
                                statistics.Volume = (decimal)shipment.Volume;
                                statistics.VolumeUnitCode = shipment.VolumeUnitCode;
                                statistics.OriginCode = shipment.MainCarriageFromPortCode;
                                statistics.DestinationCode = shipment.MainCarriageFinalDestinationPortCode;
                                statistics.DescriptionOfGoods = shipment.DescriptionOfGoods;
                                statistics.ShipperName = shipment.ShipperName;
                                statistics.ConsigneeName = shipment.ConsigneeName;
                                statistics.Flight1 = shipment.MainCarriageCarrierCode + shipment.MainCarriageCarrierNumber;
                                statistics.Flight1Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD;
                                statistics.Flight2 = shipment.Transshipment1CarrierCode + shipment.Transshipment1CarrierNumber;
                                statistics.Flight2Date = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD;
                                statistics.Flight3 = shipment.Transshipment2CarrierCode + shipment.Transshipment2CarrierNumber;
                                statistics.Flight3Date = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD;
                                statistics.OnCarriageTo = shipment.OnCarriageToPortCode;
                                statistics.OnCarriageDate = shipment.OnCarriageATD != null ? shipment.OnCarriageATD : shipment.OnCarriageETD;
                                statistics.PreCarriageFrom = shipment.PreCarriageFromPortCode;
                                statistics.PreCarriageDate = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD;
                                statistics.IsCancelled = shipment.IsCancelled;
                                statistics.AirlinePrefix = shipment.AirlinePrefix;
                                statistics.Sender = isDirect ? shipment.LastSentByUserName : null;
                                statistics.Direct = isDirect;
                                statistics.SearchFields = this.BuildSearchFields(statistics);

                                airlineStatisticsRepository.Update(statistics);
                                #endregion
                            }

                            else
                            {
                                #region
                                TenantRepository tenantRepository = new TenantRepository(myCommonContext);
                                Tenant forwarderTenant = tenantRepository.GetSingleTenant(tenant);

                                AirlineStatistics newRecord = new AirlineStatistics();

                                newRecord.Id = IdCounter.GetNumber("AirlineStatistics", myAirlineTenantManagement.Id);
                                newRecord.Tenant = myAirlineTenantManagement.Id;
                                newRecord.SourceTenant = forwarderTenant.Id;
                                newRecord.SourceTenantName = forwarderTenant.Company;
                                newRecord.ShipmentId = shipment.Id;
                                newRecord.ShipmentLevelCode = shipment.ShipmentLevelCode;
                                newRecord.EntityReference = shipment.ShipmentNumber;
                                newRecord.AWBNumber = shipment.Master;
                                newRecord.HWBNumber = shipment.House;
                                newRecord.AirlineCode = shipment.MainCarriageCarrierCode;
                                newRecord.CreateDate = TenantServerConfigration.GetCurrentDateTime(myAirlineTenantManagement.Id);
                                newRecord.UpdateDate = TenantServerConfigration.GetCurrentDateTime(myAirlineTenantManagement.Id);
                                newRecord.EntitiyCreateDate = shipment.CreateDateTime;
                                newRecord.EntitiyUpdateDate = shipment.LastUpdateDate;
                                newRecord.EntityCreatedByUserName = shipment.CreatedByUserName;
                                newRecord.MessageType = shipment.ShipmentLevelCode == "H" ? "FHL" : "FWB";
                                newRecord.LastSentDate = shipment.ShipmentLevelCode == "H" ? shipment.FHLStatusDate : shipment.FWBStatusDate;
                                newRecord.EntityStatus = shipment.ShipmentStatusName;
                                newRecord.NumberOfPackages = shipment.NumberOfPackages;
                                newRecord.ChargeableWeight = shipment.ChargeableWeight == null ? 0 : (decimal)shipment.ChargeableWeight;
                                newRecord.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
                                newRecord.GrossWeight = shipment.GrossWeight == null ? 0 : (decimal)shipment.GrossWeight;
                                newRecord.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
                                newRecord.Volume = shipment.Volume == null ? 0 : (decimal)shipment.Volume;
                                newRecord.VolumeUnitCode = shipment.VolumeUnitCode;
                                newRecord.OriginCode = shipment.MainCarriageFromPortCode;
                                newRecord.DestinationCode = shipment.MainCarriageFinalDestinationPortCode;
                                newRecord.DescriptionOfGoods = shipment.DescriptionOfGoods;
                                newRecord.ShipperName = shipment.ShipperName;
                                newRecord.ConsigneeName = shipment.ConsigneeName;
                                newRecord.Flight1 = shipment.MainCarriageCarrierCode + shipment.MainCarriageCarrierNumber;
                                newRecord.Flight1Date = shipment.MainCarriageATD != null ? shipment.MainCarriageATD : shipment.MainCarriageETD;
                                newRecord.Flight2 = shipment.Transshipment1CarrierCode + shipment.Transshipment1CarrierNumber;
                                newRecord.Flight2Date = shipment.Transshipment1ATD != null ? shipment.Transshipment1ATD : shipment.Transshipment1ETD;
                                newRecord.Flight3 = shipment.Transshipment2CarrierCode + shipment.Transshipment2CarrierNumber;
                                newRecord.Flight3Date = shipment.Transshipment2ATD != null ? shipment.Transshipment2ATD : shipment.Transshipment2ETD;
                                newRecord.OnCarriageTo = shipment.OnCarriageToPortCode;
                                newRecord.OnCarriageDate = shipment.OnCarriageATD != null ? shipment.OnCarriageATD : shipment.OnCarriageETD;
                                newRecord.PreCarriageFrom = shipment.PreCarriageFromPortCode;
                                newRecord.PreCarriageDate = shipment.PreCarriageATD != null ? shipment.PreCarriageATD : shipment.PreCarriageETD;
                                newRecord.IsCancelled = shipment.IsCancelled;
                                newRecord.AirlinePrefix = shipment.AirlinePrefix;
                                newRecord.Sender = isDirect ? shipment.LastSentByUserName : null;
                                newRecord.Direct = isDirect;
                                newRecord.SearchFields = BuildSearchFields(newRecord);

                                airlineStatisticsRepository.Add(newRecord);
                                #endregion
                            }
                            #endregion
                        }

                        else if (objectTableName == "Booking")
                        {
                            #region
                            IBookingContext bookingContext = BookingContext.GetContext(tenant);
                            BookingListQueryService bookingQuery = new BookingListQueryService(bookingContext);
                            BookingList booking = bookingQuery.GetBookingsListByForworderTenant(tenant).Where(d => d.Id == entityId).FirstOrDefault();

                            bool isExists = (from a in airlineStatisticsRepository.GetAirlineStatistics(myAirlineTenantManagement.Id) where a.BookingId == entityId select a).Any();
                            if (isExists)
                            {
                                #region
                                AirlineStatistics statistics = airlineStatisticsRepository.GetSingleAirlineStatisticsByBookingId(entityId);

                                statistics.AWBNumber = booking.Master;
                                statistics.AirlineCode = booking.MainCarriageCarrierCode;
                                statistics.UpdateDate = TenantServerConfigration.GetCurrentDateTime(statistics.Tenant);
                                statistics.EntitiyUpdateDate = booking.UpdateDate.Value;
                                statistics.LastSentDate = booking.FFRStatusDate;
                                statistics.EntityStatus = booking.BookingStatusName;
                                statistics.MessagingStatus = booking.FFRStatusName;
                                statistics.NumberOfPackages = booking.NumberOfPackages;
                                statistics.ChargeableWeight = booking.ChargeableWeight;
                                statistics.ChargeableWeightUnitCode = booking.ChargeableWeightUnitCode;
                                statistics.GrossWeight = booking.GrossWeight;
                                statistics.GrossWeightUnitCode = booking.GrossWeightUnitCode;
                                statistics.Volume = booking.Volume;
                                statistics.VolumeUnitCode = booking.VolumeUnitCode;
                                statistics.OriginCode = booking.MainCarriageFromPortCode;
                                statistics.DestinationCode = booking.MainCarriageFinalDestinationPortCode;
                                statistics.DescriptionOfGoods = booking.DescriptionOfGoods;
                                statistics.ShipperName = booking.ShipperName;
                                statistics.ConsigneeName = booking.ConsigneeName;
                                statistics.Flight1 = booking.MainCarriageCarrierNumber;
                                statistics.Flight1Date = booking.MainCarriageETD;
                                statistics.Flight2 = booking.Transshipment1CarrierNumber;
                                statistics.Flight2Date = booking.Transshipment1ETD;
                                statistics.Flight3 = booking.Transshipment2CarrierNumber;
                                statistics.Flight3Date = booking.Transshipment2ETD;
                                statistics.Allotment = booking.MainCarriageAllotmentIdentification;
                                statistics.IsCancelled = booking.IsCancelled;
                                statistics.AirlinePrefix = booking.AirlinePrefix;
                                statistics.ProductName = booking.BookingProductName;
                                statistics.Sender = isDirect ? booking.LastSentByUserName : null;
                                statistics.Direct = isDirect;
                                statistics.SearchFields = this.BuildSearchFields(statistics);

                                airlineStatisticsRepository.Update(statistics);
                                #endregion
                            }

                            else
                            {
                                #region
                                TenantRepository tenantRepository = new TenantRepository(myCommonContext);
                                Tenant forwarderTenant = tenantRepository.GetSingleTenant(tenant);

                                AirlineStatistics newRecord = new AirlineStatistics()
                                {
                                    Id = IdCounter.GetNumber("AirlineStatistics", myAirlineTenantManagement.Id),
                                    Tenant = myAirlineTenantManagement.Id,
                                    SourceTenant = forwarderTenant.Id,
                                    SourceTenantName = forwarderTenant.Company,
                                    BookingId = booking.Id,
                                    EntityReference = booking.BookingNumber,
                                    AWBNumber = booking.Master,
                                    AirlineCode = booking.MainCarriageCarrierCode,
                                    CreateDate = TenantServerConfigration.GetCurrentDateTime(myAirlineTenantManagement.Id),
                                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(myAirlineTenantManagement.Id),
                                    EntitiyCreateDate = booking.CreateDate.Value,
                                    EntitiyUpdateDate = booking.UpdateDate.Value,
                                    EntityCreatedByUserName = booking.CreatedByUserName,
                                    MessageType = "FFR",
                                    LastSentDate = booking.FFRStatusDate,
                                    EntityStatus = booking.BookingStatusName,
                                    MessagingStatus = booking.FFRStatusName,
                                    NumberOfPackages = booking.NumberOfPackages,
                                    ChargeableWeight = booking.ChargeableWeight,
                                    ChargeableWeightUnitCode = booking.ChargeableWeightUnitCode,
                                    GrossWeight = booking.GrossWeight,
                                    GrossWeightUnitCode = booking.GrossWeightUnitCode,
                                    Volume = booking.Volume,
                                    VolumeUnitCode = booking.VolumeUnitCode,
                                    OriginCode = booking.MainCarriageFromPortCode,
                                    DestinationCode = booking.MainCarriageFinalDestinationPortCode,
                                    DescriptionOfGoods = booking.DescriptionOfGoods,
                                    ShipperName = booking.ShipperName,
                                    ConsigneeName = booking.ConsigneeName,
                                    Flight1 = booking.MainCarriageCarrierNumber,
                                    Flight1Date = booking.MainCarriageETD,
                                    Flight2 = booking.Transshipment1CarrierNumber,
                                    Flight2Date = booking.Transshipment1ETD,
                                    Flight3 = booking.Transshipment2CarrierNumber,
                                    Flight3Date = booking.Transshipment2ETD,
                                    Allotment = booking.MainCarriageAllotmentIdentification,
                                    IsCancelled = booking.IsCancelled,
                                    AirlinePrefix = booking.AirlinePrefix,
                                    ProductName = booking.BookingProductName,
                                    Sender = isDirect ? booking.LastSentByUserName : null,
                                    Direct = isDirect,
                                };

                                newRecord.SearchFields = BuildSearchFields(newRecord);

                                airlineStatisticsRepository.Add(newRecord);
                                #endregion
                            }
                            #endregion
                        }
                    }
                }

                bool myResult = true;

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private string BuildSearchFields(AirlineStatistics entity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entity.SourceTenant.ToString());
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.SourceTenantName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.EntityReference);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.AWBNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.MessageType);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.ShipperName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.ConsigneeName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.EntityCreatedByUserName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            return mySearchFields;
        }

        public HttpResponseMessage GetBusinessHourBM()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                WebFreightDomainService domain = new WebFreightDomainService();
                BusinessHourPM myResult = domain.GetSingleBusinessHourPM(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBatchServicesLogs(string serviceCode, string dateFilterCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                DateTime? filterByDate = null;
                switch (dateFilterCode)
                {
                    case "L1D":
                        {
                            filterByDate = DateTime.UtcNow.AddHours(-24);
                            break;
                        }
                    case "L2D":
                        {
                            filterByDate = DateTime.UtcNow.AddHours(-48);
                            break;
                        }
                    case "L1H":
                        {
                            filterByDate = DateTime.UtcNow.AddHours(-1);
                            break;
                        }
                    case "L2H":
                        {
                            filterByDate = DateTime.UtcNow.AddHours(-2);
                            break;
                        }
                    case "L1Y":
                        {
                            filterByDate = DateTime.UtcNow.AddYears(-1);
                            break;
                        }
                }

                ISystemLogContext systemLogContext = SystemLogContext.GetContext();
                BatchServicesLogRepository batchServicesLogRepository = new BatchServicesLogRepository(systemLogContext);
                BatchServicesLogQuery batchServicesLogQuery = new BatchServicesLogQuery();

                IQueryable<BatchServicesLog> pocosList = batchServicesLogRepository.GetBatchServicesLogsByServiceCode(serviceCode, filterByDate);
                IQueryable<BatchServicesLogList> myResult = batchServicesLogQuery.GetIQueryableEntityList(pocosList);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRoleFeaturesChanges(string RoleId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext myContext = CommonDataContext.GetContext(tenant);

                IQueryable<FeatureChange> iQueryable = (from a in myContext.FeatureChanges
                                                        where a.Tenant == tenant
                                                        && a.RoleId == RoleId
                                                        select a);

                List<FeatureChangeList> myResult = (from d in iQueryable.Include("User").Include("User.Contact")
                                                    select new FeatureChangeList()
                                                    {
                                                        Id = d.Id,
                                                        Tenant = d.Tenant,
                                                        Name = d.Name,
                                                        Notes = d.Notes,
                                                        EventDateTime = d.EventDateTime,
                                                        PackageCode = d.PackageCode,
                                                        RoleId = d.RoleId,
                                                        SearchFields = d.SearchFields,
                                                        UserId = d.UserId,
                                                        UserName = d.User == null ? null : d.User.Contact.EnglishName,
                                                    }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult.OrderByDescending(d => d.EventDateTime));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetPackageFeaturesChanges(string PackageCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext myContext = CommonDataContext.GetContext(tenant);

                IQueryable<FeatureChange> iQueryable = (from a in myContext.FeatureChanges
                                                        where a.Tenant == tenant
                                                        && a.PackageCode == PackageCode
                                                        select a);

                List<FeatureChangeList> myResult = (from d in iQueryable.Include("User").Include("User.Contact")
                                                    select new FeatureChangeList()
                                                    {
                                                        Id = d.Id,
                                                        Tenant = d.Tenant,
                                                        Name = d.Name,
                                                        Notes = d.Notes,
                                                        EventDateTime = d.EventDateTime,
                                                        PackageCode = d.PackageCode,
                                                        RoleId = d.RoleId,
                                                        SearchFields = d.SearchFields,
                                                        UserId = d.UserId,
                                                        UserName = d.User == null ? null : d.User.Contact.EnglishName,
                                                    }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult.OrderByDescending(d => d.EventDateTime));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void BuildFeatureChangesAdded(ref string myFeatureChanges, List<FeaturePM> list)
        {
            string myChange = null;

            foreach (FeaturePM item in list)
            {
                myChange = null;

                switch (item.FeatureTypeCode)
                {
                    case "MODL":
                        {
                            myChange = item.ObjectTableName + " Added: Module";
                            break;
                        }

                    case "READ":
                        {
                            myChange = item.ObjectTableName + " Added: Read";
                            break;
                        }

                    case "NEW":
                        {
                            myChange = item.ObjectTableName + " Added: New";
                            break;
                        }

                    case "EDIT":
                    case "UPDT":
                        {
                            myChange = item.ObjectTableName + " Added: Update";
                            break;
                        }

                    case "AREA":
                        {
                            myChange = item.ObjectTableName + " Added: Area/" + item.TranslatedName;
                            break;
                        }

                    case "ACT":
                        {
                            myChange = item.ObjectTableName + " Added: Action/" + item.TranslatedName;
                            break;
                        }

                    case "QUER":
                        {
                            myChange = item.ObjectTableName + " Added: Query/" + item.TranslatedName;
                            break;
                        }

                    case "MENU":
                        {
                            myChange = "Menu Added: " + item.TranslatedName;
                            break;
                        }

                    case "SET":
                    case "OTH":
                        {
                            myChange = "Settings and Others Added: " + item.TranslatedName;
                            break;
                        }
                }

                this.AddToFeatureChanges(ref myFeatureChanges, myChange);
            }
        }
        private void BuildFeatureChangesRemoved(ref string myFeatureChanges, List<FeaturePM> list)
        {
            string myChange = null;

            foreach (FeaturePM item in list)
            {
                myChange = null;

                switch (item.FeatureTypeCode)
                {
                    case "MODL":
                        {
                            myChange = item.ObjectTableName + " Removed: Module";
                            break;
                        }

                    case "READ":
                        {
                            myChange = item.ObjectTableName + " Removed: Read";
                            break;
                        }

                    case "NEW":
                        {
                            myChange = item.ObjectTableName + " Removed: New";
                            break;
                        }

                    case "EDIT":
                    case "UPDT":
                        {
                            myChange = item.ObjectTableName + " Removed: Update";
                            break;
                        }

                    case "AREA":
                        {
                            myChange = item.ObjectTableName + " Removed: Area/" + item.TranslatedName;
                            break;
                        }

                    case "ACT":
                        {
                            myChange = item.ObjectTableName + " Removed: Action/" + item.TranslatedName;
                            break;
                        }

                    case "QUER":
                        {
                            myChange = item.ObjectTableName + " Removed: Query/" + item.TranslatedName;
                            break;
                        }

                    case "MENU":
                        {
                            myChange = "Menu Removed: " + item.TranslatedName;
                            break;
                        }

                    case "SET":
                    case "OTH":
                        {
                            myChange = "Settings and Others Removed: " + item.TranslatedName;
                            break;
                        }
                }

                this.AddToFeatureChanges(ref myFeatureChanges, myChange);
            }
        }
        public void AddToFeatureChanges(ref string myFeatureChanges, string myChange)
        {
            if (!string.IsNullOrEmpty(myChange))
            {
                myFeatureChanges = string.IsNullOrEmpty(myFeatureChanges) ? myChange : myFeatureChanges + Environment.NewLine + myChange;
            }
        }
        private string GetLoggedUserId(string loggedUserEmail, int tenant)
        {
            string loggedUserId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContactPM = contactQuery.GetContactByNameAndTenant(loggedUserEmail, tenant, true);
            if (loggedContactPM == null)
            {
                loggedContactPM = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            }

            if (loggedContactPM != null)
            {
                loggedUserId = loggedContactPM.Id;
            }

            return loggedUserId;
        }

        public HttpResponseMessage GetDeleteDataForTenant(int entityId, string type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("TenantManagement", "TenantManagement.Action.EraseData", tenant);

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commonContext);
                UserRepository userRepository = new UserRepository(commonContext);

                if (!string.IsNullOrEmpty(authToken.Email))
                {
                    Contact contact = contactRepository.GetSingleContactByEmail(authToken.Email, tenant);
                    if (contact != null)
                    {
                        User user = userRepository.GetSingleUser(contact.Id, tenant);

                        if (user == null)
                        {
                            throw new Exception("Not a user");
                        }
                    }

                    else
                    {
                        throw new Exception("No logged user");
                    }
                }

                else
                {
                    throw new Exception("No logged user");
                }

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                if (string.IsNullOrEmpty(ip) && ip != "82.213.2.230")
                {
                    throw new Exception("Not logged in from company IP");
                }

                EraseTenantDataArgs args = new EraseTenantDataArgs() { Type = type, EntityId = entityId };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(EraseTenantDataArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();

                BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Delete Records",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "WebFreight.Web.Helpers.APIHelpers.EraseTenantDataHelper,WebFreight.Web",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",
                };

                IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });

                return Request.CreateResponse(HttpStatusCode.OK, taskExe);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDataCountForTenant(int entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ShipmentRepository shipmentRepository = new ShipmentRepository(entityId);
                QuoteRepository quoteRepository = new QuoteRepository(entityId);
                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(entityId);
                APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(entityId);
                ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(entityId);
                APPaymentRepository aPPaymentRepository = new APPaymentRepository(entityId);
                CardRepository cardRepository = new CardRepository(entityId);
                TicketRepository ticketRepository = new TicketRepository(entityId);
                ActivityRepository activityRepository = new ActivityRepository(entityId);
                OpportunityRepository opportunityRepository = new OpportunityRepository(entityId);

                IQueryable<Shipment> shipments = shipmentRepository.GetShipments(entityId);
                IQueryable<Quote> quotes = quoteRepository.GetQuotes(entityId);
                IQueryable<ARInvoice> aRInvoices = aRInvoiceRepository.GetARInvoices(entityId);
                IQueryable<APInvoice> aPInvoices = aPInvoiceRepository.GetAPInvoices(entityId);
                IQueryable<ARPayment> aRPayments = aRPaymentRepository.GetARPayments(entityId);
                IQueryable<APPayment> aPPayments = aPPaymentRepository.GetAPPayments(entityId);
                IQueryable<Card> cards = cardRepository.GetCards(entityId).Where(d => d.PartnerTypeId == "CS" || d.PartnerTypeId == "PO");
                IQueryable<Ticket> tickets = ticketRepository.GetAll(entityId);
                IQueryable<Activity> activities = activityRepository.GetAll(entityId);
                IQueryable<Opportunity> opportunities = opportunityRepository.GetAll(entityId);

                BusinessRecordsSummary myResult = new BusinessRecordsSummary();
                myResult.ShipmentsCount = shipments.Count();
                myResult.QuotesCount = quotes.Count();
                myResult.ARInvoicesCount = aRInvoices.Count();
                myResult.APInvoicesCount = aPInvoices.Count();
                myResult.ARPaymentsCount = aRPayments.Count();
                myResult.APPaymentsCount = aPPayments.Count();
                myResult.CustomersCount = cards.Count();
                myResult.TicketsCount = tickets.Count();
                myResult.ActivitiesCount = activities.Count();
                myResult.OpportunitiesCount = opportunities.Count();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetResetCountersForTenant(int entityId, string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityId);
                CounterRepository counterRepository = new CounterRepository(webFreightContext);
                CounterDefinitionRepository counterDefRep = new CounterDefinitionRepository(webFreightContext);
                CounterStatRepository counterStatRep = new CounterStatRepository(webFreightContext);
                CounterLastNumberRepository counterLastNumberRepository = new CounterLastNumberRepository(webFreightContext);

                if (code == "B")
                {
                    #region Shipment
                    Counter counter_SH = counterRepository.GetCounterByCode("SHIP", entityId);
                    if (counter_SH != null)
                    {
                        List<CounterStat> counterStat_SH = counterStatRep.GetCounterCounterStats(counter_SH.Id, entityId);
                        foreach (CounterStat item in counterStat_SH)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region Master
                    Counter counter_MS = counterRepository.GetCounterByCode("MAST", entityId);
                    if (counter_MS != null)
                    {
                        List<CounterStat> counterStat_MS = counterStatRep.GetCounterCounterStats(counter_MS.Id, entityId);
                        foreach (CounterStat item in counterStat_MS)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region HAWB
                    Counter counter_HW = counterRepository.GetCounterByCode("HAWB", entityId);
                    if (counter_HW != null)
                    {
                        List<CounterStat> counterStat_HW = counterStatRep.GetCounterCounterStats(counter_HW.Id, entityId);
                        foreach (CounterStat item in counterStat_HW)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region Quote
                    Counter counter_QT = counterRepository.GetCounterByCode("QUOT", entityId);
                    if (counter_QT != null)
                    {
                        List<CounterStat> counterStat_QT = counterStatRep.GetCounterCounterStats(counter_QT.Id, entityId);
                        foreach (CounterStat item in counterStat_QT)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region Constituent Invoice
                    Counter counter_CI = counterRepository.GetCounterByCode("CNST", entityId);
                    if (counter_CI != null)
                    {
                        List<CounterStat> counterStat_CI = counterStatRep.GetCounterCounterStats(counter_CI.Id, entityId);
                        foreach (CounterStat item in counterStat_CI)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region AR Invoice
                    Counter counter_AI = counterRepository.GetCounterByCode("INVC", entityId);
                    if (counter_AI != null)
                    {
                        List<CounterStat> counterStat_AI = counterStatRep.GetCounterCounterStats(counter_AI.Id, entityId);
                        foreach (CounterStat item in counterStat_AI)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region AP Invoice
                    Counter counter_PI = counterRepository.GetCounterByCode("APIC", entityId);
                    if (counter_PI != null)
                    {
                        List<CounterStat> counterStat_PI = counterStatRep.GetCounterCounterStats(counter_PI.Id, entityId);
                        foreach (CounterStat item in counterStat_PI)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region AR Payment
                    Counter counter_RP = counterRepository.GetCounterByCode("ARPT", entityId);
                    if (counter_RP != null)
                    {
                        List<CounterStat> counterStat_RP = counterStatRep.GetCounterCounterStats(counter_RP.Id, entityId);
                        foreach (CounterStat item in counterStat_RP)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion

                    #region AP Payment
                    Counter counter_PP = counterRepository.GetCounterByCode("APPT", entityId);
                    if (counter_PP != null)
                    {
                        List<CounterStat> counterStat_PP = counterStatRep.GetCounterCounterStats(counter_PP.Id, entityId);
                        foreach (CounterStat item in counterStat_PP)
                        {
                            counterStatRep.Remove(item);
                        }
                    }
                    #endregion
                }

                else if (code == "P")
                {
                    #region Customer                
                    CounterLastNumber customerCounterLastNumber = counterLastNumberRepository.GetSingleByTableName("Customer", entityId);
                    if (customerCounterLastNumber != null)
                    {
                        counterLastNumberRepository.Remove(customerCounterLastNumber);
                    }
                    #endregion
                }

                else if (code == "T")
                {
                    #region Tickets                
                    CounterLastNumber ticketCounterLastNumber = counterLastNumberRepository.GetSingleByTableName("Ticket", entityId);
                    if (ticketCounterLastNumber != null)
                    {
                        counterLastNumberRepository.Remove(ticketCounterLastNumber);
                    }
                    #endregion
                }

                webFreightContext.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        public HttpResponseMessage GetPutTenantSettings(bool DocumentFilingByEmailEnabled)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                TenantSettingRepository tenantSettingRepository = new TenantSettingRepository(tenant);
                List<TenantSetting> tenantSettings = tenantSettingRepository.GetTenantSettingsByTenant(tenant);

                foreach (TenantSetting item in tenantSettings)
                {
                    item.IsDocumentFilingByEmailEnabled = DocumentFilingByEmailEnabled;
                    tenantSettingRepository.Update(item);
                }

                tenantSettingRepository.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllTasksSchedulerPMs(string schedulerType)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                TasksSchedulerRepository tasksSchedulerRepository = new TasksSchedulerRepository(tenant);
                TasksSchedulerQuery tasksSchedulerQuery = new TasksSchedulerQuery(tasksSchedulerRepository);
                List<TasksSchedulerPM> myResult = tasksSchedulerQuery.GetTasksSchedulerPMsBByType(schedulerType, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTaskSchedulerHistory(string taskId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                TaskSchedulerHistoryRepository taskSchedulerHistoryRepository = new TaskSchedulerHistoryRepository(tenant);
                TaskSchedulerHistoryQuery taskSchedulerHistoryQuery = new TaskSchedulerHistoryQuery(taskSchedulerHistoryRepository);

                string loggedUserEmail = authToken.Email;
                string loggedUserId = null;
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    loggedUserId = contact.Id;
                }

                FilterSerializer serializer = new FilterSerializer();
                QueryOperations queryOperations = new QueryOperations();
                queryOperations.SetFilter("FollowUpOwnerUserId", loggedUserId, true, "Equals", null, true);
                queryOperations.SetFilter("TaskId", taskId, false, "Equals", null, false);
                queryOperations.PageSize = 100;
                queryOperations.PageIndex = 0;
                byte[] arrayOfBytes = serializer.SerializeFilterItems(queryOperations);

                WebFreightDomainService service = new WebFreightDomainService();
                IQueryable<TaskSchedulerHistoryList> query2 = service.GetTaskSchedulerHistoryFilters(arrayOfBytes, tenant);

                //IQueryable<TaskSchedulerHistory> iQueryable = taskSchedulerHistoryRepository.GetTaskSchedulerHistory(tenant, taskId);                
                //IQueryable<TaskSchedulerHistoryList> query2 = taskSchedulerHistoryQuery.GetIQueryableEntityList(iQueryable);

                return Request.CreateResponse(HttpStatusCode.OK, query2);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetResendAnalyzeQueue(string AnalyzeQueueId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                AnalyzeQueue analyzeQueue = null;

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    AnalyzeQueueRepository analyzeQueueRepository = new AnalyzeQueueRepository();
                    analyzeQueue = analyzeQueueRepository.GetSingleAnalyzeQueue(AnalyzeQueueId);

                    if (analyzeQueue != null)
                    {
                        analyzeQueue.Retries = 0;
                        analyzeQueue.Status = "W";
                        analyzeQueue.ErrorMessage = null;
                        analyzeQueue.StackTrace = null;
                        analyzeQueue.DoneDate = null;

                        analyzeQueue.AckReason = null;
                        analyzeQueue.AWBNumber = null;

                        analyzeQueue.Tenant = 0;
                        analyzeQueue.ConnectedToEntity = false;
                        analyzeQueue.ConnectedToTenant = false;
                        analyzeQueue.EntityReference = null;
                        analyzeQueue.CommunicationLogId = null;

                        analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;

                        analyzeQueueRepository.Update(analyzeQueue);
                        analyzeQueueRepository.SubmitChanges();
                    }

                    scope.Complete();
                }

                if (analyzeQueue != null)
                {
                    DbQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("ChampAnalyzer", tenant);
                    queueservice.Send(new Dictionary<string, string>() { { "AnalyzeQueueId", analyzeQueue.Id } });
                    queueservice.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetByBIReportId(string Id, string dWQueryId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                BIReportQueryService query = new BIReportQueryService(authToken.Tenant);
                BIReportPM entityPM = query.GetSingle(Id, false, false);
                BIReportXMLData QueryData = new BIReportXMLData();

                DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(authToken.Tenant);
                DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePMByQueryid(dWQueryId, authToken.Tenant);
                DWQueryData DWQueryData = new DWQueryData();
                DWQueryData.PageIndex = 0;
                DWQueryData.PageSize = 0;
                bool isUpdated = false; 
     
                List<DWObjectFieldsDetails> Columns = null;
                if (dWSubQueryPM != null)
                {
                    Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                    var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);
                    DWQueryData.SubQueryData = dWSubQueryPM;
                    DWQueryData.Columns = Columns;
                    DWQueryData.Filters = Filters;
                }
                QueryData.DWQueryData = DWQueryData;


                if (entityPM != null)
                {
                    QueryData.BIReportPM = entityPM;
                    QueryData.BIReportId = entityPM.Id;
                    var sortingList = new List <Column> (); 

                    if (!string.IsNullOrEmpty(entityPM.AGGridOptionsXML))
                    {
                        var bITabularViewSettings = LogitudeXmlSerializer.DeserializeObject<BITabularViewSettings>(entityPM.AGGridOptionsXML);
                        if(bITabularViewSettings!= null && Columns != null)
                        {
                            foreach(Column item in bITabularViewSettings.Columns.ToList())
                            {
                                if (item.SortDirction != null)
                                {
                                    sortingList.Add(item);
                                }
                                
                                var queryColumn = Columns.Where(a => a.DisplayName.Replace("[", "").Replace("]", "") == item.Code).FirstOrDefault();
                                if (queryColumn == null)
                                {
                                    isUpdated = true;
                                    bITabularViewSettings.Columns.RemoveAll(a => a.Code == item.Code);
                                }
                            }
                        }

                        if(sortingList != null && sortingList.Count() > 0)
                        {
                            foreach (Column item in sortingList.OrderBy(o => o.SortOrder).ToList())
                            {
                                QueryData.DWQueryData.ColumnsSort += "["+item.Code +"]"+ " " + item.SortDirction + ",";
                            }
                            QueryData.DWQueryData.ColumnsSort = QueryData.DWQueryData.ColumnsSort.TrimEnd(',');

                        }

                        foreach (var item in Columns)
                        {
                            var queryColumn = bITabularViewSettings.Columns.Where(a => a.Code == item.DisplayName.Replace("[", "").Replace("]", "")).FirstOrDefault();
                            if (queryColumn == null)
                            {
                                isUpdated = true;
                                bITabularViewSettings.Columns.Add(new Column
                                {
                                    Code = item.DisplayName.Replace("[", "").Replace("]", ""),
                                    Name = item.Name,
                                    IsChecked = true,
                                    Width = 150,
                                    DataTypeCode = item.DataTypeCode,
                                    Index = bITabularViewSettings.Columns.Max(a => a.Index) + 1,
                                });
                            }
                        }

                        QueryData.BITabularViewSettings = bITabularViewSettings;
                        if (isUpdated)
                        {
                            var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.BITabularViewSettings);
                            IInfrastructureContext objectContext = InfrastructureContext.GetContext(authToken.Tenant);
                            BIReportRepository repository = new BIReportRepository(objectContext);
                            var entityPOCO = repository.GetSingle(entityPM.Id, entityPM.Tenant);
                            if (entityPM != null)
                            {
                                entityPOCO.AGGridOptionsXML = ColumnsXML;
                                repository.Update(entityPOCO);
                                repository.SubmitChanges();
                            }

                            isUpdated = false;
                        }
                    }

                    else
                    {
                        var bITabularViewSettings = new BITabularViewSettings();
                        bITabularViewSettings.Columns = new List<Column>();
                        foreach (var item in Columns)
                        {
                            bITabularViewSettings.Columns.Add(new Column
                            {
                                Code = item.DisplayName.Replace("[", "").Replace("]", ""),
                                Name = item.Name,
                                IsChecked = true,
                                Width = 150,
                                DataTypeCode = item.DataTypeCode,
                            });
                        }
                        QueryData.BITabularViewSettings = bITabularViewSettings;
                    }
                }
                else
                {
                    var bITabularViewSettings = new BITabularViewSettings();
                    bITabularViewSettings.Columns = new List<Column>();
                    foreach (var item in Columns)
                    {
                        bITabularViewSettings.Columns.Add(new Column
                        {
                            Code = item.DisplayName.Replace("[", "").Replace("]", ""),
                            Name = item.Name,
                            IsChecked = true,
                            Width = 150,
                            DataTypeCode = item.DataTypeCode,
                        });
                    }
                    QueryData.BITabularViewSettings = bITabularViewSettings;

                }
                return Request.CreateResponse(HttpStatusCode.OK, QueryData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutBIReport(BIReportXMLData QueryData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.BITabularViewSettings);

                IInfrastructureContext objectContext = InfrastructureContext.GetContext(authToken.Tenant);
                BIReportRepository repository = new BIReportRepository(objectContext);
                BIReportXMLData QueryData_Updated = new BIReportXMLData();

                var entityPM = QueryData.BIReportPM;
                var entityPOCO = repository.GetSingle(entityPM.Id, entityPM.Tenant);
                if (entityPM != null)
                {
                    entityPM.AGGridOptionsXML = ColumnsXML;
                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    entityPOCO.AGGridOptionsXML = entityPM.AGGridOptionsXML;
                    repository.Update(entityPOCO);
                    repository.SubmitChanges();

                    var bITabularViewSettings = LogitudeXmlSerializer.DeserializeObject<BITabularViewSettings>(entityPM.AGGridOptionsXML);
                    List<DWObjectFieldsDetails> Columns = null;
                    DWSubQueryQuery dWSubQueryQuery = new DWSubQueryQuery(authToken.Tenant);
                    DWSubQueryPM dWSubQueryPM = dWSubQueryQuery.GetSinglePMByQueryid(entityPM.DWQueryId, authToken.Tenant);
                    DWQueryData DWQueryData = new DWQueryData();
                    if (dWSubQueryPM != null)
                    {
                        Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(dWSubQueryPM.ColumnsXML);
                        var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(dWSubQueryPM.FiltersXML);
                        DWQueryData.SubQueryData = dWSubQueryPM;
                        DWQueryData.Columns = Columns;
                        DWQueryData.Filters = Filters;
                    }
                    var sortingList = new List<Column>();
                    foreach (Column item in bITabularViewSettings.Columns.ToList())
                    {
                        if (item.SortDirction != null)
                        {
                            sortingList.Add(item);
                        }

                    }
                    if (sortingList != null && sortingList.Count() > 0)
                    {
                        foreach (Column item in sortingList.OrderBy(o => o.SortOrder).ToList())
                        {
                            DWQueryData.ColumnsSort += "[" + item.Code + "]" + " " + item.SortDirction + ",";
                        }
                        DWQueryData.ColumnsSort = QueryData.DWQueryData.ColumnsSort.TrimEnd(',');
                    }

                    QueryData_Updated.BIReportPM = entityPM;
                    QueryData_Updated.BIReportId = entityPM.Id;
                    QueryData_Updated.DWQueryData = DWQueryData;
                    QueryData_Updated.BITabularViewSettings = bITabularViewSettings;
                }
                return Request.CreateResponse(HttpStatusCode.OK, QueryData_Updated);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDeleteBIReport(string Id )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IInfrastructureContext objectContext = InfrastructureContext.GetContext(authToken.Tenant);
                IWebFreightContext webContext = WebFreightContext.GetContext(authToken.Tenant);
                BIReportRepository repository = new BIReportRepository(objectContext);
                DWQueryRepository dWQueryRepository = new DWQueryRepository(webContext);
                DWSubQueryRepository dWSubQueryRepository = new DWSubQueryRepository(webContext);

                BIReport BIReport = repository.GetSingle(Id, authToken.Tenant);
                if (BIReport != null)
                {
                    var queryId = BIReport.DWQueryId;
                    repository.Remove(BIReport);
                    repository.SubmitChanges();

                    DWSubQuery DWSubQuery = dWSubQueryRepository.GetSingleDWSubQueryByDWQueryId(queryId, authToken.Tenant);
                    DWQuery DWQuery = dWQueryRepository.GetSingleDWQuery(queryId, authToken.Tenant);
                    if(DWQuery != null)
                    {
                        if (DWSubQuery != null)
                        {
                            dWSubQueryRepository.Remove(DWSubQuery);
                        }
                        
                        dWQueryRepository.Remove(DWQuery);
                        dWQueryRepository.SubmitChanges();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDeleteFolder(string Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IInfrastructureContext objectContext = InfrastructureContext.GetContext(authToken.Tenant);
                IWebFreightContext webContext = WebFreightContext.GetContext(authToken.Tenant);
                BIReportFolderRepository repository = new BIReportFolderRepository(objectContext);
            
                BIReportFolder bIReportFolder = repository.GetSingle(Id, authToken.Tenant);
                if (bIReportFolder != null)
                {
                    repository.Remove(bIReportFolder);
                    repository.SubmitChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
       

        public HttpResponseMessage GetFeatureToggles()
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

                    IInfrastructureContext context = InfrastructureContext.GetContext(0);
                    FeatureToggleRepository repository = new FeatureToggleRepository(context);
                    FeatureToggleListQueryService listQueryService = new FeatureToggleListQueryService(context);

                    IQueryable<FeatureToggle> featureToggles = repository.GetAll(0);
                    IQueryable<FeatureToggleList> myResult = listQueryService.GetIqueryableList(featureToggles);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}

public class BusinessRecordsSummary
{
    public int ShipmentsCount { get; set; }
    public int QuotesCount { get; set; }
    public int ARInvoicesCount { get; set; }
    public int APInvoicesCount { get; set; }
    public int ARPaymentsCount { get; set; }
    public int APPaymentsCount { get; set; }
    public int CustomersCount { get; set; }
    public int TicketsCount { get; set; }
    public int ActivitiesCount { get; set; }
    public int OpportunitiesCount { get; set; }

    public BusinessRecordsSummary()
    {
        this.ShipmentsCount = 0;
        this.QuotesCount = 0;
        this.ARInvoicesCount = 0;
        this.APInvoicesCount = 0;
        this.ARPaymentsCount = 0;
        this.APPaymentsCount = 0;
        this.CustomersCount = 0;
        this.TicketsCount = 0;
        this.ActivitiesCount = 0;
        this.OpportunitiesCount = 0;
    }   
}