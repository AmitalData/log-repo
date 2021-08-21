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
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
declare var attachmentUploader, ResultAsArray: any;
declare var window;

@Component({
    selector: 'TaxReportUploadLinesComponent',

    templateUrl: './TaxReportUploadLinesComponent.html',
})
export class TaxReportUploadLinesComponent extends BaseComponent {
    public DataContext:any  = this;

    public UploadFileId: string = Guid.NewRandomString();
   public  FileName: string;
    fileSize: string;
    fileExtension: string;
    file: any;
    fileUploadParamerter: ImageParameter;
    public FileData: number;
    progressBarPercentText: string;
    public IsShowProgressBar: boolean = false;
    isUploadCanceled: boolean;
    isUploadInProgress: boolean;
    placeholder: any;
    entityPM: TaxReportPM;
    responseMessage: any;
    public UploadButtonIsEnabled: boolean = true;
    decodedLoadedString: string;

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

    CheckIfThereIsUploadedFile() {
        this.ValidationErrorsList = [];
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {           
            this.ValidationErrorsList = [];

        } else {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("TaxReport.O.UploadFile"));
        }
    }
    OkButtonClicked() {
        this.CheckIfThereIsUploadedFile();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CreateTaxReportLines();
    }

    CreateTaxReportLines() {
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this.taxReportLineExtendedListService.PostCreateTaxReportLinesByTextFile(this.fileUploadParamerter)
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

    OpenUploadFile() {
        document.getElementById(this.UploadFileId).click();
    }
  
    UploadFile(event: any) {
        this.file = attachmentUploader(this.UploadFileId);
        if (this.file) {
            var temp = this.file.name.split('.');
            this.fileExtension = temp[temp.length - 1];
            this.FileName = this.file.name.replace("." + this.fileExtension, "");
            this.ValidateFile();        
            if (this.valid) {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;
                this.CreateImageParameters();
                this.ArrayBufferToBase64(this.file, this);
            }
            else { this.UploadButtonIsEnabled = true;}
            }
        }
    
    valid: boolean = true;
    ValidateFile() {
        if (this.fileExtension.toLowerCase() != FileType.TextFile) {
            this.valid = false;
            this.ShowMessage(TextCodeTranslator.Translate("TaxReport.O.TextFileAllowed"));
        }
        else this.valid = true;
       
    }
    CreateImageParameters() {
        this.fileUploadParamerter = new ImageParameter();
        this.fileUploadParamerter.Key = Guid.newGuid();
        this.fileUploadParamerter.IsFirstTry = true;
        this.fileUploadParamerter.Extension = this.fileExtension;
        this.fileUploadParamerter.FileSize = this.file.size;
        this.fileUploadParamerter.Tenant = SessionLocator.Tenant;
        this.fileUploadParamerter.EntityId = this.entityPM.Id;
        this.fileUploadParamerter.UploadMode = UploadMode.BlockMode;
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
            viewmodel.decodedLoadedString = decodedString;
            viewmodel.fileUploadParamerter.Base64String = window.btoa(decodedString);

        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
  
  
}

enum FileType {
    TextFile = "txt",
}
enum UploadMode {
    BlockMode="Block",
}
