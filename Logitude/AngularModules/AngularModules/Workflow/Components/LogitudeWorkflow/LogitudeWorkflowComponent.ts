import { Component, ComponentRef, ElementRef, OnDestroy, OnInit, ViewChild } from "@angular/core";
import ReactFlowModeler from "logitude-workflow";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { WorkFlowPM } from "Workflow/EntityPMs/WorkFlowPM";
import { WorkFlowPMService } from "Workflow/Services/StandardPMs/WorkFlowPMService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
//import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

//const logitudeWorkflowComponentContainer = "LogitudeWorkflowComponentContainer";

@Component({
    // selector: "logitude-workflow",
    // template: `
    // <style>
    //   .logitude-workflow-component-container {
    //     width: 100%;
    //     height: 100%;
    //   }
    // </style>
    // <div #${logitudeWorkflowComponentContainer} class="logitude-workflow-component-container"></div>
    // `
    templateUrl: "./LogitudeWorkflowComponent.html"
})

export class LogitudeWorkflowComponent extends BaseComponent implements OnInit, OnDestroy {

    @ViewChild("logitudeWorkflowComponentContainer", { static: false }) containerRef: ElementRef;

    public ComponentRef: ComponentRef<LogitudeWorkflowComponent>;

    public EntityPM: WorkFlowPM = null;
    public EntityId: string;

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public WidthBusyIndicator: number = 200;

    public BackButtonLable: string = "Workflows";

    public WorkflowName: string;

    public ValidationErrorsList: string[] = [];

    public RequestedFlow: any = null;

    public EventKeyPostfix: string = (Date.now())?.toString();
    public ReturnPropertiesDataEventKey: string = "returnPropertiesDataEventKey_" + this.EventKeyPostfix;
    public RequestFlowEventKey: string = "requestFlowEventKey_" + this.EventKeyPostfix;

    public WorkFlowPMService: WorkFlowPMService;

