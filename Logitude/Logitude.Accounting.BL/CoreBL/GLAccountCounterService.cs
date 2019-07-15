using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
//using System.Reflection.Emit;
using System.Text;
using System.Threading;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.CloseTables;
using static Simplog.Server.Infrastructure.DbContextBase;
using System.Transactions;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Server.Tools.Resolvers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.CoreBL
{
    public class GLAccountCounterService
    {
        int tenant;
        private FullAccountingSettingPM fullAccountingSettings;
        public GLAccountCounterService(int _tenant)
        {
            tenant = _tenant;
            fullAccountingSettings = GetFullAccountingSettings();
        }

        public GLAccountCounterPM GetOrInsertCounter(string prefix)
        {
            GLAccountCounterPM gLAccountCounterPM = GetByPrefix(prefix);

            if(gLAccountCounterPM == null)
            {
                gLAccountCounterPM = GetNewCounter(prefix);
                SubmitCounter(gLAccountCounterPM);
            }
            else
            {
                gLAccountCounterPM.ChangeSetOp = ChangeSetOperation.Update;
                gLAccountCounterPM.CurrentNumber++;
                SubmitCounter(gLAccountCounterPM);
            }

            return gLAccountCounterPM;
        }

        public string GetNewDisplayNumber(string prefix)
        {
            GLAccountCounterPM counter = GetOrInsertCounter(prefix);

            string displayNumber = FormatDisplayNumber(prefix, counter);

            return displayNumber;
        }
        public string GetNewDisplayNumber(GLAccountPM gLAccountPM)
        {
            FillGLAccountChartOfAccountCode(gLAccountPM);

            string prefix = gLAccountPM.ChartOfAccountsCode + gLAccountPM.ChartOfAccountsTypeCode;
            string displayNumber = GetNewDisplayNumber(prefix);
            return displayNumber;
        }



        private FullAccountingSettingPM GetFullAccountingSettings()
        {
            FullAccountingSettingQueryService fullAccountingSettingQuery = new FullAccountingSettingQueryService(tenant);
            FullAccountingSettingPM _faSettings = fullAccountingSettingQuery.GetSingleFullAccountingSetting(tenant);
            return _faSettings;
        }
        private void FillGLAccountChartOfAccountCode(GLAccountPM gLAccountPM)
        {
            if (gLAccountPM.ChartOfAccountsCode == null)
            {
                ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(tenant);
                ChartOfAccountPM chartOfAccounts = chartOfAccountQueryService.GetSingle(gLAccountPM.ChartOfAccountsId, false, true);
                if (chartOfAccounts != null)
                {
                    gLAccountPM.ChartOfAccountsCode = chartOfAccounts.Code;
                }
            }
        }

        private string FormatDisplayNumber(string prefix, GLAccountCounterPM counter)
        {
            int prefixLength = prefix.Length;

            CheckCounterLengthField();

            int counterLength = fullAccountingSettings.GLAccounterCounterLength ?? 0;
            string paddingNumber = (counter.CurrentNumber).ToString().PadLeft(counterLength - prefixLength, '0');

            string displayNumber = counter.Prefix + paddingNumber;
            return displayNumber;
        }

        private void CheckCounterLengthField()
        {
            if (fullAccountingSettings.GLAccounterCounterLength == null)
            {
                var showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
                var msg = TextCodesTranslator.TranslateText("GLAccount.O.CounterIsntDefined", tenant, showLocal);

                throw new ApplicationException(msg);
            }
        }

        private GLAccountCounterPM GetNewCounter(string prefix)
        {
            GLAccountCounterPM gLAccountCounterPM = new GLAccountCounterPM()
            {
                Id = IdCounterUtilResolver.GetNewIdCounter("GLAccountCounter", tenant),
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,

                Prefix = prefix,
                StartNumber = 1,
                CurrentNumber = 1,
            };
            return gLAccountCounterPM;
        }
        private GLAccountCounterPM GetByPrefix(string prefix)
        {
            // get counter
            GLAccountCounterQueryService gLAccountCounterQuery = new GLAccountCounterQueryService(tenant);
            GLAccountCounterPM gLAccountCounterPM = gLAccountCounterQuery.GetByPrefix(prefix, tenant);
            return gLAccountCounterPM;
        }
        private void SubmitCounter(GLAccountCounterPM gLAccountCounterPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            GLAccountCounterUpdateService gLAccountCounterUpdateService = new GLAccountCounterUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            gLAccountCounterUpdateService.Update(gLAccountCounterPM, true);
        }
    }
}
