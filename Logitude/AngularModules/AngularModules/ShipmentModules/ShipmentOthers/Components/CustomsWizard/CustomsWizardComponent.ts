import {Component} from '@angular/core';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {CustomsWizardArgs} from '../../../../Shipment/Args';
import {ABMWebService, ABMResult} from '../../../../Infrastructure/Services/WebServices/ABMWebService';
@Component({
    selector: 'CustomsWizardComponent',
    moduleId: module.id,
    templateUrl: './CustomsWizardComponent.html',
})

export class CustomsWizardComponent {
    public EntityPM: ShipmentPM;
    public ValidationErrorsList: string[] = [];
    private myABMWebService: ABMWebService;
    constructor() {
        this.myABMWebService = new ABMWebService();
    }

    SetWindowArgs(windowArgs: CustomsWizardArgs) {
        this.EntityPM = windowArgs.EntityPM;
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    public IsSendingMessageVisible: boolean = false;
    public IsSendButtonEnabled: boolean = true;
    public CancelButtonContent: string = "Cancel";
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            SessionLocator.CurrentSession.StartBusyIndicator("Sending in Progress..");

            this.myABMWebService.Send(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse == null) {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }

                else if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }

                else {
                    var myResult: ABMResult = myResponse.Result;
                    
                    if (myResult == null) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        SessionLocator.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        this.IsSendingMessageVisible = true;
                        this.IsSendButtonEnabled = false;
                        this.CancelButtonContent = "Close";
                        SessionLocator.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    }
}