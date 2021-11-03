using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using WebFreight.Web.DataProviders;
using WebFreight.Web.ReportsWebServices;

namespace WebFreight.Web.Helpers.Reports
{
    public class ReportManipulationDataService
    {
        private object dataProvider;
        private ReportFliter reportFliter;
        ReportsTemplatesWebService reportsTemplatesWebService;
        ReportsTemplatesVersionRepository reportsTemplatesVersionRepository;
        public ReportManipulationDataService(object dataProvider, ReportFliter reportFliter)
        {
            this.dataProvider = dataProvider;
            this.reportFliter = reportFliter;
            reportsTemplatesWebService = new ReportsTemplatesWebService();
            reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(reportFliter.tenant);
        }

        public bool IsDataProviderHaveListWithValues()
        {
            if (!reportFliter.IsSchedulerReport) return true;
            if (reportFliter.SendIfEmpty) return true;
            List<string> allBusniessListObjectFieldNames = GetallBusniessListObjectFieldNames();
            if (allBusniessListObjectFieldNames.Count() == 0) return false;
            return IsThereAnyBusinessObjectGuidUsedInReportTemplate(allBusniessListObjectFieldNames);
        }

        private List<string> GetallBusniessListObjectFieldNames()
        {
            List<string> allBusniessListObjectFieldNames = new List<string>();
            List<PropertyInfo> properties = dataProvider?.GetType()?.GetProperties()?
                            .Where(prop => prop.GetValue(dataProvider) is IList).ToList();
            foreach (PropertyInfo propInfo in properties)
            {
                AddBusniessObjectFieldNameForListHaveData(allBusniessListObjectFieldNames, propInfo);
            }

            return allBusniessListObjectFieldNames;
        }

        private void AddBusniessObjectFieldNameForListHaveData(List<string> allBusniessListObjectFieldNames, PropertyInfo propInfo)
        {
            object value = propInfo.GetValue(dataProvider, null);
            List<object> genericList = (value as IEnumerable<object>).Cast<object>()?.ToList();
            if (IsListHaveData(genericList))
                allBusniessListObjectFieldNames.Add(propInfo.Name);
        }

        private bool IsListHaveData(List<object> genericList)
        {
            bool listHaveData = false;
            if (genericList != null && genericList.Count() == 1 && typeof(GLAccountBalanceList).IsInstanceOfType(genericList[0]))
                listHaveData = GLAccountBalanceListsHaveData(genericList) ? true : false;
            else if (genericList != null && genericList.Count() > 0)
                listHaveData = true;

            return listHaveData;
        }

        private bool GLAccountBalanceListsHaveData(List<object> genericList)
        {
            GLAccountBalanceList gLAccountBalanceLists = (GLAccountBalanceList)genericList[0];
            string GLAccountBalanceCurrencyId = gLAccountBalanceLists.CurrencyId;
            return !string.IsNullOrEmpty(GLAccountBalanceCurrencyId);
        }

        private bool IsThereAnyBusinessObjectGuidUsedInReportTemplate(List<string> allBusniessListObjectFieldNames)
        {
            bool usedListWithValues = false;
            byte[] template = GetReportTemplateByReportTemplateId();
            string reportTemplateAsXmlString = Encoding.UTF8.GetString(template);
            List<string> allBusniessListObjectFieldGuids = GetallBusniessListObjectFieldGuids(reportTemplateAsXmlString, allBusniessListObjectFieldNames);
            allBusniessListObjectFieldGuids.ForEach(busniessListObjectFieldGuid => {
                if (reportTemplateAsXmlString.IndexOf("<BusinessObjectGuid>" + busniessListObjectFieldGuid + "</BusinessObjectGuid>") > -1) usedListWithValues = true;
            });
            return usedListWithValues;
        }

        private byte[] GetReportTemplateByReportTemplateId()
        {
            string reportDocumentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(reportFliter.DefaultTemplateId, reportFliter.tenant);
            byte[] template = reportsTemplatesWebService.GetReportTemplate(reportDocumentId, reportFliter.tenant, false);
            return template;
        }

        private List<string> GetallBusniessListObjectFieldGuids(string reportTemplateAsXmlString, List<string> allBusniessListObjectFieldNames)
        {
            List<string> allBusniessListObjectFieldguids = new List<string>();
            allBusniessListObjectFieldNames.ForEach(busniessObjectFieldName =>
            {
                allBusniessListObjectFieldguids.Add(GetBusinessObjectFieldGuid(reportTemplateAsXmlString, busniessObjectFieldName));
            });

            return allBusniessListObjectFieldguids;
        }

        private string GetBusinessObjectFieldGuid(string reportTemplateAsXmlString, string busniessObjectFieldName)
        {
            const string openGuidKeyWordWithAngleBrackets = "<Guid>";
            const string closedGuidKeyWordWithAngleBrackets = "</Guid>";
            int startIndexOfbusniessObject = reportTemplateAsXmlString.IndexOf("<" + busniessObjectFieldName);
            int lengthOfbusniessObject = reportTemplateAsXmlString.IndexOf("</" + busniessObjectFieldName + ">") - reportTemplateAsXmlString.IndexOf("<" + busniessObjectFieldName) - busniessObjectFieldName.Length;
            string busniessObject = reportTemplateAsXmlString.Substring(startIndexOfbusniessObject, lengthOfbusniessObject);
            int startIndexOfbusniessObjectGuid = busniessObject.IndexOf(openGuidKeyWordWithAngleBrackets) + openGuidKeyWordWithAngleBrackets.Length;
            int lengthOfbusniessObjectGuid = busniessObject.IndexOf(closedGuidKeyWordWithAngleBrackets) - busniessObject.IndexOf(openGuidKeyWordWithAngleBrackets) - openGuidKeyWordWithAngleBrackets.Length;
            
            return(busniessObject.Substring(startIndexOfbusniessObjectGuid, lengthOfbusniessObjectGuid));
        }
    }
}