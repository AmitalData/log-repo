import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EventTypePM } from '../../../../Infrastructure/EntityPMs/EventTypePM';
import { PartnerTypeListService } from 'Common/Services/StandardLists/PartnerTypeListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PartnerTypePM } from 'Common/EntityPMs/PartnerTypePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EventRemarkPM } from 'Infrastructure/EntityPMs/EventRemarkPM';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';

@Component({

    selector: 'EventRemarksComponent',
    templateUrl: './EventRemarksComponent.html',
})

export class EventRemarksComponent extends BaseComponent {
    public EntityPM: EventTypePM;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    public EventRemarksList: EventRemarkPM[] = [];
    public partnerTypesEvent: PartnerTypesEvent[];
    public tempPartnerTypesEvent: PartnerTypesEvent[] = [];
    public tempEventRemarks: EventRemarkPM[] = [];
    public tempEventRemarks1: EventRemarkPM[] = [];
    public eventRemarkPM: EventRemarkPM = new EventRemarkPM();
    public ObjectTableName: string = "EventRemark";
    public DataContext = this;
    public PartnerTypes: PartnerTypePM[] = [];
    ValidationErrorsList: string[];
    public newEventReamrk = new EventRemarkPM();

    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        this._entityResourceService.getEntityResourceByTableName("EventType", 0).subscribe((response: any) => {
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) { this.Run(); }
        });
        this.partnerTypesEvent.forEach(val => this.tempPartnerTypesEvent.push(val));
    }

    Run() {
        this.partnerTypesEvent = [];

        if (!this.EntityPM.EventRemarks) return;
        this.EventRemarksList = [];
        this.EntityPM.EventRemarks.forEach((copy) => {
            this.EventRemarksList.push(copy);
        });

        this._PartnerTypeListService.getAll()
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.EntityPM = myResponse.Result;
                var size = myResponse.Result.length;
                for (let i = 0; i < size; i++) {
                    this.PartnerTypes[i] = this.EntityPM[i];
                    var x = this.EventRemarksList.find(x => x.PartnerTypeId === this.PartnerTypes[i].Id);
                    if (x != null) {
                        this.partnerTypesEvent.push(new PartnerTypesEvent(new EventRemarkPM(), true, true,this.PartnerTypes[i].Name, this.PartnerTypes[i].Id, this.PartnerTypes[i].SearchFields));

                    } else {
                        this.partnerTypesEvent.push(new PartnerTypesEvent(new EventRemarkPM(), false, true,this.PartnerTypes[i].Name, this.PartnerTypes[i].Id, this.PartnerTypes[i].SearchFields));

                    }
                }
                this.partnerTypesEvent.forEach(val => this.tempPartnerTypesEvent.push(val));
            });

    }

    CheckboxIsSelectedByDefaultClick(selectedItem: PartnerTypesEvent, value: any) {
        if (selectedItem == null) return;
        this.EntityPM = this.entityArgs.EntityPM;
        let index = this.partnerTypesEvent.findIndex(x => x.PartnerTypeId === selectedItem.PartnerTypeId);
        if (index > -1) {
            this.partnerTypesEvent[index].IsChoose = value;
        }
        this.FullEventRemarksList(this.partnerTypesEvent);
    }

    ClearAllClicked() {
        this.EntityPM = this.entityArgs.EntityPM;
        for (let i = 0; i < this.partnerTypesEvent.length; i++) {
            if(this.partnerTypesEvent[i].IsVisible == true)
                this.partnerTypesEvent[i].IsChoose = false;
        }
        this.FullEventRemarksList(this.partnerTypesEvent);
    }

    SelectAllClicked() {
        this.EntityPM = this.entityArgs.EntityPM;
        var size = this.partnerTypesEvent.length;
        for (let i = 0; i < size; i++) {
            if(this.partnerTypesEvent[i].IsVisible == true)
                this.partnerTypesEvent[i].IsChoose = true;
        }
        this.FullEventRemarksList(this.partnerTypesEvent);
    }

    FullEventRemarksList(partnerTypesEvent) {
        //this.EventRemarksList = [];

        for (let i = 0; i < partnerTypesEvent.length; i++) 
        {
            if(this.partnerTypesEvent[i].IsVisible == true)
            {
                partnerTypesEvent[i].EventRemark.Tenant = this.EntityPM.Tenant;
                partnerTypesEvent[i].EventRemark.CreateDate = new Date(Date.now());
                partnerTypesEvent[i].EventRemark.CreatedByUserId = SessionLocator.LoggedUserId;;
                partnerTypesEvent[i].EventRemark.SearchFields = partnerTypesEvent[i].PartnerTypeSearchFields;
                partnerTypesEvent[i].EventRemark.EventTypeId = this.EntityPM.Id;
                partnerTypesEvent[i].EventRemark.PartnerTypeId = partnerTypesEvent[i].PartnerTypeId;
                if (partnerTypesEvent[i].IsChoose == true) {
                    this.FindInEventRemarksList(partnerTypesEvent[i], true);
                }
                else {
                    this.FindInEventRemarksList(partnerTypesEvent[i], false);
                }
            }  
        }     
        this.EntityPM.EventRemarks = [];
        this.EventRemarksList.forEach( val=> this.EntityPM.EventRemarks.push(val) );
        if(this.CurrentSession.CurrentEditComponent!=null)
        {
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
            this.CurrentSession.CurrentEditComponent.EntityPM.OldEntityPM=null;
        }
    }

    FindInEventRemarksList(partnerTypesEvent, value) {
        let index = this.EventRemarksList.findIndex(x => x.PartnerTypeId === partnerTypesEvent.PartnerTypeId);
        if (index == -1) {
            this.EventRemarksList.push(partnerTypesEvent.EventRemark);
        }
        else {
            this.EventRemarksList[index].IsChoose = value;
        }
        partnerTypesEvent.EventRemark.IsChoose = value;
    }

    TextChanged(searchEvent) {      
        if (searchEvent == "" || searchEvent == null) {
            var size = this.partnerTypesEvent.length;
            this.tempPartnerTypesEvent = [];
            for (let i = 0; i < size; i++)
            {
                this.partnerTypesEvent[i].IsVisible=true;
                this.tempPartnerTypesEvent.push(this.partnerTypesEvent[i]);
            }
        }
        else if (searchEvent != "" || searchEvent != null) {
            var size = this.partnerTypesEvent.length;
            this.tempPartnerTypesEvent = [];
            for (let i = 0; i < size; i++)
                {
                if (this.partnerTypesEvent[i].PartnerTypeName.toUpperCase().match(searchEvent.toUpperCase())) {
                    this.partnerTypesEvent[i].IsVisible=true;
                    this.tempPartnerTypesEvent.push(this.partnerTypesEvent[i]);
                }
                else
                {
                    this.partnerTypesEvent[i].IsVisible=false; 
                }
            }
        }
    }
}

class PartnerTypesEvent {
    public EventRemark: EventRemarkPM;
    public IsChoose: boolean
    public IsVisible: boolean
    public PartnerTypeName: string
    public PartnerTypeId: string
    public PartnerTypeSearchFields: string
    constructor(EventRemark: EventRemarkPM, IsChoose: boolean, IsVisible: boolean,PartnerTypeName: string, PartnerTypeId: string, PartnerTypeSearchFields: string) {
        this.EventRemark = EventRemark;
        this.IsChoose = IsChoose;
        this.IsVisible = IsVisible;
        this.PartnerTypeName = PartnerTypeName;
        this.PartnerTypeId = PartnerTypeId;
        this.PartnerTypeSearchFields = PartnerTypeSearchFields;
    }
}

