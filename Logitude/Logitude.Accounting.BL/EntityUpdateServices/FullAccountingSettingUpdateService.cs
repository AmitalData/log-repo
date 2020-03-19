using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class FullAccountingSettingUpdateService : EntityUpdateService<FullAccountingSetting, FullAccountingSettingPM, EntityPM>
    {

        protected override void OnCreating(FullAccountingSettingPM entityPM, EntityPM entityParentPM)
        {
            TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
            Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
            tenant.PaymentTermId = entityPM.TenantPaymentTermId;
            tenant.AccountingActivationDate = entityPM.AccountingActivationDate;
            tenant.AccountingActivated = entityPM.AccountingActivated;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }

        protected override void OnUpdating(FullAccountingSettingPM entityPM, FullAccountingSetting entityPOCO)
        {


            if (entityPM.ChangeSetOp == ChangeSetOperation.Update 
                && entityPM.GLAccounterCounterLength != entityPOCO.GLAccounterCounterLength
                && entityPOCO.GLAccounterCounterLength != null)
            {
                bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(EntityPM.Tenant);
                string msg = TextCodesTranslator.TranslateText("FullAccountingSetting.O.CantChangeCounterLength", entityPM.Tenant, showLocal);
                throw new ApplicationException(msg);
            }

            TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
            Tenant tenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
            tenant.PaymentTermId = entityPM.TenantPaymentTermId;
            tenant.AccountingActivationDate = entityPM.AccountingActivationDate;
            tenant.AccountingActivated = entityPM.AccountingActivated;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();

            string key = "FullAccountingSettingPM," + tenant.ToString();
            CacheManager.CacheWrapper.Invalidate(key);
        }

        

        protected override void Validate(FullAccountingSettingPM entityPM)
        {

            //validate GLAccounterCounterLength
            if (entityPM.GLAccounterCounterLength != null && (entityPM.GLAccounterCounterLength < 8 || entityPM.GLAccounterCounterLength > 15))
            {
                bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
                string msg = TextCodesTranslator.TranslateText("FullAccountingSetting.O.CounterLengthBetween8n15", entityPM.Tenant, useLocal);
                throw new ApplicationException(msg);
            }

            if (entityPM.NumberOfAgingMonths != null && (entityPM.NumberOfAgingMonths < 1 || entityPM.NumberOfAgingMonths > 9))
            {
                bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
                string msg = TextCodesTranslator.TranslateText("FullAccountingSetting.O.NoOfAgingMonthsBW1n9", entityPM.Tenant, useLocal);
                throw new ApplicationException(msg);
            }



        }


        //protected override void Trace(FullAccountingSettingPM entityPM, FullAccountingSetting entityPOCO, string changesXml)
        //{
        //    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
        //    {
        //        //create trace event with created type.
        //    }
        //    else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
        //    {


        //    }
        //    base.Trace(entityPM, entityPOCO, changesXml);
        //}



        //private ContactPM LoggedContact(int tenant)
        //{
        //    ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant);
        //    if (loggedContact == null)
        //    {
        //        loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
        //    }
        //    return loggedContact;
        //}

    }
}
