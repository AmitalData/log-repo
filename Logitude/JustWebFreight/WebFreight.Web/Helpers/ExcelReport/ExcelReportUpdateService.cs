using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.ExcelReport
{
    public class ExcelReportUpdateService
    {
        private readonly int tenant;
        private readonly ExcelReportFileService excelReportFileService;
        public ExcelReportUpdateService(int tenant)
        {
            this.tenant = tenant;
            excelReportFileService = new ExcelReportFileService(tenant);
        }

        public void UpdateReport(ExcelReportArguments excelReportArguments, string userEmail)
        {
            byte[] fileData = GetFieldsData(excelReportArguments);

            if (excelReportArguments.IsNew)
            {
                excelReportFileService.SaveDocumentOnSameFile(excelReportArguments.ReportsTemplateId, fileData);
            }
            else
            {
                excelReportFileService.SaveDocumentOnDifferentFile(excelReportArguments.ReportsTemplateId, fileData, userEmail);
            }
        }

        private byte[] GetFieldsData(ExcelReportArguments excelReportArguments)
        {
            var xmlString = GetXMLFromDataProviderFields(excelReportArguments.DataProviderFields);
            StringBuilder stringbuilder = new StringBuilder();
            Encoding encoding = new UTF8Encoding();
            stringbuilder.AppendLine(xmlString);
            return encoding.GetBytes(stringbuilder.ToString());
        }

        private string GetXMLFromDataProviderFields(List<DataProviderField> dataProviderFields)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DataProviderField>));
            var stringwriter = new System.IO.StringWriter();
            serializer.Serialize(stringwriter, dataProviderFields);
            return stringwriter.ToString();
        }
    }
}