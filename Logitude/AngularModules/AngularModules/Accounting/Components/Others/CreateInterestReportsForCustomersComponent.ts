import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import { InterestReportPMService } from '../../Services/StandardPMs/InterestReportPMService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { InterestReportGeneralTabComponent } from '../EditTabs/InterestReport/GeneralTab/InterestReportGeneralTabComponent';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../Infrastructure/Tools';
import { InterestReportExtendedListService } from 'Accounting/Services/ExtendedLists/InterestReportExtendedListService';


@Component({
    selector: 'CreateInterestReportsForCustomersComponent',
    
    providers: [EntityListService,InterestReportExtendedListService],
    templateUrl: './CreateInterestReportsForCustomersComponent.html',
})

export class CreateInterestReportsForCustomersComponent extends BaseComponent implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "InterestReport";
    public DataContext=this;
    constructor(public entityArgs: EntityArgs,private interestReportExtendedListService:InterestReportExtendedListService) {
        super();
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.Validate();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.interestReportExtendedListService.PostInterestReportsForEligibleCustomerCreationInBatch(this.interestCalculationDate)
            .subscribe((myResult:ServiceResponse) => {
                   
                this.CurrentSession.StopBusyIndicator();
                if(myResult.HasError){
                    this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                }
                else
                {
                    this.CurrentSession.CloseCurrentWindow();
                }

            });
        } 
    }
    
    
    ngOnDestroy() {
    }


    Validate(){
        if(this.InterestCalculationDate==null)
        {
            this.ValidationErrorsList.push("Interest Calculation Date is Required");
        }
    }

    private interestCalculationDate:Date;
    get InterestCalculationDate() {
        if (this.interestCalculationDate != null) {
            return this.interestCalculationDate;
        }
        else
            return null;
    }
    set InterestCalculationDate(newValue: Date) {
        if (this.interestCalculationDate != newValue) {
            this.interestCalculationDate= newValue;
        }
    }
}