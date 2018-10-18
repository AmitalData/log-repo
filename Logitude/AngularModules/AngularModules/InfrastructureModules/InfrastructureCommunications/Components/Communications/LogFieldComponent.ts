declare var window: any;
import { Directive, ChangeDetectorRef, Renderer, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit } from '@angular/core';

import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { CommunicationLogStepDataViewModel } from '../CommunicationLog/ViewModel/CommunicationLogStepDataViewModel';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

import { CommunicationLogStepListService } from '../../../../Common/Services/ExtendedLists/CommunicationLogStepListService';




@Component({
    moduleId: module.id,

    selector: 'communication-LogField',
    templateUrl: './LogFieldComponent.html',
})

export class LogFieldComponent
    extends BaseComponent
    implements OnInit {
    


    public ObjectTableName: string = "Customs.CommunicationLog";
    public DataContext: any = this;
    public Tab: LogTab;
    public IsDisplayOnly: boolean = false;

    _CommunicationLogStepDataViewModel: any; //CommunicationLogStepDataViewModel
    _Log: string;
    





    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        
    }
    ngOnInit() {
        ///this.BuildColumns();
        let a = 1;

    }
    _inputLogId: string;
    SetTabArgs(args: any) {
        this._CommunicationLogStepDataViewModel = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        this._inputLogId = "_inputLogId";

        
    }
    CopyLog2Clipboard() {

        var temp = document.getElementById(this._inputLogId) as HTMLInputElement;
        //temp.value = this._Log;
        temp.select();
        document.execCommand("copy");
    }
    //SetWindowArgs(args: any) {
    //    this.EntityPM = args; ///CommunicationLogStepDataViewModel
    //    if (this.EntityPM)
    //    {
    //        this._Log = this.EntityPM.Log;
           
    //    }

    //    this.Tab = args.Tab;
    //    this.IsDisplayOnly = args.Disabled;


    //}

    SetWindowArgs(ShowLog: string) {
        this._Log = ShowLog;

        //this.Tab = args.Tab;
        //this.IsDisplayOnly = args.Disabled;


    }
}