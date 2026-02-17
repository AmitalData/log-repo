using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public CRMFilterSettingPM GetSingleCRMFilterSettingPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            cRMFilterSettingQueryService = new CRMFilterSettingQueryService(crmContext);

            CRMFilterSettingPM entityPM = cRMFilterSettingQueryService.GetSingle(id, false, false);
            return entityPM;
        }

        public CRMFilterSettingList GetSingleCRMFilterSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);

            return listService.GetSingle(id);
        }

        public List<CRMFilterSettingList> GetCRMFilterSettingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);

            return listService.GetList(tenant);
        }

        public List<CRMFilterSettingList> GetCRMFilterSettingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);

            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            return listService.GetList(queryOperations, tenant);
        }

        public int GetCRMFilterSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CRMFilterSettingListQueryService listService = new CRMFilterSettingListQueryService(crmContext);

            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            return listService.GetListCount(queryOperations, tenant);
        }

        [Invoke]
        public void InvokeUpdateCRMSettingsFilter(string myControlName, string myFilterName, string myFilterValue, int tenant, string loggedUserId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            //string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            //ContactRepository contactRep = new ContactRepository(tenant);
            //Contact loggedUser = contactRep.GetSingleContactByEmail(loggedUserEmail, tenant);

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
    }
}