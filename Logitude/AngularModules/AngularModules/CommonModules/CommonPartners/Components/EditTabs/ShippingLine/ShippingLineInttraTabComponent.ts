import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShippingLinePM} from '../../../../../Common/EntityPMs/ShippingLinePM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ShippingLineInttraTabComponent.html',
})

export class ShippingLineInttraTabComponent extends BaseComponent {
    public EntityPM: ShippingLinePM;
    public ObjectTableName: string = "ShippingLine";
    public DataContext = this;  
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();
    }

    private SetUIProperties() {
        var isFieldEnabled: boolean = false;

        if (SessionLocator.Tenant == 0) {
            isFieldEnabled = true;
        }

        this.UIProperties.SetEnabled("IsINTTRARegistered", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("INTTRARegistrationNotes", this.ObjectTableName, isFieldEnabled);
    }

    public get IsINTTRARegistered() { return this.EntityPM.IsINTTRARegistered; }
    public set IsINTTRARegistered(value: boolean) {
        if (this.EntityPM.IsINTTRARegistered != value) {
            this.EntityPM.IsINTTRARegistered = value;
        }
    }

    public get INTTRARegistrationNotes() { return this.EntityPM.INTTRARegistrationNotes; }
    public set INTTRARegistrationNotes(value: string) {
        if (this.EntityPM.INTTRARegistrationNotes != value) {
            this.EntityPM.INTTRARegistrationNotes = value;
        }
    }
}