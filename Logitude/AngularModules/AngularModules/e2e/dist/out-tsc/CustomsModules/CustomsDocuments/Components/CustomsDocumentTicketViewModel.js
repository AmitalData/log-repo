"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsDocumentPM_1 = require("../../../Customs/EntityPMs/CustomsDocumentPM");
var CustomsDocumentMetaDataValuePM_1 = require("../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CustomDocumentTypeMetaDataListService_1 = require("../../../Customs/Services/StandardLists/CustomDocumentTypeMetaDataListService");
var CustomDocumentTypeListService_1 = require("../../../Customs/Services/StandardLists/CustomDocumentTypeListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var RelatedDocumentViewModel_1 = require("./RelatedDocumentViewModel");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var CustomsDocumentPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentPMService");
var CustomsDocumentsTicketPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentsTicketPMService");
var CustomsDocumentsTicketsExtendedService_1 = require("../../../Customs/Services/ExtendedPMs/CustomsDocumentsTicketsExtendedService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var CustDocRelatedDocsWebService_1 = require("../../../Customs/Services/WebServices/CustDocRelatedDocsWebService");
var CustDocMetaDataValuesWebService_1 = require("../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsDocumentTicketViewModel = /** @class */ (function () {
    function CustomsDocumentTicketViewModel(customsDocumentsTicketPM, customsDocumentMetaDataValuePMs, isNew, isDisplayOnly, EntityPM, objectTableName, iCustomsDocumentsController) {
        var _this = this;
        this.customsDocumentsTicketPM = customsDocumentsTicketPM;
        this.isNew = isNew;
        this.isDisplayOnly = isDisplayOnly;
        this.EntityPM = EntityPM;
        this.objectTableName = objectTableName;
        this.iCustomsDocumentsController = iCustomsDocumentsController;
        this.FromCompanyDocumentType2Add = false;
        this.metaDataList = [];
        this.PreventEdit = false;
        this._SInvoiceNumber = null;
        this._IsClassified = false;
        //*****************************************//
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ListOfStatusCode2Show = ["1", "2"];
        this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        if (customsDocumentMetaDataValuePMs != null) {
            this.customsDocumentMetaDataValuePMs = customsDocumentMetaDataValuePMs.filter(function (d) { return d.CustomsDocumentId == customsDocumentsTicketPM.DocumentsFilingId; });
        }
        var customDocumentTypeListService = new CustomDocumentTypeListService_1.CustomDocumentTypeListService();
        customDocumentTypeListService.getAllFromCache().subscribe(function (resp) {
            _this.customsDocumentTypeLists = resp.Result;
            var customDocumentType = _this.customsDocumentTypeLists.filter(function (d) { return d.Code == _this.customsDocumentsTicketPM.DocumentTypeCode; })[0];
            _this.DocumentTypeName = customDocumentType.LocalName;
        });
        this.DocumentStatusName = this.customsDocumentsTicketPM.DocumentStatusName;
        this.CustomsDocIdLabelText = this.GetCustomsDocIdLabelText();
        this.SetStatusImages();
        this.SetApprovedDeniedImages();
        if (!this.isNew) {
            this.SetCustomDocumentMetaData();
        }
        if (customsDocumentsTicketPM.DocumentTypeCode == "380" && !Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.ConnectedInvoicesSequences)) {
            var dec = EntityPM;
            if (dec) {
                var ary = this.customsDocumentsTicketPM.ConnectedInvoicesSequences.split(",");
                var firstSeq = ary[0];
                var sp = dec.SupplierInvoices.filter(function (r) { return !Tools_1.AppTool.IsNullOrEmpty(r.SequenceNumeric) && r.SequenceNumeric.toString() == firstSeq; })[0];
                if (sp) {
                    this._SInvoiceNumber = sp.InvoiceNumber;
                    if (ary.length > 1) {
                        this._SInvoiceNumber = this._SInvoiceNumber + "...";
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(customsDocumentsTicketPM.DocumentsFilingId)) {
                        var custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
                        custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(customsDocumentsTicketPM.DocumentsFilingId).
                            subscribe(function (resp) {
                            var documentFiling = resp.Result;
                            if (documentFiling.DocumentTypeCode == "CLSI") {
                                _this._IsClassified = true;
                            }
                        });
                    }
                }
            }
        }
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(function (response) {
            CustomsDocumentTicketViewModel.Customs_Claim_TH_CustomAnswer = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Claim.TH.CustomAnswer");
        });
    }
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "Id", {
        //****************Properties****************//
        get: function () { return this.customsDocumentsTicketPM.Id; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.Id != value) {
                this.customsDocumentsTicketPM.Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "DocumentTypeCode", {
        get: function () { return this.customsDocumentsTicketPM.DocumentTypeCode; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.DocumentTypeCode != value) {
                this.customsDocumentsTicketPM.DocumentTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "CustomsDocId", {
        get: function () { return this.customsDocumentsTicketPM.CustomsDocId; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.CustomsDocId != value) {
                this.customsDocumentsTicketPM.CustomsDocId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "RequestedDocumentId", {
        get: function () { return this.customsDocumentsTicketPM.RequestedCustomsDocId; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.RequestedCustomsDocId != value) {
                this.customsDocumentsTicketPM.RequestedCustomsDocId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "DocumentsFilingId", {
        get: function () { return this.customsDocumentsTicketPM.DocumentsFilingId; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.DocumentsFilingId != value) {
                this.customsDocumentsTicketPM.DocumentsFilingId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "Extension", {
        get: function () { return this.customsDocumentsTicketPM.Extension; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.Extension != value) {
                this.customsDocumentsTicketPM.Extension = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "ExternalAttachmentId", {
        get: function () { return this.customsDocumentsTicketPM.ExternalAttachmentId; },
        set: function (value) {
            if (this.customsDocumentsTicketPM.ExternalAttachmentId != value) {
                this.customsDocumentsTicketPM.ExternalAttachmentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "ExternalAttachmentIdVisibility", {
        get: function () {
            if (this.ExternalAttachmentId) {
                return true;
            }
            else {
                return false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "HaveCustomAnswer", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentStatusCode) ||
                Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentsFilingId) ||
                //entityPm.DocumentStatusCode!= "1" 
                this.ListOfStatusCode2Show.indexOf(this.customsDocumentsTicketPM.DocumentStatusCode) == -1) {
                return false;
            }
            return true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentTicketViewModel.prototype, "CustomAnswerTitle", {
        get: function () {
            if (this.HaveCustomAnswer) {
                return CustomsDocumentTicketViewModel.Customs_Claim_TH_CustomAnswer; //
            }
            else {
                return TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Edit");
                ;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomsDocumentTicketViewModel.prototype.SetCustomDocumentMetaData = function (metaData) {
        var _this = this;
        if (metaData === void 0) { metaData = null; }
        var customDocumentTypeMetaDataListService = new CustomDocumentTypeMetaDataListService_1.CustomDocumentTypeMetaDataListService();
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomDocumentTypeMetaData").subscribe(function (response) {
            customDocumentTypeMetaDataListService.getAllFromCache().subscribe(function (res) {
                _this.customDocumentTypeMetaDataLists = res.Result;
                if (metaData == null) {
                    _this.SetCustomDocumentMetaDataFromAll();
                }
                else {
                    _this.SetCustomDocumentMetaDataFromDictionary(metaData);
                }
            });
        });
    };
    CustomsDocumentTicketViewModel.prototype.SetCustomDocumentMetaDataFromDictionary = function (metaData) {
        var _this = this;
        if (metaData === void 0) { metaData = {}; }
        this.metaDataList = [];
        var keys = Object.keys(metaData);
        keys.forEach(function (key) {
            var my = _this.customDocumentTypeMetaDataLists.filter(function (d) { return d.MetaDataTypeCode == key; })[0];
            var viewmodel = new MetaDataValueViewModel();
            viewmodel.MetaDataValue = metaData[key] != null ? metaData[key] + "" : null;
            viewmodel.Tenant = SessionLocator_1.SessionLocator.Tenant;
            viewmodel.MetaDataTypeCode = key;
            if (my != null) {
                viewmodel.IsLeading = my.IsLeading;
                viewmodel.MetaDataTypeName = my.MetaDataTypeName;
            }
            _this.metaDataList.push(viewmodel);
        });
        var leading = this.metaDataList.filter(function (d) { return d.IsLeading; })[0];
        if (leading) {
            this.LeadingMetaDataValue = leading.MetaDataValue == "True" ? "כן" : (leading.MetaDataValue == "False" ? "לא" : leading.MetaDataValue);
            this.LeadingMetaDataName = leading.MetaDataTypeName;
        }
        else if (this.customDocumentTypeMetaDataLists != null) {
            {
                var types = this.customDocumentTypeMetaDataLists.filter(function (d) { return d.DocumentTypeCode == _this.customsDocumentsTicketPM.DocumentTypeCode; }).sort(function (a, b) { return (a.MetaDataTypeCode === b.MetaDataTypeCode) ? 0 : (a.MetaDataTypeCode < b.MetaDataTypeCode) ? -1 : 1; });
                ;
                var leadingType = types.filter(function (d) { return d.Mandatory && d.DocumentTypeCode == _this.customsDocumentsTicketPM.DocumentTypeCode; })[0];
                if (leadingType) {
                    var leadingValue = this.metaDataList.filter(function (d) { return d.MetaDataTypeCode == leadingType.MetaDataTypeCode; })[0];
                    this.LeadingMetaDataValue = leadingValue != null ? leadingValue.MetaDataValue : null;
                    this.LeadingMetaDataName = leadingType.MetaDataTypeName;
                }
            }
        }
    };
    CustomsDocumentTicketViewModel.prototype.SetCustomDocumentMetaDataFromAll = function () {
        var _this = this;
        this.customDocumentTypeMetaDataLists = this.customDocumentTypeMetaDataLists.filter(function (d) { return d.DocumentTypeCode === _this.customsDocumentsTicketPM.DocumentTypeCode; });
        if (this.customDocumentTypeMetaDataLists) {
            var requiredMetaDatas = this.customDocumentTypeMetaDataLists.filter(function (d) { return d.Mandatory && d.DocumentTypeCode === _this.customsDocumentsTicketPM.DocumentTypeCode; });
            var counter = 0;
            if (this.customsDocumentMetaDataValuePMs == null) {
                counter = requiredMetaDatas.length;
            }
            else {
                requiredMetaDatas.forEach(function (item) {
                    var value = _this.customsDocumentMetaDataValuePMs.filter(function (d) { return d.MetaDataTypeCode == item.MetaDataTypeCode; })[0];
                    if (value && Tools_1.AppTool.IsNullOrEmpty(value.MetaDataValue)) {
                        counter++;
                    }
                    else if (value == null || value == undefined) {
                        counter++;
                    }
                });
            }
            this.MetaDataCount = counter;
            if (this.MetaDataCount > 0) {
                this.IsMetaDataVisible = true;
            }
            else {
                this.IsMetaDataVisible = false;
            }
        }
        if (this.customsDocumentMetaDataValuePMs && this.customDocumentTypeMetaDataLists) {
            var leading = this.customDocumentTypeMetaDataLists.filter(function (d) { return d.IsLeading && d.DocumentTypeCode == _this.customsDocumentsTicketPM.DocumentTypeCode; })[0];
            if (leading) {
                var leadingValue = this.customsDocumentMetaDataValuePMs.filter(function (d) { return d.MetaDataTypeCode == leading.MetaDataTypeCode; })[0];
                this.LeadingMetaDataValue = leadingValue != null ? (leadingValue.MetaDataValue == "True" ? "כן" : (leadingValue.MetaDataValue == "False" ? "לא" : leadingValue.MetaDataValue)) : null;
                this.LeadingMetaDataName = leading.MetaDataTypeName;
            }
            else if (this.customDocumentTypeMetaDataLists != null) {
                var types = this.customDocumentTypeMetaDataLists.filter(function (d) { return d.DocumentTypeCode == _this.customsDocumentsTicketPM.DocumentTypeCode; }).sort(function (a, b) { return (a.MetaDataTypeCode === b.MetaDataTypeCode) ? 0 : (a.MetaDataTypeCode < b.MetaDataTypeCode) ? -1 : 1; });
                ;
                // var leadingType: CustomDocumentTypeMetaDataList = types.filter(d => d.Mandatory && d.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode)[0];
                leading = types.filter(function (d) { return d.Mandatory && d.DocumentTypeCode == _this.customsDocumentsTicketPM.DocumentTypeCode; })[0];
                if (leading != null) {
                    var leadingValue = this.customsDocumentMetaDataValuePMs.filter(function (d) { return d.MetaDataTypeCode == leading.MetaDataTypeCode; })[0];
                    this.LeadingMetaDataValue = leadingValue != null ? (leadingValue.MetaDataValue == "True" ? "כן" : (leadingValue.MetaDataValue == "False" ? "לא" : leadingValue.MetaDataValue)) : null;
                    this.LeadingMetaDataName = leading.MetaDataTypeName;
                }
            }
        }
    };
    CustomsDocumentTicketViewModel.prototype.GetStatusFontColor = function () {
        var customsDocIdForeground = null;
        if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "8") {
            return "#F78232";
        }
        if (this.customsDocumentsTicketPM.DocumentStatusCode == "1") {
            customsDocIdForeground = "#018057";
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "2") {
            customsDocIdForeground = "red";
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentStatusCode)) {
            customsDocIdForeground = "#881C1D";
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "7" || this.customsDocumentsTicketPM.DocumentStatusCode == "8") {
            customsDocIdForeground = "#F78232";
        }
        else {
            customsDocIdForeground = "#312C31";
        }
        return customsDocIdForeground;
    };
    CustomsDocumentTicketViewModel.prototype.GetCustomsDocIdLabelText = function () {
        var customsDocIdLabelText = null;
        if (Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentStatusCode)) { // this will cause a problem in statuses. || (AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode) && this.customsDocumentsTicketPM.RequestedCustomsDocId)) { //WI 35024
            this.DocumentStatusName = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocNotSentYet");
            customsDocIdLabelText = null; //TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocNotSentYet");
        }
        else if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "8") { //WI 35024
            this.DocumentStatusName = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocInVerificationProgress");
            customsDocIdLabelText = null;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode)) {
            // already filled in the init
            customsDocIdLabelText = null;
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "7") {
            this.DocumentStatusName = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocSendInProgress");
            customsDocIdLabelText = null; //TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocSendInProgress");
        }
        else {
            customsDocIdLabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocuments.CustomsDocIdLabel");
        }
        return customsDocIdLabelText;
    };
    CustomsDocumentTicketViewModel.prototype.SetStatusImages = function () {
        if (this.customsDocumentsTicketPM.DocumentStatusCode == "1" || this.customsDocumentsTicketPM.DocumentStatusCode == "2" || this.customsDocumentsTicketPM.DocumentStatusCode == "7") {
            this.Status1ImageGreen = true;
            this.Status1ImageGray = false;
        }
        else {
            this.Status1ImageGreen = false;
            this.Status1ImageGray = true;
        }
        if (this.customsDocumentsTicketPM.DocumentStatusCode == "2") {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = true;
        }
        else if (this.customsDocumentsTicketPM.DocumentStatusCode == "1") {
            this.Status2ImageGreen = true;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = false;
        }
        else {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = true;
            this.Status2ErrorImage = false;
        }
    };
    CustomsDocumentTicketViewModel.prototype.SetApprovedDeniedImages = function () {
        if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "4" || this.customsDocumentsTicketPM.VerificationStatusTypeCode == "5") {
            //approvedDeniedImageSource = "/Images/icons/ApprovedDocument.png";
            this.DeniedImageVisibility = false;
            this.ApprovedImageVisibility = true;
        }
        else if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "6") {
            // approvedDeniedImageSource = "/Images/icons/DeniedStamp.png";
            this.DeniedImageVisibility = true;
            this.ApprovedImageVisibility = false;
        }
        else {
            this.DeniedImageVisibility = false;
            this.ApprovedImageVisibility = false;
        }
    };
    CustomsDocumentTicketViewModel.prototype.allowDrop = function (event) {
        event.preventDefault();
        var documentFilingPMId = event.dataTransfer.getData("Id");
        //if (!documentFilingPMId) {
        //    event.dataTransfer.effectAllowed = "none";
        //    event.dataTransfer.dropEffect = "none";
        //}
    };
    CustomsDocumentTicketViewModel.prototype.ConnectDocumentToTicket = function (event, RelatedDocuments, dataContext) {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        }
        if (this.isDisplayOnly && !this.customsDocumentsTicketPM.RequestedCustomsDocId) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 200;
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("ההצהרה כבר שולמה - לא ניתן לקשר מסמכים חדשים");
            messageWindow.WindowClosed.subscribe(function (event) {
                messageWindow.Close();
            });
            return;
        }
        this.DataContext = dataContext;
        var documentFilingPMId = event.dataTransfer.getData("Id");
        if (!Tools_1.AppTool.IsNullOrEmpty(documentFilingPMId)) {
            var relatedDocumentViewModel = RelatedDocuments.filter(function (d) { return d.Id == documentFilingPMId; })[0];
            if (!this.isDisplayOnly || !Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.DocumentsFilingId)) {
                    this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
                    if (relatedDocumentViewModel.CustomDocument == null) {
                        var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                        var docId = encodeURIComponent(relatedDocumentViewModel.Id);
                        customsDocumentPMService.get(docId).subscribe(function (response) {
                            relatedDocumentViewModel.CustomDocument = response.Result;
                            if (relatedDocumentViewModel.CustomDocument) {
                                _this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                            }
                            else {
                                var customsDocumentPM = new CustomsDocumentPM_1.CustomsDocumentPM();
                                customsDocumentPM.DocumentsFilingId = relatedDocumentViewModel.Id;
                                customsDocumentPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                customsDocumentPM.DocumentTypeCode = _this.customsDocumentsTicketPM.DocumentTypeCode;
                                customsDocumentPM.DeclarationId = _this.EntityPM.Id;
                                customsDocumentPM.IsPartOfDeclaration = true;
                                relatedDocumentViewModel.CustomDocument = customsDocumentPM;
                                customsDocumentPMService.insert(customsDocumentPM).subscribe(function (resp) {
                                    if (!resp.HasError) {
                                        _this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                                    }
                                    else {
                                        _this.CurrentSession.StopBusyIndicator();
                                        if (_this.CurrentSession.CurrentEditComponent) {
                                            _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = resp.ErrorsArray;
                                        }
                                        else {
                                            var messageWindow = new MessageWindow_1.MessageWindow();
                                            messageWindow.Width = 400;
                                            messageWindow.Height = 200;
                                            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                                            if (resp.ErrorsArray && resp.ErrorsArray.length > 0) {
                                                messageWindow.Show(resp.ErrorsArray[0]);
                                            }
                                            else {
                                                messageWindow.Show("Server Error");
                                            }
                                            messageWindow.WindowClosed.subscribe(function (event) {
                                                messageWindow.Close();
                                            });
                                        }
                                    }
                                    //this.CurrentSession.StartBusyIndicatorSaving();
                                    //                                this.CurrentSession.StopBusyIndicator();
                                });
                            }
                        });
                    }
                    else if (relatedDocumentViewModel.CustomDocument != null) {
                        this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                    }
                }
            }
        }
    };
    CustomsDocumentTicketViewModel.prototype.StartCustomsDocumentMetaDataCheck = function (relatedDocumentViewModel) {
        var _this = this;
        var isDifferentData = false;
        relatedDocumentViewModel.CustomDocument.CustomsDocumentMetaDataValues.forEach(function (metaDataValue) {
            var metaDataViewModel = _this.metaDataList.filter(function (d) { return d.MetaDataTypeCode == metaDataValue.MetaDataTypeCode; })[0];
            if (metaDataViewModel != null) {
                if (metaDataViewModel.MetaDataValue != null && metaDataValue.MetaDataValue != null) {
                    if (metaDataViewModel.MetaDataValue != metaDataValue.MetaDataValue) {
                        isDifferentData = true;
                    }
                }
            }
        });
        if (isDifferentData && this.customsDocumentsTicketPM.CustomsDocumentPointers.length == 1) {
            this.CurrentSession.StopBusyIndicator();
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocuments.MetaDataDifference"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.ProcessConnectDocument(relatedDocumentViewModel);
                    confirmWindow.Close();
                }
                if (confirmWindow.No) {
                    confirmWindow.Close();
                }
            });
        }
        else {
            this.ProcessConnectDocument(relatedDocumentViewModel);
        }
    };
    CustomsDocumentTicketViewModel.prototype.SaveGeneratedPointer = function (relatedDocumentViewModel) {
        var _this = this;
        if (relatedDocumentViewModel === void 0) { relatedDocumentViewModel = null; }
        var customsDocumentsTicketPMService = new CustomsDocumentsTicketPMService_1.CustomsDocumentsTicketPMService();
        customsDocumentsTicketPMService.insert(this.customsDocumentsTicketPM).subscribe(function (myResp) {
            if (!myResp.HasError) {
                _this.SetSavedMetaData(relatedDocumentViewModel);
                _this.isNew = false;
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                if (_this.CurrentSession.CurrentEditComponent) {
                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResp.ErrorsArray;
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 200;
                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                    if (myResp.ErrorsArray && myResp.ErrorsArray.length > 0) {
                        messageWindow.Show(myResp.ErrorsArray[0]);
                    }
                    else {
                        messageWindow.Show("Server Error");
                    }
                    messageWindow.WindowClosed.subscribe(function (event) {
                        messageWindow.Close();
                    });
                }
            }
        });
    };
    CustomsDocumentTicketViewModel.prototype.SetSavedMetaData = function (relatedDocumentViewModel) {
        var _this = this;
        var documentsFilingId = encodeURIComponent(relatedDocumentViewModel.Id);
        var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
        if (relatedDocumentViewModel.CustomDocument == null) {
            customsDocumentPMService.get(documentsFilingId).subscribe(function (response) {
                relatedDocumentViewModel.CustomDocument = response.Result;
                _this.ApplySaveMetaData(relatedDocumentViewModel);
            });
        }
        else {
            this.ApplySaveMetaData(relatedDocumentViewModel);
        }
    };
    CustomsDocumentTicketViewModel.prototype.ApplySaveMetaData = function (relatedDocumentViewModel) {
        var _this = this;
        this.metaDataList.forEach(function (value) {
            var metadata = null;
            if (_this.customsDocumentMetaDataValuePMs != null) {
                metadata = _this.customsDocumentMetaDataValuePMs.filter(function (d) { return d.MetaDataTypeCode == value.MetaDataTypeCode; })[0];
            }
            var metadataValue = null;
            if (metadata != null) {
                metadataValue = metadata.MetaDataValue;
                var editedValue = relatedDocumentViewModel.CustomDocument.CustomsDocumentMetaDataValues.filter(function (d) { return d.MetaDataTypeCode == metadata.MetaDataTypeCode; })[0];
                if (editedValue != null) {
                    editedValue.MetaDataValue = metadata.MetaDataValue != null ? metadata.MetaDataValue : value.MetaDataValue;
                }
            }
            else {
                var newValue = new CustomsDocumentMetaDataValuePM_1.CustomsDocumentMetaDataValuePM(relatedDocumentViewModel.CustomDocument);
                newValue.MetaDataTypeCode = value.MetaDataTypeCode;
                newValue.CustomsDocumentId = relatedDocumentViewModel.Id;
                newValue.MetaDataValue = metadataValue != null ? metadataValue : value.MetaDataValue;
                newValue.Tenant = SessionLocator_1.SessionLocator.Tenant;
                relatedDocumentViewModel.CustomDocument.AddCustomsDocumentMetaDataValue(newValue);
            }
        });
        var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
        customsDocumentPMService.update(relatedDocumentViewModel.CustomDocument).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (response.HasError) {
                if (_this.CurrentSession.CurrentEditComponent) {
                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 200;
                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                    if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                        messageWindow.Show(response.ErrorsArray[0]);
                    }
                    else {
                        messageWindow.Show("Server Error");
                    }
                    messageWindow.WindowClosed.subscribe(function (event) {
                        messageWindow.Close();
                    });
                }
            }
            else {
                _this.DataContext.SelectedDocumentId = _this.Id;
                _this.DataContext.RefreshEntity();
                //if (this.CurrentSession.CurrentEditComponent) {
                //    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                //}
                //this.DataContext.RefreshButtonClicked(this.Id);
                //this.DataContext.EditCustomsDocumentsTicket(this);
            }
        });
    };
    CustomsDocumentTicketViewModel.prototype.ProcessConnectDocument = function (relatedDocumentViewModel) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
        if (relatedDocumentViewModel != null) {
            var fileSizeInMB = relatedDocumentViewModel.FileSize / (1024 * 1024);
            if (fileSizeInMB > 30) {
                this.CurrentSession.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Height = 200;
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.ShowNoButton = false;
                confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.DocumentSizeLimit"));
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        confirmWindow.Close();
                    }
                });
                return;
            }
            if (relatedDocumentViewModel.CustomDocument.DocumentTypeCode == null) {
                relatedDocumentViewModel.CustomDocument.DocumentTypeCode = this.customsDocumentsTicketPM.DocumentTypeCode;
            }
            if (relatedDocumentViewModel.CustomDocument.DocumentTypeCode == this.customsDocumentsTicketPM.DocumentTypeCode) {
                this.customsDocumentMetaDataValuePMs = relatedDocumentViewModel.CustomDocument.CustomsDocumentMetaDataValues;
                //CustomsDocumentsTicketId adjustment mohammad 18.10.14
                this.customsDocumentsTicketPM.DocumentsFilingId = relatedDocumentViewModel.documentsFilingPM.Id;
                this.customsDocumentsTicketPM.Name = relatedDocumentViewModel.documentsFilingPM.DocumentTypeName;
                this.customsDocumentsTicketPM.Extension = relatedDocumentViewModel.documentsFilingPM.FileExtension;
                this.customsDocumentsTicketPM.FileSize = relatedDocumentViewModel.documentsFilingPM.FileSize;
                this.customsDocumentsTicketPM.IsMetaDataReady = relatedDocumentViewModel.CustomDocument.IsMetaDataReady;
                /// <---field to refresh Screen
                this.customsDocumentsTicketPM.DocumentStatusName = relatedDocumentViewModel.CustomDocument.DocumentStatusName;
                this.customsDocumentsTicketPM.DocumentStatusCode = relatedDocumentViewModel.CustomDocument.DocumentStatusCode;
                this.customsDocumentsTicketPM.CustomsDocId = relatedDocumentViewModel.CustomDocument.CustomsDocId;
                this.customsDocumentsTicketPM.ExternalAttachmentId = relatedDocumentViewModel.CustomDocument.ExternalAttachmentId;
                if (this.isNew) {
                    this.SaveGeneratedPointer(relatedDocumentViewModel);
                }
                else {
                    var customsDocumentsTicketPMService = new CustomsDocumentsTicketPMService_1.CustomsDocumentsTicketPMService();
                    customsDocumentsTicketPMService.update(this.customsDocumentsTicketPM).subscribe(function (response) {
                        if (!response.HasError) {
                            relatedDocumentViewModel.IsConnected = true;
                            relatedDocumentViewModel.CustomDocument.IsPartOfDeclaration = true;
                            _this.SetSavedMetaData(relatedDocumentViewModel);
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            if (_this.CurrentSession.CurrentEditComponent) {
                                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
                            }
                            else {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.Width = 400;
                                messageWindow.Height = 200;
                                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                                if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                                    messageWindow.Show(response.ErrorsArray[0]);
                                }
                                else {
                                    messageWindow.Show("Server Error");
                                }
                                messageWindow.WindowClosed.subscribe(function (event) {
                                    messageWindow.Close();
                                });
                            }
                        }
                    });
                }
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Width = 400;
                messageWindow.Height = 200;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DocumentAndCustomDocumentType"));
                messageWindow.WindowClosed.subscribe(function (event) {
                    messageWindow.Close();
                });
            }
        }
    };
    CustomsDocumentTicketViewModel.prototype.DisconnectButtonClicked_old = function (dataContext) {
        var _this = this;
        this.DataContext = dataContext;
        if (this.customsDocumentsTicketPM.DocumentStatusCode == "8") {
            //var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
            //if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
            var message = ".לא ניתן לנתק מסמך בתהליך אימות";
            //}
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    confirmWindow.Close();
                }
            });
        }
        else if (this.customsDocumentsTicketPM.VerificationStatusTypeCode == "4" || this.customsDocumentsTicketPM.VerificationStatusTypeCode == "5" || this.customsDocumentsTicketPM.VerificationStatusTypeCode == "6") {
            //var message: string = TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
            //if (AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
            var message = ".לא ניתן לנתק מסמך אומת/נדחה";
            //}
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    confirmWindow.Close();
                }
            });
        }
        else {
            this.iCustomsDocumentsController.CheckRequestsInProgress(this.customsDocumentsTicketPM.DocumentsFilingId).subscribe(function (response) {
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.customsDocumentsTicketPM.CustomsDocId)) {
                    if (response.Result.IsDisplayOnly) {
                        if (_this.isDisplayOnly) {
                            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
                            if (Tools_1.AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
                                message = ".לא ניתן לנתק מסמך עם סימוכין - הצהרה לתצוגה בלבד";
                            }
                            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            confirmWindow.Width = 400;
                            confirmWindow.Height = 200;
                            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                            confirmWindow.ShowNoButton = false;
                            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
                            confirmWindow.Show(message);
                            confirmWindow.WindowClosed.subscribe(function (event) {
                                if (confirmWindow.Yes) {
                                    confirmWindow.Close();
                                }
                            });
                        }
                        else {
                            _this.ApplyDisconnectFromDocument(true);
                        }
                    }
                    else {
                        _this.ApplyDisconnectFromDocument(true);
                    }
                }
                else {
                    if (_this.isDisplayOnly && response.Result.IsDisplayOnly) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.customsDocumentsTicketPM.RequestedCustomsDocId) && !(response.Result.IsDisplayOnly)) {
                            _this.ApplyDisconnectFromDocument(true);
                        }
                        else {
                            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
                            if (Tools_1.AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
                                message = "לא ניתן לנתק מסמך נדרש – קיימת בקשה בתהליך";
                            }
                            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            confirmWindow.Width = 400;
                            confirmWindow.Height = 200;
                            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                            confirmWindow.ShowNoButton = false;
                            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
                            confirmWindow.Show(message);
                            confirmWindow.WindowClosed.subscribe(function (event) {
                                if (confirmWindow.Yes) {
                                    confirmWindow.Close();
                                }
                            });
                        }
                    }
                    else {
                        if (response.Result.IsDisplayOnly) {
                            var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DisconnectNotAllowed");
                            if (Tools_1.AppTool.IsNullOrEmpty(message) || message == "Customs.Declaration.O.DisconnectNotAllowed") {
                                message = ".לא ניתן לנתק מסמך עם בקשה בתהליך";
                            }
                            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            confirmWindow.Width = 400;
                            confirmWindow.Height = 200;
                            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                            confirmWindow.ShowNoButton = false;
                            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
                            confirmWindow.Show(message);
                            confirmWindow.WindowClosed.subscribe(function (event) {
                                if (confirmWindow.Yes) {
                                    confirmWindow.Close();
                                }
                            });
                        }
                        else {
                            _this.ApplyDisconnectFromDocument(true);
                        }
                    }
                }
            });
        }
    };
    CustomsDocumentTicketViewModel.prototype.DisconnectButtonClicked = function (dataContext) {
        //Display only Logic - (Task 35024)
        var applyDisconnect = true;
        var message = "";
        if (this.customsDocumentsTicketPM) {
            var entitySpecialCondition = this.iCustomsDocumentsController.GetAddEditDocumentsEntitySpecialCondition();
            if ((this.isDisplayOnly || !entitySpecialCondition) && Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) { //(Regular doc in a paid declaration) or just disabled declaration
                applyDisconnect = false;
                message = "ההצהרה לתצוגה בלבד - לא ניתן לנתק מסמכים";
            }
            if (this.isDisplayOnly && Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && !Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.CustomsDocId)) { //(Regular doc that was already sent to customs)
                applyDisconnect = false;
                message = ".לא ניתן לנתק מסמך עם סימוכין - הצהרה לתצוגה בלבד";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && !Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode)) { //(Requested doc that was verified/denied/in verification process)
                applyDisconnect = false;
                message = ".לא ניתן לנתק מסמך אומת/נדחה/בתהליך אימות";
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId) && Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.VerificationStatusTypeCode)) { //Requested Doc that wasn't send to customs for verification allow disconnect according to design.
                applyDisconnect = true;
                //message = "לא ניתן לנתק מסמך נדרש – קיימת בקשה בתהליך";
            }
            if (this.customsDocumentsTicketPM.DocumentStatusCode == '7') {
                applyDisconnect = false;
                message = "לא ניתן לנתק את המסמך - קיימת בקשה בתהליך";
            }
        }
        if (applyDisconnect) {
            this.ApplyDisconnectFromDocument(true);
        }
        else {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = false;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
            confirmWindow.Show(message);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    confirmWindow.Close();
                }
            });
        }
    };
    CustomsDocumentTicketViewModel.prototype.ApplyDisconnectFromDocument = function (submit) {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
        this.customsDocumentsTicketPM.DocumentsFilingId = null;
        this.customsDocumentsTicketPM.Extension = null;
        this.customsDocumentsTicketPM.Name = null;
        this.customsDocumentsTicketPM.FileSize = null;
        this.LeadingMetaDataName = null;
        this.LeadingMetaDataValue = null;
        if (this.customsDocumentMetaDataValuePMs != null) {
            this.customsDocumentMetaDataValuePMs = [];
            this.customsDocumentMetaDataValuePMs = null;
        }
        this.customsDocumentsTicketPM.IsMetaDataReady = false;
        if (submit) {
            var customsDocumentsTicketPMService = new CustomsDocumentsTicketPMService_1.CustomsDocumentsTicketPMService();
            customsDocumentsTicketPMService.update(this.customsDocumentsTicketPM).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                if (response.HasError) {
                    if (_this.CurrentSession.CurrentEditComponent) {
                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
                    }
                    else {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 200;
                        messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                            messageWindow.Show(response.ErrorsArray[0]);
                        }
                        else {
                            messageWindow.Show("Server Error");
                        }
                        messageWindow.WindowClosed.subscribe(function (event) {
                            messageWindow.Close();
                        });
                    }
                }
                else {
                    //this.DataContext.RefreshButtonClicked();
                    _this.DataContext.RefreshEntity();
                    //if (this.CurrentSession.CurrentEditComponent) {
                    //    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    //}
                }
            });
        }
    };
    CustomsDocumentTicketViewModel.prototype.DeleteButtonClicked = function (dataContext) {
        var _this = this;
        this.DataContext = dataContext;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
        if (!this.isNew) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.RequestedCustomsDocId)) {
                this.CurrentSession.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Height = 200;
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                confirmWindow.ShowNoButton = false;
                confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
                confirmWindow.Show("Can't delete a ticket with a requested document id"); //(TextCodeTranslator.Translate("Customs.General.O.DocumentSizeLimit"));
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        confirmWindow.Close();
                    }
                });
            }
            else {
                this.iCustomsDocumentsController.CheckRequestsInProgress(this.customsDocumentsTicketPM.DocumentsFilingId).subscribe(function (response) {
                    if (response.Result.IsDisplayOnly) {
                        _this.CurrentSession.StopBusyIndicator();
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Width = 400;
                        confirmWindow.Height = 200;
                        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow.ShowNoButton = false;
                        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.No");
                        confirmWindow.Show("Can't delete a ticket with a request in progress"); //(TextCodeTranslator.Translate("Customs.General.O.DocumentSizeLimit"));
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                confirmWindow.Close();
                            }
                        });
                    }
                    else {
                        var customsDocumentsTicketPMService = new CustomsDocumentsTicketsExtendedService_1.CustomsDocumentsTicketsExtendedService();
                        customsDocumentsTicketPMService.delete(_this.customsDocumentsTicketPM.Id).subscribe(function (deleteResp) {
                            _this.CurrentSession.StopBusyIndicator();
                            if (!deleteResp.HasError) {
                                _this.DataContext.RefreshButtonClicked();
                            }
                            else {
                                if (_this.CurrentSession.CurrentEditComponent) {
                                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = deleteResp.ErrorsArray;
                                }
                                else {
                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                    messageWindow.Width = 400;
                                    messageWindow.Height = 200;
                                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                                    if (deleteResp.ErrorsArray && deleteResp.ErrorsArray.length > 0) {
                                        messageWindow.Show(deleteResp.ErrorsArray[0]);
                                    }
                                    else {
                                        messageWindow.Show("Server Error");
                                    }
                                    messageWindow.WindowClosed.subscribe(function (event) {
                                        messageWindow.Close();
                                    });
                                }
                            }
                        });
                    }
                });
            }
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.DataContext.RefreshButtonClicked();
        }
    };
    CustomsDocumentTicketViewModel.prototype.ViewDocumentsQuery = function () {
        var _this = this;
        var windowArgs = {};
        var entityInfo = this.iCustomsDocumentsController.GetParentAndChildrenEntityCodesAndIds();
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.RelatedDocuments");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 800;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnAddEditWindowClosed($event); });
        logWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Loading"));
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/DocumentsFilingsQueryComponent');
    };
    CustomsDocumentTicketViewModel.prototype.OnAddEditWindowClosed = function (event) {
        var _this = this;
        if (event != 'cancel' && this.isDisplayOnly && !this.customsDocumentsTicketPM.RequestedCustomsDocId) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 200;
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("ההצהרה כבר שולמה - לא ניתן לקשר מסמכים חדשים");
            messageWindow.WindowClosed.subscribe(function (event) {
                messageWindow.Close();
            });
            return;
        }
        if (event != 'cancel' && !Tools_1.AppTool.IsNullOrEmpty(event)) {
            var documentsFilingId = event; //encodeURIComponent(event);
            var custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
            custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe(function (resp) {
                var documentFiling = resp.Result;
                var custDocMetaDataValuesWebService = new CustDocMetaDataValuesWebService_1.CustDocMetaDataValuesWebService();
                custDocMetaDataValuesWebService.GetCustomsDocumentMetaDataValuesByConnectedEntity(documentFiling.EntityId).subscribe(function (metadataResp) {
                    var values = metadataResp.Result;
                    var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                    var documentsFilingIdEnc = encodeURIComponent(documentsFilingId);
                    customsDocumentPMService.get(documentsFilingIdEnc).subscribe(function (response) {
                        var relatedDocumentViewModel = new RelatedDocumentViewModel_1.RelatedDocumentViewModel(documentFiling, values, _this.isDisplayOnly);
                        relatedDocumentViewModel.CustomDocument = response.Result;
                        if (relatedDocumentViewModel.CustomDocument) {
                            _this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                        }
                        else {
                            var customsDocumentPM = new CustomsDocumentPM_1.CustomsDocumentPM();
                            customsDocumentPM.DocumentsFilingId = relatedDocumentViewModel.Id;
                            customsDocumentPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            customsDocumentPM.DocumentTypeCode = _this.customsDocumentsTicketPM.DocumentTypeCode;
                            customsDocumentPM.DeclarationId = _this.EntityPM.Id;
                            relatedDocumentViewModel.CustomDocument = customsDocumentPM;
                            customsDocumentPMService.insert(customsDocumentPM).subscribe(function (resp) {
                                if (!resp.HasError) {
                                    _this.StartCustomsDocumentMetaDataCheck(relatedDocumentViewModel);
                                }
                                else {
                                    _this.CurrentSession.StopBusyIndicator();
                                    if (_this.CurrentSession.CurrentEditComponent) {
                                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = resp.ErrorsArray;
                                    }
                                    else {
                                        var messageWindow = new MessageWindow_1.MessageWindow();
                                        messageWindow.Width = 400;
                                        messageWindow.Height = 200;
                                        messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                                        if (resp.ErrorsArray && resp.ErrorsArray.length > 0) {
                                            messageWindow.Show(resp.ErrorsArray[0]);
                                        }
                                        else {
                                            messageWindow.Show("Server Error");
                                        }
                                        messageWindow.WindowClosed.subscribe(function (event) {
                                            messageWindow.Close();
                                        });
                                    }
                                }
                                //this.CurrentSession.StartBusyIndicatorSaving();
                                //                                this.CurrentSession.StopBusyIndicator();
                            });
                        }
                    });
                });
            });
        }
    };
    CustomsDocumentTicketViewModel.prototype.PreventEditTicket = function () {
        this.PreventEdit = true;
    };
    CustomsDocumentTicketViewModel.prototype.AllowEditTicket = function () {
        this.PreventEdit = false;
    };
    CustomsDocumentTicketViewModel.prototype.ShowMustSend = function () {
        if (!this.customsDocumentsTicketPM.IsSendMandatory) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.customsDocumentsTicketPM.CustomsDocId)) {
            return true;
        }
        else {
            return false;
        }
    };
    return CustomsDocumentTicketViewModel;
}());
exports.CustomsDocumentTicketViewModel = CustomsDocumentTicketViewModel;
var MetaDataValueViewModel = /** @class */ (function () {
    function MetaDataValueViewModel() {
    }
    return MetaDataValueViewModel;
}());
exports.MetaDataValueViewModel = MetaDataValueViewModel;
//# sourceMappingURL=CustomsDocumentTicketViewModel.js.map