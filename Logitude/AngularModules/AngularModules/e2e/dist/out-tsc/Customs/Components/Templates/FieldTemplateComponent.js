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
var CourierMasterService_1 = require("../../Services/Others/CourierMasterService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var FieldTemplateComponent = /** @class */ (function () {
    function FieldTemplateComponent() {
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.SpotlightDataTemplate = null;
        this.IsSpotLightTemplate = false;
        this.IsHeaderScreenTemplate = false;
        this.courierMasterService = new CourierMasterService_1.CourierMasterService();
        this.Retries = 0;
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
        }
        if (this.IsSpotLightTemplate) {
            this.RunComponent();
        }
    };
    FieldTemplateComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.SpotLightViewContainerRef) {
            this.SpotLightViewContainerRef.clear();
            var myComponentPath = "./Customs/Components/Spotlight/CustomsSpotlightComponent";
            SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.SpotLightViewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.Run(_this.Entity.Id);
            });
        }
        else {
            this.RunComponentTimer();
        }
    };
    FieldTemplateComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    FieldTemplateComponent.prototype.OpenCourierMaster = function () {
        var _this = this;
        this.courierMasterService.getCourierMasterByDeclarationId(this.Entity.Id).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.EditEntity("Customs.CourierMaster", response.Result.Id, null, "COGN");
                }
            }
        });
    };
    FieldTemplateComponent.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
        });
    };
    __decorate([
        core_1.ViewChild('SpotLight', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], FieldTemplateComponent.prototype, "SpotLightViewContainerRef", void 0);
    FieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FieldTemplateComponent);
    return FieldTemplateComponent;
}());
exports.FieldTemplateComponent = FieldTemplateComponent;
//# sourceMappingURL=FieldTemplateComponent.js.map