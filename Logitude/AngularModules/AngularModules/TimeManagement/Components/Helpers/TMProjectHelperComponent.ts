import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TMProjectPM} from '../../EntityPMs/TMProjectPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
@Component({
    moduleId: module.id,
    templateUrl: './TMProjectHelperComponent.html',
})

export class TMProjectHelperComponent  {
    public EntityPM: TMProjectPM;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;              
    }

    NewInnerProject() {
        var window: LogitudeWindow = new LogitudeWindow();
        window.Title = "New Project";
        window.WindowArgs = { EntityArgs: this.EntityPM }
        window.Show('./TimeManagement/Components/NewEntity/NewProjectComponent');
    }


    ConnectParentProject() {
        var window: LogitudeWindow = new LogitudeWindow();
        window.Title = "Connect to Parent";
        window.WindowArgs = { EntityArgs: this.EntityPM }
        window.Height = 170;
        window.Width = 500;
        window.Show('./TimeManagement/Components/Connections/ConnectToParentComponent');
    }

   }