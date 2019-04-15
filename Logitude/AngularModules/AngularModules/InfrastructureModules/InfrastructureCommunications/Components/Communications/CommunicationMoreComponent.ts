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

//import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { CommunicationLogListService } from '../../../../Common/Services/StandardLists/CommunicationLogListService';

import { CommunicationLogList } from '../../../../Common/EntityLists/CommunicationLogList';


@Component({
    moduleId: module.id,

    selector: 'communication-steps',
    templateUrl: './CommunicationMoreComponent.html',
})

export class CommunicationMoreComponent
    extends BaseComponent
    implements OnInit {
    public EntityPM: any;

    
    public ObjectTableName: string = "Customs.CommunicationLog";
    public DataContext: any = this;
    public Tab: LogTab;
    public IsDisplayOnly: boolean = false;

    public CorrelationID: string = "";
    public ExternalID: string = ""; 
    public Logs: string = "";
    public ExceptionMessage: string = "";
    _CommunicationLogList: CommunicationLogList;
    _CommunicationLogListService: CommunicationLogListService

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this._CommunicationLogListService = new CommunicationLogListService();
        
    }
    ngOnInit() {
        ///this.BuildColumns();
       
        
    }
    SetTabArgs(args: any) {
//'Id': CustomsRequestsSheetList.RequestComminicationId,
//'Tenant': CustomsRequestsSheetList.Tenant,
//'CorrelationId': CustomsRequestsSheetList.CorrelationId,
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        if (this._CommunicationLogList == null) {
            this.LoadCommunicationLog();
        }
        console.log("arg");
    }
    _input2CopyToClipboardId: string = "_input2CopyToClipboardId";
    Copy2Clipboard(token:string) {
        console.log("Copy2Clipboard..");
        var temp = document.getElementById(this._input2CopyToClipboardId) as HTMLInputElement;
        switch (token) {
            case "CorrelationID": { temp.value = this.CorrelationID;; break; }
            case "ExternalID": { temp.value = this.ExternalID;; break; }
            case "Logs": { temp.value = this.Logs;; break; }
            case "ExceptionMessage": { temp.value = this.ExceptionMessage;; break; }
        }
        
        
        temp.select();
        document.execCommand("copy");
    }
    LoadCommunicationLog() {

        this._CommunicationLogListService.getSingle(this.EntityPM.Id).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
           
                this._CommunicationLogList = result;
                this.CorrelationID = this._CommunicationLogList.CorrelationID || this.EntityPM.CorrelationId;
                this.ExternalID = this.EntityPM.CustomsRequestsSheetId;
                this.Logs = this._CommunicationLogList.Logs;
                this.ExceptionMessage = this._CommunicationLogList.ExceptionMessage;
                // this.CurrentSession.StopBusyIndicator();
            }
            else {

                // this.CurrentSession.StopBusyIndicator();
            }

        });



    }
   
   
}
