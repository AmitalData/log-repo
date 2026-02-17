import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ParticipantPM} from '../../../../../Common/EntityPMs/ParticipantPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ContactPMService} from '../../../../../Common/Services/StandardPMs/ContactPMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'ParticipantNotifyTabComponent',
    moduleId: module.id,
    templateUrl: './ParticipantNotifyTabComponent.html',

})

export class ParticipantNotifyTabComponent extends BaseComponent {

    public EntityPM: ParticipantPM;
    public DataContext: ParticipantNotifyTabComponent = this;
    public ObjectTableName: string = "Participant";
    constructor(public entityArgs: EntityArgs, private _entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }
    public get FWBNotifyContacts() { return this.EntityPM.FWBNotifyContacts; }
    public set FWBNotifyContacts(value: string) { if (this.EntityPM.FWBNotifyContacts != value) this.EntityPM.FWBNotifyContacts = value; }

    public get FHLNotifyContacts() { return this.EntityPM.FHLNotifyContacts; }
    public set FHLNotifyContacts(value: string) { if (this.EntityPM.FHLNotifyContacts != value) this.EntityPM.FHLNotifyContacts = value; }

    public get FFRNotifyContacts() { return this.EntityPM.FFRNotifyContacts; }
    public set FFRNotifyContacts(value: string) { if (this.EntityPM.FFRNotifyContacts != value) this.EntityPM.FFRNotifyContacts = value; }


    Add(code:string) {
        var windowTitle = "Add Contact";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(p => {
            logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (!AppTool.IsNullOrEmpty(s) && s !="cancel") {
                        var service: ContactPMService = new ContactPMService();
                        service.get(s).subscribe(p => {
                            var email = p.Result.EnglishName;
                            if (code == "FWB")
                                this.FWBNotifyContacts = this.FWBNotifyContacts + ";" + email;
                            else if (code == "FHL")
                                this.FHLNotifyContacts = this.FHLNotifyContacts + ";" + email;
                            else
                                this.FFRNotifyContacts = this.FFRNotifyContacts + ";" + email;

                        });

                    }
                });
            });
        });
    }

}
