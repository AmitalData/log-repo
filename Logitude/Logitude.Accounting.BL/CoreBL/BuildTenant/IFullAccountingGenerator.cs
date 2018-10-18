using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant
{
    public interface IDisplayNumberProvider
    {
        string GetDisplayNumber15CHAR(string chartOfAccountsTypeCode, bool isControl, int currentCounter,int tenant);
    }

    public interface IChartOfAccountProvider
    {
        string GetChartOfAccountsId(int tenant, ChartOfAccountsTypeEnum chartOfAccountsTypeCode);
        void CreateCOA(IAccountingContext accountingContext, int tenant);


    }
    
}