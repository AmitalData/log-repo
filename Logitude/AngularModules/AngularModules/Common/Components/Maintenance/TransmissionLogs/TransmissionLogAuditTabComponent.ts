declare var System: any;
declare var window: any;
import {Component, ChangeDetectorRef, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeMessagesTransmissionLogPM } from '../../../EntityPMs/LogitudeMessagesTransmissionLogPM';

import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
@Component({
    selector: 'TransmissionLogAuditTabComponent',
    moduleId: module.id,
    templateUrl: './TransmissionLogAuditTabComponent.html',

})

export class TransmissionLogAuditTabComponent implements OnInit {
    public EntityPM: LogitudeMessagesTransmissionLogPM = null;

    EntityId: string;
    public ObjectTableName = "LogitudeMessagesTransmissionLog";
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
                if (tabCode == "MTAD") {
                    this.CurrentSession.FireEvent(this.ObjectTableName);
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }








}
