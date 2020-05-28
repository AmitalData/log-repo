"use strict";
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var LogSearchWindowButtonsComponent = /** @class */ (function () {
    function LogSearchWindowButtonsComponent(CD, _entityListService) {
        this.CD = CD;
        this._entityListService = _entityListService;
        this.PartnerTypes = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    LogSearchWindowButtonsComponent.prototype.setVariables = function (rowData, fieldName) {
        var _this = this;
        this.rowData = rowData;
        var arry = fieldName.split(',');
        this.fieldName = arry[0];
        this.LookUpTableName = arry[1];
        this.LookUpTable = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
        var apiQueryFilter;
        apiQueryFilter = new ApiQueryFilters_1.ApiQueryFilters();
        var partnerTypes;
        this._entityListService.getAllFromCache("PartnerType", apiQueryFilter).then(function (res3) {
            res3.subscribe(function (res4) {
                _this.PartnerTypes = res4.Result;
            });
        });
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    LogSearchWindowButtonsComponent.prototype.ngOnInit = function () {
    };
    LogSearchWindowButtonsComponent.prototype.EditButtonClicked22 = function () {
        this.CurrentSession.PseventRowSelectEvent.emit(this.LookUpTable.Name);
        var id = this.rowData['Id'];
        if (!Tools_1.AppTool.IsNullOrEmpty(id) && !Tools_1.AppTool.IsNullOrEmpty(this.LookUpTableName)) {
            console.log("Editing: " + this.LookUpTableName + " " + id);
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit " + TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.LookUpTableName);
            logWindow.ShowEditComponent(id, this.LookUpTableName);
            //logWindow.WindowClosed.subscribe(($event: any) => {
            //    this.FireEvent("ok from LSWBC");
            //});
        }
    };
    LogSearchWindowButtonsComponent.prototype.EditButtonClicked = function () {
        var _this = this;
        this.CurrentSession.PseventRowSelectEvent.emit(this.LookUpTable.Name);
        var id = this.rowData['Id'];
        if (!Tools_1.AppTool.IsNullOrEmpty(id) && !Tools_1.AppTool.IsNullOrEmpty(this.LookUpTableName)) {
            if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "UPDATE") && this.LookUpTable.EnableSecurity) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("You have no permission to edit an entity of this type.");
                return;
            }
            if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Your package doesn't include this module..");
                return;
            }
            if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
                this.LookUpTableName = this.GetObjectTableNameForDependency(this.rowData["PartnerTypeId"], this.LookUpTableName);
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Edit " + TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.LookUpTableName);
                logWindow.ShowEditComponent(id, this.LookUpTableName);
                logWindow.WindowClosed.subscribe(function ($event) {
                    _this.OnEditCompleted();
                });
            }
        }
    };
    LogSearchWindowButtonsComponent.prototype.OnEditCompleted = function () {
        this.CurrentSession.SessionEvent.emit(this.rowData['Id']);
    };
    LogSearchWindowButtonsComponent.prototype.GetObjectTableNameForDependency = function (dependency, parentObjectName) {
        if (parentObjectName == "Card" && dependency == "PO") {
            dependency = "CS";
        }
        var partnerType = this.PartnerTypes.filter(function (p) { return p.Id.toLowerCase() == dependency.toLowerCase(); })[0];
        if (partnerType != null && partnerType != undefined) {
            var name = partnerType.Name.replace(" ", "");
            var table = window.ObjectTables.filter(function (d) { return d.Name.toLowerCase() === name.toLocaleLowerCase(); })[0];
            if (table != null) {
                return table.Name;
            }
            else {
                return parentObjectName;
            }
        }
        else {
            return parentObjectName;
        }
    };
    LogSearchWindowButtonsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogSearchWindowButtonsComponent',
            templateUrl: './LogSearchWindowButtonsComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], LogSearchWindowButtonsComponent);
    return LogSearchWindowButtonsComponent;
}());
exports.LogSearchWindowButtonsComponent = LogSearchWindowButtonsComponent;
//# sourceMappingURL=LogSearchWindowButtonsComponent.js.map