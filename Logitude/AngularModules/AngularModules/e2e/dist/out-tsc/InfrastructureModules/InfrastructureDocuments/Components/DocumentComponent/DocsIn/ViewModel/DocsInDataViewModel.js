"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var LogitudeWindow_1 = require("../../../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../../../../../Infrastructure/Utilities/Guid");
var GeneralDocumentFollowUpHelper_1 = require("../../../../../../Infrastructure/Helpers/GeneralDocumentFollowUpHelper");
var DownloadManager_1 = require("../../../../../../Infrastructure/Utilities/DownloadManager");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocsInDataViewModel = /** @class */ (function (_super) {
    __extends(DocsInDataViewModel, _super);
    function DocsInDataViewModel(currentDocument, docsInTabComponent, documentType, entityId, childEntityId, childReference, externalDocuments, objectTableId) {
        var _this = _super.call(this) || this;
        _this.FirstTime = true;
        _this.DataContext = _this;
        // Extention: string;
        _this.PageType = "DocIn";
        _this.DocumentTypeId = "";
        _this.DocumentTypeName = "";
        _this.HasFollowUp = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isUpload = false;
        _this.Key = Guid_1.Guid.newGuid();
        _this.DocsInComponent = docsInTabComponent;
        _this.DocumentType = documentType;
        _this.EntityId = entityId;
        _this.ChildEntityId = childEntityId;
        _this.ChildReference = childReference;
        _this.ExternalDocuments = externalDocuments;
        _this.Id = _this.DocumentType.Id;
        _this.Name = documentType.Name;
        _this.Code = documentType.Code;
        _this.DocumentTypeId = documentType.Id;
        _this.DocumentTypeName = documentType.Name;
        _this.CurrentDocument = currentDocument;
        if (!_this.CurrentDocument) {
            if (_this.ExternalDocuments) {
                _this.CurrentDocument = _this.ExternalDocuments.filter(function (d) { return d.DocumentTypeId == documentType.Id; })[0];
            }
        }
        if (_this.CurrentDocument) {
            _this.ReceivedByUserId = _this.CurrentDocument.ReceivedByUserId;
            _this.ReceivedByUserName = _this.CurrentDocument.ReceivedByUserName;
            _this.ExternalDocumentId = _this.CurrentDocument.Id;
            _this.FollowUpId = _this.CurrentDocument.FollowUpId;
            _this.ReceivedDate = _this.CurrentDocument.ReceivedDate;
            _this.DocumentId = _this.CurrentDocument.DocumentId;
            _this.DocumentHasFile = _this.CurrentDocument.HasFile;
            _this.SecurityId = _this.CurrentDocument.SecurityId;
            _this.FileName = _this.CurrentDocument.FileExtension ? _this.CurrentDocument.FileName + "." + _this.CurrentDocument.FileExtension : _this.CurrentDocument.FileName;
            _this.Note = _this.CurrentDocument.Notes;
            if (_this.CurrentDocument.FileExtension) {
                _this.Extention = _this.CurrentDocument.FileExtension.toUpperCase();
                _this.SetAttachedIconVisibility = true;
            }
            else
                _this.SetAttachedIconVisibility = false;
        }
        return _this;
    }
    Object.defineProperty(DocsInDataViewModel.prototype, "ReceivedDate", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.ReceivedDate;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.CurrentDocument && this.CurrentDocument.ReceivedDate != newValue) {
                this.CurrentDocument.ReceivedDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "SetAttachedIconVisibility", {
        get: function () {
            if (this.CurrentDocument && this.CurrentDocument.FileExtension) {
                this.setAttachedIconVisibility = true;
            }
            else
                this.setAttachedIconVisibility = false;
            return this.setAttachedIconVisibility;
        },
        set: function (newValue) {
            this.setAttachedIconVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "SecurityId", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.SecurityId;
            }
            else
                return "";
        },
        set: function (newValue) {
            if (this.CurrentDocument && this.CurrentDocument.SecurityId != newValue) {
                this.CurrentDocument.SecurityId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "Note", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.Notes;
            }
            else
                return "";
        },
        set: function (newValue) {
            if (this.CurrentDocument != null && this.CurrentDocument.Notes != newValue) {
                this.CurrentDocument.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "DocumentId", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.DocumentId;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.CurrentDocument != null) {
                this.CurrentDocument.DocumentId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "ExternalDocumentId", {
        get: function () {
            if (this.CurrentDocument) {
                return this.CurrentDocument.Id;
            }
            else
                return null;
        },
        set: function (newValue) {
            if (this.CurrentDocument != null) {
                this.CurrentDocument.Id = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "IsRequested", {
        get: function () {
            if (this.CurrentDocument) {
                this.isRequested = this.CurrentDocument.IsRequested;
            }
            else
                this.isRequested = false;
            return this.isRequested;
        },
        set: function (newValue) {
            if (this.CurrentDocument != null) {
                if (this.CurrentDocument.IsRequested != newValue) {
                    this.CurrentDocument.IsRequested = newValue;
                    this.SubmitChanges("Saving is required...");
                }
            }
            else {
                this.CreateDocument("IsRequested", newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "Received", {
        get: function () {
            if (this.CurrentDocument) {
                this.received = this.CurrentDocument.Received;
            }
            else
                this.received = false;
            return this.received;
        },
        set: function (newValue) {
            if (this.CurrentDocument != null) {
                this.CurrentDocument.Received = newValue;
                this.SetFollowUpAsDone();
            }
            else {
                this.CreateDocument("Received", newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocsInDataViewModel.prototype, "Exists", {
        get: function () {
            if (this.CurrentDocument) {
                this.exists = true;
            }
            else
                this.exists = false;
            return this.exists;
        },
        set: function (newValue) {
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
        },
        enumerable: true,
        configurable: true
    });
    DocsInDataViewModel.prototype.OnUploadComplete = function (event) {
        if (event === void 0) { event = null; }
        if (this.CurrentDocument != null) {
            this.FileName = this.CurrentDocument.FileName;
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
                this.DocsInComponent._documentsFilingExtendedPMService.CreateDocumentShipmentEvent(this.DocsInComponent.EntityId, "Shipment", this.FileName).subscribe(function (res) {
                });
            }
            this.DocsInComponent.CheckHasDocuments();
        }
    };
    DocsInDataViewModel.prototype.CreateDocument = function (propertyName, value) {
        var _this = this;
        if (this.CurrentDocument == null) {
            this.DocsInComponent._documentsFilingExtendedPMService.CreateDocumentsFiling(this.Id, this.DocsInComponent.EntityId, this.DocsInComponent.ChildEntityId, this.DocsInComponent.ChildEntityReference, this.DocsInComponent.ObjectTableId, "I", this.DocsInComponent.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.DocsInComponent.externalDocs.push(myResult);
                        _this.CurrentDocument = myResult;
                        var message = "";
                        switch (propertyName) {
                            case "Note":
                                _this.CurrentDocument.Notes = value;
                                break;
                            case "Received":
                                _this.CurrentDocument.Received = value;
                                _this.ReceivedDate = _this.CurrentDocument.ReceivedDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                                _this.ReceivedByUserId = _this.CurrentDocument.ReceivedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                _this.ReceivedByUserName = _this.CurrentDocument.ReceivedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                                if (_this.CurrentDocument.Received) {
                                    _this.SetFollowUpAsDone();
                                }
                                break;
                            case "IsRequested":
                                message = "Saving is required...";
                                _this.CurrentDocument.IsRequested = value;
                                break;
                            case "Upload":
                                _this.DocsInComponent.IsClickToUpload = false;
                                _this.UploadButtonClicked();
                                break;
                        }
                        _this.SubmitChanges(message);
                        _this.IsRequested = _this.CurrentDocument.IsRequested;
                        _this.Note = _this.CurrentDocument.Notes;
                        if (propertyName != "Upload") {
                            _this.Received = _this.CurrentDocument.Received;
                        }
                        _this.DocumentId = _this.CurrentDocument.DocumentId;
                        _this.SecurityId = _this.CurrentDocument.SecurityId;
                        if (_this.CurrentDocument.FileExtension) {
                            _this.Extention = _this.CurrentDocument.FileExtension.toUpperCase();
                            _this.setAttachedIconVisibility = true;
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
    };
    DocsInDataViewModel.prototype.SetFollowUpAsDone = function () {
        var generalFollowUpHelper = new GeneralDocumentFollowUpHelper_1.GeneralDocumentFollowUpHelper(this.DocsInComponent.ObjectTableName, this.EntityId, this.ChildEntityId, this.ChildReference, "DocIn", this, this.DocsInComponent.EntityPM);
        generalFollowUpHelper.MarkFollowUpAsDone();
    };
    DocsInDataViewModel.prototype.RemoveDocumentsFilingPM = function (createDocument) {
    };
    DocsInDataViewModel.prototype.SubmitChanges = function (messageLoading) {
        var _this = this;
        if (messageLoading === void 0) { messageLoading = null; }
        if (this.CurrentDocument) {
            if (messageLoading) {
                this.CurrentSession.StartBusyIndicator(messageLoading);
            }
            this.DocsInComponent.documentsFilingPMService.update(this.CurrentDocument).subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.CurrentDocument = myResult;
                    }
                }
            });
        }
    };
    DocsInDataViewModel.prototype.EditNote = function (item) {
        if (item.CurrentDocument != null) {
            item.SubmitChanges("Saving Notes..");
        }
        else {
            item.CreateDocument("Note", item.Note);
        }
    };
    DocsInDataViewModel.prototype.EditReceiveDate = function (item, value) {
        if (!this.FirstTime) {
            if (item.ReceivedDate != value) {
                item.ReceivedDate = value;
                if (value == null) {
                    this.ReceivedByUserId = null;
                    this.ReceivedByUserName = null;
                }
                else {
                    this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
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
    };
    DocsInDataViewModel.prototype.SetReceivedButtonClicked = function () {
        this.SetReceived();
    };
    DocsInDataViewModel.prototype.SetReceived = function () {
        this.Received = true;
        if (this.CurrentDocument) {
            this.ReceivedDate = this.CurrentDocument.ReceivedDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            this.ReceivedByUserId = this.CurrentDocument.ReceivedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.ReceivedByUserName = this.CurrentDocument.ReceivedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            this.SubmitChanges();
        }
    };
    DocsInDataViewModel.prototype.UndoReceived = function () {
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
    };
    DocsInDataViewModel.prototype.UploadButtonClicked = function () {
        if (!this.DocsInComponent.IsClickToUpload) {
            this.DocsInComponent.IsClickToUpload = true;
            if (this.CurrentDocument != null)
                this.ShowAttachExternal();
            else {
                this.isUpload = true;
                this.Exists = true;
                // this.DocsInComponent.IsClickToUpload = false;
            }
        }
    };
    DocsInDataViewModel.prototype.ShowAttachExternal = function () {
        //var OnCloseAttachmentUploadEvent= new EventEmitter();
        var _this = this;
        //OnCloseAttachmentUploadEvent.subscribe(($event: any) => {
        //    this.OnUploadComplete();
        //    AppTool.KillEventEmitter(OnCloseAttachmentUploadEvent);
        //});
        this.IsEnableLinkAttachExternal = false;
        var windowArgs = {};
        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.RequsetPageName = "DocIn";
        windowArgs.CurrentDocument = this.CurrentDocument;
        windowArgs.TiggerViewModel = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            _this.DocsInComponent.IsClickToUpload = false;
        });
    };
    DocsInDataViewModel.prototype.VeiwDocumentButtonClicked = function (item) {
        if (item.CurrentDocument != null) {
            DownloadManager_1.DownloadManager.DownloadPage("", item.SecurityId);
        }
    };
    DocsInDataViewModel.prototype.RefreshFileName = function (fileName, fileExtension) {
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
    };
    DocsInDataViewModel.prototype.AdditionalButtonClicked = function () {
        var _this = this;
        //  Creating Document"
        this.CurrentSession.StartBusyIndicator("Creating Document");
        this.DocsInComponent._documentsFilingExtendedPMService.CreateDocumentsFiling(this.CurrentDocument.DocumentTypeId, this.DocsInComponent.EntityId, this.DocsInComponent.ChildEntityId, this.DocsInComponent.ChildEntityReference, this.DocsInComponent.ObjectTableId, "I", this.DocsInComponent.Tenant).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.DocsInComponent.LoadDocumentsFilingPM(myResult);
                }
            }
        });
    };
    DocsInDataViewModel.prototype.RemoveDocument = function () {
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
        if (!this.Received)
            this.SetReceivedButtonVisibility = true;
    };
    return DocsInDataViewModel;
}(BaseComponent_1.BaseComponent));
exports.DocsInDataViewModel = DocsInDataViewModel;
//# sourceMappingURL=DocsInDataViewModel.js.map