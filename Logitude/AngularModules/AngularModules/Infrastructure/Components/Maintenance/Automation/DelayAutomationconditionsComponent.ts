
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';

import {AutomationCondition} from '../../../../Infrastructure/DataContracts/AutomationCondition';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AutomationConditionViewModel} from './ViewModel/AutomationConditionViewModel';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools'
import {AddEditAutomationsComponent} from '../../../../Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent';

import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
@Component({
    moduleId: module.id,
    selector: 'DelayAutomationconditionsComponent',
    templateUrl: './DelayAutomationconditionsComponent.html',

   
})

export class DelayAutomationconditionsComponent extends BaseComponent implements OnInit {


    IsViewCondition: boolean;
    AutomationCondationOrList: AutomationConditionViewModel[] = [];
    AutomationCondationAndList: AutomationConditionViewModel[] = [];
    SelectedAutomationCondationAndList: AutomationConditionViewModel;
    SelectedAutomationCondationOrList: AutomationConditionViewModel;
    DataContext: DelayAutomationconditionsComponent = this;
    addEditAutomationsComponent: AddEditAutomationsComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

    }

    ngOnInit() {


    }

    SetDataContext(dataContext: any) {

        this.addEditAutomationsComponent = dataContext;
        this.BuildAutomationCondition();
    }

    




    BuildAutomationCondition() {

        this.IsViewCondition = false;
        var automationConditionPMList = this.addEditAutomationsComponent.AutomatedBackupClass.DelayAautomationConditionLists;

        if (automationConditionPMList != null) {
            automationConditionPMList.forEach((item) => {
                if (item.ConditionType == "And") {
                    this.AutomationCondationAndList.push(new AutomationConditionViewModel(item, this.addEditAutomationsComponent, this));
                }
                else if (item.ConditionType == "Or") {
                    this.AutomationCondationOrList.push(new AutomationConditionViewModel(item, this.addEditAutomationsComponent, this));
                }
            });
        }
    }




    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {

        var automationConditionList: AutomationCondition[] = [];

        this.AutomationCondationAndList.forEach((item) => {
           
                item.CurrentEntityPM.AutomationsId = this.addEditAutomationsComponent.CurrentEntityPM.Id;
                item.CurrentEntityPM.UpdateDate = this.addEditAutomationsComponent.CurrentEntityPM.UpdateDate;
                item.CurrentEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                automationConditionList.push(item.CurrentEntityPM);

            });

        this.AutomationCondationOrList.forEach((item) => {

            item.CurrentEntityPM.AutomationsId = this.addEditAutomationsComponent.CurrentEntityPM.Id;
            item.CurrentEntityPM.UpdateDate = this.addEditAutomationsComponent.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);

        });


      this.addEditAutomationsComponent.AutomatedBackupClass.DelayAautomationConditionLists = automationConditionList;
 
       this.CurrentSession.CloseCurrentWindow();
    }

    AddAutomationConditionMethod(conditionType: string) {
        var automationConditionPM: AutomationCondition = new AutomationCondition();

        automationConditionPM.ConditionType = conditionType;
        automationConditionPM.Tenant = SessionLocator.TenantPM.Id;
        automationConditionPM.CreatedByUserId = SessionLocator.LoggedUserId;
        automationConditionPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        automationConditionPM.Value = "";
        automationConditionPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.OperatorCode = "Equals";
        automationConditionPM.ObjectFieldId = "";
        automationConditionPM.AutomationsId = this.addEditAutomationsComponent.CurrentEntityPM.Id;


        if (conditionType == "And") {
            this.AutomationCondationAndList.push(new AutomationConditionViewModel(automationConditionPM, this.addEditAutomationsComponent, this));
        }

        else {
            this.AutomationCondationOrList.push(new AutomationConditionViewModel(automationConditionPM, this.addEditAutomationsComponent , this));

        }

    }



}

