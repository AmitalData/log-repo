import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CustomerTeamPM } from '../../../../Common/EntityPMs/CustomerTeamPM';

@Component({
    selector: 'CustomerTeamGeneralTabComponent',
    templateUrl: './CustomerTeamGeneralTabComponent.html',
})

export class CustomerTeamGeneralTabComponent extends BaseComponent implements OnInit {
    public EntityPM: CustomerTeamPM;
    public DataContext: CustomerTeamGeneralTabComponent = this;
    public ObjectTableName: string = "CustomerTeam";

    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) {
        if (this.EntityPM.LocalName != value)
            this.EntityPM.LocalName = value;
    }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) {
        if (this.EntityPM.InActive != value)
            this.EntityPM.InActive = value;
    }
}
