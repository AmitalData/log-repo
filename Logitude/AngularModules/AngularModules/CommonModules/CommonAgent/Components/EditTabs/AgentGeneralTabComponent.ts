import { Component, OnInit, ViewChild, ViewContainerRef, OnDestroy} from '@angular/core';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './AgentGeneralTabComponent.html',
})

export class AgentGeneralTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: AgentPM;
    public ObjectTableName: string = "Agent";
    public TenantPM: TenantPM;
    public LabelColumnWidth: number = 100;
    public ControlColumnWidth: number = 200;
    public DataContext: AgentGeneralTabComponent = this;
    private ScreenCode: string = "Agent.AdditionalFields";
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.TenantPM = SessionLocator.TenantPM;
        this.RunComponent();       
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
            this.Listen();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
            });
    }

    public isRAFieldsVisibile: boolean = false;
    SetUIProperties() {

        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        //var isRAFieldsVisibile: boolean = false;
        if (this.TenantPM.RegulatedAgentRegimeActivated) {
            this.isRAFieldsVisibile = true;
        }

        this.UIProperties.SetVisibility("RegulatedAgentCode", this.ObjectTableName, this.isRAFieldsVisibile);
    }

    // Properties 
    get Code() { return this.EntityPM.Code; }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get IATACode() { return this.EntityPM.IATACode; }
    set IATACode(newValue: string) {
        if (this.EntityPM.IATACode != newValue) {
            this.EntityPM.IATACode = newValue;
        }
    }

    get CASSCode() { return this.EntityPM.CASSCode; }
    set CASSCode(newValue: string) {
        if (this.EntityPM.CASSCode != newValue) {
            this.EntityPM.CASSCode = newValue;
        }
    }

    get RegulatedAgentCode() { return this.EntityPM.RegulatedAgentCode; }
    set RegulatedAgentCode(newValue: string) {
        if (this.EntityPM.RegulatedAgentCode != newValue) {
            this.EntityPM.RegulatedAgentCode = newValue;
        }
    }

    get Website() { return this.EntityPM.Website; }
    set Website(newValue: string) {
        if (this.EntityPM.Website != newValue) {
            this.EntityPM.Website = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }
}
