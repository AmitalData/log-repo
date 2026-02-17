import { Component, OnInit } from '@angular/core';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './OccasionDocsInTabComponent.html',
})

export class OccasionDocsInTabComponent implements OnInit {
    public EntityPM: OccasionPM;
    public ObjectTableName: string = "Occasion";
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
