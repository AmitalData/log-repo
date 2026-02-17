
declare var window: any;
import {Component, OnInit, EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {EventTypeExtendedPMService} from '../../Infrastructure/Services/ExtendedPMs/EventTypeExtendedPMService';
import {EventTypePM} from '../../Infrastructure/EntityPMs/EventTypePM';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {EventPermissiosViewModel} from './ViewModel/EventPermissiosViewModel';


@Component({
    moduleId: module.id,
    selector: 'SharedLogisticsEventPermissios',
    templateUrl: './SharedLogisticsEventPermissiosComponent.html',
    inputs: ['OnCloseWindowEvent'],
    providers: [EventTypeExtendedPMService],
})
export class SharedLogisticsEventPermissiosComponent implements OnInit {


     myTenantZeroList: EventTypePM[];
     myTenantList: EventTypePM[];
     EventPermissiosLists: EventPermissiosViewModel[];
     OnCloseWindowEvent = new EventEmitter();
     ObjectTableId: string;
    FullComponentsVisibility: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _eventTypeExtendedPMService:EventTypeExtendedPMService) {
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit(

    ) {

        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();
           
        });
     
        this.Run();



    }


    Run() {

     
        this.ObjectTableId= window.ObjectTables.filter(d=> d.Name == "Shipment")[0].Id;
        this.LoadTenantZeroDate();

    }
    LoadTenantZeroDate() {

        this.myTenantZeroList = [];

        this._eventTypeExtendedPMService.GetEventTypesByObjectTable(this.ObjectTableId,0).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantZeroList = pmResponse.Result;

                this.LoadTenantData();
            }
            else this.CurrentSession.StopBusyIndicator();
        });


    }


    LoadTenantData() {
        this.myTenantList = [];

        this._eventTypeExtendedPMService.GetEventTypesByObjectTable(this.ObjectTableId, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantList = pmResponse.Result;

                this.BuildData();
            }

             this.CurrentSession.StopBusyIndicator();
        });

    }




    mySearchText: string;

    BuildData() {
        var myList: EventTypePM[] = [];
        this.EventPermissiosLists = [];
        if (!this.mySearchText) {
            myList = this.myTenantList;
        }
        else {

            myList = this.myTenantList.filter(d=> (d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.LocalName && d.LocalName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }

        myList =  this.SortItemSource(myList);
        myList.forEach((item) => {
            var tenantZeroItem = this.myTenantZeroList.filter(t => t.Code == item.Code && t.IsSharedLogisticsEnabled)[0];
            if (tenantZeroItem != null) {
                this.EventPermissiosLists.push(new EventPermissiosViewModel(tenantZeroItem, item));
            }
            else {
                if (item.AddedManually) {
                    this.EventPermissiosLists.push(new EventPermissiosViewModel(null, item));
                }
            }


        });
    

    }

    SortItemSource(items: any) {

        items.sort((a, b) => {
            if (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) {
                return -1;
            }
            else if (a.EnglishName.toLowerCase() > b.EnglishName.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });
        return items;
    }


    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }



    SaveButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myTenantList = [];
        this.EventPermissiosLists.forEach((item) => {

            if (item.entityPM.IsDirty) {
                this.myTenantList.push(item.entityPM);
            }
        });

        if (this.myTenantList.length > 0) {

            this._eventTypeExtendedPMService.update(this.myTenantList).subscribe(res => {
                this.CloseButtonClicked();
            });
        }
        else {
            this.CloseButtonClicked();
        }


    }
    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
    }

    SetWindowArgs(args: any) {
        this.FullComponentsVisibility = true;
    }




}
