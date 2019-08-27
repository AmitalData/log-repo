"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var DocumentsFilingPM_1 = require("../../../../Common/EntityPMs/DocumentsFilingPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var UIProperties_1 = require("../../../../Infrastructure/Components/LogitudeComponents/UIProperties");
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../../../Infrastructure/DataContracts/ServiceArgs");
var forms_1 = require("@angular/forms");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var DocumentsFilingPMService_1 = require("../../../../Common/Services/StandardPMs/DocumentsFilingPMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var GeneralEmailSender_1 = require("../../../../Infrastructure/Helpers/GeneralEmailSender");
var AttachmentsList_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList");
var DocumentTypeMetaDataExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService");
var DocumentsFilingMetaDataValuePM_1 = require("../../../../Common/EntityPMs/DocumentsFilingMetaDataValuePM");
var DocumentTypeListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var LogBoxSignatureClientService_1 = require("../../../../Shipment/Services/Others/LogBoxSignatureClientService");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AddEditImporterDocumentComponent = /** @class */ (function () {
    function AddEditImporterDocumentComponent(Fb) {
        this.EntityPm = new DocumentsFilingPM_1.DocumentsFilingPM();
        this.DataContext = this;
        this.IsNewDocument = true;
        this.UIProperties = this.EntityPm.UIProperties;
        this.AgentLable = "Agent";
        this.ShareAsDefault = false;
        this.OrigionalShareAsDefault = false;
        this.UploadFileId = Guid_1.Guid.NewRandomString();
        this.IsPDF = false;
        this.IFrameURI = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsPrivateLabel = false;
        this.ShowTypes = false;
        this.description = "";
        this.metaDataVisibility = false;
        this.fileStatusText = "Choose a file";
        this.ShowNote = false;
        this.IsEmptyDocumentCreated = false;
        this.metaDataVisibile = false;
        this.issharedWithAgentButtonEnabled = true;
        this.IsOkButtonClicked = false;
        this.IsUploadDone = false;
        this.IsUploadCanceled = false;
        this.FirstTimeUpload = true;
        //Uploader
        this.IsShareWithAgent = false;
        this.IsOpenWidnow = false;
        this.SelectedValue = "";
        this.SelectedName = "";
        this.TypeSelected = false;
        this.messageWindow = new MessageWindow_1.MessageWindow();
        this._documentExtendedService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this._documentsFilingPMService = new DocumentsFilingPMService_1.DocumentsFilingPMService();
        this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService_1.DocumentTypeMetaDataExtendedService();
        this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        this.myForm = Fb.group({});
        this._DocumentTypeListService = new DocumentTypeListExtendedService_1.DocumentTypeListExtendedService();
        this.MyInActiveFilter = new ApiQueryFilters_1.ApiQueryFilters();
        this.MyInActiveFilter.Filter1Name = "InActive";
        this.MyInActiveFilter.Filter1Operator = "Equals";
        this.MyInActiveFilter.Filter1Value = false;
        this._LogBoxSignatureClientService = new LogBoxSignatureClientService_1.LogBoxSignatureClientService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
    }
    AddEditImporterDocumentComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            this.AgentLable = SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.IsPrivateLabel = true;
        }
        this.ShareAsDefault = SessionLocator_1.SessionLocator.TenantPM.DocumentShareAsDefault;
        this.OrigionalShareAsDefault = SessionLocator_1.SessionLocator.TenantPM.DocumentShareAsDefault;
        this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", true);
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
            this.UIProperties.SetRequired("Description", "DocumentsFiling", true);
        }
        this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (res.Name == "LogBoxUploader") {
                _this.IsUploadCanceled = res.IsUploadCanceled;
                _this.IsUploadDone = res.IsUploadDone;
                if (_this.IsPDF && _this.IsUploadDone == true) {
                    _this.myCommonDomainService.GetFilingAttachPdfReport(_this.EntityPm.DocumentId).subscribe(function (response) {
                        if (!response.HasError) {
                            var buffer = EntityResourceService_1.EntityResourceService.base64ToBufferConvertor(response.Result);
                            var blob = new Blob([buffer], { type: 'application/pdf' });
                            var objectURL = URL.createObjectURL(blob);
                            _this.IFrameURI = objectURL;
                        }
                    });
                }
                if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPm.Description)) {
                    _this.Description = res.FileName;
                }
                //if (this.IsUploadDone == true) {
                //    this.IsSharedWithForwarder = this.ShareAsDefault;
                //}
            }
        });
        this.TopTypes = [];
        var objectTablePm = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
        this._DocumentTypeListService.getTop5DocumentTypesPMsByObjectTableAndTenant(SessionLocator_1.SessionLocator.Tenant, objectTablePm.Id).subscribe(function (res) {
            var MyType = "";
            res.Result.forEach(function (item) {
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
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPm.DocumentTypeId)) {
                //this.DocumentTypeId = res.Result[0].Id;
                _this.SelectedValue = "";
                _this.ShowTypes = false;
            }
            else {
                if (res.Result.filter(function (a) { return a.Id == _this.EntityPm.DocumentTypeId; }).length == 0) {
                    _this.SelectedValue = "O";
                    _this.ShowTypes = false;
                    _this.TypeSelected = true;
                    _this.SelectedName = _this.EntityPm.DocumentTypeName;
                }
                else {
                    var myTempData = res.Result.filter(function (a) { return a.Id == _this.EntityPm.DocumentTypeId; })[0];
                    _this.DocumentTypeId = myTempData.Id;
                    _this.SelectedValue = "";
                    _this.ShowTypes = false;
                    _this.TypeSelected = true;
                    _this.SelectedName = myTempData.Name;
                }
            }
            _this.TopTypes = res.Result;
        });
    };
    AddEditImporterDocumentComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.ShipmentList = args.SelectedShipment;
        this.IsNewDocument = args.IsNewDocument;
        if (args.EntityPm) {
            this.EntityPm = args.EntityPm;
            if (this.IsNewDocument == false && this.EntityPm.HasFile == true && this.EntityPm.FileExtension.toLowerCase() == "pdf") {
                this.IsPDF = true;
                if (this.IsPDF) {
                    this.myCommonDomainService.GetFilingAttachPdfReport(this.EntityPm.DocumentId).subscribe(function (response) {
                        if (!response.HasError) {
                            var buffer = EntityResourceService_1.EntityResourceService.base64ToBufferConvertor(response.Result);
                            var blob = new Blob([buffer], { type: 'application/pdf' });
                            var objectURL = URL.createObjectURL(blob);
                            _this.IFrameURI = objectURL;
                        }
                    });
                }
                else {
                    this.IsPDF = false;
                }
            }
        }
    };
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "Code", {
        get: function () { return this.EntityPm.Code; },
        set: function (newValue) { this.EntityPm.Code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "Title", {
        get: function () {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "Description", {
        get: function () {
            this.description = this.EntityPm.Description;
            return this.description;
        },
        set: function (newValue) {
            this.EntityPm.Description = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.UIProperties.SetRequired("Description", "DocumentsFiling", true);
            }
            else {
                this.UIProperties.SetRequired("Description", "DocumentsFiling", false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "Notes", {
        get: function () { return this.EntityPm.Notes; },
        set: function (newValue) {
            this.EntityPm.Notes = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "DocumentType", {
        get: function () { return this.documentType; },
        set: function (newValue) {
            this.documentType = newValue;
            if (newValue) {
                this.DocumentTypeId = newValue.Id;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "MetaDataVisibility", {
        get: function () { return this.metaDataVisibility; },
        set: function (newValue) {
            this.metaDataVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "DocumentTypeId", {
        get: function () {
            //if (this.TopTypes.filter(a => a.Id == this.EntityPm.DocumentTypeId).length == 0) {
            //    this.SelectedValue = "O";
            //}
            return this.EntityPm.DocumentTypeId;
        },
        set: function (newValue) {
            var _this = this;
            this.DocumentTypeMetaData = [];
            this.documentMetaDataValueList = [];
            this.EntityPm.DocumentTypeId = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                this.UIProperties.SetRequired("DocumentTypeId", "DocumentsFiling", true);
            }
            else {
                this.UIProperties.SetRequired("DocumentTypeId", "DocumentsFiling", false);
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
                this.DocumentTypeMetaDataList = [];
                this._DocumentTypeMetaDataExtendedService.GetDocumentTypeMetaDataByDocumentTypeId(this.EntityPm.DocumentTypeId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
                    _this.DocumentTypeMetaDataList = myResult.Result;
                    _this.DocumentTypeMetaData = myResult.Result;
                    if (_this.DocumentTypeMetaDataList.length > 0) {
                        _this.MetaDataVisibility = true;
                        _this._DocumentTypeMetaDataExtendedService.GetDocumentMetaDataValuesByDocument(SessionLocator_1.SessionLocator.Tenant, _this.EntityPm.Id).subscribe(function (myResult) {
                            if (!myResult.Result || myResult.Result.length == 0) {
                                for (var i = 0; i < _this.DocumentTypeMetaDataList.length; i++) {
                                    var value = new DocumentsFilingMetaDataValuePM_1.DocumentsFilingMetaDataValuePM(_this.EntityPm);
                                    //DocumentsMetaDataTypeId = metaData.DocumentsMetaDataTypeId, Tenant = TenantContext.Current.Id, DocumentsFilingId = entityPM.Id, ChangeSetOp = ChangeSetOperation.Insert
                                    value.DocumentsMetaDataTypeId = _this.DocumentTypeMetaDataList[i].DocumentsMetaDataTypeId;
                                    value.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                    value.DocumentsFilingId = _this.EntityPm.Id;
                                    value.ChangeSetOp = "Insert";
                                    _this.DocumentTypeMetaData.filter(function (a) { return a.DocumentsMetaDataTypeId == _this.DocumentTypeMetaDataList[i].DocumentsMetaDataTypeId; })[0].DocumentsFilingMetaDataValuePM = value;
                                }
                            }
                            else {
                                _this.EntityPm.DocumentsFilingMetaDataValues.forEach(function (item) {
                                    item.UIProperties = new UIProperties_1.UIProperties;
                                    item.ChangeSetOp = "Update";
                                    item.OldEntityPM = item;
                                    _this.DocumentTypeMetaData.filter(function (a) { return a.DocumentsMetaDataTypeId == item.DocumentsMetaDataTypeId; })[0].DocumentsFilingMetaDataValuePM = item;
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
                        _this.MetaDataVisibility = false;
                    }
                });
            }
            //        if (!string.IsNullOrEmpty(importerDocumentDataViewModel.EntityPM.DocumentTypeId)) {
            //            LoadMetaData = Context.Load(Context.GetDocumentTypeMetaDataByDocumentTypeIdQuery(importerDocumentDataViewModel.EntityPM.DocumentTypeId, importerDocumentDataViewModel.EntityPM.Tenant));
            //            LoadMetaData.Completed += LoadMetaData_Completed;
            //        }
            //        DocumentMetaDataControlViewModel.LoadMetaData(DocumentMetaDataControlViewModel.documentMetaDataValueList);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "FileStatusText", {
        get: function () {
            if (this.EntityPm.HasFile) {
                return "File Uploaded";
            }
            else {
                return this.fileStatusText;
            }
        },
        set: function (newValue) {
            this.fileStatusText = newValue;
        },
        enumerable: true,
        configurable: true
    });
    AddEditImporterDocumentComponent.prototype.ShowNoteClicked = function () {
        //var element = document.getElementById(id);
        //if (element.style.display == "none") {
        //    element.style.display = "block";
        //}
        //else {
        //    element.style.display = "none";
        //}
        this.ShowNote = true;
    };
    AddEditImporterDocumentComponent.prototype.HideNoteClicked = function () {
        //var element = document.getElementById(id);
        //if (element.style.display == "none") {
        //    element.style.display = "block";
        //}
        //else {
        //    element.style.display = "none";
        //}
        this.ShowNote = false;
    };
    AddEditImporterDocumentComponent.prototype.CancelButtonClicked = function () {
        if (this.IsNewDocument && this.IsEmptyDocumentCreated) {
            this.DeleteDocumentWithFile();
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditImporterDocumentComponent.prototype.DeleteDocumentWithFile = function () {
        var _this = this;
        this._documentExtendedService.GetDocumentById(this.EntityPm.DocumentId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            var RemovedDoc = myResult.Result;
            if (RemovedDoc) {
                _this._ImageLibraryService.RemoveFile(_this.EntityPm.DocumentId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    //this._documentExtendedService.Delete(item.DocumentId, SessionLocator.Tenant).subscribe(myResult => { 
                    //    this.ReloadDocuments(); 
                    //}); 
                    _this.EntityPm.HasFile = false;
                    _this.EntityPm.FileSize = null;
                    _this.EntityPm.FileExtension = null;
                    _this.EntityPm.FileName = null;
                    _this.EntityPm.DocumentId = null;
                    _this.EntityPm.IsDeleted = true;
                    _this._documentsFilingPMService.update(_this.EntityPm).subscribe(function (myResult) {
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                });
            }
            else {
                _this.EntityPm.IsDeleted = true;
                _this._documentsFilingPMService.update(_this.EntityPm).subscribe(function (myResult) {
                    _this.CurrentSession.CloseCurrentWindow();
                });
            }
        });
    };
    AddEditImporterDocumentComponent.prototype.OnDocumentTypeChanged = function (event) {
        if (event) {
            this.DocumentType = event;
            this.TypeSelected = true;
            this.SelectedName = event.Name;
            this.ShowTypes = false;
        }
    };
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "ObjectTableId", {
        get: function () { return this.EntityPm.ObjectTableId; },
        set: function (newValue) {
            this.EntityPm.ObjectTableId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "DirectionCode", {
        get: function () { return this.EntityPm.DirectionCode; },
        set: function (newValue) {
            this.EntityPm.DirectionCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "EntityId", {
        get: function () { return this.EntityPm.EntityId; },
        set: function (newValue) {
            this.EntityPm.EntityId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "IsSharedWithForwarder", {
        get: function () { return this.EntityPm.IsSharedWithForwarder; },
        set: function (newValue) {
            this.EntityPm.IsSharedWithForwarder = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "DontAddToQueue", {
        get: function () { return this.EntityPm.DontAddToQueue; },
        set: function (newValue) {
            this.EntityPm.DontAddToQueue = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "MetaDataVisibile", {
        get: function () { return this.metaDataVisibile; },
        set: function (newValue) {
            this.metaDataVisibile = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditImporterDocumentComponent.prototype, "IssharedWithAgentButtonEnabled", {
        get: function () {
            if (this.EntityPm.IsSharedWithForwarder || (this.EntityPm.IsSharedWithCustomer == true && this.EntityPm.IsRequested == false)) {
                this.issharedWithAgentButtonEnabled = false;
                this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", false);
            }
            else {
                this.issharedWithAgentButtonEnabled = true;
                this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", true);
            }
            return this.issharedWithAgentButtonEnabled;
        },
        set: function (newValue) {
            this.issharedWithAgentButtonEnabled = newValue;
            if (newValue == true) {
                this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", true);
            }
            else {
                this.UIProperties.SetEnabled("DocumentTypeId", "DocumentsFiling", false);
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditImporterDocumentComponent.prototype.SaveChanges = function () {
        var _this = this;
        this.IsOkButtonClicked = true;
        this.EntityPm.DocumentsFilingMetaDataValues = [];
        if (this.DocumentTypeMetaDataList) {
            this.DocumentTypeMetaDataList.forEach(function (item) {
                //if (item.DocumentsFilingMetaDataValuePM && !AppTool.IsNullOrEmpty(item.DocumentsFilingMetaDataValuePM.MetaDataValue)) {
                _this.EntityPm.DocumentsFilingMetaDataValues.push(item.DocumentsFilingMetaDataValuePM);
                //}
            });
        }
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //if (errors == null)
        //{
        this.ValidationErrorsList = [];
        //}
        //Validator.TryValidateObject(importerDocumentDataViewModel.EntityPM, new ValidationContext(importerDocumentDataViewModel.EntityPM, null, null), errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Description"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
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
                            if (Tools_1.AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
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
                        this._documentExtendedService.update(this.EntityPm, true).subscribe(function (myResult) {
                            _this.CurrentSession.StopBusyIndicator();
                            _this.CurrentSession.CloseCurrentWindow();
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
                        if (Tools_1.AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
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
                    this._documentExtendedService.update(this.EntityPm, true).subscribe(function (myResult) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                    //Context.SubmitChanges().Completed += new EventHandler(SaveOp_Completed);
                }
                if (!this.IsUploadDone && !this.IsUploadCanceled) {
                    //if (!this.EntityPm.IsSharedWithForwarder) {
                    this.EntityPm.DontAddToQueue = true;
                    //}
                    this._documentExtendedService.update(this.EntityPm, true).subscribe(function (myResult) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                }
            }
        }
    };
    AddEditImporterDocumentComponent.prototype.CreateDocumentMethod = function (file) {
        var _this = this;
        this.ValidationErrorsList = [];
        var objectTablePm = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
        this.ObjectTableName = objectTablePm.Name;
        this.ObjectTableId = objectTablePm.Id;
        this.DirectionCode = "I";
        this.EntityId = this.ShipmentList.Id;
        //this.DocumentTypeId = this.DocumentTypeId;
        //this.Description = this.Description;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPm.DocumentTypeId)) {
            this._documentExtendedService.GetDocumentsFilingByDocumentType(this.EntityPm.DocumentTypeId, this.EntityPm.ObjectTableId, this.EntityPm.EntityId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var temp = pmResponse.Result;
                    if (temp != null && !temp.HasFile) {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.ValidationErrorsList.push("There already an empty document with this document type !");
                    }
                    else {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
                        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPm.DocumentTypeId)) {
                            _this.ValidationErrorsList.push(msg.replace("%FieldName", "Document Type"));
                        }
                        if (_this.ValidationErrorsList.length == 0) {
                            if (_this.CurrentSession.CurrentWindow != null) {
                                // this.CurrentSession.CurrentWindow.StartBusyIndicator("Creating...");
                            }
                            _this.IsEmptyDocumentCreated = true;
                            if (!_this.EntityPm.IsSharedWithForwarder) {
                                _this.EntityPm.DontAddToQueue = true;
                            }
                            _this.EntityPm.IsRequested = true;
                            _this.EntityPm.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            _this.EntityPm.Code = "xxx";
                            _this.EntityPm.CreatedByUserId = "xxx";
                            _this.EntityPm.OwnerId = "xxx";
                            _this._documentExtendedService.insert(_this.EntityPm, true).subscribe(function (myResult) {
                                _this.CurrentSession.StopBusyIndicator();
                                if (!myResult.HasError) {
                                    if (_this.IsOkButtonClicked) {
                                        _this.CurrentSession.CloseCurrentWindow();
                                    }
                                    else {
                                        _this.OpenUploadProgressWindow(file);
                                        // this.SetAttached();
                                    }
                                }
                                else {
                                    _this.ValidationErrorsList = myResult.ErrorsArray;
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
    };
    AddEditImporterDocumentComponent.prototype.UploadClicked = function () {
        if (this.EntityPm != null) {
            document.getElementById(this.UploadFileId).click();
        }
    };
    AddEditImporterDocumentComponent.prototype.FileUploaderOpen = function (event) {
        var file = attachmentUploader(this.UploadFileId);
        if (file) {
            this.IsOkButtonClicked = false;
            if (this.EntityPm.IsSharedWithCustomer == true && this.EntityPm.IsRequested == true) {
                if ((this.IsNewDocument && this.FirstTimeUpload)) {
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
    };
    AddEditImporterDocumentComponent.prototype.OpenUploadProgressWindow = function (file, isShareWithAgent) {
        var _this = this;
        if (isShareWithAgent === void 0) { isShareWithAgent = false; }
        if (file && file.size > 0) {
            this._documentExtendedService.GetFileSizeAndUnit(file.size).subscribe(function (res) {
                var temp = file.name.split('.');
                var fileExtension = temp[temp.length - 1];
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.FileSize = myResult;
                    }
                }
                if (fileExtension.toLowerCase() == "pdf") {
                    _this.IsPDF = true;
                }
                else {
                    _this.IsPDF = false;
                }
                if (fileExtension && fileExtension.length > 10) {
                    _this.ShowMessage("File extension should be less than or equal 10 characters");
                }
                else {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 450;
                    logitudeWindow.Height = 300;
                    var windowArgs = {};
                    windowArgs.ShareWithAgent = isShareWithAgent;
                    windowArgs.FileSize = _this.FileSize;
                    windowArgs.File = file;
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Title = "File Uploading";
                    logitudeWindow.DataContext = _this;
                    logitudeWindow.Show("./ShipmentModules/ShipmentLogBox/Components/Logbox/LogboxUploaderComponent");
                }
            });
        }
    };
    AddEditImporterDocumentComponent.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    AddEditImporterDocumentComponent.prototype.DeleteDocumentFile = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "Confirm Deletion";
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.Show("Are you sure you want to delete the file ?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                //this.StartBusyIndicator("Loading ..");
                _this._ImageLibraryService.RemoveFile(_this.EntityPm.DocumentId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                    //this._documentExtendedService.Delete(item.DocumentId, SessionLocator.Tenant).subscribe(myResult => { 
                    //    this.ReloadDocuments(); 
                    //}); 
                    _this.EntityPm.HasFile = false;
                    _this.EntityPm.FileSize = null;
                    _this.EntityPm.FileExtension = null;
                    _this.EntityPm.FileName = null;
                    _this.EntityPm.DocumentId = null;
                    _this._documentsFilingPMService.update(_this.EntityPm).subscribe(function (myResult) {
                        //this.ReloadDocuments();
                    });
                });
            }
            else {
            }
        });
    };
    AddEditImporterDocumentComponent.prototype.SendDocumentFile = function () {
        var attachment = new AttachmentsList_1.AttachmentsList();
        attachment.Tenant = SessionLocator_1.SessionLocator.Tenant;
        attachment.DocumentTypeCopyNameWithDocumentTypeName = this.EntityPm.FileName;
        attachment.FileSize = this.EntityPm.FileSize;
        attachment.ShowRemoveLink = true;
        attachment.Id = this.EntityPm.DocumentId;
        attachment.FileExtension = this.EntityPm.FileExtension;
        var attachmentsList = new Array();
        attachmentsList.push(attachment);
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender_1.GeneralEmailSender("Shipment", this.EntityPm.DocumentTypeCode, this.ShipmentList.Id, this.ShipmentList.ShipmentNumber, null, null, this.EntityPm.Id, this.EntityPm.Description, attachmentsList);
            this.EmailSender.SendMessage();
        }
    };
    AddEditImporterDocumentComponent.prototype.DownloadDocumentFile = function () {
        var _this = this;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "View Document");
        this._ImageLibraryService.DownloadFile(this.EntityPm.DocumentId, this.EntityPm.FileExtension, this.EntityPm.Folder, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var EntityNumber = "";
            if (_this.ShipmentList != null) {
                if (_this.ShipmentList.ForwarderShipmentNumber == null) {
                    EntityNumber = _this.ShipmentList.ShipmentNumber;
                }
                else {
                    EntityNumber = _this.ShipmentList.ForwarderShipmentNumber;
                }
            }
            var documentName = _this.EntityPm.DocumentId + "*" + _this.EntityPm.DocumentTypeCode + "-" + (!Tools_1.AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : _this.EntityPm.EntityId) + "-" + _this.EntityPm.Code; // +"." + CurrentDocument.Extension;
            //if (this.ShipmentList != null) {
            //    EntityNumber = this.ShipmentList.ShipmentNumber;
            //}
            //var documentName = this.EntityPm.DocumentId + "*" + this.EntityPm.DocumentTypeCode + "-" + (!AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : this.EntityPm.EntityId) + "-" + this.EntityPm.Code;// +"." + CurrentDocument.Extension;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        });
    };
    AddEditImporterDocumentComponent.prototype.ShareWithAgent = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //List < ValidationResult > errors = new List<ValidationResult>();
        //Validator.TryValidateObject(importerDocumentDataViewModel.EntityPM, new ValidationContext(importerDocumentDataViewModel.EntityPM, null, null), errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPm.Description)) {
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
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Share Document With Agent");
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
            if (this.EntityPm.IsSharedWithForwarder == true) {
                this.EntityPm.IsSharedWithForwarder = false;
                this.EntityPm.DontAddToQueue = true;
                //BlueSharedWithAgentVisibility = Visibility.Visible;
                //GraySharedWithAgentVisibility = Visibility.Collapsed;
            }
            else {
                this.EntityPm.IsSharedWithForwarder = true;
                if (Tools_1.AppTool.IsNullOrEmpty(this.ShipmentList.ForwarderShipmentNumber)) {
                    this.EntityPm.DontAddToQueue = true;
                }
                else {
                    this.EntityPm.DontAddToQueue = false;
                }
                //BlueSharedWithAgentVisibility = Visibility.Collapsed;
                //GraySharedWithAgentVisibility = Visibility.Visible;
            }
            this._documentsFilingPMService.update(this.EntityPm).subscribe(function (myResult) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.IssharedWithAgentButtonEnabled = false;
                //this.ReloadDocuments();
                //this.StopBusyIndicator();
            });
            //if (!importerDocumentDataViewModel.EntityPM.IsSharedWithForwarder) {
            //    importerDocumentDataViewModel.EntityPM.DontAddToQueue = true;
            //}
        }
    };
    AddEditImporterDocumentComponent.prototype.itemClicked = function (itemValue, Name) {
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
    };
    AddEditImporterDocumentComponent.prototype.RedxClick = function () {
        this.SelectedName = "";
        this.TypeSelected = false;
        this.DocumentTypeId = "";
        if (this.SelectedValue == "O") {
            this.ShowTypes = true;
        }
    };
    AddEditImporterDocumentComponent.prototype.itemMouseOver = function (itemValue) {
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
    };
    AddEditImporterDocumentComponent.prototype.itemMouseLeave = function (itemValue) {
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
    };
    AddEditImporterDocumentComponent.prototype.onUnShare = function () {
        this.ShareAsDefault = false;
    };
    AddEditImporterDocumentComponent.prototype.onShare = function () {
        this.ShareAsDefault = true;
    };
    AddEditImporterDocumentComponent.prototype.SetDigitallySigned = function (EntityPm) {
        var _this = this;
        if (this.IsNewDocument == true && this.IsOkButtonClicked == false) {
            //this.messageWindow.Width = 300;
            //this.messageWindow.Height = 150;
            //this.messageWindow.Title = "Warning !";
            //this.messageWindow.Message = "The document isn't saved yet, please save the file first.";
            //this.messageWindow.Show(this.messageWindow.Message);
            this.EntityPm.DontAddToQueue = true;
            this.EntityPm.SignRequestByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
            this.EntityPm.SignDueDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            var tempMin = this.EntityPm.SignDueDate.getMinutes() + 5;
            this.EntityPm.SignDueDate.setMinutes(tempMin);
            this.EntityPm.CancellSignRequest = false;
        }
        else {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            this.TimerStartDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this.RefreshTimer = setInterval(function () { return _this.RunSignBusyIndicator(false, EntityPm.Id); }, 5000); //setTimeout(() => this.CheckIfSignDone(EntityPm.Id), 2000);
            if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "LBDS")) {
                var window = new ConfirmWindow_1.ConfirmWindow();
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
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Title = "Confirm Deletion";
                        confirmWindow.Width = 450;
                        confirmWindow.Height = 190;
                        confirmWindow.YesButtonText = "Ok";
                        confirmWindow.NoButtonText = "Cancel";
                        confirmWindow.Show("Document was already sent to customs and cannot be updated , we will create a copy of it for the customs agent.");
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.RunSignBusyIndicator(true, EntityPm.Id);
                                EntityPm.DontAddToQueue = true;
                                EntityPm.SignRequestByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
                                EntityPm.SignDueDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                                var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                                EntityPm.SignDueDate.setMinutes(CurrMin);
                                EntityPm.CancellSignRequest = false;
                                _this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe(function (Result) {
                                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                                    if (Result.Result != null && Result.Result.HasError) {
                                        //this.RunSignBusyIndicator(false, EntityPm.Id);
                                        _this.EntityPm.SignRequestByUserEmail = null;
                                        _this.messageWindow.Width = 300;
                                        _this.messageWindow.Height = 150;
                                        _this.messageWindow.Title = "Warning !";
                                        _this.messageWindow.Message = Result.Result.ErrorsArray[0];
                                        _this.messageWindow.Show(_this.messageWindow.Message);
                                    }
                                    else if (Result.Result == null) {
                                        _this.messageWindow.Width = 300;
                                        _this.messageWindow.Height = 150;
                                        _this.messageWindow.Title = "Warning !";
                                        _this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                                        _this.messageWindow.Show(_this.messageWindow.Message);
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
                        EntityPm.SignRequestByUserEmail = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
                        EntityPm.SignDueDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                        var CurrMin = EntityPm.SignDueDate.getMinutes() + 5;
                        EntityPm.SignDueDate.setMinutes(CurrMin);
                        EntityPm.CancellSignRequest = false;
                        this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPm).subscribe(function (Result) {
                            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Sign Document");
                            if (Result.Result != null && Result.Result.HasError) {
                                //this.RunSignBusyIndicator(false, EntityPm.Id);
                                _this.EntityPm.SignRequestByUserEmail = null;
                                _this.messageWindow.Width = 300;
                                _this.messageWindow.Height = 150;
                                _this.messageWindow.Title = "Warning !";
                                _this.messageWindow.Message = Result.Result.ErrorsArray[0];
                                _this.messageWindow.Show(_this.messageWindow.Message);
                            }
                            else if (Result.Result == null) {
                                _this.messageWindow.Width = 300;
                                _this.messageWindow.Height = 150;
                                _this.messageWindow.Title = "Warning !";
                                _this.messageWindow.Message = "Please make sure that cloud sign app installed to your computer.";
                                _this.messageWindow.Show(_this.messageWindow.Message);
                            }
                            else {
                            }
                        });
                    }
                }
                else {
                    var window = new ConfirmWindow_1.ConfirmWindow();
                    window.Width = 450;
                    window.Height = 190;
                    window.Title = "Warning !";
                    window.YesButtonText = "Ok";
                    window.ShowNoButton = false;
                    window.Show("You can only sign PDF files ..");
                }
            }
        }
    };
    AddEditImporterDocumentComponent.prototype.CheckIfSignDone = function (DocId) {
        var _this = this;
        this._documentsFilingPMService.get(DocId).subscribe(function (res) {
            var pmResponse = res;
            if (pmResponse != null && !pmResponse.HasError) {
                var currentdocument = pmResponse.Result;
                if (currentdocument && currentdocument.IsDigitallySigned == true && currentdocument.SignRequestByUserEmail == null) {
                    _this.EntityPm.SignRequestByUserEmail = null;
                    _this.EntityPm.IsDigitallySigned = currentdocument.IsDigitallySigned;
                    _this.EntityPm.SignersList = currentdocument.SignersList;
                    _this.SendSignedDocumentToAgent(currentdocument);
                }
                else {
                }
            }
        });
    };
    AddEditImporterDocumentComponent.prototype.SendSignedDocumentToAgent = function (Document) {
        if (Document.IsSharedWithCustomer == true) {
            Document.DontAddToQueue = false;
            Document.ForwarderDocumentId = null;
            this._documentsFilingPMService.update(Document).subscribe(function (myResult) {
            });
        }
    };
    AddEditImporterDocumentComponent.prototype.CancelSignProcess = function (EntityPM) {
        var _this = this;
        EntityPM.DontAddToQueue = true;
        EntityPM.SignRequestByUserEmail = null;
        EntityPM.CancellSignRequest = true;
        this._LogBoxSignatureClientService.GetSignRequestReceived(EntityPM).subscribe(function (Result) {
            _this.RunSignBusyIndicator(false, EntityPM.Id);
        });
    };
    AddEditImporterDocumentComponent.prototype.RunSignBusyIndicator = function (IsStart, Id) {
        var _this = this;
        var MyDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc(); // new Date();
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
            this.RefreshTimer = setInterval(function () { return _this.CheckIfSignDone(Id); }, 2000);
        }
        else {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            //this.TimerStartDate = DateTool.GetCurrentDateTimeAsUtc();
            this.RefreshTimer = setInterval(function () { return _this.CheckIfSignDone(Id); }, 1000);
        }
    };
    AddEditImporterDocumentComponent = __decorate([
        core_1.Component({
            selector: 'AddEditImporterDocument',
            moduleId: module.id,
            templateUrl: './AddEditImporterDocumentComponent.html',
            providers: [http_1.Http, ServiceArgs_1.ServiceArgs, DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService],
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder])
    ], AddEditImporterDocumentComponent);
    return AddEditImporterDocumentComponent;
}());
exports.AddEditImporterDocumentComponent = AddEditImporterDocumentComponent;
//# sourceMappingURL=AddEditImporterDocumentComponent.js.map