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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ContactPMService_1 = require("../../../../../Common/Services/StandardPMs/ContactPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ParticipantNotifyTabComponent = /** @class */ (function (_super) {
    __extends(ParticipantNotifyTabComponent, _super);
    function ParticipantNotifyTabComponent(entityArgs, _entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Participant";
        _this.EntityPM = _this.entityArgs.EntityPM;
        return _this;
    }
    Object.defineProperty(ParticipantNotifyTabComponent.prototype, "FWBNotifyContacts", {
        get: function () { return this.EntityPM.FWBNotifyContacts; },
        set: function (value) { if (this.EntityPM.FWBNotifyContacts != value)
            this.EntityPM.FWBNotifyContacts = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantNotifyTabComponent.prototype, "FHLNotifyContacts", {
        get: function () { return this.EntityPM.FHLNotifyContacts; },
        set: function (value) { if (this.EntityPM.FHLNotifyContacts != value)
            this.EntityPM.FHLNotifyContacts = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ParticipantNotifyTabComponent.prototype, "FFRNotifyContacts", {
        get: function () { return this.EntityPM.FFRNotifyContacts; },
        set: function (value) { if (this.EntityPM.FFRNotifyContacts != value)
            this.EntityPM.FFRNotifyContacts = value; },
        enumerable: true,
        configurable: true
    });
    ParticipantNotifyTabComponent.prototype.Add = function (code) {
        var _this = this;
        var windowTitle = "Add Contact";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(function (p) {
            logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(s) && s != "cancel") {
                        var service = new ContactPMService_1.ContactPMService();
                        service.get(s).subscribe(function (p) {
                            var email = p.Result.Email;
                            if (code == "FWB")
                                _this.FWBNotifyContacts = _this.FWBNotifyContacts + ";" + email;
                            else if (code == "FHL")
                                _this.FHLNotifyContacts = _this.FHLNotifyContacts + ";" + email;
                            else
                                _this.FFRNotifyContacts = _this.FFRNotifyContacts + ";" + email;
                        });
                    }
                });
            });
        });
    };
    ParticipantNotifyTabComponent = __decorate([
        core_1.Component({
            selector: 'NewCurrencyComponent',
            moduleId: module.id,
            templateUrl: './ParticipantNotifyTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ParticipantNotifyTabComponent);
    return ParticipantNotifyTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ParticipantNotifyTabComponent = ParticipantNotifyTabComponent;
//# sourceMappingURL=ParticipantNotifyTabComponent.js.map