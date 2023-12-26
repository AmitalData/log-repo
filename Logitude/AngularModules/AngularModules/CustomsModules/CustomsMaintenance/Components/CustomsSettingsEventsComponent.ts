import {Component, OnInit, AfterViewInit, ViewChildren, QueryList} from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';

@Component({
    
    selector: 'CustomsSettingsEventsComponent',
    templateUrl: './CustomsSettingsEventsComponent.html',
    providers: [EntityArgs],
})

export class CustomsSettingsEventsComponent extends BaseComponent implements AfterViewInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public DataContext: any = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        
    }

    SetWindowArgs(args: any) {
        this.entityArgs.EntityPM = args.EntityPM;
        this.entityArgs.ObjectTableName = args.ObjectTableName;
    }

    OkButtonClicked() {
        this.CancelButtonClicked();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ngAfterViewInit() {
        this.LoadEventsTab();
    }

    LoadEventsTab() {
        let locs = this.AllLocations.toArray().filter(f => f.Code == 'EventsLocation');
        let myLocation: LocationDirective = locs.filter(f => f.ItemCode == "1")[0];
        if (myLocation != null) {
            SessionLocator.DynamicLoader.Load("./Common/Components/Events/EventsTabComponent", myLocation.viewContainerRef)
                .then(cmpRef => {

                });
        }

    }

}
