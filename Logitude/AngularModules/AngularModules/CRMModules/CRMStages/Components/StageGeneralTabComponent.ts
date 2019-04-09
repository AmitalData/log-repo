
import {Component} from '@angular/core';
import {StagePM} from '../../../CRM/EntityPMs/StagePM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'StageGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './StageGeneralTabComponent.html',
})

export class StageGeneralTabComponent extends BaseComponent {
    public EntityPM: StagePM;
    public ObjectTableName: string = "Stage";
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;

        if (this.entityArgs.EntityPM != null)
            this.EntityPM = this.entityArgs.EntityPM;
        else if (this.CurrentSession.CurrentWindow.WindowArgs.EntityPM != null)
            this.EntityPM = this.CurrentSession.CurrentWindow.WindowArgs.EntityPM;
        else
            this.EntityPM = new StagePM();
        this.SetUIProperties();
    }

    private SetUIProperties() {
        if (this.EntityPM.Code == "CWN" || this.EntityPM.Code == "CLS") {
            this.UIProperties.SetEnabled("Probability", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("Probability", this.ObjectTableName, true);
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) { this.EntityPM.Name = value;  }

    get Probability() { return this.EntityPM.Probability; }
    set Probability(value: number) { this.EntityPM.Probability = value; }

    get MaxDays() { return this.EntityPM.MaxDays; }
    set MaxDays(value: number){ this.EntityPM.MaxDays = value; }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) { this.EntityPM.InActive = value;  }
}
