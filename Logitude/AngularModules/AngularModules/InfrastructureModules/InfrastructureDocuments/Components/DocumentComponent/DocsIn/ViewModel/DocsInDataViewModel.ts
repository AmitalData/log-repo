declare var System: any;
declare var window: any;
import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {LogitudeWindow} from '../../../../../../Controls/Windows/LogitudeWindow';
import {DocumentsFilingPM} from '../../../../../../Common/EntityPMs/DocumentsFilingPM';
import {DocsInTabComponent} from '../../../../../../Infrastructure/Components/Documents/DocsInTabComponent';
import {SessionLocator} from '../../../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../../../../Infrastructure/Utilities/ServiceHelper';
import {Guid} from '../../../../../../Infrastructure/Utilities/Guid';
import {GeneralDocumentFollowUpHelper} from '../../../../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper';
import {DownloadManager} from '../../../../../../Infrastructure/Utilities/DownloadManager';
import {AppTool, DateTool} from '../../../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
export class DocsInDataViewModel extends BaseComponent{
    Name: string;
    Code: string;
    public FirstTime: boolean = true;
    FollowUpId: string;
    Id: string;
    Key: string;
    DataContext: any = this;
    public EntityId: string;
    public ChildEntityId: string;
    public ChildReference: string;
    public EntityNumber: string;
    public ExternalEntityReference: string;
    public ExternalEntityName: string;
    public ExternalDocuments: DocumentsFilingPM[];
    CurrentDocument: DocumentsFilingPM;
    public ObjectTableId: string;
    public DivSelectBackgroud: string;
    DocumentType: any;
    SetAttachedButtonVisibility: boolean;
    SetReceivedButtonVisibility: boolean;
    DownloadButtonVisibility: boolean;
    extrDocument: DocumentsFilingPM;
    public ShowFollowUp: boolean;
    FileName: string;
    Extention: string;
    public DocumentHasFile: boolean;
    //DocumentId: string;
    ReceivedByUserId: string;
    ReceivedByUserName: string;

    private receivedDate: Date;
    public get ReceivedDate() {
        if (this.CurrentDocument) {
           return this.CurrentDocument.ReceivedDate;
        }
        else return null;
    }
    public set ReceivedDate(newValue: Date) {

        if (this.CurrentDocument && this.CurrentDocument.ReceivedDate != newValue) {
            this.CurrentDocument.ReceivedDate = newValue;
        }

    }


    private setAttachedIconVisibility: boolean;
    public get SetAttachedIconVisibility() {
        if (this.CurrentDocument && this.CurrentDocument.FileExtension) {
            this.setAttachedIconVisibility = true;
        }
        else this.setAttachedIconVisibility = false;
        return this.setAttachedIconVisibility;
    }
    public set SetAttachedIconVisibility(newValue: boolean) {
        this.setAttachedIconVisibility = newValue;

    }



    get SecurityId() {
        if (this.CurrentDocument) {
            return this.CurrentDocument.SecurityId;
        }
        else return "";
    }
    set SecurityId(newValue: string) {
        if (this.CurrentDocument && this.CurrentDocument.SecurityId != newValue) {
            this.CurrentDocument.SecurityId  = newValue;

        }
    }




    private  note:string;
    public get Note() {

        if (this.CurrentDocument) {
            this.note= this.CurrentDocument.Notes;
        }
        return this.note;
    }
    public set Note(newValue: string) {

        if (this.note != newValue) {
            this.note = newValue;
            if (this.CurrentDocument != null) {
                this.CurrentDocument.Notes = this.note;
            }
        }
    }




    public get DocumentId() {
        if (this.CurrentDocument) {
            return this.CurrentDocument.DocumentId;
        } else return null;

    }
    public set DocumentId(newValue: string) {
        if (this.CurrentDocument != null) {
            this.CurrentDocument.DocumentId = newValue;
        }

    }


    public get ExternalDocumentId() {

        if (this.CurrentDocument) {
            return this.CurrentDocument.Id;
        } else return null;
        
    }
    public set ExternalDocumentId(newValue: string) {
        if (this.CurrentDocument != null) {
            this.CurrentDocument.Id = newValue;
        }

    }

    private isRequested: boolean;
    public get IsRequested() {

        if (this.CurrentDocument) {
            this.isRequested = this.CurrentDocument.IsRequested;
        }
        else this.isRequested = false;

        return this.isRequested;


    }
    public set IsRequested(newValue: boolean) {
        if (this.CurrentDocument != null) {

            if (this.CurrentDocument.IsRequested != newValue) {
                this.CurrentDocument.IsRequested = newValue;
                this.SubmitChanges("Saving is required...");
            }

        }
        else {
            this.CreateDocument("IsRequested", newValue);

        }

    }




