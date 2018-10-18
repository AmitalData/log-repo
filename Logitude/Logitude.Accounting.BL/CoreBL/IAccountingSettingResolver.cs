using System;
namespace Logitude.Accounting.BL.CoreBL
{
    public interface IAccountingSettingResolver
    {
        string ResolveAccountingCurrencyId(int tenant);
        decimal ResolveVat(int tenant, DateTime documentDate);
        string ResolveVATOutputGLAccountId(int tenant);
    }
}
