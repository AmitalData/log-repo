using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class GlAccountService
    {
        public  GLAccountPM Update(GLAccountPM gLAccount, Table table)
        {
            dynamic gLAccountNewProperty = table.CreateDynamicInstance();
            return new GLAccountPMBuilder().WithModel(gLAccount)
                .IsMultiCurrency((bool)gLAccountNewProperty.IsMultiCurrency)
                .DisplayNumber(DateTime.Now.Ticks.ToString().Substring(3))
                .AccountTypeCode((int)GLAccountTypeEnum.Vendor + "")
                .LocalName((string)gLAccountNewProperty.LocalName)
                .EnglishName((string)gLAccountNewProperty.EnglishName)
                .CurrencyId(null)
                .CurrencyCode(null)
                .RevenueExpenseType((int)RevenueExpenseTypeEnum.Other + "")
                .IsControlAccount((bool)gLAccountNewProperty.IsControlAccount)
                .ControlAccountId(FullAccountingData.GLAccount1Id)
                .Build();


        }

        public void Assert(GLAccountPM gLAccount, GLAccountPM updatedGLAccount)
        {
            updatedGLAccount.Should().NotBeNull();
            updatedGLAccount.IsMultiCurrency.Should().Be(gLAccount.IsMultiCurrency);
            updatedGLAccount.DisplayNumber.Should().Be(gLAccount.DisplayNumber);
            updatedGLAccount.AccountTypeCode.Should().Be(gLAccount.AccountTypeCode);
            updatedGLAccount.LocalName.Should().Be(gLAccount.LocalName);
            updatedGLAccount.EnglishName.Should().Be(gLAccount.EnglishName);
            updatedGLAccount.CurrencyId.Should().Be(gLAccount.CurrencyId);
            updatedGLAccount.RevenueExpenseType.Should().Be(gLAccount.RevenueExpenseType);
            updatedGLAccount.IsControlAccount.Should().Be(gLAccount.IsControlAccount);
            updatedGLAccount.ControlAccountId.Should().Be(gLAccount.ControlAccountId);
        }
    }
}
