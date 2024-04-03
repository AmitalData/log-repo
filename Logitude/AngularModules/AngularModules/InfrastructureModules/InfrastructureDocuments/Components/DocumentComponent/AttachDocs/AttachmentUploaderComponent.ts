declare var window: any;
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, EventEmitter}  from '@angular/core';
import {DocumentsFilingPM} from '../../../../../Common/EntityPMs/DocumentsFilingPM';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {DocumentsFilingExtendedPMService} from '../../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {DocumentsFilingPMService} from '../../../../../Common/Services/StandardPMs/DocumentsFilingPMService';
import {ServiceArgs} from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ImageLibraryService} from '../../../../../Common/Services/Others/ImageLibraryService';
import {AttachmentsList} from '../DocsOut/Filters/AttachmentsList';
import {ImageParameter} from '../../../../../Infrastructure/DataContracts/ImageParameter';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
declare var attachmentUploader, ResultAsArray: any;
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {ServiceLocator} from '../../../../../Infrastructure/Locators/ServiceLocator';
import { Console } from 'console';
import { CommonDomainService } from '../../../../../Common/Services/CommonDomainService';
import { DownloadManager } from '../../../../../Infrastructure/Utilities/DownloadManager';
declare var require: any

@Component({
    
    selector: 'AttachExternal',
    templateUrl: './AttachmentUploaderComponent.html',
    providers: [DocumentsFilingExtendedPMService, ImageLibraryService, DocumentsFilingPMService],
})

