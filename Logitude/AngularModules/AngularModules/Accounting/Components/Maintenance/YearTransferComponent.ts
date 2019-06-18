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
import {JournalOpService} from '../../Services/Others/JournalOpService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';


@Component({
    moduleId: module.id,
    selector: 'YearTransferComponent',
    templateUrl: './YearTransferComponent.html',
    providers: [EntityArgs],
})

export class YearTransferComponent extends BaseComponent {
    public DataContext: YearTransferComponent = this;
    public ObjectTableName: string = "AccountingPeriod";
    _JournalOpService: JournalOpService;
    public ValidationErrorsList: string[];
    

    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        this._JournalOpService = new JournalOpService();
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Year", this.ObjectTableName, true);
        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe((response: any) => { });
        SessionLocator.CurrentSession.StopBusyIndicator();
    }

    // Properties
    private year;
    get Year() { return this.year; }
    set Year(value: number) {
        if (this.year != value) {
            this.year = value;
            
        }
    }

    

    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.year)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Year Field is Required");
        }
        if (this.year > (new Date().getFullYear())) {

            this.ValidationErrorsList.push("Future Year!");
        } else if (this.year < 1900) {

        } else {
            this.ValidationErrorsList = [];

        }
    }
    _JournalPM: JournalPM = null;
    OkButtonClicked() {
        
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        SessionLocator.CurrentSession.StartBusyIndicatorCreating();
        this._JournalOpService
            .GetYearTransferJournal(this.year)
            .subscribe(
            (res: ServiceResponse) => {
                if (res.HasError) {
                    this.ValidationErrorsList = res.ErrorsArray;
                        
                } else {
                    this._JournalPM = res.Result;
                }
            },
            (err) => {
                alert(err);
            },
            () => {
                SessionLocator.CurrentSession.StopBusyIndicator();
            }
        );
    }
    OpenJournal() {
        if (!AppTool.IsNullOrEmpty(this._JournalPM.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this._JournalPM.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    OnKeyUp(key) {
        if (!AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    }

}
