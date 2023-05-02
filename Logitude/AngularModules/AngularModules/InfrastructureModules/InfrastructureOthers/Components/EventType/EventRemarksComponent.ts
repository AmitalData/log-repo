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
    public tempPartnerTypesEvent: PartnerTypesEvent[];
    public tempEventRemarks: EventRemarkPM[] = [];
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
        this.partnerTypesEvent=[];

        if (!this.EntityPM.EventRemarks) return;
        this.EventRemarksLists = [];
        this.EntityPM.EventRemarks.forEach((copy) => {
            this.EventRemarksLists.push(copy);
            this.partnerTypesEvent.push(new PartnerTypesEvent(copy,true, "" ,copy.PartnerTypeId,""));
        });

        
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
                this.partnerTypesEvent.push(new PartnerTypesEvent( new EventRemarkPM(),false, this.PartnerTypes[i].Name ,this.PartnerTypes[i].Id,this.PartnerTypes[i].SearchFields));
            }        
        }); 

    }

    CheckboxIsSelectedByDefaultClick(selectedItem: PartnerTypesEvent ,value: any) {
        if (selectedItem == null) return;
        this.EntityPM = this.entityArgs.EntityPM;
    
            selectedItem.EventRemark.Tenant = this.EntityPM.Tenant;
            selectedItem.EventRemark.CreateDate = new Date(Date.now());
            selectedItem.EventRemark.CreatedByUserId = SessionLocator.LoggedUserId;;
            selectedItem.EventRemark.SearchFields =selectedItem.PartnerTypeSearchFields;
            selectedItem.EventRemark.EventTypeId = this.EntityPM.Id;
            selectedItem.EventRemark.PartnerTypeId =selectedItem.PartnerTypeId;
            let index =this.EventRemarksLists.findIndex(x => x.PartnerTypeId === selectedItem.PartnerTypeId && x.EventTypeId ===selectedItem.EventRemark.EventTypeId);
            if(index == -1 || selectedItem.IsChoose != value){
                selectedItem.EventRemark.IsChoose=value;
                this.EventRemarksLists.push(selectedItem.EventRemark);
            }   
      
        this.EntityPM.EventRemarks = this.EventRemarksLists;
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
    }

    ClearAllClicked(event){
        this.EntityPM = this.entityArgs.EntityPM;
        this.IsAllSelected = false;
        var size=this.EventRemarksLists.length;
        this.tempEventRemarks=this.EventRemarksLists;
        this.EventRemarksLists=[];
        for (let i = 0; i < size; i++) 
        {
            this.tempEventRemarks[i].IsChoose=false;
            this.EventRemarksLists.push(this.tempEventRemarks[i]);
            this.partnerTypesEvent[i].IsChoose=false;
        }
        this.EntityPM.EventRemarks = this.EventRemarksLists;
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
    }
    SelectAllClicked(event){
        this.EntityPM = this.entityArgs.EntityPM;
        this.IsAllSelected = event;
        var size=this.partnerTypesEvent.length;
        for (let i = 0; i < size; i++) 
        {
            this.partnerTypesEvent[i].EventRemark.Tenant = this.EntityPM.Tenant;
            this.partnerTypesEvent[i].EventRemark.CreateDate = new Date(Date.now());
            this.partnerTypesEvent[i].EventRemark.CreatedByUserId = SessionLocator.LoggedUserId;;
            this.partnerTypesEvent[i].EventRemark.SearchFields =this.partnerTypesEvent[i].PartnerTypeSearchFields;
            this.partnerTypesEvent[i].EventRemark.EventTypeId = this.EntityPM.Id;
            this.partnerTypesEvent[i].EventRemark.PartnerTypeId =this.partnerTypesEvent[i].PartnerTypeId;
            let index =this.EventRemarksLists.findIndex(x => x.PartnerTypeId === this.partnerTypesEvent[i].PartnerTypeId && x.EventTypeId ===this.partnerTypesEvent[i].EventRemark.EventTypeId);
            if(index == -1 && this.partnerTypesEvent[i].IsChoose != true){
                this.partnerTypesEvent[i].EventRemark.IsChoose=true;
                this.EventRemarksLists.push( this.partnerTypesEvent[i].EventRemark);
                this.partnerTypesEvent[i].IsChoose=true;
            }  
        }
        this.EntityPM.EventRemarks = this.EventRemarksLists;
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = true;
    }

    TextChanged(searchEvent){

        if(searchEvent=="" || searchEvent==null){
            this.partnerTypesEvent = this.tempPartnerTypesEvent;  
        }
        else{
            this.tempPartnerTypesEvent = this.partnerTypesEvent;
            let x =this.partnerTypesEvent.find(x => x.PartnerTypeName.toLocaleLowerCase() === searchEvent.toLocaleLowerCase()); 
            if(x!=null){
                this.partnerTypesEvent=[];
                this.partnerTypesEvent.push(x);
            }
            else{
                this.tempPartnerTypesEvent = this.partnerTypesEvent;  
            }
           
        }

    }
    
}
class PartnerTypesEvent {
    public EventRemark: EventRemarkPM;
    public IsChoose: boolean
    public PartnerTypeName: string
    public PartnerTypeId: string
    public PartnerTypeSearchFields: string
    constructor(EventRemark: EventRemarkPM,IsChoose: boolean , PartnerTypeName:string , PartnerTypeId: string,PartnerTypeSearchFields: string) {
        this.EventRemark= EventRemark;
        this.IsChoose = IsChoose;
        this.PartnerTypeName = PartnerTypeName;
        this.PartnerTypeId = PartnerTypeId;
        this.PartnerTypeSearchFields = PartnerTypeSearchFields;
    } 
}

