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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DecCargoSplitConsItemPM_1 = require("../../../../Customs/EntityPMs/DecCargoSplitConsItemPM");
var DecCargoSplitConsPackDetPM_1 = require("../../../../Customs/EntityPMs/DecCargoSplitConsPackDetPM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var PackingTypeListService_1 = require("../../../../Customs/Services/StandardLists/PackingTypeListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var DeclarationCargoSplitPMService_1 = require("../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService");
var DecCargoSplitConsPackDetComponent = /** @class */ (function (_super) {
    __extends(DecCargoSplitConsPackDetComponent, _super);
    function DecCargoSplitConsPackDetComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.DecCargoSplitConsPackDet";
        _this.DataContext = _this;
        _this.declarationCargoSplitPMService = new DeclarationCargoSplitPMService_1.DeclarationCargoSplitPMService();
        //public DecCargoSplitConPMService: DecCargoSplitConPMService = new DecCargoSplitConPMService();
        _this.ValidationErrorsList = [];
        _this.IsHeaderVisible = false;
        _this.IsChanged = false;
        //IsFromCustomsAnswers: boolean = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.notMandatoryIsNotEmpty = false;
        _this.SelectedRow = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        return _this;
    }
    DecCargoSplitConsPackDetComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var _entityResourceService = new EntityResourceService_1.EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("Customs.PackingType", 0).subscribe(function (res) {
            var entityListService = new PackingTypeListService_1.PackingTypeListService();
            entityListService.getAllFromCache().subscribe(function (res) {
            });
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.decCargoSplitConsItemPM = args.DecCargoSplitConsItemPM;
            this.declarationCargoSplitPM = args.DeclarationCargoSplitPM;
            this.BuildCertificatesList();
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.OriginalItemPM = args.DecCargoSplitConsItemPM;
            this.ClonedItemPM = this.CloneEntity(args.DecCargoSplitConsItemPM);
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DeclarationError)) {
                //this.IsFromCustomsAnswers = true;
                this.IsHeaderVisible = true;
                this.decCargoSplitCon = args.DecCargoSplitConPM;
                //this.PackageTypeCode = args.PackageTypeCode;
                //Select a line
                this.LineNumber = args.LineNumber;
                this.LineNumber = this.LineNumber.split(",")[0];
                var selectedRow = this.ItemsSource.Collection.find(function (d) { return d.DecCargoSplitConsItemLine == _this.LineNumber; });
                this.SelectedRow = selectedRow;
                this.ShowXMLErrors(args.DeclarationError);
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(args.AmendmentView)) {
                this.IsHeaderVisible = true;
                this.decCargoSplitCon = args.DecCargoSplitConPM;
                //this.PackageTypeCode = args.PackageTypeCode;
                this.ShowXMLCorrections(args.AmendmentView);
            }
        }
    };
    DecCargoSplitConsPackDetComponent.prototype.ShowXMLErrors = function (error) {
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
            if (error.EntityName.toLowerCase() == "DecCargoSplitConsItem") {
            }
        }
    };
    DecCargoSplitConsPackDetComponent.prototype.ShowXMLCorrections = function (error) {
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Field)) {
            this.UIProperties.SetValidity(error.Field, "Customs.DecCargoSplitConsPackDet", false, error.ErrorType);
        }
        var errors = [];
        errors.push(error.ErrorType);
        this.ValidationErrorsList = errors;
    };
    DecCargoSplitConsPackDetComponent.prototype.BuildCertificatesList = function () {
        this.ItemsSource.Clear();
        for (var _i = 0, _a = this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets; _i < _a.length; _i++) {
            var item = _a[_i];
            this.ItemsSource.Insert(new DecCargoSplitConsPackDetLine(item, this));
        }
    };
    DecCargoSplitConsPackDetComponent.prototype.Add = function () {
        if (!this.IsDisplayOnly) {
            var counter = 0;
            if (this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.length > 0) {
                var items = this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.sort(function (a, b) { return (a.PackageLine === b.PackageLine) ? 0 : (a.PackageLine < b.PackageLine) ? -1 : 1; });
                if (items.length == 0)
                    counter = 0;
                else {
                    counter = items[this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.length - 1].PackageLine;
                }
            }
            counter += 1;
            var item = new DecCargoSplitConsPackDetPM_1.DecCargoSplitConsPackDetPM(this.decCargoSplitConsItemPM);
            item.DeclarationCargoSplitId = this.decCargoSplitConsItemPM.DeclarationCargoSplitId;
            item.Tenant = this.decCargoSplitConsItemPM.Tenant;
            item.DecCargoSplitConsLineNo = this.decCargoSplitConsItemPM.DecCargoSplitConsLineNo;
            item.DecCargoSplitConsItemLine = this.decCargoSplitConsItemPM.ItemLine;
            item.PackageLine = counter;
            if (!this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.includes(item)) {
                this.decCargoSplitConsItemPM.AddDecCargoSplitConsPackDet(item);
                this.ItemsSource.Insert(new DecCargoSplitConsPackDetLine(item, this));
            }
        }
    };
    DecCargoSplitConsPackDetComponent.prototype.CloneEntity = function (entityToClone) {
        var _this = this;
        var clonedEntity;
        clonedEntity = new DecCargoSplitConsItemPM_1.DecCargoSplitConsItemPM(entityToClone.EntityParentPM);
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        clonedEntity.DecCargoSplitConsPackDets = [];
        entityToClone.DecCargoSplitConsPackDets.forEach(function (itemMod) {
            var clonedItemMod = new DecCargoSplitConsPackDetPM_1.DecCargoSplitConsPackDetPM(itemMod.EntityParentPM);
            _this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.DecCargoSplitConsPackDets.push(clonedItemMod);
        });
        return clonedEntity;
    };
    DecCargoSplitConsPackDetComponent.prototype.RejectChanges = function () {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    };
    DecCargoSplitConsPackDetComponent.prototype.MapEntitytoEntity = function (srcEntity, targetEntity, takeKeysFromTarget) {
        if (takeKeysFromTarget === void 0) { takeKeysFromTarget = false; }
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    };
    DecCargoSplitConsPackDetComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        if (!this.IsDisplayOnly && this.IsChanged) {
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CancelMessage"));
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
    DecCargoSplitConsPackDetComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errors = [];
        this.isValid = true;
        this.inValid = false;
        for (var _i = 0, _a = this.decCargoSplitConsItemPM.DecCargoSplitConsPackDets; _i < _a.length; _i++) {
            var item = _a[_i];
            if (Tools_1.AppTool.IsNullOrEmpty(item.PackageTypeCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitConsPackDet.F.PackageTypeCode")));
                this.inValid = true;
                this.isValid = false;
                break;
            }
            else {
                if (item.PackageQuantity == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DecCargoSplitConsPackDet.F.PackageQuantity")));
                    this.inValid = true;
                    this.isValid = false;
                    break;
                }
                else {
                    if (item.PackageQuantity.toString().length > 8) {
                        errors.push("כמות לא יכולה להיות ארוכה משמונה תווים");
                        this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", false, "כמות לא יכולה להיות ארוכה משמונה תווים");
                        this.inValid = false;
                        this.isValid = false;
                        break;
                    }
                    else {
                        this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
                        if (item.GrossMassMeasure != null) {
                            var strValue = item.GrossMassMeasure.toString();
                            if (strValue.indexOf(".") > -1)
                                strValue = item.GrossMassMeasure.toString().substring(0, item.GrossMassMeasure.toString().indexOf("."));
                            if (Tools_1.AppTool.IsNullOrEmpty(strValue)) {
                                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
                            }
                            else if (strValue.length > 11) {
                                errors.push("משקל לא יכול להיות ארוך מאחד עשר תווים");
                                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", false, "משקל לא יכול להיות ארוך מאחד עשר תווים");
                                this.inValid = false;
                                this.isValid = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (errors.length != 0) {
            this.ValidationErrorsList = errors;
        }
        if (this.inValid) {
            this.isValid = false;
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            //confirm.ShowNoButton = true;
            confirm.WindowClosed.subscribe(function (event) {
                if (confirm.Yes) {
                    if (errors.length == 0) {
                        var isSave = 1;
                        if (isSave == 1) {
                            _this.OriginalItemPM.ChangeSetOp = "Update";
                            _this.CurrentSession.CloseCurrentWindow();
                            /*
                            this.declarationCargoSplitPMService.update(this.declarationCargoSplitPM).subscribe((response: any) => {
                                var result = response.Result;
                                this.IsChanged = false;
                                console.log("[response/declarationCargoSplitPMService.update]", result);
                                this.CurrentSession.CloseCurrentWindow();
                                if (!AppTool.IsNullOrEmpty(result)) {

                                } else {
                                }
                            });
                            */
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindow();
                        }
                    }
                }
                else {
                    _this.ValidationErrorsList = errors;
                }
                confirm.Close();
            });
            // confirm.Show(errors.toString());
        }
        else {
            if (errors.length == 0) {
                var isSave = 1;
                if (isSave == 1) {
                    this.OriginalItemPM.ChangeSetOp = "Update";
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                    //this.CurrentSession.CloseCurrentWindow();
                    /*
                    this.declarationCargoSplitPMService.update(this.declarationCargoSplitPM).subscribe((response: any) => {
                        var result = response.Result;
                        this.IsChanged = false;
                        console.log("[response/declarationCargoSplitPMService.update]", result);
                        this.CurrentSession.CloseCurrentWindow();
                        if (!AppTool.IsNullOrEmpty(result)) {

                        } else {
                        }
                    });
                    */
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
    DecCargoSplitConsPackDetComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
        this.IsChanged = true;
    };
    DecCargoSplitConsPackDetComponent.prototype.OnRowEnded = function ($event) {
        console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            this.Add();
        }
    };
    DecCargoSplitConsPackDetComponent.prototype.OnFocus = function () {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    };
    DecCargoSplitConsPackDetComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DecCargoSplitConsPackDetComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DecCargoSplitConsPackDetComponent);
    return DecCargoSplitConsPackDetComponent;
}(BaseComponent_1.BaseComponent));
exports.DecCargoSplitConsPackDetComponent = DecCargoSplitConsPackDetComponent;
var DecCargoSplitConsPackDetLine = /** @class */ (function (_super) {
    __extends(DecCargoSplitConsPackDetLine, _super);
    function DecCargoSplitConsPackDetLine(EntityPM, Parent) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DecCargoSplitConsPackDet";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.valid = true;
        _this.entityPM = EntityPM;
        _this.parent = Parent;
        return _this;
    }
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "PackageTypeCode", {
        //#region properties
        get: function () { return this.entityPM.PackageTypeCode; },
        set: function (value) {
            if (this.entityPM.PackageTypeCode != value) {
                this.entityPM.PackageTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "PackageTypeName", {
        get: function () { return this.entityPM.PackageTypeName; },
        set: function (value) {
            if (this.entityPM.PackageTypeName != value) {
                this.entityPM.PackageTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "PackingType", {
        get: function () { return this.PackingType; },
        set: function (value) {
            if (this.PackingType != value) {
                this.PackingType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.PackageTypeName = value.LocalName;
            }
            else {
                this.PackageTypeCode = null;
                this.PackageTypeName = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "PackageQuantity", {
        get: function () { return this.entityPM.PackageQuantity; },
        set: function (value) {
            if (this.entityPM.PackageQuantity != value) {
                this.entityPM.PackageQuantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "GrossMassMeasure", {
        get: function () { return this.entityPM.GrossMassMeasure; },
        set: function (value) {
            if (this.entityPM.GrossMassMeasure != value) {
                this.entityPM.GrossMassMeasure = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "MarksNumbers", {
        get: function () { return this.entityPM.MarksNumbers; },
        set: function (value) {
            if (this.entityPM.MarksNumbers != value) {
                this.entityPM.MarksNumbers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsPackDetLine.prototype, "ManifestNumber", {
        get: function () { return this.entityPM.ManifestNumber; },
        set: function (value) {
            if (this.entityPM.ManifestNumber != value) {
                this.entityPM.ManifestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    DecCargoSplitConsPackDetLine.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    DecCargoSplitConsPackDetLine.prototype.DeleteButtonClicked = function () {
        this.parent.ItemsSource.Remove(this);
        if (this.parent.decCargoSplitConsItemPM.DecCargoSplitConsPackDets.includes(this.entityPM)) {
            this.parent.decCargoSplitConsItemPM.RemoveDecCargoSplitConsPackDet(this.entityPM);
        }
        //    this.parent.BuildCertificatesList();
    };
    DecCargoSplitConsPackDetLine.prototype.PackageQuantityKeyUp = function (event, logCellTemplate, packageQuantityTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnPackageQuantityLostFocus(logCellTemplate, packageQuantityTextBox);
        }
    };
    DecCargoSplitConsPackDetLine.prototype.OnPackageQuantityLostFocus = function (logCellTemplate, packageQuantityTextBox) {
        var newValue = this.PackageQuantity;
        this.valid = true;
        this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
        }
        else if (newValue.toString().length > 8) {
            this.valid = false;
            this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", false, "כמות לא יכולה להיות ארוכה משמונה תווים");
        }
        else {
            this.valid = true;
            this.UIProperties.SetValidity("PackageQuantity", "Customs.DecCargoSplitConsPackDet", true, "");
        }
        if (this.valid != true) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: packageQuantityTextBox.InputId });
        }
        //this.PackageQuantity = newValue;
        //packageQuantityTextBox.TextValue = newValue;
    };
    DecCargoSplitConsPackDetLine.prototype.GrossMassMeasureKeyUp = function (event, logCellTemplate, grossMassMeasureTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnGrossMassMeasureLostFocus(logCellTemplate, grossMassMeasureTextBox);
        }
    };
    DecCargoSplitConsPackDetLine.prototype.OnGrossMassMeasureLostFocus = function (logCellTemplate, grossMassMeasureTextBox) {
        var newValue = this.GrossMassMeasure;
        this.valid = true;
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
        }
        else {
            this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1)
                strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (Tools_1.AppTool.IsNullOrEmpty(strValue)) {
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
            }
            else if (strValue.length > 11) {
                this.valid = false;
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", false, "משקל לא יכול להיות ארוך מאחד עשר תווים");
            }
            else {
                this.valid = true;
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
            }
        }
        if (this.valid != true) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: grossMassMeasureTextBox.InputId });
        }
    };
    return DecCargoSplitConsPackDetLine;
}(BaseComponent_1.BaseComponent));
exports.DecCargoSplitConsPackDetLine = DecCargoSplitConsPackDetLine;
//# sourceMappingURL=DecCargoSplitConsPackDetComponent.js.map