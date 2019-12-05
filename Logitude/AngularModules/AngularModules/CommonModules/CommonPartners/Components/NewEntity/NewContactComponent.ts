import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {ContactPMService} from '../../../../Common/Services/StandardPMs/ContactPMService';
import {ContactInputTemplate, ContactInputTemplateArgs} from '../Templates/ContactInputTemplate';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './NewContactComponent.html',
})

export class NewContactComponent {
    public EntityPM: ContactPM;
    public ValidationErrorsList: string[] = [];
    private myService: ContactPMService;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.EntityPM = new ContactPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.myService = new ContactPMService();
        this.RunComponent();
    }

    private CustomerId: string = null
    private CardDependencyProperty1: string = null;
    private CustomerLable: string = null;
    private CardDependencyProperty1IsList: boolean = false;
    private ComponentName: string = null;
    SetWindowArgs(args: ContactInputTemplateArgs) {
        this.CustomerId = args.CustomerId;
        this.CardDependencyProperty1 = args.CardDependencyProperty1;
        this.CustomerLable = args.CustomerLable;
        this.CardDependencyProperty1IsList = args.CardDependencyProperty1IsList;
        this.ComponentName = args.ComponentName;
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

    private ContactTemplate: ContactInputTemplate;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/ContactInputTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.ContactTemplate = cmpRef.instance;
                var args = new ContactInputTemplateArgs();
                args.IsNewEntity = true;
                args.EntityPM = this.EntityPM;
                args.IsCustomerVisible = true;
                args.CustomerId = this.CustomerId;
                args.CardDependencyProperty1 = this.CardDependencyProperty1;
                args.CustomerLable = this.CustomerLable;
                args.ComponentName = this.ComponentName;
                this.ContactTemplate.InitTemplate(args);
            });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        this.ValidationErrorsList = this.ContactTemplate.Validate();

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
