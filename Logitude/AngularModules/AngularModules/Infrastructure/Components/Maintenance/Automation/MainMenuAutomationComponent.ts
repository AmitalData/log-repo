
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
@Component({
    moduleId: module.id,

    selector: 'MainMenuAutomationComponent',
    templateUrl: './MainMenuAutomationComponent.html',


})
export class MainMenuAutomationComponent implements OnInit {

    IsShowTicket: boolean = false;
    IsShowTransmissionLogs: boolean = false;
    AutomationEntityLists: AutomationItemClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


    }

    ngOnInit(


    ) {

        
        if (FeatureLocator.HasFeaturePermession("Shipment", "AUTOMATION")) {
            this.AutomationEntityLists.push(new AutomationItemClass("Masters", "Master","Master", "Masters & Directs"));
            this.AutomationEntityLists.push(new AutomationItemClass("Shipments", "Shipment","Shipment", "Houses & Directs"));
        }

        if (FeatureLocator.HasFeaturePermession("Ticket", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Tickets", "Ticket", "Ticket"));

        if (FeatureLocator.HasFeaturePermession("LogitudeMessagesTransmissionLog", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Transmission Logs", "LogitudeMessagesTransmissionLog","Transmission Log"));

       

    }

    SetDataContext(dataContext: any) {
    

    }


    ItemClicked(item: AutomationItemClass) {


        var logWindow = new LogitudeWindow();
        logWindow.Width = 980;
        logWindow.Height = 650;
        logWindow.Title = (item.DisplayName + " Automations / Results Settings");
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = item;
        logWindow.Show('./Infrastructure/Components/Automation/AutomationsSettingsComponent');

        logWindow.WindowClosed.subscribe(($event: any) => {
          


        });

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
    constructor(name: string, objectTableName: string, displayName:string, description: string = null) {

        this.Name = name;
        this.ObjectTableName = objectTableName;
        this.DisplayName = displayName;
        this.Description = description;
    }

}
