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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CustomsRequierdFieldsWebService_1 = require("../../../../Customs/Services/WebServices/CustomsRequierdFieldsWebService");
var RequiredFieldsComponent = /** @class */ (function (_super) {
    __extends(RequiredFieldsComponent, _super);
    function RequiredFieldsComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsRequiredField";
        _this.customsRequierdFieldsWebService = new CustomsRequierdFieldsWebService_1.CustomsRequierdFieldsWebService();
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.OriginalTablesList = [];
        _this.TablesList = [];
        _this.FieldsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsNoFields = false;
        _this.GetCustomsObjectTables();
        return _this;
        //this.BuildTablesList();
    }
    RequiredFieldsComponent.prototype.GetCustomsObjectTables = function () {
        var _this = this;
        this.customsRequierdFieldsWebService.GetSomeObjectTables().subscribe(function (response) {
            var res = response.Result;
            console.log("[Response] customsRequierdFieldsWebService.GetSomeObjectTables: ", res);
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                _this.TablesList = [];
                _this.TablesList = res;
                _this.BuildTablesList();
            }
        });
    };
    RequiredFieldsComponent.prototype.BuildTablesList = function () {
        //this.TablesList = [];
        //this.TablesList.push({ Name: "demo Declaration"});
        //this.TablesList.push({ Name: "demo Supplier Invoices Modification"});
        //this.TablesList.push({ Name: "demo Supplier invoices connected Declaration"});
        //this.TablesList.push({ Name: "demo Physical Checks"});
        //this.TablesList.push({ Name: "demo Procedural Faults"});
        //this.TablesList.push({ Name: "demo Bla Bla Bla"});
        //this.TablesList.push({ Name: "table number 7" });
        this.TablesList.forEach(function (el) {
            el["TranslatedName"] = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(el.Name);
        });
        this.OriginalTablesList = this.TablesList;
    };
    RequiredFieldsComponent.prototype.AddRemoveFieldsButtonClicked = function () {
        var _this = this;
        var window = new LogitudeWindow_1.LogitudeWindow();
        if (this.SelectedTable) {
            window.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.AddRemoveRequiredFields");
            window.ShowCloseButton = true;
            window.WindowArgs = {
                SelectedObjectTableName: this.SelectedTable.Name,
                SelectedObjectFields: this.FieldsList,
            };
            window.Show("./CustomsModules/CustomsMaintenance/Components/RequiredFields/AddEditRequiredFieldsComponent");
            window.WindowClosed.subscribe(function ($event) {
                _this.TableNameClicked(_this.SelectedTable);
            });
        }
        else {
            var msg = new MessageWindow_1.MessageWindow();
            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.SelectObjectTable"));
        }
    };
    RequiredFieldsComponent.prototype.TableNameSearchTextChanged = function (event) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(event)) {
            this.TablesList = [];
            this.OriginalTablesList.forEach(function (el) {
                if (el.TranslatedName.toLowerCase().includes(event.toLowerCase()))
                    _this.TablesList.push(el);
            });
        }
        else {
            this.TablesList = this.OriginalTablesList;
        }
    };
    RequiredFieldsComponent.prototype.TableNameClicked = function (table) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(table)) {
            this.SelectedTable = table;
            this._EntityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (res) {
                _this.customsRequierdFieldsWebService.GetCustomsRequiredFieldListsByObjectTable(table.Id)
                    .subscribe(function (response) {
                    var res = response.Result;
                    _this.IsNoFields = false;
                    console.log("[Response] GetCustomsRequiredFieldListsByObjectTable: ", res);
                    if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                        _this.FieldsList = [];
                        _this.FieldsList = res;
                        if (_this.FieldsList.length == 0) {
                            _this.IsNoFields = true;
                        }
                        else {
                            _this.TranslateFieldsNames();
                        }
                    }
                });
            });
        }
    };
    RequiredFieldsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RequiredFieldsComponent.prototype.TranslateFieldsNames = function () {
        this.FieldsList.forEach(function (field) {
            var objectField = window.ObjectFields.find(function (x) { return x.Id == field.ObjectfieldId; });
            field.ObjectFieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(objectField.FullNameTextCodeCode);
        });
    };
    RequiredFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'RequiredFieldsComponent',
            templateUrl: 'RequiredFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], RequiredFieldsComponent);
    return RequiredFieldsComponent;
}(BaseComponent_1.BaseComponent));
exports.RequiredFieldsComponent = RequiredFieldsComponent;
//# sourceMappingURL=RequiredFieldsComponent.js.map