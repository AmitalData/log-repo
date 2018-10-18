declare var window: any;
import {Component, OnInit} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerDocsOutTabComponent.html',
})

export class CustomerDocsOutTabComponent implements OnInit {
    public EntityPM: CustomerPM = null;
    public ObjectTableName = "Customer";
    public DataContext: this;
    public ObjectTableId: string;
    public EntityId: string;
    public EntityReference: string
    public IsVisible = false;

    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {

            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.EntityReference = "";
        }
    }
}