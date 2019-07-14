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
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';


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
    
    private CurrentSession = SessionLocator.SelectedSession;
    _CancelYearTransfer: boolean;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
        this._JournalOpService = new JournalOpService();
        this.UIProperties.SetEnabled("Year", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Year", this.ObjectTableName, true);
        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });
        this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe((response: any) => { });
        this.CurrentSession.StopBusyIndicator();
    }
    SetWindowArgs(arg: any) {
        this._CancelYearTransfer = arg.CancelYearTransfer;
        if (!this._CancelYearTransfer) {
            this.myOperation = "Check_CreateQBatchTaskYearTransfer";
        } else {
            this.myOperation = "CheckCancelYear";
        }
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
    myOperation: string="";
    lastYearTransferJournalPMId: string="";
    _JournalPM: JournalPM = null;
    OkButtonClicked() {
        
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        
        this._JournalOpService
            .GetYearTransferJournal(this.year, this.myOperation, this.lastYearTransferJournalPMId)
            .subscribe(
            (res: ServiceResponse) => {
                
                if (res.HasError) {
                    this.ValidationErrorsList = res.ErrorsArray;
                        
                } else {
                    switch (this.myOperation) {
                        case"Check_CreateQBatchTaskYearTransfer":
                            {
                                let obj: any = res.Result;
                                let BatchTaskYearTransferId = obj.BatchTaskYearTransferId;
                                var winMessage = new MessageWindow()
                                winMessage.Show("העברת השנה תבוצע בתהליך רקע ");
                                winMessage.WindowClosed.subscribe(($event: any) => {
                                    this.CancelButtonClicked();
                                });
                            }
                            break;
                        case "CheckCancelYear":
                            {
                                let obj: any = res.Result;
                                let lastYearTransferJournalPMId = obj.lastYearTransferJournalPMId;
                                let confirmWindow = new ConfirmWindow()
                                //Are you sure you want to cancel the year transfer (YY)?   “האם אתה בטוח שברצונך לבטל את העברת השנה (YY)?”
                                confirmWindow.Show("האם אתה בטוח שברצונך לבטל את העברת השנה (" + this.year +")");
                                confirmWindow.WindowClosed.subscribe((event: any) => {
                                    if (confirmWindow.Yes) {
                                        this.myOperation = "DoCancelYear"
                                        this.lastYearTransferJournalPMId = lastYearTransferJournalPMId;
                                        this.OkButtonClicked()
                                    }
                                    else if (confirmWindow.No) {
                                        this.CancelButtonClicked();
                                    }
                                });
                            }
                            break;
                        case "DoCancelYear":
                            {
                                this._JournalPM = res.Result;
                            }
                            break;
                        default:
                            {

                            }
                            break;
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
    OpenJournal() {
        if (!AppTool.IsNullOrEmpty(this._JournalPM.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this._JournalPM.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OnKeyUp(key) {
        if (!AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    }

}
