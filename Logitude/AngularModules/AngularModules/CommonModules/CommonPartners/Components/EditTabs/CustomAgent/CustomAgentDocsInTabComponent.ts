declare var window: any;
import { Component, OnInit } from '@angular/core';
import { CustomAgentPM } from '../../../../../Common/EntityPMs/CustomAgentPM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './CustomAgentDocsInTabComponent.html',
})

export class CustomAgentDocsInTabComponent implements OnInit {
    public EntityPM: CustomAgentPM;
    public ObjectTableName: string = "CustomAgent";
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
