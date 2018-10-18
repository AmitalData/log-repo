import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';

@Component({
    moduleId: module.id,
    selector: 'SupportTabComponent',
    templateUrl: './SupportTabComponent.html',
})

export class SupportTabComponent extends BaseComponent implements OnInit {
    public DataContext: SupportTabComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    public IsEditingAllowed: boolean = false;
    ngOnInit() {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("SupportEmail", this.ObjectTableName, this.SupportActivated);
    }

    get SupportEmail() { return this.EntityPM.SupportEmail; }
    set SupportEmail(newValue: string) {
        if (this.EntityPM.SupportEmail != newValue) {
            this.EntityPM.SupportEmail = newValue;
        }
    }

    get SupportActivated() { return this.EntityPM.SupportActivated; }
    set SupportActivated(newValue: boolean) {
        if (this.EntityPM.SupportActivated != newValue) {
            this.EntityPM.SupportActivated = newValue;

            this.SetUIProperties();
        }
    }
}