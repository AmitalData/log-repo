import {Component, OnInit, AfterViewInit} from '@angular/core';
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
import {AccountingPeriodExtendedListService} from '../../Services/ExtendedLists/AccountingPeriodExtendedListService';
import {AccountingPeriodExtendedPMService} from '../../Services/ExtendedPMs/AccountingPeriodExtendedPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {AccountingPeriodPM} from '../../EntityPMs/AccountingPeriodPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';


@Component({
    
    selector: 'AccountingPeriodsComponent',
    templateUrl: './AccountingPeriodsComponent.html',
    providers: [EntityArgs],
})

export class AccountingPeriodsComponent extends BaseComponent {
    public DataContext: AccountingPeriodsComponent = this;
    public ObjectTableName: string = "AccountingPeriod";
    accountingPeriodListService: AccountingPeriodListService;
    accountingPeriodPMService: AccountingPeriodPMService;
    _AccountingPeriodExtendedListService: AccountingPeriodExtendedListService;
    _AccountingPeriodExtendedPMService: AccountingPeriodExtendedPMService;
    public ValidationErrorsList: string[];
    PeriodsList: AccountingPeriodList[];
    ShowPrompt: boolean = false;
    public isRTL: boolean = false;
    public hasReadPermision: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        this.accountingPeriodListService = new AccountingPeriodListService();
        this.accountingPeriodPMService = new AccountingPeriodPMService();
        this._AccountingPeriodExtendedListService = new AccountingPeriodExtendedListService();
        this._AccountingPeriodExtendedPMService = new AccountingPeriodExtendedPMService();
        // this.UIProperties.SetEnabled("Year", this.ObjectTableName, true);


        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.hasReadPermision = FeatureLocator.HasEntityPermessions(this.ObjectTableName, 'READ',false);
        if(this.hasReadPermision)
            this.UIProperties.SetEnabled("Year", this.ObjectTableName, true);
        else
            this.UIProperties.SetEnabled("Year", this.ObjectTableName, false);


    }

    // Properties
    private year;
    get Year() { return this.year; }
    set Year(value: number) {
        if (this.year != value) {
            this.year = value;
            this.ShowPrompt = false;
        }
    }
    noPeriodMsg: string = "";
    GetPeriods(year) {
        if (!AppTool.IsNullOrEmpty(year) && this.hasReadPermision) {

            var filters = new ApiQueryFilters();
            filters.GetAll = true;
            filters.addAdditionalFilter("Year", year, null, null, "Equal", false, false, false, "number");

            this.accountingPeriodListService.getByFilters(filters).subscribe((myResult: any) => {
                var result = myResult.Result;
                if (!AppTool.IsNullOrEmpty(result) && result.length > 0) {
                    this.ShowPrompt = false;
                    this.PeriodsList = result;

                } else {
                    this.PeriodsList = [];
                    this.ShowPrompt = true;

                    var msg = TextCodeTranslator.Translate("Accounting.O.PeriodLinesIsNotCreated");
                    this.noPeriodMsg = msg.replace("%Year", this.Year + "");
                }
            });

        }
    }

    BrowseButtonClicked() {
        if (AppTool.IsNullOrEmpty(this.year)) {
            this.Year = new Date().getFullYear();
        }
        if (this.year > (new Date().getFullYear())) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.FutureYear"));//"Future Year!");
        } else if (this.year < 1900) {

        } else {
            this.ValidationErrorsList = [];
            this.GetPeriods(this.year);
        }
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    EditPeriod(period: AccountingPeriodList) {
        if (!AppTool.IsNullOrEmpty(period)) {

            var windowArgs: Args = new Args();
            windowArgs.EntityId = period.Id;

            if (period.PeriodTypeCode == "2" || period.PeriodTypeCode == "3") { // 2-Invoice 3-Interest Invoice
                var row = this.PeriodsList.find(d => d.PeriodTypeCode == "1"); // 1-Accounting
                if (row != null) {
                    windowArgs.AccountingRow = row; // attach accounting period to window to use it in logic
                }
            }

            var logWindow = new LogitudeWindow();
            logWindow.Width = 530;
            logWindow.Height = 400;
            logWindow.Title = TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.GetPeriods(this.year));
            logWindow.Show('./Accounting/Components/Maintenance/EditAccountingPeriodComponent');
        }
    }

    ViewPeriodEvents(id: string) {

        this.accountingPeriodPMService.get(id).subscribe((myResult: any) => {
            var result = myResult.Result;
            if (!AppTool.IsNullOrEmpty(result)) {

                var entityPM = result;
                var windowArgs: EntityArgs = new EntityArgs();
                windowArgs.ObjectTableName = this.ObjectTableName;
                windowArgs.EntityPM = entityPM;
                //this.entityArgs = new EntityArgs();
                this.entityArgs.EntityPM = entityPM;
                var logWindow = new LogitudeWindow();
                logWindow.Width = 950;
                logWindow.Height = 600;
                logWindow.Title = "Accounting Period Events";
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => this.GetPeriods(this.year));
                logWindow.Show('./Accounting/Components/Maintenance/AccountingPeriodEventComponent');
            } else {
            }
        });


    }

    CreateRecord() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this._AccountingPeriodExtendedPMService.createDefaultPeriods(this.year).subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();
            var result = myResult.Result;
            this.BrowseButtonClicked();

        });
    }

    OnKeyUp(key) {
        if (!AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.BrowseButtonClicked();
            }
        }
    }

}
export class Args {
    EntityId: string;
    AccountingRow: AccountingPeriodList;
}
