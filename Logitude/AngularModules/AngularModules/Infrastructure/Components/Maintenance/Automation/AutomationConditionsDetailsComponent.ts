import { Component } from '@angular/core';
import { AutomationHistoryPM } from '../../../../Common/EntityPMs/AutomationHistoryPM';
import { EntityChangePM } from '../../../../Common/EntityPMs/EntityChangePM';
import { AutomationHistoryExtendedPMService } from '../../../../Common/Services/ExtendedPMs/AutomationHistoryExtendedPMService';
import { ApiQueryFilters } from '../../../DataContracts/ApiQueryFilters';
import { AutomationCondition } from '../../../DataContracts/AutomationCondition';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { ObjectFieldPM } from '../../../EntityPMs/ObjectFieldPM';
import { EntityListService } from '../../../Services/EntityListService';
import { AppTool } from '../../../Tools';
import { SessionLocator } from '../../../Utilities/SessionLocator';
import { AutomationConditionViewModel } from './ViewModel/AutomationConditionViewModel';
declare var window: any;

  
@Component({
    templateUrl: './AutomationConditionsDetailsComponent.html',
    providers: [EntityListService, AutomationHistoryExtendedPMService]

})
export class AutomationConditionsDetailsComponent {
    public EntityPM: any;
    public DataContext: any; 
    private CurrentSession = SessionLocator.SelectedSession;
    AutomationCondationList: AutomationConditionViewModel[] = [];
    AutomationCondationAndList: AutomationConditionViewModel[] = [];
    AutomationCondationOrList: AutomationConditionViewModel [] = [];
    DisplayValue: string; 
    public SelectedItem: any;
    AllowedinAutomationConditionsFieldLists: ObjectFieldPM[] = [];
    ObjectTableId: string;
    ObjectTableName: string;
    CurrentEntityPM: any;
    DataViewModel: any;
    AutomationHistoryLists: AutomationHistoryPM[];

    constructor(public entityListService: EntityListService, public _automationHistoryExtendedPMService: AutomationHistoryExtendedPMService) {
         
    }
     

    SetWindowArgs(windowArgs) {
        this.DataViewModel = windowArgs.DataViewModel;
        this.ObjectTableName = this.DataViewModel.ObjectTableName;

        console.log("windowArgs" + windowArgs);
        this.intial();
        this.InitializeConditionsList();

        var automationConditionPMList = windowArgs.CurrentEntityPM[0].ConditionsList;
        this.CurrentEntityPM = windowArgs.CurrentEntityPM;
         
         

        if (automationConditionPMList != null) {
            automationConditionPMList.forEach((item) => {

                console.log("TEST CONDITION LIST " + item.ConditionType);
                if (item.ConditionType == "And") {
                    this.AutomationCondationAndList.push(new AutomationConditionViewModel(item, this));
                }
                else if (item.ConditionType == "Or") {
                    this.AutomationCondationOrList.push(new AutomationConditionViewModel(item, this));
                }
            });
        }

 

    }


    intial() {

        var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
        this.ObjectTableId = table.Id;
        this.AllowedinAutomationConditionsFieldLists = window.ObjectFields.filter(f => f.ObjectTableId == table.id && (f.AllowedinAutomationConditions == true || f.IsCustom));
        window.ObjectFields.filter(f => f.DisplayInAutomationAsEnitity == true && f.ObjectTableId == table.id && (!f.RecordType || (f.RecordType && f.RecordType.split(',').filter(d => d == this.ObjectTableName)[0]))).forEach((otherEntityObjectField) => {
            window.ObjectFields.filter(f => f.AllowedinAutomationConditions || f.IsCustom == true && f.ObjectTableId == otherEntityObjectField.LookUpTableId).forEach((item) => {
                this.AllowedinAutomationConditionsFieldLists.push(item);
            });
        });

    }


    private InitializeConditionsList() {
        var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
          
        this.AllowedinAutomationConditionsFieldLists = [];

        this.AllowedinAutomationConditionsFieldLists = window.ObjectFields.filter(f => f.ObjectTableId == this.ObjectTableId && (f.AllowedinAutomationConditions == true || f.IsCustom));
         
        //Additional Automation Entity
        //AutomationCondition
        window.ObjectFields.filter(f => f.DisplayInAutomationAsEnitity == true && f.ObjectTableId == this.ObjectTableId && (!f.RecordType || (f.RecordType && f.RecordType.split(',').filter(d => d == this.ObjectTableName)[0]))).forEach((otherEntityObjectField) => {
            window.ObjectFields.filter(f => f.AllowedinAutomationConditions || f.IsCustom == true && f.ObjectTableId == otherEntityObjectField.LookUpTableId).forEach((item) => {
                this.AllowedinAutomationConditionsFieldLists.push(item);
                console.log("item " + item);
            });

        });

         
       
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
