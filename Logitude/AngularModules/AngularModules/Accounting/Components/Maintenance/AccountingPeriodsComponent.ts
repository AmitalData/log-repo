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
import { PeriodTypeListService } from '../../Services/StandardLists/PeriodTypeListService';
import { PeriodTypeList } from '../../EntityLists/PeriodTypeList';
import { CounterDefinitionPMExtendedService } from 'Infrastructure/Services/ExtendedPMs/CounterDefinitionPMExtendedService';
import { CounterDefinitionPM } from 'Infrastructure/EntityPMs/CounterDefinitionPM';
import { switchMap } from 'rxjs/operators';


@Component({
    
    selector: 'AccountingPeriodsComponent',
    templateUrl: './AccountingPeriodsComponent.html',
    providers: [EntityArgs],
})

export class AccountingPeriodsComponent extends BaseComponent {
    public DataContext: AccountingPeriodsComponent = this;
    public ObjectTableName: string = "AccountingPeriod";
    accountingPeriodListService: AccountingPeriodListService;
    periodTypeListService: PeriodTypeListService;
    counterDefinitionPMExtendedService: CounterDefinitionPMExtendedService;
    accountingPeriodPMService: AccountingPeriodPMService;
    _AccountingPeriodExtendedListService: AccountingPeriodExtendedListService;
    _AccountingPeriodExtendedPMService: AccountingPeriodExtendedPMService;
    public ValidationErrorsList: string[];
    GotPeriodsList: AccountingPeriodList[];
    PeriodsList: AccountingPeriodList[];
    PeriodTypesList: PeriodTypeList[];
    CounterDef: CounterDefinitionPM[];
 
    ShowPrompt: boolean = false;
    public isRTL: boolean = false;
    public hasReadPermision: boolean = false;
    public uniquePeriodPrefix: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        this.accountingPeriodListService = new AccountingPeriodListService();
        this.periodTypeListService = new PeriodTypeListService();
        this.accountingPeriodPMService = new AccountingPeriodPMService();
        this._AccountingPeriodExtendedListService = new AccountingPeriodExtendedListService();
        this._AccountingPeriodExtendedPMService = new AccountingPeriodExtendedPMService();
        this.counterDefinitionPMExtendedService = new CounterDefinitionPMExtendedService();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.hasReadPermision = FeatureLocator.HasEntityPermessions(this.ObjectTableName, 'READ',false);
        if(this.hasReadPermision)
            this.UIProperties.SetEnabled("Year", this.ObjectTableName, true);
        else
            this.UIProperties.SetEnabled("Year", this.ObjectTableName, false);

        this.GetPeriodTypes();
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
            const showLocals = !SessionLocator.LoggedUserPM.DontShowLocalLabels;
            var filters = new ApiQueryFilters();
            filters.GetAll = true;
            filters.addAdditionalFilter("Year", year, null, null, "Equal", false, false, false, "number");

            this.accountingPeriodListService.getByFilters(filters).subscribe((myResult: any) => {
                var result = myResult.Result;
                if (!AppTool.IsNullOrEmpty(result) && result.length > 0) {
                    this.ShowPrompt = false;
                    this.GotPeriodsList = result;
                    if (this.GotPeriodsList != null && showLocals && this.PeriodTypesList != null) {
                        let localMap = new Map<string, string>();
                        this.PeriodTypesList.forEach(s => {
                            localMap.set(s.Code, s.LocalName);
                        });
                        this.PeriodsList = [];
                        this.GotPeriodsList.forEach(s => {
                            let localName = localMap.get(s.PeriodTypeCode);
                            s.PeriodTypeName = localName;
                            this.PeriodsList.push(s);
                        });

                    } else {
                        this.PeriodsList = this.GotPeriodsList;
                        
                    }
                    if(!this.uniquePeriodPrefix)
                        this.PeriodsList = this.PeriodsList.filter(a => a.PeriodTypeCode === PeriodTypeCode.Accounting || a.PeriodTypeCode === PeriodTypeCode.Invoice); 
                    this.PeriodsList.sort((a, b) => (a.PeriodTypeCode > b.PeriodTypeCode) ? 1 : ((b.PeriodTypeCode > a.PeriodTypeCode) ? -1 : 0));

                } else {
                    this.PeriodsList = [];
                    this.ShowPrompt = true;

                    var msg = TextCodeTranslator.Translate("Accounting.O.PeriodLinesIsNotCreated");
                    this.noPeriodMsg = msg.replace("%Year", this.Year + "");
                }
            });

        }
    }

    GetPeriodTypes() {
        const COUNTER_NAME_AR_INVOICE = "A/R Invoice";

        this.CurrentSession.StartBusyIndicatorLoading();
        const filters = new ApiQueryFilters();
        filters.GetAll = true;
     
        this.counterDefinitionPMExtendedService
        .GetCounterDefinitionsByCounterName(COUNTER_NAME_AR_INVOICE)
        .pipe(
            switchMap((counterResult: any) => {
            
                              
                this.CounterDef = !AppTool.IsNullOrEmpty(counterResult?.Result) ? counterResult.Result : [];
                this.uniquePeriodPrefix = this.CounterDef?.find(a => a.Parameter1 === "IN")?.UniquePerPrefix ?? true;
                return this.periodTypeListService.getByFilters(filters);
            })
        )
        .subscribe(
            (periodResult: any) => {
                const result = periodResult?.Result || [];

                if (result.length > 0) {
                    this.ShowPrompt = false;

                    this.PeriodTypesList = result.map((item: any) => {
                        const code = this.mapPeriodTypToCode(item.Code);

                        const counter = this.CounterDef.find(cd => cd.Parameter1 === code);

                        const prefix = counter?.Prefix || '';

                        return {
                            ...item,
                            LocalName: prefix ? `(${prefix}) ${item.LocalName || ''}`.trim() : item.LocalName
                        };
                    });
                } else {
                    this.PeriodTypesList = [];
                }

                this.CurrentSession.StopBusyIndicator();
            },
            (error) => {
                console.error('Error loading PeriodTypes:', error);
                this.PeriodTypesList = [];
                this.CurrentSession.StopBusyIndicator();
            }
        );

    }
    mapPeriodTypToCode(periodTyp: string): string {
        switch (periodTyp) {
            case PeriodTypeCode.Accounting: return 'Accounting';
            case PeriodTypeCode.Invoice: return 'IN';
            case PeriodTypeCode.InterestInvoice: return 'IT';
            case PeriodTypeCode.CreditNote: return 'CD';
            default: return '';
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
            if(!this.uniquePeriodPrefix){
                windowArgs.AccountingRows = this.GotPeriodsList;
             }
            var logWindow = new LogitudeWindow();
            logWindow.Width = 530;
            logWindow.Height = 400;
            logWindow.Title = TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => {

                this.GetPeriods(this.year)
            });
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
    AccountingRows: AccountingPeriodList[];
}
 export enum PeriodTypeCode {
    Accounting = "1",
    Invoice = "2",
    InterestInvoice = "3",
    CreditNote = "4"
  }
  