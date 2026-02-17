
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
            this.AutomationEntityLists.push(new AutomationItemClass("Masters", "Master", "Masters & Directs"));
            this.AutomationEntityLists.push(new AutomationItemClass("Shipments", "Shipment", "Houses & Directs"));
        }

        if (FeatureLocator.HasFeaturePermession("Ticket", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Tickets", "Ticket"));

        if (FeatureLocator.HasFeaturePermession("LogitudeMessagesTransmissionLog", "AUTOMATION")) this.AutomationEntityLists.push(new AutomationItemClass("Transmission Logs", "LogitudeMessagesTransmissionLog"));

       

    }

    SetDataContext(dataContext: any) {
    

    }


    ItemClicked(item: AutomationItemClass) {


        var logWindow = new LogitudeWindow();
        logWindow.Width = 980;
        logWindow.Height = 650;
        logWindow.Title = "Automations / Results Settings";
        logWindow.IsShowCloseButton = true;
        logWindow.DataContext = item.ObjectTableName;
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
    constructor(name: string, objectTableName: string, description: string = null) {

        this.Name = name;
        this.ObjectTableName = objectTableName;
        this.Description = description;
    }

}
