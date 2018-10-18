using System;
namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public interface ICurrencyQuery
    {
        System.Linq.IQueryable<Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM> GetCurrenciesByTenantPM(int tenant);
        System.Linq.IQueryable<Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM> GetCurrencyByCodeOrName(string code, string name, int tenant);
        System.Linq.IQueryable<Logitude.BL.CommonDataModel.EntityLists.CurrencyList> GetIQueryableEntityList(System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Currency> iQueryable);
        Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM GetSingleCurrencyByCode(string code, int tenant);
        Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM GetSinglePM(string id, int tenant);
    }
}
