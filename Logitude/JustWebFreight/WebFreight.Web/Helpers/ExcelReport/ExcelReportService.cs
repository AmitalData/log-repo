using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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

        public List<DataProviderField> GetDataProviderFields(string reportId, string reportTemplateId, bool isNew)
        {
            Report report = new ReportRepository(tenant).GetSingleReport(reportId, tenant);

            List<DataProviderField> dataProvderFields = new ExcelDataProviderFieldsBuilder().Build(report.Code);
            List<DataProviderField> templateDataProvderFields = GetDataProviderFieldsFromXML(reportTemplateId, isNew);
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

        private List<DataProviderField> GetDataProviderFieldsFromXML(string reportTemplateId, bool isNew)
        {
            if (string.IsNullOrEmpty(reportTemplateId))
                return null;

            byte[] fileData = excelReportFileService.GetReport(isNew, reportTemplateId);
            if (fileData == null)
                return null;

            string xmlString = Encoding.UTF8.GetString(fileData);
            if (xmlString == null)
                return null;
            try
            {
                XDocument doc = XDocument.Parse(xmlString);
                return DeserializeXDocument<DataProviderField>(doc);
            }
            catch (Exception)
            {
                return null;
            }

        }

        private List<T> DeserializeXDocument<T>(XDocument doc)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));

            System.Xml.XmlReader reader = doc.CreateReader();

            List<T> result = (List<T>)serializer.Deserialize(reader);
            reader.Close();

            return result;
        }
    }
}