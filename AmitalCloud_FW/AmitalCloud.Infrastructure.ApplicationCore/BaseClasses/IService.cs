using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AmitalCloud.Infrastructure.Application.BaseClasses
{
    public interface IService<TEntityPOCO, TEntityKeys, TEntityPM, TEntityParentPM, TEntityParentKeys, TEntityList, TkeyType>
        where TEntityPOCO : class, new()
        where TEntityKeys : IEntityKeyFields<TEntityPOCO, TkeyType>, new()
        where TEntityPM : IEntityPM, new()
        where TEntityParentPM : IEntityPM
        where TEntityParentKeys : IEntityKeyFields<TEntityPOCO, TkeyType>
        where TEntityList : class, new()
    {
        bool DontAddTransaction { get; set; }

        void GetAncestor(out object entityPOCO, out object entityPM, out object entityParentPM);
        void GetAncestorEntityUpdateService(out object AncestorEntityUpdateService);
        void GetComposition(TEntityKeys entityKeys, TEntityPM entityPM);
        string GetDebugTrace();
        TEntityPM GetEntityPM(TEntityPOCO entityPOCO, TEntityKeys entityKeys, bool getComposition = false);
        int GetListCount(QueryOperations queryOperations);
        int GetListCount(QueryOperations queryOperations, TreeFilterQueryArgs treeFilterQueryArgs);
        List<TEntityPM> GetMulti(TEntityParentKeys entityParentKeys, bool getFromCache, bool getComposition = true);
        TEntityList GetSingle(IEnumerable<KeyValuePair<string, string>> paramList);
        void InitializeEntityPM(TEntityPM entityPM);
        void InitializeSettings();
        void InitializeUpdateService();
        void Update(TEntityPM entityPM, bool commit, TimeSpan? transactionTimeout = null);
        void UpdateMulti(List<TEntityPM> entityPMList, List<TEntityPM> deletedEntityPMList, TEntityParentPM entityParentPM, bool commit);
    }
}