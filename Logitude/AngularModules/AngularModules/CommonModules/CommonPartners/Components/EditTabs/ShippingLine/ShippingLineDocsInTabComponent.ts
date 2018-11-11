declare var window: any;
import { Component, OnInit } from '@angular/core';
import { ShippingLinePM } from '../../../../../Common/EntityPMs/ShippingLinePM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './ShippingLineDocsInTabComponent.html',
})

export class ShippingLineDocsInTabComponent implements OnInit {
    public EntityPM: ShippingLinePM;
    public ObjectTableName: string = "ShippingLine";
    public DataContext: this;
    public ObjectTableId: string;
    public EntityId: string;
    constructor(private entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
        }
    }
}
