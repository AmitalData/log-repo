using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
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
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class GlobalDomainController : ApiController
    {
        public HttpResponseMessage GetMessagingStockTenantsList(int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                IGlobalContext objectContext = GlobalContext.GetContext();

                List<TenantManagementList> myResult = (from d in objectContext.TenantManagements.Include("GlobalTenant")
                                                        where
                                                        (d.IsAWBStockPrepaid || d.IsINTTRAStockPrepaid)
                                                        &&
                                                        (d.GlobalTenant != null && d.GlobalTenant.IsActive)
                                                        select new TenantManagementList()
                                                        {
                                                            Id = d.Id,
                                                            Name = d.Name,
                                                            PackageCode = d.PackageCode,
                                                            IsAWBStockPrepaid = d.IsAWBStockPrepaid,
                                                            IsINTTRAStockPrepaid = d.IsINTTRAStockPrepaid,
                                                        }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCheckTenantMangmnt(string loggedUserId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                IGlobalContext context = new GlobalContext();
                GlobalDomainService globalDomainService = new GlobalDomainService(context);
                TenantUserDataClass myResult = globalDomainService.CheckTenantMangmnt(tenant, loggedUserId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetGlobalSetting()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    JSGlobalSettings myResult = new JSGlobalSettings();

                    SettingRepository mySettingRepository = new SettingRepository();
                    Setting mySetting = mySettingRepository.GetSingleSetting("1");
                    if (mySetting != null)
                    {
                        myResult.Id = mySetting.Id;
                        myResult.LogitudeURL = mySetting.LogitudeURL;
                        myResult.LogoCode = mySetting.LogoCode;
                        myResult.WorkEnvironment = mySetting.WorkEnvironment;
                        myResult.SameUserLoginEnabled = mySetting.SameUserLoginEnabled;
                        myResult.LayoutDirection = mySetting.LayoutDirection;
                        myResult.ReportsRunUsingWR = mySetting.ReportsRunUsingWR;
                        myResult.DocumentFilingEmailDomain = mySetting.DocumentFilingEmailDomain;
                        myResult.DeploymentStage = mySetting.DeploymentStage;

                        if (LogitudeSettings.IsCostomsDeploy)
                        {
                            myResult.ProductInfo = LogitudeSettings.ProductInfo;//.Replace(Environment.NewLine ,"<br>") ;
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception e)
            {
                // the following Code was added by Rabaia in order to fix the Isolation Snapshot problem (Login Stuck on 87%) (Temporarly) 
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticationOnTenant(tenant);

                        JSGlobalSettings myResult = new JSGlobalSettings();

                        SettingRepository mySettingRepository = new SettingRepository();
                        Setting mySetting = mySettingRepository.GetSingleSetting("1");
                        if (mySetting != null)
                        {
                            myResult.Id = mySetting.Id;
                            myResult.LogitudeURL = mySetting.LogitudeURL;
                            myResult.LogoCode = mySetting.LogoCode;
                            myResult.WorkEnvironment = mySetting.WorkEnvironment;
                            myResult.SameUserLoginEnabled = mySetting.SameUserLoginEnabled;
                            myResult.LayoutDirection = mySetting.LayoutDirection;
                            myResult.DocumentFilingEmailDomain = mySetting.DocumentFilingEmailDomain;
                        }

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
        public HttpResponseMessage GetPrivateLableById(string Id)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    //string token = HttpContext.Current.Request.Headers["Token"];
                    //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    //int tenant = authToken.Tenant;

                    //SecurityUtility.AuthenticationOnTenant(tenant);

                    TenantManagmentPrivateLabelsRepository myRepository = new TenantManagmentPrivateLabelsRepository();
                    TenantManagmentPrivateLabels PrivateLable = myRepository.GetSingleTenantManagmentPrivateLabels(Id);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, PrivateLable);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTenantSetting()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    TenantSettingRepository entityRepository = new TenantSettingRepository(tenant);
                    TenantSettingQuery entityQuery = new TenantSettingQuery(entityRepository);
                    List<TenantSettingPM> myResult = entityQuery.GetTenantSettingsByTenant(tenant).ToList();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAccountingSystem(string AccountingSystemCode)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    AccountingSystemPM myResult = null;
                    if (!string.IsNullOrEmpty(AccountingSystemCode) && AccountingSystemCode != "null")
                    {
                        AccountingSystemQuery accountSystems = new AccountingSystemQuery(tenant);
                        myResult = accountSystems.GetSinglePM(AccountingSystemCode);
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllHelpResources()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                IGlobalContext context = new GlobalContext();
                GlobalDomainService globalDomainService = new GlobalDomainService(context);
                var result = globalDomainService.GetAllHelpResources(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineTenantExistsForAirline(string code)
        {
            try
            {
                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenantManagementRepository);

                    TenantManagement entityPOCO = tenantManagementRepository.GetTenantManagementByConnectedArline(code);
                    TenantManagementPM myResult = null;

                    if (entityPOCO != null)
                    {
                        myResult = tenantManagementQuery.GetSinglePM(entityPOCO.Id);
                    }
                    
                    //scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                //}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllBatchServicesDefinitionsPMs(string filterByDateCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                
                List<BatchServicesDefinitionPM> myResult = this.GetAllBatchServices(filterByDateCode);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private List<BatchServicesDefinitionPM> GetAllBatchServices(string dateFilterCode)
        {
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

            BatchServicesDefinitionRepository repository = new BatchServicesDefinitionRepository();
            List<BatchServicesDefinitionPM> TempList = new List<BatchServicesDefinitionPM>();

            IQueryable<BatchServicesDefinitionPM> result = (from a in repository.context.BatchServicesDefinitions
                          select new BatchServicesDefinitionPM()
                          {
                              ClassName = a.ClassName,
                              Code = a.Code,
                              InActive = a.BatchServicesDefinitionMods.InActive,
                              NumberOfThreads = a.BatchServicesDefinitionMods.NumberOfThreads,
                              Parameter1 = a.Parameter1,
                              Parameter2 = a.Parameter2
                          });

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                BatchServicesLogRepository Repo = new BatchServicesLogRepository();

                foreach (BatchServicesDefinitionPM item in result)
                {
                    List<BatchServicesLog> Logs = Repo.GetBatchServicesLogsByCode(item.Code, filterByDate);
                    int DoneItemsInFiveMinutes = 0;
                    int DoneItemsInOneHour = 0;
                    int DoneItemsInOneMinute = 0;
                    int NumberOfDoneItems = 0;
                    DateTime? LActivity = null;

                    foreach (BatchServicesLog Log in Logs)
                    {
                        item.CPU = Log.CPU;
                        DoneItemsInFiveMinutes += Log.DoneItemsInFiveMinutes;
                        DoneItemsInOneHour += Log.DoneItemsInOneHour;
                        DoneItemsInOneMinute += Log.DoneItemsInOneMinute;
                        NumberOfDoneItems += Log.NumberOfDoneItems != null ? (int)Log.NumberOfDoneItems : 0;

                        if (Log.LastActivity > LActivity && LActivity != null)
                        {
                            LActivity = Log.LastActivity;
                        }
                        else if (LActivity == null && Log.LastActivity != null)
                        {
                            LActivity = Log.LastActivity;
                        }
                    }

                    item.DoneItemsInFiveMinutes = DoneItemsInFiveMinutes;
                    item.DoneItemsInOneHour = DoneItemsInOneHour;
                    item.DoneItemsInOneMinute = DoneItemsInOneMinute;
                    item.NumberOfDoneItems = NumberOfDoneItems;
                    item.LastActivity = LActivity;
                    TempList.Add(item);
                }

                scope.Complete();
            }

            return TempList;
        }
        public HttpResponseMessage GetUpdateTenantZeroService(string Message)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                UpdateTenantZeroService myService = new UpdateTenantZeroService();
                myService.SendMessageToQueue(Message);

                bool myResult = true;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetParentTenants()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                IGlobalContext objectContext = GlobalContext.GetContext();
                TenantManagementRepository repository = new TenantManagementRepository(objectContext);
                TenantManagementQuery query = new TenantManagementQuery(repository);

                IQueryable<TenantManagement> parentTenants = repository.GetParentTenantManagements();
                IQueryable<TenantManagementList> result = query.GetIQueryableEntityList(parentTenants);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSettingsDocumentFilingEmailDomain()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SettingRepository mySettingRepository = new SettingRepository();
                Setting mySetting = mySettingRepository.GetSingleSetting("1");
                string myResult = mySetting.DocumentFilingEmailDomain;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTenantManagementJS()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int id = authToken.Tenant;

                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(id);
                TenantManagementPM entityPM = tenantManagementQuery.GetSinglePM(id);

                TenantManagementJS myResult = new TenantManagementJS();

                if (entityPM != null)
                {
                    myResult = new TenantManagementJS()
                    {
                        Id = entityPM.Id,
                        Name = entityPM.Name,
                        PackageCode = entityPM.PackageCode,
                        PackageName = entityPM.PackageName,
                        AWBMessagesCCSTypeCode = entityPM.AWBMessagesCCSTypeCode,
                        IsAWBStockPrepaid = entityPM.IsAWBStockPrepaid,
                        PaidUntilDate = entityPM.PaidUntilDate,
                        TTY = entityPM.TTY,
                        PIMA = entityPM.PIMA,
                        TrialEndDate = entityPM.TrialEndDate,
                        TrialStartDate = entityPM.TrialStartDate,
                        BluesnapAccount = entityPM.BluesnapAccount,
                        BluesnapContractId = entityPM.BluesnapContractId,
                        ChangeHeaderColor = entityPM.ChangeHeaderColor,
                        IsCargonautEnabled = entityPM.IsCargonautEnabled,
                        IsDEXXConnectionEnabled = entityPM.IsDEXXConnectionEnabled,
                        IsEAWBOnlyDemo = entityPM.IsEAWBOnlyDemo,
                        IsINTTRAOnlyDemo = entityPM.IsINTTRAOnlyDemo,
                        IsMultiPackage = entityPM.IsMultiPackage,
                        IsRecurring = entityPM.IsRecurring,
                        IsRestrictedByAirline = entityPM.IsRestrictedByAirline,
                        IsTrial = entityPM.IsTrial,
                        ManageLicencesPerUser = entityPM.ManageLicencesPerUser,
                        ManagesRegisteredAgent = entityPM.ManagesRegisteredAgent,
                        NumberOfUsers = entityPM.NumberOfUsers,
                        PaidDaysLeft = entityPM.PaidDaysLeft,
                        PaymentFailure = entityPM.PaymentFailure,
                        PrivateLabelId = entityPM.PrivateLabelId,
                        SuspendDate = entityPM.SuspendDate,
                        SuspendDaysLeft = entityPM.SuspendDaysLeft,
                        TemporalPackageCode = entityPM.TemporalPackageCode,
                        PackagesCodes_BS = entityPM.PackagesCodes_BS,
                        PackagesCodes_PK = entityPM.PackagesCodes_PK,
                        TrailDaysLeft = entityPM.TrailDaysLeft,
                         TenantManagementLicenses = entityPM.TenantManagementLicenses,
                         CountryName=entityPM.CountryName,
                         BluesnapContractQTY=entityPM.BluesnapContractQTY,
                        BluesnapCRMContractQTY = entityPM.BluesnapCRMContractQTY,
                        BluesnapEAWBContractQTY = entityPM.BluesnapEAWBContractQTY,
                        BluesnapEAWBSContractQTY = entityPM.BluesnapEAWBSContractQTY,
                        BluesnapOneTimeContractQTY = entityPM.BluesnapOneTimeContractQTY,
                        BluesnapCRMContractId = entityPM.BluesnapCRMContractId,
                        BluesnapEAWBContractId = entityPM.BluesnapEAWBContractId,
                        BluesnapEAWBSContractId = entityPM.BluesnapEAWBSContractId,
                        BluesnapOneTimeContract = entityPM.BluesnapOneTimeContract,

                    };
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class JSGlobalSettings
    {
        [Key]
        public string Id { get; set; }
        public string LogitudeURL { get; set; }
        public string LogoCode { get; set; }
        public string WorkEnvironment { get; set; } //customs,main....
        public bool SameUserLoginEnabled { get; set; }
        public string LayoutDirection { get; set; }
        public string ProductInfo { get; internal set; }
        public bool ReportsRunUsingWR { get; set; }
        public string DocumentFilingEmailDomain { get; set; }
        public string DeploymentStage { get; set; }
    }

    public class TenantManagementJS
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PackageCode { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public DateTime? PaidUntilDate { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string AWBMessagesCCSTypeCode { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public string PrivateLabelId { get; set; }
        public bool PaymentFailure { get; set; }
        public DateTime? SuspendDate { get; set; }
        public bool IsTrial { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public bool IsRestrictedByAirline { get; set; }
        public bool IsCargonautEnabled { get; set; }
        public bool IsDEXXConnectionEnabled { get; set; }
        public bool ManageLicencesPerUser { get; set; }
        public bool ChangeHeaderColor { get; set; }
        public int TrailDaysLeft { get; set; }
        public int PaidDaysLeft { get; set; }
        public int SuspendDaysLeft { get; set; }
        public int NumberOfUsers { get; set; }
        public string BluesnapContractId { get; set; }
        public string BluesnapAccount { get; set; }
        public string BluesnapCRMContractId { get; set; }
        public string BluesnapEAWBContractId { get; set; }
        public string BluesnapEAWBSContractId { get; set; }
        public string BluesnapOneTimeContract { get; set; }
        public int BluesnapContractQTY { get; set; }
        public int BluesnapCRMContractQTY { get; set; }
        public int BluesnapEAWBContractQTY { get; set; }
        public int BluesnapEAWBSContractQTY { get; set; }
        public int BluesnapOneTimeContractQTY { get; set; }
        public bool ManagesRegisteredAgent { get; set; }
        public bool IsMultiPackage { get; set; }
        public bool IsINTTRAOnlyDemo { get; set; }
        public string PackageName { get; set; }
        public string TemporalPackageCode { get; set; }
        public string CountryName { get; set; }

        private List<string> packagesCodes_PK;
        public List<string> PackagesCodes_PK
        {
            get
            {
                if (packagesCodes_PK == null)
                {
                    packagesCodes_PK = new List<string>();
                }

                return packagesCodes_PK;
            }

            set
            {
                packagesCodes_PK = value;
            }
        }

        private List<string> packagesCodes_BS;
        public List<string> PackagesCodes_BS
        {
            get
            {
                if (packagesCodes_BS == null)
                {
                    packagesCodes_BS = new List<string>();
                }

                return packagesCodes_BS;
            }

            set
            {
                packagesCodes_BS = value;
            }
        }

        private List<TenantManagementLicensePM> tenantManagementLicenses;
        public virtual List<TenantManagementLicensePM> TenantManagementLicenses
        {
            get
            {
                if (tenantManagementLicenses == null)
                {
                    tenantManagementLicenses = new List<TenantManagementLicensePM>();
                }

                return tenantManagementLicenses;
            }

            set
            {
                tenantManagementLicenses = value;
            }
        }
    }
}