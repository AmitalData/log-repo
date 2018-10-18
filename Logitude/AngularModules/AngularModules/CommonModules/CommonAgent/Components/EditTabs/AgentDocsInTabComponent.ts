declare var window: any;
import {Component, OnInit} from '@angular/core';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './AgentDocsInTabComponent.html',
})

export class AgentDocsInTabComponent implements OnInit {
    public EntityPM: AgentPM;
    public ObjectTableName: string = "Agent";
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