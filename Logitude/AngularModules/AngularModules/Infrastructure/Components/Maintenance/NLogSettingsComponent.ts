import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ObservableCollection } from '../../Utilities/ObservableCollection';
import { CacheLogService } from '../../Services/ExtendedLists/CacheLogService';
import { Component } from '@angular/core';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { EntityArgs } from '../../DataContracts/EntityArgs';

import { AppTool } from '../../Tools';
import { CardPMService } from '../../../Common/Services/StandardPMs/CardPMService';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { WindowArgs } from 'Infrastructure/DataContracts/WindowArgs';
import { NLogService } from 'Infrastructure/Services/ExtendedLists/NLogService';

//
@Component({
    selector: 'NLogSettingsComponent',
    templateUrl: './NLogSettingsComponent.html',
    providers: [EntityArgs],
})

export class NLogSettingsComponent extends BaseComponent {
    public DataContext: NLogSettingsComponent = this;

    nLogService: NLogService = new NLogService();
    rulesList: Rule[] = [];
    Rules: ObservableCollection = new ObservableCollection([]);
    inputPattern: string = '';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.GetNLogsRules();
       
    }
   
    GetNLogsRules() {

        this.nLogService.getRules().subscribe((myResult: any) => {
            var result = myResult.Result;
            if (!AppTool.IsNullOrEmpty(result) && result.length > 0) {
                this.rulesList = result.sort((a, b) => {
                    const nameA = a.Name || '';
                    const nameB = b.Name || '';
                
                    return nameA.localeCompare(nameB);
                });
                this.Rules = new ObservableCollection([]);
                this.Rules.InsertCollection(this.rulesList, true);




            } else {
                this.rulesList = [];
                this.Rules = new ObservableCollection([]);
            }
        });

    }
    KillRule(){
        this.nLogService.killRule().subscribe((myResult: any) => {
          if(myResult)
            this.RefreshButtonClicked();
        })
    }
    AddLogTarget() {
        var windowArgs: any = {};
        windowArgs.list =  this.rulesList;        
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "New NLog";
        logitudeWindow.Show('./Accounting/Components/Maintenance/NewNLogComponent');  
        logitudeWindow.WindowClosed.subscribe(s => {    
            this.RefreshButtonClicked();
        });
    }
   
     
    searchText: string= "";
      
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
   
    RefreshButtonClicked() {
        this.GetNLogsRules();
    }

  

    
   
}
export class Rule {

    LogPattern: string;
    Minlevel: string;
    Target: string;
    Name: string;

}
