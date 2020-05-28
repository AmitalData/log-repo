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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CustomsDocumentPointerPM_1 = require("../../../Customs/EntityPMs/CustomsDocumentPointerPM");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CustomsDocumentsTicketPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentsTicketPMService");
var CustomsDocumentPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentPMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var CustDocTypeMetaDataWebService_1 = require("../../../Customs/Services/WebServices/CustDocTypeMetaDataWebService");
var CustomsDocumentMetaDataValuePM_1 = require("../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM");
var CustomsClosedTableListService_1 = require("../../../Customs/Services/StandardLists/CustomsClosedTableListService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var CustomDocumentViewerService_1 = require("../../../Customs/Services/WebServices/CustomDocumentViewerService");
var CustomsSettingListService_1 = require("../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomDocumentTypeListService_1 = require("../../../Customs/Services/StandardLists/CustomDocumentTypeListService");
var AddEditCustomsDocumentComponent = /** @class */ (function (_super) {
    __extends(AddEditCustomsDocumentComponent, _super);
    //***********************************************************************//
    function AddEditCustomsDocumentComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsTicketChanged = false;
        _this.ChildrenComboBoxVisibility = true;
        _this.UpperHeight = { 'height': '130px' };
        _this.SupplierInvoiceItemsVisibility = true;
        _this.LayoutDirection = 'rtl';
        _this.SecondChildVisibility = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsUserTabSelected = false;
        _this._CustomDocumentViewerService = new CustomDocumentViewerService_1.CustomDocumentViewerService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this.IsConnectedToUniFreight = false;
        _this.updateCustomsDocument = false;
        //#region Tabs Code
        _this.TabsSource = [];
        _this.SelectedTab = "";
        return _this;
    }
    Object.defineProperty(AddEditCustomsDocumentComponent.prototype, "DocumentTypeCode", {
        get: function () {
            if (this.CustomsDocument) {
                this.documentTypeCode = this.CustomsDocument.DocumentTypeCode;
            }
            else if (this.CustomsDocumentsTicket) {
                this.documentTypeCode = this.CustomsDocumentsTicket.DocumentTypeCode;
            }
            return this.documentTypeCode;
        },
        set: function (value) {
            if (value != this.documentTypeCode) {
                this.documentTypeCode = value;
                //if (value != null) {
                if (this.CustomsDocument) {
                    //not needed
                    //if (this.newVersionAdded) {
                    //    this.documentTypeCode = value;
                    //    this.CurrentSession.StopBusyIndicator();
                    //    var confirmWindow = new ConfirmWindow();
                    //    confirmWindow.Width = 400;
                    //    confirmWindow.Height = 200;
                    //    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    //    confirmWindow.ShowNoButton = false;
                    //    confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                    //    confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsDocuments.DocMetadataWarning"));
                    //    confirmWindow.WindowClosed.subscribe((event: any) => {
                    //        if (confirmWindow.Yes) {
                    //            this.CustomsDocument.DocumentTypeCode = this.documentTypeCode;
                    //            //customDocumentMetaDataControlViewModel.LoadMetaData(previousValueList);
                    //            confirmWindow.Close();
                    //        }
                    //    });
                    //}
                    // else {
                    this.CustomsDocument.DocumentTypeCode = value;
                    this.ClearAllMetaDataValues();
                    this.InitializeMetaData(this.previousValueList);
                    // }
                }
                if (this.CustomsDocumentsTicket) {
                    this.CustomsDocumentsTicket.DocumentTypeCode = value;
                    //ChangePointersDocumentTypeCode(value);
                    //this.IsTicketChanged = true;
                }
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsDocumentComponent.prototype, "Remarks", {
        get: function () {
            if (this.CustomsDocumentsTicket) {
                return this.CustomsDocumentsTicket.Remarks;
            }
            else {
                return null;
            }
        },
        set: function (value) {
            this.CustomsDocumentsTicket.Remarks = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsDocumentComponent.prototype, "VerificationRemarks", {
        get: function () {
            if (this.CustomsDocumentsTicket) {
                return this.CustomsDocumentsTicket.VerificationRemarks;
            }
            else {
                return null;
            }
        },
        set: function (value) {
            this.CustomsDocumentsTicket.VerificationRemarks = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsDocumentComponent.prototype, "UserRemarks", {
        get: function () {
            if (this.CustomsDocumentsTicket) {
                return this.CustomsDocumentsTicket.UserRemarks;
            }
            else {
                return null;
            }
        },
        set: function (value) {
            this.CustomsDocumentsTicket.UserRemarks = value;
        },
        enumerable: true,
        configurable: true
    });
    AddEditCustomsDocumentComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.WindowArgs = windowArgs;
        this.CustomsDocumentsTicket = windowArgs.CustomsDocumentsTicket;
        if (this.CustomsDocumentsTicket) {
            this.CustomsDocumentsTicket.CloneMe();
        }
        this.IsDisplayOnly = windowArgs.IsDisplayOnly;
        this.IsEntityDisplayOnly = windowArgs.IsEntityDisplayOnly;
        this.IsNewState = windowArgs.IsNewState;
        console.log(this.CustomsDocument);
        this.CustomsDocument = windowArgs.CustomsDocument;
        if (this.CustomsDocument) {
            this.CustomsDocument.CloneMe();
        }
        this.ParentEntityCode = windowArgs.ParentEntityCode;
        this.ParentEntityId = windowArgs.ParentEntityId;
        this.Child1EntityCode = windowArgs.Child1EntityCode;
        this.Child1EntityId = windowArgs.Child1EntityId;
        this.Child2EntityCode = windowArgs.Child2EntityCode;
        this.Child2EntityId = windowArgs.Child2EntityId;
        this.Child3EntityCode = windowArgs.Child3EntityCode;
        this.Child3EntityId = windowArgs.Child3EntityId;
        this.iCustomsDocumentsController = windowArgs.iCustomsDocumentsController;
        this.IsCustomsDocumentInRequest = windowArgs.IsCustomsDocumentInRequest;
        this.EntityPM = windowArgs.EntityPM;
        this.CheckEditEnabled(this.IsCustomsDocumentInRequest, !this.IsDisplayOnly);
        this.SelectedIndex = 0;
        this.FillConnectedToItems();
        if (this.CustomsDocumentsTicket != null) { //&& this.objectTableName != "Customs.CustomsCollateral") { this is to be added later when work on collateral
            this.ChildrenComboBoxVisibility = this.iCustomsDocumentsController.GetChildrenComboboxVisibility();
            this.SecondChildVisibility = this.iCustomsDocumentsController.GetSecondChildVisibility();
            if (this.ChildrenComboBoxVisibility) {
                this.UpperHeight = { 'height': '130px' };
            }
            else {
                this.UpperHeight = { 'height': '30px' };
            }
        }
        else {
            this.ChildrenComboBoxVisibility = false;
            this.UpperHeight = { 'height': '30px' };
        }
        this.RelatedEntityLable = this.iCustomsDocumentsController.GetRelatedEntityLabel();
        this.RefereshConnectedInvoices();
        if (this.CustomsDocumentsTicket != null) {
            this.RemarksTextBoxVisiblity = true;
            this.VerificationRemarksTextBoxVisiblity = true;
            this.UserRemarksTextBoxVisiblity = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.VerificationRemarks)) {
                this.VerificationRemarksTextBoxVisiblity = true;
            }
            else {
                this.VerificationRemarksTextBoxVisiblity = false;
            }
            //if (this.CustomsDocumentsTicket.DocumentsFilingId == null) {
            //    this.RemarksTextBoxVisiblity = false;
            //    this.IsUserTabSelected = true;
            //}
            //else {
            //    this.RemarksTextBoxVisiblity = true;
            //    this.IsUserTabSelected = false;
            //}
        }
        else {
            this.RemarksTextBoxVisiblity = false;
            this.VerificationRemarksTextBoxVisiblity = false;
            this.UserRemarksTextBoxVisiblity = false;
        }
        this.BuildTabs();
        if (this.CustomsDocument) {
            this.InitializeMetaData(null);
        }
        this.LoadCustomsSettings();
    };
    AddEditCustomsDocumentComponent.prototype.LoadCustomsSettings = function () {
        var _this = this;
        this.customsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString()).subscribe(function (response) {
            var list = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                var customsSetting = list;
                _this.IsConnectedToUniFreight = customsSetting.IsConnectedToUniFreight;
            }
            _this.LoadDocumentPage();
        });
    };
    AddEditCustomsDocumentComponent.prototype.LoadDocumentPage = function () {
        var _this = this;
        if (this.CustomsDocument) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._CustomDocumentViewerService.GetDocumentPage(this.CustomsDocument.DocumentId, 0, this.IsConnectedToUniFreight).subscribe(function (myResponse) {
                var result = myResponse.Result;
                if (result) {
                    console.log("[Response] GetDocumentPage", result);
                    if (!Tools_1.AppTool.IsNullOrEmpty(result.Page)) {
                        _this.base64Image = "data:image/png;base64," + result.Page;
                    }
                    else {
                        _this.base64Image = null;
                    }
                }
                else {
                    _this.base64Image = null;
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    //#endregion
    AddEditCustomsDocumentComponent.prototype.CheckEditEnabled_Old = function (isCustomsDocumentInRequest, isEditEnabled) {
        this.ViewDisableMessageVisibility = false;
        if (this.CustomsDocument != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)) {
                if (!isCustomsDocumentInRequest && this.CustomsDocument.DocumentStatusCode != "7" && this.CustomsDocument.DocumentStatusCode != "1") {
                    this.IsDocumentTypeEnabled = false;
                    this.IsEditEnabled = true;
                    this.IsMetaDataEditEnabled = true;
                    this.IsSendDocumentEnabled = true;
                }
                else { //removed redundunt code
                    //if (!isEditEnabled) {
                    //    this.IsEditEnabled = false;
                    //    this.IsMetaDataEditEnabled = false;
                    //    this.IsSendDocumentEnabled = false;
                    //    this.IsDocumentTypeEnabled = false;
                    //    this.ViewDisableMessageVisibility = true;
                    //    this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                    //}
                    //else {
                    this.IsEditEnabled = false;
                    this.IsMetaDataEditEnabled = false;
                    this.IsDocumentTypeEnabled = false;
                    this.IsSendDocumentEnabled = false;
                    this.ViewDisableMessageVisibility = true;
                    this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                    //}
                }
            }
            else {
                if (!isEditEnabled) {
                    this.IsEditEnabled = false;
                    this.IsMetaDataEditEnabled = false;
                    this.IsSendDocumentEnabled = false;
                    this.IsDocumentTypeEnabled = false;
                    this.ViewDisableMessageVisibility = true;
                    this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                }
                else {
                    if (!isCustomsDocumentInRequest && this.CustomsDocument.DocumentStatusCode != "7" && this.CustomsDocument.DocumentStatusCode != "1") {
                        this.IsDocumentTypeEnabled = false;
                        this.IsEditEnabled = true;
                        this.IsMetaDataEditEnabled = true;
                        this.IsSendDocumentEnabled = true;
                        this.ViewDisableMessageVisibility = false;
                    }
                    else {
                        this.IsEditEnabled = false;
                        this.IsMetaDataEditEnabled = false;
                        this.IsSendDocumentEnabled = false;
                        this.IsDocumentTypeEnabled = false;
                        this.ViewDisableMessageVisibility = true;
                        this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                    }
                }
            }
        }
        else {
            if (this.CustomsDocumentsTicket != null) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)) {
                    this.IsMetaDataEditEnabled = true;
                    this.IsSendDocumentEnabled = true;
                    this.IsEditEnabled = false;
                    this.IsDocumentTypeEnabled = false;
                }
                else {
                    this.IsMetaDataEditEnabled = true;
                    this.IsSendDocumentEnabled = true;
                    this.IsEditEnabled = true;
                    this.IsDocumentTypeEnabled = true;
                }
            }
        }
        //-----mohamma bug 34085***********
        if (this.CustomsDocumentsTicket == null) {
            this.IsDocumentTypeEnabled = true;
        }
        // Mohammad- task 31851 ****************
        if (this.CustomsDocumentsTicket && this.CustomsDocument) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && this.CustomsDocument.DocumentStatusCode == '3') {
                this.IsSendDocumentEnabled = true;
            }
        }
        if (this.CustomsDocument) {
            if (this.CustomsDocument.DocumentStatusCode == '4' || this.CustomsDocument.DocumentStatusCode == '8') {
                this.IsMetaDataEditEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.IsEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
            }
        }
        //******************
        //*********************** task 32398********************//
        if (this.CustomsDocument) {
            var one = "1";
            var seven = "7";
            var statusCodes = ['1', '7'];
            if (statusCodes.indexOf(this.CustomsDocument.DocumentStatusCode) > -1 && !Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)
                && this.CustomsDocumentsTicket && Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)
                && !this.IsEntityDisplayOnly && this.CustomsDocument.CurrentEntityId == this.ParentEntityId) {
                this.IsActionButtonsEnabled = true;
            }
            else {
                if (this.CustomsDocument.DocumentStatusCode == '2') {
                    this.IsActionButtonsEnabled = true;
                }
                else {
                    this.IsActionButtonsEnabled = false;
                }
            }
        }
        //*********************************************************//
        this.UIProperties.SetEnabled("DocumentTypeCode", "Customs.CustomsDocument", this.IsDocumentTypeEnabled);
    };
    AddEditCustomsDocumentComponent.prototype.CheckEditEnabled = function (isCustomsDocumentInRequest, isEditEnabled) {
        // default values
        this.IsEditEnabled = true;
        this.IsMetaDataEditEnabled = true;
        this.IsDocumentTypeEnabled = true;
        this.IsSendDocumentEnabled = true;
        this.IsPointerChangeEnabled = true;
        this.ViewDisableMessageVisibility = false;
        this.DisplayOnlyMessage = null;
        this.IsActionButtonsEnabled = false;
        // document varification 
        if (this.CustomsDocumentsTicket) {
            if (this.CustomsDocumentsTicket.VerificationStatusTypeCode == '4' || this.CustomsDocumentsTicket.VerificationStatusTypeCode == '5') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.ViewDisableMessageVisibility = true;
                this.DisplayOnlyMessage = " לתצוגה בלבד - מסמך כבר אומת על ידי המכס "; //Document Was already Verified By Customs
            }
            if (this.CustomsDocumentsTicket.VerificationStatusTypeCode == '8') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.ViewDisableMessageVisibility = true;
                this.DisplayOnlyMessage = " לתצוגה בלבד - המסמך בתהליך אימות במכס "; //Document is in verfication prgress
            }
            if (this.CustomsDocumentsTicket.VerificationStatusTypeCode == '6') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.ViewDisableMessageVisibility = true;
                this.DisplayOnlyMessage = " לתצוגה בלבד - המסמך נדחה על ידי המכס "; //Document was deneid
            }
            if (this.IsEntityDisplayOnly) { // declaration display only
                this.IsPointerChangeEnabled = false;
                this.IsEditEnabled = false;
                //this.IsSendDocumentEnabled = false;
                //this.IsDocumentTypeEnabled = false;
            }
        }
        //Display only Logic - (Task 35024)
        if (this.CustomsDocumentsTicket && this.CustomsDocument) {
            var entitySpecialCondition = this.iCustomsDocumentsController.GetAddEditDocumentsEntitySpecialCondition();
            if ((this.IsEntityDisplayOnly || !entitySpecialCondition) && Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)) { //(Regular doc in a paid declaration)
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && !Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)) { //(Regular doc that was already sent to customs)
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && !Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.VerificationStatusTypeCode)) { //(Requested doc that was verified/denied/in verification process)
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.VerificationStatusTypeCode)) { //Requested Doc that wasn't send to customs for verification
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = true;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = true;
            }
            if (this.CustomsDocument.DocumentStatusCode == '7') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            if (!entitySpecialCondition || this.CustomsDocument.DocumentStatusCode == '7' || this.IsEntityDisplayOnly) {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                //this.IsSendDocumentEnabled = false;
                //this.IsDocumentTypeEnabled = false;
            }
            else {
                this.IsEditEnabled = true;
                this.IsPointerChangeEnabled = true;
            }
        }
        //*********************** task 32398 new version button********************//
        if (this.CustomsDocument) {
            var isSendWithCustomsDocId = false;
            if (this.ParentEntityCode == "CustomsCollateral") {
                isSendWithCustomsDocId = true;
            }
            if (this.CustomsDocument.CustomsDocId && !isSendWithCustomsDocId) {
                this.IsMetaDataEditEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            var statusCodes = ['1', '7'];
            if (statusCodes.indexOf(this.CustomsDocument.DocumentStatusCode) > -1 && !Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)
                && this.CustomsDocumentsTicket && Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)
                && !this.IsEntityDisplayOnly && this.CustomsDocument.CurrentEntityId == this.ParentEntityId) {
                this.IsActionButtonsEnabled = true;
            }
            else {
                if (this.CustomsDocument.DocumentStatusCode == '2') {
                    this.IsActionButtonsEnabled = true;
                }
                else {
                    this.IsActionButtonsEnabled = false;
                }
            }
        }
        //*********************************************************//
        this.UIProperties.SetEnabled("DocumentTypeCode", "Customs.CustomsDocument", this.IsDocumentTypeEnabled);
    };
    AddEditCustomsDocumentComponent.prototype.OkButtonClicked = function () {
        this.OkMethod(false);
    };
    AddEditCustomsDocumentComponent.prototype.SendButtonClicked = function () {
        this.OkMethod(true);
    };
    AddEditCustomsDocumentComponent.prototype.OkMethod = function (isSendToQueue) {
        var _this = this;
        var errors = [];
        if (this.CustomsDocument) {
            Validator_1.Validator.TryValidateObject(this.CustomsDocument, "Customs.CustomsDocument", errors);
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
                return;
            }
        }
        if (this.CustomsDocumentsTicket) {
            Validator_1.Validator.TryValidateObject(this.CustomsDocumentsTicket, "Customs.CustomsDocumentsTicket", errors);
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
                return;
            }
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
        if (this.ViewDisableMessageVisibility) {
            this.CancelButtonClicked();
            return;
        }
        var requiredFieldsErrors = [];
        if (this.CustomsDocument != null) {
            requiredFieldsErrors = this.GetRequiredFieldsErrors();
            this.MetaDataViewModels.forEach(function (viewModel) {
                var value = _this.CustomsDocument.CustomsDocumentMetaDataValues.filter(function (d) { return d.MetaDataTypeCode == viewModel.MetaDataValue.MetaDataTypeCode; })[0];
                if (!value) {
                    _this.CustomsDocument.AddCustomsDocumentMetaDataValue(viewModel.MetaDataValue);
                }
            });
            this.CustomsDocument.IsSendToQueue = isSendToQueue;
            var updateByController = true;
            if (!updateByController) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocument.DeclarationId)) {
                    this.CustomsDocument.DeclarationId = this.ParentEntityId;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocument.DeclarationId)) {
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
            else {
                this.iCustomsDocumentsController.UpdateCustomsDocumentb4Send(this.CustomsDocument);
            }
            //throw new Exception("Do not send  CustomsDocumentPM  without  parentEntityId !!!");
        }
        if (this.CustomsDocumentsTicket) {
            if (this.CustomsDocument != null) {
                this.updateCustomsDocument = true;
                this.CustomsDocument.CurrentCustomsDocumentsTicketId = this.CustomsDocumentsTicket.Id;
            }
            if (this.IsTicketChanged || this.IsNewState) {
                // Validator.TryValidateObject(CustomsDocumentsTicket, new ValidationContext(CustomsDocumentsTicket), errors);
                if (this.CustomsDocumentsTicket.CustomsDocumentPointers.length == 0) {
                    var newPointer = new CustomsDocumentPointerPM_1.CustomsDocumentPointerPM(this.CustomsDocumentsTicket);
                    newPointer.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newPointer.ParentEntityId = this.ParentEntityId;
                    newPointer.ParentEntityCode = this.ParentEntityCode;
                    newPointer.Child1EntityCode = null;
                    newPointer.Child2EntityCode = null;
                    newPointer.Child3EntityCode = null;
                    newPointer.Child1EntityId = null;
                    newPointer.Child2EntityId = null;
                    newPointer.Child3EntityId = null;
                    newPointer.DocumentTypeCode = this.CustomsDocumentsTicket.DocumentTypeCode;
                    this.CustomsDocumentsTicket.AddCustomsDocumentPointer(newPointer);
                }
                else {
                    this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach(function (newPointer) {
                        newPointer.DocumentTypeCode = _this.CustomsDocumentsTicket.DocumentTypeCode;
                    });
                }
                this.InsertNewTicket();
                return;
            }
        }
        // for required meta data
        //string requiredFieldsWarning = "";
        //foreach(ValidationResult result in requiredFieldsErrors)
        //{
        //    if (string.IsNullOrEmpty(requiredFieldsWarning)) {
        //        requiredFieldsWarning = result.ErrorMessage;
        //    }
        //    else {
        //        requiredFieldsWarning = requiredFieldsWarning + Environment.NewLine + result.ErrorMessage;
        //    }
        //}
        var requiredFieldsWarning = "";
        requiredFieldsErrors.forEach(function (error) {
            if (Tools_1.AppTool.IsNullOrEmpty(requiredFieldsWarning)) {
                requiredFieldsWarning = error;
            }
            else {
                requiredFieldsWarning = requiredFieldsWarning + '\n' + error;
            }
        });
        if (requiredFieldsErrors.length == 0) {
            if (this.CustomsDocument != null) {
                this.CustomsDocument.IsMetaDataReady = true;
            }
            this.PerformSubmitChanges();
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.CustomsDocument.IsMetaDataReady = false;
            //here we pop a confirm window to ask weather to continue or not if there is required fields errors.
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
            confirmWindow.Show(requiredFieldsWarning);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
                    _this.PerformSubmitChanges();
                    confirmWindow.Close();
                }
                if (confirmWindow.No) {
                    confirmWindow.Close();
                }
            });
        }
    };
    AddEditCustomsDocumentComponent.prototype.CancelButtonClicked = function () {
        if (this.CustomsDocument) {
            this.CustomsDocument.RejectChanges();
        }
        if (this.CustomsDocumentsTicket) {
            this.CustomsDocumentsTicket.RejectChanges();
        }
        if (this.newVersionAdded) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit("cancel");
        }
    };
    AddEditCustomsDocumentComponent.prototype.InsertNewTicket = function () {
        var _this = this;
        var customsDocumentsTicketPMService = new CustomsDocumentsTicketPMService_1.CustomsDocumentsTicketPMService();
        customsDocumentsTicketPMService.insert(this.CustomsDocumentsTicket).subscribe(function (resp) {
            _this.CurrentSession.StopBusyIndicator();
            if (!resp.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
        });
    };
    AddEditCustomsDocumentComponent.prototype.FillConnectedToItems = function () {
        this.ConnectedToItems = this.iCustomsDocumentsController.FillConnectedToItems();
        //if (entityCode.toLowerCase() == "declaration") {
        //    var connectedItem1 = new ConnectedToItem();
        //    connectedItem1.Id = 0;
        //    connectedItem1.Name = TextCodeTranslator.Translate("Customs.Declaration");
        //    var connectedItem2 = new ConnectedToItem();
        //    connectedItem2.Id = 1;
        //    connectedItem2.Name = TextCodeTranslator.Translate("Customs.SupplierInvoice");
        //    var connectedItem3 = new ConnectedToItem();
        //    connectedItem3.Id = 2;
        //    connectedItem3.Name = TextCodeTranslator.Translate("Customs.SupplierInvoiceItem");
        //    this.ConnectedToItems.push(connectedItem1);
        //    this.ConnectedToItems.push(connectedItem2);
        //    this.ConnectedToItems.push(connectedItem3);
        // }
        // else if (entityCode.toLowerCase() == "claim") {
        //    this.ConnectedToItems.push(new ConnectedToItem() { Id = 0, Name = TextCodeTranslator.Translate("Customs.Claim") });
        //    this.ConnectedToItems.push(new ConnectedToItem() { Id = 1, Name = TextCodeTranslator.Translate("Customs.ClaimsRelatedEntity") });
        // }
        // else if (entityCode.toLowerCase() == "customscollateral") {
        //    this.ConnectedToItems.push(new ConnectedToItem() { Id = 0, Name = TextCodeTranslator.Translate("Customs.CustomsCollateral") });
        // }
    };
    AddEditCustomsDocumentComponent.prototype.ConnectedItemSelectionChanged = function (index) {
        if (index != 0) {
            var selectInvoicesOnly = true;
            if (index == 2) {
                selectInvoicesOnly = false;
            }
            this.ShowSelectionComponent(selectInvoicesOnly);
        }
    };
    //if(index != 0) {
    //    bool selectInvoicesOnly = true;
    //    if (index == 2) {
    //        selectInvoicesOnly = false;
    //    }
    //    SimplogWindow window = new SimplogWindow();
    //    window.Height = 700;
    //    window.Width = 1000;
    //    window.ShowCloseButtonOnly = true;
    //    if (this.objectTableName == "Customs.Declaration") {
    //        CustomsDocumentPointerChildSelectionViewModel viewModel = new CustomsDocumentPointerChildSelectionViewModel(selectInvoicesOnly, this.context, window, CustomsDocumentsTicket, null, parentEntityId, entityChild1Id, entityChild2Id, entityChild3Id);
    //        viewModel.ChildrenChoosingCompleted += viewModel_ChildrenChoosingCompleted;
    //        CustomsDocumentPointerChildSelectionControl control = new CustomsDocumentPointerChildSelectionControl() { DataContext = viewModel };
    //        window.Add(control);
    //        window.Show();
    //    }
    //    else if (this.objectTableName == "Customs.Claim") {
    //        ClaimRelatedEntitiesPointersSelectionViewModel CREViewModel = new ClaimRelatedEntitiesPointersSelectionViewModel(new ClaimDomainContext(), window, CustomsDocumentsTicket, parentEntityId, entityChild1Id, entityChild2Id, entityChild3Id);
    //        CREViewModel.ChildrenChoosingCompleted += CREViewModel_ChildrenChoosingCompleted;
    //        ClaimRelatedEntitiesPointersSelectionControl control = new ClaimRelatedEntitiesPointersSelectionControl() { DataContext = CREViewModel };
    //        window.Height = 500;
    //        window.Width = 800;
    //        window.Add(control);
    //        window.Show();
    //    }
    //}
    AddEditCustomsDocumentComponent.prototype.ShowSelectionComponent = function (selectInvoicesOnly) {
        var _this = this;
        if (selectInvoicesOnly === void 0) { selectInvoicesOnly = false; }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket)) {
            this.iCustomsDocumentsController.SelectionCompleted.subscribe(function (s) {
                _this.SelectionCompleted(s);
            });
            this.iCustomsDocumentsController.ShowSelectionComponent(this.CustomsDocumentsTicket, this.EntityPM, selectInvoicesOnly, this.IsEntityDisplayOnly);
        }
    };
    AddEditCustomsDocumentComponent.prototype.RefereshConnectedInvoices = function () {
        if (this.CustomsDocumentsTicket != null) {
            this.SupplierInvoiceItemNumber = this.CustomsDocumentsTicket.ConnectedInvoiceItemsSequences;
            this.SupplierInvoiceNumber = this.CustomsDocumentsTicket.ConnectedInvoicesSequences;
            this.CRENumber = this.CustomsDocumentsTicket.ConnectedCREsSequences;
            this.DisplayConnectedEntityNumber = this.CustomsDocumentsTicket.ConnectedInvoicesSequences != null ? this.CustomsDocumentsTicket.ConnectedInvoicesSequences : this.CustomsDocumentsTicket.ConnectedCREsSequences;
        }
    };
    AddEditCustomsDocumentComponent.prototype.BuildTabs = function () {
        this.TabsSource = [];
        if (!this.IsUserTabSelected) {
            this.SelectedTab = "CustomsDocumentRemarks";
        }
        else {
            this.SelectedTab = "UserRemarks";
        }
        if (this.RemarksTextBoxVisiblity) {
            this.TabsSource.push({ Name: "CustomsDocumentRemarks", isSelected: true, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CustomsDocumentRemarks") });
        }
        else if (this.SelectedTab != "UserRemarks") {
            this.SelectedTab = "VerificationRemarks";
        }
        if (this.VerificationRemarksTextBoxVisiblity) {
            this.TabsSource.push({ Name: "VerificationRemarks", isSelected: false, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocumentsTicket.F.VerificationRemarks") });
        }
        if (this.UserRemarksTextBoxVisiblity) {
            this.TabsSource.push({ Name: "UserRemarks", isSelected: this.IsUserTabSelected, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocumentsTicket.F.UserRemarks") });
        }
    };
    AddEditCustomsDocumentComponent.prototype.SelectionChanged = function (tab) {
        this.TabsSource.forEach(function (item) {
            item.isSelected = false;
        });
        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab);
            return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    };
    //#endregion
    AddEditCustomsDocumentComponent.prototype.PerformSubmitChanges = function () {
        var _this = this;
        if (this.CustomsDocument) {
            if (this.CustomsDocument.IsDirty) {
                var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                customsDocumentPMService.update(this.CustomsDocument).subscribe(function (docRes) {
                    if (!docRes.HasError) {
                        _this.SubmitTicketChanges();
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                        if (docRes.ErrorsArray && docRes.ErrorsArray.length > 0) {
                            _this.ValidationErrorsList = docRes.ErrorsArray;
                        }
                        //let jDoit = false;
                        //if (jDoit && !AppTool.IsNullOrEmpty(docRes.ErrorsArray[0])) {//in customsDocumentPMService.update there is message : לא נמצא כרטיס חתימה חברתי (מסר 2715)
                        //    this.CurrentSession.StopBusyIndicator();
                        //    var messageWindow = new MessageWindow();
                        //    messageWindow.Width = 400;
                        //    messageWindow.Height = 200;
                        //    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        //    messageWindow.Show(docRes.ErrorsArray[0]);
                        //    messageWindow.WindowClosed.subscribe((event: any) => {
                        //        messageWindow.Close();
                        //    });
                        //}
                    }
                });
            }
        }
        else {
            this.SubmitTicketChanges();
        }
    };
    AddEditCustomsDocumentComponent.prototype.SubmitTicketChanges = function () {
        var _this = this;
        if (this.CustomsDocumentsTicket) {
            if (this.CustomsDocumentsTicket.IsDirty) {
                var customsDocumentsTicketPMService = new CustomsDocumentsTicketPMService_1.CustomsDocumentsTicketPMService();
                customsDocumentsTicketPMService.update(this.CustomsDocumentsTicket).subscribe(function (ticketRes) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!ticketRes.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 200;
                        messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show(ticketRes.ErrorsArray[0]);
                        messageWindow.WindowClosed.subscribe(function (event) {
                            messageWindow.Close();
                        });
                    }
                });
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    AddEditCustomsDocumentComponent.prototype.InitializeMetaData = function (previousList) {
        var _this = this;
        this.previousValueList = previousList;
        var custDocTypeMetaDataWebService = new CustDocTypeMetaDataWebService_1.CustDocTypeMetaDataWebService();
        var customDocumentTypeListService = new CustomDocumentTypeListService_1.CustomDocumentTypeListService();
        custDocTypeMetaDataWebService.GetCustomDocumentTypeMetaDataByType(this.CustomsDocument.DocumentTypeCode).subscribe(function (res) {
            _this.customDocumentTypeMetaDataList = res.Result;
            _this.customDocumentMetaDataValueList = _this.CustomsDocument.CustomsDocumentMetaDataValues;
            var customsClosedTableListService = new CustomsClosedTableListService_1.CustomsClosedTableListService();
            customsClosedTableListService.getAll().subscribe(function (resp) {
                _this.customsClosedTableList = resp.Result;
                customDocumentTypeListService.getSingle(_this.CustomsDocument.DocumentTypeCode).subscribe(function (docTypeRes) {
                    if (_this.previousValueList != null) {
                        _this.customDocumentTypeMetaDataList.forEach(function (metaData) {
                            var value = _this.customDocumentMetaDataValueList.filter(function (d) { return d.MetaDataTypeCode == metaData.MetaDataTypeCode; })[0];
                            if (value == null) {
                                value = new CustomsDocumentMetaDataValuePM_1.CustomsDocumentMetaDataValuePM(_this.CustomsDocument);
                                value.MetaDataTypeCode = metaData.MetaDataTypeCode;
                                value.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                value.CustomsDocumentId = _this.CustomsDocument.DocumentsFilingId;
                                value.ChangeSetOp = "Insert";
                                if (docTypeRes.Result) {
                                    if (docTypeRes.Result.AutoSetOriginalDocumentTrue) {
                                        if (value.MetaDataTypeCode == "87") {
                                            value.MetaDataValue = "True";
                                        }
                                    }
                                }
                                _this.customDocumentMetaDataValueList.push(value);
                            }
                        });
                        _this.SetCommonMetaDataValues(_this.previousValueList, docTypeRes.Result);
                    }
                    else {
                        _this.GenerateControl(docTypeRes.Result);
                    }
                });
            });
        });
    };
    AddEditCustomsDocumentComponent.prototype.SetCommonMetaDataValues = function (previousValues, docType) {
        var _this = this;
        previousValues.forEach(function (valuePM) {
            var newValue = _this.customDocumentMetaDataValueList.filter(function (d) { return d.MetaDataTypeCode == valuePM.MetaDataTypeCode; })[0];
            if (newValue != null) {
                newValue.MetaDataValue = valuePM.MetaDataValue;
            }
        });
        this.GenerateControl(docType);
    };
    AddEditCustomsDocumentComponent.prototype.GenerateControl = function (docType) {
        var _this = this;
        this.MetaDataViewModels = [];
        this.customDocumentTypeMetaDataList = this.customDocumentTypeMetaDataList.sort(function (a, b) {
            return (a.Mandatory === b.Mandatory) ? 0 : (a.Mandatory < b.Mandatory) ? 1 : -1;
        });
        this.customDocumentTypeMetaDataList.forEach(function (type) {
            var value = _this.customDocumentMetaDataValueList.filter(function (d) { return d.MetaDataTypeCode == type.MetaDataTypeCode; })[0];
            var metaDataViewModel = new MetaDataViewModel(type, value, _this.CustomsDocument, _this.customsClosedTableList, docType);
            _this.MetaDataViewModels.push(metaDataViewModel);
        });
    };
    AddEditCustomsDocumentComponent.prototype.GetRequiredFieldsErrors = function () {
        var _this = this;
        var requiredFieldsErrors = [];
        this.MetaDataViewModels.forEach(function (viewModel) {
            var metaDataType = _this.customDocumentTypeMetaDataList.filter(function (d) { return d.MetaDataTypeCode == viewModel.MetaDataType.MetaDataTypeCode; })[0];
            if (metaDataType.Mandatory) {
                if (Tools_1.AppTool.IsNullOrEmpty(viewModel.MetaDataValue.MetaDataValue)) {
                    requiredFieldsErrors.push(metaDataType.MetaDataTypeName + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.IsRequired"));
                }
            }
        });
        return requiredFieldsErrors;
    };
    AddEditCustomsDocumentComponent.prototype.SelectionCompleted = function (args) {
        this.RefereshConnectedInvoices();
    };
    AddEditCustomsDocumentComponent.prototype.ClearAllMetaDataValues = function () {
        var _this = this;
        this.previousValueList = [];
        this.customDocumentMetaDataValueList.forEach(function (value) {
            _this.previousValueList.push(value);
        });
        this.previousValueList.forEach(function (value) {
            var exists = _this.CustomsDocument.CustomsDocumentMetaDataValues.filter(function (d) { return d.MetaDataTypeCode == value.MetaDataTypeCode; })[0];
            if (exists) {
                _this.CustomsDocument.RemoveCustomsDocumentMetaDataValue(exists);
            }
        });
    };
    AddEditCustomsDocumentComponent.prototype.NewVersion = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.ShowNoButton = true;
        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsDocuments.NewVersionWarning"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CustomsDocument.DocumentVersion = _this.CustomsDocument.DocumentVersion + 1;
                _this.CustomsDocument.DocumentStatusCode = null;
                _this.CustomsDocument.CustomRecievedDate = null;
                _this.CustomsDocument.CustomsDocId = null;
                _this.CustomsDocument.ForceRemoveCustomsDocId = true;
                confirmWindow.Close();
                _this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Saving"));
                var customsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                customsDocumentPMService.update(_this.CustomsDocument).subscribe(function (docRes) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!docRes.HasError) {
                        // this.CheckEditEnabled(this.IsCustomsDocumentInRequest, !this.IsDisplayOnly);
                        _this.WindowArgs.CustomsDocument = docRes.Result;
                        _this.SetWindowArgs(_this.WindowArgs);
                        _this.newVersionAdded = true;
                    }
                    else {
                        _this.ValidationErrorsList = docRes.ErrorsArray;
                    }
                });
            }
            else {
                confirmWindow.Close();
            }
        });
    };
    AddEditCustomsDocumentComponent.prototype.OnTextAreaKeyDown = function (event) {
        event.preventDefault();
    };
    AddEditCustomsDocumentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditCustomsDocumentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditCustomsDocumentComponent);
    return AddEditCustomsDocumentComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomsDocumentComponent = AddEditCustomsDocumentComponent;
