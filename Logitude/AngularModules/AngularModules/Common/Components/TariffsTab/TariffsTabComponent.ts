import { Component} from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'TariffsTabComponent',
    templateUrl: './TariffsTabComponent.html',
})

export class TariffsTabComponent extends BaseComponent{
    public EntityPM: any = null;
    public ObjectTableName: string;
    public TabHeaderTextCode: string;
    public DataContext: TariffsTabComponent = this;
    constructor(entityArgs: EntityArgs) {
        super();

        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.Tariffs";
       
    }

    public get ImportLocalCustomerGroupId() {
        return this.EntityPM.ImportLocalCustomerGroupId;
    }
    public set ImportLocalCustomerGroupId(value: string) {
        if (this.EntityPM.ImportLocalCustomerGroupId != value) {
            this.EntityPM.ImportLocalCustomerGroupId = value;
        }
    }

    public get ExportLocalCustomerGroupId() {
        return this.EntityPM.ExportLocalCustomerGroupId;
    }
    public set ExportLocalCustomerGroupId(value: string) {
        if (this.EntityPM.ExportLocalCustomerGroupId != value) {
            this.EntityPM.ExportLocalCustomerGroupId = value;
        }
    }
}
