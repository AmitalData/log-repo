using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class GLAccountPMBuilder
    {
        private GLAccountPM glAccountPM;

        public GLAccountPMBuilder()
        {
            this.Reset();
        }

        public GLAccountPM Build()
        {
            GLAccountPM result = glAccountPM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            glAccountPM = new GLAccountPM();
        }

        public GLAccountPMBuilder AccountTypeCode(string accountTypeCode)
        {
            glAccountPM.AccountTypeCode = accountTypeCode;
            return this;
        }
        public GLAccountPMBuilder IsMultiCurrency(bool isMultiCurrency)
        {
            glAccountPM.IsMultiCurrency = isMultiCurrency;
            return this;
        }
        public GLAccountPMBuilder DisplayNumber(string displayNumber)
        {
            glAccountPM.DisplayNumber = displayNumber;
            return this;
        }

        public GLAccountPMBuilder CurrencyId(string currencyId)
        {
            glAccountPM.CurrencyId = currencyId;
            return this;
        }
        public GLAccountPMBuilder CurrencyCode(string currencyCode)
        {
            glAccountPM.CurrencyCode = currencyCode;
            return this;
        }
        
        public GLAccountPMBuilder LocalName(string localName)
        {
            glAccountPM.LocalName = localName;
            return this;
        }
        public GLAccountPMBuilder EnglishName(string englishName)
        {
            glAccountPM.EnglishName = englishName;
            return this;
        }
        public GLAccountPMBuilder RevenueExpenseType(string revenueExpenseType)
        {
            glAccountPM.RevenueExpenseType = revenueExpenseType;
            return this;
        }
        public GLAccountPMBuilder IsControlAccount(bool isControlAccount)
        {
            glAccountPM.IsControlAccount = isControlAccount;
            return this;
        }
        public GLAccountPMBuilder ControlAccountId(string controlAccountId)
        {
            glAccountPM.ControlAccountId = controlAccountId;
            return this;
        }

        public GLAccountPMBuilder WithModel(GLAccountPM tMEmployeeTime)
        {
            glAccountPM = tMEmployeeTime;
            return this;
        }

        public GLAccountPMBuilder WithDefualtValues()
        {
            glAccountPM = new GLAccountPM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId
            };
            return this;
        }

    }
}
