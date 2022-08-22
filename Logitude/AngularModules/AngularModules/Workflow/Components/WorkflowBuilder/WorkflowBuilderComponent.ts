import { Component, ComponentRef, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from "@angular/core";
import ReactFlowModeler from "logitude-workflow";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { WorkFlowPM } from "Workflow/EntityPMs/WorkFlowPM";
import { WorkFlowPMService } from "Workflow/Services/StandardPMs/WorkFlowPMService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";

@Component({
    templateUrl: "./WorkflowBuilderComponent.html"
})

export class WorkflowBuilderComponent extends BaseComponent implements OnInit, OnDestroy {

    @ViewChild("workflowBuilderComponentContainer", { static: false }) containerRef: ElementRef;

    public ComponentRef: ComponentRef<WorkflowBuilderComponent>;

    public ReactFlowInstance: any = null;

    public EntityPM: WorkFlowPM = null;
    public EntityId: string;

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;

    public BackButtonLable: string = "Workflows";

    public WorkflowName: string;

    public ValidationErrorsList: string[] = [];

    public EventKeyPostfix: string = (Date.now())?.toString();
    public ReturnPropertiesDataEventKey: string = "returnPropertiesDataEventKey_" + this.EventKeyPostfix;

    public WorkFlowPMService: WorkFlowPMService;

    private hasChanges = false;
    get HasChanges() {
        if (this.EntityId == null || (this.EntityPM != null && this.EntityPM.IsDirty) || this.hasChanges) {
            return true;
        }
        return false;
    }

    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

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

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
    }

    initializeServices() {
        this.WorkFlowPMService = new WorkFlowPMService();
    }

    loadWorkflow() {
        if (this.EntityId) {
            this.startBusyIndicator("Loading ...");
            this.WorkFlowPMService.get(this.EntityId).subscribe((serviceResponse: ServiceResponse) => { this.handleGetWorkflowResponse(serviceResponse); });
        }
    }

    handleGetWorkflowResponse(serviceResponse: ServiceResponse) {
        if (!serviceResponse.HasError) {
            this.EntityPM = serviceResponse.Result;
            this.WorkflowName = this.EntityPM.Name;
            this.renderReactFlowModeler();
            this.stopBusyIndicator();
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
                flowChangedEvent: this.flowChangedEvent,
                openPropertiesEvent: this.openPropertiesEvent,
                returnPropertiesDataEventKey: this.ReturnPropertiesDataEventKey,
                setReactFlowInstance: this.setReactFlowInstance,
            };

            ReactDOM.render(React.createElement(ReactFlowModeler, props), this.containerRef.nativeElement);
        }
    }

    flowChangedEvent = () => {
        this.hasChanges = true;
    }

    openPropertiesEvent = (openPropertiesEventObject: any) => {
        if (openPropertiesEventObject) {
            this.handleOpenPropertiesEvent(openPropertiesEventObject);
        }
    }

    setReactFlowInstance = (reactFlowInstance: any) => {
        this.ReactFlowInstance = reactFlowInstance;
    }

    handleOpenPropertiesEvent = (openPropertiesEventObject: any) => {
        let propertiesComponentPath = this.getPropertiesComponentPath(openPropertiesEventObject.nodeType);
        if (propertiesComponentPath) {
            let propertiesWindow = this.buildPropertiesWindow(openPropertiesEventObject);
            propertiesWindow.Show(propertiesComponentPath);
            propertiesWindow.WindowClosed.subscribe((data: any) => { this.handlePropertiesWindowClosed(data); });
        }
    }

    getPropertiesComponentPath = (nodeType: string) => {
        let propertiesComponentPath = "./Workflow/Components/Properties/";
        switch (nodeType) {
            case "startNode":
                return null;
            //return (propertiesComponentPath + "StartPropertiesComponent");
            case "conditionNode":
                return (propertiesComponentPath + "ConditionPropertiesComponent");
            case "loopNode":
                return (propertiesComponentPath + "LoopPropertiesComponent");
            case "setValueNode":
                return (propertiesComponentPath + "SetValuePropertiesComponent");
            default:
                return null;
        }
    }

    buildPropertiesWindow = (openPropertiesEventObject: any) => {
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            Data: JSON.parse(JSON.stringify(openPropertiesEventObject.nodeData))
        };
        propertiesWindow.Width = 800;
        propertiesWindow.Height = 420;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = ("Configure " + openPropertiesEventObject.nodeLabel);
        propertiesWindow.WindowArgs = propertiesWindowArgs;
        return propertiesWindow;
    }

    buildEditWindow = () => {
        let editWindow = new LogitudeWindow();
        let editWindowArgs: any = {
            EntityId: this.EntityPM.Id,
            IsNewEntity: false
        };
        editWindow.Width = 960;
        editWindow.Height = 570;
        editWindow.RTL = false;
        editWindow.Title = "Edit Workflow";
        editWindow.WindowArgs = editWindowArgs;
        return editWindow;
    }

    handlePropertiesWindowClosed = (data: any) => {
        if (data) {
            document.dispatchEvent(new CustomEvent(this.ReturnPropertiesDataEventKey, { detail: data }));
        }
    }

    backButtonClicked() {
        if (this.HasChanges) {
            this.confirmSaveWorkflow();
        }
        else {
            this.goBack(false);
        }
    }

    confirmSaveWorkflow() {
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 200;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.Title = "Unsaved Changes";
        confirmWindow.YesButtonText = "Save";
        confirmWindow.NoButtonText = "Don't Save";
        confirmWindow.CancelButtonText = "Cancel";
        confirmWindow.Show("This workflow has unsaved changes. Do you want to save it ?");
        confirmWindow.WindowClosed.subscribe(() => { this.handleConfirmWindowClosed(confirmWindow); });
    }

    handleConfirmWindowClosed(confirmWindow: ConfirmWindow) {
        if (confirmWindow.Yes) {
            this.saveWorkflow(true);
        }
        else if (confirmWindow.No) {
            this.goBack(false);
        }
    }

    goBack(backCompleted: boolean = true) {
        if (this.ComponentRef) {
            this.BackCompleted.emit(backCompleted);
            this.ComponentRef.destroy();
        }
    }

    editWorkflowClicked() {
        let editWindow = this.buildEditWindow();
        editWindow.Show("./Workflow/Components/CreateEditWorkflow/CreateEditWorkflowComponent");
        editWindow.WindowClosed.subscribe((entityPM: WorkFlowPM) => { this.handleEditWindowClosed(entityPM); });
    }

    handleEditWindowClosed(entityPM: WorkFlowPM) {
        if (entityPM) {
            this.EntityPM = entityPM;
            this.WorkflowName = entityPM.Name;
        }
    }

    saveWorkflow(backAfterSave: boolean = false) {
        let flowObject = this.ReactFlowInstance ? this.ReactFlowInstance.toObject() : null;
        if (flowObject) {
            this.EntityPM.FlowJson = JSON.stringify(flowObject);
            this.startBusyIndicator("Saving ...");
            this.WorkFlowPMService.update(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => { this.handleUpdateWorkflowResponse(serviceResponse, backAfterSave); });
        }
    }

    handleUpdateWorkflowResponse(serviceResponse: ServiceResponse, backAfterSave: boolean) {
        if (!serviceResponse.HasError) {
            this.handleSaveWorkflowResponse(serviceResponse.Result);
            this.stopBusyIndicator();
            if (backAfterSave) {
                this.goBack();
            }
        }
    }

    handleSaveWorkflowResponse(workflowPM: WorkFlowPM) {
        this.EntityPM = workflowPM;
        this.EntityId = workflowPM.Id;
        this.hasChanges = false;
    }
}