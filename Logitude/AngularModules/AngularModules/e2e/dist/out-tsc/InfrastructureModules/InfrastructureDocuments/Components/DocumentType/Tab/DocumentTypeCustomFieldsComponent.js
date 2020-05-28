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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var DocumentTypeCustomFieldsViewModel_1 = require("../ViewModel/DocumentTypeCustomFieldsViewModel");
var DocumentTypeCustomFieldPM_1 = require("../../../../../Common/EntityPMs/DocumentTypeCustomFieldPM");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var FieldDataTypeService_1 = require("../../../../../Common/Services/ExtendedPMs/FieldDataTypeService");
var DocumentTypeCustomFieldsComponent = /** @class */ (function (_super) {
    __extends(DocumentTypeCustomFieldsComponent, _super);
    function DocumentTypeCustomFieldsComponent(entityArgs, _fieldDataTypeService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._fieldDataTypeService = _fieldDataTypeService;
        _this.IsShowButtonDelete = false;
        _this.EditingIsEnabled = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsVisibile = false;
        return _this;
    }
    DocumentTypeCustomFieldsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.FullCustomFieldsLists = [];
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeCustomField", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            _this.EntityPM = _this.entityArgs.EntityPM;
            if (_this.EntityPM) {
                _this.Run();
            }
        });
    };
    DocumentTypeCustomFieldsComponent.prototype.Run = function () {
        var _this = this;
        this.FieldDataTypeLists = [];
        this.CustomFieldsLists = [];
        this.FullCustomFieldsLists = [];
        if (this.EntityPM.DocumentTypeCustomFields) {
            this.EntityPM.DocumentTypeCustomFields.forEach(function (customFields) {
                var item = new DocumentTypeCustomFieldsViewModel_1.DocumentTypeCustomFieldsViewModel(customFields);
                _this.CustomFieldsLists.push(item);
                _this.FullCustomFieldsLists.push(item);
            });
        }
        this._fieldDataTypeService.GetFieldDataTypes(this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.FieldDataTypeLists = myResult;
                }
            }
        });
    };
    DocumentTypeCustomFieldsComponent.prototype.onSearchTextChangeEvent = function (search) {
        if (search) {
            if (search != "Search" && this.FullCustomFieldsLists) {
                this.CustomFieldsLists = this.FullCustomFieldsLists.filter(function (d) { return d.EntityPM.Name && d.EntityPM.Name.toUpperCase().indexOf(search.toUpperCase()) > -1; });
            }
        }
        else
            this.CustomFieldsLists = this.FullCustomFieldsLists;
    };
    DocumentTypeCustomFieldsComponent.prototype.AddCustomFieldButtonClicked = function () {
        var entityPM = new DocumentTypeCustomFieldPM_1.DocumentTypeCustomFieldPM();
        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        entityPM.DocumentTypeId = this.EntityPM.Id;
        this.ShowAddEditDocumentTypeCustomFieldComponent(entityPM, "Add", "Add Custom Fields");
    };
    DocumentTypeCustomFieldsComponent.prototype.EditCustomFieldButtonClicked = function () {
        if (this.SelectedDocumentTypeCustomFieldsViewModel) {
            this.ShowAddEditDocumentTypeCustomFieldComponent(this.SelectedDocumentTypeCustomFieldsViewModel.EntityPM, "Edit", "Edit Custom Fields");
        }
    };
    DocumentTypeCustomFieldsComponent.prototype.ShowAddEditDocumentTypeCustomFieldComponent = function (entityPM, mode, title) {
        var windowArgs = {};
        windowArgs.EntityPM = entityPM;
        windowArgs.DataViewModel = this;
        windowArgs.Mode = mode;
        windowArgs.FieldDataTypeLists = this.FieldDataTypeLists;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = title;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/Tab/AddEditDocumentTypeCustomFieldComponent");
    };
    DocumentTypeCustomFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentTypeCustomFields',
            templateUrl: './DocumentTypeCustomFieldsComponent.html',
            providers: [FieldDataTypeService_1.FieldDataTypeService],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, FieldDataTypeService_1.FieldDataTypeService])
    ], DocumentTypeCustomFieldsComponent);
    return DocumentTypeCustomFieldsComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentTypeCustomFieldsComponent = DocumentTypeCustomFieldsComponent;
//# sourceMappingURL=DocumentTypeCustomFieldsComponent.js.map