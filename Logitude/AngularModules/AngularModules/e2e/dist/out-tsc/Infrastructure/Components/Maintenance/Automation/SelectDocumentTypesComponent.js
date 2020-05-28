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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AutomationFollowUp_1 = require("../../../../Infrastructure/DataContracts/AutomationFollowUp");
var SelectDocumentTypesComponent = /** @class */ (function (_super) {
    __extends(SelectDocumentTypesComponent, _super);
    function SelectDocumentTypesComponent() {
        var _this = _super.call(this) || this;
        _this.DocumentTypes = [];
        _this.DocumentTypeLists = [];
        _this.SelectedDocumentTypeLists = [];
        _this.Area = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    SelectDocumentTypesComponent.prototype.ngOnInit = function () {
    };
    SelectDocumentTypesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.AddEditAutomationsComponent = args.AddEditAutomationsComponent;
        if (this.AddEditAutomationsComponent) {
            if (this.AddEditAutomationsComponent.ResultCodeSelected) {
                if (this.AddEditAutomationsComponent.ResultCodeSelected.Code == "DOCOUTFOLLOWUP")
                    this.Area = "DocOut";
                else if (this.AddEditAutomationsComponent.ResultCodeSelected.Code == "DOCINFOLLOWUP")
                    this.Area = "DocIn";
            }
            if (this.AddEditAutomationsComponent.AutomationFollowUp) {
                this.SelectedDocumentTypeLists = this.AddEditAutomationsComponent.AutomationFollowUp.DocumentTypeLists;
                if (!this.SelectedDocumentTypeLists)
                    this.SelectedDocumentTypeLists = [];
                this.SelectedDocumentTypeLists = this.SelectedDocumentTypeLists.filter(function (d) { return d.Area == _this.Area; });
            }
            if (this.AddEditAutomationsComponent.AllDocumentTypeLists) {
                if (this.Area == "DocIn") {
                    this.DocumentTypes = this.AddEditAutomationsComponent.AllDocumentTypeLists.filter(function (d) { return d.IsDocIn && d.ObjectTableId == _this.AddEditAutomationsComponent.ObjectTableId; });
                }
                else {
                    this.DocumentTypes = this.AddEditAutomationsComponent.AllDocumentTypeLists.filter(function (d) { return d.IsDocOut && d.ObjectTableId == _this.AddEditAutomationsComponent.ObjectTableId; });
                }
                this.DocumentTypes.forEach(function (item) {
                    _this.DocumentTypeLists.push(new SelectDocumentTypeViewModel(item, _this.Area, _this));
                });
            }
        }
    };
    SelectDocumentTypesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SelectDocumentTypesComponent.prototype.SaveButtonClicked = function () {
        this.AddEditAutomationsComponent.AutomationFollowUp.DocumentTypeLists = this.SelectedDocumentTypeLists;
        this.CurrentSession.CurrentWindow.Close("Save");
    };
    SelectDocumentTypesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SelectDocumentTypesComponent',
            templateUrl: './SelectDocumentTypesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SelectDocumentTypesComponent);
    return SelectDocumentTypesComponent;
}(BaseComponent_1.BaseComponent));
exports.SelectDocumentTypesComponent = SelectDocumentTypesComponent;
var SelectDocumentTypeViewModel = /** @class */ (function () {
    function SelectDocumentTypeViewModel(documentType, area, viewModel) {
        this.Name = documentType.Name;
        this.ViewModel = viewModel;
        this.DocumentType = documentType;
    }
    Object.defineProperty(SelectDocumentTypeViewModel.prototype, "IsSelect", {
        get: function () {
            var _this = this;
            var isSelect = false;
            if (this.ViewModel.SelectedDocumentTypeLists && this.DocumentType) {
                if (this.ViewModel.SelectedDocumentTypeLists.filter(function (d) { return d.Id == _this.DocumentType.Id; })[0]) {
                    isSelect = true;
                }
            }
            return isSelect;
        },
        set: function (newValue) {
            var _this = this;
            this.ViewModel.AddEditAutomationsComponent.IsChangeAutomation = true;
            if (this.ViewModel.SelectedDocumentTypeLists.filter(function (d) { return d.Id == _this.DocumentType.Id; })[0]) {
                this.ViewModel.SelectedDocumentTypeLists = this.ViewModel.SelectedDocumentTypeLists.filter(function (d) { return d.Id != _this.DocumentType.Id; });
            }
            else {
                var document = new AutomationFollowUp_1.FollowUpDocumentTypeList();
                document.Id = this.DocumentType.Id;
                document.Name = this.DocumentType.Name;
                document.Area = this.ViewModel.Area;
                this.ViewModel.SelectedDocumentTypeLists.push(document);
            }
        },
        enumerable: true,
        configurable: true
    });
    return SelectDocumentTypeViewModel;
}());
//# sourceMappingURL=SelectDocumentTypesComponent.js.map