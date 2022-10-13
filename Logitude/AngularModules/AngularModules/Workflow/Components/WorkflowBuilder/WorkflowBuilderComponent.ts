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
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ObjectFieldListService } from "Infrastructure/Services/StandardLists/ObjectFieldListService";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { FlowReader } from "Workflow/Models/FlowReader";

@Component({
    templateUrl: "./WorkflowBuilderComponent.html"
})

export class WorkflowBuilderComponent extends BaseComponent implements OnInit, OnDestroy {

    @ViewChild("workflowBuilderComponentContainer", { static: false }) containerRef: ElementRef;

    public ComponentRef: ComponentRef<WorkflowBuilderComponent>;

    public ReactFlowInstance: any = null;
    public EntityPM: WorkFlowPM = null;
    public WorkflowId: string;
    public WorkflowEntity: string = null;
    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;
    public BackButtonLable: string = "Workflows";
    public WorkflowName: string;
    public ValidationErrorsList: string[] = [];
    public ReturnPropertiesDataEventKey: string = "returnPropertiesDataEventKey_" + (Date.now())?.toString();
    public HasChanges = false;

    public FlowObjectFields: ObjectFieldPM[] = [];

    public LoadedObjectFieldsEntities: string[] = [];

    public WorkFlowPMService: WorkFlowPMService;

    private CurrentSession = SessionLocator.SelectedSession;

    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor() {
        super();
    }

    public Run(args: any) {
        this.initializeServices();
        this.WorkflowId = args['EntityId'];
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
        if (this.WorkflowId) {
            this.startBusyIndicator("Loading ...");
            this.WorkFlowPMService.get(this.WorkflowId).subscribe((serviceResponse: ServiceResponse) => { this.handleGetWorkflowResponse(serviceResponse); });
        }
    }

    handleGetWorkflowResponse(serviceResponse: ServiceResponse) {
        if (!serviceResponse.HasError) {
            this.EntityPM = serviceResponse.Result;
            this.WorkflowName = this.EntityPM.Name;
            this.WorkflowEntity = this.EntityPM.Entity;
            this.renderReactFlowModeler();
            this.loadFlowObjectFields();
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
            let flowObject = this.getEntityFlowObject();
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

    loadFlowObjectFields() {
        let flowObject = this.getEntityFlowObject();
        if (flowObject) {
            let objectFieldListService = new ObjectFieldListService();
            let workflowEntitiesIds = this.getWorkflowEntitiesIds(flowObject);
            let apiQueryFilters = ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(workflowEntitiesIds, null, null, true);
            objectFieldListService.getByFilters(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
                if (!serviceResponse.HasError) {
                    this.FlowObjectFields = serviceResponse.Result;
                    this.LoadedObjectFieldsEntities = workflowEntitiesIds.split(",");
                }
                this.stopBusyIndicator();
            });
        } else {
            this.stopBusyIndicator();
        }
    }

    loadEntityObjectFields(entityId: string) {
        this.startBusyIndicator("Loading ...");
        let objectFieldListService = new ObjectFieldListService();
        let apiQueryFilters = ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(entityId, null, null, true);
        objectFieldListService.getByFilters(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.FlowObjectFields = this.FlowObjectFields.concat(serviceResponse.Result);
                this.LoadedObjectFieldsEntities.push(entityId);
            }
            this.stopBusyIndicator();
        });
    }

    getWorkflowEntitiesIds(flowObject: any) {
        let entitiesIds = [];
        if (flowObject) {
            FlowReader.getNodes(flowObject).forEach((node: any) => {
                let entity = node.data["entity"];
                if (entity) {
                    let entityObjectTable = (window as any).ObjectTables.filter((o: any) => o.Name === entity)[0];
                    if (entityObjectTable) {
                        entitiesIds.push(entityObjectTable.Id);
                    }
                }
            });
        }
        return entitiesIds.filter((v, i, a) => a.indexOf(v) === i).join(",");
    }

    flowChangedEvent = (event: any) => {
        if (event.status === "success") {
            this.HasChanges = true;
        }
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
            propertiesWindow.WindowClosed.subscribe((data: any) => { this.handlePropertiesWindowClosed(data, openPropertiesEventObject.nodeType); });
        }
    }

    getPropertiesComponentPath = (nodeType: string) => {
        let propertiesComponentPath = "./Workflow/Components/Properties/";
        let propertiesComponentName = nodeType ? ((nodeType.charAt(0).toUpperCase() + nodeType.slice(1)).replace("Node", "") + "PropertiesComponent") : "";
        return (propertiesComponentPath + propertiesComponentName);
    }

    buildPropertiesWindow = (openPropertiesEventObject: any) => {
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            Data: JSON.parse(JSON.stringify(openPropertiesEventObject.nodeData)),
            WorkflowEntity: this.WorkflowEntity,
            FlowObject: this.getCurrentFlowObject(),
            CurrentNodeId: openPropertiesEventObject.nodeId,
            FlowObjectFields: this.FlowObjectFields
        };
        if (openPropertiesEventObject.nodeType == "declareVariableNode") {
            propertiesWindow.Height = 320;
        } else {
            propertiesWindow.Height = 850;
        }
        propertiesWindow.Width = 970;
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

    handlePropertiesWindowClosed = (data: any, nodeType: string) => {
        if (data) {
            document.dispatchEvent(new CustomEvent(this.ReturnPropertiesDataEventKey, { detail: data }));

            let entity = data["entity"];
            if (entity) {

                if (nodeType === "startNode") {
                    this.WorkflowEntity = entity;
                }

                let entityObjectTable = (window as any).ObjectTables.filter((o: any) => o.Name === entity)[0];
                let entityId = entityObjectTable ? entityObjectTable.Id : null;

                if (entityId && this.LoadedObjectFieldsEntities.indexOf(entityId) === -1) {
                    this.loadEntityObjectFields(entityId);
                }

            }

            this.HasChanges = true;
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
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityPM: this.EntityPM, ObjectTableName: "WorkFlow", BackButtonLabel: 'WorkFlows' });

                let isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (!isEditComponentSaved) {
                        this.WorkFlowPMService.get(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => {
                            if (!serviceResponse.HasError) {
                                this.WorkflowName = serviceResponse.Result ? serviceResponse.Result.Name : "";
                                this.EntityPM = serviceResponse.Result
                            }
                        });
                    }
                });

                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                        this.WorkFlowPMService.get(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => {
                            if (!serviceResponse.HasError) {
                                this.WorkflowName = serviceResponse.Result ? serviceResponse.Result.Name : "";
                            }
                        });
                    }
                });
            });
    }

    saveWorkflow(backAfterSave: boolean = false) {
        let flowObject = this.getCurrentFlowObject();
        if (flowObject) {
            let startNode = FlowReader.getStartNode(flowObject);
            this.EntityPM.Entity = startNode ? (startNode.data["entity"] || null) : null;
            this.EntityPM.Trigger = startNode ? (startNode.data["trigger"] || null) : null;
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
        this.WorkflowId = workflowPM.Id;
        this.HasChanges = false;
    }

    getCurrentFlowObject() {
        return this.ReactFlowInstance ? this.ReactFlowInstance.toObject() : null;
    }

    getEntityFlowObject() {
        return this.EntityPM.FlowJson && this.EntityPM.FlowJson !== "" ? JSON.parse(this.EntityPM.FlowJson) : null;
    }
}