declare var System: any;
declare var window: any;
import {Component, OnInit} from '@angular/core';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './OpportunityDocsInTabComponent.html',
})

export class OpportunityDocsInTabComponent implements OnInit {
    public EntityPM: OpportunityPM;
    public ObjectTableName: string = "Opportunity";
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