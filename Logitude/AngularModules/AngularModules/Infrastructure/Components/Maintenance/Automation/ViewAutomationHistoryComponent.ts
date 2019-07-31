
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';

import {AutomationCondition} from '../../../../Infrastructure/DataContracts/AutomationCondition';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AutomationConditionViewModel} from './ViewModel/AutomationConditionViewModel';
import {AutomationHistoryExtendedPMService} from '../../../../Common/Services/ExtendedPMs/AutomationHistoryExtendedPMService';

import {AutomationHistoryPM} from '../../../../Common/EntityPMs/AutomationHistoryPM';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
@Component({
    moduleId: module.id,
    selector: 'ViewAutomationHistoryComponent',
    templateUrl: './ViewAutomationHistoryComponent.html',
    providers: [AutomationHistoryExtendedPMService],

})

export class ViewAutomationHistoryComponent extends BaseComponent implements OnInit {

    DataContext: ViewAutomationHistoryComponent = this;
    IsViewCondition: boolean;
    AutomationCondationOrList: AutomationConditionViewModel[] = [];
    AutomationCondationAndList: AutomationConditionViewModel[] = [];
    SelectedAutomationCondationAndList: AutomationConditionViewModel;
    SelectedAutomationCondationOrList: AutomationConditionViewModel;
    DataViewModel: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _automationHistoryExtendedPMService: AutomationHistoryExtendedPMService) {
        super();

    }

    ngOnInit() {


    }

    CurrentEntityPM: AutomationHistoryPM;


    SetWindowArgs(args: any) {
        this.CurrentEntityPM = args.AutomationHistoryPM;
        this.DataViewModel = args.DataViewModel;



        if (this.CurrentEntityPM) {
            if (this.CurrentEntityPM.AutomatedDataBackup) {
                this.BuildAutomationCondition();
            }
            else {
                this.LoadAutomatedDataBackup();
            }
        }

    }

    LoadAutomatedDataBackup() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._automationHistoryExtendedPMService.getAutomationBackupDataByAutomationId(this.CurrentEntityPM.AutomationsId, this.CurrentEntityPM.Version, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.CurrentEntityPM.AutomatedDataBackup = myResult;
                this.BuildAutomationCondition();
                this.DataViewModel.UpdateCurrentAutomationHository(this.CurrentEntityPM);

            }

        });
    }


    BuildAutomationCondition() {

        if (this.CurrentEntityPM.AutomatedDataBackup && this.CurrentEntityPM.AutomatedDataBackup.AautomationConditionLists) {

        this.IsViewCondition = false;
        var automationConditionPMList = this.CurrentEntityPM.AutomatedDataBackup.AautomationConditionLists;

        if (automationConditionPMList != null) {
            automationConditionPMList.forEach((item) => {
                if (item.ConditionType == "And") {
                    this.AutomationCondationAndList.push(new AutomationConditionViewModel(item, this.DataViewModel, this));
                }
                else if (item.ConditionType == "Or") {
                    this.AutomationCondationOrList.push(new AutomationConditionViewModel(item, this.DataViewModel, this));
                }
            });
        }
        }
    }


    CloseButtonClicked() {

     

        this.CurrentSession.CloseCurrentWindow();
    }



}

