using Logitude.ReportTests.Models;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.ReportTests.Services.Mapps
{
    public abstract class ReportFilterMapper
    {
        private List<ReportFliterItemEntity> reportFliterItemEntities;
        private readonly EntityService entityService;

        protected ReportFilterMapper()
        {
            entityService = new EntityService();
            reportFliterItemEntities = new List<ReportFliterItemEntity>();
            FillFliterItems();
        }

        protected abstract void FillFliterItems();

        protected void AddFliterItem(ReportFliterItemEntity reportFliterItemEntity)
        {
            if (reportFliterItemEntities == null) reportFliterItemEntities = new List<ReportFliterItemEntity>();
            reportFliterItemEntities.Add(reportFliterItemEntity);
        }

        public ReportFliterItem RenderReportFliterItemValue(ReportFliterItem reportFliterItem)
        {
            if (string.IsNullOrEmpty(reportFliterItem.Map)) return reportFliterItem;

            var reportFliterItemEntity = this.reportFliterItemEntities.FirstOrDefault(x => x.Map == reportFliterItem.Map);
            if (reportFliterItemEntity == null) return reportFliterItem;

            reportFliterItem = MapReportFliterItemFields(reportFliterItemEntity, reportFliterItem);
            reportFliterItem.Value = entityService.GetIdentity(reportFliterItem.SearchKeyValue, reportFliterItem.SearchKeyName, reportFliterItem.EntityName);
            return reportFliterItem;
        }

        private ReportFliterItem MapReportFliterItemFields(ReportFliterItemEntity reportFliterItemEntity, ReportFliterItem reportFliterItem)
        {
            reportFliterItem.SearchKeyName = reportFliterItemEntity.SearchKeyName;
            reportFliterItem.SearchKeyValue = reportFliterItemEntity.SearchKeyValue;
            reportFliterItem.EntityName = reportFliterItemEntity.EntityName;
            return reportFliterItem;
        }

    }
}
