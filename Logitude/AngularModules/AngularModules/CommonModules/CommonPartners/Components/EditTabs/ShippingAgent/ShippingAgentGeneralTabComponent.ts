import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ShippingAgentPM } from '../../../../../Common/EntityPMs/ShippingAgentPM';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    
    templateUrl: './ShippingAgentGeneralTabComponent.html',
})

export class ShippingAgentGeneralTabComponent extends BaseComponent {
    public EntityPM: ShippingAgentPM;
    public ObjectTableName: string = "ShippingAgent";
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    public ScreenCode: string = "ShippingAgent.GeneralTabScreen";
    public ImageId: string = "";
    public EntityId: string = "";
    public EntityName: string = "";
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ImageId = this.EntityPM.ImageDetailId;
        this.EntityName = "ShippingAgent";
        this.EntityId = this.EntityPM.Id;
        
        this.RunComponent();
    }   

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();            
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

    ImageUploadedCompleted(code) {
        this.ImageId = code;
        this.EntityPM.ImageDetailId = code;
    }
}
