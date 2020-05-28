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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CertificateTicket_1 = require("../../../../../Customs/DataContract/CertificateTicket");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var MultiCertificatesService_1 = require("../../../../../Customs/Services/Others/MultiCertificatesService");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var AmitalGatewayUtil_1 = require("../../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var ConfirmationTypeListService_1 = require("../../../../../Customs/Services/StandardLists/ConfirmationTypeListService");
var CreateEditTicketComponent = /** @class */ (function (_super) {
    __extends(CreateEditTicketComponent, _super);
    function CreateEditTicketComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.SupplierInvioceItemCertificat";
        _this.DataContext = _this;
        _this.connectedItems = [];
        _this.Items = [];
        _this.multiCertificatesService = new MultiCertificatesService_1.MultiCertificatesService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this.ExcludedItems = [];
        _this._CardListService = new CardListService_1.CardListService();
        _this._ConfirmationTypeListService = new ConfirmationTypeListService_1.ConfirmationTypeListService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ExemptionFilterSelectedValue = 'other';
        _this.ResConfirmationFilterSelectedValue = "request";
        _this.notMandatoryIsNotEmpty = false;
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        return _this;
    }
    CreateEditTicketComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.IsSearchIconVisibile = false;
        this.connectedItems = [];
        this.Items = [];
        this.customsSettingListService.getAll().subscribe(function (response) {
            var list = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                var customsSetting = 
                //list[0];
                list.filter(function (d) { return d.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0];
                if (!Tools_1.AppTool.IsNullOrEmpty(customsSetting)) {
                    if (customsSetting.UnifreightCertificateActivated) {
                        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                        var myDec = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        //if (AmitalGatewayUtil.Instance.IsDeclarationInUse(myDec.CustomFileNo, myDec.IsConvertedDeclaration, myDec.IsConnectedToUnifreight)) {
                        if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) { //'Search Certificate Document' Icon Is Not Appearing - Certificate Multi Entry - Edit Declaration 
                            _this.IsSearchIconVisibile = true;
                        }
                    }
                    //else {
                    //    this.IsSearchIconVisibile = false;
                    //}
                }
            }
        });
        this.ticket = args.Ticket;
        this.isNew = args.IsNew;
        this.connectedItems = args.ConnectedItems;
        this.ExcludedItems = args.ExcludedItems;
        this.IsAllSelected = args.IsAllSelected;
        this.Items = args.Items;
        this.declarationId = args.DeclarationId;
        this.oldAttachment = this.ticket.AttachmentTypeCode;
        this.oldCertificateNumber = this.ticket.CertificateNumber;
        this.oldCertificateExempt = this.ticket.CertificateExemptionTypeCode;
        this.oldResConfirmation = this.ticket.ResConfirmationTypeCode;
        this.Parent = args.Parent;
        this.ReqConfirmationTypeCode = this.ticket.ReqConfirmationTypeCode;
        if (!this.isNew) {
            this.FilterSelectedValue = this.ticket.AttachmentTypeCode;
            this.AttachmentTypeCode = this.ticket.AttachmentTypeCode;
            this.CertificateNumber = this.ticket.CertificateNumber;
            this.CertificateExemptionTypeCode = this.ticket.CertificateExemptionTypeCode;
            this.ResConfirmationTypeCode = this.ticket.ResConfirmationTypeCode;
            this.ReqConfirmationTypeCode = this.ticket.ReqConfirmationTypeCode;
        }
        if (this.ticket.CertificateExemptionTypeCode == "92") {
            this.ExemptionFilterSelectedValue = "92";
        }
        else if (this.ticket.CertificateExemptionTypeCode == "96") {
            this.ExemptionFilterSelectedValue = "96";
        }
        else {
            this.ExemptionFilterSelectedValue = "other";
        }
        this.SetFieldsVisibility(this.FilterSelectedValue);
        this.SetFieldsDisabled(this.isNew);
    };
    CreateEditTicketComponent.prototype.SetFieldsDisabled = function (isNew) {
        if (isNew || this.AttachmentTypeCode == "3" || this.AttachmentTypeCode == null) {
            this.UIProperties.SetEnabled("CertificateNumber", "Customs.SupplierInvioceItemCertificat", false);
            this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
        }
        else if (this.CertificateExemptionTypeCode == "96" || this.CertificateExemptionTypeCode == "92") {
            this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
        }
        else if (this.ResConfirmationTypeCode == this.ReqConfirmationTypeCode) {
            this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
        }
        //  this.ResConfirmationFilterSelectedValue = "request";
    };
    Object.defineProperty(CreateEditTicketComponent.prototype, "CertificateNumber", {
        get: function () { return this.certificateNumber; },
        set: function (newValue) {
            this.certificateNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreateEditTicketComponent.prototype, "ResConfirmationTypeCode", {
        get: function () { return this.resConfirmationTypeCode; },
        set: function (newValue) {
            this.resConfirmationTypeCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreateEditTicketComponent.prototype, "ReqConfirmationTypeCode", {
        get: function () { return this.reqConfirmationTypeCode; },
        set: function (newValue) {
            this.reqConfirmationTypeCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreateEditTicketComponent.prototype, "AttachmentTypeCode", {
        get: function () { return this.attachmentTypeCode; },
        set: function (newValue) {
            this.attachmentTypeCode = newValue;
            this.UIProperties.SetEnabled("CertificateNumber", "Customs.SupplierInvioceItemCertificat", true);
            this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", true);
            this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", true);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreateEditTicketComponent.prototype, "CertificateExemptionTypeCode", {
        get: function () { return this.certificateExemptionTypeCode; },
        set: function (newValue) {
            this.certificateExemptionTypeCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    CreateEditTicketComponent.prototype.SetFieldsVisibility = function (value) {
        switch (value) {
            case "1":
                {
                    this.IsConfirmationVisibile = true;
                    this.IsExemptVisibile = false;
                    this.ExemptRowHeight = 0;
                    this.ResConfirmationFilterVisibility = false;
                    break;
                }
            case "2": {
                this.IsConfirmationVisibile = true;
                this.IsExemptVisibile = false;
                this.ExemptRowHeight = 0;
                this.ResConfirmationFilterVisibility = true;
                this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
                this.ResConfirmationTypeCode = this.ReqConfirmationTypeCode;
                break;
            }
            case "4": {
                this.IsConfirmationVisibile = false;
                this.IsExemptVisibile = true;
                this.ExemptRowHeight = 30;
                this.ResConfirmationFilterVisibility = false;
                break;
            }
            default: {
                this.IsConfirmationVisibile = true;
                this.IsExemptVisibile = false;
                this.ExemptRowHeight = 0;
                this.ResConfirmationFilterVisibility = false;
                break;
            }
        }
        //if (this.ticket.ResConfirmationTypeCode == "102" || this.ticket.ResConfirmationTypeCode == "219") {
        if (this.ResConfirmationTypeCode == this.ReqConfirmationTypeCode) {
            this.ResConfirmationFilterSelectedValue = "request";
        }
        else {
            this.ResConfirmationFilterSelectedValue = "other";
        }
    };
    CreateEditTicketComponent.prototype.FilterItemClicked = function (itemValue) {
        var _this = this;
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = null;
            this.ExemptionFilterSelectedValue = "other";
            this.ResConfirmationFilterSelectedValue = "request";
            if (this.CertificateNumber != null || this.ResConfirmationTypeCode != null || this.CertificateExemptionTypeCode != null) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeletingDetails");
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 300;
                confirmWindow.Height = 150;
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                confirmWindow.Show(msg);
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CertificateNumber = null;
                        _this.ResConfirmationTypeCode = null;
                        _this.CertificateExemptionTypeCode = null;
                        _this.FilterSelectedValue = itemValue;
                        _this.AttachmentTypeCode = itemValue;
                        _this.SetFieldsVisibility(itemValue);
                    }
                    else {
                        _this.FilterSelectedValue = _this.AttachmentTypeCode;
                    }
                });
            }
            else {
                this.FilterSelectedValue = itemValue;
                this.AttachmentTypeCode = itemValue;
                this.SetFieldsVisibility(itemValue);
            }
        }
    };
    CreateEditTicketComponent.prototype.ExemptionFilterClicked = function (value) {
        if (this.ExemptionFilterSelectedValue != value) {
            this.ExemptionFilterSelectedValue = value;
            if (value == "92") {
                this.CertificateExemptionTypeCode = "92";
                this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            }
            else if (value == "96") {
                this.CertificateExemptionTypeCode = "96";
                this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            }
            else {
                this.CertificateExemptionTypeCode = null;
                this.UIProperties.SetEnabled("CertificateExemptionTypeCode", "Customs.SupplierInvioceItemCertificat", true);
            }
        }
    };
    CreateEditTicketComponent.prototype.ResConfirmationFilterClicked = function (value) {
        if (this.ResConfirmationFilterSelectedValue != value) {
            this.ResConfirmationFilterSelectedValue = value;
            if (value == "request") {
                this.ResConfirmationTypeCode = this.ReqConfirmationTypeCode;
                this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", false);
            }
            else {
                this.ResConfirmationTypeCode = null;
                this.UIProperties.SetEnabled("ResConfirmationTypeCode", "Customs.SupplierInvioceItemCertificat", true);
            }
        }
    };
    CreateEditTicketComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("no");
        // this.CurrentSession.CloseCurrentWindow();
    };
    CreateEditTicketComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    CreateEditTicketComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = [];
        //  this.connectedItems = [];
        this.isValid = true;
        this.inValid = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AttachmentTypeCode)) {
            errors.push(this.GetRequierdFieldErrorText("Customs.SupplierInvioceItemCertificat.F.AttachmentTypeCode"));
            this.isValid = false;
        }
        else {
            if (this.AttachmentTypeCode == "1" || this.AttachmentTypeCode == "2") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.CertificateNumber) || Tools_1.AppTool.IsNullOrEmpty(this.ticket.ReqConfirmationTypeCode) || Tools_1.AppTool.IsNullOrEmpty(this.ResConfirmationTypeCode)) {
                    this.isValid = false;
                    this.inValid = true;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CertificateExemptionTypeCode)) {
                    this.inValid = true;
                    this.notMandatoryIsNotEmpty = true;
                }
            }
            else {
                if (this.AttachmentTypeCode == "4") {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.CertificateExemptionTypeCode) || Tools_1.AppTool.IsNullOrEmpty(this.ticket.ReqConfirmationTypeCode)) {
                        this.isValid = false;
                        this.inValid = true;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.CertificateNumber) || !Tools_1.AppTool.IsNullOrEmpty(this.ResConfirmationTypeCode)) {
                        this.inValid = true;
                        this.notMandatoryIsNotEmpty = true;
                    }
                }
            }
        }
        if (this.inValid) {
            this.isValid = false;
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.Width = 300;
            confirm.Height = 150;
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirm.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
            if (this.notMandatoryIsNotEmpty) {
                confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CertificateNotMandatoryFields"));
            }
            else {
                confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CertificateMandatoryFields"));
            }
            confirm.WindowClosed.subscribe(function (event) {
                if (confirm.Yes) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.connectedItems) && _this.connectedItems.length > 0) {
                        var certificate = _this.Parent.CertificateTicketsList.filter(function (d) { return d.AttachmentTypeCode == _this.AttachmentTypeCode && d.CertificateNumber == _this.CertificateNumber && d.CertificateExemptionTypeCode == _this.CertificateExemptionTypeCode && d.ResConfirmationTypeCode == _this.ResConfirmationTypeCode; })[0];
                        if (certificate != null) {
                            if (certificate.ticket != null) {
                                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                confirmWindow.Width = 300;
                                confirmWindow.Height = 150;
                                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                                confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                                confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.TicketAlreadyExist"));
                                confirmWindow.WindowClosed.subscribe(function (event) {
                                    if (confirmWindow.Yes) {
                                        _this.UpdateTicket();
                                    }
                                });
                            }
                            else if (certificate.ticket == _this.ticket) {
                                _this.CancelButtonClicked();
                            }
                        }
                        else if (certificate == null) {
                            _this.UpdateTicket();
                        }
                    }
                    else {
                        _this.UpdateTicket();
                    }
                }
            });
        }
        else {
            if (errors.length == 0) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.connectedItems) && this.connectedItems.length > 0) {
                    var certificate = this.Parent.CertificateTicketsList.filter(function (d) { return d.AttachmentTypeCode == _this.AttachmentTypeCode && d.CertificateNumber == _this.CertificateNumber && d.CertificateExemptionTypeCode == _this.CertificateExemptionTypeCode && d.ResConfirmationTypeCode == _this.ResConfirmationTypeCode; })[0];
                    if (certificate != null) {
                        if (certificate.ticket == this.ticket) {
                            this.CancelButtonClicked();
                        }
                        else if (certificate.ticket != null) {
                            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            confirmWindow.Width = 300;
                            confirmWindow.Height = 150;
                            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
                            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
                            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.TicketAlreadyExist"));
                            confirmWindow.WindowClosed.subscribe(function (event) {
                                if (confirmWindow.Yes) {
                                    _this.UpdateTicket();
                                }
                            });
                        }
                    }
                    else if (certificate == null) {
                        this.UpdateTicket();
                    }
                }
                else {
                    this.UpdateTicket();
                }
            }
            else {
                this.ValidationErrorsList = errors;
            }
        }
    };
    CreateEditTicketComponent.prototype.UpdateTicket = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        var certificateTicket = new CertificateTicket_1.CertificateTicket();
        certificateTicket.DeclarationId = this.declarationId;
        certificateTicket.InvoiceNumber = null;
        certificateTicket.AttachmentTypeCode = this.AttachmentTypeCode;
        certificateTicket.CertificateNumber = this.CertificateNumber;
        certificateTicket.ResConfirmationTypeCode = this.ResConfirmationTypeCode;
        certificateTicket.CertificateExemptionTypeCode = this.CertificateExemptionTypeCode;
        certificateTicket.ReqConfirmationTypeCode = this.ticket.ReqConfirmationTypeCode;
        certificateTicket.oldAttachment = this.oldAttachment;
        certificateTicket.oldCertificateExempt = this.oldCertificateExempt;
        certificateTicket.oldCertificateNumber = this.oldCertificateNumber;
        certificateTicket.oldResConfirmation = this.oldResConfirmation;
        certificateTicket.IsAllSelected = this.IsAllSelected;
        certificateTicket.ExternalCertificatCode = this.ticket.ExternalCertificatCode; // Itzik :  Response.ExternalCertificatCode  from  UnifreightCertificateCallbackAction
        certificateTicket.SelectedItems = [];
        //if (!certificateTicket.IsAllSelected) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.connectedItems) && this.connectedItems.length > 0) {
            certificateTicket.SelectedItems = this.connectedItems;
            //   certificateTicket.SelectedItems = this.Items;
            certificateTicket.ConnectedItemsKeys = "";
            if (certificateTicket.SelectedItems) {
                certificateTicket.SelectedItems.forEach(function (item) {
                    certificateTicket.ConnectedItemsKeys = certificateTicket.ConnectedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
                });
                certificateTicket.ConnectedItemsKeys = certificateTicket.ConnectedItemsKeys.substr(1, certificateTicket.ConnectedItemsKeys.length - 1);
            }
        }
        certificateTicket.ExcludedItemsKeys = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ExcludedItems)) {
            this.ExcludedItems.forEach(function (item) {
                certificateTicket.ExcludedItemsKeys = certificateTicket.ExcludedItemsKeys + "," + item.DeclarationId + ";" + item.InvoiceCounterKey + ";" + item.LineNumber + ";" + item.ItemCertificateCounterKey;
            });
            certificateTicket.ExcludedItemsKeys = certificateTicket.ExcludedItemsKeys.substr(1, certificateTicket.ExcludedItemsKeys.length - 1);
        }
        this.multiCertificatesService.PutCertificateTickets(certificateTicket)
            .subscribe(function (response) {
            if (!response.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
                //this.CurrentSession.CloseCurrentWindow();
            }
        });
    };
    CreateEditTicketComponent.prototype.SearchMethod = function () {
        var _this = this;
        var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(function (myUnifreightMessageM) {
            if (myUnifreightMessageM.LogitudeViewModel == "CreateEditTicketComponent" &&
                (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                myUnifreightMessageM.LogitudeEntityNumber == _this.Parent.DeclarationPM.Id) {
                sub.unsubscribe();
                _this.CurrentSession.StopBusyIndicator();
                _this.UnifreightCertificateCallbackAction(myUnifreightMessageM);
            }
        });
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._CardListService.getSingle(this.Parent.DeclarationPM.CustomerId)
            .subscribe(function (res) {
            var cardList = res.Result;
            var unifaceCustId = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(cardList)) {
                unifaceCustId = cardList.Code;
                //alert(unifaceCustId);
            }
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance
                .ShowDeclarationCertificatesByGroups(_this.Parent.DeclarationPM.CustomFileNo, _this.Parent.DeclarationPM.Id, "CreateEditTicketComponent", //this.GetType().Name,
            unifaceCustId);
        });
    };
    CreateEditTicketComponent.prototype.UnifreightCertificateCallbackAction = function (unifreightMessageM) {
        //if ($instanceparent($instanceparent($instanceparent)) == "GGGQWBLOGITUDE")
        //    ;;; $$GGG_OUT = ""
        //PutItem / id v_Cert_Details , "Response.ExternalCertificatCode", SERIAL_NO.GCRCRTF
        //PutItem / id v_Cert_Details, "Response.CertificateNumber", CERTIFICATE_NO.GCRCRTF
        //PutItem / id v_Cert_Details, "Response.ResponseConfirmationTypeCode", CERTIFICATE_TYPE.GCRCRTF
        //PutItem / id v_Cert_Details, "Response.AttachmentTypeCode", cert_source.GCRCRTF
        //PutItem / Id $$GGG_OUT, "Certificate_Details", v_Cert_Details
        var _this = this;
        //endif
        var sAttachmentTypeCode = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.AttachmentTypeCode");
        var sCertificateNumber = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.CertificateNumber");
        //                   CERTIFICATE_NO
        var sResponseConfirmationTypeCode = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ResponseConfirmationTypeCode");
        //CERTIFICATE_TYPE
        var sExternalCertificatCode = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.ExternalCertificatCode");
        //  SERIAL_NO
        if (Tools_1.AppTool.IsNullOrEmpty(sExternalCertificatCode)) {
            console.log("sExternalCertificatCode is null - u did not choose any Certificate");
            return;
        }
        sResponseConfirmationTypeCode = sResponseConfirmationTypeCode.replace("0", "");
        //if (AppTool.IsNullOrEmpty(sResponseConfirmationTypeCode)) {
        //    return;
        //}
        //this.FilterItemClicked(sAttachmentTypeCode);
        this.AttachmentTypeCode = this.ticket.AttachmentTypeCode = sAttachmentTypeCode;
        var funcSetTicketAndOkClick = function () {
            _this.CertificateNumber = _this.ticket.CertificateNumber = sCertificateNumber;
            //this.CertificateExemptionTypeCode =
            _this.ResConfirmationTypeCode = _this.ticket.ResConfirmationTypeCode = sResponseConfirmationTypeCode;
            //ticket.CertificateExemptionTypeCode = "";
            _this.ticket.ExternalCertificatCode = sExternalCertificatCode;
            _this.OkButtonClicked();
        };
        if (Tools_1.AppTool.IsNullOrEmpty(sResponseConfirmationTypeCode)) {
            funcSetTicketAndOkClick();
            return;
        }
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._ConfirmationTypeListService.getSingle(sResponseConfirmationTypeCode)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var myConfirmationTypeList = res.Result;
            if (Tools_1.AppTool.IsNullOrEmpty(myConfirmationTypeList)) {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Width = 350;
                msg.Show("\u05E1\u05D5\u05D2 \u05D0\u05D9\u05E9\u05D5\u05E8 \u05DC\u05D0 \u05E7\u05D9\u05D9\u05DD \u05D1\u05DE\u05E2\u05E8\u05DB\u05EA (" + sResponseConfirmationTypeCode + ")");
                return;
            }
            funcSetTicketAndOkClick();
            //this.CertificateNumber = this.ticket.CertificateNumber = sCertificateNumber;
            ////this.CertificateExemptionTypeCode =
            //this.ResConfirmationTypeCode =this.ticket.ResConfirmationTypeCode = sResponseConfirmationTypeCode;
            ////ticket.CertificateExemptionTypeCode = "";
            //this.ticket.ExternalCertificatCode = sExternalCertificatCode;
            //this.OkButtonClicked();
        });
    };
    CreateEditTicketComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CreateEditTicketComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CreateEditTicketComponent);
    return CreateEditTicketComponent;
}(BaseComponent_1.BaseComponent));
exports.CreateEditTicketComponent = CreateEditTicketComponent;
//# sourceMappingURL=CreateEditTicketComponent.js.map