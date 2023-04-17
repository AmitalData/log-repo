import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EventTypePM } from '../../../../Infrastructure/EntityPMs/EventTypePM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { PartnerTypeListService } from 'Common/Services/StandardLists/PartnerTypeListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PartnerTypePM } from 'Common/EntityPMs/PartnerTypePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EventRemarkPM } from 'Infrastructure/EntityPMs/EventRemarkPM';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { EventTypePMService } from 'Infrastructure/Services/StandardPMs/EventTypePMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

declare var window: any;

@Component({

    selector: 'EventRemarksComponent',
    templateUrl: './EventRemarksComponent.html',
})

export class EventRemarksComponent extends BaseComponent{
    public EntityPM: EventTypePM;
    private CurrentSession = SessionLocator.SelectedSession;
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    _EventTypePMService : EventTypePMService = new EventTypePMService();
    public  E;
    //public ObjectTableName: string = "PartnerType";
    public DataContext = this;
    public PartnerTypes: PartnerTypePM[]=[];
    
    constructor(public entityArgs: EntityArgs) {
        super();
       
        this._PartnerTypeListService.getAll()
        .subscribe((myResponse: ServiceResponse) =>
        {
            this.CurrentSession.StopBusyIndicator();
            this.EntityPM = myResponse.Result;
            var size=myResponse.Result.length;
            for (let i = 0; i < size; i++) {
                this.PartnerTypes[i]=this.EntityPM[i];
            }
            
        });
        this.E = entityArgs.EntityPM; 
    }
    CheckboxIsSelectedByDefaultClick(selectedItem: EventTypePM, value: any) {
        if (selectedItem == null) return;
       
        this._EventTypePMService.get( this.E.id)
        .subscribe((myResponse: ServiceResponse) =>
        {
            this.EntityPM = myResponse.Result;
            this.EntityPM.EventRemarks.push();
            this.EntityPM.MarkAsDirty();
        });
        this._EventTypePMService.update(this.EntityPM)
            .subscribe((myResponse: ServiceResponse) =>
            {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        if (myResponse.Result != undefined && myResponse.Result != null) {
                            this.CurrentSession.CloseCurrentWindow();
                            this.EntityPM.EnglishName='x';
                            this.EntityPM.MarkAsDirty();

                        }
                        else {
                            const messageWindow = new MessageWindow();
                            messageWindow.Show("Error happened while updating totals");
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    }
                }
            });
        this.E.id;
        this.E.code;
        this.E.tenant;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        
       // selectedItem.EventRemarks[0].IsChoose = value;
    }

    // get Name() { return this.EntityPM.Name; }
    // set Name(value: string) {
    //     if (this.EntityPM.Name != value) {
    //         this.EntityPM.Name = value;
    //     }
    // }

}
