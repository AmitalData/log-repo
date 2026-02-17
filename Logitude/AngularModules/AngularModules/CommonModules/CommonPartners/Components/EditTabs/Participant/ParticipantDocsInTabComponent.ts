declare var window: any;
import { Component, OnInit } from '@angular/core';
import { ParticipantPM } from '../../../../../Common/EntityPMs/ParticipantPM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './ParticipantDocsInTabComponent.html',
})

export class ParticipantDocsInTabComponent implements OnInit {
    public EntityPM: ParticipantPM;
    public ObjectTableName: string = "Participant";
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
