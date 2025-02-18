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
import { NLogService } from 'Infrastructure/Services/ExtendedLists/NLogService';
import { Rule } from 'Infrastructure/Components/Maintenance/NLogSettingsComponent';


@Component({
    
    selector: 'NewNLogComponent',
    templateUrl: './NewNLogComponent.html',
    providers: [EntityArgs],
})

export class NewNLogComponent extends BaseComponent {
    public DataContext: NewNLogComponent = this;
    public ObjectTableName: string = "Nlog";
    public ValidationErrorsList: string[];
    nLogService: NLogService = new NLogService();
    rulesList:Rule[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityArgs: EntityArgs) {
        super();
      
        this.CurrentSession.StopBusyIndicator();
    }
    SetWindowArgs(arg: any) {
        this.rulesList= arg.list;
    }
    // Properties
    private logPattern;
    get LogPattern() { return this.logPattern; }
    set LogPattern(value: string) {
        if (this.logPattern != value) {
            this.logPattern = value;
        }
    }

    

    FillErrors() {
        this.ValidationErrorsList = [];
          
        if (this.rulesList?.some(rule => rule.Name?.startsWith('ManualLog'))) {
            this.ValidationErrorsList.push('ManualLog Already Exists');
        }
    }
    
    OkButtonClicked() {
        
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
               

        this.nLogService.addLog(this.LogPattern).subscribe((myResult: any) => {
            if(myResult){
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
            }
                
        });
    
       
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
   

}
