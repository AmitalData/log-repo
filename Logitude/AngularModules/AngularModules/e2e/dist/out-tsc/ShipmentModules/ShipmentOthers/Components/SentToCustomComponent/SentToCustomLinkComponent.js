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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SentToCustomLinkComponent = /** @class */ (function (_super) {
    __extends(SentToCustomLinkComponent, _super);
    function SentToCustomLinkComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    SentToCustomLinkComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.EntityPM = windowArgs;
    };
    SentToCustomLinkComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SentToCustomLinkComponent.prototype.ViewCustomsSettings = function () {
        var _this = this;
        this.CurrentSession.CloseCurrentWindow();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Customs Settings";
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent');
        logWindow.WindowClosed.subscribe(function (comp) {
            if (comp == "OK") {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Customs Transmissions";
                logWindow.Height = 600;
                logWindow.WindowArgs = _this.EntityPM;
                logWindow.Show('./ShipmentModules/ShipmentOthers/Components/SentToCustomComponent/SentToCustomComponent');
            }
        });
    };
    SentToCustomLinkComponent = __decorate([
        core_1.Component({
            selector: 'SentToCustomLinkComponent',
            moduleId: module.id,
            templateUrl: './SentToCustomLinkComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SentToCustomLinkComponent);
    return SentToCustomLinkComponent;
}(BaseComponent_1.BaseComponent));
exports.SentToCustomLinkComponent = SentToCustomLinkComponent;
//# sourceMappingURL=SentToCustomLinkComponent.js.map