declare var window: any;
import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';

import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';


@Component({
    

    selector: 'DWLogSearchAddFieldsComponent',
    templateUrl: './DWLogSearchAddFieldsComponent.html',
})

export class DWLogSearchAddFieldsComponent implements OnInit {
    //@Output() Toevent = new EventEmitter();
    public rowData: any;
    public fieldName: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef, private _entityListService: EntityListService) {


    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;

    }

    ngOnInit() {

    }



    AddField(item: any) {

        if (item) {

            this.Destroyed();
            this.CurrentSession.SessionEvent.emit({ Item: item, ComponentName: "DWLogSearchAddFieldsComponent", IsFirstRequest:true });
        }
    }

    ChooseField(item: any) {
        if (!item) {
            return;
        }
        this.Destroyed();
        this.CurrentSession.SessionEvent.emit({ Item: item, ComponentName: "DWLogSearchAddFieldsComponent", IsFirstRequest: true, ChooseOne: true });
    }

    Destroyed() {
        var isDestroyed: boolean = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    }


}



