using System;
using System.Collections.Generic;
namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public interface ITrailReportBase:IDisposable
    {
        List<TrailReportM> Execute();
        string DbLog { get; }
    }
}
