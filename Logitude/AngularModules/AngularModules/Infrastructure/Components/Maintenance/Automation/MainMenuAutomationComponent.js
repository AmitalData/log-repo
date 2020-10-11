import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import { Component } from '@angular/core';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
export var MainMenuAutomationComponent = (function () {
    function MainMenuAutomationComponent() {
        this.IsShowTicket = false;
        this.IsShowTransmissionLogs = false;
        this.AutomationEntityLists = [];
    }
    MainMenuAutomationComponent.prototype.ngOnInit = function () {
        if (FeatureLocator.HasFeaturePermession("Shipment", "AUTOMATION"))
            this.AutomationEntityLists.push(new AutomationItemClass("Shipments", "Shipment"));
        if (FeatureLocator.HasFeaturePermession("Ticket", "AUTOMATION"))
            this.AutomationEntityLists.push(new AutomationItemClass("Tickets", "Ticket"));
        if (FeatureLocator.HasFeaturePermession("LogitudeMessagesTransmissionLog", "AUTOMATION"))
            this.AutomationEntityLists.push(new AutomationItemClass("Transmission Logs", "LogitudeMessagesTransmissionLog"));
    };
    MainMenuAutomationComponent.prototype.SetDataContext = function (dataContext) {
    };
    MainMenuAutomationComponent.prototype.ItemClicked = function (item) {
        var logWindow = new LogitudeWindow();
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
        SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    MainMenuAutomationComponent.decorators = [
        { type: Component, args: [{
                    moduleId: module.id,
                    selector: 'MainMenuAutomationComponent',
                    templateUrl: './MainMenuAutomationComponent.html',
                },] },
    ];
    /** @nocollapse */
    MainMenuAutomationComponent.ctorParameters = [];
    return MainMenuAutomationComponent;
}());
export var AutomationItemClass = (function () {
    function AutomationItemClass(name, objectTableName) {
        this.Name = name;
        this.ObjectTableName = objectTableName;
    }
    return AutomationItemClass;
}());
//# sourceMappingURL=MainMenuAutomationComponent.js.map