using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.BIReport
{
    public class BIReportTotalsService
    {
        List<string> ChargesFactMeasurementFields = new List<string>()
       {"Gross Weight Per Ton", "Order Gross Weight", "Order Gross Weight in Ton",
        "Order Volume", "Total Volume (CBM)", "Volumetric Weight", "Number of Packages",
        "Order Number of Packages", "Gross Weight (KG)","Open Payables ( Foreign )","Open Receivable ( Foreign )","Invoice Line Amount (Foreign)","Invoice Exchange Rate","Expected Payable Amount"
        };
        List<string> ARInvoicesFactMeasurementFields = new List<string>()
        {
            "Line Amount (Foreign)","Line VAT Percentage","Invoice Currency Exchange Rate","Regional Tax Percentage","Foreign Exchange Rate"
        };

        private BIReportXMLData bIReportXMLData = new BIReportXMLData();
        private BITabularViewSettings bITabularViewSettings = new BITabularViewSettings();
        private List<string> measurmentColumns = null;
        public List<ExcelTotals> totalsList;
        private int tenant;
        private string factTableName;
        private BIReportQueryService bIReportQueryService;
        private DWObjectFieldQuery dWObjectFieldQuery;
        private List<DWObjectFieldPM> dWObjectMaxMeasurementFieldPMs = new List<DWObjectFieldPM>();


        public BIReportTotalsService(BIReportXMLData bIReportXMLData, int tenant)
        {
            this.bIReportXMLData = bIReportXMLData;
            bITabularViewSettings = bIReportXMLData.BITabularViewSettings;
            this.tenant = tenant;
            totalsList = new List<ExcelTotals>();
            bIReportQueryService = new BIReportQueryService(tenant);
            dWObjectFieldQuery = new DWObjectFieldQuery(tenant);
        }

        public List<string> GetReportMeasurementColumns()
        {
            factTableName = GetFactTableName();
            dWObjectMaxMeasurementFieldPMs = dWObjectFieldQuery.GetDWObjectFieldByDWObjectTableCode(0, factTableName).Where(dwField => dwField.IsMeasurement && dwField.AggregationTypeCode == "MAX").ToList();
            measurmentColumns = bITabularViewSettings.Columns.Where(c => ValidTotalColumn(c)).Select(c => c.Code).ToList();
            return measurmentColumns;
        }

        public bool ValidTotalColumn(Column column)
        {
            if (column == null) return false;
            return (column.DataTypeCode == "Double" || column.DataTypeCode == "Decimal" || column.DataTypeCode == "Integer") && IncludeColumnInTotal(column);
        }

        private bool IncludeColumnInTotal(Column selectedColumn)
        {
            if (factTableName == "Fact_Charges" || factTableName == "Fact_MasterCharges")
            {
                return !ChargesFactMeasurementFields.Any(f => f == selectedColumn.Name);
            }
            else if (factTableName == "Fact_ARInvoices")
            {
                return !ARInvoicesFactMeasurementFields.Any(f => f == selectedColumn.Name) && !dWObjectMaxMeasurementFieldPMs.Any(f => f.Code == selectedColumn.FieldCode);
            }

            return !dWObjectMaxMeasurementFieldPMs.Any(f => f.Code == selectedColumn.FieldCode);
        }
        private string GetFactTableName()
        {
            BIReportPM biReportEntityPM = bIReportQueryService.GetSingle(bIReportXMLData.BIReportId, false, false);
            return biReportEntityPM == null ? "" : biReportEntityPM.FactTableName;
        }

        public List<ExcelTotals> BuildTotalMeasurementColumnList()
        {
            var columnNames = bITabularViewSettings.Columns.OrderBy(a => a.Index).Select(d => d.Code).ToList();
            int columnIndex = 0;

            foreach (var columnName in columnNames)
            {
                AddToTotalMeasurementColumnList(columnIndex, columnName);
                columnIndex++;
            }

            return totalsList;
        }

        private void AddToTotalMeasurementColumnList(int columnIndex, string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return;
            var measurmentColumn = measurmentColumns.Where(c => c == columnName).FirstOrDefault();
            if (string.IsNullOrEmpty(measurmentColumn)) return;
            totalsList.Add(new ExcelTotals(measurmentColumn, 0, columnIndex));
        }
    }
}