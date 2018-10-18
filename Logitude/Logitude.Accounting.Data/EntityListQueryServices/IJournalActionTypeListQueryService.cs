using System;
namespace Logitude.Accounting.Data.EntityListQueryServices
{
   public interface IJournalActionTypeListQueryService
    {
        Logitude.Accounting.Data.EntityLists.JournalActionTypeList GetByCode(string code, int tenant);
        System.Collections.Generic.List<Logitude.Accounting.Data.EntityLists.JournalActionTypeList> GetList(Simplog.Server.Infrastructure.DataContracts.QueryOperations queryOperations, int tenant);
        System.Collections.Generic.List<Logitude.Accounting.Data.EntityLists.JournalActionTypeList> GetList(int tenant);
        int GetListCount(Simplog.Server.Infrastructure.DataContracts.QueryOperations queryOperations, int tenant);
        Logitude.Accounting.Data.EntityLists.JournalActionTypeList GetSingle(string id);
    }
}
