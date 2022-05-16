using Logitude.ReportTests.Models;
using System;

namespace Logitude.ReportTests.Services.Mapps.Mappers
{
    public class OpenShipmentsByCustomerReportFilterMapper : ReportFilterMapper
    {
        protected override void FillFliterItems()
        {
            AddFliterItem(new ReportFliterItemEntity("CustomerId", "Card", "Code", "70000"));
        }
    }
}
