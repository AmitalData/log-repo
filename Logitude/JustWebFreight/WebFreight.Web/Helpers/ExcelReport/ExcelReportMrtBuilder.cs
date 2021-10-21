using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
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
        public ExcelReportMrtBuilder(int tenant)
        {
            this.tenant = tenant;
            this.excelReportFileService = new ExcelReportFileService(tenant);
            this.excelDataProviderFieldsBuilder = new ExcelDataProviderFieldsBuilder();
        }

        public byte[] Build(ReportsTemplate reportsTemplate, string documentId, bool isDocument)
        {
            List<DataProviderField> selectedFields = excelReportFileService.GetDataProviderFieldsFromXML(reportsTemplate.Id, documentId, isDocument);

            report = GetReport(reportsTemplate.ReportId);

            stiReport = new StiReport
            {
                ScriptLanguage = StiReportLanguageType.CSharp
            };

            BuildDataProvider();

            if(selectedFields!=null && selectedFields.Count > 0)
            {
                BuildReport(selectedFields);
            }
    
            return stiReport.SaveToByteArray();
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
                AddVariables(selectedFields, page);
                AddExcelReportLists(selectedFields, page);
                AddHeader(report.Name, page);
                AddFooter(page);
            }
        }

        private void AddVariables(List<DataProviderField> dataProviderFields, StiPage page)
        {
            var variables = dataProviderFields.Where(x => x.Type != "List" && x.Type != "Class").ToList();

            StiHeaderBand stiBand = new StiHeaderBand
            {
                Height = ((double)variables.Count() / 4) + 0.2,
                Name = "VariablesHeader",
            };
            page.Components.Add(stiBand);


            int columnWidth = 2;
            double posX = 0;
            double posY = 0;
            foreach (var item in variables)
            {
                if (posX != 0 && posX % 8 == 0)
                {
                    posX = 0;
                    posY += 0.5;
                }
                CreateVariableHeader(stiBand, columnWidth, posX, posY, item);

                posX += columnWidth;

                CreateVariableText(stiBand, columnWidth, posX, posY, item);

                posX += columnWidth;
            }
        }

        private void CreateVariableText(StiHeaderBand stiBand, int columnWidth, double posX, double posY, DataProviderField item)
        {
            StiText text = new StiText(new RectangleD(posX, posY, columnWidth, 0.5))
            {
                HorAlignment = StiTextHorAlignment.Center,
                Name = item.Name + "Text",
                Text = item.Expression
            };
            text.HorAlignment = StiTextHorAlignment.Center;
            text.VertAlignment = StiVertAlignment.Center;
            text.Font = new Font("Arial", 8, FontStyle.Regular);
            text.TextBrush = new StiSolidBrush(Color.Black);
            text.WordWrap = true;
            stiBand.Components.Add(text);
        }

        private void CreateVariableHeader(StiHeaderBand stiBand, int columnWidth, double posX, double posY, DataProviderField item)
        {
            StiText hText = new StiText(new RectangleD(posX, posY, columnWidth, 0.5))
            {
                HorAlignment = StiTextHorAlignment.Center,
                Name = item.Name + "TextLabel",
                Text = item.Text + " : ",
            };
            hText.HorAlignment = StiTextHorAlignment.Center;
            hText.VertAlignment = StiVertAlignment.Center;
            hText.Font = new Font("Arial", 8, FontStyle.Bold);
            hText.TextBrush = new StiSolidBrush(Color.Black);
            hText.WordWrap = true;
            stiBand.Components.Add(hText);
        }

        private void AddHeader(string text, StiPage page)
        {

            StiPageHeaderBand stiBand = new StiPageHeaderBand
            {
                Height = 0.5,
                Name = "PageHeaderBand",
            };
            page.Components.Add(stiBand);

            StiText stiText = new StiText(new RectangleD(0, 0, page.Width, page.Height / 20), text)
            {
                HorAlignment = StiTextHorAlignment.Center,
                Font = new Font("Arial", 22.5f),
                Name = "PageHeaderText",
            };
            stiBand.Components.Add(stiText);
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
                Name = dataProviderField.Name + "HeaderBand",
                PrintIfEmpty = true,
                PrintOnAllPages = true
            };
            page.Components.Add(headerBand);


            StiDataBand dataBand = new StiDataBand
            {
                BusinessObjectGuid = GetBusinessObjectGuid(dataProviderField),
                Height = 0.3,
                Name = dataProviderField.Name + "DataBand",
            };
            page.Components.Add(dataBand);


            int columnWidth = 2;
            double pos = 0;
            totalWidth = columnWidth * dataProviderField.Fields.Count();
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
                Name = item.Name + "Text",
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
                Name = item.Name + "HeaderLabel",
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