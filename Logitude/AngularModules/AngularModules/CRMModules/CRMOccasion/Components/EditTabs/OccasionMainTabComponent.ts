import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'OccasionMainTabComponent',
    moduleId: module.id,
    templateUrl: './OccasionMainTabComponent.html',
})

export class OccasionMainTabComponent extends BaseComponent {

    public EntityPM: OccasionPM;
    public EntityId: string;
    public IsCheckedAllContacts: false;
    public DataContext: OccasionMainTabComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.EntityId = this.EntityPM.Id;
        }        
    }


    AddContactsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Add Contact";
        logWindow.WindowArgs = this.EntityPM;        
        logWindow.Show("./CRMModules/CRMOccasion/Components/AddEdit/AddEditOccasionContactComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {

        });
    }
}
