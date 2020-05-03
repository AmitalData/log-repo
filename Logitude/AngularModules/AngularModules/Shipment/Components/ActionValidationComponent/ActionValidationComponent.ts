import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';


import {AppTool} from '../../../Infrastructure/Tools';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CCSWebService, CCSResult, AWBResultClass, FHLShipmentValidator} from '../../../Infrastructure/Services/WebServices/CCSWebService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    

    templateUrl: './ActionValidationComponent.html',
})

export class ActionValidationComponent {
    private Tenant: number;
    private entityPM: ShipmentPM;
  
    public ValidationErrorsList: string[];
    public ValidationWarningsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
    }

    public SetWindowArgs(args: any) {
        this.Tenant = args.EnttiyPM.Tenant;
        this.entityPM = args.EnttiyPM;

        this.ValidationErrorsList = args.ErrorsList;
        this.ValidationWarningsList = args.WarningsList;
         
        this.InitializeComponent();
    }

    private InitializeComponent() {
       
         
        this.SetConfirmButton();

      
    }
 

    // Warnings
    private SetWarnings() {
        var warnings: string[] = [];
 

        this.ValidationWarningsList = warnings;
    }

  
    public IsConfirmButtonVisible: boolean = false;
    private SetConfirmButton() {
      
 
        if (this.ValidationErrorsList.length == 0) {
            this.IsConfirmButtonVisible = true;
        }
         
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ConfirmClicked() {

       // this.CurrentSession.StartBusyIndicator("Saving...");
 
        var errors: string[] = [];//this.ValidateSending();

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            //this.myCCSWebService.GetSendingValidations(this.entityPM.Id, this.SelectedRecipient, this.isSendingFHLs, this.isSendingCargonaut, this.isSendingDEXX, this.entityPM.MainCarriageCarrierId).subscribe((myResponse: ServiceResponse) => {

            //    if (myResponse == null) {
            //        this.CurrentSession.StopBusyIndicator();
            //    }

            //    else if (myResponse.HasError) {
            //        this.ValidationErrorsList = myResponse.ErrorsArray;
            //        this.CurrentSession.StopBusyIndicator();
            //    }

            //    else {
            //        var myResult: AWBResultClass = myResponse.Result;

            //        this.myValidationResultClass = myResult;

            //        if (myResult.IsValid) {
            //            this.InitializeSendingData();
            //        }

            //        else {

            //            this.ValidationErrorsList = myResult.ErrorsList;

            //            if (myResult.HasStockErrors) {
            //                this.StockErrorMessage = "You are trying to send (" + myResult.SendingCount + ") messages, your remaining stock is (" + myResult.StockRemainingBefore + ") which is insufficient for this operation. Please purchase another messaging stock via the link";
            //                this.StockErrorIsVisible = true;
            //            }

            //            this.SendingResultForeground = this.redForeground;

            //            if (this.entityPM.ShipmentLevelCode == "C" && this.isSendingFHLs) {
            //                this.SendingResultMessage = "Error sending FHL(s)";
            //            }

            //            else {
            //                this.SendingResultMessage = "Error sending " + this.MessageType;
            //            }

            //            this.CurrentSession.StopBusyIndicator();
            //        }
            //    }
            //});
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }
}
