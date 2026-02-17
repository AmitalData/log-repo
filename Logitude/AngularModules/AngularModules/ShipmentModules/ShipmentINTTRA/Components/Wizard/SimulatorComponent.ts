import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {INTRAWebService, INTTRASimulator} from '../../../../Shipment/Services/INTRAWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './SimulatorComponent.html',
})

export class SimulatorComponent {
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

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

            var simulator = new INTTRASimulator();
            simulator.AnalyzeQueueId = this.AnalyzeQueueId;
            simulator.XmlString = this.XML_Text;

            var myService = new INTRAWebService();

            myService.Simulate(simulator).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var myResult: INTTRASimulator = myResponse.Result;

                    if (myResult.Success) {                       
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
    ReadFTPClicked() {

        var errors: string[] = [];

        this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicator("Reading FTP...");

        var myService = new INTRAWebService();
        myService.ReadFTP().subscribe((myResponse: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                var myResult: INTTRASimulator = myResponse.Result;

                if (myResult.Success) {
                    var messageWindow = new MessageWindow();

                    if (myResult.FilesCount == 0) {
                        messageWindow.Show("The FTP folder is empty");
                    }

                    else {
                        messageWindow.Show(myResult.FilesCount + " Files found");
                    }
                }

                else {
                    this.ValidationErrorsList = myResult.Errors;
                }
            }
        });        
    }
}