    //private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
    }

    public Run(args: any) {
        this.initializeServices();
        this.EntityId = args['EntityId'];
    }

    ngOnInit() {
        this.loadWorkflow();
    }

    // ngOnChanges() {
    //     this.render();
    // }

    // ngAfterViewInit() {
    //     this.render();
    // }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
    }

    initializeServices() {
        this.WorkFlowPMService = new WorkFlowPMService();
    }

    loadWorkflow() {
        if (this.EntityId) {
            this.startBusyIndicator("Loading ...");
            this.WorkFlowPMService.get(this.EntityId).subscribe((serviceResponse: ServiceResponse) => {
                if (!serviceResponse.HasError) {
                    this.EntityPM = serviceResponse.Result;
                    this.WorkflowName = this.EntityPM.Name;
                    this.renderReactFlowModeler();
                    this.stopBusyIndicator();
                }
            });
        }
    }

    startBusyIndicator(message: string) {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    renderReactFlowModeler() {
        if (this.EntityPM) {
            let flowObject = this.EntityPM.FlowJson && this.EntityPM.FlowJson !== "" ? JSON.parse(this.EntityPM.FlowJson) : null;
            let props = {
                flow: flowObject,
                returnPropertiesDataEventKey: this.ReturnPropertiesDataEventKey,
                requestFlowEventKey: this.RequestFlowEventKey,
                openPropertiesEvent: this.openPropertiesEvent,
                returnFlowEvent: this.returnFlowEvent,
            };
            ReactDOM.render(React.createElement(ReactFlowModeler, props), this.containerRef.nativeElement);
        }
    }

    openPropertiesEvent = (openPropertiesEventObject: any) => {
        if (openPropertiesEventObject) {
            this.handleOpenPropertiesEvent(openPropertiesEventObject);
        }
    }

    returnFlowEvent = (flow: any) => {
        this.RequestedFlow = flow;
    }

    handleOpenPropertiesEvent = (openPropertiesEventObject: any) => {
        let nodePropertiesComponentPath = this.getNodePropertiesComponentPath(openPropertiesEventObject.nodeType);
        if (nodePropertiesComponentPath) {
            let nodePropertiesWindow = this.buildNodePropertiesWindow(openPropertiesEventObject);
            nodePropertiesWindow.Show(nodePropertiesComponentPath);
            nodePropertiesWindow.WindowClosed.subscribe((data: any) => { this.handleNodePropertiesWindowClosed(data); });
        }
    }

    getNodePropertiesComponentPath = (nodeType: string) => {
        let nodePropertiesComponentPath = "./Workflow/Components/NodeProperties/";
        switch (nodeType) {
            case "conditionNode":
                return (nodePropertiesComponentPath + "ConditionNodePropertiesComponent");
            case "loopNode":
                return (nodePropertiesComponentPath + "LoopNodePropertiesComponent");
            case "setValueNode":
                return (nodePropertiesComponentPath + "SetValueNodePropertiesComponent");
            default:
                return null;
        }
    }

    buildNodePropertiesWindow = (openPropertiesEventObject: any) => {
        let nodePropertiesWindow = new LogitudeWindow();
        let nodePropertiesWindowArgs: any = {
            Data: JSON.parse(JSON.stringify(openPropertiesEventObject.nodeData))
        };

        nodePropertiesWindow.Width = 600;
        nodePropertiesWindow.Height = 500;
        nodePropertiesWindow.RTL = false;
        nodePropertiesWindow.Title = (openPropertiesEventObject.isNewNode ? "New " : "Edit ") + openPropertiesEventObject.nodeLabel + " Element";
        nodePropertiesWindow.WindowArgs = nodePropertiesWindowArgs;

        return nodePropertiesWindow;
    }

    handleNodePropertiesWindowClosed = (data: any) => {
        if (data) {
            document.dispatchEvent(new CustomEvent(this.ReturnPropertiesDataEventKey, { detail: data }));
        }
    }

    requestFlow = () => {
        document.dispatchEvent(new CustomEvent(this.RequestFlowEventKey));
    }

    backButtonClicked() {
        console.log("backButtonClicked");
        // this.requestFlow();
        // console.log(this.RequestedFlow);

        
        // if (this.HasChanges) {
        //     var confirmWindow = new ConfirmWindow();
        //     confirmWindow.Width = 450;
        //     confirmWindow.Height = 190;
        //     confirmWindow.ShowCancelButton = true;
        //     confirmWindow.NoButtonText = "Don't Save";
        //     confirmWindow.YesButtonText = "Save ";
        //     confirmWindow.CancelButtonText = "Cancel";
        //     confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        //     confirmWindow.Show("This Report has unsaved changes. Do you want to save it?");
        //     confirmWindow.WindowClosed.subscribe((event: any) => {
        //         if (confirmWindow.Yes) {
        //             // save
        //             this.SaveBIReport(true);
        //         }
        //         else if (confirmWindow.No) {
        //             if (this.ComponentRef) {
        //                 this.BackCompleted.emit(false);
        //                 this.ComponentRef.destroy();
        //             }
        //         }
        //         else if (confirmWindow.Cancel) {
        //             // nth
        //         }
        //     });
        // }
        // else {
        //     if (this.ComponentRef) {
        //         this.BackCompleted.emit(false);
        //         this.CurrentSession.FireEvent("ReloadAllList");
        //         this.ComponentRef.destroy();
        //     }
        // }
    }

    editWorkflowClicked() {
        console.log("editWorkflowClicked");
        // if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //     SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        //         .then(cmpRef => {
        //             cmpRef.instance.ComponentRef = cmpRef;
        //             cmpRef.instance.Run({ EntityId: this.EntityPM.Id, ObjectTableName: 'BIReport' });
        //             cmpRef.instance.BackCompleted.subscribe(bk => {
        //                 this._EntityPMService.getSingle("BIReport", this.EntityPM.Id).then((res: any) => {
        //                     res.subscribe((aa: any) => {
        //                         this.BIReportName = aa.Result != null ? aa.Result.Name : "";
        //                     })
        //                 });
        //             });
        //         });
        // }
    }
}