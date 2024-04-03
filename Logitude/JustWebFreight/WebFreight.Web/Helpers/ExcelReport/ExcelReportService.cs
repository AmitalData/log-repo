using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
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

        public ExcelReportResult GetDataProviderFields(string reportId, string reportTemplateId)
        {
            ExcelReportResult excelReportResult = new ExcelReportResult();
            Report report = new ReportRepository(tenant).GetSingleReport(reportId, tenant);
            excelReportResult.DataProviderFields = new ExcelDataProviderFieldsBuilder().Build(report.Code);
            excelReportResult.DataProviderFields = RemoveSubCollectionFromProvderFields(excelReportResult.DataProviderFields);

            List<DataProviderField> templateDataProvderFields = excelReportFileService.GetDataProviderFieldsFromXML(reportTemplateId, null, false);
            if (templateDataProvderFields == null) return excelReportResult;

            templateDataProvderFields = RemoveSubCollectionFromProvderFields(templateDataProvderFields);
            excelReportResult.SelectedDataProviderFields = templateDataProvderFields;
            ResolveTemplateDifference(excelReportResult.DataProviderFields, templateDataProvderFields);

            return excelReportResult;
        }

        private List<DataProviderField> RemoveSubCollectionFromProvderFields(List<DataProviderField> templateDataProvderFields)
        {
            return templateDataProvderFields
                        .Select(templateDataProvderField =>
                        {
                            return FilterDataProviderFields(templateDataProvderField);
                        }).ToList();
        }

        private static DataProviderField FilterDataProviderFields(DataProviderField templateDataProvderField)
        {
            if (templateDataProvderField.Fields == null || !templateDataProvderField.Fields.Any()) return templateDataProvderField;
            templateDataProvderField.Fields = templateDataProvderField.Fields.Where(field => field.Type != "List" && field.Type != "Class" && (field.Fields == null || field.Fields.Count == 0)).ToList();
            return templateDataProvderField;
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
            if (templateDataProviderField == null) return;

            dataProviderField.IsChecked = true;
            if (dataProviderField.Fields != null && dataProviderField.Fields.Count != 0)
            {
                ResolveTemplateDifference(dataProviderField.Fields, templateDataProviderField.Fields);
            }
        }

    }
}