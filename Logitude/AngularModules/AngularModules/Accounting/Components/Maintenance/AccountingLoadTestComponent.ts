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
import { JournalOpService } from '../../Services/Others/JournalOpService';

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
    IsCreateJournalEvery: boolean;
    IsCreateJournal: boolean;
    _JournalOpService: JournalOpService = new JournalOpService();
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService)//, public entityArgs: EntityArgs)
    {
        super();
        this.ActionTypeItems = [];

        this.ActionTypeItems.push({ Id: /*0, Code:*/ "", Name: "" });
        this.ActionTypeItems.push({ Id: /*1, Code: */"CreateVendors", Name: "Create Vendors " });
        this.ActionTypeItems.push({ Id: /*2, Code: */"CreateCustomers", Name: "Create Customers" });
        this.ActionTypeItems.push({ Id: /*3, Code: */"CreateJournal", Name: "Create Journal " });
        this.ActionTypeItems.push({ Id: /*4, Code: */"CreateJournalEvery", Name: "Create Journal Every" });
        this.EveryMinuteItems = [1, 5, 30, 60, 90, 120];
        this.clearScreen();
    }
    clearScreen() {
        this.Year = (new Date().getFullYear()) - 1;
        this._SelectedIndexActionTypeItem = 1;
        this._SelectedIndexEveryMinuteItem = 1;
        this.Amount = 10;
    }
    _SelectedIndexActionTypeItem = 1;
    _SelectedIndexEveryMinuteItem = 1;
    _SelectedActionTypeValue: string = "";
    ActionItemSelectionChanged(selectControl: any) {
        this._SelectedActionTypeValue = selectControl.value;
        this.IsCreateJournal = (this._SelectedActionTypeValue == "CreateJournal");
        this.IsCreateJournalEvery = (this._SelectedActionTypeValue == "CreateJournalEvery");
        if (this.IsCreateJournalEvery) {
            this._SelectedIndexEveryMinuteItem = 3;
        }
    }
    _SelectedEveryMinuteValue: number = 0;
    EveryMinuteItemSelectionChanged(selectControl: any) {
        this._SelectedEveryMinuteValue = selectControl.value;
    }
    private year: number;
    get Year() { return this.year; }
    set Year(value: number) {
        if (this.year != value) {
            this.year = value;

        }
    }
    FillErrors() {
        this.ValidationErrorsList = [];
        if (this.Amount<1) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Amount must be greater then zero ");
        }
        if (AppTool.IsNullOrEmpty(this._SelectedActionTypeValue)) {
            this.ValidationErrorsList.push("Selected Action Type is nothing");
        }
        if (this._SelectedActionTypeValue == "CreateJournalEvery" || "CreateJournal" == this._SelectedActionTypeValue) {


            if (AppTool.IsNullOrEmpty(this.year)) {
                //this.Year = new Date().getFullYear();
                this.ValidationErrorsList.push("Year Field is Required");
            }
            if (this.year > (new Date().getFullYear())) {

                this.ValidationErrorsList.push("Future Year!");
            } else if (this.year < 1900) {

            }
        }
        if (this._SelectedActionTypeValue == "CreateJournalEvery") {
            if (this._SelectedEveryMinuteValue < 1) {
                this.ValidationErrorsList.push("Selected Every Minute Value must be greater then zero ");
            }
        }
    }

    SetWindowArgs(args: any) {
        //this.EntityPM = args.EntityPM;
    }
    _closeWin: boolean = true;
    SendButtonClicked() {
        this._closeWin = true;
        this.JustSend();
        
    }
    SendandNewButtonClicked() {
        this._closeWin = false;
        this.JustSend();
    }
    JustSend()
    {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        this._JournalOpService
            .GetTaskLoadTest(SessionLocator.Tenant, this._SelectedActionTypeValue, this.Amount, this._SelectedEveryMinuteValue ,this.Year)
            .subscribe(
                (res: ServiceResponse) => {
                    if (res.HasError) {
                        this.ValidationErrorsList = res.ErrorsArray;

                    } else {
                        //this._JournalPM = res.Result;
                        if (this._closeWin) {
                            this.CancelButtonClicked();
                        } else {
                            this.clearScreen();
                        }
                    }
                },
                (err) => {
                    alert(err);
                },
                () => {
                    this.CurrentSession.StopBusyIndicator();
                }
            );   
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ngAfterViewInit() {
        
    }
    Amount: number

   

}
