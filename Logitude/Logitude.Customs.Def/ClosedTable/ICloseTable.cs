using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Customs.Def.ClosedTable
{
    public  interface  ICloseTable<TPOCO,TDetails>
    {
        string Code { get; set; }
        string GetSearchFields(TPOCO rec);
        List<TDetails> GetAll();
        void MapPoco(TPOCO poco);
    }
    public interface ICustomCloseTable
    {
        string Code { get; set; }
    }
    public interface IRepoCloseTable<TPOCO>
    {
        List<TPOCO> GetAll();
        TPOCO GetSingle(string code);
    }
}
