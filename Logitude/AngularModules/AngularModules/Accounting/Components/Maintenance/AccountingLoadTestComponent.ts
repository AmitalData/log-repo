import {Component, OnInit, AfterViewInit, ViewChildren, QueryList} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {AccountingPeriodList} from '../../EntityLists/AccountingPeriodList';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AccountingPeriodListService} from '../../Services/StandardLists/AccountingPeriodListService';
import {AccountingPeriodPMService} from '../../Services/StandardPMs/AccountingPeriodPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {AccountingPeriodPM} from '../../EntityPMs/AccountingPeriodPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';

import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';


@Component({
    moduleId: module.id,
    selector: 'AccountingLoadTestComponent',
    templateUrl: './AccountingLoadTestComponent.html',
    //providers: [EntityArgs],
})

export class AccountingLoadTestComponent extends BaseComponent implements AfterViewInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public DataContext: AccountingLoadTestComponent = this;
    public ObjectTableName: string = "Journal"
    ActionTypeItems: any[];
    EveryMinuteItems: number[];
    IsCreateJournalEvery: boolean
    constructor(private _entityResourceService: EntityResourceService)//, public entityArgs: EntityArgs)
    {
        super();
        this.ActionTypeItems = [];

        this.ActionTypeItems.push({ Id: /*0, Code:*/ "", Name: "" });
        this.ActionTypeItems.push({ Id: /*1, Code: */"CreateVendors", Name: "Create Vendors " });
        this.ActionTypeItems.push({ Id: /*2, Code: */"CreateCustomers", Name: "Create Suppliers " });
        this.ActionTypeItems.push({ Id: /*3, Code: */"CreateJournal", Name: "Create Journal " });
        this.ActionTypeItems.push({ Id: /*4, Code: */"CreateJournalEvery", Name: "Create Journal Every" });
        this.EveryMinuteItems = [1,5, 30, 60, 90, 120];

    }
    ActionItemSelectionChanged(selectControl:any) {
        this.IsCreateJournalEvery=selectControl.value == "CreateJournalEvery";
    }
    EveryMinuteItemSelectionChanged(selectControl: any) {

    }

    SetWindowArgs(args: any) {
        //this.EntityPM = args.EntityPM;
    }
    SendButtonClicked() {
        this.CancelButtonClicked();
    }
    SendandNewButtonClicked() {
        
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    ngAfterViewInit() {
        this.LoadEventsTab();
    }
    Amount: number

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
