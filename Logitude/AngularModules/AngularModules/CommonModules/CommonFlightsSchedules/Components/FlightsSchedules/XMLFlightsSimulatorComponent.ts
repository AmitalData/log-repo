import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {FlightsSchedulesRequestPM} from '../../../../Booking/EntityPMs/FlightsSchedulesRequestPM';
import {FlightsSchedulesRequestPMService} from '../../../../Booking/Services/StandardPMs/FlightsSchedulesRequestPMService';
import {FlightsSchedulesComponent} from './FlightsSchedulesComponent';
import {FVRWebService, FVASimulatorResult} from '../../../../Infrastructure/Services/WebServices/FVRWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './XMLFlightsSimulatorComponent.html',
})

export class XMLFlightsSimulatorComponent {
    private BookingId: string = null;
    private ShipmentId: string = null;
    public ValidationErrorsList: string[] = [];
    private myFVRWebService: FVRWebService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    private FatherComponent: FlightsSchedulesComponent;
    SetWindowArgs(args: any) {
        this.BookingId = args['BookingId'];
        this.ShipmentId = args['ShipmentId'];
        this.FatherComponent = args['FatherComponent'];
    }

    public IsFNA: boolean = false;
    public RequestId: string = null;
    public XML_Text: string = null;

    private useRequestId: boolean = false;
    get UseRequestId() { return this.useRequestId; }
    set UseRequestId(newValue: boolean) {
        if (this.useRequestId != newValue) {
            this.useRequestId = newValue;

            if (newValue) {
                this.XML_Text = null;
            }

            else {
                this.RequestId = null;
            }

            this.IsFNA = false;
        }
    }

    CancelClicked() {
        this.Close();
    }

    OkClicked() {
        var errors: string[] = [];

        if (this.UseRequestId) {
            if (AppTool.IsNullOrEmpty(this.RequestId)) {
                errors.push("Please fill your Request Id");
            }
        }

        else {
            if (AppTool.IsNullOrEmpty(this.XML_Text)) {
                errors.push("Please fill your XML body");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Simulating...");

            if (this.UseRequestId) {
                this.SimulateRequest();
            }

            else {
                this.SimulateXML();
            }
        }
    }

    private entityPMService: FlightsSchedulesRequestPMService;
    private SimulateRequest() {
        if (this.entityPMService == null) {
            this.entityPMService = new FlightsSchedulesRequestPMService();
        }

        this.entityPMService.get(this.RequestId).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                var entityPM = myResponse.Result;

                if (entityPM == null) {
                    var errors: string[] = [];
                    errors.push("This Request not exists");
                    this.ValidationErrorsList = errors;
                }

                else {
                    var myResult = new FVASimulatorResult();
                    myResult.ETD = entityPM.ETD;
                    myResult.AirlineId = entityPM.AirlineId;
                    myResult.FromPortId = entityPM.FromPortId;
                    myResult.ToPortId = entityPM.ToPortId;
                    myResult.RequestId = entityPM.Id;

                    this.FatherComponent.UpgradeFromXML(myResult);
                    this.Close();
                }
            }
        });
    }

    private SimulateXML() {
        if (this.myFVRWebService == null) {
            this.myFVRWebService = new FVRWebService();
        }

        this.myFVRWebService.SimulateXML(this.XML_Text, this.ShipmentId, this.BookingId, this.IsFNA).subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator(); 

            if (myResponse != null) {

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var myResult: FVASimulatorResult = myResponse.Result;

                    if (myResult.IsValid) {
                        this.FatherComponent.UpgradeFromXML(myResult);
                        this.Close();
                    }

                    else {
                        if (!myResult.IsValidXML) {
                            this.ValidationErrorsList = [];
                            this.ValidationErrorsList.push("Invalid FVA XML");
                        }

                        else if (!myResult.IsValid) {
                            this.ValidationErrorsList = myResult.Errors;
                        }
                    }
                }
            }
        });
    }

    private Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