    private received: boolean;
    public get Received() {


        if (this.CurrentDocument) {
            this.received = this.CurrentDocument.Received;
        }
        else this.received = false;

        return this.received;


    }
    public set Received(newValue: boolean) {
        if (this.CurrentDocument != null) {
            this.CurrentDocument.Received = newValue;
            this.SetFollowUpAsDone();

        }
        else {
            this.CreateDocument("Received", newValue);

        }

    }




    private exists: boolean;
    public get Exists() {


        if (this.CurrentDocument) {
            this.exists = true;
        }
        else this.exists = false;

        return this.exists;


    }
    public set Exists(newValue: boolean) {

        if (newValue) {
            if (!this.isUpload) {
                this.CreateDocument("", "");
            }
            else {
                this.isUpload = false;
                this.CreateDocument("Upload", "");
            }
        }
        else {
            if (this.CurrentDocument && this.CurrentDocument.Id) {
                this.RemoveDocumentsFilingPM(this.CurrentDocument);
                this.CurrentDocument = null;
            }

            else {

                if (this.extrDocument) {
                    this.RemoveDocumentsFilingPM(this.extrDocument);
                    this.CurrentDocument = null;
                }
            }
        }

        this.exists = newValue;


    }


    // Extention: string;

    public PageType: string = "DocIn";
    DocsInComponent: DocsInTabComponent;
    DocumentTypeId: string = "";
    DocumentTypeName: string = "";
   
    public HasFollowUp: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(currentDocument: DocumentsFilingPM, docsInTabComponent: DocsInTabComponent, documentType: any, entityId: string, childEntityId: string, childReference: string, externalDocuments: DocumentsFilingPM[], objectTableId: string, entityNumber: string, externalEntityName: string, externalEntityReference:string) {
        super();
        this.Key = Guid.newGuid();
        this.DocsInComponent = docsInTabComponent;
        this.DocumentType = documentType;
        this.EntityId = entityId;
        this.ChildEntityId = childEntityId;
        this.ChildReference = childReference;
        this.ExternalDocuments = externalDocuments;
        this.Id = this.DocumentType.Id;
        this.Name = documentType.Name;
        this.Code = documentType.Code;
        this.DocumentTypeId = documentType.Id;
        this.DocumentTypeName = documentType.Name;
        this.CurrentDocument = currentDocument;
        this.EntityNumber = entityNumber;
        this.ExternalEntityName = externalEntityName;
        this.ExternalEntityReference = externalEntityReference;
        if (!this.CurrentDocument) {

            if (this.ExternalDocuments) {
                this.CurrentDocument = this.ExternalDocuments.filter(d => d.DocumentTypeId == documentType.Id)[0];
            }
        }
        if (this.CurrentDocument) {
         
            this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId;
            this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName;
            this.ExternalDocumentId = this.CurrentDocument.Id;
            this.FollowUpId = this.CurrentDocument.FollowUpId;
            this.ReceivedDate = this.CurrentDocument.ReceivedDate;
            this.DocumentId = this.CurrentDocument.DocumentId;
            this.DocumentHasFile = this.CurrentDocument.HasFile;

                this.SecurityId = this.CurrentDocument.SecurityId;
                this.FileName = this.CurrentDocument.FileExtension ? this.CurrentDocument.FileName+"." + this.CurrentDocument.FileExtension : this.CurrentDocument.FileName;
               this.Note = this.CurrentDocument.Notes;


            if (this.CurrentDocument.FileExtension) {
                this.Extention = this.CurrentDocument.FileExtension.toUpperCase();
                this.SetAttachedIconVisibility = true;
            }

            else this.SetAttachedIconVisibility = false;

        }


    }

    public OnUploadComplete(event:any=null) {
     
        if (this.CurrentDocument != null) {
   
            this.FileName =this.CurrentDocument.FileName;

            this.DocsInComponent.DeleteAttachmentButtonEnable = true;
            if (this.CurrentDocument.FileExtension) {
                this.Extention = this.CurrentDocument.FileExtension.toUpperCase();
                this.SetAttachedIconVisibility = true;
            }
            else {
                this.SetAttachedIconVisibility = false;
            }

            this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId;
            this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName;
            this.ReceivedDate = this.CurrentDocument.ReceivedDate;
            this.DocumentHasFile = true;
            this.SetAttachedButtonVisibility = false;
            this.SetReceivedButtonVisibility = false;
            this.DownloadButtonVisibility = true;
            this.Received = this.CurrentDocument.Received;
            if (this.DocsInComponent.ObjectTableName == "Shipment") {
                this.DocsInComponent._documentsFilingExtendedPMService.CreateDocumentShipmentEvent(this.DocsInComponent.EntityId, this.FileName,"DOUP").subscribe((res:any) => {

                });
            }
         
            this.DocsInComponent.CheckHasDocuments();

            if (this.CurrentSession.CurrentEditComponent) this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

        }


    }

