import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {INTRAWebService, INTRAResult} from '../../../../Shipment/Services/INTRAWebService';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './WizardComponent.html',
})

export class WizardComponent extends BaseComponent {
    public EntityPM: ShipmentPM = null;
    public DataContext = this;
    public ObjectTableName: string = "Shipment";
    public ShipmentId: string = null;
    public ResultMessage: string = null;
    public ValidationErrorsList: string[] = [];
    public IsValid: boolean = true;
    public IsLimited: boolean = false;
    public IsDevelopment: boolean = false;
    public SimulatorIsVisible: boolean = false;
    private myService: INTRAWebService;
    private entityArgs: EntityArgs;
    constructor() {
        super();
        this.myService = new INTRAWebService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['Shipment'];
        this.entityArgs = args['EntityArgs'];
        this.ShipmentId = this.EntityPM.Id;
        this.IsDevelopment = FeatureLocator.IsPackage_DVMT();
        this.SimulatorIsVisible = FeatureLocator.HasFeaturePermession(this.ObjectTableName, "INTTRASimulator") ? true : false;
        this.SetSendingLimitation(this.EntityPM.INTTRASIStatusCode);
        this.Clone();

        if (!this.IsLimited) {
            this.myService.Validate(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var myResult: INTRAResult = myResponse.Result;

                    if (!this.IsDevelopment) {
                        this.IsLimited = myResult.IsLimited;
                    }

                    if (myResult.Errors.length > 0) {
                        SessionLocator.CurrentSession.StopBusyIndicator();
                        this.ValidationErrorsList = myResult.Errors;
                    }

                    else if (myResult.IsCarrierRegisteredToINTTRA == false) {
                        this.ValidationErrorsList.push("Shipping line is not Registered To INTTRA");
                    }

                    else if (myResult.IsCarrierRegisteredToBranch == false) {
                        this.ValidationErrorsList.push("Shipping line is not Registered To Branch");
                    }

                    else {
                        this.ResultMessage = "Shipping instructions are ready for sending to INTTRA";
                    }
                }
            });
        }
    }
    SetSendingLimitation(StatusCode: string) {
        var isLimited: boolean = false;

        if (!this.IsDevelopment) {
            if (!AppTool.IsNullOrEmpty(StatusCode)) {
                switch (StatusCode) {
                    case "NSEN":
                    case "RJIN":
                        {

                            break;
                        }

                    default: {
                        isLimited = true;
                        break;
                    }
                }
            }
        }

        this.IsLimited = isLimited;
    }

    get EmergencyContactId() { return this.EntityPM.EmergencyContactId; }
    set EmergencyContactId(value: string) {
        if (this.EntityPM.EmergencyContactId != value) {
            this.EntityPM.EmergencyContactId = value;
        }
    }

    get INTTRAContractNumber() { return this.EntityPM.INTTRAContractNumber; }
    set INTTRAContractNumber(value: string) {
        if (this.EntityPM.INTTRAContractNumber != value) {
            this.EntityPM.INTTRAContractNumber = value;
        }
    }

    get INTTRAInstructions() { return this.EntityPM.INTTRAInstructions; }
    set INTTRAInstructions(value: string) {
        if (this.EntityPM.INTTRAInstructions != value) {
            this.EntityPM.INTTRAInstructions = value;
        }
    }

    get INTTRAComments() { return this.EntityPM.INTTRAComments; }
    set INTTRAComments(value: string) {
        if (this.EntityPM.INTTRAComments != value) {
            this.EntityPM.INTTRAComments = value;
        }
    }

    get SIHasAttachList() { return this.EntityPM.SIHasAttachList; }
    set SIHasAttachList(value: boolean) {
        if (this.EntityPM.SIHasAttachList != value) {
            this.EntityPM.SIHasAttachList = value;
        }
    }

    get INTTRADocumentQTY() { return this.EntityPM.INTTRADocumentQTY; }
    set INTTRADocumentQTY(value: number) {
        if (this.EntityPM.INTTRADocumentQTY != value) {
            this.EntityPM.INTTRADocumentQTY = value;
        }
    }

    get INTTRADocumentTypeCode() { return this.EntityPM.INTTRADocumentTypeCode; }
    set INTTRADocumentTypeCode(value: string) {
        if (this.EntityPM.INTTRADocumentTypeCode != value) {
            this.EntityPM.INTTRADocumentTypeCode = value;
        }
    }

    get INTTRAIsFreighted() { return this.EntityPM.INTTRAIsFreighted; }
    set INTTRAIsFreighted(value: boolean) {
        if (this.EntityPM.INTTRAIsFreighted != value) {
            this.EntityPM.INTTRAIsFreighted = value;
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    CloseButtonClicked() {
        this.RejectChanges();
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    SendButtonClicked() {
        this.ResultMessage = null;
        this.ValidationErrorsList = [];

        if (this.EntityPM.IsDirty) {
            this.Save();
        }

        else {
            this.Send();
        }
    }
    Save() {
        if (!this.SaveCompletedEvent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = null;
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.Clone();
                    this.Send();
                }

                else {
                    this.ValidationErrorsList = this.entityArgs.EditComponent.ValidationErrorsList;
                }

                AppTool.KillEventEmitter(this.SaveCompletedEvent);
                this.SaveCompletedEvent = null;
            });

            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    Send() {
        SessionLocator.CurrentSession.StartBusyIndicator("Sending...");

        this.myService.Validate(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                SessionLocator.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                var myResult: INTRAResult = myResponse.Result;

                if (!this.IsDevelopment) {
                    this.IsLimited = myResult.IsLimited;
                }

                if (myResult.Errors.length > 0) {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    this.ValidationErrorsList = myResult.Errors;
                }

                else if (myResult.IsCarrierRegisteredToINTTRA == false) {
                    SessionLocator.CurrentSession.StopBusyIndicator();

                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show("Shipping line is not Registered To INTTRA");
                }

                else if (myResult.IsCarrierRegisteredToBranch == false) {
                    SessionLocator.CurrentSession.StopBusyIndicator();

                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show("Shipping line is not Registered To Branch");
                }

                else {
                    if (this.IsLimited) {
                        SessionLocator.CurrentSession.StopBusyIndicator();

                        var messageWindow: MessageWindow = new MessageWindow();
                        messageWindow.Show("SI re-sending to INTTRA is not allowed");
                    }

                    else {
                        this.myService.Send(this.ShipmentId).subscribe((myResponse: ServiceResponse) => {
                            if (myResponse.HasError) {
                                this.ValidationErrorsList = myResponse.ErrorsArray;
                                SessionLocator.CurrentSession.StopBusyIndicator();
                            }

                            else {
                                var myResult: INTRAResult = myResponse.Result;

                                if (myResult.Errors.length > 0) {
                                    this.ValidationErrorsList = myResult.Errors;
                                    SessionLocator.CurrentSession.StopBusyIndicator();
                                }

                                else {

                                    if (!this.IsDevelopment) {
                                        this.IsLimited = true;
                                    }

                                    this.ResultMessage = "Message has been sent Successfully";
                                    SessionLocator.CurrentSession.StopBusyIndicator();

                                    if (!this.LoadCompletedEvent) {
                                        this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                                            if (isLoadSuccess) {
                                                this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                                                this.Clone();
                                            }

                                            else {
                                                this.ValidationErrorsList = this.entityArgs.EditComponent.ValidationErrorsList;
                                            }

                                            AppTool.KillEventEmitter(this.SaveCompletedEvent);
                                            this.SaveCompletedEvent = null;
                                        });                                       

                                        this.entityArgs.EditComponent.ReloadEntityPM();
                                    }    
                                }
                            }
                        });
                    }
                }
            }
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('EmergencyContactId');
        this.myCloner.AddField('INTTRAContractNumber');
        this.myCloner.AddField('INTTRAInstructions');
        this.myCloner.AddField('INTTRAComments');
        this.myCloner.AddField('SIHasAttachList');
        this.myCloner.AddField('INTTRADocumentQTY');
        this.myCloner.AddField('INTTRADocumentTypeCode');

        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    SimulateClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "INTTRA Simulator";
        logWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Wizard/SimulatorComponent');
    }
}