export class AttachmentUploaderComponent extends BaseComponent implements OnInit {
    public ValidationErrorsList: string[];
    public DataContext: AttachmentUploaderComponent = this;
    public myForm: FormGroup;
    DocumentTypeId: string;
    DependencyValue2: boolean = true;
    File: any;
    externalDocs: DocumentsFilingPM[];
    IsUploadVisibile: boolean = false;
    CurrentDocument: DocumentsFilingPM = null;
    FileName: string;
    FileSize: string;
    FileExtension: string;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    FileData: number;
    DocumentsFilingHasFile: boolean = false;
    TiggerViewModel: any;
    childEntityId: string = "";
    IsShowProgressBar: boolean = false;
    UploadedSuccessfully: boolean = false;
    UploadingErrorsVisibility: boolean = false;
    UploadButtonIsEnabled: boolean = true;
    IsUploadDone: boolean = false;
    UploadFileId: string;
    DragDropUploadFileId: string;
    IsCloseButtonVisibile: boolean = false;
    IsCancelVisibile: boolean = true;
    public documentsFilingPMService: DocumentsFilingPMService;
    filterImageParameter: ImageParameter;
    EntityId: string;
    ObjectTableId: string;
    ObjectTableName: string;
    Tenant: number;
    RequsetPageName: string;
    ProgressBarId: string = Guid.newGuid();
    DragDropProgressBarId: string = Guid.newGuid();
    EntityNumber: string;
    ExternalEntityReference: string;
    ExternalEntityName: string;
    public IFrameURI: string = "";
    public IsPDF = false;
    public HasUploadDragDropFeature = false;
    private imageLibraryService: ImageLibraryService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor( public _imageLibraryService: ImageLibraryService, fb: FormBuilder, public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService) {
        super();
        this.myForm = fb.group({});
        this.UploadFileId = Guid.NewRandomString();
        this.DragDropUploadFileId = Guid.NewRandomString();
        if (this.documentsFilingPMService == null) {
            this.documentsFilingPMService = new DocumentsFilingPMService();
        }
        this.imageLibraryService = new ImageLibraryService();
        this.HasUploadDragDropFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "UDD")[0] ? true : false;
    }

    ngOnInit() {
     
        this._entityResourceService.getEntityResourceByTableName("DocsIn").subscribe(response=> {

        });

    }

    Entity: any;
    SetWindowArgs(args: any) {
   
        this.CurrentDocument = args.CurrentDocument;
        this.EntityId = args.EntityId;
        this.ObjectTableId = AppTool.IsNullOrEmpty(args.ObjectTableId) ? args.CurrentDocument?.ObjectTableId: args.ObjectTableId;
        this.Tenant = SessionLocator.Tenant;
        this.RequsetPageName = args.RequsetPageName;
        this.TiggerViewModel = args.TiggerViewModel;
        this.EntityNumber = args.EntityNumber;
        this.ExternalEntityName = args.ExternalEntityName;
        this.ExternalEntityReference = args.ExternalEntityReference;
        this.Entity = args.Entity;
        var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];
  
        if (table) this.ObjectTableName = table.Name;
       
        this.Run();
    }

    Run() {
      
        if (this.RequsetPageName == "SendControl") this.LoadDocumentsFiling();
       
        else {
            this.IsUploadVisibile = true;
       
        }

    }

    LoadDocumentsFiling() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._documentsFilingExtendedPMService.getDocumentsFilingsByEntityIdAndObjectTable(this.EntityId, this.childEntityId, this.ObjectTableId, "I", this.Tenant, false).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.externalDocs = myResult;
                }
            }



            this.CurrentSession.CurrentWindow.StopBusyIndicator();




        });

    }



    NextButtonClcik() {
        if (this.DocumentTypeId) {

            if (this.externalDocs != null) {
                this.CurrentDocument = this.externalDocs.filter(d=> d.DocumentTypeId == this.DocumentTypeId)[0];
                if (this.CurrentDocument == null) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
                    this._documentsFilingExtendedPMService.CreateDocumentsFiling(this.DocumentTypeId, this.EntityId, this.childEntityId, "", this.ObjectTableId, "I", this.Tenant, this.ExternalEntityName, this.ExternalEntityReference, this.EntityNumber).subscribe((res: any) => {

                        var pmResponse: ServiceResponse = res;

                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                this.CurrentDocument = myResult;
                            }
                        }
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.IsUploadVisibile = true;
                    });
                }
                else {
                    this.IsUploadVisibile = true;
                }



            }
        }
        else {
            this.ShowMessage("Please choose a document type to upload");
        }

    }
    public ShowMessage(message: string) {

        const messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }



    CloseButtonClicked() {

        if (this.RequsetPageName != "DocIn" && this.RequsetPageName!= "SharedDocument") {
            if (this.TiggerViewModel) this.TiggerViewModel.OnUploadComplete(this);
        }
        this.CurrentSession.CurrentWindow.Close("");
    }


    CancelButtonClicked() {

        if (this.IsUploadDone) {
            this.CloseButtonClicked();

        }
        else {
            if (this.IsUploadInProgress) {
                this.IsUploadCanceled = true;
                this._imageLibraryService.CancelUpload(this.CurrentDocument.Id, this.CurrentDocument.Tenant).subscribe((result:any) => {
                    this.IsUploadInProgress = false;
                    this.IsUploadDone = false;
                    this.IsUploadCanceled = true;
                    this.CloseButtonClicked();
                });
            }
            else {
                this.CloseButtonClicked();
            }
        }


    }


    onDragOver(event: any): void {
        event.preventDefault();
    }

    onDrop(event: any): void {
        if (this.UploadedSuccessfully) return;
        event.preventDefault();
        const files: File[] = event.dataTransfer.files;
        if (files[0]) this.UploadFile(event, files[0]);
    }

    OpenUpLoadFile() {
        document.getElementById(this.GetUploadElementId()).click();
    }

    private GetUploadElementId() {
        return this.HasUploadDragDropFeature ? this.DragDropUploadFileId : this.UploadFileId;
    }

    UploadFile(event: any, uploadedFile = null) {
        var file: any = uploadedFile ? uploadedFile : attachmentUploader(this.GetUploadElementId());
        //document.querySelector('#UploadFile').files[0];
        if (file && file.size>0) {

            this.FileName = file.name;

            var fileInfo = file.name.split('.');

            if (fileInfo.length > 1) {
                this.FileExtension = fileInfo[fileInfo.length-1];
            }
            else {
                this.FileExtension = fileInfo[1];
            }

            this.IsPDF = this.FileExtension.toLowerCase() == "pdf";

            this._documentsFilingExtendedPMService.GetFileSizeAndUnit(file.size).subscribe((res:any) => {

                var pmResponse: ServiceResponse = res;

                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.FileSize = myResult;
                    }
                }

                if (this.FileExtension && this.FileExtension.length > 10) {
                    this.ShowMessage("File extension should be less than or equal 10 characters");
                }
                else {
               
                    this.IsUploadVisibile = true;
                    this.IsShowProgressBar = true;
                    this.UploadButtonIsEnabled = false;
                    this.IsUploadInProgress = true;
                    this.filterImageParameter =  new ImageParameter();

                    this.filterImageParameter.IsFirstTry = true;
                    this.filterImageParameter.Tenant = SessionLocator.Tenant;
                    this.filterImageParameter.Extension = this.FileExtension;
                    this.filterImageParameter.UploadMode = "AttachmentUploader";
                    this.filterImageParameter.EntityId = this.CurrentDocument.Id;

                    ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "UploadDocsIn");

                    this.File = file;
                    var filebuffer = null;
                    var ChunkSize = 100000;
                    if (this.File.size > 2000000) {
                        ChunkSize = 1000000;
                    }
                    this.filterImageParameter.PartsNumber = this.File.size / ChunkSize;

                    if (this.filterImageParameter.PartsNumber > 1) filebuffer = this.File.slice(0, ChunkSize);
                    else filebuffer = this.File.slice(0, file.size);

                  
                    this.filterImageParameter.FileSize = file.size;
                    this.filterImageParameter.SendPartNumber = 1;
                    this.filterImageParameter.BufferNumber = -1;
                    this.filterImageParameter.SentSize = 0;
                    this.filterImageParameter.IsFirstTry = true;
                    this.filterImageParameter.FileName = this.FileName;
                    this.ArrayBufferToBase64(filebuffer, this);



                }

            });
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


        this._imageLibraryService.UploadFile(filter).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            var result= null;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    result = myResult;
                    if (result) {
                        this.filterImageParameter = result;
                        var ChunkSize = 100000;
                        if (result.FileSize > 2000000) {
                            ChunkSize = 1000000;
                        }
                        if (result.SentSize < result.FileSize && !this.IsUploadCanceled) {
                            var filebuffer = null;
                            if ((result.FileSize - result.SentSize) >= ChunkSize) {
                                filebuffer = this.File.slice(result.SentSize, result.SentSize + ChunkSize);
                            }
                            else {
                                filebuffer = this.File.slice(result.SentSize, result.FileSize);
                            }

                            this.ArrayBufferToBase64(filebuffer, this);
                            this.IsUploadInProgress = true;
                            this.IncreaseProgressBar(result);

                        }
                        else {
                            if (result.Result) {

                                if (this.CurrentDocument) {
                                    this.CurrentDocument.DocumentId = result.Result.split('.')[0];
                                    this.CurrentDocument.HasFile = true;
                                    this.CurrentDocument.Received = true;
                                    this.CurrentDocument.ReceivedDate = DateTool.GetCurrentDateTimeAsUtc();
                                    this.CurrentDocument.ReceivedByUserId = SessionLocator.LoggedUserId;
                                    this.CurrentDocument.FileExtension = this.FileExtension;
                                    this.CurrentDocument.FileSize = result.FileSize;
                                    this.CurrentDocument.FileName = this.FileName;
                                    this.CurrentDocument.ReceivedByUserName = SessionLocator.LoggedUserPM.EnglishName;
                                    this.CurrentDocument.ObjectTableName = this.ObjectTableName;

                                    if (this.RequsetPageName == "DocIn") this.CurrentDocument.IsUoloadedField = true;



                                    if (this.RequsetPageName == "DocIn" || this.RequsetPageName == "SharedDocument") {
                                        if (this.CurrentDocument.IsSharedOut) {

                                            this.CurrentDocument.IsUpdateSharedDocument = true;
                                        }
                                    }
                                    console.log("Rabaia - dublicate documentsFiling attachement")
                                    this.documentsFilingPMService.update(this.CurrentDocument).subscribe((myownResult: ServiceResponse) => {

                                        var pmResponse: ServiceResponse = myownResult;

                                        if (!pmResponse.HasError) {
                                            var myResult1 = pmResponse.Result;
                                            if (myResult1) {
                                                this.IncreaseProgressBar(result);
                                                this.CurrentDocument.IsUpdateSharedDocument = false;
                                                this.IsCloseButtonVisibile = true;
                                                this.IsCancelVisibile = false;
                                                this.IsUploadDone = true;
                                                this.IsUploadInProgress = false;
                                                this.UploadedSuccessfully = true;
                                                this.PreviewUploadedFile();
                                                if ((this.RequsetPageName == "DocIn" || this.RequsetPageName == "SharedDocument") && this.TiggerViewModel) {
                                                    if (this.RequsetPageName == "DocIn") this.TiggerViewModel.OnUploadComplete();
                                                    else if (this.RequsetPageName == "SharedDocument") this.TiggerViewModel.OnUploadComplete(this.Entity);





                                                }
                                            }
                                        }
                                        else {

                                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                                this.ShowMessage(pmResponse.ErrorsArray[0]);
                                            }

                                        }





                                    });
                                }


                            }

                        }
                    }
                }
            }
            else {


                this.IsCloseButtonVisibile = true;
                this.IsCancelVisibile = false;

                let error = "Upload file Failed"; 
                if(pmResponse.HasError && pmResponse.ErrorsArray &&pmResponse.ErrorsArray.length > 0)
                    error = error + ": " + pmResponse.ErrorsArray[0];

                this.ShowMessage(error);
                
            }

           

        });

    }

    private PreviewUploadedFile() {
        if (!this.HasUploadDragDropFeature || !this.IsPDF) return;
        let commonDomainService: CommonDomainService = new CommonDomainService();
        commonDomainService.GetFilingAttachPdfReport(this.CurrentDocument.DocumentId).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            var buffer = EntityResourceService.base64ToBufferConvertor(response.Result);
            var blob = new Blob([buffer], { type: 'application/pdf' });
            var objectURL = URL.createObjectURL(blob);
            this.IFrameURI = objectURL;
        });
    }

    DownloadDocumentFile() {
        this.imageLibraryService.DownloadFile(this.CurrentDocument.DocumentId, this.CurrentDocument.FileExtension, this.CurrentDocument.Folder, SessionLocator.Tenant).subscribe((res: any) => {
            let documentName = this.CurrentDocument.DocumentId + "*";
            documentName += (this.FileName && this.FileExtension) ? this.FileName.replace("." + this.FileExtension, "") : "UploadedFile";
            DownloadManager.DownloadPage(documentName);
        });
    }

    ProgressBarPercentText: string;
    IncreaseProgressBar(filter: ImageParameter) {

        if (!this.IsUploadCanceled) {
            var pre = 100 / filter.BlocksNumber;
            var ProgressBarValue = (filter.BufferNumber + 1) * pre;

            let progressBarElementId = this.HasUploadDragDropFeature ? this.DragDropProgressBarId : this.ProgressBarId;
            var elem = document.getElementById(progressBarElementId);
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
