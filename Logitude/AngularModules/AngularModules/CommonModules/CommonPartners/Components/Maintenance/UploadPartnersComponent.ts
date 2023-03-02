import { Component, OnDestroy } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { BatchTaskExecutionPM } from '../../../../Infrastructure/EntityPMs/BatchTaskExecutionPM';
import { BatchTaskExecutionListService } from '../../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { CommonDomainService, PartnersUploadExcelParameter } from '../../../../Common/Services/CommonDomainService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { AppTool } from '../../../../Infrastructure/Tools';

declare var ResultAsArray: any;

@Component({
    selector: 'UploadPartnersComponent',

    templateUrl: './UploadPartnersComponent.html',
})

export class UploadPartnersComponent extends BaseComponent implements OnDestroy {
    public DataContext = this;
    private CommonDomainService: CommonDomainService;
    public ValidationErrorsList = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public ErrorsList = [];

    constructor() {
        super();
        this.CommonDomainService = new CommonDomainService();
    }

    ngOnDestroy() {
        this.StopTimer();
    }
    // Commands
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    DownloadUploadPartnersTemplate() {
        this.CommonDomainService.DownloadUploadPartnersTemplate().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + fileName;
                {
                    window.open(url);
                }
            }
        });
    }

    private computingPartnerCode: string;
    get ComputingPartnerCode() { return this.computingPartnerCode; }
    set ComputingPartnerCode(newValue: string) {
        if (this.computingPartnerCode != newValue) {
            this.computingPartnerCode = newValue;
        }
    }

    OnFileChanged(fileEvent) {
        var file = fileEvent.target.files[0];

        if (file) {
            var extension: string = file.name.split('.')[1];

            if (extension.includes("xls")) {
                this.SelectExcelFile(fileEvent);
            }

            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }
    }
    FileName: string;
    SelectExcelFile(fileEvent) {
        var file = fileEvent.target.files[0];
        this.FileName = null;
        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.FileName = name[0];
            }
        }
        if (file && file.size > 0) {
            var documentExtendedService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
            documentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    }
    StartUploadingExcelFile(file: any) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    }
    public partnersUploadExcelParameter: PartnersUploadExcelParameter;
    ConvertArrayBufferToBase64(file: any, context: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;

            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            context.partnersUploadExcelParameter = new PartnersUploadExcelParameter();
            context.partnersUploadExcelParameter.FileData = window.btoa(binary);
            context.partnersUploadExcelParameter.FileName = context.FileName;
            context.partnersUploadExcelParameter.ComputingPartnerCode = context.ComputingPartnerCode;
            context.SendExcelToServer(context.partnersUploadExcelParameter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    SendExcelToServer(filter: any) {
        this.CommonDomainService.PostUploadPartnersExcelFile(filter).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
               var batchEntity = myResponse.Result;
                if (batchEntity != null) {
                   
                    this.IsResponseProgressVisible = true;
                    this.CheckBatchTaskExecution(batchEntity.Id);
                }
            }
            else {
                this.StopTimer();
                var window = new MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    }

    // Timer
    public batchEntity: BatchTaskExecutionList;
    private timer: any;
    public IsResponseProgressVisible: boolean = false;
    public DoneMsg: string;
    public IsDone = false;
    CheckBatchTaskExecution(BatchTaskExecutionId: string) {
        this.ValidationErrorsList = [];
        this.ErrorsList = [];
        this.DoneMsg = "";
        this.IsDone = false;
        var iBatchService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        iBatchService.getSingle(BatchTaskExecutionId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BatchTaskExecutionList = myResponse.Result;
                this.batchEntity = list;
                if (list.StatusCode == "D") {
                    this.StopTimer();
                    if (list.ProgressMessage != null && list.ProgressMessage.indexOf(',') > -1) {
                        this.ErrorsList = list.ProgressMessage.split(',');
                    }
                    else if (list.ProgressMessage != null && list.ProgressMessage.indexOf("continue?") > -1) {
                        var myConfirmWindow = new ConfirmWindow();
                        myConfirmWindow.Width = 400;
                        myConfirmWindow.Show(list.ProgressMessage);
                        myConfirmWindow.WindowClosed.subscribe(s => {
                            if (myConfirmWindow.Yes) {
                                var parameter = new PartnersUploadExcelParameter();
                                parameter.FileData = this.partnersUploadExcelParameter.FileData;
                                parameter.IsConfirmationByUser = true;
                                parameter.FileName = this.partnersUploadExcelParameter.FileName;
                                parameter.DocumentId = this.partnersUploadExcelParameter.DocumentId;
                                parameter.ComputingPartnerCode = this.partnersUploadExcelParameter.ComputingPartnerCode;
                                this.SendExcelToServer(parameter);
                            }
                        });
                    }
                    else {
                        this.DoneMsg = list.ProgressMessage;
                        this.IsDone = true;
                    }
                }

                else if (list.StatusCode == "F") {
                    this.StopTimer();
                    if (list.ProgressMessage != null && list.ProgressMessage.indexOf(',') > -1) {
                        this.ErrorsList = list.ProgressMessage.split(',');
                    }
                }

                else {
                    this.CurrentSession.StartBusyIndicator("Uploading Partners... " + list.ProgressPercentage + "%");
                    this.CheckBatchTaskExecution(BatchTaskExecutionId);
                }
            }

            else {
                this.StopTimer();
                var window = new MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    }
    StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
        this.IsResponseProgressVisible = false;
        this.CurrentSession.StopBusyIndicator();
    }

    CloseResponseProgressClicked() {
        this.StopTimer();
    }
}
