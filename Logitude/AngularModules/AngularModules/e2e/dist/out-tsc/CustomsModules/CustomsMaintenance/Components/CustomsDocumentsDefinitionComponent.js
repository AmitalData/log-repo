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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CustomsDocumentsDefinitionPM_1 = require("../../../Customs/EntityPMs/CustomsDocumentsDefinitionPM");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomsDocumentsDefinitionListService_1 = require("../../../Customs/Services/StandardLists/CustomsDocumentsDefinitionListService");
var CustomsDocumentsDefinitionPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentsDefinitionPMService");
var CustomsDocumentsDefinitionExtendedService_1 = require("../../../Customs/Services/ExtendedPMs/CustomsDocumentsDefinitionExtendedService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsDocumentsDefinitionComponent = /** @class */ (function (_super) {
    __extends(CustomsDocumentsDefinitionComponent, _super);
    function CustomsDocumentsDefinitionComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.EntityPM = new CustomsDocumentsDefinitionPM_1.CustomsDocumentsDefinitionPM();
        _this.ObjectTableName = "Customs.CustomsDocumentsDefinition";
        _this.isControlEnabled = true;
        _this.IsLoaded = false;
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._EntityListService = new CustomsDocumentsDefinitionListService_1.CustomsDocumentsDefinitionListService();
        _this._EntityPMService = new CustomsDocumentsDefinitionPMService_1.CustomsDocumentsDefinitionPMService();
        _this._EntityPMExtendedService = new CustomsDocumentsDefinitionExtendedService_1.CustomsDocumentsDefinitionExtendedService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllDocumentsDefinitionResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.DocumentsDefinitionResultList = new ObservableCollection_1.ObservableCollection([]);
        _this.DeleteDocumentsDefinitionList = new ObservableCollection_1.ObservableCollection([]);
        _this.DocumentTypeFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.DocumentTypeFilterItems.addAdditionalFilter("Code", "380", null, null, "Exclude", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("LocalName", "380", null, null, "NotContains", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("SearchFields", "380", null, null, "NotContains", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("Code", "271", null, null, "Exclude", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("LocalName", "271", null, null, "NotContains", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("SearchFields", "271", null, null, "NotContains", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("Code", "864", null, null, "Exclude", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("LocalName", "864", null, null, "NotContains", false, false, false, "string", false, true);
        _this.DocumentTypeFilterItems.addAdditionalFilter("SearchFields", "864", null, null, "NotContains", false, false, false, "string", false, true);
        _this.CurrentSession.StartBusyIndicator("");
        _this._EntityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            _this.BuildDocumentsDefinitionList();
            _this.IsLoaded = true;
        });
        return _this;
    }
    CustomsDocumentsDefinitionComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    CustomsDocumentsDefinitionComponent.prototype.ngOnInit = function () {
    };
    CustomsDocumentsDefinitionComponent.prototype.BuildDocumentsDefinitionList = function () {
        var _this = this;
        this._EntityListService.getAll().subscribe(function (myResult) {
            console.log("Get All Customs Documents Definition: ", myResult);
            if (myResult != null && myResult.Result != null) {
                _this.AllDocumentsDefinitionResultList = new ObservableCollection_1.ObservableCollection([]);
                _this.DocumentsDefinitionResultList = new ObservableCollection_1.ObservableCollection([]);
                for (var _i = 0, _a = myResult.Result; _i < _a.length; _i++) {
                    var item = _a[_i];
                    _this.AllDocumentsDefinitionResultList.Insert(new DocumentsDefinitionPMComponent(item.DocumentTypeCode, item.ProcessTypeCode, item.TransportationTypeCode, item.CargoTypeCode));
                    _this.DocumentsDefinitionResultList.Insert(new DocumentsDefinitionComponent(item, false));
                }
                return;
            }
        });
    };
    Object.defineProperty(CustomsDocumentsDefinitionComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentsDefinitionComponent.prototype, "DocumentTypeCode", {
        get: function () { return this.EntityPM.DocumentTypeCode; },
        set: function (newValue) { this.EntityPM.DocumentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentsDefinitionComponent.prototype, "ProcessTypeCode", {
        get: function () { return this.EntityPM.ProcessTypeCode; },
        set: function (newValue) { this.EntityPM.ProcessTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentsDefinitionComponent.prototype, "TransportationTypeCode", {
        get: function () { return this.EntityPM.TransportationTypeCode; },
        set: function (newValue) { this.EntityPM.TransportationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsDocumentsDefinitionComponent.prototype, "CargoTypeCode", {
        get: function () { return this.EntityPM.CargoTypeCode; },
        set: function (newValue) { this.EntityPM.CargoTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    CustomsDocumentsDefinitionComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    CustomsDocumentsDefinitionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsDocumentsDefinitionComponent.prototype.CheckBeforeSave = function () {
        var isValide = true;
        //First Check Befor Save For Duplicates
        if (this.DocumentsDefinitionResultList != null && this.DocumentsDefinitionResultList.Collection.length > 0) {
            var _loop_1 = function (item) {
                if (item.DocumentTypeCode == "380" || item.DocumentTypeCode == "271" || item.DocumentTypeCode == "864") {
                    messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                    messageWindow.Width = 250;
                    messageWindow.Height = 150;
                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                    messageWindow.Show("לא ניתן להגדיר סוג מסמך מסוג 380/271/864");
                    isValide = false;
                    return "break";
                }
                if (this_1.AllDocumentsDefinitionResultList != null && this_1.AllDocumentsDefinitionResultList.Length > 0) {
                    nullVM = this_1.DocumentsDefinitionResultList.Collection.filter(function (vm) { return vm.DocumentTypeCode == item.DocumentTypeCode &&
                        vm.TransportationTypeCode == item.TransportationTypeCode &&
                        vm.CargoTypeCode == item.CargoTypeCode &&
                        vm.ProcessTypeCode == item.ProcessTypeCode; });
                    if (nullVM.length > 1) {
                        messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                        messageWindow.Width = 250;
                        messageWindow.Height = 150;
                        messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show("לא ניתן להזין רשומות כפולות ");
                        isValide = false;
                        return "break";
                    }
                    nullAllVM = this_1.AllDocumentsDefinitionResultList.Collection.filter(function (vm) { return vm.DocumentTypeCode == item.DocumentTypeCode &&
                        vm.TransportationTypeCode == item.TransportationTypeCode &&
                        vm.CargoTypeCode == item.CargoTypeCode &&
                        vm.ProcessTypeCode == item.ProcessTypeCode; });
                    if (nullAllVM.length > 1 || (item.IsNew == true && nullAllVM.length >= 1) || nullVM.length > 1) {
                        messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                        messageWindow.Width = 250;
                        messageWindow.Height = 150;
                        messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show("לא ניתן להזין רשומות כפולות ");
                        isValide = false;
                        return "break";
                    }
                }
            };
            var this_1 = this, messageWindow, nullVM, messageWindow, nullAllVM, messageWindow;
            for (var _i = 0, _a = this.DocumentsDefinitionResultList.Collection; _i < _a.length; _i++) {
                var item = _a[_i];
                var state_1 = _loop_1(item);
                if (state_1 === "break")
                    break;
            }
        }
        return isValide;
    };
    CustomsDocumentsDefinitionComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.CheckBeforeSave() != true) {
            return;
        }
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        if (this.DocumentsDefinitionResultList != null && this.DocumentsDefinitionResultList.Collection.length > 0) {
            this.DocumentsDefinitionResultList.Collection.forEach(function (item) {
                if (item.IsNew == true) {
                    item.entityPM.Tenant = 1; // ????
                    _this._EntityPMService.insert(item.entityPM).subscribe(function (response) {
                        var res = response;
                        if (res.HasError) {
                            //this.ValidationErrorsList = res.ErrorsArray;
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            return;
                        }
                    });
                }
                else {
                    _this._EntityPMService.update(item.entityPM).subscribe(function (response) {
                        var res = response;
                        if (res.HasError) {
                            //this.ValidationErrorsList = res.ErrorsArray;
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            return;
                        }
                    });
                }
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
            console.log("..Saved Successfully ");
        }
        if (this.DeleteDocumentsDefinitionList != null && this.DeleteDocumentsDefinitionList.Collection.length > 0) {
            this.DeleteDocumentsDefinitionList.Collection.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.entityPM.Id)) {
                    _this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
                    _this._EntityPMExtendedService.delete(item.entityPM.Id).subscribe(function (response) {
                        var res = response;
                        if (res.HasError) {
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            return;
                        }
                    });
                }
            });
            console.log("..Deleted Successfully ");
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsDocumentsDefinitionComponent.prototype.SortResultList = function () {
        if (this.DocumentsDefinitionResultList == null ||
            this.DocumentsDefinitionResultList != null && this.DocumentsDefinitionResultList.Length == 0) {
            return;
        }
        this.DocumentsDefinitionResultList.Collection.sort(function (a, b) {
            return (a.CargoTypeCode === b.CargoTypeCode) ? 0 : (a.CargoTypeCode > b.CargoTypeCode) ? -1 : 1;
        });
        this.DocumentsDefinitionResultList.Collection.sort(function (a, b) {
            return (a.ProcessTypeCode === b.ProcessTypeCode) ? 0 : (a.ProcessTypeCode > b.ProcessTypeCode) ? -1 : 1;
        });
        this.DocumentsDefinitionResultList.Collection.sort(function (a, b) {
            return (a.TransportationTypeCode === b.TransportationTypeCode) ? 0 : (a.TransportationTypeCode > b.TransportationTypeCode) ? -1 : 1;
        });
        this.DocumentsDefinitionResultList.Collection.sort(function (a, b) {
            return (a.DocumentTypeCode === b.DocumentTypeCode) ? 0 : (a.DocumentTypeCode > b.DocumentTypeCode) ? -1 : 1;
        });
    };
    CustomsDocumentsDefinitionComponent.prototype.AddDocumentsDefinitionCommand = function () {
        this.DocumentsDefinitionResultList.Insert(new DocumentsDefinitionComponent(new CustomsDocumentsDefinitionPM_1.CustomsDocumentsDefinitionPM(), true));
    };
    CustomsDocumentsDefinitionComponent.prototype.DeleteDocumentsDefinitionCommand = function (item) {
        this.DocumentsDefinitionResultList.Remove(item);
        if (item.IsNew == false) {
            this.DeleteDocumentsDefinitionList.Insert(item);
        }
    };
    CustomsDocumentsDefinitionComponent.prototype.SearchDocumentsDefinitionCommand = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("DocumentTypeCode", this.DocumentTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("ProcessTypeCode", this.ProcessTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("TransportationTypeCode", this.TransportationTypeCode, null, null, "Equals", false, false, false, "Text");
        filters.addAdditionalFilter("CargoTypeCode", this.CargoTypeCode, null, null, "Equals", false, false, false, "Text");
        this._EntityListService.getByFilters(filters).subscribe(function (myResult) {
            console.log("Customs Documents Definition: ", myResult);
            if (myResult == null ||
                (myResult != null && myResult.Result == null) ||
                (myResult != null && myResult.Result != null && myResult.Result.length == 0)) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("לא נמצאו נתונים");
                _this.DocumentsDefinitionResultList.Clear();
                ;
                return;
            }
            _this.DocumentsDefinitionResultList = new ObservableCollection_1.ObservableCollection([]);
            for (var _i = 0, _a = myResult.Result; _i < _a.length; _i++) {
                var item = _a[_i];
                _this.DocumentsDefinitionResultList.Insert(new DocumentsDefinitionComponent(item, false));
            }
            _this.SortResultList();
        });
    };
    CustomsDocumentsDefinitionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsDocumentsDefinitionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsDocumentsDefinitionComponent);
    return CustomsDocumentsDefinitionComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsDocumentsDefinitionComponent = CustomsDocumentsDefinitionComponent;
var DocumentsDefinitionComponent = /** @class */ (function (_super) {
    __extends(DocumentsDefinitionComponent, _super);
    function DocumentsDefinitionComponent(entityPM, isNew) {
        var _this = _super.call(this) || this;
        _this.entityPM = entityPM;
        _this.ObjectTableName = "Customs.CustomsDocumentsDefinition";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsNew = isNew;
        if (_this.IsNew) {
            _this.Mandatory = false;
            _this.Inactive = false;
        }
        return _this;
    }
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "DocumentTypeCode", {
        get: function () { return this.entityPM.DocumentTypeCode; },
        set: function (newValue) { this.entityPM.DocumentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "DocumentTypeName", {
        get: function () { return this.entityPM.DocumentTypeName; },
        set: function (newValue) { this.entityPM.DocumentTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "ProcessTypeCode", {
        get: function () { return this.entityPM.ProcessTypeCode; },
        set: function (newValue) { this.entityPM.ProcessTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "ProcessTypeName", {
        get: function () { return this.entityPM.ProcessTypeName; },
        set: function (newValue) { this.entityPM.ProcessTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "TransportationTypeCode", {
        get: function () { return this.entityPM.TransportationTypeCode; },
        set: function (newValue) { this.entityPM.TransportationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "TransportationTypeName", {
        get: function () { return this.entityPM.TransportationTypeName; },
        set: function (newValue) { this.entityPM.TransportationTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "CargoTypeCode", {
        get: function () { return this.entityPM.CargoTypeCode; },
        set: function (newValue) { this.entityPM.CargoTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "CargoTypeName", {
        get: function () { return this.entityPM.CargoTypeName; },
        set: function (newValue) { this.entityPM.CargoTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "Mandatory", {
        get: function () { return this.entityPM.Mandatory; },
        set: function (newValue) { this.entityPM.Mandatory = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "Inactive", {
        get: function () { return this.entityPM.Inactive; },
        set: function (newValue) { this.entityPM.Inactive = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionComponent.prototype, "IsNew", {
        get: function () { return this._IsNew; },
        set: function (newValue) { this._IsNew = newValue; },
        enumerable: true,
        configurable: true
    });
    //private _IsDelete: boolean;
    //public get IsDelete() { return this._IsDelete; }
    //public set IsDelete(newValue: boolean) { this._IsDelete = newValue; }
    DocumentsDefinitionComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    DocumentsDefinitionComponent.prototype.DocumentTypeLostFocus = function (logCellTemplate, documentTypeCodeLovBox) {
        var _this = this;
        var newValue = documentTypeCodeLovBox.SelectedItemObject == null ? documentTypeCodeLovBox.SearchTextNgModel : documentTypeCodeLovBox.SelectedItemObject.Code;
        if (newValue == "380" || newValue == "271" || newValue == "864") {
            this.DocumentTypeCode = newValue;
            documentTypeCodeLovBox.selectedValue = newValue;
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.WindowClosed.subscribe(function (event) {
                SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
                _this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, OnBlurEvent: documentTypeCodeLovBox.OnBlurEvent });
            });
            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן להגדיר סוג מסמך מסוג 380/271/864");
            return;
        }
    };
    return DocumentsDefinitionComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentsDefinitionComponent = DocumentsDefinitionComponent;
var DocumentsDefinitionPMComponent = /** @class */ (function (_super) {
    __extends(DocumentsDefinitionPMComponent, _super);
    function DocumentsDefinitionPMComponent(documentTypeCode, processTypeCode, transportationTypeCode, cargoTypeCode) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.CustomsDocumentsDefinition";
        _this.DataContext = _this;
        _this.DocumentTypeCode = documentTypeCode;
        _this.ProcessTypeCode = processTypeCode;
        _this.TransportationTypeCode = transportationTypeCode;
        _this.CargoTypeCode = cargoTypeCode;
        return _this;
    }
    Object.defineProperty(DocumentsDefinitionPMComponent.prototype, "DocumentTypeCode", {
        get: function () { return this._DocumentTypeCode; },
        set: function (newValue) { this._DocumentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionPMComponent.prototype, "ProcessTypeCode", {
        get: function () { return this._ProcessTypeCode; },
        set: function (newValue) { this._ProcessTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionPMComponent.prototype, "TransportationTypeCode", {
        get: function () { return this._TransportationTypeCode; },
        set: function (newValue) { this._TransportationTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentsDefinitionPMComponent.prototype, "CargoTypeCode", {
        get: function () { return this._CargoTypeCode; },
        set: function (newValue) { this._CargoTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    return DocumentsDefinitionPMComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentsDefinitionPMComponent = DocumentsDefinitionPMComponent;
//# sourceMappingURL=CustomsDocumentsDefinitionComponent.js.map