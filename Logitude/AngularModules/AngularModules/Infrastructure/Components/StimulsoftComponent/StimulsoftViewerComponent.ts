
declare var System: any;
declare var window: any;
import { Component, AfterViewInit, OnInit } from '@angular/core';
import { BuildStimulReportResult, EditableFieldPosition } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/BuildStimulReportResult';
import { StimulsoftArg } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/StimulsoftArg';
import { DocumentTypeTemplatePMService } from '../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import { ReportService } from '../../../Common/Services/ExtendedLists/ReportService';
import { ReportsTemplateList } from '../../../Common/EntityLists/ReportsTemplateList';
import { AppTool } from '../../../Infrastructure/Tools';
import { DocumentTypeTemplatePM } from '../../../Common/EntityPMs/DocumentTypeTemplatePM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
declare var SelectionInput, GetPercentageImageHeight, SetNewValue, jQuery, HTMLID: any;
import { DocumentTypeTemplatePMExtendedService } from '../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import { DocumentTypeTemplateFilter } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/DocumentTypeTemplateFilter';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { GeneralEmailSender } from '../../../Infrastructure/Helpers/GeneralEmailSender';
import { AttachmentsList } from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import { ReportsTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { SchedulerReportMessageTemplateService } from './Services/SchedulerReportMessageTemplateService';
import { ReportFliter } from 'Report/Components/Filters/ReportFliter';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';

@Component({


    selector: 'StimulsoftViewer',
    templateUrl: './StimulsoftViewerComponent.html',
    providers: [DocumentTypeTemplatePMService, DocumentTypeTemplatePMExtendedService, ReportService],
    inputs: ['StimulsoftArgData']

})

export class StimulsoftViewerComponent implements OnInit {
    public StimulsoftArgData: StimulsoftArg;
    element: HTMLImageElement;
    ImageUrl: string;
    EditableField: EditableFieldPosition[];
    ScreenHeight: string;
    ScreenWidth: string;
    ImageWidth: string;
    ImageHeight: string;
    NumberofEditedfield: number = 0;
    IsShowSendButton: boolean = false;
    IsShowReportManageTemplateLink: boolean = false;
    ExportDataOnly: boolean = false;
    ExportObjectFormatting: boolean = false;
    UseOnePageHeaderandFooter: boolean = false;

    Precentage: number;
    OrginalImageWidth: string;
    ImageMargeLeft: string = "15px";
    ImageMargetop: string;
    IsCloseEditWindow: boolean = false;
    Headervisibility: string = "none";
    Footervisibility: string = "none";
    GoToLastPageButtonEnable: boolean = true;
    GoToFirstPageButtonEnable: boolean = false;

    NextButtonEnable: boolean = true;
    PreviouseButtonEnable: boolean = false;
    ImageWidthNumber: number;
    ImageHeightNumber: number;

    IsShowExportPrinttoPDF: boolean = false;
    IsShowExportMicrosoftExcel: boolean = false;
    NumberOfPage: number = 1;
    PagesCount: number = 1;
    SelectInput: Node;
    Imagevisibility: string;
    HeightImg: number = 1;
    MaxScreenwidth: string;
    IsEditingEnabled: boolean;
    IsSchedulerReport: boolean = false;
    IsShowShiftToolbar: boolean = false;
    ShowReportsTemlatesLists: boolean = false;
    ShowMessageTemlatesLists: boolean = false;
    PreviewStimualDivId: string;
    ViewerContentDivId: string;

    TemplateType: string = "R";
    TemplateTypeName: string = "PDF Template";

    SelectedReportsTemplateList: ReportsTemplateList;
    SelectedExcelReportsTemplateList: ReportsTemplateList;
    SelectedMessageTemplateList: ReportsTemplateList;
    ReportsTemplatesLists: ReportsTemplateList[] = [];
    ExcelReportsTemplatesLists: ReportsTemplateList[] = [];
    MessageTemplatesLists: ReportsTemplateList[] = [];
    EntityPM: any;
    public documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    public reportService: ReportService;
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    IsEnableReportTemplateExcel: boolean = false;
    IsEnableButtonExcel: boolean = false;

    SelectedFontSize: number;
    FontSizeLists: number[] = [];
    public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService;
    private CurrentSession = SessionLocator.SelectedSession;
    private schedulerReportMessageTemplateService: SchedulerReportMessageTemplateService;
    LayoutDirection: string = 'ltr';

    constructor() {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;


        this.FillFontSizeLists();

        if (this.schedulerReportMessageTemplateService == null) {
            this.schedulerReportMessageTemplateService = new SchedulerReportMessageTemplateService();
        }

        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();

        }
        if (this._documentTypeTemplatePMExtendedService == null) {
            this._documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService();
        }

        if (FeatureLocator.HasFeaturePermession("Report", "UPDATE")) this.IsShowReportManageTemplateLink = true;
        if (this.reportService == null) {
            this.reportService = new ReportService();

        }




        this.PreviewStimualDivId = Guid.newGuid();
        this.ViewerContentDivId = Guid.newGuid();
    }


    FillFontSizeLists() {

        let fontSizes = "8,9,10,11,12,14,16,18,20,22,24,26,28,36,48,72";
        fontSizes.split(',').forEach((fontsize) => {
            this.FontSizeLists.push(Number(fontsize));
        });

        this.SelectedFontSize = this.FontSizeLists[0];
    }
    ngAfterViewInit() {
        this.SetReportTypeClickText(this.StimulsoftArgData?.TemplateType);

     }
    ngOnInit() {

        this.StimulsoftArgData.StimulsoftViewerComponent = this;
        this.EntityPM = this.StimulsoftArgData.ReportsPreviewComponent.Report;
        
        const processMenuTemplateId = this.StimulsoftArgData?.ProcessMenuTemplateId;
        const templates = this.StimulsoftArgData?.ReportsTemplateLists ?? [];
        const selected = templates.find(t => t.Id === processMenuTemplateId);

        if (!AppTool.IsNullOrEmpty(processMenuTemplateId) && selected) {

            this.TemplateType = selected?.TemplateType ?? "R";
            this.SetReportTypeClickText(this.TemplateType);

            if (this.TemplateType === "E") {
                this.StimulsoftArgData.DefaultExcelTemplateId = processMenuTemplateId;
                this.ReportsTemplatesLists = templates.filter(t => t.TemplateType === "E" && t.UseStimul);
            } 
            else {
                this.StimulsoftArgData.DefaultTemplateId = processMenuTemplateId;
                this.ReportsTemplatesLists = templates.filter(t => t.TemplateType === "R");
            }

            this.SelectedReportsTemplateList = selected ?? null;
        }
        else {
            this.ReportsTemplatesLists = templates.filter(t => t.TemplateType === "R");
            this.SelectedReportsTemplateList = this.ReportsTemplatesLists.find(t => t.Id === this.StimulsoftArgData.DefaultTemplateId) ?? null;
        }

        this.ExcelReportsTemplatesLists = this.StimulsoftArgData.ReportsTemplateLists.filter(d => d.TemplateType == "E" && !d.UseStimul);
        this.MessageTemplatesLists = this.StimulsoftArgData.MessageTemplateLists;
        this.MessageTemplatesLists = this.StimulsoftArgData.MessageTemplateLists.filter(messageTemplate => messageTemplate.EntityId == this.StimulsoftArgData.EntityId || AppTool.IsNullOrEmpty(messageTemplate.EntityId));
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
        
        if (this.ExcelReportsTemplatesLists) { 
            this.SelectedExcelReportsTemplateList = this.ExcelReportsTemplatesLists.find(d => d.Id == this.EntityPM.DefaultExcelNoStimId) ?? null;
        }
        if (this.MessageTemplatesLists) {
            this.SelectedMessageTemplateList = this.MessageTemplatesLists.filter(d => d.Id == this.StimulsoftArgData.DefaultMessageTemplateId)[0];
        }

        this.ShowReportsTemlatesLists = this.StimulsoftArgData.ShowReportsTemlatesLists;
        this.ShowMessageTemlatesLists = this.StimulsoftArgData.IsSchedulerReport && this.StimulsoftArgData.ResultType == "Email";
        if ((this.StimulsoftArgData.ReportsPreviewComponent && this.StimulsoftArgData.ReportsPreviewComponent.FilterConrolHeight) || this.StimulsoftArgData.TypePage != "Report") {

            if (this.StimulsoftArgData.ScreenHeight && this.StimulsoftArgData.ScreenWidth) {
                var screenHeight = this.StimulsoftArgData.ScreenHeight;
                if (this.StimulsoftArgData.ShowStimulFooter) screenHeight -= 30;
                if (this.StimulsoftArgData.ShowStimulHeader) screenHeight -= 30;

                this.ScreenHeight = this.CustomPixel(screenHeight);

                this.SetStimualData();

            }
        }    
        this.IsEnableButtonExcel = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "REE")[0] ? true : false;
    
        if (FeatureLocator.HasFeaturePermession("ReportsTemplate", "ReportTemplateExcel") && this.StimulsoftArgData.IsExcelReportAllowed == true) {
            this.IsEnableReportTemplateExcel = true;
        }
    }

    SetReportTypeClickText(templateType: string) {
        if (AppTool.IsNullOrEmpty(templateType))
            return;
        this.TemplateType = templateType;
        this.TemplateTypeName = templateType == "E" ? "Excel Template" : "PDF Template";
    }


    IsShowStimulImage: boolean = true;
    SetStimualData() {

        var isDisablePreview = this.StimulsoftArgData.ReportFliter ? this.StimulsoftArgData.ReportFliter.DisablePreview : false;

        this.IsShowStimulImage = !isDisablePreview;
        if (this.StimulsoftArgData.ShowStimulHeader) this.Headervisibility = "block";
        if (this.StimulsoftArgData.ShowStimulFooter) this.Footervisibility = "block";

        if (!isDisablePreview) this.BuildStimulImage(this.StimulsoftArgData.BuildStimulReportResult, this);
        this.IsShowExportMicrosoftExcel = this.StimulsoftArgData.IsShowExportMicrosoftExcel;
        this.IsShowExportPrinttoPDF = this.StimulsoftArgData.IsShowExportPrinttoPDF;

        this.IsShowSendButton = this.StimulsoftArgData.IsShowSendButton;
        if (this.StimulsoftArgData.IsSchedulerReport) {
            this.IsSchedulerReport = true;
            this.IsShowExportPrinttoPDF = false;
            this.IsShowSendButton = false;
            this.IsShowExportMicrosoftExcel = false;
        }

        this.PagesCount = this.StimulsoftArgData.PagesCount;
        if (this.PagesCount == null) this.PagesCount = 0;
        this.NumberOfPage = this.StimulsoftArgData.NumberOfPage;

        if (isDisablePreview) this.IsEditingEnabled = true;
        this.SetEnableButtonPager();


    }


    BuildStimulImage(buildStimulReportResult: BuildStimulReportResult, dataContext: any) {


        var element = document.getElementById(this.ViewerContentDivId);

        if (element) {
            element.innerHTML = "";
            this.ScrolToTop();
        }

        if (buildStimulReportResult) {
            var editableFieldPosition = buildStimulReportResult.EditableFieldPositionLists;
            this.EditableField = editableFieldPosition;
            this.Imagevisibility = "none";

            if (buildStimulReportResult.StimulImageBase64) {
                this.IsEditingEnabled = true;
                this.Imagevisibility = "block";
                this.ImageUrl = buildStimulReportResult.StimulImageBase64;
                this.PagesCount = buildStimulReportResult.PageCount;
                this.StimulsoftArgData.PagesCount = buildStimulReportResult.PageCount;
                this.StimulsoftArgData.ReportKey = buildStimulReportResult.ReportKey;


                var stimulImage = new Image();
                stimulImage.onload = function () {

                    dataContext.Precentage = (stimulImage.height / stimulImage.width);
                    dataContext.OrginalImageWidth = stimulImage.width;
                    dataContext.SetScreenWidthAndHeight(dataContext.StimulsoftArgData.ScreenWidth, dataContext.StimulsoftArgData.ScreenHeight);
                    if (dataContext.StimulsoftArgData.ReportFliter) {
                        dataContext.StimulsoftArgData.ReportFliter.ReportKey = buildStimulReportResult.ReportKey;
                    }

                    dataContext.BuildEditableField(editableFieldPosition, dataContext);
                };

                stimulImage.src = this.ImageUrl;

            } else {
                this.IsEditingEnabled = false;
            }
            if (numberofEditedfield) {
                var numberofEditedfield = editableFieldPosition.filter(d => d.FieldName == "NumberOfEditedField")[0];
                if (numberofEditedfield) {
                    this.NumberofEditedfield = Number(numberofEditedfield.FieldValue);
                }
            }
        }
    }


    EditableFieldPositions: EditableFieldPosition[]
    BuildEditableField(editableFieldPosition: EditableFieldPosition[], dataContext: any) {
        this.EditableFieldPositions = editableFieldPosition;

        var numberofEditedfield = this.NumberofEditedfield;
        var element = document.getElementById(dataContext.ViewerContentDivId);


        if (editableFieldPosition && editableFieldPosition.length > 0) {

            editableFieldPosition.forEach((item) => {

                if (item.FieldName != "Image" && item.FieldName != "NumberOfEditedField") {
                    var precePageHeight: number = dataContext.ImageHeightNumber / item.HeightPagePrecentage;
                    // var precePageHeight: number = dataContext.ImageHeightNumber / (item.WidthPagePrecentage * dataContext.Precentage);
                    var precePageWidth: number = dataContext.ImageWidthNumber / item.WidthPagePrecentage;
                    var fontSize = "";
                    if (dataContext.OrginalImageWidth && dataContext.OrginalImageWidth > 0) {
                        var perfont = dataContext.OrginalImageWidth / item.FontSize;
                        fontSize = (dataContext.ImageWidthNumber / perfont).toString() + "px";
                    }
                    else fontSize = item.FontSize.toString() + "px";


                    var Left = (item.Left * precePageWidth).toString() + "px";
                    var Top = (item.Top * precePageHeight).toString() + "px";
                    var Width = (item.Width * precePageWidth).toString() + "px";
                    var Height = (item.Height * precePageHeight).toString() + "px";
                    var fontFamily = item.FontFamily;
                    var fontweight = item.Fontweight;


                    var FieldName = item.FieldName;
                    var FieldValue = item.FieldValue;
                    var FieldKey = item.Key = Guid.newGuid();



                    if (item.ControlType == "text") {


                        var border = "1px solid #808080";// !item.IsEditedField ? "1px solid #808080" : "1px solid red";
                        var borderRadius = !item.IsEditedField ? "0px" : "3px";



                        var backgroundColor = !item.IsEditedField ? "#CDF7FF" : "#b6fcb6";

                        var style = "left:" + Left + ";top:" + Top + ";height:" + Height + ";width:" + Width + ";position: absolute;margin:0px" + ";min-width:" + Width + ";min-height:" + Height + ";font-size:" + fontSize + ";font-weight:" + fontweight + ";font-family:" + fontFamily + ";color:" + item.TextColor + ";vertical-align:" + item.VerticalAlign + "; text-align:" + item.TextAligh + ";float:" + item.Float + ";rows=5" + ";border:" + border + ";border-radius:" + borderRadius + "; resize:none;background-color:" + backgroundColor + ";overflow:hidden";

                        var textarea = document.createElement("textarea");
                        textarea.value = FieldValue;
                        textarea.id = FieldKey;

                        textarea.setAttribute("style", style);
                        textarea.setAttribute("id", FieldKey);
                        textarea.setAttribute("name", FieldName);
                        textarea.setAttribute("value", FieldValue);
                        textarea.onblur = function () {

                            if (editableFieldPosition) {
                                var field = editableFieldPosition.filter(d => d.Key == textarea.id)[0];

                                var oldValueArea = document.createElement("textarea");
                                oldValueArea.value = field.OldValue;

                                var fieldvalueArea = document.createElement("textarea");
                                fieldvalueArea.value = field.NewValue;
                                field.ReturnToOriginValue = false;

                                if (oldValueArea.value != textarea.value) {
                                    field.IsEditedField = true;

                                    if ((fieldvalueArea.value != textarea.value) || (AppTool.IsNullOrEmpty(textarea.value))) {
                                        field.NewValue = textarea.value;
                                        field.Status = "Change";
                                        field.IsTextValueChange = true;
                                    }
                                }
                                else {
                                    if (field.FieldValue == oldValueArea.value) dataContext.ResetTextValueEditableField(field);
                                    else {
                                        field.Status = "Change";
                                        field.IsTextValueChange = true;
                                        field.NewValue = field.OldValue;
                                        field.ReturnToOriginValue =field.IsFontSizeChange? false:true;
                                    }

                                    field.IsEditedField = (field.IsFontSizeChange || field.FontSize != field.OriginalFontSize);

                                }


                            }


                        };


                        textarea.onfocus = function () {

                            textarea.style.backgroundColor = "white";
                            textarea.style.borderColor = "red";
                            textarea.style.borderRadius = "3px"
                            dataContext.SelectTextBoxElement = textarea;

                            if (editableFieldPosition != null) {

                                editableFieldPosition.forEach((item) => {
                                    var element = document.getElementById(item.Key);

                                    if (element != null && element != textarea) {
                                        element.style.backgroundColor = !item.IsEditedField ? "#CDF7FF" : "#b6fcb6";
                                        element.style.borderColor = "#808080";
                                        element.style.borderRadius = "0px";

                                    }

                                });
                                SelectionInput(textarea);

                                dataContext.NumberofEditedfield = editableFieldPosition.filter(d => d.IsEditedField).length;

                            }
                        };

                        element.appendChild(textarea);
                    }
                    else if (item.ControlType == "checkbox") {

                        Height = Height.replace("px", "");
                        var H: number = Number(Height) - 2;
                        var backgroundColor = "#CDF7FF";
                        Width = Width.replace("px", "");
                        var W: number = Number(Width) - 2;

                        var style = "left:" + Left + ";top:" + Top + ";height:" + (H + "px") + ";width:" + (W + "px") + ";position: absolute;margin:0px;" + ";min-width:" + (W + "px") + ";min-height:" + (H + "px");
                        var div = document.createElement('div');

                        div.id = FieldKey;
                        div.setAttribute("style", style);
                        div.setAttribute("name", FieldName);
                        var backgroundColor = !item.IsEditedField ? "#CDF7FF" : "#b6fcb6";
                        div.style.backgroundColor = backgroundColor;


                        var style = "left:0;top:0;right:0;bottom:0;position:absolute;margin:auto";

                        var image = document.createElement("IMG");
                        image.setAttribute("src", "./Images/BlackTick.png");
                        image.setAttribute("height", (W / 2).toString());
                        image.setAttribute("width", (W / 2).toString());
                        image.id = FieldKey + "Img";
                        image.setAttribute("name", FieldName + "IMG");
                        image.setAttribute("style", style);
                        image.style.visibility = FieldValue == "true" ? 'visible' : 'hidden';

                        div.appendChild(image);
                        div.onclick = function () {
                            if (editableFieldPosition) {
                                var field = editableFieldPosition.filter(d => d.Key == div.id)[0];
                                var image = document.getElementById(div.id + "Img");


                                image.style.visibility = image.style.visibility == 'visible' ? 'hidden' : 'visible';

                                var newValue = image.style.visibility == 'visible' ? "true" : "false";
                                field.ReturnToOriginValue = false;
                                if (field.OldValue != newValue) {
                                    field.IsEditedField = true;

                                    if (field.NewValue != newValue) {
                                        if (field != null) {
                                            field.NewValue = newValue;
                                            field.Status = "Change";
                                            field.IsTextValueChange = true;

                                        }
                                    }
                                }
                                else {
                                    if (field.FieldValue == field.OldValue) {
                                        field.Status =field.IsFontSizeChange? field.Status:"";
                                        field.NewValue = "";
                                    }
                                    else {
                                        field.Status = "Change";
                                        field.IsTextValueChange = true;
                                        field.NewValue = field.OldValue;
                                        field.ReturnToOriginValue =field.IsFontSizeChange? false:true;
                                    }

                                    field.IsEditedField = false;

                                }

                                div.style.backgroundColor = !field.IsEditedField ? "#CDF7FF" : "#b6fcb6";

                            }



                            dataContext.NumberofEditedfield = editableFieldPosition.filter(d => d.IsEditedField).length;

                        };

                        element.appendChild(div);




                    }
                }

            });

            var selectelement = document.getElementById(editableFieldPosition[0].Key);
            if (selectelement != null && editableFieldPosition[0].ControlType == "text") {
                selectelement.style.backgroundColor = "white";
                selectelement.style.borderColor = "red";
                selectelement.style.borderRadius = "3px"
                dataContext.SelectTextBoxElement = selectelement;
                selectelement.focus();
            }
        }
    }
    selectTextBoxElement: HTMLElement;
    get SelectTextBoxElement() {
        return this.selectTextBoxElement;
    }
    set SelectTextBoxElement(value: HTMLElement) {
        if (value == this.selectTextBoxElement) return;
        this.selectTextBoxElement = value;
        this.SetSelectedFontSize();
    }



    SetSelectedFontSize() {

        if (!this.SelectTextBoxElement) return;
        if (!this.EditableFieldPositions || this.EditableFieldPositions.length == 0) return;
        let elementKey = this.SelectTextBoxElement.id;
        let editableField = this.EditableFieldPositions.filter(d => d.Key == elementKey)[0];
        if (!editableField) return;
        let fontSize = editableField.Status == "Change" && editableField.IsFontSizeChange ? editableField.NewFontSize : editableField.FontSize;

        this.SelectedFontSize  = this.FontSizeLists.reduce(function (prev, curr) {
            return (Math.abs(curr - fontSize) < Math.abs(prev - fontSize) ? curr : prev);
        });


    }

    FontSizeSelectedItemChanged(fontSize: number) {
        this.SelectedFontSize = fontSize;
        if (!this.SelectTextBoxElement) return;
        let elementKey = this.SelectTextBoxElement.id;
        let editableField = this.EditableFieldPositions.filter(d => d.Key == elementKey)[0];
        if (!editableField) return;
        if (editableField.NewFontSize == fontSize) return;
        editableField.NewFontSize = fontSize;
        editableField.Status = "Change";
        editableField.IsFontSizeChange = true;

        if (!editableField.NewValue) {
            editableField.NewValue = editableField.FieldValue;
        }
        editableField.IsTextValueChange = true;
        editableField.IsEditedField = true;
        editableField.ReturnToOriginValue = false;
        this.SetElementFontSize(fontSize , this.SelectTextBoxElement);

    }



    CalculateFontSizeDependedOnScreenSize(fontSize: number) {

        if (AppTool.IsNullOrEmpty(this.OrginalImageWidth)) return fontSize;
        return (this.ImageWidthNumber / (Number(this.OrginalImageWidth) / fontSize));
    }

    SetElementFontSize(fontSize: number, element: HTMLElement) {
        element.style.fontSize = (this.CalculateFontSizeDependedOnScreenSize(fontSize) + "px");
    }

    SetScreenWidthAndHeight(screenWidth: number, screenHeight: number) {
        if (this.StimulsoftArgData.ShowStimulFooter) screenHeight -= 30;
        if (this.StimulsoftArgData.ShowStimulHeader) screenHeight -= 30;
        this.ScreenHeight = this.CustomPixel(screenHeight);

        if (!this.Precentage) {
            this.Precentage = 1.414398064125831;
        }
        if (this.StimulsoftArgData.IsShowShiftToolbar) {
            this.IsShowShiftToolbar = true;
            if (this.StimulsoftArgData.EditDocumentComponent) {
                if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                    var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(d => d.Id == this.StimulsoftArgData.DocumenttypetemplateId)[0];
                    if (item) {
                        this.HorizontalShift = item.HorizontalShift;
                        this.VerticalShift = item.VerticalShift;
                    }
                    else {
                        this.GetDocumentTypeTemplate(this.StimulsoftArgData.DocumenttypetemplateId, "Set", false);
                    }
                }
            }


        }

        var imagewidth = screenWidth - 30;//Marge 15 left and 15 right
        var imageheight = imagewidth * this.Precentage;
        if (imageheight > screenHeight) {
            imagewidth -= 15;// scrol 15
        }

        this.ImageWidth = this.CustomPixel(imagewidth);
        this.ImageHeight = this.CustomPixel(imagewidth * this.Precentage);


        // ReSize Text Box
        //this.ImageHeightNumber = (imagewidth * this.Precentage) / (1653 * this.Precentage);
        //this.PrecentageImagewidth = imagewidth / 1653;

        this.ImageHeightNumber = (imagewidth * this.Precentage);
        this.ImageWidthNumber = imagewidth;


    }

    public ComputeWdithHeightControlInpopupWindow(popup: number, percentage: number) {


        var percentagecontrol = popup * percentage;
        var stumalWidth = popup - percentagecontrol;
        return stumalWidth;

    }

    SaveToExcelFileAdvancedButtonClick() {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.StimulsoftViewerComponent = this;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 370;
        logWindow.Height = 160;
        logWindow.Title = "Export Advanced Settings";

        logWindow.Show("./Infrastructure/Components/StimulsoftComponent/ExportSettingAdvanceComponent");

    }

    SaveToExcelFileAdvanced(exportDataOnly: boolean = false, exportObjectFormatting: boolean = false, useOnePageHeaderAndFooter: boolean = false) {

        if (this.StimulsoftArgData.ReportKey) {
            this.SaveReport(this.StimulsoftArgData.ReportKey, this.StimulsoftArgData.Tenant, "MicrosoftExceAdvanced", exportDataOnly, exportObjectFormatting, useOnePageHeaderAndFooter);
        }
    }

    SaveToExcelFile() {

        if (this.StimulsoftArgData.ReportKey) {
            this.SaveReport(this.StimulsoftArgData.ReportKey, this.StimulsoftArgData.Tenant, "MicrosoftExce");
        }
    }

    ReportTemplatesChange(item, runReport) {
        if (this.StimulsoftArgData) {
            if (item) {
                if (this.TemplateType === "E") 
                    this.StimulsoftArgData.DefaultExcelTemplateId = item.Id;
                else
                    this.StimulsoftArgData.DefaultTemplateId = item.Id;

                this.StimulsoftArgData.TemplateDescription = item.Description;
                if (this.StimulsoftArgData.ReportFilterConmponent && runReport) {
                    this.RunReport();
                }
            } else {
                this.StimulsoftArgData.DefaultTemplateId = "";
                this.StimulsoftArgData.DefaultExcelTemplateId = "";
            }
        }
    }

      
      ReportExcelTemplatesChange(item: any) {

        this.SelectedExcelReportsTemplateList = item ?? null;
    
        if (this.StimulsoftArgData) {
            this.StimulsoftArgData.DefaultExcelTemplateId = item ? item.Id : null;
        }
    }

    MessageTemplatesChange(item) {
        if (!this.StimulsoftArgData) return;
        this.StimulsoftArgData.DefaultMessageTemplateId = item ? item.Id : "";
        this.SelectedMessageTemplateList = item;
    }


    private RunReport() {
        if (this.TemplateType === "E") {
            if (!this.ReportsTemplatesLists.length) {
                new MessageWindow().Show("There is no Excel Template for this report");
                return;
            }

            if (!this.SelectedReportsTemplateList) {
                new MessageWindow().Show("Please select an Excel Template");
                return;
            }
        }
        if (this.StimulsoftArgData.ReportFilterConmponent['RunReport']) {
            this.StimulsoftArgData.ReportFilterConmponent.RunReport(true);
            return;
        }
        if (this.StimulsoftArgData.ReportFilterConmponent['RunButtonClicked']) {
            this.StimulsoftArgData.ReportFilterConmponent.RunButtonClicked();
            return;
        }
    }

    CustomPixel(pixel: any) {

        if (pixel && pixel.toString().indexOf('.') > -1) {
            pixel = pixel.toString().split('.')[0] + "px";
        }
        else pixel = pixel + "px";


        return pixel;
    }

    PrintToPDF() {

        if (this.StimulsoftArgData.ReportKey) {
            this.SaveReport(this.StimulsoftArgData.ReportKey, this.StimulsoftArgData.Tenant, "PrintToPDF");
        }
    }


    EmailSender: GeneralEmailSender;
    SendDocumentFile(docuemnt: any) {

        var attachment = new AttachmentsList();
        attachment.Tenant = SessionLocator.Tenant;
        attachment.DocumentTypeCopyNameWithDocumentTypeName = docuemnt.FileName;
        attachment.FileSize = docuemnt.FileSize;
        attachment.ShowRemoveLink = true;
        attachment.Id = docuemnt.Id;
        attachment.FileExtension = docuemnt.Extension ? docuemnt.Extension.replace(".", "") : "";


        var attachmentsList = new Array<AttachmentsList>();
        attachmentsList.push(attachment);
        var subject: string = this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent.Title : "Report";
        var entityId: string = this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent.Report ? this.StimulsoftArgData.ReportsPreviewComponent.Report.Id : "" : "";
        var reportFliter:ReportFliter = this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent?.ReportFliter : null;
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender("Report", "", entityId, "StimualReport", null, null, "", subject, attachmentsList,null,this.EntityPM, null,null,null,null,null,reportFliter);
            this.EmailSender.PartnersObslist = this.StimulsoftArgData.PartnersObslist;
            this.EmailSender.SendMessage(this.StimulsoftArgData.ReportFilterConmponent.GLAccountId);
        }
    }

    SendButtonClick(type: string) {

        this.CurrentSession.StartBusyIndicator("Presend...");

        //  var fileName: string = this.StimulsoftArgData.ReportKey + "@" + (this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent.Report.Name:"");
        var fileName: string = this.StimulsoftArgData.ReportKey + "@" + this.StimulsoftArgData.TemplateDescription;
        this.reportService.GetPrepareSendReport(type, fileName, SessionLocator.Tenant).subscribe((res: any) => {
            this.CurrentSession.StopBusyIndicator();

            var pmResponse: ServiceResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    result = myResult;
                    if (result) {
                        this.SendDocumentFile(result);
                    }
                }
            }

        });


    }




    SetEnableButtonPager() {
        if (this.StimulsoftArgData.NumberOfPage == 1) {
            this.PreviouseButtonEnable = false;
            this.GoToFirstPageButtonEnable = false;
        }
        else {
            this.PreviouseButtonEnable = true;
            this.GoToFirstPageButtonEnable = true;
        }



        if (this.StimulsoftArgData.NumberOfPage == this.PagesCount) {
            this.NextButtonEnable = false;
            this.GoToLastPageButtonEnable = false;
        }
        else {
            this.NextButtonEnable = true;
            this.GoToLastPageButtonEnable = true;
        }



    }


    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }

    public IsEditableFieldChanged(field: EditableFieldPosition) {
        return field.FieldValue != field.NewValue || field.FontSize != field.NewFontSize
    }

