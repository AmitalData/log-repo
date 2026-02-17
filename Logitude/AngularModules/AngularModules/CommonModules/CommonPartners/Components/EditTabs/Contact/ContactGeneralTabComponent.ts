import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {ContactPM} from '../../../../../Common/EntityPMs/ContactPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ContactInputTemplate, ContactInputTemplateArgs} from '../../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';

@Component({
    moduleId: module.id,
    templateUrl: './ContactGeneralTabComponent.html',
})

export class ContactGeneralTabComponent {
    public EntityPM: ContactPM;
    public ObjectTableName: string = "Contact";
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
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

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    } 

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/ContactInputTemplate", this.viewContainerRef)
            .then(cmpRef => {
                var myTemplate: ContactInputTemplate = cmpRef.instance;

                var args = new ContactInputTemplateArgs();
                args.EntityPM = this.EntityPM;
                myTemplate.InitTemplate(args);
            });
    }
}