declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';

@Component({
    selector: 'TicketDocsInTabComponent',
    moduleId: module.id,
    templateUrl: './TicketDocsInTabComponent.html',
})

export class TicketDocsInTabComponent implements OnInit {
    public EntityPM: TicketPM = null;
    public ObjectTableName = "Ticket";
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