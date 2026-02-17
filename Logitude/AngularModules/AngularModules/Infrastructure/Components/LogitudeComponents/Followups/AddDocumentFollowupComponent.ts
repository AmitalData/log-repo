import {Component, OnInit}  from '@angular/core';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {QuoteFollowUpPM} from '../../../../Quote/EntityPMs/QuoteFollowUpPM';
import {FollowUpPM} from '../../../../Infrastructure/EntityPMs/FollowUpPM';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,

    selector: 'DocumentFollowUp',
    templateUrl: './AddDocumentFollowupComponent.html',
})


export class AddDocumentFollowupComponent extends BaseComponent {
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    EntityPM: any;
    CurrentFollowUp: FollowUpPM = new FollowUpPM();
    ObjectTableName: string;
    IsStardLoadPage: boolean = false;
    ShipmentFollowUpEntity: ShipmentFollowUpPM;
    QuoteFollowUpEntity: any;
    validator: Validator;
    Date: Date;
    OwnerUserId: string;
    Note: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();



    }



    SetWindowArgs(args: any) {

        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.CurrentFollowUp = args.CurrentFollowUp;

        this.IsStardLoadPage = true;

    }


    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }



    NumericButtonClicked(isIncreas: boolean) {
        if (this.CurrentFollowUp.Date == null) {
            this.CurrentFollowUp.Date = DateTool.GetCurrentDateAsUtc();
        }

        else {
            var day: number = isIncreas ? 1 : -1;
            var newDate: Date = DateTool.GetDateParts(this.CurrentFollowUp.Date).DateObject;
            newDate.setUTCMilliseconds(0);
            newDate.setUTCSeconds(0);
            newDate.setUTCMinutes(0);
            newDate.setUTCHours(0);
            newDate.setUTCDate(newDate.getDate() + day);
            newDate.setUTCMonth(newDate.getMonth());
            newDate.setUTCFullYear(newDate.getFullYear());
            this.CurrentFollowUp.Date = newDate;
        }
    }

    SaveButtonClicked() {
        if (this.EntityPM) {

            this.ValidationErrorsList = [];
            Validator.TryValidateObject(this.CurrentFollowUp, "FollowUp", this.ValidationErrorsList);

            if (this.ValidationErrorsList.length == 0) {

                if (this.ObjectTableName == "Quote") {
                    var myQuoteFollowUpPM = new QuoteFollowUpPM(null);
                    myQuoteFollowUpPM.Tenant = this.EntityPM.Tenant;
                    myQuoteFollowUpPM.QuoteId = this.EntityPM.Id;
                    myQuoteFollowUpPM.IsNew = true;
                    myQuoteFollowUpPM.EventTypeId = this.CurrentFollowUp.EventTypeId;
                    myQuoteFollowUpPM.EventTypeFollowUpName = this.CurrentFollowUp.EventTypeFollowUpName;
                    myQuoteFollowUpPM.ManualActivatedFollowUp = this.CurrentFollowUp.ManualActivatedFollowUp;
                    myQuoteFollowUpPM.Date = this.CurrentFollowUp.Date;
                    myQuoteFollowUpPM.OwnerUserId = this.CurrentFollowUp.OwnerUserId;
                    myQuoteFollowUpPM.Note = this.CurrentFollowUp.Notes;
                    myQuoteFollowUpPM.Done = this.CurrentFollowUp.Done;
                    myQuoteFollowUpPM.DocumentTypeId = this.CurrentFollowUp.DocumentTypeId;
                    myQuoteFollowUpPM.Area = this.CurrentFollowUp.Area;
                    myQuoteFollowUpPM.ExternalDocumentId = this.CurrentFollowUp.ExternalDocumentId;
                    this.EntityPM.AddQuoteFollowUpPM(myQuoteFollowUpPM);
                }

                else if (this.ObjectTableName == "Shipment") {

                    var myShipmentFollowUpPM = new ShipmentFollowUpPM(null);
                    myShipmentFollowUpPM.Tenant = this.EntityPM.Tenant;
                    myShipmentFollowUpPM.ShipmentId = this.EntityPM.Id;
                    myShipmentFollowUpPM.IsNew = true;
                    myShipmentFollowUpPM.EventTypeId = this.CurrentFollowUp.EventTypeId;
                    myShipmentFollowUpPM.EventTypeFollowUpName = this.CurrentFollowUp.EventTypeFollowUpName;
                    myShipmentFollowUpPM.ManualActivatedFollowUp = this.CurrentFollowUp.ManualActivatedFollowUp;
                    myShipmentFollowUpPM.Date = this.CurrentFollowUp.Date;
                    myShipmentFollowUpPM.OwnerUserId = this.CurrentFollowUp.OwnerUserId;
                    myShipmentFollowUpPM.Note = this.CurrentFollowUp.Notes;
                    myShipmentFollowUpPM.Done = this.CurrentFollowUp.Done;
                    myShipmentFollowUpPM.DocumentTypeId = this.CurrentFollowUp.DocumentTypeId;
                    myShipmentFollowUpPM.Area = this.CurrentFollowUp.Area;
                    myShipmentFollowUpPM.ExternalDocumentId = this.CurrentFollowUp.ExternalDocumentId;
                    this.EntityPM.AddShipmentFollowUp(myShipmentFollowUpPM);

                }
                this.CurrentSession.FireEvent("FollowupsChanged");
                this.CurrentSession.CurrentWindow.Close("AddFollowUpSucceeded");


            }

        }

    }


}



