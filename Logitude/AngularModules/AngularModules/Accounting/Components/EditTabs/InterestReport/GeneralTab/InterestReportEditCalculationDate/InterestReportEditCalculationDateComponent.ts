import { Component} from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportPM } from 'Accounting/EntityPMs/InterestReportPM';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from 'Infrastructure/Tools';

@Component({
    
    templateUrl: './InterestReportEditCalculationDateComponent.html',
})

export class InterestReportEditCalculationDateComponent extends BaseComponent{

    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string = "InterestReport";
    public DataContext: InterestReportEditCalculationDateComponent = this;
    public myService: InterestReportPMService = new InterestReportPMService();
    public isRTL: boolean = false;

    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
         
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked(Val:any) {
        if(AppTool.IsNullOrEmpty(Val)){
           Val=null;
        }
        this.CurrentSession.CloseCurrentWindowEmit(Val);
    }

    private interestCalculationDate:Date;
    get InterestCalculationDate() {
                    return this.interestCalculationDate;
           }
    set InterestCalculationDate(newValue: Date) {
        if (this.interestCalculationDate != newValue) {
            this.interestCalculationDate = newValue;
        }
    }
}