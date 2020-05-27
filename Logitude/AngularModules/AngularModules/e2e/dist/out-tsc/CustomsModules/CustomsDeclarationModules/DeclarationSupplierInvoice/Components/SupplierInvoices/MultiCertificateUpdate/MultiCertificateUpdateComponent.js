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
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var MultiCertificatesService_1 = require("../../../../../../Customs/Services/Others/MultiCertificatesService");
var MessageWindow_1 = require("../../../../../../Controls/Windows/MessageWindow");
var MultiCertificateUpdateComponent = /** @class */ (function (_super) {
    __extends(MultiCertificateUpdateComponent, _super);
    function MultiCertificateUpdateComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CertTableName = "Customs.SupplierInvioceItemCertificat";
        _this.IsDisplayOnly = false;
        _this._MultiCertificatesService = new MultiCertificatesService_1.MultiCertificatesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Properties
        _this._ExternalRequestTypeCode = null;
        _this._ApprovalRequestNumber = null;
        _this._ReqConfirmationTypeCode = null;
        _this._AttachmentTypeCode = null;
        _this._CertificateNumber = null;
        _this._CertificateExemptionTypeCode = null;
        _this._ResConfirmationTypeCode = null;
        _this.SetUIProperties();
        return _this;
    }
    MultiCertificateUpdateComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.InvoicPM = args.EntityPM;
        }
    };
    MultiCertificateUpdateComponent.prototype.SetUIProperties = function () {
        //all cases required
        this.UIProperties.SetRequired("ExternalRequestTypeCode", this.CertTableName, true);
        this.UIProperties.SetValidity("ExternalRequestTypeCode", this.CertTableName, true, "");
        this.UIProperties.SetRequired("ApprovalRequestNumber", this.CertTableName, true);
        this.UIProperties.SetValidity("ApprovalRequestNumber", this.CertTableName, true, "");
        this.UIProperties.SetRequired("ReqConfirmationTypeCode", this.CertTableName, true);
        this.UIProperties.SetValidity("ReqConfirmationTypeCode", this.CertTableName, true, "");
        this.UIProperties.SetRequired("AttachmentTypeCode", this.CertTableName, true);
        this.UIProperties.SetValidity("AttachmentTypeCode", this.CertTableName, true, "");
        if (this.AttachmentTypeCode == "1" || this.AttachmentTypeCode == "2") {
            this.UIProperties.SetRequired("CertificateNumber", this.CertTableName, true); //required
            this.UIProperties.SetValidity("CertificateNumber", this.CertTableName, true, "");
            this.UIProperties.SetRequired("ResConfirmationTypeCode", this.CertTableName, true); //required
            this.UIProperties.SetValidity("ResConfirmationTypeCode", this.CertTableName, true, "");
            this.UIProperties.SetRequired("CertificateExemptionTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateExemptionTypeCode", this.CertTableName, true, "");
        }
        else if (this.AttachmentTypeCode == "3") {
            this.UIProperties.SetRequired("CertificateNumber", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateNumber", this.CertTableName, true, "");
            this.UIProperties.SetRequired("ResConfirmationTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("ResConfirmationTypeCode", this.CertTableName, true, "");
            this.UIProperties.SetRequired("CertificateExemptionTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateExemptionTypeCode", this.CertTableName, true, "");
        }
        else if (this.AttachmentTypeCode == "4") {
            this.UIProperties.SetRequired("CertificateNumber", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("CertificateNumber", this.CertTableName, true, "");
            this.UIProperties.SetRequired("ResConfirmationTypeCode", this.CertTableName, false); //optional
            this.UIProperties.SetValidity("ResConfirmationTypeCode", this.CertTableName, true, "");
            this.UIProperties.SetRequired("CertificateExemptionTypeCode", this.CertTableName, true); //required
            this.UIProperties.SetValidity("CertificateExemptionTypeCode", this.CertTableName, true, "");
        }
    };
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "ExternalRequestTypeCode", {
        get: function () { return this._ExternalRequestTypeCode; },
        set: function (value) { this._ExternalRequestTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "ApprovalRequestNumber", {
        get: function () { return this._ApprovalRequestNumber; },
        set: function (value) { this._ApprovalRequestNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "ReqConfirmationTypeCode", {
        get: function () { return this._ReqConfirmationTypeCode; },
        set: function (value) {
            this._ReqConfirmationTypeCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "AttachmentTypeCode", {
        get: function () { return this._AttachmentTypeCode; },
        set: function (value) {
            this._AttachmentTypeCode = value;
            this.SetUIProperties();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "CertificateNumber", {
        get: function () { return this._CertificateNumber; },
        set: function (value) { this._CertificateNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "CertificateExemptionTypeCode", {
        get: function () { return this._CertificateExemptionTypeCode; },
        set: function (value) { this._CertificateExemptionTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultiCertificateUpdateComponent.prototype, "ResConfirmationTypeCode", {
        get: function () { return this._ResConfirmationTypeCode; },
        set: function (value) {
            this._ResConfirmationTypeCode = value;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    MultiCertificateUpdateComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    MultiCertificateUpdateComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        this.ValidationErrorsList = errors;
        //#region Manditory fields validation
        if (!this.ExternalRequestTypeCode ||
            !this.ApprovalRequestNumber ||
            !this.ReqConfirmationTypeCode ||
            !this.AttachmentTypeCode) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AllFieldsAreRequired"));
        }
        else if (this.AttachmentTypeCode == "1" || this.AttachmentTypeCode == "2") {
            if (!this.CertificateNumber || !this.ResConfirmationTypeCode)
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AllFieldsAreRequired"));
        }
        else if (this.AttachmentTypeCode == "4") {
            if (!this.CertificateExemptionTypeCode)
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.AllFieldsAreRequired"));
        }
        //#endregion 
        if (errors.length == 0) {
            //this.UpdateChanges();
            this.UpdateChangesOnClientSide();
        }
        else {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
    };
    MultiCertificateUpdateComponent.prototype.UpdateChanges = function () {
        // this is method is not used
        // to use it , call this function, and change code when closing
        // multi certificate window
        var _this = this;
        this._MultiCertificatesService.UpdateCertificatesBySearchFields(this.InvoicPM.DeclarationId, this.InvoicPM.InvoiceCounterKey, this.ExternalRequestTypeCode, this.ApprovalRequestNumber, this.ReqConfirmationTypeCode, this.AttachmentTypeCode, this.CertificateNumber, this.CertificateExemptionTypeCode, this.ResConfirmationTypeCode).subscribe(function (myResponse) {
            if (myResponse != null) {
                var updatedRowCount = myResponse.Result;
                if (updatedRowCount != null && updatedRowCount != undefined) {
                    if (updatedRowCount == 0) {
                        var msg = new MessageWindow_1.MessageWindow();
                        msg.RTL = true;
                        msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Nomatchinglineswerefound"));
                    }
                    else {
                        var msg = new MessageWindow_1.MessageWindow();
                        var txt = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.itemswereupdated");
                        msg.RTL = true;
                        msg.ShowSuccessIcon = true;
                        msg.WindowClosed.subscribe(function () {
                            _this.CurrentSession.CloseCurrentWindowEmit("ok");
                        });
                        msg.Show(txt.replace("#Number", updatedRowCount + ""));
                    }
                }
                else {
                    console.error("ERROR IN SERVICE!!!!!", myResponse.ErrorsArray);
                }
            }
        });
    };
    MultiCertificateUpdateComponent.prototype.UpdateChangesOnClientSide = function () {
        var _this = this;
        var updatedRowCount = 0;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Updating...");
        //Update certificates by search fields
        if (this.InvoicPM.SupplierInvoiceItems) {
            this.InvoicPM.SupplierInvoiceItems.forEach(function (item) {
                if (item.SupplierInvioceItemCertificats) {
                    item.SupplierInvioceItemCertificats.forEach(function (cert) {
                        //check by search fields
                        if (cert.ExternalRequestTypeCode == _this.ExternalRequestTypeCode && cert.ApprovalRequestNumber == _this.ApprovalRequestNumber) {
                            cert.ReqConfirmationTypeCode = _this.ReqConfirmationTypeCode;
                            cert.AttachmentTypeCode = _this.AttachmentTypeCode;
                            cert.CertificateNumber = _this.CertificateNumber;
                            cert.CertificateExemptionTypeCode = _this.CertificateExemptionTypeCode;
                            cert.ResConfirmationTypeCode = _this.ResConfirmationTypeCode;
                            updatedRowCount++;
                        }
                        //update invoice item status
                        _this.UpdateCertStatusAlaaMethod(cert, item);
                        //this.UpdateCertStatus(cert, item);
                    });
                }
            });
        }
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        // Show response message
        if (updatedRowCount == 0) {
            var msg = new MessageWindow_1.MessageWindow();
            msg.RTL = true;
            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Nomatchinglineswerefound"));
        }
        else {
            var msg = new MessageWindow_1.MessageWindow();
            var txt = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.itemswereupdated");
            msg.RTL = true;
            msg.ShowSuccessIcon = true;
            msg.WindowClosed.subscribe(function () {
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
            });
            msg.Show(txt.replace("#Number", updatedRowCount + ""));
        }
    };
    //itzik method
    MultiCertificateUpdateComponent.prototype.UpdateCertStatus = function (cert, item) {
        if (cert && item) {
            var statusCode;
            if (cert.AttachmentTypeCode == null) {
                statusCode = "2";
            }
            else if (cert.AttachmentTypeCode == "1" || cert.AttachmentTypeCode == "2") {
                if (!(cert.CertificateNumber) || !(cert.ReqConfirmationTypeCode) || !(cert.ResConfirmationTypeCode) || (cert.CertificateExemptionTypeCode)) {
                    statusCode = "2";
                }
                else {
                    statusCode = "1";
                }
            }
            else if (cert.AttachmentTypeCode == "4") {
                if (!(cert.CertificateExemptionTypeCode) || !(cert.ReqConfirmationTypeCode) || (cert.CertificateNumber) || (cert.ResConfirmationTypeCode)) {
                    statusCode = "2";
                }
                else {
                    statusCode = "1";
                }
            }
            else {
                statusCode = "1";
            }
            item.CertificatesStatusCode = statusCode;
        }
    };
    MultiCertificateUpdateComponent.prototype.UpdateCertStatusAlaaMethod = function (cert, item) {
        if (cert && item) {
            var statusCode;
            var valid = true;
            var hasRequest = !Tools_1.AppTool.IsNullOrEmpty(cert.ApprovalRequestNumber);
            if (cert.AttachmentTypeCode == null) {
                valid = false;
            }
            else {
                if (cert.AttachmentTypeCode == "1" || cert.AttachmentTypeCode == "2") {
                    if (Tools_1.AppTool.IsNullOrEmpty(cert.CertificateNumber) || Tools_1.AppTool.IsNullOrEmpty(cert.ReqConfirmationTypeCode) || Tools_1.AppTool.IsNullOrEmpty(cert.ResConfirmationTypeCode)) {
                        valid = false;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(cert.CertificateExemptionTypeCode) || !Tools_1.AppTool.IsNullOrEmpty(cert.CustomsAttachmentID)) {
                        valid = false;
                    }
                }
                else {
                    if (cert.AttachmentTypeCode == "4") {
                        if (Tools_1.AppTool.IsNullOrEmpty(cert.CertificateExemptionTypeCode) || Tools_1.AppTool.IsNullOrEmpty(cert.ReqConfirmationTypeCode)) {
                            valid = false;
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(cert.CertificateNumber) || !Tools_1.AppTool.IsNullOrEmpty(cert.ResConfirmationTypeCode) || !Tools_1.AppTool.IsNullOrEmpty(cert.CustomsAttachmentID)) {
                            valid = false;
                        }
                    }
                }
            }
            if (valid) {
                if (hasRequest) {
                    item.CertificatesStatusCode = "3";
                }
                else {
                    item.CertificatesStatusCode = "1";
                }
            }
            else {
                if (hasRequest) {
                    item.CertificatesStatusCode = "4";
                }
                else {
                    item.CertificatesStatusCode = "2";
                }
            }
        }
    };
    MultiCertificateUpdateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MultiCertificateUpdateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MultiCertificateUpdateComponent);
    return MultiCertificateUpdateComponent;
}(BaseComponent_1.BaseComponent));
exports.MultiCertificateUpdateComponent = MultiCertificateUpdateComponent;
//# sourceMappingURL=MultiCertificateUpdateComponent.js.map