ResetEditableField(field: EditableFieldPosition){
                            field.Status = "";
                           field.IsFontSizeChange = false;
                           field.IsTextValueChange = false;
}

    ResetTextValueEditableField(field: EditableFieldPosition) {
        if (field.IsFontSizeChange) return;
        field.Status = "";
        field.IsTextValueChange = false;
    }



    GoToPage(numberOfPage: number, processName: string) {

        this.SaveShift();
        if (this.StimulsoftArgData.EditDocumentComponent) {

            var filter = new DocumentTypeTemplateFilter();
            filter.Tenant = this.StimulsoftArgData.Tenant;
            filter.ReportKey = this.StimulsoftArgData.ReportKey;
            filter.DocumentOutId = this.StimulsoftArgData.EditDocumentComponent ? this.StimulsoftArgData.EditDocumentComponent.CurrentDocumentOutId : "";
            filter.Body = "";
            filter.Processtype = "SaveEditFields";
            filter.PageIndex = numberOfPage;
            filter.EditableFieldLists = [];
            if (this.EditableField != null && this.EditableField.length > 0) {

                this.EditableField.filter(d => d.Status == "Change").forEach((field) => {
                    if (this.IsEditableFieldChanged(field) ) {
                        filter.EditableFieldLists.push(field);
                    }
                });
            }

            if (filter.EditableFieldLists && filter.EditableFieldLists.length > 0) {

                this.CurrentSession.StartBusyIndicatorSaving();

                this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    this.CurrentSession.StopBusyIndicator();

                    if (!pmResponse.HasError) {
                        this.EditableField.filter(d => d.Status == "Change" ).forEach((field) => {

                           this.ResetEditableField(field);

                        });

                        if (this.StimulsoftArgData.EditDocumentComponent && this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                            this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            this.StimulsoftArgData.EditDocumentComponent.CurrentDocument.EditableFields = pmResponse.Result;
                        }

                        this.SaveEditFieldComplete(processName);
                    }
                    else {
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                        }
                    }



                });
            }
            else this.SaveEditFieldComplete(processName);


        }
        else this.SaveEditFieldComplete(processName);
    }


    SaveEditFieldComplete(processName: string) {

        if (this.StimulsoftArgData.ReportsPreviewComponent) {
            this.StimulsoftArgData.ReportFliter.ProcessType = processName;
            this.StimulsoftArgData.ReportFliter.NumberOfPage = this.StimulsoftArgData.NumberOfPage;
            this.StimulsoftArgData.ReportsPreviewComponent.GenerateReport(this.StimulsoftArgData.ReportFliter, true);
        }
        else {
            if (this.StimulsoftArgData.EditDocumentComponent) {
                var isdisplayonly = this.StimulsoftArgData.TypePage == "StimaulEdit" ? false : true;
                this.StimulsoftArgData.EditDocumentComponent.LoadstimulData(null, isdisplayonly, true, this.StimulsoftArgData.NumberOfPage, processName, this.StimulsoftArgData.ReportKey);
            }

        }


    }

    NextPage() {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage < this.PagesCount) {
            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage += 1;

            this.GoToPage(saveIndexPage, "NextPage");

        }
    }

    GoToFirstPage() {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage != 1) {
            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage = 1;
            this.GoToPage(saveIndexPage, "FirstPage");
        }


    }

    GoToLastPage() {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage != this.StimulsoftArgData.PagesCount) {

            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage = this.StimulsoftArgData.PagesCount;
            this.GoToPage(saveIndexPage, "LastPage");
        }
    }

    PreviousPage() {

        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage > 1) {

            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage -= 1;
            this.GoToPage(saveIndexPage, "PreviousPage");

        }


    }





    documenttypetemplatePM: DocumentTypeTemplatePM;
    SaveReport(reportKey: string, tenant: number, processType: string, exportDataOnly: boolean = false, exportObjectFormatting: boolean = false, useOnePageHeaderAndFooter: boolean = false) {

        var advanceSetting: string = "";

        if (processType == "MicrosoftExce") advanceSetting = "&UseOnePageHF=" + true;

        else if (processType == "MicrosoftExceAdvanced") {

            this.ExportDataOnly = exportDataOnly;
            this.ExportObjectFormatting = exportObjectFormatting;
            this.UseOnePageHeaderandFooter = useOnePageHeaderAndFooter;


            if (useOnePageHeaderAndFooter) advanceSetting += ("&useOnePageHF=" + true);
            if (exportDataOnly) advanceSetting += ("&exportDataOnly=" + true);
            if (exportObjectFormatting) advanceSetting += ("&exportObjectForm=" + true);


        }

        if (this.StimulsoftArgData.ReportsPreviewComponent) {


            var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadReportPage.aspx?fileName=" + reportKey + "@" + this.StimulsoftArgData.TemplateDescription + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&type=" + processType + advanceSetting;
            window.open(url);
        }
    }

    DownloadDisablePreview() {
        //var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadReportPage.aspx?fileName=" + this.StimulsoftArgData.ReportKey + "@" + this.StimulsoftArgData.TemplateDescription + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&type=DisablePreview"
        //window.open(url);

    }



    ManagementReport() {

        if (this.StimulsoftArgData?.ReportsPreviewComponent?.Report?.Id) 
        {
            var reportId: string = this.StimulsoftArgData.ReportsPreviewComponent.Report.Id;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    const instance = cmpRef.instance;
                    instance.ComponentRef = cmpRef;
                    instance.Run({ EntityId: reportId, ObjectTableName: "Report" });

                    const loadTemplates = (runReport: boolean, isRefreshDefaultTemplate: boolean = true) => {
                        const entity = instance.EntityPM;
                        const defaultTemplateId = entity?.DefaultTemplateId || "";
                        const defaultExcelTemplateId = entity?.DefaultExcelTemplateId || "";
                        const defaultExcelNoStimId = entity?.DefaultExcelNoStimId || "";
        
                        this.LoadReportTemplate(defaultTemplateId, runReport, defaultExcelTemplateId, isRefreshDefaultTemplate, defaultExcelNoStimId);
                    };
        
                    instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => isSaveSuccess ?? loadTemplates(false, true));
                    instance.SaveCompleted.subscribe(() => loadTemplates(false, true));
                    instance.BackCompleted.subscribe(() => loadTemplates(false, false));
                   
                });
        }
    }

    LoadReportTemplate(defultTemplateId: any, runReport, defaultExcelTemplateId: any = "", isRefreshDefaultTemplate: boolean = true, defaultExcelNoStimId : any = "") {

        const reportId = this.StimulsoftArgData.ReportsPreviewComponent.Report.Id;
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                const allTemplates = myResponse.Result;
                this.ReportsTemplatesLists = allTemplates.filter(t => t.TemplateType === this.TemplateType && (this.TemplateType !== "E" || t.UseStimul));
                this.ExcelReportsTemplatesLists = allTemplates.filter(t => t.TemplateType === "E" && !t.UseStimul);

                this.StimulsoftArgData.ReportsPreviewComponent.ReportsTemplateLists = this.ReportsTemplatesLists;
                this.StimulsoftArgData.ReportsTemplateLists = this.ReportsTemplatesLists;

                if (!isRefreshDefaultTemplate) 
                {
                    if(AppTool.IsNullOrEmpty(this.StimulsoftArgData.DefaultExcelNoStimId))
                        this.StimulsoftArgData.DefaultExcelNoStimId = defaultExcelNoStimId;
                    if(AppTool.IsNullOrEmpty(this.StimulsoftArgData.DefaultExcelTemplateId))
                        this.StimulsoftArgData.DefaultExcelTemplateId = defaultExcelTemplateId;
                    if(AppTool.IsNullOrEmpty(this.StimulsoftArgData.DefaultTemplateId))
                        this.StimulsoftArgData.DefaultTemplateId = defultTemplateId;
                    this.SelectedExcelReportsTemplateList = this.ExcelReportsTemplatesLists.find(a => a.Id === this.StimulsoftArgData.DefaultExcelNoStimId) ?? null;
                    
                    const defaultTemplateId = this.TemplateType === "E" ? this.StimulsoftArgData.DefaultExcelTemplateId : this.StimulsoftArgData.DefaultTemplateId;
                    this.SelectedReportsTemplateList = this.ReportsTemplatesLists.find(a => a.Id === defaultTemplateId) ?? null;
                    
                    return;
                }

                let item = this.SetDefaultTemplate(defultTemplateId, defaultExcelTemplateId, defaultExcelNoStimId);
                this.SelectedReportsTemplateList = item ?? null;
                
                if (!AppTool.IsNullOrEmpty(defaultExcelNoStimId)) 
                    this.SelectedExcelReportsTemplateList = this.ExcelReportsTemplatesLists.find(t => t.Id === defaultExcelNoStimId) ?? null;
                
                this.ReportTemplatesChange(item, runReport);
                
            }

        });

    }


    SetDefaultTemplate(defaultTemplateId: any, defaultExcelTemplateId: any, defaultExcelNoStimId: any = "") {
        
        
        this.StimulsoftArgData.ReportsPreviewComponent.Report.DefaultTemplateId = defaultTemplateId;
        this.StimulsoftArgData.DefaultTemplateId = defaultTemplateId;
        this.StimulsoftArgData.DefaultExcelTemplateId = defaultExcelTemplateId;
    
        if (!AppTool.IsNullOrEmpty(defaultExcelNoStimId)) {
            this.StimulsoftArgData.DefaultExcelNoStimId = defaultExcelNoStimId;
        }
    
        const searchId = this.TemplateType === "E" 
            ? this.StimulsoftArgData.DefaultExcelTemplateId 
            : this.StimulsoftArgData.DefaultTemplateId;
    
        const item = this.ReportsTemplatesLists.find(t => t.Id === searchId && (this.TemplateType !== "E" || t.UseStimul));
    
        return item;
    }

    GetDefaultTemplate(any) {
        if (this.TemplateType == "R") {
            return any.DefaultTemplateId;
        }
        if (this.TemplateType == "E") {
            return any.DefaultExcelTemplateId;
        }
    }
    
    ReportTypeClick(type: string, text: string) {
        this.TemplateType = type;
        this.LoadReportTemplate(this.StimulsoftArgData.DefaultTemplateId, false, this.StimulsoftArgData.DefaultExcelTemplateId, true, this.StimulsoftArgData.DefaultExcelNoStimId);
        this.TemplateTypeName = text;
    }






    HorizontalShift: number;
    VerticalShift: number;

    public RefreshStimual() {

        this.StimulsoftArgData.EditDocumentComponent.LoadstimulData(null, this.StimulsoftArgData.EditDocumentComponent.IsDisplayOnly, true, this.StimulsoftArgData.NumberOfPage, "GenerateReport", this.StimulsoftArgData.ReportKey, "", false);
    }


    public ApplayShift(isCloseEditWindow: boolean) {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        if (this.StimulsoftArgData.EditDocumentComponent) {
            if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(d => d.Id == this.StimulsoftArgData.DocumenttypetemplateId)[0];
                if (item) {
                    var isChange: boolean = false;
                    if (item.HorizontalShift != this.HorizontalShift) isChange = true;
                    if (item.VerticalShift != this.VerticalShift) isChange = true;

                    item.HorizontalShift = this.HorizontalShift;
                    item.VerticalShift = this.VerticalShift;
                    if (isChange) {
                        this.UpdateDocumentTypeTemplate(item, isCloseEditWindow);
                    }
                    else {
                        this.RefreshStimual();
                    }

                }
                else {
                    this.GetDocumentTypeTemplate(this.StimulsoftArgData.DocumenttypetemplateId, "Update", isCloseEditWindow);

                }
            }
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }



    }
    UpdateDocumentTypeTemplate(item: any, isCloseEditWindow: boolean) {

        this.documentTypeTemplatePMService.update(item).subscribe((myResult: any) => {

            this.CurrentSession.CurrentWindow.StopBusyIndicator();



            if (this.StimulsoftArgData.EditDocumentComponent && this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;


                if (isCloseEditWindow) {

                    //if (!this.StimulsoftArgData.EditDocumentComponent.IsDisplayOnly) {
                    //    this.StimulsoftArgData.EditDocumentComponent.SaveEditFeild();
                    //}
                    this.StimulsoftArgData.EditDocumentComponent.CloseButtonClicked();

                }
                else {

                    this.StimulsoftArgData.EditDocumentComponent.LoadstimulData(null, this.StimulsoftArgData.EditDocumentComponent.IsDisplayOnly, true, this.StimulsoftArgData.NumberOfPage, "GenerateReport", this.StimulsoftArgData.ReportKey, "", isCloseEditWindow);
                }



            }




        });
    }

    GetDocumentTypeTemplate(Id: any, mode: string, isCloseEditWindow: boolean) {
        this.documentTypeTemplatePMService.get(Id).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;

                if (myResult) {
                    var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(d => d.Id == myResult.Id)[0];
                    if (item && this.StimulsoftArgData.EditDocumentComponent && this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                        this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(d => d.Id != myResult.Id);
                        this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.push(myResult);
                    }
                    else {
                        this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.push(myResult);
                    }

                    if (mode == "Update") {
                        myResult.HorizontalShift = this.HorizontalShift;
                        myResult.VerticalShift = this.VerticalShift;
                        this.UpdateDocumentTypeTemplate(myResult, isCloseEditWindow);
                    }
                    else {

                        this.HorizontalShift = myResult.HorizontalShift;
                        this.VerticalShift = myResult.VerticalShift;
                    }

                }
            }
            else {
                console.error(pmResponse.ErrorsArray);
            }



        });

    }

    CheckIfChangeShift() {

        var ischange = false;
        if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
            var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(d => d.Id == this.StimulsoftArgData.DocumenttypetemplateId)[0];
            if (item) {

                if (item.HorizontalShift != this.HorizontalShift || item.VerticalShift != this.VerticalShift) {
                    ischange = true;
                }
                else ischange = false;
            }
            else ischange = true;

        }
        return ischange;
    }


    SaveShift() {

        if (this.StimulsoftArgData.EditDocumentComponent) {
            if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(d => d.Id == this.StimulsoftArgData.DocumenttypetemplateId)[0];
                if (item) {
                    var isChange: boolean = false;
                    if (item.HorizontalShift != this.HorizontalShift) isChange = true;
                    if (item.VerticalShift != this.VerticalShift) isChange = true;

                    item.HorizontalShift = this.HorizontalShift;
                    item.VerticalShift = this.VerticalShift;
                    if (isChange) {
                        this.documentTypeTemplatePMService.update(item).subscribe((myResult: any) => {
                            if (this.StimulsoftArgData.EditDocumentComponent && this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                                this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            }
                        });
                    }
                }

            }
        }

    }

    ResetButtonClick() {
        this.StimulsoftArgData.IsReset = true;
        if (this.EditableField != null) {
            this.EditableField.filter(d => d.IsEditedField || d.Status == "Change").forEach((item) => {
                var element: HTMLElement = document.getElementById(item.Key);
                if (element != null) {

                    if (item.ControlType == "text") {
                        element.style.backgroundColor = "#CDF7FF";
                        element.style.borderColor = "#808080";
                        element.style.borderRadius = "0px";
                        SetNewValue(element, item.OldValue, item.ControlType);
                        this.SetElementFontSize(item.OriginalFontSize, element);


                    }
                    else {

                        var image = document.getElementById(element.id + "Img");
                        if (image) image.style.visibility = 'hidden';


                    }
                }

                item.IsEditedField = false;
                item.FieldValue = item.OldValue;
                item.NewValue = "";
                item.FontSize = item.OriginalFontSize;
                item.NewFontSize = null;
                this.ResetEditableField(item);

            });

            var selectelement = document.getElementById(this.EditableField[0].Key);
            if (selectelement != null) {
                selectelement.style.backgroundColor = "white";
                selectelement.style.borderColor = "red";
                selectelement.style.borderRadius = "3px"
                selectelement.focus();
            }
        }
        this.NumberofEditedfield = 0;




    }

    ScrolToTop() {
        var htmlid = HTMLID(this.PreviewStimualDivId);
        htmlid.scrollTop(0);

    }



    IsSaveEditField: boolean = false;

    SaveEditFeild(numberOfPage: number) {

        if (this.StimulsoftArgData && this.StimulsoftArgData.EditDocumentComponent) {
            this.SaveShift();
            var editableFieldLists: any[] = [];
            if (this.EditableField != null && this.EditableField.length > 0) {
                this.EditableField.filter(d => d.Status == "Change").forEach((field) => {
                    if (this.IsEditableFieldChanged(field)) {
                        editableFieldLists.push(field);
                    }
                });
            }

            if (editableFieldLists.length > 0 || this.StimulsoftArgData.IsReset) {
                if (this.StimulsoftArgData.IsReset) {

                    this.StimulsoftArgData.EditDocumentComponent._exportDocumentService.GetResetEditableFields(this.StimulsoftArgData.EditDocumentComponent.CurrentDocumentOutId).subscribe((res: any) => {
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            if (this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                                this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            }

                            if (this.StimulsoftArgData && this.StimulsoftArgData.EditDocumentComponent && this.StimulsoftArgData.EditDocumentComponent.CurrentDocument) {
                                this.StimulsoftArgData.EditDocumentComponent.CurrentDocument.EditableFields = pmResponse.Result;
                                this.StimulsoftArgData.IsReset = false;
                            }
                        }
                        this.SaveStimualFeild(numberOfPage);
                    });

                }
                else {
                    this.SaveStimualFeild(numberOfPage);
                }
            }


        }

    }

    SaveStimualFeild(numberOfPage: number) {

        if (this.StimulsoftArgData.EditDocumentComponent) {

            var filter = new DocumentTypeTemplateFilter();
            filter.Tenant = this.StimulsoftArgData.Tenant;
            filter.ReportKey = this.StimulsoftArgData.ReportKey;
            filter.DocumentOutId = this.StimulsoftArgData.EditDocumentComponent ? this.StimulsoftArgData.EditDocumentComponent.CurrentDocumentOutId : "";
            filter.Body = "";
            filter.Processtype = "SaveEditFields";
            filter.PageIndex = numberOfPage;
            filter.EditableFieldLists = [];
            if (this.EditableField != null && this.EditableField.length > 0) {
                this.EditableField.filter(d => d.Status == "Change").forEach((field) => {
                    if (this.IsEditableFieldChanged(field)) {
                        filter.EditableFieldLists.push(field);
                    }
                });
            }

            if (filter.EditableFieldLists && filter.EditableFieldLists.length > 0) {
                this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        this.EditableField.filter(d => d.Status == "Change" ).forEach((field) => {
                            this.ResetEditableField(field);
                        });

                        if (this.StimulsoftArgData.EditDocumentComponent && this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                            this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            this.StimulsoftArgData.EditDocumentComponent.CurrentDocument.EditableFields = pmResponse.Result;
                        }
                    }
                });
            }



        }

    }

    EditMessageTemplate(selectedMessageTemplateList) {

        if (!selectedMessageTemplateList) return;
        let args = {
            EntityId: (this.StimulsoftArgData?.EntityId) ? this.StimulsoftArgData.EntityId : null,
            ObjectTableId: (this.StimulsoftArgData?.ObjectTableId) ? this.StimulsoftArgData.ObjectTableId : null,
            DataViewModel: this,
            ParentEntityId: (this.StimulsoftArgData?.ReportsPreviewComponent?.Report?.Id) ? this.StimulsoftArgData.ReportsPreviewComponent.Report.Id : null,
        }
        this.schedulerReportMessageTemplateService.SetArgs(args);
        this.schedulerReportMessageTemplateService.Edit(selectedMessageTemplateList);
    }

    
    AddMessageTemplate() {
        let args = {
            EntityId: (this.StimulsoftArgData?.EntityId) ? this.StimulsoftArgData.EntityId : null,
            ObjectTableId: (this.StimulsoftArgData?.ObjectTableId) ? this.StimulsoftArgData.ObjectTableId : null,
            DataViewModel: this,
            ParentEntityId: (this.StimulsoftArgData?.ReportsPreviewComponent?.Report?.Id) ? this.StimulsoftArgData.ReportsPreviewComponent.Report.Id : null,
        }
        this.schedulerReportMessageTemplateService.SetArgs(args);
        this.schedulerReportMessageTemplateService.Add();
    }

    EditMessageTemplateListFromPM(reportTemplatePM) {
        if (!reportTemplatePM) return;
        let reportTemplateList = this.schedulerReportMessageTemplateService.MapMessageTemplatePMToList(reportTemplatePM);
        this.MessageTemplatesLists = this.MessageTemplatesLists.filter(temp => temp.Id != reportTemplatePM.Id);
        this.MessageTemplatesLists.push(reportTemplateList);
        this.MessageTemplatesChange(reportTemplateList);
        this.SelectedMessageTemplateList = this.MessageTemplatesLists.filter(temp => temp.Id == reportTemplateList.Id)[0];
    }

    AddNewMessageTemplateListFromPM(reportTemplatePM) {
        if (!reportTemplatePM) return;
        let reportTemplateList = this.schedulerReportMessageTemplateService.MapMessageTemplatePMToList(reportTemplatePM);
        this.MessageTemplatesLists.push(reportTemplateList);
        this.SelectedMessageTemplateList = this.MessageTemplatesLists.filter(temp => temp.Id == reportTemplateList.Id)[0];
        this.MessageTemplatesChange(reportTemplateList);
        this.EditMessageTemplate(reportTemplateList);
    }

    RefreshMessageTemplate(type: string) {
        this.ShowMessageTemlatesLists = (type == "Email");
    }

    async ExportToExcel() {
        if(!this.ExcelReportsTemplatesLists.length)
        {
            const messageWindow = new MessageWindow();
            messageWindow.Show("There is no Excel Template for this report");
            return;
        }
        if(!this.SelectedExcelReportsTemplateList){
            const messageWindow = new MessageWindow();
            messageWindow.Show("Please select Excel Template");
            return;
        }
        
        this.StimulsoftArgData.ReportsPreviewComponent.IsUsedExportToExel = true;
       
        if (this.StimulsoftArgData.ReportFilterConmponent) {
            this.RunReport();
        }
    }
}
