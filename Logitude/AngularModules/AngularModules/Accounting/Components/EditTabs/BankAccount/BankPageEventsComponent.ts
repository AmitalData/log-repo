import {Component, OnInit, AfterViewInit, ViewChildren, QueryList} from '@angular/core';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslationPipe } from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { AccountingPeriodList } from '../../../EntityLists/AccountingPeriodList';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AccountingPeriodListService} from '../../../Services/StandardLists/AccountingPeriodListService';
import {AccountingPeriodPMService} from '../../../Services/StandardPMs/AccountingPeriodPMService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import {AccountingPeriodPM} from '../../../EntityPMs/AccountingPeriodPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';

import { AppTool } from '../../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';


@Component({
    moduleId: module.id,
    selector: 'BankPageEventsComponent',
    templateUrl: './BankPageEventsComponent.html',
    providers: [EntityArgs],
})

export class BankPageEventsComponent extends BaseComponent implements AfterViewInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public DataContext: any = this;
    public ObjectTableName: string = "ReconcileExternalPage"
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        //this.entityArgs = new EntityArgs();
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        //this.entityArgs = args;
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
