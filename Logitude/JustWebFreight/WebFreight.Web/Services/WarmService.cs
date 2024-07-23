using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebFreight.Web.Controllers.GlobalModel;
using WebFreight.Web.InfrastructureModel;

public class WarmService
{
    private static void RunWithExceptionHandling(string actionName, Action action)
    {
        try
        {
            action.Invoke();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, successed", null, actionName);
        }
        catch (Exception ex)
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "WarmService Failor - action: {0}, Exception Message: {1}", null, actionName, ex.Message);
        }
    }
    public static void MakeWarmCalls(int tenant)
    {
        IWebFreightContext ObjectContext = WebFreightContext.GetContext(tenant);
        ObjectTableRuleQuery objectTableRuleQuery = new ObjectTableRuleQuery(tenant);
        TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
        ObjectTableQuery objectTableQuery = new ObjectTableQuery(objectTabelRepository);
        GeneralDomainService generalDomainService = new GeneralDomainService();
        SystemMetadataLastUpdateController systemMetadataLastUpdateController = new SystemMetadataLastUpdateController();
        TranslationRepository translationRepository = new TranslationRepository(ObjectContext);
        CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
        CustomsSettingQueryService customsSettingQueryService = new CustomsSettingQueryService(tenant);
        TenantRepository tenantRepository = new TenantRepository(tenant);

        var tasks = new List<Action>
        {
            ()=>RunWithExceptionHandling("ObjectTableRuleQuery.GetObjectTableRulePMsByTenant",() => {
                var result = objectTableRuleQuery.GetObjectTableRulePMsByTenant(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}",null,"ObjectTableRuleQuery.GetObjectTableRulePMsByTenant",result.Count);
            }),
            ()=>RunWithExceptionHandling("TenantManagementQuery.GetSinglePM", () => {
                var result = tenantManagementQuery.GetSinglePM(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Id: {1}",null,"TenantManagementQuery.GetSinglePM",result.Id);
            }),
            ()=>RunWithExceptionHandling("GetObjectPMsByTenant", () => {
                var result = objectTableQuery.GetObjectPMsByTenant(tenant).ToList();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}",null,"GetObjectPMsByTenant",result.Count);
            }),
            ()=>RunWithExceptionHandling("GetAllFieldsTranslations", () => {
                var result = generalDomainService.GetAllFieldsTranslations(tenant, "EN", 0, 220000).ToList();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GetAllFieldsTranslations", result.Count);
            }),
            ()=>RunWithExceptionHandling("GetSystemMetadataLastUpdatesCacheHandle", () => {
                var result = systemMetadataLastUpdateController.GetSystemMetadataLastUpdatesCacheHandle(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Id: {1}", null, "GetSystemMetadataLastUpdatesCacheHandle", result.Id);
            }),
            ()=>RunWithExceptionHandling("ObjectTabelRepository.GetObjectTableByName(\"GLAccount\", 0, true)", () => {
                var result = objectTabelRepository.GetObjectTableByName("GLAccount", 0, true);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Id: {1}", null, "ObjectTabelRepository.GetObjectTableByName(\"GLAccount\", 0, true)", result.Id);
            }),
            () => RunWithExceptionHandling("GlobalDbHelper.GetGlobalDB", () => {
                var result = GlobalDbHelper.GetGlobalDB(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Id: {1}", null, "GlobalDbHelper.GetGlobalDB", result.Id);
            }),
            () => RunWithExceptionHandling("GlobalTenantRepository.GetGlobalTenants", () => {
            var result = GlobalTenantRepository.GetGlobalTenants();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GlobalTenantRepository.GetGlobalTenants", result.Count);
            }),
            () => RunWithExceptionHandling("GlobalDBRepository.GetGlobalDBByTenant", () => {
                var result = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Id: {1}", null, "GlobalDBRepository.GetGlobalDBByTenant", result.Id);
            }),
            () => RunWithExceptionHandling("GetSingleTenantByIdAndTenant", () =>
            {
                var result = tenantRepository.GetSingleTenantByIdAndTenant(tenant, true);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result id: {1}", null, "GetSingleTenantByIdAndTenant", result.Id);
            }),
            () => RunWithExceptionHandling("FullAccountingSettingQueryService.Get", () => {
                var result = FullAccountingSettingQueryService.Get(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result id: {1}", null, "FullAccountingSettingQueryService.Get", result.Id);
            }),
            () => RunWithExceptionHandling("GetSingleTenantPM", () => {
                var result = TenantQuery.GetSingleTenantPM(tenant, true);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result id: {1}", null, "GetSingleTenantPM", result.Id);
            }),
             () => RunWithExceptionHandling("GetSingleTenant", () => {
                var result = TenantRepository.GetSingleTenant(tenant, true);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result id: {1}", null, "GetSingleTenant", result.Id);
             }),
            () => RunWithExceptionHandling("GetTranslations", () =>
            {
                var result1 = translationRepository.GetLastTranslationsByTenant(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Id: {1}", null, "GlobalDbHelper.GetLastTranslationsByTenant", result1.Id);
                var result2 = translationRepository.GetTranslationsByTenant(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GlobalDbHelper.GetTranslationsByTenant", result2.Count());
            }),
            () => RunWithExceptionHandling("GetTenantTextCodesWithTenantZero", () => {
                var result = TextCodeRepository.GetTenantTextCodesWithTenantZero(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GetTenantTextCodesWithTenantZero", result.Count);
            }),
            () => RunWithExceptionHandling("GetObjectRuleConditionFieldsByTenant", () => {
                var result = RuleConditionFieldRepository.GetObjectRuleConditionFieldsByTenant(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GetObjectRuleConditionFieldsByTenant", result.Count);
            }),
            () => RunWithExceptionHandling("GetObjectTableRulesByTenant", () => {
                var result = ObjectTableRuleRepository.GetObjectTableRulesByTenant(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GetObjectTableRulesByTenant", result.Count);
            }),
            () => RunWithExceptionHandling("GetObjectTablesWithTenantZero", () => {
                var result = ObjectTableRepository.GetObjectTablesWithTenantZero(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Length: {1}", null, "GetObjectTablesWithTenantZero", result.Count);
            }),
            () => RunWithExceptionHandling("GetTenantRuleFields", () => {
                var result = ObjectTableRuleFieldRepository.GetTenantRuleFields(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result length: {1}", null, "GetTenantRuleFields", result.Count);
            }),
            () => RunWithExceptionHandling("GetCustomPickListsCashe", () =>
            {
                var result = customPickListRepository.GetCustomPickListsCashe(tenant);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result length: {1}", null, "GetCustomPickListsCashe", result.Count);
            }),
            () => RunWithExceptionHandling("sATInterfaceSettingQuery.GetSinglePM(id)",()=>
            {
               SATInterfaceSettingQuery sATInterfaceSettingQuery = new SATInterfaceSettingQuery(tenant);
               var result= sATInterfaceSettingQuery.GetSinglePM(tenant);
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WarmService - action: {0}, result Tenant: {1}", null, "sATInterfaceSettingQuery.GetSinglePM(id)", result.Tenant);

            })
        };

        foreach (var task in tasks)
        {
            task.Invoke();
        }
    }
}
