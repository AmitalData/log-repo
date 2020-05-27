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
var SupplierInvoiceItemPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM");
var SupplierInvioceItemCertificatPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvioceItemCertificatPM");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var SupplierInvoiceItemsProdIdentPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemsProdIdentPM");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var ProductIdentificationTypeListService_1 = require("../../../../../../Customs/Services/StandardLists/ProductIdentificationTypeListService");
var CertificateExemptionTypeListService_1 = require("../../../../../../Customs/Services/StandardLists/CertificateExemptionTypeListService");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var SupplierInvoicePMService_1 = require("../../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService");
var FeatureLocator_1 = require("../../../../../../Infrastructure/Utilities/FeatureLocator");
var AttachmentTypeListService_1 = require("../../../../../../Customs/Services/StandardLists/AttachmentTypeListService");
var SupplierInvoiceItemCertificatesComponent = /** @class */ (function (_super) {
    __extends(SupplierInvoiceItemCertificatesComponent, _super);
    function SupplierInvoiceItemCertificatesComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.SupplierInvioceItemCertificat";
        _this.DataContext = _this;
        _this.productIdentificationTypeListService = new ProductIdentificationTypeListService_1.ProductIdentificationTypeListService();
        _this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        _this.ValidationErrorsList = [];
        _this.IsHeaderVisible = false;
        _this.IsFromCustomsAnswers = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IKEAFeature = 'hidden';
        _this.notMandatoryIsNotEmpty = false;
        _this.SelectedRow = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        var ikeaFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "IKEA") && f.ObjectTableId == table.Id; })[0];
        if (ikeaFeature) {
            _this.IKEAFeature = 'visibile';
        }
        return _this;
    }
    SupplierInvoiceItemCertificatesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var _entityResourceService = new EntityResourceService_1.EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("Customs.CertificateExemptionType", 0).subscribe(function (res) {
            var entityListService = new CertificateExemptionTypeListService_1.CertificateExemptionTypeListService();
            entityListService.getAllFromCache().subscribe(function (res) {
            });
        });
        _entityResourceService.getEntityResourceByTableName("Customs.AttachmentType", 0).subscribe(function (res) {
            var listService = new AttachmentTypeListService_1.AttachmentTypeListService();
            listService.getAllFromCache().subscribe(function (res) {
            });
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.invoiceItemPM = args.SupplierInvoiceItemPM;
            this.BuildCertificatesList();
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.OriginalItemPM = args.SupplierInvoiceItemPM;
            this.ClonedItemPM = this.CloneEntity(args.SupplierInvoiceItemPM);
            if (this.IsDisplayOnly) {
                this.UIProperties.SetEnabled("CatalogNumber", "Customs.SupplierInvioceItemCertificat", false);
            }
            var identification = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.filter(function (d) { return d.TypeCode == "MN"; })[0];
            if (identification != null) {
                this.CatalogNumber = identification.Identification;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DeclarationError)) {
                this.IsFromCustomsAnswers = true;
                this.IsHeaderVisible = true;
                this.invoice = args.SupplierInvoicePM;
                this.InvoiceNumber = args.InvoiceNumber;
                //Select a line
                this.LineNumber = args.LineNumber;
                this.LineNumber = this.LineNumber.split(",")[0];
                var selectedRow = this.ItemsSource.Collection.find(function (d) { return d.SequenceNumeric == _this.LineNumber; });
                this.SelectedRow = selectedRow;
                this.ShowXMLErrors(args.DeclarationError);
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(args.AmendmentView)) {
                this.IsFromCustomsAnswers = true;
                this.IsHeaderVisible = true;
                this.invoice = args.SupplierInvoicePM;
                this.InvoiceNumber = args.InvoiceNumber;
                //Select a line
                //this.LineNumber = args.LineNumber;
                //this.LineNumber = this.LineNumber.split(",")[0];
                //var selectedRow = this.ItemsSource.Collection.find(d => d.SequenceNumeric == this.LineNumber);
                //this.SelectedRow = selectedRow;
                this.ShowXMLCorrections(args.AmendmentView);
            }
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.ShowXMLErrors = function (error) {
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, this.ObjectTableName, false, error.Description);
        }
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors = error.Description.split(/,|:/);
            for (var _i = 0, xmlErrors_1 = xmlErrors; _i < xmlErrors_1.length; _i++) {
                var xmlError = xmlErrors_1[_i];
                errors.push(xmlError);
            }
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
                //if (OnShowXMLErrors != null) {
                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
                //}
            }
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.ShowXMLCorrections = function (error) {
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvioceItemCertificat", false, error.ErrorType);
        }
        var errors = [];
        errors.push(error.ErrorType);
        this.ValidationErrorsList = errors;
    };
    Object.defineProperty(SupplierInvoiceItemCertificatesComponent.prototype, "CatalogNumber", {
        get: function () { return this.invoiceItemPM.CatalogNumber; },
        set: function (value) {
            if (this.invoiceItemPM.CatalogNumber != value) {
                this.invoiceItemPM.CatalogNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemCertificatesComponent.prototype, "ClassificationCode", {
        get: function () { return this.invoiceItemPM.ClassificationCode; },
        set: function (value) {
            if (this.invoiceItemPM.ClassificationCode != value) {
                this.invoiceItemPM.ClassificationCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemCertificatesComponent.prototype, "InvoiceNumber", {
        get: function () { return this.invoiceNumber; },
        set: function (value) {
            if (this.invoiceNumber != value) {
                this.invoiceNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceItemCertificatesComponent.prototype.BuildCertificatesList = function () {
        this.ItemsSource.Clear();
        for (var _i = 0, _a = this.invoiceItemPM.SupplierInvioceItemCertificats; _i < _a.length; _i++) {
            var item = _a[_i];
            this.ItemsSource.Insert(new InvoiceItemCertificateLine(item, this));
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.Add = function () {
        if (!this.IsDisplayOnly) {
            var counter = 0;
            if (this.invoiceItemPM.SupplierInvioceItemCertificats.length > 0) {
                var items = this.invoiceItemPM.SupplierInvioceItemCertificats.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
                if (items.length == 0)
                    counter = 0;
                else {
                    counter = items[this.invoiceItemPM.SupplierInvioceItemCertificats.length - 1].LineNumber;
                }
            }
            counter += 1;
            var sequence = 0;
            if (this.invoiceItemPM.SupplierInvioceItemCertificats.length > 0) {
                var items = this.invoiceItemPM.SupplierInvioceItemCertificats.sort(function (a, b) { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1; });
                if (items.length == 0)
                    sequence = 0;
                else {
                    sequence = items[this.invoiceItemPM.SupplierInvioceItemCertificats.length - 1].SequenceNumeric;
                }
            }
            sequence += 1;
            var item = new SupplierInvioceItemCertificatPM_1.SupplierInvioceItemCertificatPM(this.invoiceItemPM);
            item.DeclarationId = this.invoiceItemPM.DeclarationId,
                item.Tenant = this.invoiceItemPM.Tenant;
            item.InvoiceCounterKey = this.invoiceItemPM.CounterKey,
                item.LineNumber = this.invoiceItemPM.LineNumber,
                item.ItemCertificateCounterKey = counter,
                item.SequenceNumeric = sequence;
            if (!this.invoiceItemPM.SupplierInvioceItemCertificats.includes(item)) {
                this.invoiceItemPM.AddSupplierInvioceItemCertificat(item);
                this.ItemsSource.Insert(new InvoiceItemCertificateLine(item, this));
            }
        }
        //    this.BuildCertificatesList();
    };
    SupplierInvoiceItemCertificatesComponent.prototype.CloneEntity = function (entityToClone) {
        var _this = this;
        var clonedEntity;
        clonedEntity = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(entityToClone.EntityParentPM);
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        clonedEntity.SupplierInvioceItemCertificats = [];
        entityToClone.SupplierInvioceItemCertificats.forEach(function (itemMod) {
            var clonedItemMod = new SupplierInvioceItemCertificatPM_1.SupplierInvioceItemCertificatPM(itemMod.EntityParentPM);
            _this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvioceItemCertificats.push(clonedItemMod);
        });
        return clonedEntity;
    };
    SupplierInvoiceItemCertificatesComponent.prototype.RejectChanges = function () {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    };
    SupplierInvoiceItemCertificatesComponent.prototype.MapEntitytoEntity = function (srcEntity, targetEntity, takeKeysFromTarget) {
        if (takeKeysFromTarget === void 0) { takeKeysFromTarget = false; }
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        if (this.invoiceItemPM.IsDirty && !this.IsDisplayOnly && !this.IsFromCustomsAnswers) {
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
            confirm.WindowClosed.subscribe(function (event) {
                if (confirm.Yes) {
                    confirm.Close();
                    _this.OkButtonClicked();
                }
                else {
                    _this.RejectChanges();
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit('cancel');
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errors = [];
        this.isValid = true;
        this.inValid = false;
        for (var _i = 0, _a = this.invoiceItemPM.SupplierInvioceItemCertificats; _i < _a.length; _i++) {
            var item = _a[_i];
            if (!Tools_1.AppTool.IsNullOrEmpty(item.ApprovalRequestNumber)) {
                this.hasRequest = true;
            }
            if (item.AttachmentTypeCode == null) {
                //var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                //var fieldError: string = translatedRequiredError.replace("%FieldName", "AttachmentTypeCode");
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvioceItemCertificat.F.AttachmentTypeCode")));
                this.isValid = false;
                break;
            }
            else {
                if (item.AttachmentTypeCode == "1" || item.AttachmentTypeCode == "2") {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CertificateNumber) || Tools_1.AppTool.IsNullOrEmpty(item.ReqConfirmationTypeCode) || Tools_1.AppTool.IsNullOrEmpty(item.ResConfirmationTypeCode)) {
                        this.isValid = false;
                        this.inValid = true;
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.CertificateExemptionTypeCode) || !Tools_1.AppTool.IsNullOrEmpty(item.CustomsAttachmentID)) {
                        this.inValid = true;
                        this.notMandatoryIsNotEmpty = true;
                    }
                }
                else {
                    if (item.AttachmentTypeCode == "4") {
                        if (Tools_1.AppTool.IsNullOrEmpty(item.CertificateExemptionTypeCode) || Tools_1.AppTool.IsNullOrEmpty(item.ReqConfirmationTypeCode)) {
                            this.isValid = false;
                            this.inValid = true;
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.CertificateNumber) || !Tools_1.AppTool.IsNullOrEmpty(item.ResConfirmationTypeCode) || !Tools_1.AppTool.IsNullOrEmpty(item.CustomsAttachmentID)) {
                            this.inValid = true;
                            this.notMandatoryIsNotEmpty = true;
                        }
                    }
                }
            }
        }
        if (this.inValid) {
            this.isValid = false;
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirm.ShowNoButton = true;
            if (this.notMandatoryIsNotEmpty) {
                confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CertificateNotMandatoryFields"));
            }
            else {
                confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CertificateMandatoryFields"));
            }
            confirm.WindowClosed.subscribe(function (event) {
                if (confirm.Yes) {
                    if (errors.length == 0) {
                        if (_this.hasRequest) {
                            _this.invoiceItemPM.CertificatesStatusCode = "4";
                        }
                        else {
                            _this.invoiceItemPM.CertificatesStatusCode = "2";
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.CatalogNumber)) {
                            _this.AddIdentification();
                        }
                        if (_this.IsFromCustomsAnswers) {
                            //SaveValidCertificateEvent saveValidCertificateEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<SaveValidCertificateEvent>();
                            //saveValidCertificateEvent.Publish(new SaveValidCertificateEventArgs() { Valid = true });
                            _this.supplierInvoicePMService.update(_this.invoice).subscribe(function (response) {
                                var result = response.Result;
                                console.log("[response/supplierInvoicePMService.update]", result);
                                _this.CurrentSession.CloseCurrentWindow();
                                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                                }
                                else {
                                }
                            });
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                        //this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        _this.ValidationErrorsList = errors;
                    }
                    confirm.Close();
                }
            });
        }
        else {
            if (errors.length == 0) {
                if (this.invoiceItemPM.SupplierInvioceItemCertificats.length > 0) {
                    if (this.hasRequest) {
                        this.invoiceItemPM.CertificatesStatusCode = "3";
                    }
                    else {
                        this.invoiceItemPM.CertificatesStatusCode = "1";
                    }
                }
                else {
                    this.invoiceItemPM.CertificatesStatusCode = null;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CatalogNumber)) {
                    this.AddIdentification();
                }
                if (this.IsFromCustomsAnswers) {
                    this.supplierInvoicePMService.update(this.invoice).subscribe(function (response) {
                        var result = response.Result;
                        console.log("[response/supplierInvoicePMService.update]", result);
                        _this.CurrentSession.CloseCurrentWindow();
                        if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                        }
                        else {
                        }
                    });
                }
                else {
                    this.CurrentSession.CloseCurrentWindow();
                }
            }
            else {
                this.ValidationErrorsList = errors;
            }
        }
        return this.isValid;
    };
    SupplierInvoiceItemCertificatesComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    SupplierInvoiceItemCertificatesComponent.prototype.OnRowEnded = function ($event) {
        console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            this.Add();
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.OnFocus = function () {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    };
    SupplierInvoiceItemCertificatesComponent.prototype.AddIdentification = function () {
        var line = 0;
        var count = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.length;
        if (this.invoiceItemPM.SupplierInvoiceItemsProdIdents.length > 0) {
            var identifications = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.sort(function (d) { return d.LineNumber; });
            line = identifications[count - 1].LineNumber;
        }
        line += 1;
        var item = new SupplierInvoiceItemsProdIdentPM_1.SupplierInvoiceItemsProdIdentPM(this.invoiceItemPM);
        item.DeclarationId = this.invoiceItemPM.DeclarationId;
        item.Tenant = this.invoiceItemPM.Tenant;
        item.InvoiceCounterKey = this.invoiceItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.invoiceItemPM.LineNumber;
        item.LineNumber = line,
            item.Identification = this.CatalogNumber;
        item.TypeCode = "MN";
        this.productIdentificationTypeListService.getSingle(item.TypeCode).subscribe(function (response) {
            var result;
            result = response.Result;
            item.TypeName = result.LocalName;
        });
        var productIdent = this.invoiceItemPM.SupplierInvoiceItemsProdIdents.find(function (m) { return m.TypeCode == "MN"; });
        if (productIdent != null) {
            if (productIdent.Identification == null) {
                productIdent.Identification = this.CatalogNumber;
            }
        }
        else {
            this.invoiceItemPM.AddSupplierInvoiceItemsProdIdent(item);
        }
    };
    SupplierInvoiceItemCertificatesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SupplierInvoiceItemCertificatesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SupplierInvoiceItemCertificatesComponent);
    return SupplierInvoiceItemCertificatesComponent;
}(BaseComponent_1.BaseComponent));
exports.SupplierInvoiceItemCertificatesComponent = SupplierInvoiceItemCertificatesComponent;
var InvoiceItemCertificateLine = /** @class */ (function (_super) {
    __extends(InvoiceItemCertificateLine, _super);
    function InvoiceItemCertificateLine(EntityPM, Parent) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.SupplierInvioceItemCertificat";
        _this.entityPM = EntityPM;
        _this.parent = Parent;
        return _this;
    }
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "SequenceNumeric", {
        //#region properties
        get: function () { return this.entityPM.SequenceNumeric; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ConfirmationType", {
        get: function () { return this.confirmationType; },
        set: function (value) {
            if (this.confirmationType != value) {
                this.confirmationType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.ConfirmationTypeName = value.LocalName;
            }
            else {
                this.ConfirmationTypeCode = null;
                this.ConfirmationTypeName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "AttachmentType", {
        get: function () { return this.attachmentType; },
        set: function (value) {
            if (this.attachmentType != value) {
                this.attachmentType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.AttachmentTypeName = value.LocalName;
            }
            else {
                this.AttachmentTypeCode = null;
                this.AttachmentTypeName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "CertificateExemptionType", {
        get: function () { return this.certificateExemptionType; },
        set: function (value) {
            if (this.certificateExemptionType != value) {
                this.certificateExemptionType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.CertificateExemptionTypeName = value.LocalName;
            }
            else {
                this.CertificateExemptionTypeCode = null;
                this.CertificateExemptionTypeName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ResConfirmationType", {
        get: function () { return this.resConfirmationType; },
        set: function (value) {
            if (this.resConfirmationType != value) {
                this.resConfirmationType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.ResConfirmationTypeName = value.LocalName;
            }
            else {
                this.ResConfirmationTypeCode = null;
                this.ResConfirmationTypeName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ConfirmationTypeCode", {
        get: function () { return this.entityPM.ReqConfirmationTypeCode; },
        set: function (value) {
            if (this.entityPM.ReqConfirmationTypeCode != value) {
                this.entityPM.ReqConfirmationTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ConfirmationTypeName", {
        get: function () { return this.entityPM.ReqConfirmationTypeName; },
        set: function (value) {
            if (this.entityPM.ReqConfirmationTypeName != value) {
                this.entityPM.ReqConfirmationTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ResConfirmationTypeCode", {
        get: function () { return this.entityPM.ResConfirmationTypeCode; },
        set: function (value) {
            if (this.entityPM.ResConfirmationTypeCode != value) {
                this.entityPM.ResConfirmationTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ResConfirmationTypeName", {
        get: function () { return this.entityPM.ResConfirmationTypeName; },
        set: function (value) {
            if (this.entityPM.ResConfirmationTypeName != value) {
                this.entityPM.ResConfirmationTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "CertificateNumber", {
        get: function () { return this.entityPM.CertificateNumber; },
        set: function (value) {
            if (this.entityPM.CertificateNumber != value) {
                this.entityPM.CertificateNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "CertificateExemptionTypeCode", {
        get: function () { return this.entityPM.CertificateExemptionTypeCode; },
        set: function (value) {
            if (this.entityPM.CertificateExemptionTypeCode != value) {
                this.entityPM.CertificateExemptionTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "CertificateExemptionTypeName", {
        get: function () { return this.entityPM.CertificateExemptionTypeName; },
        set: function (value) {
            if (this.entityPM.CertificateExemptionTypeName != value) {
                this.entityPM.CertificateExemptionTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "AttachmentTypeCode", {
        get: function () { return this.entityPM.AttachmentTypeCode; },
        set: function (value) {
            if (this.entityPM.AttachmentTypeCode != value) {
                this.entityPM.AttachmentTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "AttachmentTypeName", {
        get: function () { return this.entityPM.AttachmentTypeName; },
        set: function (value) {
            if (this.entityPM.AttachmentTypeName != value) {
                this.entityPM.AttachmentTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "CustomsAttachmentID", {
        get: function () { return this.entityPM.CustomsAttachmentID; },
        set: function (value) {
            if (this.entityPM.CustomsAttachmentID != value) {
                this.entityPM.CustomsAttachmentID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ApprovalRequestNumber", {
        get: function () { return this.entityPM.ApprovalRequestNumber; },
        set: function (value) {
            if (this.entityPM.ApprovalRequestNumber != value) {
                this.entityPM.ApprovalRequestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemCertificateLine.prototype, "ExternalRequestTypeCode", {
        get: function () { return this.entityPM.ExternalRequestTypeCode; },
        set: function (value) {
            if (this.entityPM.ExternalRequestTypeCode != value) {
                this.entityPM.ExternalRequestTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    InvoiceItemCertificateLine.prototype.DeleteButtonClicked = function () {
        this.parent.ItemsSource.Remove(this);
        if (this.parent.invoiceItemPM.SupplierInvioceItemCertificats.includes(this.entityPM)) {
            this.parent.invoiceItemPM.RemoveSupplierInvioceItemCertificat(this.entityPM);
        }
        //    this.parent.BuildCertificatesList();
    };
    return InvoiceItemCertificateLine;
}(BaseComponent_1.BaseComponent));
exports.InvoiceItemCertificateLine = InvoiceItemCertificateLine;
//# sourceMappingURL=SupplierInvoiceItemCertificatesComponent.js.map