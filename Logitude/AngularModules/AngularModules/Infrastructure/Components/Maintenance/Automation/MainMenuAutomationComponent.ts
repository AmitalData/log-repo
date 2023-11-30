
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Component, OnInit } from '@angular/core';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from 'Infrastructure/Args';
@Component({


    selector: 'MainMenuAutomationComponent',
    templateUrl: './MainMenuAutomationComponent.html',


})
export class MainMenuAutomationComponent implements OnInit {

    IsShowTicket: boolean = false;
    IsShowTransmissionLogs: boolean = false;
    AutomationEntityLists: AutomationItemClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    IsMainteneceView: boolean = false;

    constructor() {
    }

    ngOnInit() {

        if (FeatureLocator.HasFeaturePermession("Shipment", "AUTOMATION")) {
            this.AutomationEntityLists.push(new AutomationItemClass("Masters", "Master", "Master", "Masters & Directs"));
            this.AutomationEntityLists.push(new AutomationItemClass("Shipments", "Shipment", "Shipment", "Houses & Directs"));
        }

        if (FeatureLocator.HasFeaturePermession("Ticket", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Tickets", "Ticket", "Ticket"));
        if (FeatureLocator.HasFeaturePermession("LogitudeMessagesTransmissionLog", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Transmission Logs", "LogitudeMessagesTransmissionLog", "Transmission Log"));
        if (FeatureLocator.HasFeaturePermession("ARInvoice", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("AR Invoices", "ARInvoice", "AR Invoice", "Receivables Invoices"));
        if (FeatureLocator.HasFeaturePermession("APInvoice", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("AP Invoices", "APInvoice", "AP Invoice", "Payables Invoices"));
        if (FeatureLocator.HasFeaturePermession("Quote", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Quotes", "Quote", "Quote"));
         if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Customs.Declaration", "Customs.Declaration", "הצהרות"));

         if (FeatureLocator.HasFeaturePermession("Container", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Containers", "Container", "Container"));

        if (FeatureLocator.HasFeaturePermession("WorkFlow", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Workflows", "WorkFlow", "Workflow"));
        if (FeatureLocator.HasFeaturePermession("WarehouseEntry", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Cross Dock Entry", "WarehouseEntry", "Cross Dock Entry"));
        if (FeatureLocator.HasFeaturePermession("WarehouseRelease", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Cross Dock Release", "WarehouseRelease", "Cross Dock Release"));

 
    }

    SetDataContext(dataContext: any) {
        this.IsMainteneceView = dataContext.IsMainteneceView;
    }


    ItemClicked(item: AutomationItemClass) {

        if (item.ObjectTableName === "WorkFlow") {

            if(this.IsMainteneceView){
                this.CloseButtonClicked();
            }

            let listArgs = new ListComponentArgs();
            listArgs.QueryCode = "All Workflows";
            listArgs.ObjectTableName = "WorkFlow";
            listArgs.DisplayTitle = "Workflows";
            listArgs.BackButtonTitle = this.IsMainteneceView ? "Maintenance" : "Automations";
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });
            
        } else {
            var windowArgs: any = {};
            windowArgs.AutomationItemClass = item;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 980;
            logWindow.Height = 650;
            logWindow.Title = (item.DisplayName + " Automations / Results Settings");
            logWindow.IsShowCloseButton = true;
            logWindow.DataContext = item;
            logWindow.Show('./Infrastructure/Components/Automation/AutomationsSettingsComponent');
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => {



            });
        }

    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }





}

export class AutomationItemClass {

    Name: string;
    ObjectTableName: string;
    Description: string;
    DisplayName: string;
    constructor(name: string, objectTableName: string, displayName: string, description: string = null) {

        this.Name = name;
        this.ObjectTableName = objectTableName;
        this.DisplayName = displayName;
        this.Description = description;
    }

}
