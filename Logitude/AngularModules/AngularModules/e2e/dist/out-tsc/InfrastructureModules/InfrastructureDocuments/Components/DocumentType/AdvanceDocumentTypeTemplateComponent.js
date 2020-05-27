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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DocumentTypeTemplatePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var forms_1 = require("@angular/forms");
var AdvanceDocumentTypeTemplateComponent = /** @class */ (function (_super) {
    __extends(AdvanceDocumentTypeTemplateComponent, _super);
    function AdvanceDocumentTypeTemplateComponent(fb) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CountryId = "";
        _this.IsLoadPage = false;
        _this.CountryLists = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (_this.documentTypeTemplatePMService == null) {
            _this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
        }
        _this.myForm = fb.group({});
        return _this;
    }
    AdvanceDocumentTypeTemplateComponent.prototype.ngOnInit = function () {
    };
    AdvanceDocumentTypeTemplateComponent.prototype.SetDataContext = function (entityPM) {
        var _this = this;
        this.EntityPM = entityPM;
        var myService = new CountryListService_1.CountryListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError && myResponse.Result) {
                _this.CountryLists = myResponse.Result;
                if (_this.CountryLists) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CountryCode)) {
                        var countryList = _this.CountryLists.filter(function (d) { return d.Code == _this.EntityPM.CountryCode; })[0];
                        if (countryList) {
                            _this.CountryId = countryList.Id;
                        }
                    }
                }
            }
            _this.IsLoadPage = true;
        });
    };
    AdvanceDocumentTypeTemplateComponent.prototype.CountrySelectedChange = function (value) {
        if (value) {
            this.EntityPM.CountryCode = value.Code;
        }
        else
            this.EntityPM.CountryCode = "";
    };
    AdvanceDocumentTypeTemplateComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        this.documentTypeTemplatePMService.update(this.EntityPM).subscribe(function (res) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.CloseButtonClicked();
        });
    };
    AdvanceDocumentTypeTemplateComponent.prototype.CloseButtonClicked = function () {
        this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AdvanceDocumentTypeTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AdvanceDocumentTypeTemplate',
            templateUrl: './AdvanceDocumentTypeTemplateComponent.html',
            providers: [DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService]
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder])
    ], AdvanceDocumentTypeTemplateComponent);
    return AdvanceDocumentTypeTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.AdvanceDocumentTypeTemplateComponent = AdvanceDocumentTypeTemplateComponent;
//# sourceMappingURL=AdvanceDocumentTypeTemplateComponent.js.map