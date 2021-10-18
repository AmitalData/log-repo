using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.ExcelReport
{
    public class ExcelReportService
    {
        private readonly int tenant;
        private readonly ExcelReportFileService excelReportFileService;
        public ExcelReportService(int tenant)
        {
            this.tenant = tenant;
            excelReportFileService = new ExcelReportFileService(tenant);
        }

        public List<DataProviderField> GetDataProviderFields(string reportId, string reportTemplateId)
        {
            Report report = new ReportRepository(tenant).GetSingleReport(reportId, tenant);

            List<DataProviderField> dataProvderFields = new ExcelDataProviderFieldsBuilder().Build(report.Code);
            List<DataProviderField> templateDataProvderFields = excelReportFileService.GetDataProviderFieldsFromXML(reportTemplateId, null, false);
            if (templateDataProvderFields == null)
                return dataProvderFields;

            ResolveTemplateDifference(dataProvderFields, templateDataProvderFields);

            return dataProvderFields;
        }

        private void ResolveTemplateDifference(List<DataProviderField> dataProvderFields, List<DataProviderField> templateDataProvderFields)
        {
            foreach (var dataProviderField in dataProvderFields)
            {
                CheckIfFieldExists(dataProviderField, templateDataProvderFields);
            }
        }

        private void CheckIfFieldExists(DataProviderField dataProviderField, List<DataProviderField> templateDataProvderFields)
        {
            DataProviderField templateDataProviderField = templateDataProvderFields.FirstOrDefault(a => a.Name == dataProviderField.Name);
            if (templateDataProviderField == null)
                return;

            dataProviderField.IsChecked = true;
            if (dataProviderField.Fields != null && dataProviderField.Fields.Count != 0)
            {
                ResolveTemplateDifference(dataProviderField.Fields, templateDataProviderField.Fields);
            }
        }

    }
}