var MetaDataViewModel = /** @class */ (function (_super) {
    __extends(MetaDataViewModel, _super);
    function MetaDataViewModel(MetaDataType, MetaDataValue, CustomsDocument, closedTables, docType) {
        var _this = _super.call(this) || this;
        _this.MetaDataType = MetaDataType;
        _this.MetaDataValue = MetaDataValue;
        _this.CustomsDocument = CustomsDocument;
        _this.closedTables = closedTables;
        if (!_this.MetaDataValue) {
            _this.MetaDataValue = new CustomsDocumentMetaDataValuePM_1.CustomsDocumentMetaDataValuePM(CustomsDocument);
            _this.MetaDataValue.CustomsDocumentId = _this.CustomsDocument.DocumentsFilingId;
            _this.MetaDataValue.MetaDataTypeCode = _this.MetaDataType.MetaDataTypeCode;
            _this.MetaDataValue.Tenant = SessionLocator_1.SessionLocator.Tenant;
            if (docType && docType.AutoSetOriginalDocumentTrue && _this.MetaDataValue.MetaDataTypeCode == "87") {
                _this.MetaDataValue.MetaDataValue = "True";
            }
        }
        if (MetaDataType.ValuesTable) {
            var currentClosedTable = _this.closedTables.filter(function (d) { return d.Id == MetaDataType.ValuesTable; })[0];
            var lookUpTable = window.ObjectTables.filter(function (d) { return d.Id === currentClosedTable.ObjectTableId; })[0];
            _this.ValuesTableName = lookUpTable.Name;
        }
        switch (MetaDataType.Format.toLocaleLowerCase()) {
            case "string": {
                if (_this.MetaDataType.ValuesTable) {
                    _this.controlType = 'loglov';
                }
                else {
                    _this.controlType = 'logtextbox';
                }
                break;
            }
            case "int":
                {
                    _this.controlType = 'logtextboxint';
                    break;
                }
            case "date":
                {
                    _this.GetDateValue();
                    _this.controlType = 'datepicker';
                    break;
                }
            case "boolean":
                {
                    _this.controlType = 'boolean';
                    break;
                }
        }
        return _this;
    }
    Object.defineProperty(MetaDataViewModel.prototype, "DateMetaDataValue", {
        get: function () {
            return this.dateMetaDataValue;
        },
        set: function (newValue) {
            this.SetDateValue(newValue);
        },
        enumerable: true,
        configurable: true
    });
    MetaDataViewModel.prototype.GetDateValue = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MetaDataValue.MetaDataValue)) {
            var date;
            var dateArray = this.MetaDataValue.MetaDataValue.split('.');
            var day = Number(dateArray[0]);
            var month = Number(dateArray[1]);
            var year = Number(dateArray[2]);
            var nowDate = new Date();
            var currentYear = nowDate.getFullYear();
            var currentYearMillinium = currentYear.toString().substring(0, 1);
            currentYearMillinium = currentYearMillinium + "000";
            var currentMillinium = Number(currentYearMillinium);
            if (year == 0) {
                year = currentYear;
            }
            if (year < 1000) {
                year = year + currentMillinium;
            }
            this.dateMetaDataValue = year + "." + this.ApplyPadding(month + "") + "." + this.ApplyPadding(day + "");
        }
    };
    MetaDataViewModel.prototype.SetDateValue = function (newValue) {
        if (newValue instanceof Date) {
            var valueDate = newValue;
            var day = valueDate.getUTCDate();
            var month = valueDate.getUTCMonth() + 1;
            var year = valueDate.getUTCFullYear();
            var shortYear = (year + "").substr(2, 2);
            this.MetaDataValue.MetaDataValue = this.ApplyPadding(day + "") + "." + this.ApplyPadding(month + "") + "." + shortYear;
        }
    };
    MetaDataViewModel.prototype.ApplyPadding = function (str) {
        var pad = "00";
        var ans = pad.substring(0, pad.length - str.length) + str;
        return ans;
    };
    return MetaDataViewModel;
}(BaseComponent_1.BaseComponent));
exports.MetaDataViewModel = MetaDataViewModel;
//# sourceMappingURL=AddEditCustomsDocumentComponent.js.map