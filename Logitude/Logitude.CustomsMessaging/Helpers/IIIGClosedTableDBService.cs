using System;
namespace Logitude.CustomsMessaging.Helpers
{
    interface IIIGClosedTableDBService
    {
        System.Collections.Generic.List<Logitude.Customs.Def.Contracts.IIIGClosedTable> GetAllDbPM();
        void GetNewUpdateService();
        int GetTransRowMax();
        void Update(Logitude.Customs.Def.Contracts.IIIGClosedTable curDbPM);
    }
}
