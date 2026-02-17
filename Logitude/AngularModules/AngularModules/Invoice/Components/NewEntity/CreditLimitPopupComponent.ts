import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentDomainService} from '../../../Shipment/Services/ShipmentDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './CreditLimitPopupComponent.html',
})

export class CreditLimitPopupComponent {
    public IsError: boolean = false;
    public ErrorMessage: string;
    public WarningMessage: string;
    public OkButtonLabel: string;
    public IsOkButtonVisible: boolean = false;
    public LocalCurrencyCode: string;
    public HasCreditOverrideFeature: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.HasCreditOverrideFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Override");
    }

    SetWindowArgs(args: any) {
        var errors: string[] = args['Errors'];
        var warnings: string[] = args['Warnings'];
        var shipmentId: string = args['ShipmentId'];
        var isBlockingShipment: boolean = args['IsBlockingShipment'];

        if (errors.length > 0) {
            this.IsError = true;
            this.ErrorMessage = errors[0];
            this.OkButtonLabel = TextCodeTranslator.Translate("General.B.Approve");

            if (this.HasCreditOverrideFeature) {
                this.IsOkButtonVisible = true;
            }
        }

        else {
            this.WarningMessage = warnings[0];
            this.OkButtonLabel = TextCodeTranslator.Translate("General.B.Continue");
            this.IsOkButtonVisible = true;
        }

        if (isBlockingShipment == true) {
            var myService = new ShipmentDomainService();
            myService.BlockNewARInvoice(shipmentId).subscribe((myResponse: ServiceResponse) => {

            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }
}
