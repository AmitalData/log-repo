import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../EntityPMs/TicketPM';
import {TicketStageList} from '../../EntityLists/TicketStageList';
import {TicketStageListService} from '../../Services/StandardLists/TicketStageListService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {TicketValidator} from '../../Validators/TicketValidator';
import {TicketStagesArgs, TicketClosureArgs} from '../../Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {OpportunityPM} from '../../EntityPMs/OpportunityPM';

@Component({
    moduleId: module.id,
    templateUrl: "OpportunityHelperComponent.html",
})

export class OpportunityHelperComponent {
    public EntityPM: OpportunityPM;
    public StagesList: TicketStagesArgs[] = [];
    public ObjectTableName = "Opportunity";

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;        
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }
}