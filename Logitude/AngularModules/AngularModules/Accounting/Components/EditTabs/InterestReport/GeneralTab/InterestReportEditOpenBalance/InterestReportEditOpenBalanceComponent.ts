import { Component} from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportPM } from 'Accounting/EntityPMs/InterestReportPM';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from 'Infrastructure/Tools';
 


@Component({
    
    templateUrl: './InterestReportEditOpenBalanceComponent.html',
})

export class InterestReportEditOpenBalanceComponent extends BaseComponent{
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string = "InterestReport";
    public DataContext: InterestReportEditOpenBalanceComponent = this;
    public myService: InterestReportPMService = new InterestReportPMService();
    public isRTL: boolean = false;
 
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
         
    }
 
    SetDataContext(OpenBalance: number) {
        this.OpenBalance = OpenBalance;
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked(Val:any) {
        if(AppTool.IsNullOrEmpty(Val)){
           Val=0;
        }
        this.CurrentSession.CloseCurrentWindowEmit(Val);
    }
    private openBalance:number; 
    get OpenBalance(){
        return this.openBalance
    }
    set OpenBalance(val: number){
         this.openBalance=val;
    }
    GetAccountingPeriods() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myService.update(this.EntityPM ).subscribe((myResponse: ServiceResponse) => {
        this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (!myResponse.HasError) {
 
                }
            }
        });
    }
}
 
 