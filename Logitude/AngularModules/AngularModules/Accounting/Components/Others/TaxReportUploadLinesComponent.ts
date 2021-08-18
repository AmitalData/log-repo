import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnDestroy } from '@angular/core';

import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../Infrastructure/Tools';
import { TaxReportLineExtendedListService } from '../../Services/ExtendedLists/TaxReportLineExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { TaxReportPM } from '../../EntityPMs/TaxReportPM';
declare var attachmentUploader, ResultAsArray: any;
declare var window;

@Component({
    selector: 'TaxReportUploadLinesComponent',

    templateUrl: './TaxReportUploadLinesComponent.html',
})
export class TaxReportUploadLinesComponent extends BaseComponent {
    public DataContext:any  = this;

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
    entityPM: TaxReportPM;
    ResponseMessage: any;
    UploadButtonIsEnabled: boolean = true;
    _DecodedLoadedString: string;

    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    taxReportLineExtendedListService: TaxReportLineExtendedListService;
    constructor() {
        super();
        this.CurrentSession.StopBusyIndicator();
        this.taxReportLineExtendedListService = new TaxReportLineExtendedListService();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    SetWindowArgs(args: any) {
        if (args != null) {
            this.entityPM = args.EntityPM;
        }
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
        this.CreateTaxReportLines();
    }

    CreateTaxReportLines() {
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this.taxReportLineExtendedListService.PostCreateTaxReportLines(this.fileUploadParamerter)
                .subscribe((myServiceResponse: ServiceResponse) => {
                    console.log("[Send] Response/PostCreateTaxReportLines: ", myServiceResponse.Result);
                    var response = myServiceResponse.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (myServiceResponse.HasError) {
                        this.ValidationErrorsList = myServiceResponse.ErrorsArray;                     
                    }
                    else {
                        if (!AppTool.IsNullOrEmpty(response)) {
                            this.ShowMessage(response.Message);
                            this.CancelButtonClicked();
                        }
                    }
                });
        }

    }
    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    OpenUpLoadFile() {
        document.getElementById(this.UploadFileId).click();
    }
    file: any;
    UploadFile(event: any) {
        this.file = attachmentUploader(this.UploadFileId);
        if (this.file) {
            var temp = this.file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = this.file.name.replace("." + this.FileExtension, "");
            this.ValidateFile();        
            if (this.valid) {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;
                this.MapFileParameters();
                this.ArrayBufferToBase64(this.file, this);
            }
            }
        }
    
    valid: boolean = true;
    ValidateFile() {
        if (this.FileExtension.toLowerCase() != "txt") {
            this.ShowMessage("חובה קובץ TXT");
            return;
        }
        if (this.FileExtension && this.FileExtension.length > 10) {
            this.ShowMessage("File extension should be less than or equal 10 characters");
            this.valid = false;
        }
    }
    MapFileParameters() {
        this.fileUploadParamerter = new ImageParameter();
        this.fileUploadParamerter.Key = Guid.newGuid();
        this.fileUploadParamerter.IsFirstTry = true;
        this.fileUploadParamerter.Extension = this.FileExtension;
        this.fileUploadParamerter.UploadMode = "Block";
        this.fileUploadParamerter.FileSize = this.file.size;
        this.fileUploadParamerter.Tenant = SessionLocator.Tenant;
        this.fileUploadParamerter.EntityId = this.entityPM.Id;
    }
    ArrayBufferToBase64(file: any, viewmodel: TaxReportUploadLinesComponent) {
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

        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
  
  
}
