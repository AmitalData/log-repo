import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {AirlineListService} from '../../../../Common/Services/StandardLists/AirlineListService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FlightsSchedulesRequestPM} from '../../../../Booking/EntityPMs/FlightsSchedulesRequestPM';
import {FVASimulatorWindowArgs} from '../../Args';
import {MessageSimulatingService, SimulatorArgs, SimulatorResult, SimulatorFVA} from '../../../../Infrastructure/Services/WebServices/MessageSimulatingService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './FVASimulatorComponent.html',
})

export class FVASimulatorComponent extends BaseComponent {
    public Recipient: string;
    public EntityPM: FlightsSchedulesRequestPM;
    public EntityId: string;
    public ObjectTableName: string = "FlightsSchedulesRequest";
    public DataContext: FVASimulatorComponent = this;
    public ValidationErrorsList: string[] = [];
    private myAirlineService: AirlineListService;
    private mySimulatingService: MessageSimulatingService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.mySimulatingService = new MessageSimulatingService();
    }

    SetWindowArgs(args: FVASimulatorWindowArgs) {
        this.EntityId = args.EntityPM.Id;
        this.Recipient = args.Recipient;
        this.Airline = args.AirlineList;
        var myEntity: FlightsSchedulesRequestPM = args.EntityPM;

        this.EntityPM = new FlightsSchedulesRequestPM();
        this.EntityPM.Tenant = myEntity.Tenant;
        this.EntityPM.CreateDate = myEntity.CreateDate;
        this.EntityPM.CreatedByUserId = myEntity.CreatedByUserId;
        this.EntityPM.AirlineId = myEntity.AirlineId;
        this.EntityPM.BookingId = myEntity.BookingId;
        this.EntityPM.ShipmentId = myEntity.ShipmentId;
        this.EntityPM.AnswerOSI = myEntity.AnswerOSI;
        this.EntityPM.AnswerReasonForNoReply = myEntity.AnswerReasonForNoReply;
        this.EntityPM.ETA = myEntity.ETA;
        this.EntityPM.ETD = myEntity.ETD;
        this.EntityPM.FromPortId = myEntity.FromPortId;
        this.EntityPM.ToPortId = myEntity.ToPortId;
        this.EntityPM.GrossWeight = myEntity.GrossWeight;
        this.EntityPM.GrossWeightUnitCode = myEntity.GrossWeightUnitCode;
        this.EntityPM.Volume = myEntity.Volume;
        this.EntityPM.VolumeUnitCode = myEntity.VolumeUnitCode;
        this.EntityPM.Tenant = myEntity.Tenant;
        this.EntityPM.RequestDetails = myEntity.RequestDetails;
        this.EntityPM.ResponseDate = myEntity.ResponseDate;
        this.EntityPM.StatusCode = myEntity.StatusCode;
        this.SetUIProperties();
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("AirlineId", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("AnswerOSI", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("AnswerReasonForNoReply", this.ObjectTableName, this.isReasonForNoReplayChecked);

        if (this.isReasonForNoReplayChecked) {
            this.UIProperties.SetRequired("AnswerReasonForNoReply", this.ObjectTableName, (AppTool.IsNullOrEmpty(this.AnswerReasonForNoReply) ? true : false));
        }

        else {
            this.UIProperties.SetRequired("ETD", this.ObjectTableName, (this.ETD == null ? true : false));

            var isWeightRequired = false;
            if (this.GrossWeight > 0) {
                if (AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                    isWeightRequired = true;
                }
            }

            var isVolumeRequired = false;
            if (this.Volume > 0) {
                if (AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
                    isVolumeRequired = true;
                }
            }

            this.UIProperties.SetRequired("Volume", this.ObjectTableName, isVolumeRequired);
            this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, isWeightRequired);
        }

        this.SetUIProperties_Airline();
    }
    private SetUIProperties_Airline() {
        var isFieldEnabled = true;

        if (this.isReasonForNoReplayChecked) {
            isFieldEnabled = false;
        }

        else {
            if (this.Airline != null) {                
                if (this.Airline.NoAvailabilityInFVAMessages) {
                    isFieldEnabled = false;
                }
            }
        }

        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, isFieldEnabled);
    }

    private Airline: AirlineList;
    get AirlineId() { return this.EntityPM.AirlineId; }
    set AirlineId(newValue: string) {
        if (this.EntityPM.AirlineId != newValue) {
            this.EntityPM.AirlineId = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.SetUIProperties_Airline();
            }

            else {
                if (this.myAirlineService == null) {
                    this.myAirlineService = new AirlineListService();
                }

                this.myAirlineService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        this.Airline = myResponse.Result;
                        this.SetUIProperties_Airline();
                    }
                });
            }
        }
    }

    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(newValue: string) {
        if (this.EntityPM.FromPortId != newValue) {
            this.EntityPM.FromPortId = newValue;
        }
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(newValue: string) {
        if (this.EntityPM.ToPortId != newValue) {
            this.EntityPM.ToPortId = newValue;
        }
    }

    get ETD() { return this.EntityPM.ETD; }
    set ETD(newValue: Date) {
        if (this.EntityPM.ETD != newValue) {
            this.EntityPM.ETD = newValue;
            this.SetUIProperties();
        }
    }

    get ETA() { return this.EntityPM.ETA; }
    set ETA(newValue: Date) {
        if (this.EntityPM.ETA != newValue) {
            this.EntityPM.ETA = newValue;
            this.SetUIProperties();
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = newValue;
            this.SetUIProperties();
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = newValue;
            this.SetUIProperties();
        }
    }

    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    set VolumeUnitCode(newValue: string) {
        if (this.EntityPM.VolumeUnitCode != newValue) {
            this.EntityPM.VolumeUnitCode = newValue;
            this.SetUIProperties();
        }
    }

    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    set GrossWeightUnitCode(newValue: string) {
        if (this.EntityPM.GrossWeightUnitCode != newValue) {
            this.EntityPM.GrossWeightUnitCode = newValue;
            this.SetUIProperties();
        }
    }

    get AnswerOSI() { return this.EntityPM.AnswerOSI; }
    set AnswerOSI(newValue: string) {
        if (this.EntityPM.AnswerOSI != newValue) {
            this.EntityPM.AnswerOSI = newValue;            
        }
    }

    get AnswerReasonForNoReply() { return this.EntityPM.AnswerReasonForNoReply; }
    set AnswerReasonForNoReply(newValue: string) {
        if (this.EntityPM.AnswerReasonForNoReply != newValue) {
            this.EntityPM.AnswerReasonForNoReply = newValue;
            this.SetUIProperties();
        }
    }

    private isReasonForNoReplayChecked: boolean;
    get IsReasonForNoReplayChecked() { return this.isReasonForNoReplayChecked; }
    set IsReasonForNoReplayChecked(newValue: boolean) {
        if (this.isReasonForNoReplayChecked != newValue) {
            this.isReasonForNoReplayChecked = newValue;
            this.SetUIProperties();
        }
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendClicked() {
        var isValid = this.Validate();

        if (isValid) {
            this.CurrentSession.StartBusyIndicator("Sending...");

            var simulatorArgs = new SimulatorArgs();
            simulatorArgs.Id = SessionLocator.Tenant;
            simulatorArgs.Tenant = SessionLocator.Tenant;
            simulatorArgs.MessageIdentifier = "FVA";
            simulatorArgs.AirlineId = this.AirlineId;
            simulatorArgs.EntityId = this.EntityId;
            simulatorArgs.EntityName = "FlightsSchedulesRequest";
            simulatorArgs.FVA = new SimulatorFVA();
            simulatorArgs.FVA.ETA = this.ETA;
            simulatorArgs.FVA.ETD = this.ETD;
            simulatorArgs.FVA.FromPortId = this.FromPortId;
            simulatorArgs.FVA.ToPortId = this.ToPortId;
            simulatorArgs.FVA.Volume = this.Volume;
            simulatorArgs.FVA.VolumeUnitCode = this.VolumeUnitCode;
            simulatorArgs.FVA.GrossWeight = this.GrossWeight;
            simulatorArgs.FVA.GrossWeightUnitCode = this.GrossWeightUnitCode;
            simulatorArgs.FVA.Recipient = this.Recipient;
            simulatorArgs.FVA.AnswerOSI = this.AnswerOSI;
            simulatorArgs.FVA.AnswerReasonForNoReply = this.AnswerReasonForNoReply;

            this.mySimulatingService.Simulate(simulatorArgs).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse != null) {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        var myResult: SimulatorResult = myResponse.Result;

                        if (myResult.IsValid) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Show("FVA message sent successfully");
                        }

                        else {
                            this.ValidationErrorsList = myResult.Errors;
                        }
                    }
                }
            });
        }
    }

    private ValidationText: string = null;
    private Validate() {
        var isValid = true;
        var errors: string[] = [];

        if (this.ValidationText == null) {
            this.ValidationText = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        }

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.ETD == null) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.ETD");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }

        if (this.Volume != null && AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.VolumeUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }

        if (this.GrossWeight != null && AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
            var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.GrossWeightUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }

        if (this.IsReasonForNoReplayChecked) {
            if (AppTool.IsNullOrEmpty(this.AnswerReasonForNoReply)) {
                var field = TextCodeTranslator.Translate(this.ObjectTableName + ".F.AnswerReasonForNoReply");
                errors.push(this.ValidationText.replace("%FieldName", field));
            }
        }

        this.ValidationErrorsList = errors;
        isValid = errors.length == 0 ? true : false;
        return isValid;
    }
}
