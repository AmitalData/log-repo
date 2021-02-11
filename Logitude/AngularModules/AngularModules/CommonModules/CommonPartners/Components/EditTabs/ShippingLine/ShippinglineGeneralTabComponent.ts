import { Component, ViewChild, ViewContainerRef } from '@angular/core';

import { ShippingLinePM } from '../../../../../Common/EntityPMs/ShippingLinePM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    
    templateUrl: './ShippinglineGeneralTabComponent.html',
})

export class ShippinglineGeneralTabComponent extends BaseComponent {
    public EntityPM: ShippingLinePM;
    public ObjectTableName: string = "ShippingLine";
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    public ScreenCode: string = "ShippingLine.GeneralTabScreen";
    public ImageId: string = "";
    public EntityId: string = "";
    public EntityName: string = "";
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ImageId = this.EntityPM.ImageDetailId;
        this.EntityName = "ShippingLine";
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
