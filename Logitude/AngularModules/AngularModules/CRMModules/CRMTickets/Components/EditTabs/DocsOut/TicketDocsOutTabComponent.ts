import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';

@Component({
    selector: 'TicketDocsOutTabComponent',
    moduleId: module.id,
    templateUrl: './TicketDocsOutTabComponent.html',
})

export class TicketDocsOutTabComponent {
    public EntityPM: TicketPM = null;
    public ObjectTableName = "Ticket";
    public DataContext: this;
    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
    }
}