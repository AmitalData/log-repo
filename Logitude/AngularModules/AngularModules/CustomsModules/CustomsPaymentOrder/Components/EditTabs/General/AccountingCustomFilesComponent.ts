declare var window: any;

import { Directive, ChangeDetectorRef, Renderer, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { CommunicationLogStepDataViewModel } from '../../../../../InfrastructureModules/InfrastructureCommunications/Components/CommunicationLog/ViewModel/CommunicationLogStepDataViewModel';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'AccountingCustomFilesComponent',
    moduleId: module.id,
    templateUrl: './AccountingCustomFilesComponent.html',
})

export class AccountingCustomFilesComponent
    extends BaseComponent
    implements OnInit {

    public ObjectTableName: string = "Customs.CommunicationLog";
    public DataContext: any = this;
    public Tab: LogTab;
    public IsDisplayOnly: boolean = false;

    AccountingCustomFilesList: any[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.AccountingCustomFilesList = [];
    }

    ngOnInit() {

    }

    SetWindowArgs(accountingCustomFilesList: any[]) {
        accountingCustomFilesList.forEach((item) => {
            this.AccountingCustomFilesList.push(item);
        });
    }

    public SelectedFile: string;
    OnSelectedFile(myArgs: string) {
        this.SelectedFile = myArgs;
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedFile);
    }
}
