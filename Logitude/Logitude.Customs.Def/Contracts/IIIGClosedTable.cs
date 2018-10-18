using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
namespace Logitude.Customs.Def.Contracts
{

    public interface IIIGClosedTable
    {
        string Code { get; set; }
        string EnglishName { get; set; }
        string LocalName { get; set; }
        string SearchFields { get; set; }
        bool Inactive { get; set; }
        ChangeSetOperation ChangeSetOp { get; set; }
    }
    public interface IIIGClosedTableDummyTenant
    {
        int Tenant { get; set; }
    }
    
    public interface ICanUpdateClosedTable<TEntityPM>
    {
        void Update(TEntityPM entityPM, bool commit);
    }


    public interface ICanGetAllWithAdditionalClosedTable<TEntityPM>
        //where TEntityPM : IIIGClosedTableDummyTenant
    {
        List<TEntityPM> GetAllWithAdditional(int tenant);
    }
    public interface ICanGetAllClosedTable<TEntityPM>
    {
        List<TEntityPM> GetAll();
    }
    public interface IUpdateSingleClosedTable
    {
        void UpdateSingleClosedTable();
        bool ClosedTableHasChanged();

        
    }
}
