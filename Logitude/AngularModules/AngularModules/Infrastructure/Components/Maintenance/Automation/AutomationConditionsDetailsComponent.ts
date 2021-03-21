import { Component } from '@angular/core';
import { EntityChangePM } from '../../../../Common/EntityPMs/EntityChangePM';
import { ApiQueryFilters } from '../../../DataContracts/ApiQueryFilters';
import { AutomationCondition } from '../../../DataContracts/AutomationCondition';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { EntityListService } from '../../../Services/EntityListService';
import { SessionLocator } from '../../../Utilities/SessionLocator';
import { AutomationConditionViewModel } from './ViewModel/AutomationConditionViewModel';

  
@Component({
    templateUrl: './AutomationConditionsDetailsComponent.html',
    providers: [EntityListService]

})
export class AutomationConditionsDetailsComponent {
    public EntityPM: any;
    public DataContext: any; 
    private CurrentSession = SessionLocator.SelectedSession;
    AutomationCondationList: AutomationCondition[] = [];
    AutomationCondationAndList: AutomationCondition[] = [];
    AutomationCondationOrList: AutomationCondition[] = [];
    DisplayValue: string; 
    public SelectedItem: any; 
    constructor(public entityListService: EntityListService) {

    }

    SetWindowArgs(windowArgs) {

        console.log("windowArgs" + windowArgs);
        this.AutomationCondationList = windowArgs.CurrentEntityPM[0].ConditionsList;

        this.InitializeConditionsList(); 

    }

    private InitializeConditionsList() {
        for (var condition of this.AutomationCondationList) {
            if (condition.ConditionType == "And") {
                this.AutomationCondationAndList.push(condition);
            } else {
                this.AutomationCondationOrList.push(condition);
            }
        }
    }

    SetDataContext(DataContext: EntityChangePM) {
        this.DataContext = DataContext; 
        console.log("DataContext" + this.DataContext); 
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("Cancel");
    } 

    SelectedItemChanged(event) {

        console.log("event" + event);
    }
 

}
