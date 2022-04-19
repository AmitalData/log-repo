using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers.ExcelReport
{
    public class ExcelReportMrtBuilder
    {

        private readonly int tenant;
        private readonly ExcelReportFileService excelReportFileService;
        private readonly ExcelDataProviderFieldsBuilder excelDataProviderFieldsBuilder;
        private StiReport stiReport;
        private Report report;
        private int columnWidth = 2;
        private double positionX;
        private double positionY;
        public ExcelReportMrtBuilder(int tenant)
        {
            this.tenant = tenant;
            this.excelReportFileService = new ExcelReportFileService(tenant);
            this.excelDataProviderFieldsBuilder = new ExcelDataProviderFieldsBuilder();
        }

        public byte[] Build(ReportsTemplate reportsTemplate, string documentId, bool isDocument)
        {
            List<DataProviderField> selectedFields = excelReportFileService.GetDataProviderFieldsFromXML(reportsTemplate.Id, documentId, isDocument);
            selectedFields = ReOrderDataProviderFields(selectedFields);
            report = GetReport(reportsTemplate.ReportId);

            stiReport = new StiReport
            {
                ScriptLanguage = StiReportLanguageType.CSharp
            };

            BuildDataProvider();

            if (selectedFields != null && selectedFields.Count > 0)
            {
                BuildReport(selectedFields);
            }

            return stiReport.SaveToByteArray();
        }

        private List<DataProviderField> ReOrderDataProviderFields(List<DataProviderField> selectedFields)
        {
            if (selectedFields.Any(x => x.Sort == 0)) return selectedFields;
            return selectedFields.OrderBy(x => x.Sort).ToList();
        }

        private void BuildDataProvider()
        {

            Type reportType = excelDataProviderFieldsBuilder.GetDataProviderType(report.Code);
            List<StiBusinessObjectData> businessObjects = new List<StiBusinessObjectData>
            {
                new StiBusinessObjectData(report.Name,
                reportType.Name,
                reportType.Name,
                reportType)
            };
            stiReport.RegBusinessObject(businessObjects);
            stiReport.Dictionary.SynchronizeBusinessObjects(businessObjects.Count());
        }

        private Report GetReport(string reportId)
        {
            ReportRepository reportsTemplateRepository = new ReportRepository(tenant);
            return reportsTemplateRepository.GetSingleReport(reportId, tenant);
        }

        private void BuildReport(List<DataProviderField> selectedFields)
        {
            foreach (StiPage page in stiReport.Pages)
            {
                AddExcelReportLists(selectedFields, page);
                AddHeader(selectedFields, report.Name, page);
                AddFooter(page);
            }
        }

        private StiText CreateVariableText(DataProviderField variable)
        {
            StiText stiText = new StiText(new RectangleD(positionX, positionY, columnWidth, 0.5))
            {
                HorAlignment = StiTextHorAlignment.Center,
                Name = GetNameFromExpression(variable.Expression) + "Text",
                Text = variable.Expression
            };
            stiText.HorAlignment = StiTextHorAlignment.Center;
            stiText.VertAlignment = StiVertAlignment.Center;
            stiText.Font = new Font("Arial", 8, FontStyle.Regular);
            stiText.TextBrush = new StiSolidBrush(Color.Black);
            stiText.WordWrap = true;
            positionX += columnWidth;
            return stiText;
        }

        private string GetNameFromExpression(string expression)
        {
            return expression.Replace("{", string.Empty).Replace("}", string.Empty).Replace(".", string.Empty);
        }

        private StiText CreateVariableHeader(DataProviderField variable)
        {
            StiText stiText = new StiText(new RectangleD(positionX, positionY, columnWidth, 0.5))
            {
                HorAlignment = StiTextHorAlignment.Center,
                Name = GetNameFromExpression(variable.Expression) + "TextLabel",
                Text = variable.Text + " : ",
            };
            stiText.HorAlignment = StiTextHorAlignment.Center;
            stiText.VertAlignment = StiVertAlignment.Center;
            stiText.Font = new Font("Arial", 8, FontStyle.Bold);
            stiText.TextBrush = new StiSolidBrush(Color.Black);
            stiText.WordWrap = true;
            positionX += columnWidth;
            return stiText;
        }

        private void AddHeader(List<DataProviderField> dataProviderFields, string text, StiPage page)
        {
            var variables = dataProviderFields.Where(x => x.Type != "List" && x.Type != "Class").ToList();

            StiPageHeaderBand stiPageHeaderBand = CreateStiPageHeaderBand(variables.Count());
            StiText stiText = CreateHeaderText(text, page);

            page.Components.Add(stiPageHeaderBand);
            stiPageHeaderBand.Components.Add(stiText);

            AddVariablesToHeader(variables, stiPageHeaderBand);
        }

        private StiText CreateHeaderText(string text, StiPage page)
        {
            return new StiText(new RectangleD(0, 0, page.Width, page.Height / 20), text)
            {
                HorAlignment = StiTextHorAlignment.Center,
                Font = new Font("Arial", 22.5f),
                Name = "PageHeaderText",
            };
        }

        private StiPageHeaderBand CreateStiPageHeaderBand(int variablesCount)
        {
            StiPageHeaderBand stiBand = new StiPageHeaderBand
            {
                Height = ((double)variablesCount / 4) + 0.7,
                Name = "PageHeaderBand",
            };
            return stiBand;
        }

        private void AddVariablesToHeader(List<DataProviderField> variables, StiPageHeaderBand stiPageHeaderBand)
        {
            SetDefaultVariablesPositions();
            foreach (var variable in variables)
            {
                AddVariableToHeader(stiPageHeaderBand, variable);
            }
        }

        private void AddVariableToHeader(StiPageHeaderBand stiPageHeaderBand, DataProviderField variable)
        {
            if (positionX != 0 && positionX % 8 == 0)
            {
                positionX = 0;
                positionY += 0.5;
            }
            stiPageHeaderBand.Components.Add(CreateVariableHeader(variable));
            stiPageHeaderBand.Components.Add(CreateVariableText(variable));

        }

        private void SetDefaultVariablesPositions()
        {
            positionX = 0;
            positionY = 0.5;
        }

        private void AddFooter(StiPage page)
        {
            StiPageFooterBand pageFooterBand = new StiPageFooterBand
            {
                ClientRectangle = new RectangleD(0, 10.71, page.Width, 0.2),
                Name = "PageFooterBand",
                Border = new StiBorder(StiBorderSides.None, Color.Black, 1, StiPenStyle.Solid, false, 4, new StiSolidBrush(Color.Black)),
                Brush = new StiSolidBrush(Color.Transparent)
            };
            page.Components.Add(pageFooterBand);

            StiSystemText systemText = new StiSystemText
            {
                ClientRectangle = new RectangleD(0, 0, page.Width, 0.2),
                HorAlignment = StiTextHorAlignment.Right,
                Name = "PageFooterText",
                Border = new StiBorder(StiBorderSides.Top, Color.DimGray, 0.5, StiPenStyle.Solid, false, 4, new StiSolidBrush(Color.Black)),
                Brush = new StiSolidBrush(Color.Transparent),
                Font = new Font("Arial Unicode MS", 8F),
                TextBrush = new StiSolidBrush(Color.Black),
                TextFormat = new StiGeneralFormatService(),
                TextOptions = new StiTextOptions(false, false, false, 0F, System.Drawing.Text.HotkeyPrefix.None, StringTrimming.None),
                Text = "{PageNofM}"
            };
            pageFooterBand.Components.Add(systemText);
        }

        private void AddExcelReportLists(List<DataProviderField> selectedFields, StiPage page)
        {
            int maxWidth = 0;
            int minWidth = 8;
            foreach (var item in selectedFields.Where(x => x.Type == "List").ToList())
            {
                int listWidth = AddList(item, page);
                maxWidth = maxWidth > listWidth ? maxWidth : listWidth;
            }
            page.Width = maxWidth > minWidth ? maxWidth : minWidth;

        }

        private int AddList(DataProviderField dataProviderField, StiPage page)
        {
            int totalWidth;
            StiHeaderBand headerBand = new StiHeaderBand
            {
                Height = 0.3,
                Name = GetNameFromExpression(dataProviderField.Expression) + "HeaderBand",
                PrintIfEmpty = true,
                PrintOnAllPages = true
            };
            page.Components.Add(headerBand);


            StiDataBand dataBand = new StiDataBand
            {
                BusinessObjectGuid = GetBusinessObjectGuid(dataProviderField),
                Height = 0.3,
                Name = GetNameFromExpression(dataProviderField.Expression) + "DataBand",
            };
            page.Components.Add(dataBand);


            double pos = 0;
            totalWidth = columnWidth * dataProviderField.Fields.Count();
            dataProviderField.Fields = ReOrderDataProviderFields(dataProviderField.Fields);
            foreach (var item in dataProviderField.Fields.Where(x => x.Type != "List" && x.Type != "Class").ToList())
            {
                SetTableHeaderText(headerBand, columnWidth, pos, item);
                SetTableColumns(dataBand, columnWidth, pos, item);
                pos += columnWidth;
            }

            return totalWidth;
        }

        private string GetBusinessObjectGuid(DataProviderField dataProviderField)
        {
            string[] businessObjects = dataProviderField.Expression.Replace("{", string.Empty).Replace("}", string.Empty).Split('.');
            StiBusinessObject stiBusinessObject = null;
            foreach (var item in businessObjects)
            {
                stiBusinessObject = stiBusinessObject == null ? stiReport.Dictionary.BusinessObjects[item] : stiBusinessObject.BusinessObjects[item];
            }
            return stiBusinessObject?.Guid;
        }

        private void SetTableColumns(StiDataBand dataBand, int columnWidth, double pos, DataProviderField item)
        {
            StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5))
            {
                HorAlignment = StiTextHorAlignment.Center,
                Name = GetNameFromExpression(item.Expression) + "Text",
                Text = item.Expression,
                Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid)
            };
            hText.HorAlignment = StiTextHorAlignment.Center;
            hText.VertAlignment = StiVertAlignment.Center;
            hText.Font = new Font("Arial", 8, FontStyle.Regular);
            hText.TextBrush = new StiSolidBrush(Color.Black);
            hText.WordWrap = true;
            dataBand.Components.Add(hText);
        }

        private void SetTableHeaderText(StiHeaderBand headerBand, int columnWidth, double pos, DataProviderField item)
        {
            StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.5))
            {
                HorAlignment = StiTextHorAlignment.Center,
                Name = GetNameFromExpression(item.Expression) + "HeaderLabel",
                Text = item.Text,
                Brush = new StiSolidBrush(Color.RoyalBlue),
                Border = new StiBorder(StiBorderSides.All, Color.FromArgb(89, 89, 89), 1, StiPenStyle.Solid)
            };
            hText.HorAlignment = StiTextHorAlignment.Center;
            hText.VertAlignment = StiVertAlignment.Center;
            hText.Font = new Font("Arial", 8, FontStyle.Bold);
            hText.TextBrush = new StiSolidBrush(Color.White);
            hText.WordWrap = true;
            headerBand.Components.Add(hText);
        }


    }
}