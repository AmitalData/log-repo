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
declare var attachmentUploader, ResultAsArray: any;

@Component({
    moduleId: module.id,
    templateUrl: './Receiving1000Component.html',
})

export class Receiving1000Component extends BaseComponent {
    public DataContext: Receiving1000Component = this;
    public ObjectTableName: string = "GLAccount";

    public UploadFileId: string = Guid.NewRandomString();
    FileName: string;
    FileSize: string;
    FileExtension: string;
    File: any;
    fileUploadParamerter: ImageParameter;
    FileData: number;
    ProgressBarPercentText: string;
    IsShowProgressBar: boolean = false;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    Placeholder: any;

    ResponseMessage: any;
    UploadButtonIsEnabled: boolean = true;
    _DecodedLoadedString: string;

    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    _AccountingOpService: AccountingOpService;
    constructor() {
        super();
    
        this.UIProperties.SetRequired("Email", this.ObjectTableName, true);
        this.CurrentSession.StopBusyIndicator();
        this._AccountingOpService = new AccountingOpService();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OnKeyUp(key) {
        if (!AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    }
    FillErrors() {
        this.ValidationErrorsList = [];
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList = [];
            
        //}
        //else if (!FormatTool.IsEmail(this.Email)) {
        //    this.ValidationErrorsList.push("Email is not valid");
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
            this._AccountingOpService.PutSystem1000File(this.fileUploadParamerter)
                .subscribe((myServiceResponse: ServiceResponse) => {
                    console.log("[Send] Response/PutSystem1000File: ", myServiceResponse.Result);
                    var response = myServiceResponse.Result;


                    this.CurrentSession.StopBusyIndicator();
                    if (myServiceResponse.HasError) {
                        this.ValidationErrorsList = myServiceResponse.ErrorsArray;
                        //this.ShowMessage(response);
                    } else {

                        if (!AppTool.IsNullOrEmpty(response)) {
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

            if (this.FileExtension.toLowerCase() != "txt") {
                this.ShowMessage("חובה קובץ TXT");
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

    ArrayBufferToBase64(file: any, viewmodel: Receiving1000Component) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var decodedString = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                decodedString += String.fromCharCode(bytes[i]);
            }
            viewmodel._DecodedLoadedString = decodedString;
            viewmodel.fileUploadParamerter.Base64String = window.btoa(decodedString);

            viewmodel.IncreaseProgressBar(100);
            viewmodel.TryParseLocally();
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    TryParseLocally(): any {
        //throw new Error("Method not implemented.");
        if (AppTool.IsNullOrEmpty(this._DecodedLoadedString)) {
            this.ShowMessage("File Is Empty");
            return;
        }
        let headers = [];
        let aryLine = this._DecodedLoadedString.split("\n");
        //aryLine.forEach(currLine => {
        //    if (currLine.startsWith("031")) {
        //        headers.push({ 'L31': currLine, 'L32': "" });
        //    } else if (currLine.startsWith("032")) {
        //        var rec = headers[headers.length - 1];
        //        rec.L32 = currLine;
        //    }
        //});
        //if (headers.length < 0) {
        //    this.ShowMessage("Incorrect file format");
        //    return;
        //}
        this.OkButtonClicked();
    }
    IncreaseProgressBar(ProgressBarValue: number) {
        var elem = document.getElementById("myBar");
        if (ProgressBarValue == 100) {
            elem.style.width = (ProgressBarValue - 0.1) + '%';
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';

        }
        else {
            elem.style.width = ProgressBarValue + '%';
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
        }

    }


    //#endregion upload
}
