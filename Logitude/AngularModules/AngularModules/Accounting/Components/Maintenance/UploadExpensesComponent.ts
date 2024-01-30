import { Component, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../Infrastructure/Tools';
import { AccountingOpService } from '../../Services/Others/AccountingOpService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { GLAccountPMService } from 'Accounting/Services/StandardPMs/GLAccountPMService';
import { EditTasks } from 'e2e/CRM/Activities/EditEntity/EditTask';
declare var attachmentUploader, ResultAsArray: any;

@Component({

    templateUrl: './UploadExpensesComponent.html',
})

export class UploadExpensesComponent extends BaseComponent {
    public DataContext: UploadExpensesComponent = this;
    public UploadFileId: string = Guid.NewRandomString();
    FileName: string;
    FileExtension: string;
    File: any;
    fileUploadParamerter: ImageParameter;
    FileData: number;
    ProgressBarPercentText: string;
    IsShowProgressBar: boolean = false;
    UploadButtonIsEnabled: boolean = true;

    public ValidationErrorsList: string[];
    public RowsValidationErrorsList: string[];
    public SuccessRow: string;
    private CurrentSession = SessionLocator.SelectedSession;
    _GLAccountPMService: GLAccountPMService;
    constructor() {
        super();
        this.CurrentSession.StopBusyIndicator();
        this._GLAccountPMService = new GLAccountPMService();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {
        this.ValidationErrorsList = [];
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this.ValidationErrorsList = [];
        } else {
            this.ValidationErrorsList.push("טען קובץ לפני העלאה ");
        }
    }

    OkButtonClicked() {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this._GLAccountPMService.UpdateFromCsv(this.fileUploadParamerter)
                .subscribe((myServiceResponse: ServiceResponse) => {
                    console.log("[Send] Response/UpdateFromCsv: ", myServiceResponse.Result);
                    var response = myServiceResponse.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (myServiceResponse.HasError) {
                        this.ValidationErrorsList = myServiceResponse.ErrorsArray;
                    } else {
                        this.RowsValidationErrorsList = [];
                        if (response.UpdateCounter == 0) {
                            this.RowsValidationErrorsList.push("No rows were updated.");
                        } else {
                            this.SuccessRow = response.UpdateCounter + " rows were updated."
                        }
                        if (response.Errors != null && response.Errors.length > 0) {
                            let RowsValidationErrorsList = response.Errors.map(err => {
                                return "Error in line: " + err.LineNumber + ", Display Number: " + err.DisplayNumber + " ,Error Message: " + err.Error;
                            });
                            this.RowsValidationErrorsList = this.RowsValidationErrorsList.concat(RowsValidationErrorsList);
                        }
                        if (!AppTool.IsNullOrEmpty(response.Message)) {
                            this.ShowMessage(response.Message);
                            this.CancelButtonClicked();
                        }
                    }
                });
        }
    }

    //#region upload
    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    OpenUpLoadFile() {
        document.getElementById(this.UploadFileId).click();
    }

    UploadFile(event: any) {
        var file: any = attachmentUploader(this.UploadFileId);
        if (file) {
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");

            if (this.FileExtension.toLowerCase() != "csv") {
                this.ShowMessage("חובה קובץ csv");
                return;
            }

            this.File = file;

            if (this.FileExtension && this.FileExtension.length > 10) {
                this.ShowMessage("File extension should be less than or equal 10 characters");
            }
            else {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;

                this.fileUploadParamerter = new ImageParameter();
                this.fileUploadParamerter.Key = Guid.newGuid();
                this.fileUploadParamerter.IsFirstTry = true;
                this.fileUploadParamerter.Extension = this.FileExtension;
                this.fileUploadParamerter.UploadMode = "Block";
                this.fileUploadParamerter.FileSize = file.size;
                this.fileUploadParamerter.Tenant = SessionLocator.Tenant;

                this.ArrayBufferToBase64(file, this);
            }
        }
    }

    ArrayBufferToBase64(file: any, viewmodel: UploadExpensesComponent) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var decodedString = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                decodedString += String.fromCharCode(bytes[i]);
            }
            viewmodel.fileUploadParamerter.Base64String = window.btoa(decodedString);
            viewmodel.IncreaseProgressBar(100);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    IncreaseProgressBar(ProgressBarValue: number) {
        var elem = document.getElementById("myBar") as HTMLProgressElement;
        if (ProgressBarValue == 100) {
            elem.value = ProgressBarValue;
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';
            elem.dataset['label'] = this.ProgressBarPercentText;
        }
        else {
            elem.value = ProgressBarValue;
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
            elem.dataset['label'] = this.ProgressBarPercentText;
        }
    }
}
