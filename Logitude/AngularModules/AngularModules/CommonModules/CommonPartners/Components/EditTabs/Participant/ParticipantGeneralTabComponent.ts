import {Component,ViewChild,ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ParticipantPM} from '../../../../../Common/EntityPMs/ParticipantPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    selector: 'NewCurrencyComponent',
    moduleId: module.id,
    templateUrl: './ParticipantGeneralTabComponent.html',
})

export class ParticipantGeneralTabComponent extends BaseComponent {

    public EntityPM: ParticipantPM;
    public DataContext: ParticipantGeneralTabComponent = this;
    public ObjectTableName: string = "Participant";
    private ScreenCode = "Participant.AdditionalFields";
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.SetUIProperties();
        this.RunComponent();
    }
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;

    SetUIProperties() {
        this.UIProperties.SetEnabled("IsDirect", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RegistrationRequested", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
    }

    RunComponent() {
        if (this.viewContainerRef) {        
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {

                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
            });
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

    public get Code() { return this.EntityPM.Code; }
    public get RegistrationRequested() { return this.EntityPM.RegistrationRequested; }
    public set RegistrationRequested(value: boolean) { if (this.EntityPM.RegistrationRequested != value) this.EntityPM.RegistrationRequested = value; }
    public set IsDirect(value: boolean) { if (this.EntityPM.IsDirect != value) this.EntityPM.IsDirect = value; }
    public get IsDirect() { return this.EntityPM.IsDirect; }


    public get EnglishName() { return this.EntityPM.EnglishName; }
    public set EnglishName(value: string) { if (this.EntityPM.EnglishName != value) this.EntityPM.EnglishName = value; }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) { if (this.EntityPM.LocalName != value) this.EntityPM.LocalName = value; }

    public get TTY() { return this.EntityPM.TTY; }
    public set TTY(value: string) { if (this.EntityPM.TTY != value) this.EntityPM.TTY = value; }

    public get Registered() { return this.EntityPM.Registered; }
    public set Registered(value: boolean) { if (this.EntityPM.Registered != value) this.EntityPM.Registered = value; }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) { if (this.EntityPM.InActive != value) this.EntityPM.InActive = value; }
}
