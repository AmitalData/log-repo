"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var DocumentTypeTemplatePMService_1 = require("../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var ReportService_1 = require("../../../Common/Services/ExtendedLists/ReportService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var DocumentTypeTemplateFilter_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/DocumentTypeTemplateFilter");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var GeneralEmailSender_1 = require("../../../Infrastructure/Helpers/GeneralEmailSender");
var AttachmentsList_1 = require("../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList");
var ReportsTemplateListExtendedService_1 = require("../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var StimulsoftViewerComponent = /** @class */ (function () {
    function StimulsoftViewerComponent() {
        this.NumberofEditedfield = 0;
        this.IsShowSendButton = false;
        this.IsShowReportManageTemplateLink = false;
        this.ExportDataOnly = false;
        this.ExportObjectFormatting = false;
        this.UseOnePageHeaderandFooter = false;
        this.ImageMargeLeft = "15px";
        this.IsCloseEditWindow = false;
        this.Headervisibility = "none";
        this.Footervisibility = "none";
        this.GoToLastPageButtonEnable = true;
        this.GoToFirstPageButtonEnable = false;
        this.NextButtonEnable = true;
        this.PreviouseButtonEnable = false;
        this.IsShowExportPrinttoPDF = false;
        this.IsShowExportMicrosoftExcel = false;
        this.NumberOfPage = 1;
        this.PagesCount = 1;
        this.HeightImg = 1;
        this.IsShowShiftToolbar = false;
        this.ShowReportsTemlatesLists = false;
        this.ReportsTemplatesLists = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsRefreshReportsTemplateList = false;
        this.IsSaveEditField = false;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
        if (this._documentTypeTemplatePMExtendedService == null) {
            this._documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService();
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", "UPDATE"))
            this.IsShowReportManageTemplateLink = true;
        if (this.reportService == null) {
            this.reportService = new ReportService_1.ReportService();
        }
        this.PreviewStimualDivId = Guid_1.Guid.newGuid();
        this.ViewerContentDivId = Guid_1.Guid.newGuid();
    }
    StimulsoftViewerComponent.prototype.ngAfterViewInit = function () { };
    StimulsoftViewerComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.StimulsoftArgData.StimulsoftViewerComponent = this;
        this.ReportsTemplatesLists = this.StimulsoftArgData.ReportsTemplateLists;
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService_1.ReportsTemplateListExtendedService();
        if (this.ReportsTemplatesLists) {
            this.SelectedReportsTemplateList = this.ReportsTemplatesLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DefaultTemplateId; })[0];
        }
        this.ShowReportsTemlatesLists = this.StimulsoftArgData.ShowReportsTemlatesLists;
        if ((this.StimulsoftArgData.ReportsPreviewComponent && this.StimulsoftArgData.ReportsPreviewComponent.FilterConrolHeight) || this.StimulsoftArgData.TypePage != "Report") {
            if (this.StimulsoftArgData.ScreenHeight && this.StimulsoftArgData.ScreenWidth) {
                var screenHeight = this.StimulsoftArgData.ScreenHeight;
                if (this.StimulsoftArgData.ShowStimulFooter)
                    screenHeight -= 30;
                if (this.StimulsoftArgData.ShowStimulHeader)
                    screenHeight -= 30;
                this.ScreenHeight = this.CustomPixel(screenHeight);
                this.SetStimualData();
            }
        }
    };
    StimulsoftViewerComponent.prototype.SetStimualData = function () {
        this.BuildStimulImage(this.StimulsoftArgData.EditableFieldLists, this);
        if (this.StimulsoftArgData.ShowStimulHeader)
            this.Headervisibility = "block";
        if (this.StimulsoftArgData.ShowStimulFooter)
            this.Footervisibility = "block";
        this.IsShowExportMicrosoftExcel = this.StimulsoftArgData.IsShowExportMicrosoftExcel;
        this.IsShowExportPrinttoPDF = this.StimulsoftArgData.IsShowExportPrinttoPDF;
        this.IsShowSendButton = this.StimulsoftArgData.IsShowSendButton;
        this.PagesCount = this.StimulsoftArgData.PagesCount;
        if (this.PagesCount == null)
            this.PagesCount = 0;
        this.NumberOfPage = this.StimulsoftArgData.NumberOfPage;
        this.SetEnableButtonPager();
    };
    StimulsoftViewerComponent.prototype.BuildStimulImage = function (editableFieldPosition, dataContext) {
        var element = document.getElementById(this.ViewerContentDivId);
        if (element) {
            element.innerHTML = "";
            this.ScrolToTop();
        }
        this.EditableField = editableFieldPosition;
        this.Imagevisibility = "none";
        if (this.EditableField != null) {
            var imageField = editableFieldPosition.filter(function (d) { return d.FieldName == "Image"; })[0];
            var imagebase64 = "";
            if (imageField) {
                this.IsEditingEnabled = true;
                this.Imagevisibility = "block";
                this.ImageUrl = imageField.FieldValue;
                this.PagesCount = imageField.PageCount;
                this.StimulsoftArgData.PagesCount = imageField.PageCount;
                this.StimulsoftArgData.ReportKey = imageField.ReportKey;
                //this.Precentage = GetPercentageImageHeight(this.ImageUrl);
                var i = new Image();
                i.onload = function () {
                    dataContext.Precentage = (i.height / i.width);
                    dataContext.OrginalImageWidth = i.width;
                    dataContext.SetScreenWidthAndHeight(dataContext.StimulsoftArgData.ScreenWidth, dataContext.StimulsoftArgData.ScreenHeight);
                    if (dataContext.StimulsoftArgData.ReportFliter) {
                        dataContext.StimulsoftArgData.ReportFliter.ReportKey = imageField.ReportKey;
                    }
                    dataContext.BuildEditableField(editableFieldPosition, dataContext);
                };
                i.src = this.ImageUrl;
            }
            else {
                this.IsEditingEnabled = false;
            }
            var numberofEditedfield = editableFieldPosition.filter(function (d) { return d.FieldName == "NumberOfEditedField"; })[0];
            if (numberofEditedfield) {
                this.NumberofEditedfield = Number(numberofEditedfield.FieldValue);
            }
        }
    };
    StimulsoftViewerComponent.prototype.BuildEditableField = function (editableFieldPosition, dataContext) {
        var numberofEditedfield = this.NumberofEditedfield;
        var element = document.getElementById(dataContext.ViewerContentDivId);
        editableFieldPosition.forEach(function (item) {
            if (item.FieldName != "Image" && item.FieldName != "NumberOfEditedField") {
                var precePageHeight = dataContext.ImageHeightNumber / item.HeightPagePrecentage;
                // var precePageHeight: number = dataContext.ImageHeightNumber / (item.WidthPagePrecentage * dataContext.Precentage);
                var precePageWidth = dataContext.ImageWidthNumber / item.WidthPagePrecentage;
                var fontSize = "";
                if (dataContext.OrginalImageWidth && dataContext.OrginalImageWidth > 0) {
                    var perfont = dataContext.OrginalImageWidth / item.FontSize;
                    fontSize = (dataContext.ImageWidthNumber / perfont).toString() + "px";
                }
                else
                    fontSize = item.FontSize.toString() + "px";
                var Left = (item.Left * precePageWidth).toString() + "px";
                var Top = (item.Top * precePageHeight).toString() + "px";
                var Width = (item.Width * precePageWidth).toString() + "px";
                var Height = (item.Height * precePageHeight).toString() + "px";
                var fontFamily = item.FontFamily;
                var fontweight = item.Fontweight;
                var FieldName = item.FieldName;
                var FieldValue = item.FieldValue;
                var FieldKey = item.Key = Guid_1.Guid.newGuid();
                if (item.ControlType == "text") {
                    var border = "1px solid #808080"; // !item.IsEditedField ? "1px solid #808080" : "1px solid red";
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
                            var field = editableFieldPosition.filter(function (d) { return d.Key == textarea.id; })[0];
                            var oldValueArea = document.createElement("textarea");
                            oldValueArea.value = field.OldValue;
                            var fieldvalueArea = document.createElement("textarea");
                            fieldvalueArea.value = field.NewValue;
                            field.ReturnToOriginValue = false;
                            if (oldValueArea.value != textarea.value) {
                                field.IsEditedField = true;
                                if ((fieldvalueArea.value != textarea.value) || (Tools_1.AppTool.IsNullOrEmpty(textarea.value))) {
                                    field.NewValue = textarea.value;
                                    field.Status = "Change";
                                }
                            }
                            else {
                                if (field.FieldValue == oldValueArea.value)
                                    field.Status = "";
                                else {
                                    field.Status = "Change";
                                    field.NewValue = field.OldValue;
                                    field.ReturnToOriginValue = true;
                                }
                                field.IsEditedField = false;
                            }
                        }
                    };
                    textarea.onfocus = function () {
                        textarea.style.backgroundColor = "white";
                        textarea.style.borderColor = "red";
                        textarea.style.borderRadius = "3px";
                        if (editableFieldPosition != null) {
                            editableFieldPosition.forEach(function (item) {
                                var element = document.getElementById(item.Key);
                                if (element != null && element != textarea) {
                                    element.style.backgroundColor = !item.IsEditedField ? "#CDF7FF" : "#b6fcb6";
                                    element.style.borderColor = "#808080";
                                    element.style.borderRadius = "0px";
                                }
                            });
                            SelectionInput(textarea);
                            dataContext.NumberofEditedfield = editableFieldPosition.filter(function (d) { return d.IsEditedField; }).length;
                        }
                    };
                    element.appendChild(textarea);
                }
                else if (item.ControlType == "checkbox") {
                    Height = Height.replace("px", "");
                    var H = Number(Height) - 2;
                    var backgroundColor = "#CDF7FF";
                    Width = Width.replace("px", "");
                    var W = Number(Width) - 2;
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
                            var field = editableFieldPosition.filter(function (d) { return d.Key == div.id; })[0];
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
                                    }
                                }
                            }
                            else {
                                if (field.FieldValue == field.OldValue) {
                                    field.Status = "";
                                    field.NewValue = "";
                                }
                                else {
                                    field.Status = "Change";
                                    field.NewValue = field.OldValue;
                                    field.ReturnToOriginValue = true;
                                }
                                field.IsEditedField = false;
                            }
                            div.style.backgroundColor = !field.IsEditedField ? "#CDF7FF" : "#b6fcb6";
                        }
                        dataContext.NumberofEditedfield = editableFieldPosition.filter(function (d) { return d.IsEditedField; }).length;
                    };
                    element.appendChild(div);
                }
            }
        });
        if (editableFieldPosition && editableFieldPosition.length > 0) {
            var selectelement = document.getElementById(editableFieldPosition[0].Key);
            if (selectelement != null && editableFieldPosition[0].ControlType == "text") {
                selectelement.style.backgroundColor = "white";
                selectelement.style.borderColor = "red";
                selectelement.style.borderRadius = "3px";
                selectelement.focus();
            }
        }
    };
    StimulsoftViewerComponent.prototype.SetScreenWidthAndHeight = function (screenWidth, screenHeight) {
        var _this = this;
        if (this.StimulsoftArgData.ShowStimulFooter)
            screenHeight -= 30;
        if (this.StimulsoftArgData.ShowStimulHeader)
            screenHeight -= 30;
        this.ScreenHeight = this.CustomPixel(screenHeight);
        if (!this.Precentage) {
            this.Precentage = 1.414398064125831;
        }
        if (this.StimulsoftArgData.IsShowShiftToolbar) {
            this.IsShowShiftToolbar = true;
            if (this.StimulsoftArgData.EditDocumentComponent) {
                if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                    var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DocumenttypetemplateId; })[0];
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
        var imagewidth = screenWidth - 30; //Marge 15 left and 15 right 
        var imageheight = imagewidth * this.Precentage;
        if (imageheight > screenHeight) {
            imagewidth -= 15; // scrol 15
        }
        this.ImageWidth = this.CustomPixel(imagewidth);
        this.ImageHeight = this.CustomPixel(imagewidth * this.Precentage);
        // ReSize Text Box
        //this.ImageHeightNumber = (imagewidth * this.Precentage) / (1653 * this.Precentage);
        //this.PrecentageImagewidth = imagewidth / 1653;
        this.ImageHeightNumber = (imagewidth * this.Precentage);
        this.ImageWidthNumber = imagewidth;
    };
    StimulsoftViewerComponent.prototype.ComputeWdithHeightControlInpopupWindow = function (popup, percentage) {
        var percentagecontrol = popup * percentage;
        var stumalWidth = popup - percentagecontrol;
        return stumalWidth;
    };
    StimulsoftViewerComponent.prototype.SaveToExcelFileAdvancedButtonClick = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.StimulsoftViewerComponent = this;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 370;
        logWindow.Height = 160;
        logWindow.Title = "Export Advanced Settings";
        logWindow.Show("./Infrastructure/Components/StimulsoftComponent/ExportSettingAdvanceComponent");
    };
    StimulsoftViewerComponent.prototype.SaveToExcelFileAdvanced = function (exportDataOnly, exportObjectFormatting, useOnePageHeaderAndFooter) {
        if (exportDataOnly === void 0) { exportDataOnly = false; }
        if (exportObjectFormatting === void 0) { exportObjectFormatting = false; }
        if (useOnePageHeaderAndFooter === void 0) { useOnePageHeaderAndFooter = false; }
        if (this.StimulsoftArgData.ReportKey) {
            this.SaveReport(this.StimulsoftArgData.ReportKey, this.StimulsoftArgData.Tenant, "MicrosoftExceAdvanced", exportDataOnly, exportObjectFormatting, useOnePageHeaderAndFooter);
        }
    };
    StimulsoftViewerComponent.prototype.SaveToExcelFile = function () {
        if (this.StimulsoftArgData.ReportKey) {
            this.SaveReport(this.StimulsoftArgData.ReportKey, this.StimulsoftArgData.Tenant, "MicrosoftExce");
        }
    };
    StimulsoftViewerComponent.prototype.ReportTemplatesChange = function (item) {
        if (this.StimulsoftArgData) {
            if (item) {
                this.StimulsoftArgData.DefaultTemplateId = item.Id;
                this.StimulsoftArgData.TemplateDescription = item.Description;
                if (this.StimulsoftArgData.ReportFilterConmponent) {
                    if (this.StimulsoftArgData.ReportFilterConmponent['RunReport']) {
                        this.StimulsoftArgData.ReportFilterConmponent.RunReport(true);
                    }
                    else if (this.StimulsoftArgData.ReportFilterConmponent['RunButtonClicked']) {
                        this.StimulsoftArgData.ReportFilterConmponent.RunButtonClicked();
                    }
                }
            }
            else {
                this.StimulsoftArgData.DefaultTemplateId = "";
            }
        }
    };
    StimulsoftViewerComponent.prototype.CustomPixel = function (pixel) {
        if (pixel && pixel.toString().indexOf('.') > -1) {
            pixel = pixel.toString().split('.')[0] + "px";
        }
        else
            pixel = pixel + "px";
        return pixel;
    };
    StimulsoftViewerComponent.prototype.PrintToPDF = function () {
        if (this.StimulsoftArgData.ReportKey) {
            this.SaveReport(this.StimulsoftArgData.ReportKey, this.StimulsoftArgData.Tenant, "PrintToPDF");
        }
    };
    StimulsoftViewerComponent.prototype.SendDocumentFile = function (docuemnt) {
        var attachment = new AttachmentsList_1.AttachmentsList();
        attachment.Tenant = SessionLocator_1.SessionLocator.Tenant;
        attachment.DocumentTypeCopyNameWithDocumentTypeName = docuemnt.FileName;
        attachment.FileSize = docuemnt.FileSize;
        attachment.ShowRemoveLink = true;
        attachment.Id = docuemnt.Id;
        attachment.FileExtension = docuemnt.Extension ? docuemnt.Extension.replace(".", "") : "";
        var attachmentsList = new Array();
        attachmentsList.push(attachment);
        var subject = this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent.Title : "Report";
        var entityId = this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent.Report ? this.StimulsoftArgData.ReportsPreviewComponent.Report.Id : "" : "";
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender_1.GeneralEmailSender("Report", "", entityId, "StimualReport", null, null, "", subject, attachmentsList);
            this.EmailSender.PartnersObslist = this.StimulsoftArgData.PartnersObslist;
            this.EmailSender.SendMessage();
        }
    };
    StimulsoftViewerComponent.prototype.SendButtonClick = function (type) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Presend...");
        //  var fileName: string = this.StimulsoftArgData.ReportKey + "@" + (this.StimulsoftArgData.ReportsPreviewComponent ? this.StimulsoftArgData.ReportsPreviewComponent.Report.Name:"");
        var fileName = this.StimulsoftArgData.ReportKey + "@" + this.StimulsoftArgData.TemplateDescription;
        this.reportService.GetPrepareSendReport(type, fileName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var pmResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    result = myResult;
                    if (result) {
                        _this.SendDocumentFile(result);
                    }
                }
            }
        });
    };
    StimulsoftViewerComponent.prototype.SetEnableButtonPager = function () {
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
    };
    StimulsoftViewerComponent.prototype.ShowMessage = function (message, title) {
        if (title === void 0) { title = ""; }
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    };
    StimulsoftViewerComponent.prototype.GoToPage = function (numberOfPage, processName) {
        var _this = this;
        this.SaveShift();
        if (this.StimulsoftArgData.EditDocumentComponent) {
            var filter = new DocumentTypeTemplateFilter_1.DocumentTypeTemplateFilter();
            filter.Tenant = this.StimulsoftArgData.Tenant;
            filter.ReportKey = this.StimulsoftArgData.ReportKey;
            filter.DocumentOutId = this.StimulsoftArgData.EditDocumentComponent ? this.StimulsoftArgData.EditDocumentComponent.CurrentDocumentOutId : "";
            filter.Body = "";
            filter.Processtype = "SaveEditFields";
            filter.PageIndex = numberOfPage;
            filter.EditableFieldLists = [];
            if (this.EditableField != null && this.EditableField.length > 0) {
                this.EditableField.filter(function (d) { return d.Status == "Change"; }).forEach(function (field) {
                    if (field.FieldValue != field.NewValue) {
                        filter.EditableFieldLists.push(field);
                    }
                });
            }
            if (filter.EditableFieldLists && filter.EditableFieldLists.length > 0) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe(function (res) {
                    var pmResponse = res;
                    _this.CurrentSession.StopBusyIndicator();
                    if (!pmResponse.HasError) {
                        _this.EditableField.filter(function (d) { return d.Status == "Change"; }).forEach(function (field) {
                            field.Status = "";
                        });
                        if (_this.StimulsoftArgData.EditDocumentComponent && _this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                            _this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            _this.StimulsoftArgData.EditDocumentComponent.CurrentDocument.EditableFields = pmResponse.Result;
                        }
                        _this.SaveEditFieldComplete(processName);
                    }
                    else {
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            _this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                        }
                    }
                });
            }
            else
                this.SaveEditFieldComplete(processName);
        }
        else
            this.SaveEditFieldComplete(processName);
    };
    StimulsoftViewerComponent.prototype.SaveEditFieldComplete = function (processName) {
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
    };
    StimulsoftViewerComponent.prototype.NextPage = function () {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage < this.PagesCount) {
            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage += 1;
            this.GoToPage(saveIndexPage, "NextPage");
        }
    };
    StimulsoftViewerComponent.prototype.GoToFirstPage = function () {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage != 1) {
            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage = 1;
            this.GoToPage(saveIndexPage, "FirstPage");
        }
    };
    StimulsoftViewerComponent.prototype.GoToLastPage = function () {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage != this.StimulsoftArgData.PagesCount) {
            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage = this.StimulsoftArgData.PagesCount;
            this.GoToPage(saveIndexPage, "LastPage");
        }
    };
    StimulsoftViewerComponent.prototype.PreviousPage = function () {
        if (this.PagesCount > 1 && this.StimulsoftArgData.NumberOfPage > 1) {
            var saveIndexPage = this.StimulsoftArgData.NumberOfPage - 1;
            this.StimulsoftArgData.NumberOfPage -= 1;
            this.GoToPage(saveIndexPage, "PreviousPage");
        }
    };
    StimulsoftViewerComponent.prototype.SaveReport = function (reportKey, tenant, processType, exportDataOnly, exportObjectFormatting, useOnePageHeaderAndFooter) {
        if (exportDataOnly === void 0) { exportDataOnly = false; }
        if (exportObjectFormatting === void 0) { exportObjectFormatting = false; }
        if (useOnePageHeaderAndFooter === void 0) { useOnePageHeaderAndFooter = false; }
        var advanceSetting = "";
        if (processType == "MicrosoftExce")
            advanceSetting = "&UseOnePageHF=" + true;
        else if (processType == "MicrosoftExceAdvanced") {
            this.ExportDataOnly = exportDataOnly;
            this.ExportObjectFormatting = exportObjectFormatting;
            this.UseOnePageHeaderandFooter = useOnePageHeaderAndFooter;
            if (useOnePageHeaderAndFooter)
                advanceSetting += ("&useOnePageHF=" + true);
            if (exportDataOnly)
                advanceSetting += ("&exportDataOnly=" + true);
            if (exportObjectFormatting)
                advanceSetting += ("&exportObjectForm=" + true);
        }
        if (this.StimulsoftArgData.ReportsPreviewComponent) {
            var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadReportPage.aspx?fileName=" + reportKey + "@" + this.StimulsoftArgData.TemplateDescription + "&tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken() + "&type=" + processType + advanceSetting;
            window.open(url);
        }
    };
    StimulsoftViewerComponent.prototype.ManagementReport = function () {
        var _this = this;
        if (this.StimulsoftArgData && this.StimulsoftArgData.ReportsPreviewComponent && this.StimulsoftArgData.ReportsPreviewComponent.Report && this.StimulsoftArgData.ReportsPreviewComponent.Report.Id) {
            var reportId = this.StimulsoftArgData.ReportsPreviewComponent.Report.Id;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: reportId, ObjectTableName: "Report" });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    var defultTemplateId = cmpRef.instance.EntityPM ? cmpRef.instance.EntityPM.DefaultTemplateId : "";
                    _this.LoadReportTemplate(defultTemplateId);
                });
                cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        var defultTemplateId = cmpRef.instance.EntityPM ? cmpRef.instance.EntityPM.DefaultTemplateId : "";
                        _this.LoadReportTemplate(defultTemplateId);
                    }
                });
            });
        }
    };
    StimulsoftViewerComponent.prototype.LoadReportTemplate = function (defultTemplateId) {
        var _this = this;
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(this.StimulsoftArgData.ReportsPreviewComponent.Report.Id, "R").subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ReportsTemplatesLists = _this.StimulsoftArgData.ReportsPreviewComponent.ReportsTemplateLists = _this.StimulsoftArgData.ReportsTemplateLists = myResponse.Result;
                var item = _this.ReportsTemplatesLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DefaultTemplateId; })[0];
                if (!item) {
                    _this.StimulsoftArgData.ReportsPreviewComponent.Report.DefaultTemplateId = _this.StimulsoftArgData.DefaultTemplateId = defultTemplateId;
                    item = _this.ReportsTemplatesLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DefaultTemplateId; })[0];
                }
                _this.SelectedReportsTemplateList = _this.ReportsTemplatesLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DefaultTemplateId; })[0];
                _this.IsRefreshReportsTemplateList = !_this.IsRefreshReportsTemplateList;
                _this.ReportTemplatesChange(item);
            }
        });
    };
    StimulsoftViewerComponent.prototype.RefreshStimual = function () {
        this.StimulsoftArgData.EditDocumentComponent.LoadstimulData(null, this.StimulsoftArgData.EditDocumentComponent.IsDisplayOnly, true, this.StimulsoftArgData.NumberOfPage, "GenerateReport", this.StimulsoftArgData.ReportKey, "", false);
    };
    StimulsoftViewerComponent.prototype.ApplayShift = function (isCloseEditWindow) {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        if (this.StimulsoftArgData.EditDocumentComponent) {
            if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DocumenttypetemplateId; })[0];
                if (item) {
                    var isChange = false;
                    if (item.HorizontalShift != this.HorizontalShift)
                        isChange = true;
                    if (item.VerticalShift != this.VerticalShift)
                        isChange = true;
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
    };
    StimulsoftViewerComponent.prototype.UpdateDocumentTypeTemplate = function (item, isCloseEditWindow) {
        var _this = this;
        this.documentTypeTemplatePMService.update(item).subscribe(function (myResult) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (_this.StimulsoftArgData.EditDocumentComponent && _this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                _this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                if (isCloseEditWindow) {
                    //if (!this.StimulsoftArgData.EditDocumentComponent.IsDisplayOnly) {
                    //    this.StimulsoftArgData.EditDocumentComponent.SaveEditFeild();
                    //}
                    _this.StimulsoftArgData.EditDocumentComponent.CloseButtonClicked();
                }
                else {
                    _this.StimulsoftArgData.EditDocumentComponent.LoadstimulData(null, _this.StimulsoftArgData.EditDocumentComponent.IsDisplayOnly, true, _this.StimulsoftArgData.NumberOfPage, "GenerateReport", _this.StimulsoftArgData.ReportKey, "", isCloseEditWindow);
                }
            }
        });
    };
    StimulsoftViewerComponent.prototype.GetDocumentTypeTemplate = function (Id, mode, isCloseEditWindow) {
        var _this = this;
        this.documentTypeTemplatePMService.get(Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var item = _this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == myResult.Id; })[0];
                    if (item && _this.StimulsoftArgData.EditDocumentComponent && _this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                        _this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id != myResult.Id; });
                        _this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.push(myResult);
                    }
                    else {
                        _this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.push(myResult);
                    }
                    if (mode == "Update") {
                        myResult.HorizontalShift = _this.HorizontalShift;
                        myResult.VerticalShift = _this.VerticalShift;
                        _this.UpdateDocumentTypeTemplate(myResult, isCloseEditWindow);
                    }
                    else {
                        _this.HorizontalShift = myResult.HorizontalShift;
                        _this.VerticalShift = myResult.VerticalShift;
                    }
                }
            }
            else {
                console.error(pmResponse.ErrorsArray);
            }
        });
    };
    StimulsoftViewerComponent.prototype.CheckIfChangeShift = function () {
        var _this = this;
        var ischange = false;
        if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
            var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DocumenttypetemplateId; })[0];
            if (item) {
                if (item.HorizontalShift != this.HorizontalShift || item.VerticalShift != this.VerticalShift) {
                    ischange = true;
                }
                else
                    ischange = false;
            }
            else
                ischange = true;
        }
        return ischange;
    };
    StimulsoftViewerComponent.prototype.SaveShift = function () {
        var _this = this;
        if (this.StimulsoftArgData.EditDocumentComponent) {
            if (this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists) {
                var item = this.StimulsoftArgData.EditDocumentComponent.DocumentTypeTemplatePMLists.filter(function (d) { return d.Id == _this.StimulsoftArgData.DocumenttypetemplateId; })[0];
                if (item) {
                    var isChange = false;
                    if (item.HorizontalShift != this.HorizontalShift)
                        isChange = true;
                    if (item.VerticalShift != this.VerticalShift)
                        isChange = true;
                    item.HorizontalShift = this.HorizontalShift;
                    item.VerticalShift = this.VerticalShift;
                    if (isChange) {
                        this.documentTypeTemplatePMService.update(item).subscribe(function (myResult) {
                            if (_this.StimulsoftArgData.EditDocumentComponent && _this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                                _this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            }
                        });
                    }
                }
            }
        }
    };
    StimulsoftViewerComponent.prototype.ResetButtonClick = function () {
        this.StimulsoftArgData.IsReset = true;
        if (this.EditableField != null) {
            this.EditableField.filter(function (d) { return d.IsEditedField || d.Status == "Change"; }).forEach(function (item) {
                var element = document.getElementById(item.Key);
                if (element != null) {
                    if (item.ControlType == "text") {
                        element.style.backgroundColor = "#CDF7FF";
                        element.style.borderColor = "#808080";
                        element.style.borderRadius = "0px";
                        SetNewValue(element, item.OldValue, item.ControlType);
                    }
                    else {
                        var image = document.getElementById(element.id + "Img");
                        if (image)
                            image.style.visibility = 'hidden';
                    }
                }
                item.IsEditedField = false;
                item.Status = "";
                item.FieldValue = item.OldValue;
                item.NewValue = "";
            });
            var selectelement = document.getElementById(this.EditableField[0].Key);
            if (selectelement != null) {
                selectelement.style.backgroundColor = "white";
                selectelement.style.borderColor = "red";
                selectelement.style.borderRadius = "3px";
                selectelement.focus();
            }
        }
        this.NumberofEditedfield = 0;
    };
    StimulsoftViewerComponent.prototype.ScrolToTop = function () {
        var htmlid = HTMLID(this.PreviewStimualDivId);
        htmlid.scrollTop(0);
    };
    StimulsoftViewerComponent.prototype.SaveEditFeild = function (numberOfPage) {
        var _this = this;
        if (this.StimulsoftArgData && this.StimulsoftArgData.EditDocumentComponent) {
            this.SaveShift();
            var editableFieldLists = [];
            if (this.EditableField != null && this.EditableField.length > 0) {
                this.EditableField.filter(function (d) { return d.Status == "Change"; }).forEach(function (field) {
                    if (field.FieldValue != field.NewValue) {
                        editableFieldLists.push(field);
                    }
                });
            }
            if (editableFieldLists.length > 0 || this.StimulsoftArgData.IsReset) {
                if (this.StimulsoftArgData.IsReset) {
                    this.StimulsoftArgData.EditDocumentComponent._exportDocumentService.GetResetEditableFields(this.StimulsoftArgData.EditDocumentComponent.CurrentDocumentOutId).subscribe(function (res) {
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            if (_this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                                _this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            }
                            if (_this.StimulsoftArgData && _this.StimulsoftArgData.EditDocumentComponent && _this.StimulsoftArgData.EditDocumentComponent.CurrentDocument) {
                                _this.StimulsoftArgData.EditDocumentComponent.CurrentDocument.EditableFields = pmResponse.Result;
                                _this.StimulsoftArgData.IsReset = false;
                            }
                        }
                        _this.SaveStimualFeild(numberOfPage);
                    });
                }
                else {
                    this.SaveStimualFeild(numberOfPage);
                }
            }
        }
    };
    StimulsoftViewerComponent.prototype.SaveStimualFeild = function (numberOfPage) {
        var _this = this;
        if (this.StimulsoftArgData.EditDocumentComponent) {
            var filter = new DocumentTypeTemplateFilter_1.DocumentTypeTemplateFilter();
            filter.Tenant = this.StimulsoftArgData.Tenant;
            filter.ReportKey = this.StimulsoftArgData.ReportKey;
            filter.DocumentOutId = this.StimulsoftArgData.EditDocumentComponent ? this.StimulsoftArgData.EditDocumentComponent.CurrentDocumentOutId : "";
            filter.Body = "";
            filter.Processtype = "SaveEditFields";
            filter.PageIndex = numberOfPage;
            filter.EditableFieldLists = [];
            if (this.EditableField != null && this.EditableField.length > 0) {
                this.EditableField.filter(function (d) { return d.Status == "Change"; }).forEach(function (field) {
                    if (field.FieldValue != field.NewValue) {
                        filter.EditableFieldLists.push(field);
                    }
                });
            }
            if (filter.EditableFieldLists && filter.EditableFieldLists.length > 0) {
                this._documentTypeTemplatePMExtendedService.SaveDocumentTemplate(filter).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        _this.EditableField.filter(function (d) { return d.Status == "Change"; }).forEach(function (field) {
                            field.Status = "";
                        });
                        if (_this.StimulsoftArgData.EditDocumentComponent && _this.StimulsoftArgData.EditDocumentComponent.DataViewModel) {
                            _this.StimulsoftArgData.EditDocumentComponent.DataViewModel.IsRefreshPrintConrol = true;
                            _this.StimulsoftArgData.EditDocumentComponent.CurrentDocument.EditableFields = pmResponse.Result;
                        }
                    }
                });
            }
        }
    };
    StimulsoftViewerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'StimulsoftViewer',
            templateUrl: './StimulsoftViewerComponent.html',
            providers: [DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService, ReportService_1.ReportService],
            inputs: ['StimulsoftArgData']
        }),
        __metadata("design:paramtypes", [])
    ], StimulsoftViewerComponent);
    return StimulsoftViewerComponent;
}());
exports.StimulsoftViewerComponent = StimulsoftViewerComponent;
//# sourceMappingURL=StimulsoftViewerComponent.js.map