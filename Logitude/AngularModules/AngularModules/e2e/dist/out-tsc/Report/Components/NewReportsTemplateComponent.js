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
var DocumentTypeTemplatePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ReportsTemplatePM_1 = require("../../Common/EntityPMs/ReportsTemplatePM");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var ReportsTemplatePMService_1 = require("../../Common/Services/StandardPMs/ReportsTemplatePMService");
var ClassLevelValidator_1 = require("../../Infrastructure/Validators/ClassLevelValidator");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var ReportsTemplatePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var Tools_1 = require("../../Infrastructure/Tools");
var NewReportsTemplateComponent = /** @class */ (function () {
    function NewReportsTemplateComponent() {
        this.ReportsTemplatePM = new ReportsTemplatePM_1.ReportsTemplatePM();
        this.ValidationErrorsList = [];
        this.NewReportTypeRadio = "NewReportTypeRadio_";
        this.NewReportTypeRadioChoice = "Blank";
        this.ReportTemplateFileId = Guid_1.Guid.NewRandomString();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsVisibile = false;
        this.reportsTemplatePMService = new ReportsTemplatePMService_1.ReportsTemplatePMService();
        this.NewReportTypeRadio += this.CurrentSession.GetNewId("RadioButton");
        this.reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService_1.ReportsTemplatePMExtendedService();
        this._documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService();
        this.validator = new ClassLevelValidator_1.ClassLevelValidator();
    }
    NewReportsTemplateComponent.prototype.ngOnInit = function () {
    };
    NewReportsTemplateComponent.prototype.SetWindowArgs = function (args) {
        this.DataViewModel = args.DataViewModel;
        this.TemplateType = args.TemplateType;
        this.Area = args.Area;
    };
    NewReportsTemplateComponent.prototype.NewReportTypeRadioChange = function (type) {
        this.NewReportTypeRadioChoice = type;
    };
    NewReportsTemplateComponent.prototype.OpenUpLoadTemplateFile = function () {
        document.getElementById(this.ReportTemplateFileId).click();
    };
    NewReportsTemplateComponent.prototype.UpLoadTemplateFileMethod = function (event) {
        var file = querySelection(this.ReportTemplateFileId);
        if (file) {
            var fileExtension = file.name.split('.')[1];
            if (fileExtension) {
                if (((fileExtension == "mrt" || fileExtension == "MRT") && this.TemplateType == "R") || ((fileExtension == "xml" || fileExtension == "XML") && this.TemplateType == "M")) {
                    if (fileExtension.length > 10) {
                        this.ShowMessage("File extension should be less than or equal 10 characters");
                    }
                    else
                        this.ArrayBufferToBase64(file, this);
                }
            }
        }
    };
    NewReportsTemplateComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    NewReportsTemplateComponent.prototype.ArrayBufferToBase64 = function (file, viewmodel) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.TemplateData = window.btoa(binary);
        };
        reader.onerror = function (e) {
        };
        reader.readAsArrayBuffer(file);
    };
    NewReportsTemplateComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewReportsTemplateComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("ReportsTemplate", this.ReportsTemplatePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
        if (this.NewReportTypeRadioChoice == "FromFile" && !this.TemplateData) {
            this.ValidationErrorsList.push("Please load template");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.ReportsTemplatePM.TemplateData = this.TemplateData;
            this.ReportsTemplatePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.ReportsTemplatePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.ReportsTemplatePM.TemplateType = this.TemplateType;
            this.ReportsTemplatePM.ReportId = this.DataViewModel.EntityPM.Id;
            if (this.NewReportTypeRadioChoice == "Blank") {
                this.ReportsTemplatePM.TemplateData = null;
            }
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.NewReportTypeRadioChoice == "FromFile" && this.TemplateType == "M") {
                this._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(this.TemplateData).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var htmltemplate = myResult;
                            if (htmltemplate) {
                                _this.ReportsTemplatePM.TemplateData = null;
                                var headerHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                var footerHtml = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";
                                var body = !Tools_1.AppTool.IsNullOrEmpty(htmltemplate.BodyHtml) ? htmltemplate.BodyHtml : "";
                                body = (headerHtml + body + footerHtml);
                                _this.ReportsTemplatePM.TemplateData = StringToBase64(body);
                            }
                        }
                    }
                    _this.ComplateSave();
                });
            }
            else
                this.ComplateSave();
        }
    };
    NewReportsTemplateComponent.prototype.ComplateSave = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            this.ReportsTemplatePM.IsSystem = true;
        }
        this.reportsTemplatePMExtendedService.CreateReportTemplate(this.ReportsTemplatePM).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var viewModel = null;
                    _this.ReportsTemplatePM = result;
                    if (_this.DataViewModel) {
                        if (_this.Area != "GeneralSendComponent")
                            _this.DataViewModel.IsChange = true;
                        else {
                            viewModel = _this.DataViewModel.BuildViewModel(_this.ReportsTemplatePM);
                        }
                        if (_this.TemplateType == "R") {
                            _this.DataViewModel.ReportsTemplatePMLists.push(_this.ReportsTemplatePM);
                            _this.DataViewModel.CurrentReportsTemplatePM = _this.ReportsTemplatePM;
                            if (_this.DataViewModel.ReportsTemplatePMLists.length == 1) {
                                _this.DataViewModel.SetAsDefaultButtonClicked(_this.TemplateType);
                            }
                        }
                        else if (_this.TemplateType == "M") {
                            if (_this.Area == "GeneralSendComponent") {
                                _this.DataViewModel.ReportTemplates.push(viewModel);
                                _this.DataViewModel.AllReportTemplates.push(viewModel);
                                if (_this.DataViewModel.ReportTemplates.length == 1) {
                                    _this.DataViewModel.SetTemplateAsDeflut(viewModel);
                                }
                                _this.DataViewModel.Title = "Templates (" + _this.DataViewModel.ReportTemplates.length + ")";
                            }
                            else {
                                _this.DataViewModel.MessageReportsTemplatePMLists.push(_this.ReportsTemplatePM);
                                _this.DataViewModel.CurrentMessageReportsTemplatePM = _this.ReportsTemplatePM;
                                if (_this.DataViewModel.MessageReportsTemplatePMLists.length == 1) {
                                    _this.DataViewModel.SetAsDefaultButtonClicked(_this.TemplateType);
                                }
                            }
                        }
                    }
                    if (_this.TemplateType == "R") {
                        var windowArgs = {};
                        windowArgs.DataViewModel = _this;
                        windowArgs.ProcessType = "SaveOnSameDocument";
                        windowArgs.ReportTemplateId = _this.ReportsTemplatePM.Id;
                        windowArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        var widthwindow = window.innerWidth;
                        var heighthwindow = window.innerHeight;
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = widthwindow - 100;
                        logWindow.Height = heighthwindow - 100;
                        logWindow.Title = _this.ReportsTemplatePM.Description;
                        logWindow.IsShowCloseButton = true;
                        logWindow.WindowArgs = windowArgs;
                        window.designerClosed = false;
                        logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
                    }
                    else if (_this.TemplateType == "M") {
                        if (_this.Area == "GeneralSendComponent") {
                            _this.DataViewModel.EditTemplate(viewModel, true);
                        }
                        else {
                            _this.DataViewModel.EditMessageReportsTemplate(_this.ReportsTemplatePM, true);
                        }
                    }
                }
            }
            _this.CloseButtonClicked();
        });
    };
    NewReportsTemplateComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/',
            selector: 'NewReportsTemplateComponent',
            templateUrl: 'NewReportsTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewReportsTemplateComponent);
    return NewReportsTemplateComponent;
}());
exports.NewReportsTemplateComponent = NewReportsTemplateComponent;
//# sourceMappingURL=NewReportsTemplateComponent.js.map