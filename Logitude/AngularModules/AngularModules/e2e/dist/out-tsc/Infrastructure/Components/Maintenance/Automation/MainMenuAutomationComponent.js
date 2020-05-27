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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MainMenuAutomationComponent = /** @class */ (function () {
    function MainMenuAutomationComponent() {
        this.IsShowTicket = false;
        this.IsShowTransmissionLogs = false;
        this.AutomationEntityLists = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    MainMenuAutomationComponent.prototype.ngOnInit = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "AUTOMATION")) {
            this.AutomationEntityLists.push(new AutomationItemClass("Masters", "Master", "Masters & Directs"));
            this.AutomationEntityLists.push(new AutomationItemClass("Shipments", "Shipment", "Houses & Directs"));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "AUTOMATION"))
            this.AutomationEntityLists.push(new AutomationItemClass("Tickets", "Ticket"));
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("LogitudeMessagesTransmissionLog", "AUTOMATION"))
            this.AutomationEntityLists.push(new AutomationItemClass("Transmission Logs", "LogitudeMessagesTransmissionLog"));
    };
    MainMenuAutomationComponent.prototype.SetDataContext = function (dataContext) {
    };
    MainMenuAutomationComponent.prototype.ItemClicked = function (item) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 980;
        logWindow.Height = 650;
        logWindow.Title = "Automations / Results Settings";
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = item.ObjectTableName;
        logWindow.Show('./Infrastructure/Components/Automation/AutomationsSettingsComponent');
        logWindow.WindowClosed.subscribe(function ($event) {
        });
    };
    MainMenuAutomationComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    MainMenuAutomationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MainMenuAutomationComponent',
            templateUrl: './MainMenuAutomationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MainMenuAutomationComponent);
    return MainMenuAutomationComponent;
}());
exports.MainMenuAutomationComponent = MainMenuAutomationComponent;
var AutomationItemClass = /** @class */ (function () {
    function AutomationItemClass(name, objectTableName, description) {
        if (description === void 0) { description = null; }
        this.Name = name;
        this.ObjectTableName = objectTableName;
        this.Description = description;
    }
    return AutomationItemClass;
}());
exports.AutomationItemClass = AutomationItemClass;
//# sourceMappingURL=MainMenuAutomationComponent.js.map