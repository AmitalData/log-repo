declare var window: any;
import {Component} from '@angular/core';
import {Validator} from '../../../Validators/Validator';
import {BaseComponent} from '../BaseComponent';
import {AppTool, DateTool} from '../../../Tools';
import {FollowUpPM} from '../../../EntityPMs/FollowUpPM';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {QuoteFollowUpPM} from '../../../../Quote/EntityPMs/QuoteFollowUpPM';
import {ShipmentFollowUpPM} from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {EventTypeList} from '../../../EntityLists/EventTypeList';
import {EventTypeListService} from '../../../Services/StandardLists/EventTypeListService';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './AddFollowupComponent.html',
})

export class AddFollowupComponent extends BaseComponent {
    public EntityPM: FollowUpPM;
    public QuotePM: QuotePM = null;
    public ShipmentPM: ShipmentPM = null;
    public DataContext = this;
    public ObjectTableName: string = "FollowUp";
    public ValidationErrorsList: string[] = [];
    private AllEventTypes: EventTypeList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new FollowUpPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.OwnerUserId = SessionLocator.LoggedUserId;
        this.EntityPM.Done = false;
        this.EntityPM.IsNew = true;
        this.EntityPM.Deleted = false;
    }

    SetWindowArgs(args: any) {
        this.Date = args['Date'];
        this.QuotePM = args['QuotePM'];
        this.ShipmentPM = args['ShipmentPM'];
        this.EntityPM.LegType = args['FollowupLegType'];

        if (AppTool.IsNullOrEmpty(this.Date)) {
            this.Date = DateTool.GetCurrentDateAsUtc();
        }

        var myService = new EventTypeListService();
        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllEventTypes = myResponse.Result;
            }

            this.SetupEntity();
        });
    }

    SetupEntity() {
        var ObjectTableId: string = null;

        if (this.QuotePM) {
            ObjectTableId = window.ObjectTables.filter(x => x.Name === "Quote")[0].Id;
        }

        else if (this.ShipmentPM) {
            ObjectTableId = window.ObjectTables.filter(x => x.Name === "Shipment")[0].Id;
        }

        if (!AppTool.IsNullOrEmpty(ObjectTableId)) {
            var Code = this.GetEventTypeCode();
            var eventType = this.AllEventTypes.filter(f => f.ObjectTableId == ObjectTableId && f.Code == Code)[0];
            if (eventType) {
                this.EntityPM.EventTypeId = eventType.Id;
                this.EntityPM.EventTypeFollowUpName = eventType.FollowUpEnglishName;
            }
        }
    }
    GetEventTypeCode() {

        var myResult: string = null;

        if (this.EntityPM.LegType) {
            var legType = this.EntityPM.LegType.toLowerCase();

            if (legType.indexOf("departure") > -1) {
                switch (legType) {
                    case "precarriagedeparture": {
                        myResult = "PRCD";
                        break;
                    }

                    case "maincarriagedeparture":
                    case "transshipment1departure":
                    case "transshipment2departure":
                    case "transshipment3departure":
                        {
                            myResult = "DEP";
                            break;
                        }

                    case "oncarriagedeparture": {
                        myResult = "ONCD";
                        break;
                    }

                    case "warehouselegentry": {
                        myResult = "WHED";
                        break;
                    }

                    case "warehouselegrelease": {
                        myResult = "WHRD";
                        break;
                    }

                    default: {
                        if (legType.indexOf("pickup") > -1) {
                            myResult = "PICD";
                        }

                        else if (legType.indexOf("delivery") > -1) {
                            myResult = "DELD";
                        }

                        else if (legType.indexOf("emptycr") > -1) {
                            myResult = "DELD";
                        }

                        break;
                    }
                }
            }

            else if (legType.indexOf("arrival") > -1) {
                switch (legType) {
                    case "precarriagearrival": {
                        myResult = "PRCA";
                        break;
                    }

                    case "maincarriagearrival":
                    case "transshipment1arrival":
                    case "transshipment2arrival":
                    case "transshipment3arrival":
                        {
                            myResult = "ARR";
                            break;
                        }

                    case "oncarriagearrival": {
                        myResult = "ONCA";
                        break;
                    }

                    default: {
                        if (legType.indexOf("pickup") > -1) {
                            myResult = "RCS";
                        }

                        else if (legType.indexOf("delivery") > -1) {
                            myResult = "PIOD";
                        }

                        else if (legType.indexOf("emptycr") > -1) {
                            myResult = "PIOD";
                        }                     
                        break;
                    }
                }
            }
            else if (this.ShipmentPM) {
                if (legType.indexOf("customsclearancedate") > -1) {
                    if (this.ShipmentPM.DirectionId == "I") {
                        myResult = "ICUC";
                    }
                    else {
                        myResult = "ECUC";
                    }
                    this.Date = this.ShipmentPM.CustomsClearanceDate;
                }

                else if (legType.indexOf("freightrelease") > -1) {
                    myResult = "FRRL";
                    this.Date = this.ShipmentPM.FreightRelease
                }

                else if (legType.indexOf("terminalavailable") > -1) {
                    myResult = "TRAV";
                    this.Date = this.ShipmentPM.TerminalAvailable;
                }

                else if (legType.indexOf("warehouselegentry") > -1) {
                    myResult = "WHED";
                    this.Date = this.ShipmentPM.WarehouseLegExpectedEntryDate;
                }

                else if (legType.indexOf("warehouselegrelease") > -1) {
                    myResult = "WHRD";
                    this.Date = this.ShipmentPM.WarehouseLegExpectedReleaseDate;
                }

                else if (legType.indexOf("mawbobldate") > -1) {
                    myResult = "OBLD";
                    this.Date = this.ShipmentPM.MAWBOBLDate
                }

                else if (legType.indexOf("cutoffdate") > -1) {
                    myResult = "CUTO";
                    this.Date = this.ShipmentPM.CutoffDate
                }
            }    
        }

        return myResult;
    }

    get Date() { return this.EntityPM.Date; }
    set Date(value: Date) {
        if (this.EntityPM.Date != value) {
            this.EntityPM.Date = value;
        }
    }

    get OwnerUserId() { return this.EntityPM.OwnerUserId; }
    set OwnerUserId(value: string) {
        if (this.EntityPM.OwnerUserId != value) {
            this.EntityPM.OwnerUserId = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    NumericButtonClicked(isIncreas: boolean) {
        if (this.Date == null) {
            this.Date = DateTool.GetCurrentDateAsUtc();
        }

        else {
            var day: number = isIncreas ? 1 : -1;
            var newDate: Date = DateTool.GetDateParts(this.Date).DateObject;
            newDate.setUTCMilliseconds(0);
            newDate.setUTCSeconds(0);
            newDate.setUTCMinutes(0);
            newDate.setUTCHours(0);
            newDate.setUTCDate(newDate.getDate() + day);
            newDate.setUTCMonth(newDate.getMonth());
            newDate.setUTCFullYear(newDate.getFullYear());
            this.Date = newDate;
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (this.QuotePM) {
                var myQuoteFollowUpPM = new QuoteFollowUpPM(null);
                myQuoteFollowUpPM.Tenant = this.QuotePM.Tenant;
                myQuoteFollowUpPM.QuoteId = this.QuotePM.Id;
                myQuoteFollowUpPM.EventTypeId = this.EntityPM.EventTypeId;
                myQuoteFollowUpPM.EventTypeFollowUpName = this.EntityPM.EventTypeFollowUpName;
                myQuoteFollowUpPM.ManualActivatedFollowUp = this.EntityPM.ManualActivatedFollowUp;
                myQuoteFollowUpPM.Date = this.Date;
                myQuoteFollowUpPM.OwnerUserId = this.OwnerUserId;
                myQuoteFollowUpPM.Note = this.Notes;
                myQuoteFollowUpPM.Done = this.EntityPM.Done;
                myQuoteFollowUpPM.IsNew = this.EntityPM.IsNew;
                myQuoteFollowUpPM.LegType = this.EntityPM.LegType;
                this.QuotePM.AddQuoteFollowUpPM(myQuoteFollowUpPM);
            }

            else if (this.ShipmentPM) {
                var myShipmentFollowUpPM = new ShipmentFollowUpPM(null);
                myShipmentFollowUpPM.Tenant = this.ShipmentPM.Tenant;
                myShipmentFollowUpPM.ShipmentId = this.ShipmentPM.Id;
                myShipmentFollowUpPM.EventTypeId = this.EntityPM.EventTypeId;
                myShipmentFollowUpPM.EventTypeFollowUpName = this.EntityPM.EventTypeFollowUpName;
                myShipmentFollowUpPM.ManualActivatedFollowUp = this.EntityPM.ManualActivatedFollowUp;
                myShipmentFollowUpPM.Date = this.Date;
                myShipmentFollowUpPM.OwnerUserId = this.OwnerUserId;
                myShipmentFollowUpPM.Note = this.Notes;
                myShipmentFollowUpPM.Done = this.EntityPM.Done;
                myShipmentFollowUpPM.IsNew = this.EntityPM.IsNew;
                myShipmentFollowUpPM.LegType = this.EntityPM.LegType;
                this.ShipmentPM.AddShipmentFollowUp(myShipmentFollowUpPM);
            }

            this.CurrentSession.CloseCurrentWindowEmit("Ok");
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }
}

