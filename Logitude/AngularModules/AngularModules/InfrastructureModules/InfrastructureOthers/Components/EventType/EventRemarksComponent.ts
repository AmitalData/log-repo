import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EventTypePM } from '../../../../Infrastructure/EntityPMs/EventTypePM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { PartnerTypeListService } from 'Common/Services/StandardLists/PartnerTypeListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { PartnerTypePM } from 'Common/EntityPMs/PartnerTypePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';

declare var window: any;

@Component({

    selector: 'EventRemarksComponent',
    templateUrl: './EventRemarksComponent.html',
})

export class EventRemarksComponent extends BaseComponent{
    private CurrentSession = SessionLocator.SelectedSession;
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    public  EntityPM:  PartnerTypePM[];
    public ObjectTableName: string = "PartnerType";
    public DataContext = this;
    public Names:string[]=[];
    
    constructor() {
        super();
        this._PartnerTypeListService.getAll()
        .subscribe((myResponse: ServiceResponse) =>
        {
            this.CurrentSession.StopBusyIndicator();
            this.EntityPM = myResponse.Result;
            var size=myResponse.Result.length;
            for (let i = 0; i < size; i++) {
                this.Names[i]=this.EntityPM[i].Name;
            }
            
        });
        
    }

    // get Name() { return this.EntityPM.Name; }
    // set Name(value: string) {
    //     if (this.EntityPM.Name != value) {
    //         this.EntityPM.Name = value;
    //     }
    // }

}
