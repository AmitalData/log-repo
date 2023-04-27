import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EventTypePM } from '../../../../Infrastructure/EntityPMs/EventTypePM';
import { PartnerTypeListService } from 'Common/Services/StandardLists/PartnerTypeListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PartnerTypePM } from 'Common/EntityPMs/PartnerTypePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EventRemarkPM } from 'Infrastructure/EntityPMs/EventRemarkPM';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { BatchServicesComponent } from 'InfrastructureModules/InfrastructureBatchService/Components/BatchService/BatchServicesComponent';

@Component({

    selector: 'EventRemarksComponent',
    templateUrl: './EventRemarksComponent.html',
})

export class EventRemarksComponent extends BaseComponent{
    public EntityPM: EventTypePM;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    public EventRemarksLists: EventRemarkPM[] = [];
    public partnerTypesEvent: PartnerTypesEvent[];
    public eventRemarkPM : EventRemarkPM=new EventRemarkPM();
    public ObjectTableName: string = "EventRemark";
    public DataContext = this;
    public PartnerTypes: PartnerTypePM[]=[];
    ValidationErrorsList: string[];
    IsAllSelected: any;
    public newEventReamrk = new EventRemarkPM();
    
    constructor(public entityArgs: EntityArgs,private CD: ChangeDetectorRef) {
        super();
        this._entityResourceService.getEntityResourceByTableName("EventRemark", 0).subscribe((response:any) => {
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) { this.Run(); }
        });

           
    }
    
    Run() {
        this.partnerTypesEvent=[]
        this._PartnerTypeListService.getAll()
        .subscribe((myResponse: ServiceResponse) =>
        {
            this.CurrentSession.StopBusyIndicator();
            this.EntityPM = myResponse.Result;
            var size=myResponse.Result.length;
            for (let i = 0; i < size; i++) {
                this.PartnerTypes[i]=this.EntityPM[i];
                

                let indexToUpdate = this.partnerTypesEvent.findIndex(x => x.PartnerTypeId === this.PartnerTypes[i].Id);
                var x=this.partnerTypesEvent.find(x => x.PartnerTypeId === this.PartnerTypes[i].Id);
                if(x !=null){
                    x.IsChoose=true;
                    x.PartnerTypeName=this.PartnerTypes[i].Name;
                    x.PartnerTypeSearchFields=this.PartnerTypes[i].SearchFields;
                    this.partnerTypesEvent[indexToUpdate] = x;
                }
                else 
                this.partnerTypesEvent.push(new PartnerTypesEvent( false, this.PartnerTypes[i].Name ,this.PartnerTypes[i].Id,this.PartnerTypes[i].SearchFields));
            }        
        }); 

        if (!this.EntityPM.EventRemarks) return;
        this.EventRemarksLists = [];
        this.EntityPM.EventRemarks.forEach((copy) => {
            this.EventRemarksLists.push(copy);
            this.partnerTypesEvent.push(new PartnerTypesEvent( true, "" ,copy.PartnerTypeId,""));
        });
    }

    CheckboxIsSelectedByDefaultClick(selectedItem: PartnerTypesEvent ,value: any) {
        if (selectedItem == null) return;
        this.EntityPM = this.entityArgs.EntityPM;
        this.eventRemarkPM.Tenant = this.EntityPM.Tenant;
        this.eventRemarkPM.CreateDate = new Date(Date.now());
        this.eventRemarkPM.CreatedByUserId = SessionLocator.LoggedUserId;;
        this.eventRemarkPM.UpdateDate =  new Date(Date.now());
        this.eventRemarkPM.UpdatedByUserId = SessionLocator.LoggedUserId;;
        this.eventRemarkPM.SearchFields =selectedItem.PartnerTypeSearchFields;
        this.eventRemarkPM.EventTypeId = this.EntityPM.Id;
        this.eventRemarkPM.PartnerTypeId =selectedItem.PartnerTypeId;
        this.eventRemarkPM.IsChoose=value;
        this.EventRemarksLists.push(this.eventRemarkPM);
        this.EntityPM.EventRemarks = this.EventRemarksLists;
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
    }

    ClearAllClicked(event){
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
        this. SelectAllClicked(event);
        this.IsAllSelected = false;
    }
    SelectAllClicked(event){
        this.IsAllSelected = event;
    }
    
}
class PartnerTypesEvent {
    public IsChoose: boolean
    public PartnerTypeName: string
    public PartnerTypeId: string
    public PartnerTypeSearchFields: string
    constructor(IsChoose: boolean , PartnerTypeName:string , PartnerTypeId: string,PartnerTypeSearchFields: string) {
        this.IsChoose = IsChoose;
        this.PartnerTypeName = PartnerTypeName;
        this.PartnerTypeId = PartnerTypeId;
        this.PartnerTypeSearchFields = PartnerTypeSearchFields;
    } 
}

