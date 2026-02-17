declare var System: any;
declare var window: any;
import {Component, ChangeDetectorRef, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';

import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';

import {AppTool} from '../../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
@Component({
    selector: 'TicketAuditTabComponent',
    moduleId: module.id,
    templateUrl: './TicketAuditTabComponent.html',

})

export class TicketAuditTabComponent implements OnInit {
    public EntityPM: TicketPM = null;

    EntityId: string;
    public ObjectTableName = "Ticket";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private cd: ChangeDetectorRef) {

     
    }
    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.Listen();
    }




    private TabSelectedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "TIAU") {
                    this.CurrentSession.FireEvent(this.ObjectTableName);
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }








}
