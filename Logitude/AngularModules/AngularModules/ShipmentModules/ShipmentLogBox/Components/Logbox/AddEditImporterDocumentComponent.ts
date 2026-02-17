declare var window: any;
import {Component, Output, EventEmitter, OnInit} from '@angular/core';
import {DocumentsFilingPM} from '../../../../Common/EntityPMs/DocumentsFilingPM';
import {DocumentTypeList} from '../../../../Common/EntityLists/DocumentTypeList';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {UIProperties, UIProperty} from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {Http} from '@angular/http';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {FormGroup, FormBuilder} from '@angular/forms';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentList} from '../../../../Shipment/EntityLists/ShipmentList';
import {DocumentsFilingExtendedPMService} from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {DocumentsFilingPMService} from '../../../../Common/Services/StandardPMs/DocumentsFilingPMService'
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {GeneralEmailSender} from '../../../../Infrastructure/Helpers/GeneralEmailSender';
import {AttachmentsList} from '../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import {DocumentTypeMetaDataExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService'
import {DocumentsFilingMetaDataValuePM} from '../../../../Common/EntityPMs/DocumentsFilingMetaDataValuePM';
import {DocumentTypeMetaDataPM} from '../../../../Common/EntityPMs/DocumentTypeMetaDataPM';
import {DocumentTypeListExtendedService} from '../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogBoxSignatureClientService} from '../../../../Shipment/Services/Others/LogBoxSignatureClientService';
declare var attachmentUploader, OpenFileUploader, ResultAsArray: any;
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {CommonDomainService} from'../../../../Common/Services/CommonDomainService'; 
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'AddEditImporterDocument',
    moduleId: module.id,
    templateUrl: './AddEditImporterDocumentComponent.html',
    providers: [Http, ServiceArgs, DocumentsFilingExtendedPMService],
})

export class AddEditImporterDocumentComponent implements OnInit {
    public myForm: FormGroup;
    public EntityPm: DocumentsFilingPM = new DocumentsFilingPM();
    _ImageLibraryService: ImageLibraryService;
    DataContext: any = this;
    IsNewDocument: boolean = true;
    public UIProperties: UIProperties = this.EntityPm.UIProperties;
    ObjectTableName: string;
    AgentLable: string = "Agent";
    ShareAsDefault: boolean = false;
    OrigionalShareAsDefault: boolean = false;
    MyInActiveFilter: ApiQueryFilters;
    public UploadFileId: string = Guid.NewRandomString();
    _DocumentTypeListService: DocumentTypeListExtendedService;
    public ShipmentList: ShipmentList;
    public _documentExtendedService: DocumentsFilingExtendedPMService;
    public _documentsFilingPMService: DocumentsFilingPMService;
    public _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService;
    public _LogBoxSignatureClientService: LogBoxSignatureClientService;
    public IsPDF = false;
    private myCommonDomainService: CommonDomainService;
    TopTypes: any[];
    public IFrameURI: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(Fb: FormBuilder) {
        this._documentExtendedService = new DocumentsFilingExtendedPMService();
        this._documentsFilingPMService = new DocumentsFilingPMService();
        this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
        this._ImageLibraryService = new ImageLibraryService();
        this.myForm = Fb.group({});
        this._DocumentTypeListService = new DocumentTypeListExtendedService();
        this.MyInActiveFilter = new ApiQueryFilters();
        this.MyInActiveFilter.Filter1Name = "InActive";
        this.MyInActiveFilter.Filter1Operator = "Equals";
        this.MyInActiveFilter.Filter1Value = false;
        this._LogBoxSignatureClientService = new LogBoxSignatureClientService();
        this.myCommonDomainService = new CommonDomainService();
    }
    IsPrivateLabel: boolean = false;
    ngOnInit() {
        if (SessionLocator.PrivateLableSettings) {
            this.AgentLable = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.IsPrivateLabel = true;
        }
        this.ShareAsDefault = SessionLocator.TenantPM.DocumentShareAsDefault;
        this.OrigionalShareAsDefault = SessionLocator.TenantPM.DocumentShareAsDefault;
        this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", true);
        if (AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
            this.UIProperties.SetRequired("Description", "DocumentsFiling", true);
        }
        this.CurrentSession.SessionEvent.subscribe(res => {
            if (res.Name == "LogBoxUploader") {
                this.IsUploadCanceled = res.IsUploadCanceled;
                this.IsUploadDone = res.IsUploadDone;
                if (this.IsPDF && this.IsUploadDone == true) {
                    this.myCommonDomainService.GetFilingAttachPdfReport(this.EntityPm.DocumentId).subscribe((response: ServiceResponse) => {
                        if (!response.HasError) {
                            var buffer = EntityResourceService.base64ToBufferConvertor(response.Result);
                            var blob = new Blob([buffer], { type: 'application/pdf' });
                            var objectURL = URL.createObjectURL(blob);
                            this.IFrameURI = objectURL;
                        }
                    });
                }
                if (AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
                    this.Description = res.FileName;
                }
                //if (this.IsUploadDone == true) {
                //    this.IsSharedWithForwarder = this.ShareAsDefault;
                //}
            }
        });


        this.TopTypes = [];
        var objectTablePm = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
        this._DocumentTypeListService.getTop5DocumentTypesPMsByObjectTableAndTenant(SessionLocator.Tenant, objectTablePm.Id).subscribe(res => {
            var MyType = "";
            res.Result.forEach((item) => {
                MyType = item.Name.trim();
                var tempList = MyType.split(' ');
                var tempName = "";
                if (tempList.length == 1) {
                    tempName = tempList[0].substring(0, 3).toUpperCase();
                }
                else if (tempList.length == 2) {
                    tempName = (tempList[0].substring(0, 1) + tempList[1].substring(0, 2)).toUpperCase();
                }
                else {
                    tempName = (tempList[0].substring(0, 1) + tempList[1].substring(0, 1) + tempList[2].substring(0, 1)).toUpperCase();
                }
                item.OrderedDisplayName = tempName;

            });

            if (AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
                //this.DocumentTypeId = res.Result[0].Id;
                this.SelectedValue = "";
                this.ShowTypes = false;
            }
            else {
                if (res.Result.filter(a => a.Id == this.EntityPm.DocumentTypeId).length == 0) {
                    this.SelectedValue = "O";
                    this.ShowTypes = false;
                    this.TypeSelected = true;
                    this.SelectedName = this.EntityPm.DocumentTypeName;
                }
                else {
                    var myTempData = res.Result.filter(a => a.Id == this.EntityPm.DocumentTypeId)[0];
                    this.DocumentTypeId = myTempData.Id;
                    this.SelectedValue = "";
                    this.ShowTypes = false;
                    this.TypeSelected = true;
                    this.SelectedName = myTempData.Name;
                }
            }
            this.TopTypes = res.Result;

        });
    }
    ShowTypes: boolean = false;
    SetWindowArgs(args: any) {
        this.ShipmentList = args.SelectedShipment;
        this.IsNewDocument = args.IsNewDocument;
      
        if (args.EntityPm) {
            this.EntityPm = args.EntityPm;
            if (this.IsNewDocument == false && this.EntityPm.HasFile == true && this.EntityPm.FileExtension.toLowerCase() == "pdf") {
                this.IsPDF = true;
                if (this.IsPDF) {
                    this.myCommonDomainService.GetFilingAttachPdfReport(this.EntityPm.DocumentId).subscribe((response: ServiceResponse) => {
                        if (!response.HasError) {
                            var buffer = EntityResourceService.base64ToBufferConvertor(response.Result);
                            var blob = new Blob([buffer], { type: 'application/pdf' });
                            var objectURL = URL.createObjectURL(blob);
                            this.IFrameURI = objectURL;
                        }
                    });
                }
                else {
                    this.IsPDF = false;
                }
            }
        }

    }
    public get Code() { return this.EntityPm.Code }
    public set Code(newValue: string) { this.EntityPm.Code = newValue; }

