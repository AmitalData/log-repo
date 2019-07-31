declare var System: any;
declare var window: any;

import {Component, OnInit, Output}  from '@angular/core';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {DocumentTypeTemplatePMExtendedService} from '../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ReportsTemplatePM} from '../../Common/EntityPMs/ReportsTemplatePM';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ReportsTemplatePMService} from '../../Common/Services/StandardPMs/ReportsTemplatePMService';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ReportsTemplatePMExtendedService} from '../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
declare var querySelection, StringToBase64, resultToUnitArray: any;
import {AppTool} from '../../Infrastructure/Tools';
@Component({

    moduleId: './Report/Components/',
    selector: 'NewReportsTemplateComponent',
    templateUrl: 'NewReportsTemplateComponent.html',

})


export class NewReportsTemplateComponent implements OnInit {
    public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService;
    reportsTemplatePMService: ReportsTemplatePMService;
    ReportsTemplatePM: ReportsTemplatePM = new ReportsTemplatePM();
    reportsTemplatePMExtendedService:ReportsTemplatePMExtendedService;
    ValidationErrorsList: string[] = [];
    NewReportTypeRadio: string = "NewReportTypeRadio_";
    NewReportTypeRadioChoice: string = "Blank";
    ReportTemplateFileId: string = Guid.NewRandomString();
    validator: ClassLevelValidator;
    Area: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.reportsTemplatePMService = new ReportsTemplatePMService();
        this.NewReportTypeRadio += this.CurrentSession.GetNewId("RadioButton");
        this.reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
        this._documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService();
        this.validator = new ClassLevelValidator();
    }
    IsVisibile: boolean = false;
    ngOnInit() {

    }
    TemplateData: any;
    DataViewModel: any;
    SetWindowArgs(args: any) {
        this.DataViewModel = args.DataViewModel;
        this.TemplateType = args.TemplateType;
        this.Area = args.Area;
        
    }

    NewReportTypeRadioChange(type:string) {
        this.NewReportTypeRadioChoice = type;
    }

    OpenUpLoadTemplateFile() {

        document.getElementById(this.ReportTemplateFileId).click();

    }

    TemplateType: string;
    UpLoadTemplateFileMethod(event: any) {

        var file = querySelection(this.ReportTemplateFileId);

        if (file) {
            var fileExtension = file.name.split('.')[1];
            if (fileExtension) {
                if (((fileExtension == "mrt" || fileExtension == "MRT") && this.TemplateType == "R") || ((fileExtension == "xml" || fileExtension == "XML") && this.TemplateType == "M")) {

                    if (fileExtension.length > 10) {
                        this.ShowMessage("File extension should be less than or equal 10 characters");
                    }

                    else this.ArrayBufferToBase64(file, this);

                }
            }
        }

    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }







    ArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
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

    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("ReportsTemplate", this.ReportsTemplatePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }

        if (this.NewReportTypeRadioChoice == "FromFile" && !this.TemplateData) {
            this.ValidationErrorsList.push("Please load template");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.ReportsTemplatePM.TemplateData = this.TemplateData;
            this.ReportsTemplatePM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.ReportsTemplatePM.UpdatedByUserId = SessionLocator.LoggedUserId;

            this.ReportsTemplatePM.TemplateType = this.TemplateType;
            this.ReportsTemplatePM.ReportId = this.DataViewModel.EntityPM.Id;
            if (this.NewReportTypeRadioChoice == "Blank") {
                this.ReportsTemplatePM.TemplateData = null;
            }

            this.CurrentSession.StartBusyIndicatorSaving();

          
            if (this.NewReportTypeRadioChoice == "FromFile" && this.TemplateType == "M") {

                this._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(this.TemplateData).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            var htmltemplate: any = myResult;
                            if (htmltemplate) {
                                this.ReportsTemplatePM.TemplateData = null;

                                var headerHtml = !AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                var footerHtml = !AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";
                                var body = !AppTool.IsNullOrEmpty(htmltemplate.BodyHtml) ? htmltemplate.BodyHtml : "";
                                body = (headerHtml + body + footerHtml);
                                this.ReportsTemplatePM.TemplateData = StringToBase64(body);
                            }
                        }
                    }
                    this.ComplateSave();
                });
            }

            else this.ComplateSave();


        }


    }


    ComplateSave() {
        if (SessionLocator.Tenant == 0) {
            this.ReportsTemplatePM.IsSystem = true;
        }

        this.reportsTemplatePMExtendedService.CreateReportTemplate(this.ReportsTemplatePM).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var viewModel = null;
                  

                    this.ReportsTemplatePM = result;
                    if (this.DataViewModel) {
                        if (this.Area != "GeneralSendComponent") this.DataViewModel.IsChange = true;
                        else {
                            viewModel = this.DataViewModel.BuildViewModel(this.ReportsTemplatePM);
                        }

                        if (this.TemplateType == "R") {

                            this.DataViewModel.ReportsTemplatePMLists.push(this.ReportsTemplatePM);
                            this.DataViewModel.CurrentReportsTemplatePM = this.ReportsTemplatePM;
                            if (this.DataViewModel.ReportsTemplatePMLists.length == 1) {
                                this.DataViewModel.SetAsDefaultButtonClicked(this.TemplateType);
                            }
                        } else if (this.TemplateType == "M") {

                            if (this.Area == "GeneralSendComponent") {

                                this.DataViewModel.ReportTemplates.push(viewModel);
                                this.DataViewModel.AllReportTemplates.push(viewModel);
                                if (this.DataViewModel.ReportTemplates.length == 1) {
                                    this.DataViewModel.SetTemplateAsDeflut(viewModel);
                                }

                                this.DataViewModel.Title = "Templates (" + this.DataViewModel.ReportTemplates.length + ")";
                            }

                            else {
                                this.DataViewModel.MessageReportsTemplatePMLists.push(this.ReportsTemplatePM);
                                this.DataViewModel.CurrentMessageReportsTemplatePM = this.ReportsTemplatePM;
                                if (this.DataViewModel.MessageReportsTemplatePMLists.length == 1) {
                                    this.DataViewModel.SetAsDefaultButtonClicked(this.TemplateType);
                                }
                            }  
                            
                        }

                    }
                    if (this.TemplateType == "R") {
                        var windowArgs: any = {};
                        windowArgs.DataViewModel = this;
                        windowArgs.ProcessType = "SaveOnSameDocument";
                        windowArgs.ReportTemplateId = this.ReportsTemplatePM.Id;
                        windowArgs.Tenant = SessionLocator.Tenant;
                        var widthwindow = window.innerWidth;
                        var heighthwindow = window.innerHeight;
                        var logWindow = new LogitudeWindow();

                        logWindow.Width = widthwindow - 100;
                        logWindow.Height = heighthwindow - 100;
                        logWindow.Title = this.ReportsTemplatePM.Description;

                        logWindow.IsShowCloseButton = true;
                        logWindow.WindowArgs = windowArgs;
                        window.designerClosed = false;
                        logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
                    }
                    else if (this.TemplateType == "M") {
                        if (this.Area == "GeneralSendComponent") {
                            this.DataViewModel.EditTemplate(viewModel, true);
                        }
                      else {
                            this.DataViewModel.EditMessageReportsTemplate(this.ReportsTemplatePM, true);
                        }  

                        
                    }

                }
            }
            this.CloseButtonClicked();




        });

    }

}
