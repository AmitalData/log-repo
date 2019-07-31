
declare var window: any;
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit}  from '@angular/core';
import {LogLovComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogLovComponent';
import {DocumentsFilingPM} from '../../../../Common/EntityPMs/DocumentsFilingPM';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {DocumentsFilingPMService} from '../../../../Common/Services/StandardPMs/DocumentsFilingPMService';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
declare var ResultAsArray: any;

@Component({
    moduleId: module.id,

    selector: 'LogBoxUploader',
    templateUrl: './LogboxUploaderComponent.html',
    providers: [ImageLibraryService, DocumentsFilingPMService],


})

export class LogboxUploaderComponent extends BaseComponent implements OnInit {
    public ValidationErrorsList: string[];
    public DataContext: LogboxUploaderComponent = this;
    public myForm: FormGroup;
    DocumentTypeId: string;
    DependencyValue2: boolean = true;
    DataViewModel: any;
    File: any;
    externalDocs: DocumentsFilingPM[];

    CurrentDocument: DocumentsFilingPM = null;
    FileName: string;
    FileSize: string;
    FileExtension: string;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    FileData: number;
    childEntityId: string = "";
    UploadedSuccessfully: boolean = false;
    UploadingErrorsVisibility: boolean = false;
    UploadButtonIsEnabled: boolean = true;
    IsUploadDone: boolean = false;
    UploadFileId: string;
    IsCloseButtonVisibile: boolean = false;
    IsCancelVisibile: boolean = true;
    public documentsFilingPMService: DocumentsFilingPMService;
    filterImageParameter: ImageParameter;
    EntityId: string;
    ObjectTableId: string;
    ObjectTableName: string;
    Tenant: number;
    ProgressBarId: string = Guid.newGuid();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _imageLibraryService: ImageLibraryService, fb: FormBuilder) {
        super();
        this.myForm = fb.group({});
        this.UploadFileId = Guid.NewRandomString();
        if (this.documentsFilingPMService == null) {
            this.documentsFilingPMService = new DocumentsFilingPMService();
        }

    }

    ngOnInit() {

   

    }


    ShareWithAgent: boolean = false;
    EventUpload: any;
    SetWindowArgs(args: any) {
        this.ShareWithAgent = args.ShareWithAgent;
        this.File = args.File;
        this.FileSize = args.FileSize;

    }

    SetDataContext(dataContext: any) {
            this.DataViewModel = dataContext;
            this.EntityId = this.DataViewModel.EntityId;
            this.CurrentDocument = this.DataViewModel.EntityPm;
            if (this.File) {
                this.ContinueUploading(this.File);
            }
    }



    public ShowMessage(message: string) {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }



    CloseButtonClicked() {
        this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: this.IsUploadCanceled, FileName: this.FileName });
        this.CurrentSession.CloseCurrentWindow();
    }


    CancelButtonClicked() {
        if (this.IsUploadDone) {
            this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: this.IsUploadCanceled, FileName: this.FileName });
            this.CurrentSession.CloseCurrentWindow();

        }
        else {
            if (this.IsUploadInProgress) {
                this.IsUploadCanceled = true;
                this._imageLibraryService.CancelUpload(this.CurrentDocument.DocumentId, this.CurrentDocument.Tenant).subscribe(result => {
                    this.CurrentDocument.DocumentId = null;
                    this.CurrentDocument.HasFile = false;
                    this.CurrentDocument.Received = false;
                    this.CurrentDocument.ReceivedDate = null;
                    this.CurrentDocument.ReceivedByUserId = null;
                    this.CurrentDocument.FileExtension = null;
                    this.CurrentDocument.IsRequested = true;
                    this.documentsFilingPMService.update(this.CurrentDocument).subscribe(myResult => {
                        this.IsUploadInProgress = false;
                        this.IsUploadDone = false;
                        this.IsUploadCanceled = true;
                        this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: this.IsUploadCanceled });
                        this.CurrentSession.CloseCurrentWindow();

                    });
                });
            }
            else {
                this.CurrentSession.SessionEvent.emit({ Name: "LogBoxUploader", IsUploadDone: this.IsUploadDone, IsUploadCanceled: true, FileName: this.FileName ? this.FileName.split('.')[0] : this.FileName });
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    }



    ContinueUploading(file: any) {
        if (file && file.size>0) {
            ServiceLocator.SendTotangoUserActivity("LogBox", "Upload Document");
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");

            this.UploadButtonIsEnabled = false;
            this.IsUploadInProgress = true;
            this.filterImageParameter = new ImageParameter();

            this.filterImageParameter.IsFirstTry = true;
            this.filterImageParameter.Tenant = SessionInfo.LoggedUserTenant;
            this.filterImageParameter.Extension = this.FileExtension;
            this.filterImageParameter.UploadMode = "Block";
            this.filterImageParameter.EntityId = this.CurrentDocument.Id;


                    //ServiceLocator.SendTotangoUserActivity("LogBox", "Upload Document");

            this.File = file;
            var filebuffer = null;
            this.filterImageParameter.PartsNumber = this.File.size / 100000;

            if (this.filterImageParameter.PartsNumber > 1) {
                filebuffer = this.File.slice(0, 100000);
            }
            else {
                filebuffer = this.File.slice(0, file.size);
            }



            this.filterImageParameter.FileSize = file.size;
            this.filterImageParameter.SendPartNumber = 1;
            this.filterImageParameter.BufferNumber = -1;
            this.filterImageParameter.SentSize = 0;
            this.filterImageParameter.Buffersize = 100000;
            this.filterImageParameter.IsFirstTry = true;
            this.filterImageParameter.UploadMode = "Block";
            this.filterImageParameter.FileName = this.FileName;

            this.ArrayBufferToBase64(filebuffer, this);


        }
    }

    ArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            viewmodel.filterImageParameter.Base64String = window.btoa(binary);

            viewmodel.SendBlockToServer(viewmodel.filterImageParameter);

        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);

    }




    SendBlockToServer(filter: ImageParameter) {


        this._imageLibraryService.UploadPdfFile(filter).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            var result;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    result = myResult;
                }
            }

            if (result) {

                this.IncreaseProgressBar(result);
                this.filterImageParameter = result;

                if (result.SentSize < result.FileSize && !this.IsUploadCanceled) {
                    var filebuffer = null;
                    if ((result.FileSize - result.SentSize) >= 100000) {
                        filebuffer = this.File.slice(result.SentSize, result.SentSize + 100000);
                    }
                    else {
                        filebuffer = this.File.slice(result.SentSize, result.FileSize);
                    }

                    this.ArrayBufferToBase64(filebuffer, this);
                    this.IsUploadInProgress = true;

                }
                else if (result) {

                    if (this.CurrentDocument) {
                        if (result.Name) {
                            this.CurrentDocument.DocumentId = result.Name.split('.')[0];
                        }
                        this.CurrentDocument.HasFile = true;
                        this.CurrentDocument.Received = true;
                        this.CurrentDocument.ReceivedDate = new Date();
                        this.CurrentDocument.ReceivedByUserId = SessionInfo.LoggedUserId;
                        this.CurrentDocument.FileExtension = this.FileExtension;
                        this.CurrentDocument.IsRequested = false;
                        this.CurrentDocument.IsDigitallySigned = result.isDigitallySigned;
                        this.CurrentDocument.SignersList = result.signersList;
                        this.CurrentDocument.IsSharedWithForwarder = this.ShareWithAgent;
                        this.documentsFilingPMService.update(this.CurrentDocument).subscribe(myResult => {
                            this.IsUploadDone = true;
                            this.IsUploadInProgress = false;
                            this.UploadedSuccessfully = true;
                            this.CloseButtonClicked();

                        });
                    }

                }
            
            }

        });

    }


    ProgressBarPercentText: string;
    IncreaseProgressBar(filter: ImageParameter) {

        if (!this.IsUploadCanceled) {
            var pre = 100 / filter.BlocksNumber;
            var ProgressBarValue = (filter.BufferNumber + 1) * pre;


            var elem = document.getElementById(this.ProgressBarId);
            if (elem) {

                if (ProgressBarValue == 100) {
                    elem.style.width = (ProgressBarValue - 0.6) + '%';
                    this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';

                }
                else {
                    elem.style.width = ProgressBarValue + '%';
                    this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';

                }
            }



        }


    }

}
