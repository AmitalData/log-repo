using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class INTTRADomainController : ApiController
    {
        public HttpResponseMessage GetINTTRASettings()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);

                    INTTRASettingsHelper myResult = new INTTRASettingsHelper()
                    {
                        Id = tenant,
                    };

                    INTTRASettingRepository settingRepository = new INTTRASettingRepository(context);
                    INTTRASettingQuery settingQuery = new INTTRASettingQuery(settingRepository);
                    myResult.INTTRASetting = settingQuery.GetSinglePM(tenant);

                    if (myResult.INTTRASetting == null)
                    {
                        myResult.INTTRASetting = new INTTRASettingPM()
                        {
                            Tenant = tenant,
                            INTTRASettingModeCode = "TEST",
                        };
                    }
    
                    BranchRepository branchRepository = new BranchRepository(context);
                    BranchQuery branchQuery = new BranchQuery(branchRepository);
                    myResult.Branches = branchQuery.GetBranchPMsByTenant(tenant).ToList();

                    List<ShippingLinePM> ShippingLines = (from d in context.ShippingLines.Include("Card")
                                                          where d.IsINTTRARegistered == true && d.Tenant == tenant
                                                          select new ShippingLinePM()
                                                          {
                                                              Id = d.Id,
                                                              Tenant = d.Tenant,
                                                              Code = d.Card.Code,
                                                              EnglishName = d.Card.EnglishName,
                                                              INTTRARegistrationNotes = d.INTTRARegistrationNotes,
                                                              INTTRAUpdatesShipment = d.INTTRAUpdatesShipment,                                                              
                                                          }).ToList();

                    if (tenant != 0)
                    {
                        List<ShippingLinePM> ShippingLines_0 = (from d in context.ShippingLines.Include("Card")
                                                                where d.IsINTTRARegistered == true && d.Tenant == 0
                                                                select new ShippingLinePM()
                                                                {
                                                                    Id = d.Id,
                                                                    Tenant = d.Tenant,
                                                                    Code = d.Card.Code,
                                                                    EnglishName = d.Card.EnglishName,
                                                                    INTTRARegistrationNotes = d.INTTRARegistrationNotes,
                                                                    INTTRAUpdatesShipment = d.INTTRAUpdatesShipment,
                                                                }).ToList();

                        foreach (ShippingLinePM line in ShippingLines_0)
                        {
                            if (!ShippingLines.Where(d => d.Code == line.Code).Any())
                            {
                                ShippingLines.Add(line);
                            }
                        }
                    }

                    INTTRABranchRegisteredCarrierRepository RegisteredCarrierRepository = new INTTRABranchRegisteredCarrierRepository(context);
                    INTTRABranchRegisteredCarrierQuery RegisteredCarrierQuery = new INTTRABranchRegisteredCarrierQuery(RegisteredCarrierRepository);
                    List<INTTRABranchRegisteredCarrierPM> RegisteredCarriers = RegisteredCarrierQuery.GetAllByTenant(tenant).ToList();

                    myResult.Items = new List<INTTRASettingsHelperItem>();
                    foreach (ShippingLinePM line in ShippingLines)
                    {
                        myResult.Items.Add(new INTTRASettingsHelperItem()
                        {
                            IsLineItem = true,
                            CompinedId = line.Id,
                            Code = line.Code,
                            Name = line.EnglishName,
                            Notes = line.INTTRARegistrationNotes,
                            Tenant = line.Tenant,
                            ShippingLineId = line.Id,
                            
                            UpdatesShipmentsDates = line.INTTRAUpdatesShipment,
                            UpdatesShipmentsDates_Old = line.INTTRAUpdatesShipment,
                        });

                        foreach (BranchPM branch in myResult.Branches)
                        {
                            INTTRASettingsHelperItem item = new INTTRASettingsHelperItem()
                            {
                                CompinedId = line.Id + "," + branch.Id,                                
                                Code = line.Code,
                                Name = line.EnglishName,
                                Notes = line.INTTRARegistrationNotes,
                                Tenant = line.Tenant,
                                ShippingLineId = line.Id,
                                BranchId = branch.Id,
                            };

                            INTTRABranchRegisteredCarrierPM RegisteredCarrier = RegisteredCarriers.Where(d => d.ShippingLineId == line.Id && d.BranchId == branch.Id).FirstOrDefault();
                            if (RegisteredCarrier != null)
                            {
                                item.IsRegistered = true;
                                item.IsRegistered_Old = true;
                                item.RegisteredCarrierId = RegisteredCarrier.Id;
                            }

                            myResult.Items.Add(item);
                        }
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
        public HttpResponseMessage PutINTTRASettings(INTTRASettingsHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;
                    string loggedUserId = this.GetLoggedUserId(loggedUserEmail, tenant);

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    ICommonDataContext iContext = CommonDataContext.GetContext(tenant);

                    #region Setting
                    INTTRASettingRepository settingRepository = new INTTRASettingRepository(iContext);

                    if (args.INTTRASetting != null)
                    {
                        if (args.INTTRASetting.Id == null)
                        {
                            args.INTTRASetting.Id = IdCounter.GetNumber("INTTRASetting", tenant).ToString();

                            INTTRASetting setting = new INTTRASetting()
                            {
                                Id = args.INTTRASetting.Id,
                                Tenant = tenant,
                                InSettingsId = args.INTTRASetting.InSettingsId,
                                OutSettingsId = args.INTTRASetting.OutSettingsId,
                                INTTRASettingModeCode = args.INTTRASetting.INTTRASettingModeCode,
                                INTTRAId = args.INTTRASetting.INTTRAId,
                                INTTRAAlias = args.INTTRASetting.INTTRAAlias,
                            };

                            settingRepository.Add(setting);
                            settingRepository.SubmitChanges();
                        }

                        else
                        {
                            INTTRASetting setting = settingRepository.GetSingleById(args.INTTRASetting.Id);
                            if(setting != null)
                            {
                                setting.InSettingsId = args.INTTRASetting.InSettingsId;
                                setting.OutSettingsId = args.INTTRASetting.OutSettingsId;
                                setting.INTTRASettingModeCode = args.INTTRASetting.INTTRASettingModeCode;
                                setting.INTTRAId = args.INTTRASetting.INTTRAId;
                                setting.INTTRAAlias = args.INTTRASetting.INTTRAAlias;
                            }

                            settingRepository.Update(setting);
                            settingRepository.SubmitChanges();
                        }
                    }
                    #endregion

                    #region Branches
                    BranchRepository branchRepository = new BranchRepository(iContext);
                    List<Branch> AllBranches = branchRepository.GetBranches(tenant).ToList();

                    foreach (BranchPM myBranchPM in args.Branches)
                    {
                        Branch myBranchPOCO = AllBranches.Where(d => d.Id == myBranchPM.Id).FirstOrDefault();
                        if (myBranchPOCO != null)
                        {
                            myBranchPOCO.INTTRAId = myBranchPM.INTTRAId;
                            myBranchPOCO.INTTRAAlias = myBranchPM.INTTRAAlias;
                            myBranchPOCO.INTTRAContactId = myBranchPM.INTTRAContactId;
                            branchRepository.Update(myBranchPOCO);
                        }
                    }

                    branchRepository.SubmitChanges();
                    #endregion

                    #region Carriers

                    CardRepository iCardRepository = new CardRepository(iContext);
                    ShippingLineRepository iShippingLineRepository = new ShippingLineRepository(iContext);
                    INTTRABranchRegisteredCarrierRepository RegisteredCarrierRepository = new INTTRABranchRegisteredCarrierRepository(iContext);
                    List<INTTRABranchRegisteredCarrier> AllRegisteredCarriers = RegisteredCarrierRepository.GetAllByTenant(tenant).ToList();

                    foreach (INTTRASettingsHelperItem itemPM in args.Items.Where(d => d.IsLineItem == true))
                    {
                        if (itemPM.UpdatesShipmentsDates != itemPM.UpdatesShipmentsDates_Old)
                        {
                            string iShippingLineId = null;

                            if (itemPM.Tenant == 0 && tenant != 0)
                            {
                                ShippingLine iShippingLine = iShippingLineRepository.GetSingleShippingLineByCode(itemPM.Code, tenant);
                                if (iShippingLine != null)
                                {
                                    iShippingLine.INTTRAUpdatesShipment = itemPM.UpdatesShipmentsDates;
                                    iShippingLineRepository.Update(iShippingLine);
                                }

                                else
                                {
                                    iShippingLine = iShippingLineRepository.GetSingleShippingLine(itemPM.ShippingLineId, 0);
                                    if (iShippingLine != null)
                                    {
                                        CardQuery iCardQuery = new CardQuery(iCardRepository);
                                        CardList iCardList = iCardQuery.GetCarrierCopyToCurrentTenant(iShippingLine.Id, tenant, null, null, false, null);
                                        if (iCardList != null)
                                        {
                                            iShippingLineId = iCardList.Id;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                iShippingLineId = itemPM.ShippingLineId;
                            }

                            if(iShippingLineId != null)
                            {
                                ShippingLine iShippingLine = iShippingLineRepository.GetSingleShippingLine(iShippingLineId, tenant);
                                if (iShippingLine != null)
                                {
                                    iShippingLine.INTTRAUpdatesShipment = itemPM.UpdatesShipmentsDates;
                                    iShippingLineRepository.Update(iShippingLine);
                                }
                            }
                        }
                    }

                    iContext.SaveChanges();

                    foreach (INTTRASettingsHelperItem itemPM in args.Items.Where(d => d.IsLineItem == false))
                    {
                        if (itemPM.IsRegistered != itemPM.IsRegistered_Old)
                        {
                            INTTRABranchRegisteredCarrier itemPOCO = AllRegisteredCarriers.Where(d => d.ShippingLineId == itemPM.ShippingLineId && d.BranchId == itemPM.BranchId && d.Tenant == tenant).FirstOrDefault();

                            if (itemPM.IsRegistered)
                            {
                                if (itemPOCO == null)
                                {
                                    string iShippingLineId = null;

                                    itemPOCO = new INTTRABranchRegisteredCarrier()
                                    {
                                        Id = IdCounter.GetNumber("INTTRABranchRegisteredCarrier", tenant).ToString(),
                                        Tenant = tenant,
                                        BranchId = itemPM.BranchId,
                                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                        UpdatedByUserId = loggedUserId,
                                    };

                                    if (itemPM.Tenant == 0 && tenant != 0)
                                    {
                                        ShippingLine iShippingLine = iShippingLineRepository.GetSingleShippingLineByCode(itemPM.Code, tenant);
                                        if (iShippingLine != null)
                                        {
                                            iShippingLineId = iShippingLine.Id;
                                            iShippingLine.IsINTTRARegistered = true;
                                            iShippingLine.INTTRARegistrationNotes = itemPM.Notes;
                                            iShippingLineRepository.Update(iShippingLine);
                                        }

                                        else
                                        {
                                            iShippingLine = iShippingLineRepository.GetSingleShippingLine(itemPM.ShippingLineId, 0);
                                            if (iShippingLine != null)
                                            {
                                                CardQuery iCardQuery = new CardQuery(iCardRepository);
                                                CardList iCardList = iCardQuery.GetCarrierCopyToCurrentTenant(iShippingLine.Id, tenant, null, null, false, null);
                                                if (iCardList != null)
                                                {
                                                    iShippingLineId = iCardList.Id;
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        iShippingLineId = itemPM.ShippingLineId;
                                    }

                                    itemPOCO.ShippingLineId = iShippingLineId;

                                    RegisteredCarrierRepository.Add(itemPOCO);
                                }
                            }

                            else
                            {
                                if (itemPOCO != null)
                                {
                                    RegisteredCarrierRepository.Remove(itemPOCO);
                                }
                            }
                        }
                    }

                    iContext.SaveChanges();
                    #endregion

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetINTTRACommunicationSettings()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    INTTRACommunicationSettingsHelper myResult = new INTTRACommunicationSettingsHelper();

                    SettingRepository mySettingRepository = new SettingRepository();
                    Setting mySetting = mySettingRepository.GetSingleSetting("1");
                    if (mySetting != null)
                    {
                        myResult.Id = mySetting.Id;
                        myResult.INTTRAProdFTPHost = mySetting.INTTRAProdFTPHost;
                        myResult.INTTRATestFTPHost = mySetting.INTTRATestFTPHost;
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
        public HttpResponseMessage PutINTTRACommunicationSettings(INTTRACommunicationSettingsHelper args)
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
                        SettingRepository mySettingRepository = new SettingRepository();
                        Setting mySetting = mySettingRepository.GetSingleSetting("1");
                        if (mySetting != null)
                        {
                            mySetting.INTTRAProdFTPHost = args.INTTRAProdFTPHost;
                            mySetting.INTTRATestFTPHost = args.INTTRATestFTPHost;
                            mySettingRepository.Update(mySetting);
                            mySettingRepository.SubmitChanges();
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
    }

    public class INTTRASettingsHelper
    {
        public int Id { get; set; }
        public INTTRASettingPM INTTRASetting { get; set; }
        public List<BranchPM> Branches { get; set; }
        public List<INTTRASettingsHelperItem> Items { get; set; }
    }
    public class INTTRASettingsHelperItem
    {
        public string CompinedId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsLineItem { get; set; }
        public string BranchId { get; set; }
        public string RegisteredCarrierId { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsRegistered_Old { get; set; }
        public int Tenant { get; set; }
        public string ShippingLineId { get; set; }
        public bool UpdatesShipmentsDates { get; set; }
        public bool UpdatesShipmentsDates_Old { get; set; }
    }

    public class INTTRACommunicationSettingsHelper
    {
        public string Id { get; set; }
        public string INTTRAProdFTPHost { get; set; }
        public string INTTRATestFTPHost { get; set; }
    }
}