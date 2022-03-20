import { Component} from '@angular/core';
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    templateUrl: './TariffsTabComponent.html',
})

export class TariffsTabComponent extends BaseComponent {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public DataContext: TariffsTabComponent = this;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
    }

    public get ImportLocalCustomerGroupId() { return this.EntityPM.ImportLocalCustomerGroupId; }
    public set ImportLocalCustomerGroupId(value: string) { this.EntityPM.ImportLocalCustomerGroupId = value; }

    public get ExportLocalCustomerGroupId() { return this.EntityPM.ExportLocalCustomerGroupId; }
    public set ExportLocalCustomerGroupId(value: string) { this.EntityPM.ExportLocalCustomerGroupId = value; }
}
