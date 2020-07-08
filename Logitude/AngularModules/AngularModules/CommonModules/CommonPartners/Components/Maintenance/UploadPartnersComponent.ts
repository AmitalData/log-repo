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

declare var ResultAsArray: any;

@Component({
    selector: 'UploadPartnersComponent',

    templateUrl: './UploadPartnersComponent.html',
})

export class UploadPartnersComponent extends BaseComponent implements OnDestroy {
    public DataContext = this;
    private CommonDomainService: CommonDomainService;
    ValidationErrorsList = [];
    private CurrentSession = SessionLocator.SelectedSession;

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
    SelectExcelFile(fileEvent) {

        var file = fileEvent.target.files[0];

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

            var filter = new PartnersUploadExcelParameter();
            filter.FileData = window.btoa(binary);
            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    SendExcelToServer(filter: any) {
        this.CommonDomainService.PostUploadPartnersExcelFile(filter).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.batchEntity = myResponse.Result;
                if (this.batchEntity != null) {
                    this.IsResponseProgressVisible = true;
                    this.CheckBatchTaskExecution(this.batchEntity.Id);
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
    private batchEntity: BatchTaskExecutionPM;
    private timer: any;
    public IsResponseProgressVisible: boolean = false;
    CheckBatchTaskExecution(BatchTaskExecutionId: string) {
        var iBatchService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        iBatchService.getSingle(BatchTaskExecutionId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BatchTaskExecutionList = myResponse.Result;

                if (list.StatusCode == "D") {
                    this.StopTimer();
                    var window = new MessageWindow();
                    window.Show("File uploaded successfully");
                }

                else if (list.StatusCode == "F") {
                    this.StopTimer();
                    var window = new MessageWindow();
                    window.Show("There was an error uploading excel file. Please try again later");
                }

                else {
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
    }

    CloseResponseProgressClicked() {
        this.StopTimer();
    }
}
