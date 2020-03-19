import { Component } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { MessageSimulatingService, SimulatorArgs, SimulatorResult } from '../../../Infrastructure/Services/WebServices/MessageSimulatingService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './AnalyzeChampXMLComponent.html',
})

export class AnalyzeChampXMLComponent {
    public ValidationErrorsList: string[] = [];
    private myService: MessageSimulatingService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new MessageSimulatingService();
    }

    public AnalyzeQueueId: string = null;
    public XML_Text: string = null;

    private useAnalyzeQueueId: boolean = false;
    get UseAnalyzeQueueId() { return this.useAnalyzeQueueId; }
    set UseAnalyzeQueueId(value: boolean) {
        if (this.useAnalyzeQueueId != value) {
            this.useAnalyzeQueueId = value;

            if (value) {
                this.XML_Text = null;
            }

            else {
                this.AnalyzeQueueId = null;
            }
        }
    }

    CancelClicked() {
        this.Close();
    }
    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
    SimulateClicked() {
        var errors: string[] = [];

        if (this.UseAnalyzeQueueId) {
            if (AppTool.IsNullOrEmpty(this.AnalyzeQueueId)) {
                errors.push("Please fill your Analyze Queue Id");
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

            var simulator = new SimulatorArgs();
            simulator.Tenant = SessionLocator.Tenant;
            simulator.AnalyzeQueueId = this.AnalyzeQueueId;
            simulator.XmlText = this.XML_Text;
            simulator.MessageIdentifier = "XML";
            simulator.IsChampSimulator = true;

            if (this.UseAnalyzeQueueId) {
                simulator.MessageIdentifier = "AnalyzeQueueId";
            }

            if (!SessionLocator.IsProduction) {
                simulator.IsLocalAnalyze = true;
            }

            this.myService.Simulate(simulator).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var myResult: SimulatorResult = myResponse.Result;

                    if (myResult.IsValid) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Show("Simulated Successfully");
                    }

                    else {
                        this.ValidationErrorsList = myResult.Errors;
                    }
                }
            });
        }
    }
}
