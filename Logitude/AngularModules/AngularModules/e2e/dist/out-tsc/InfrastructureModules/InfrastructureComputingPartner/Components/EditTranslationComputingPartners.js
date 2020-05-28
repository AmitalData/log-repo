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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ComputingPartnerTranslationPM_1 = require("../../../Common/EntityPMs/ComputingPartnerTranslationPM");
var ComputingPartnerTranslationPMService_1 = require("../../../Common/Services/StandardPMs/ComputingPartnerTranslationPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var EditTranslationComputingPartners = /** @class */ (function (_super) {
    __extends(EditTranslationComputingPartners, _super);
    function EditTranslationComputingPartners() {
        var _this = _super.call(this) || this;
        _this.BackCompleted = new core_1.EventEmitter();
        _this.DataLoaded = false;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        var service = new EntityResourceService_1.EntityResourceService();
        service.getEntityResourceByTableName("ComputingPartnerTranslation", 0).subscribe(function (p) {
        });
        return _this;
    }
    EditTranslationComputingPartners.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.TranslatedEntity = args.entityPM;
        this.ObjectTableName = this.TranslatedEntity.ObjectTableName;
        var service = new ComputingPartnerTranslationPMService_1.ComputingPartnerTranslationPMService();
        var Id = this.TranslatedEntity.Id;
        if (Id != null) {
            service.get(this.TranslatedEntity.Id).subscribe(function (p) {
                if (!p.HasError) {
                    _this.EntityPM = p.Result;
                    _this.SetUiProperties();
                    _this.DataLoaded = true;
                }
            });
        }
        else {
            this.EntityPM = new ComputingPartnerTranslationPM_1.ComputingPartnerTranslationPM();
            this.EntityPM.ComputingPartnerId = this.TranslatedEntity.ComputingPartnerId;
            this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.EntityPM.OurCode = this.TranslatedEntity.OurCode;
            this.EntityPM.PartnerCode = this.TranslatedEntity.PartnerCode;
            this.EntityPM.ComputingPartnerName = this.TranslatedEntity.ComputingPartnerName;
            this.EntityPM.ObjectTableName = this.TranslatedEntity.ObjectTableName;
            this.EntityPM.ObjectTableId = this.TranslatedEntity.ObjectTableId;
            this.SetUiProperties();
            this.DataLoaded = true;
        }
    };
    EditTranslationComputingPartners.prototype.SetUiProperties = function () {
        this.UIProperties.SetEnabled("OurCode", this.ObjectTableName, false);
    };
    EditTranslationComputingPartners.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditTranslationComputingPartners.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new ComputingPartnerTranslationPMService_1.ComputingPartnerTranslationPMService();
        if (this.EntityPM.Id != null) {
            if (this.EntityPM.PartnerCode == null)
                this.EntityPM.PartnerCode = "";
            service.update(this.EntityPM).subscribe(function (p) {
                _this.CurrentSession.StopBusyIndicator();
                _this.BackCompleted.emit("event");
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
        else {
            service.insert(this.EntityPM).subscribe(function (p) {
                _this.CurrentSession.StopBusyIndicator();
                _this.BackCompleted.emit("event");
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
    };
    Object.defineProperty(EditTranslationComputingPartners.prototype, "OurCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.OurCode : null; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditTranslationComputingPartners.prototype, "PartnerCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.PartnerCode : null; },
        set: function (value) {
            if (value != this.EntityPM.PartnerCode)
                this.EntityPM.PartnerCode = value;
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditTranslationComputingPartners.prototype, "BackCompleted", void 0);
    EditTranslationComputingPartners = __decorate([
        core_1.Component({
            selector: 'EditTranslationComputingPartners',
            moduleId: module.id,
            templateUrl: './EditTranslationComputingPartners.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditTranslationComputingPartners);
    return EditTranslationComputingPartners;
}(BaseComponent_1.BaseComponent));
exports.EditTranslationComputingPartners = EditTranslationComputingPartners;
//# sourceMappingURL=EditTranslationComputingPartners.js.map