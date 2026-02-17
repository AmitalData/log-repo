declare var window: any;
import { Directive, ChangeDetectorRef , Renderer, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit } from '@angular/core';

import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from    '../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, ArrayTool } from                  '../../../../Infrastructure/Tools';
import { CommunicationLogStepDataViewModel } from   '../CommunicationLog/ViewModel/CommunicationLogStepDataViewModel';
import { ServiceResponse } from                     '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AmitalGatewayUtil } from                   '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { CommunicationLogStepListService } from '../../../../Common/Services/ExtendedLists/CommunicationLogStepListService';

import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    moduleId: module.id,

    selector: 'communication-steps',
    templateUrl: './CommunicationStepsComponent.html',
})

export class CommunicationStepsComponent
    extends BaseComponent
    implements OnInit {
    public EntityPM: any;


    public ObjectTableName: string = "Customs.CommunicationLogStep";
    public DataContext: any = this;
    public Tab: LogTab;
    public IsDisplayOnly: boolean = false;

    public columns: any[] = null;
    public _communicationLogStepListService: CommunicationLogStepListService;
    _CommunicationLogStepDataViewModelList: ObservableCollection;//CommunicationLogStepDataViewModel[];
    
    
    
    

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this._communicationLogStepListService = new CommunicationLogStepListService();
      
        this._CommunicationLogStepDataViewModelList = new ObservableCollection([]);
    }
    ngOnInit() {
        ///this.BuildColumns();
        this.LoadCommunicationLogSteps();
        
    }
    SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        
        
    }
    ViewLogMethod(item) {

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = TextCodeTranslator.Translate("CommunicationLogSteps.O.Log");
        logitudeWindow.WindowArgs = item.Log;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
    }
    ViewXMLClicked(item) {


        if (item.SecurityId) {
         
            DownloadManager.DownloadPage("", item.SecurityId);
        }
        else {
           
            DownloadManager.DownloadPage(item.DocumentId,null);
         
        }
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(link);
        //    return;

        //}
  
    }
    _showGrid: boolean = false;
    RefreshButtonClicked() {
        this._CommunicationLogStepDataViewModelList.Clear();
        this.LoadCommunicationLogSteps();
    }
    LoadCommunicationLogSteps() {

        
        this._communicationLogStepListService.getCommunicationLogStepsListsByLogId(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var communicationLogStepDataViewModelList = [];
                    
                    
                    result
                        //.sort(d => d.StepNumber)
                        .forEach((item) => {
                        communicationLogStepDataViewModelList.push(new CommunicationLogStepDataViewModel(item));
                    });
                    this._CommunicationLogStepDataViewModelList.InsertCollection(communicationLogStepDataViewModelList);
                    //this._CommunicationLogStepDataViewModelList.InsertCollection(result);
                    
                    this.cd.detectChanges();
                    this._showGrid = true;


                }
                // this.CurrentSession.StopBusyIndicator();
            }
            else {

                // this.CurrentSession.StopBusyIndicator();
            }

        });



    }
}