    public get Title() {
        if (this.IsNewDocument) {
            if (this.DocumentType != null) {
                return "New " + this.DocumentType.Name + " - " + (this.Description ? this.Description : "");
            }
            else {
                return "New " + (this.Description ? this.Description : "");
            }

        }
        else {
            if (this.DocumentType != null) {
                return "Edit " + this.DocumentType.Name + " - " + (this.Description ? this.Description : "");
            }
            else {
                return "Edit " + (this.Description ? this.Description : "");
            }
        }
    }

    private description: string = "";
    public get Description() {
        this.description = this.EntityPm.Description
        return this.description;
    }
    public set Description(newValue: string) {
        this.EntityPm.Description = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetRequired("Description", "DocumentsFiling", true);
        }
        else {
            this.UIProperties.SetRequired("Description", "DocumentsFiling", false);
        }
    }

    public get Notes() { return this.EntityPm.Notes }
    public set Notes(newValue: string) {
        this.EntityPm.Notes = newValue;
    }

    //public get DocumentTypeName() { return this.EntityPm.DocumentTypeName }
    //public set DocumentTypeName(newValue: string) {
    //    this.EntityPm.DocumentTypeName = newValue;
    //}

    private documentType: DocumentTypeList;
    public get DocumentType() { return this.documentType }
    public set DocumentType(newValue: DocumentTypeList) {
        this.documentType = newValue;
        if (newValue) {
            this.DocumentTypeId = newValue.Id;
        }
    }

    private metaDataVisibility: boolean = false;
    public get MetaDataVisibility() { return this.metaDataVisibility }
    public set MetaDataVisibility(newValue: boolean) {
        this.metaDataVisibility = newValue;
    }

    public DocumentTypeMetaData: any[];
    private documentMetaDataValueList: any[];


    DocumentTypeMetaDataList: DocumentTypeMetaDataPM[];
    public get DocumentTypeId() {
        //if (this.TopTypes.filter(a => a.Id == this.EntityPm.DocumentTypeId).length == 0) {
        //    this.SelectedValue = "O";
        //}
        return this.EntityPm.DocumentTypeId
    }
    public set DocumentTypeId(newValue: string) {
        this.DocumentTypeMetaData = [];
        this.documentMetaDataValueList = [];
        this.EntityPm.DocumentTypeId = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetRequired("DocumentTypeId", "DocumentsFiling", true);
        }
        else {
            this.UIProperties.SetRequired("DocumentTypeId", "DocumentsFiling", false);
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
            this.DocumentTypeMetaDataList = [];
            this._DocumentTypeMetaDataExtendedService.GetDocumentTypeMetaDataByDocumentTypeId(this.EntityPm.DocumentTypeId, SessionLocator.Tenant).subscribe(myResult => {
                this.DocumentTypeMetaDataList = myResult.Result;
                this.DocumentTypeMetaData = myResult.Result;
                if (this.DocumentTypeMetaDataList.length > 0) {
                    this.MetaDataVisibility = true;
                    this._DocumentTypeMetaDataExtendedService.GetDocumentMetaDataValuesByDocument(SessionLocator.Tenant, this.EntityPm.Id).subscribe(myResult => {
                        if (!myResult.Result || myResult.Result.length == 0) {
                            for (var i = 0; i < this.DocumentTypeMetaDataList.length; i++) {
                                var value = new DocumentsFilingMetaDataValuePM(this.EntityPm);
                                //DocumentsMetaDataTypeId = metaData.DocumentsMetaDataTypeId, Tenant = TenantContext.Current.Id, DocumentsFilingId = entityPM.Id, ChangeSetOp = ChangeSetOperation.Insert
                                value.DocumentsMetaDataTypeId = this.DocumentTypeMetaDataList[i].DocumentsMetaDataTypeId;
                                value.Tenant = SessionLocator.Tenant;
                                value.DocumentsFilingId = this.EntityPm.Id;
                                value.ChangeSetOp = "Insert";
                                this.DocumentTypeMetaData.filter(a => a.DocumentsMetaDataTypeId == this.DocumentTypeMetaDataList[i].DocumentsMetaDataTypeId)[0].DocumentsFilingMetaDataValuePM = value;
                            }
                        }
                        else {
                            this.EntityPm.DocumentsFilingMetaDataValues.forEach((item) => {
                                item.UIProperties = new UIProperties;
                                item.ChangeSetOp = "Update";
                                item.OldEntityPM = item;
                                this.DocumentTypeMetaData.filter(a => a.DocumentsMetaDataTypeId == item.DocumentsMetaDataTypeId)[0].DocumentsFilingMetaDataValuePM = item;
                            });
                            //for (var i = 0; i < this.DocumentTypeMetaDataList.length; i++) {
                            //    var tempVal = myResult.Result[i];
                            //    tempVal.ChangeSetOp = "Update";
                            //    //if (temp.Mandatory) {
                            //        tempVal.UIProperties = new UIProperties;//.SetRequired("DocumentTypeId", "DocumentsFilingMetaData", false);
                            //    //}
                            //    ////DocumentsMetaDataTypeId = metaData.DocumentsMetaDataTypeId, Tenant = TenantContext.Current.Id, DocumentsFilingId = entityPM.Id, ChangeSetOp = ChangeSetOperation.Insert
                            //    //value.DocumentsMetaDataTypeId = temp[i].DocumentsMetaDataTypeId;
                            //    //value.Tenant = SessionLocator.Tenant;
                            //    //value.DocumentsFilingId = this.EntityPm.Id;
                            //    //value.ChangeSetOp = "Insert";
                            //        this.DocumentTypeMetaData.filter(a => a.DocumentsMetaDataTypeId == this.DocumentTypeMetaDataList[i].DocumentsMetaDataTypeId)[0].DocumentsFilingMetaDataValuePM = tempVal;
                            //}
                            //this.documentMetaDataValueList = myResult.Result;
                        }
                    });

                }
                else {
                    this.MetaDataVisibility = false;
                }
            });
        }
        //        if (!string.IsNullOrEmpty(importerDocumentDataViewModel.EntityPM.DocumentTypeId)) {
        //            LoadMetaData = Context.Load(Context.GetDocumentTypeMetaDataByDocumentTypeIdQuery(importerDocumentDataViewModel.EntityPM.DocumentTypeId, importerDocumentDataViewModel.EntityPM.Tenant));
        //            LoadMetaData.Completed += LoadMetaData_Completed;
        //        }
        //        DocumentMetaDataControlViewModel.LoadMetaData(DocumentMetaDataControlViewModel.documentMetaDataValueList);

    }


    private fileStatusText: string = "Choose a file";
    public get FileStatusText() {
        if (this.EntityPm.HasFile) {
            return "File Uploaded";
        }
        else {
            return this.fileStatusText;
        }
    }
    public set FileStatusText(newValue: string) {
        this.fileStatusText = newValue;
    }

    ShowNote: boolean = false;
    ShowNoteClicked() {
        //var element = document.getElementById(id);
        //if (element.style.display == "none") {
        //    element.style.display = "block";
        //}
        //else {
        //    element.style.display = "none";
        //}
        this.ShowNote = true;
    }

    HideNoteClicked() {
        //var element = document.getElementById(id);
        //if (element.style.display == "none") {
        //    element.style.display = "block";
        //}
        //else {
        //    element.style.display = "none";
        //}
        this.ShowNote = false;
    }

    CancelButtonClicked() {
        if (this.IsNewDocument && this.IsEmptyDocumentCreated) {
            this.DeleteDocumentWithFile();
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    DeleteDocumentWithFile() {
        this._documentExtendedService.GetDocumentById(this.EntityPm.DocumentId, SessionLocator.Tenant).subscribe(myResult => {
            var RemovedDoc = myResult.Result;
            if (RemovedDoc) {
                this._ImageLibraryService.RemoveFile(this.EntityPm.DocumentId, SessionLocator.Tenant).subscribe(res => {
                    //this._documentExtendedService.Delete(item.DocumentId, SessionLocator.Tenant).subscribe(myResult => { 
                    //    this.ReloadDocuments(); 
                    //}); 
                    this.EntityPm.HasFile = false;
                    this.EntityPm.FileSize = null;
                    this.EntityPm.FileExtension = null;
                    this.EntityPm.FileName = null;
                    this.EntityPm.DocumentId = null;
                    this.EntityPm.IsDeleted = true;
                    this._documentsFilingPMService.update(this.EntityPm).subscribe(myResult => {
                        this.CurrentSession.CloseCurrentWindow();
                    });
                });
            }
            else {
                this.EntityPm.IsDeleted = true;
                this._documentsFilingPMService.update(this.EntityPm).subscribe(myResult => {
                    this.CurrentSession.CloseCurrentWindow();
                });
            }
        });
    }

    OnDocumentTypeChanged(event) {
        if (event) {
            this.DocumentType = event;
            this.TypeSelected = true;
            this.SelectedName = event.Name;
            this.ShowTypes = false;
        }
    }

    ValidationErrorsList: any[];
    IsEmptyDocumentCreated: boolean = false;

    public get ObjectTableId() { return this.EntityPm.ObjectTableId }
    public set ObjectTableId(newValue: string) {
        this.EntityPm.ObjectTableId = newValue;
    }
    public get DirectionCode() { return this.EntityPm.DirectionCode }
    public set DirectionCode(newValue: string) {
        this.EntityPm.DirectionCode = newValue;
    }
    public get EntityId() { return this.EntityPm.EntityId }
    public set EntityId(newValue: string) {
        this.EntityPm.EntityId = newValue;
    }

    public get IsSharedWithForwarder() { return this.EntityPm.IsSharedWithForwarder }
    public set IsSharedWithForwarder(newValue: boolean) {
        this.EntityPm.IsSharedWithForwarder = newValue;
    }

    public get DontAddToQueue() { return this.EntityPm.DontAddToQueue }
    public set DontAddToQueue(newValue: boolean) {
        this.EntityPm.DontAddToQueue = newValue;
    }
    metaDataVisibile: boolean = false;
    public get MetaDataVisibile() { return this.metaDataVisibile }
    public set MetaDataVisibile(newValue: boolean) {
        this.metaDataVisibile = newValue;
    }


    issharedWithAgentButtonEnabled: boolean = true;
    public get IssharedWithAgentButtonEnabled() {
        if (this.EntityPm.IsSharedWithForwarder || (this.EntityPm.IsSharedWithCustomer == true && this.EntityPm.IsRequested == false)) {
            this.issharedWithAgentButtonEnabled = false;
            this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", false);
        }
        else {
            this.issharedWithAgentButtonEnabled = true;
            this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", true);
        }
        return this.issharedWithAgentButtonEnabled;
    }
    public set IssharedWithAgentButtonEnabled(newValue: boolean) {
        this.issharedWithAgentButtonEnabled = newValue;
        if (newValue == true) {
            this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", true);
        }
        else {
            this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", false);
        }
    }

    IsOkButtonClicked: boolean = false;
    IsUploadDone: boolean = false;
    IsUploadCanceled: boolean = false;
    SaveChanges() {
        this.IsOkButtonClicked = true;
        this.EntityPm.DocumentsFilingMetaDataValues = [];
        if (this.DocumentTypeMetaDataList) {
            this.DocumentTypeMetaDataList.forEach((item) => {
                //if (item.DocumentsFilingMetaDataValuePM && !AppTool.IsNullOrEmpty(item.DocumentsFilingMetaDataValuePM.MetaDataValue)) {
                this.EntityPm.DocumentsFilingMetaDataValues.push(item.DocumentsFilingMetaDataValuePM);
                //}
            });
        }

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //if (errors == null)
        //{
        this.ValidationErrorsList = [];
        //}

        //Validator.TryValidateObject(importerDocumentDataViewModel.EntityPM, new ValidationContext(importerDocumentDataViewModel.EntityPM, null, null), errors);

        if (AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Description"));
        }
        if (AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Document Type"));
        }

        if (this.ValidationErrorsList.length == 0) {
            if (this.CurrentSession.CurrentWindow != null) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            }
            if (this.IsNewDocument) {
                if (!this.IsEmptyDocumentCreated) {
                    this.CreateDocumentMethod(null);
                }
                else {
                    if (this.IsUploadCanceled) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    }
                    if (this.IsUploadDone) {
                        //if (!this.EntityPm.IsSharedWithForwarder) {
                        this.EntityPm.DontAddToQueue = true;
                        //}
                        //this.IsSharedWithForwarder = this.ShareAsDefault;
                        if (this.ShareAsDefault == true) {
                            this.EntityPm.IsSharedWithForwarder = true;
                            if (AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
                                this.EntityPm.DontAddToQueue = true;
                            }
                            else {
                                this.EntityPm.DontAddToQueue = false;
                            }
                        }
                        //else {
                        //    this.EntityPm.IsSharedWithForwarder = false;
                        //    this.EntityPm.DontAddToQueue = true;
                        //}
                        this._documentExtendedService.update(this.EntityPm, true).subscribe(myResult => {
                            this.CurrentSession.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindow();
                        });
                        //Context.SubmitChanges().Completed += new EventHandler(SaveOp_Completed);
                    }
                }
            }
            else {
                if (this.IsUploadCanceled) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();
                }
                if (this.IsUploadDone) {
                    //if (!this.EntityPm.IsSharedWithForwarder) {
                    this.EntityPm.DontAddToQueue = true;
                    //}
                    if (this.ShareAsDefault == true) {
                        this.EntityPm.IsSharedWithForwarder = true;
                        if (AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
                            this.EntityPm.DontAddToQueue = true;
                        }
                        else {
                            this.EntityPm.DontAddToQueue = false;
                        }
                    }
                    //else {
                    //    this.EntityPm.IsSharedWithForwarder = false;
                    //    this.EntityPm.DontAddToQueue = true;
                    //}
                    this._documentExtendedService.update(this.EntityPm, true).subscribe(myResult => {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    });
                    //Context.SubmitChanges().Completed += new EventHandler(SaveOp_Completed);
                }
                if (!this.IsUploadDone && !this.IsUploadCanceled) {
                    //if (!this.EntityPm.IsSharedWithForwarder) {
                    this.EntityPm.DontAddToQueue = true;
                    //}
                    this._documentExtendedService.update(this.EntityPm, true).subscribe(myResult => {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    });
                }
            }

        }
    }
    FirstTimeUpload: boolean = true;
    public CreateDocumentMethod(file: any) {
        this.ValidationErrorsList = [];
        var objectTablePm = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
        this.ObjectTableName = objectTablePm.Name;
        this.ObjectTableId = objectTablePm.Id;
        this.DirectionCode = "I";
        this.EntityId = this.ShipmentList.Id;
        //this.DocumentTypeId = this.DocumentTypeId;
        //this.Description = this.Description;
        if (!AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
            this._documentExtendedService.GetDocumentsFilingByDocumentType(this.EntityPm.DocumentTypeId, this.EntityPm.ObjectTableId, this.EntityPm.EntityId, SessionLocator.Tenant).subscribe(res => {

                var pmResponse: any = res;
                if (!pmResponse.HasError) {

                    var temp = pmResponse.Result;

                    if (temp != null && !temp.HasFile) {
                        this.CurrentSession.StopBusyIndicator();
                        this.ValidationErrorsList.push("There already an empty document with this document type !");


                    }
                    else {
                        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                        if (AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
                            this.ValidationErrorsList.push(msg.replace("%FieldName", "Document Type"));
                        }
                        if (this.ValidationErrorsList.length == 0) {

                            if (this.CurrentSession.CurrentWindow != null) {
                                // this.CurrentSession.CurrentWindow.StartBusyIndicator("Creating...");
                            }
                            this.IsEmptyDocumentCreated = true;
                            if (!this.EntityPm.IsSharedWithForwarder) {
                                this.EntityPm.DontAddToQueue = true;
                            }
                            this.EntityPm.IsRequested = true;
                            this.EntityPm.Tenant = SessionLocator.Tenant;
                            this.EntityPm.Code = "xxx";
                            this.EntityPm.CreatedByUserId = "xxx";
                            this.EntityPm.OwnerId = "xxx";

                            this._documentExtendedService.insert(this.EntityPm, true).subscribe(myResult => {
                                this.CurrentSession.StopBusyIndicator();
                                if (!myResult.HasError) {
                                    if (this.IsOkButtonClicked) {
                                        this.CurrentSession.CloseCurrentWindow();
                                    }
                                    else {
                                        this.OpenUploadProgressWindow(file);
                                        // this.SetAttached();
                                    }
                                }
                                else {
                                    this.ValidationErrorsList = myResult.ErrorsArray;
                                }
                            });



                        }
                    }



                }


                //if (this.ValidationErrorsList.length >= 0 || pmResponse.HasError) {
                //    this.EntityPm.HasError = true;
                //}
                //else this.EntityPm.HasError = false;

            });
        }

    }

    //Uploader
    IsShareWithAgent: boolean = false;
    IsOpenWidnow: boolean = false;
    UploadClicked() {
        if (this.EntityPm != null) {
            document.getElementById(this.UploadFileId).click();
        }
    }

    FileSize: any;
    FileUploaderOpen(event: any) {

        var file: any = attachmentUploader(this.UploadFileId);
        if (file) {

            this.IsOkButtonClicked = false;
            if (this.EntityPm.IsSharedWithCustomer == true && this.EntityPm.IsRequested == true) {

                if ((this.IsNewDocument && this.FirstTimeUpload) ) {
                    this.CreateDocumentMethod(file);
                    this.FirstTimeUpload = false;
                }
                else {
                    this.OpenUploadProgressWindow(file, true);
                }


            }
            else {

                if ((this.IsNewDocument && this.FirstTimeUpload)) {
                    this.CreateDocumentMethod(file);
                    this.FirstTimeUpload = false;
                }
                else {
                    this.OpenUploadProgressWindow(file);
                }

            }





        }

    }

    OpenUploadProgressWindow(file: any, isShareWithAgent: boolean = false) {

        if (file && file.size>0) {
            this._documentExtendedService.GetFileSizeAndUnit(file.size).subscribe(res => {
                var temp = file.name.split('.');
                var fileExtension: string = temp[temp.length - 1];
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.FileSize = myResult;
                    }
                }
                if (fileExtension.toLowerCase() == "pdf") {
                    this.IsPDF = true;
                }
                else {
                    this.IsPDF = false;
                }
                if (fileExtension && fileExtension.length > 10) {
                    this.ShowMessage("File extension should be less than or equal 10 characters");
                } else {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 450;
                    logitudeWindow.Height = 300;
                    var windowArgs: any = {};
                    windowArgs.ShareWithAgent = isShareWithAgent;
                    windowArgs.FileSize = this.FileSize;
                    windowArgs.File = file;
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Title = "File Uploading";
                    logitudeWindow.DataContext = this;
                    logitudeWindow.Show("./ShipmentModules/ShipmentLogBox/Components/Logbox/LogboxUploaderComponent");
                }



            });
        }

    }


    public ShowMessage(message: string) {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }


    DeleteDocumentFile() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.Show("Are you sure you want to delete the file ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                //this.StartBusyIndicator("Loading ..");
                this._ImageLibraryService.RemoveFile(this.EntityPm.DocumentId, SessionLocator.Tenant).subscribe(res => {
                    //this._documentExtendedService.Delete(item.DocumentId, SessionLocator.Tenant).subscribe(myResult => { 
                    //    this.ReloadDocuments(); 
                    //}); 
                    this.EntityPm.HasFile = false;
                    this.EntityPm.FileSize = null;
                    this.EntityPm.FileExtension = null;
                    this.EntityPm.FileName = null;
                    this.EntityPm.DocumentId = null;
                    this._documentsFilingPMService.update(this.EntityPm).subscribe(myResult => {
                        //this.ReloadDocuments();
                    });
                });
            }

            else {

            }
        });
    }


    EmailSender: GeneralEmailSender;
    SendDocumentFile() {
        var attachment = new AttachmentsList();
        attachment.Tenant = SessionLocator.Tenant;
        attachment.DocumentTypeCopyNameWithDocumentTypeName = this.EntityPm.FileName;
        attachment.FileSize = this.EntityPm.FileSize;
        attachment.ShowRemoveLink = true;
        attachment.Id = this.EntityPm.DocumentId;
        attachment.FileExtension = this.EntityPm.FileExtension;

        var attachmentsList = new Array<AttachmentsList>();
        attachmentsList.push(attachment);

        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender("Shipment", this.EntityPm.DocumentTypeCode, this.ShipmentList.Id, this.ShipmentList.ShipmentNumber, null, null, this.EntityPm.Id, this.EntityPm.Description, attachmentsList);
            this.EmailSender.SendMessage();
        }

    }

    DownloadDocumentFile() {
        ServiceLocator.SendTotangoUserActivity("LogBox", "View Document");
        this._ImageLibraryService.DownloadFile(this.EntityPm.DocumentId, this.EntityPm.FileExtension, this.EntityPm.Folder, SessionLocator.Tenant).subscribe(res => {
            var EntityNumber = "";
            if (this.ShipmentList != null) {
                if (this.ShipmentList.ForwarderShipmentNumber == null) {
                    EntityNumber = this.ShipmentList.ShipmentNumber;
                }
                else {
                    EntityNumber = this.ShipmentList.ForwarderShipmentNumber;
                }
            }
            var documentName = this.EntityPm.DocumentId + "*" + this.EntityPm.DocumentTypeCode + "-" + (!AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : this.EntityPm.EntityId) + "-" + this.EntityPm.Code;// +"." + CurrentDocument.Extension;
            //if (this.ShipmentList != null) {
            //    EntityNumber = this.ShipmentList.ShipmentNumber;
            //}
            //var documentName = this.EntityPm.DocumentId + "*" + this.EntityPm.DocumentTypeCode + "-" + (!AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : this.EntityPm.EntityId) + "-" + this.EntityPm.Code;// +"." + CurrentDocument.Extension;

            DownloadManager.DownloadPage(documentName);
        });
    }

    ShareWithAgent() {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //List < ValidationResult > errors = new List<ValidationResult>();
        //Validator.TryValidateObject(importerDocumentDataViewModel.EntityPM, new ValidationContext(importerDocumentDataViewModel.EntityPM, null, null), errors);

        if (AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Discription"));
        }
        //if (!SessionLocator.PrivateLableSettings && AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
        //    this.ValidationErrorsList.push("This Shipment is not connected to agent .");
        //}
        //bool validateEntry = ValidateEntry();
        //bool hasValidationErrors = CheckValidationErrors();

        //FillErrors(errors);

        if (this.ValidationErrorsList.length == 0) {

            //var window = new ConfirmWindow();

            //window.Title = "Confirm sharing";
            //window.Width = 450;
            //window.Height = 190;
            //window.YesButtonText = "Ok";
            //window.NoButtonText = "Cancel";
            //window.Show("Are you sure you want to share this document with agent?");
            //window.WindowClosed.subscribe((event: any) => {
            //    if (window.Yes) {
                   
            //    }

            //    else {

            //    }
            //});
            ServiceLocator.SendTotangoUserActivity("LogBox", "Share Document With Agent");
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
            if (this.EntityPm.IsSharedWithForwarder == true) {
                this.EntityPm.IsSharedWithForwarder = false;
                this.EntityPm.DontAddToQueue = true;
                //BlueSharedWithAgentVisibility = Visibility.Visible;
                //GraySharedWithAgentVisibility = Visibility.Collapsed;
            }

            else {
                this.EntityPm.IsSharedWithForwarder = true;
                if (AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
                    this.EntityPm.DontAddToQueue = true;
                }
                else {
                    this.EntityPm.DontAddToQueue = false;
                }
                //BlueSharedWithAgentVisibility = Visibility.Collapsed;
                //GraySharedWithAgentVisibility = Visibility.Visible;

            }
            this._documentsFilingPMService.update(this.EntityPm).subscribe(myResult => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.IssharedWithAgentButtonEnabled = false;
                //this.ReloadDocuments();
                //this.StopBusyIndicator();
            });

                    //if (!importerDocumentDataViewModel.EntityPM.IsSharedWithForwarder) {
                    //    importerDocumentDataViewModel.EntityPM.DontAddToQueue = true;
                    //}

        }

    }
    SelectedValue: string = "";
    SelectedName: string = "";
    TypeSelected: boolean = false;
    itemClicked(itemValue: string, Name: string) {
        if (this.DocumentTypeId != itemValue) {
            if (itemValue == "O") {
                this.ShowTypes = true;
                this.SelectedValue = "O";
                this.DocumentTypeId = "";
                this.TypeSelected = false;
                this.SelectedName = "";
            }
            else {
                this.DocumentTypeId = itemValue;
                this.ShowTypes = false;
                this.SelectedValue = itemValue;
                this.TypeSelected = true;
                this.SelectedName = Name;
            }
            //var img_A = document.getElementById("TransportFilter_A");
            //var img_O = document.getElementById("TransportFilter_O");
            //var img_I = document.getElementById("TransportFilter_I");
            //img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            //img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            //img_I.setAttribute("src", "./Images/TransportModes/I_G.png");

            //switch (itemValue) {
            //    case "A": {
            //        img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
            //        break;
            //    }

            //    case "O": {
            //        img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
            //        break;
            //    }

            //    case "I": {
            //        img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
            //        break;
            //    }
            //}
        }
    }

    RedxClick() {
        this.SelectedName = "";
        this.TypeSelected = false;
        this.DocumentTypeId = "";
        if (this.SelectedValue == "O") {
            this.ShowTypes = true;
        }
    }
    itemMouseOver(itemValue: string) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");

        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A.png");
        //            break;
        //        }

        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O.png");
        //            break;
        //        }

        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I.png");
        //            //img_I.style.top = "1px";
        //            break;
        //        }
        //    }
        //}
    }
    itemMouseLeave(itemValue: string) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");

        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
        //            break;
        //        }

        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
        //            break;
        //        }

        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
        //            break;
        //        }
        //    }
        //}
    }

    onUnShare() {
        this.ShareAsDefault = false;
    }

    onShare() {
        this.ShareAsDefault = true;
    }

    TimerStartDate: Date;
    private messageWindow: MessageWindow = new MessageWindow();
    RefreshTimer: any;
    SetDigitallySigned(EntityPm) {//id 
        if (this.IsNewDocument == true && this.IsOkButtonClicked == false) {
            //this.messageWindow.Width = 300;
            //this.messageWindow.Height = 150;
            //this.messageWindow.Title = "Warning !";
            //this.messageWindow.Message = "The document isn't saved yet, please save the file first.";
            //this.messageWindow.Show(this.messageWindow.Message);
            this.EntityPm.DontAddToQueue = true;
            this.EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
            this.EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
            var tempMin = this.EntityPm.SignDueDate.getMinutes() + 5;
            this.EntityPm.SignDueDate.setMinutes(tempMin);
            this.EntityPm.CancellSignRequest = false;

        }
        else {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.RefreshTimer = setInterval(() => this.RunSignBusyIndicator(false, EntityPm.Id), 5000);//setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);

            if (!FeatureLocator.HasFeaturePermession("General", "LBDS")) {
                var window = new ConfirmWindow();
                window.Width = 450;
                window.Height = 190;
                window.Title = "You have no permession";
                window.YesButtonText = "Ok";
                window.ShowNoButton = false;
                window.Show("Your package doesn't include this module..");
            }
            else {
                if (EntityPm.FileExtension.toLowerCase() == "pdf") {
                    if (EntityPm.IsCustomReference == true) {
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Title = "Confirm Deletion";
                        confirmWindow.Width = 450;
                        confirmWindow.Height = 190;
                        confirmWindow.YesButtonText = "Ok";
                        confirmWindow.NoButtonText = "Cancel";
                        confirmWindow.Show("Document was already sent to customs and cannot be updated , we will create a copy of it for the customs agent.");
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.RunSignBusyIndicator(true, EntityPm.Id);
                                EntityPm.DontAddToQueue = true;
                                EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
                                EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
                                var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                                EntityPm.SignDueDate.setMinutes(CurrMin);
                                EntityPm.CancellSignRequest = false;
                                this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe(Result => {
                                    ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                                    if (Result.Result != null && Result.Result.HasError) {
                                        //this.RunSignBusyIndicator(false, EntityPm.Id);
                                        this.EntityPm.SignRequestByUserEmail = null;
                                        this.messageWindow.Width = 300;
                                        this.messageWindow.Height = 150;
                                        this.messageWindow.Title = "Warning !";
                                        this.messageWindow.Message = Result.Result.ErrorsArray[0];
                                        this.messageWindow.Show(this.messageWindow.Message);
                                    }
                                    else if (Result.Result == null) {
                                        this.messageWindow.Width = 300;
                                        this.messageWindow.Height = 150;
                                        this.messageWindow.Title = "Warning !";
                                        this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                                        this.messageWindow.Show(this.messageWindow.Message);
                                    }
                                    else {

                                    }

                                });
                            }
                        });
                    }
                    else {
                        this.RunSignBusyIndicator(true, EntityPm.Id);
                        EntityPm.DontAddToQueue = true;
                        EntityPm.SignRequestByUserEmail = SessionLocator.LoggedUserPM.Email;
                        EntityPm.SignDueDate = DateTool.GetCurrentDateTimeAsUtc();
                        var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                        EntityPm.SignDueDate.setMinutes(CurrMin);
                        EntityPm.CancellSignRequest = false;
                        this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe(Result => {
                            ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                            if (Result.Result != null && Result.Result.HasError) {
                                //this.RunSignBusyIndicator(false, EntityPm.Id);
                                this.EntityPm.SignRequestByUserEmail = null;
                                this.messageWindow.Width = 300;
                                this.messageWindow.Height = 150;
                                this.messageWindow.Title = "Warning !";
                                this.messageWindow.Message = Result.Result.ErrorsArray[0];
                                this.messageWindow.Show(this.messageWindow.Message);
                            }
                            else if (Result.Result == null) {
                                this.messageWindow.Width = 300;
                                this.messageWindow.Height = 150;
                                this.messageWindow.Title = "Warning !";
                                this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                                this.messageWindow.Show(this.messageWindow.Message);
                            }
                            else {

                            }

                        });
                    }
                }
                else {
                    var window = new ConfirmWindow();
                    window.Width = 450;
                    window.Height = 190;
                    window.Title = "Warning !";
                    window.YesButtonText = "Ok";
                    window.ShowNoButton = false;
                    window.Show("You can only sign PDF files ..");
                }
            }
        } 
    }

    CheckIfSignDone(DocId: string) {
        this._documentsFilingPMService.get(DocId).subscribe(res => {
            var pmResponse: any = res;
            if (pmResponse != null && !pmResponse.HasError) {
                var currentdocument = pmResponse.Result;
                if (currentdocument && currentdocument.IsDigitallySigned == true && currentdocument.SignRequestByUserEmail == null) {
                    this.EntityPm.SignRequestByUserEmail = null;
                    this.EntityPm.IsDigitallySigned = currentdocument.IsDigitallySigned;
                    this.EntityPm.SignersList = currentdocument.SignersList;
                    this.SendSignedDocumentToAgent(currentdocument);
                    
                }
               
                else {
                }
            }
        });
    }

    SendSignedDocumentToAgent(Document: any) {
        if (Document.IsSharedWithCustomer == true) {

            Document.DontAddToQueue = false;
            Document.ForwarderDocumentId = null;
            this._documentsFilingPMService.update(Document).subscribe(myResult => {

            });
        }
    }
    CancelSignProcess(EntityPM) { 
        EntityPM.DontAddToQueue = true;
        EntityPM.SignRequestByUserEmail = null;
        EntityPM.CancellSignRequest = true; 
        this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPM).subscribe(Result => {
            this.RunSignBusyIndicator(false, EntityPM.Id);
        });
    }

    RunSignBusyIndicator(IsStart: boolean, Id: string) {
        var MyDate: Date = DateTool.GetCurrentDateTimeAsUtc();// new Date();
        if (this.TimerStartDate && (MyDate.getMinutes() > (this.TimerStartDate.getMinutes() + 5))) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
        }
        else if (this.TimerStartDate && (MyDate.getMinutes() > (this.TimerStartDate.getMinutes() + 1))) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            //this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.RefreshTimer = setInterval(() => this.CheckIfSignDone(Id), 2000);
        }
        else {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            //this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.RefreshTimer = setInterval(() => this.CheckIfSignDone(Id), 1000);
        }
    }

}