    CreateDocument(propertyName: string, value: any) {

        if (this.CurrentDocument == null) {
            this.DocsInComponent._documentsFilingExtendedPMService.CreateDocumentsFiling(this.Id, this.DocsInComponent.EntityId, this.DocsInComponent.ChildEntityId, this.DocsInComponent.ChildEntityReference, this.DocsInComponent.ObjectTableId, "I", this.DocsInComponent.Tenant,this.ExternalEntityName, this.ExternalEntityReference, this.EntityNumber).subscribe((res:any) => {


                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.DocsInComponent.externalDocs.push(myResult);
                        this.CurrentDocument = myResult;
                        var message = "";
                        switch (propertyName) {

                            case "Note":
                                this.CurrentDocument.Notes = value;
                                break;

                          

                            case "Received":
                                this.CurrentDocument.Received = value;
                                this.ReceivedDate = this.CurrentDocument.ReceivedDate = DateTool.GetCurrentDateAsUtc();
                                this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId = SessionLocator.LoggedUserId;
                                this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName = SessionLocator.LoggedUserPM.EnglishName;

                                if (this.CurrentDocument.Received) {
                                    this.SetFollowUpAsDone();
                                }
                                break;

                            case "IsRequested":
                                message = "Saving is required...";
                                this.CurrentDocument.IsRequested = value;
                                break;


                            case "Upload":

                                this.DocsInComponent.IsClickToUpload = false;
                                this.UploadButtonClicked();
                                break;
                        }

                        this.SubmitChanges(message);

                        this.IsRequested = this.CurrentDocument.IsRequested;
                        this.Note = this.CurrentDocument.Notes;
                        if (propertyName != "Upload") {
                            this.Received = this.CurrentDocument.Received;
                        }
                        this.DocumentId = this.CurrentDocument.DocumentId;
                        this.SecurityId = this.CurrentDocument.SecurityId;


                        if (this.CurrentDocument.FileExtension) {
                            this.Extention = this.CurrentDocument.FileExtension.toUpperCase();
                            this.setAttachedIconVisibility = true;
                        }
                    }

                }

            });
        }
        else {
            switch (propertyName) {
                case "Note":
                    this.CurrentDocument.Notes = value;
                    break;

                case "ReceivedDate":
                   // this.CurrentDocument.ReceivedDate = value;
                    break;

                case "Received":
                    this.CurrentDocument.Received = value;
                    if (this.CurrentDocument.Received) {
                        this.SetFollowUpAsDone();
                    }
                    break;

                case "IsRequested":
                    this.CurrentDocument.IsRequested = value;
                    break;

            }
            this.SubmitChanges();
            //SetReceived();

        }

    }


    SetFollowUpAsDone() {

        var generalFollowUpHelper: GeneralDocumentFollowUpHelper = new GeneralDocumentFollowUpHelper(this.DocsInComponent.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildReference, "DocIn", this, this.DocsInComponent.EntityPM);
        generalFollowUpHelper.MarkFollowUpAsDone();
    }

    RemoveDocumentsFilingPM(createDocument: any) {


    }

    SubmitChanges(messageLoading: any = null) {

        if (this.CurrentDocument) {

            if (messageLoading) {
                this.CurrentSession.StartBusyIndicator(messageLoading);
            }
            this.DocsInComponent.documentsFilingPMService.update(this.CurrentDocument).subscribe((res:any) => {
                this.CurrentSession.StopBusyIndicator();
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.CurrentDocument = myResult;
                    }
                }


            });

        }
    }
    


    EditNote(item: DocsInDataViewModel) {
        if (item.CurrentDocument != null) {
                item.SubmitChanges("Saving Notes..");
        }
        else {
            item.CreateDocument("Note", item.Note);

        }
    }


    EditReceiveDate(item: DocsInDataViewModel, value) {

        if (!this.FirstTime) {
            if (item.ReceivedDate != value) {
                item.ReceivedDate = value;
                if (value == null) {
                    this.ReceivedByUserId = null;
                    this.ReceivedByUserName = null;
                }
                else {
                    this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId = SessionLocator.LoggedUserId;
                    this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName = SessionLocator.LoggedUserPM.EnglishName;

                }
                if (item.CurrentDocument != null) {
                    item.SubmitChanges("Saving ReceivedDate..");
                }
                else {
                    item.CreateDocument("ReceivedDate", item.ReceivedDate);

                }
            }
        }
        else
            this.FirstTime = false;
    }



    SetReceivedButtonClicked() {

        this.SetReceived();


    }

    public SetReceived() {

        this.Received = true;

        if (this.CurrentDocument) {
            this.ReceivedDate = this.CurrentDocument.ReceivedDate = DateTool.GetCurrentDateAsUtc();
            this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId = SessionLocator.LoggedUserId;
            this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            this.SubmitChanges();
        }


    }


    public UndoReceived() {


        this.Received = false;
        this.ReceivedDate = null;
        this.ReceivedByUserId = null;
        this.ReceivedByUserName = null;

        if (this.CurrentDocument) {
            //this.CurrentDocument.ReceivedDate = null;
            this.CurrentDocument.ReceivedByUserId = null;
            this.CurrentDocument.ReceivedByUserName = null;
            this.SubmitChanges();
        }

    }




    isUpload: boolean = false;

    UploadButtonClicked() {


        if (!this.DocsInComponent.IsClickToUpload) {
            this.DocsInComponent.IsClickToUpload = true;
            if (this.CurrentDocument != null) this.ShowAttachExternal();
            else {
                this.isUpload = true;
                this.Exists = true;
               // this.DocsInComponent.IsClickToUpload = false;
            }

        }
    }

    IsEnableLinkAttachExternal: boolean;

    ShowAttachExternal() {
 
        //var OnCloseAttachmentUploadEvent= new EventEmitter();


        //OnCloseAttachmentUploadEvent.subscribe(($event: any) => {

        //    this.OnUploadComplete();
        //    AppTool.KillEventEmitter(OnCloseAttachmentUploadEvent);

        //});

        this.IsEnableLinkAttachExternal = false;
        var windowArgs: any = {};
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.RequsetPageName = "DocIn";
        windowArgs.CurrentDocument = this.CurrentDocument;
        windowArgs.TiggerViewModel = this;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
        
            this.DocsInComponent.IsClickToUpload = false;
        });



    }

    VeiwDocumentButtonClicked(item: DocsInDataViewModel) {

        if (item.CurrentDocument != null) {
  
            DownloadManager.DownloadPage("",item.SecurityId);
        }
    }





      RefreshFileName(fileName: string, fileExtension: string) {
          if (this.CurrentDocument != null) {
              this.CurrentDocument.FileName = fileName;
              this.CurrentDocument.FileExtension = fileExtension;
              this.DocsInComponent.DeleteAttachmentButtonEnable = true;
              this.FileName = this.CurrentDocument.FileName;

//this.CurrentDocument.FileExtension ? this.CurrentDocument.FileName + this.CurrentDocument.FileExtension : this.CurrentDocument.FileName;
              if (this.CurrentDocument.FileExtension) {
                  this.Extention = this.CurrentDocument.FileExtension.toUpperCase();
                  this.SetAttachedIconVisibility = true;
              }
              else {
                  this.SetAttachedIconVisibility = false;
              }

        }

    }



    AdditionalButtonClicked() {
        //  Creating Document"
        this.CurrentSession.StartBusyIndicator("Creating Document");
        this.DocsInComponent._documentsFilingExtendedPMService.CreateDocumentsFiling(this.CurrentDocument.DocumentTypeId, this.DocsInComponent.EntityId, this.DocsInComponent.ChildEntityId, this.DocsInComponent.ChildEntityReference, this.DocsInComponent.ObjectTableId, "I", this.DocsInComponent.Tenant, this.ExternalEntityName, this.ExternalEntityReference, this.EntityNumber).subscribe((res:any) => {

            this.CurrentSession.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.DocsInComponent.LoadDocumentsFilingPM(myResult);
                }

            }



        });

    }

    RemoveDocument() {
        this.CurrentDocument = null;
        this.DocumentId = null;
        this.ExternalDocumentId = null;
        this.FileName = null;
        this.Extention = null;
        this.DocumentHasFile = false;
        this.SetAttachedIconVisibility = false;
        this.DownloadButtonVisibility = false;
        this.SetReceivedButtonVisibility = false;
        this.SetAttachedButtonVisibility = true;
        
        if (!this.Received) this.SetReceivedButtonVisibility = true;

    }



}
