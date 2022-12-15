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
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { FlowReader } from "Workflow/Models/FlowReader";
import { ObjectFields } from "Workflow/Models/ObjectFields";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { Formatter } from "Workflow/Models/Formatter";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { AppTool } from "Infrastructure/Tools";
import { WorkFlowVersionPM } from "Workflow/EntityPMs/WorkFlowVersionPM";
import { WorkFlowVersionPMService } from "Workflow/Services/StandardPMs/WorkFlowVersionPMService";

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
    public returnDeleteNodeConfirmationEventKey: string = "returnDeleteNodeConfirmationEventKey_" + (Date.now())?.toString();
    public HasChanges = false;

    public IsSaveDisabled: boolean = true;
    public IsSaveAsDisabled: boolean = true;
    public IsActivateDisabled: boolean = true;
    public ActivateButtonTitle: string = "Activate";

    public FlowObjectFields: ObjectFieldList[] = [];

    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();;
    public WorkFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();

    private CurrentSession = SessionLocator.SelectedSession;

    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.WorkflowId = this.entityArgs.EntityPM.Id
        this.loadWorkflow();
        this.Listen()
    }

    ngOnInit() {
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "workflowEdited") {
                        this.SetButtonStates();
                    }
                }
            );
        }
    }

    loadWorkflow() {
        if (this.EntityPM) {
            this.startBusyIndicator("Loading ...");
            this.WorkFlowPMService.get(this.WorkflowId).subscribe((serviceResponse: ServiceResponse) => { this.handleGetWorkflowResponse(); });
        }
    }

    handleGetWorkflowResponse() {
        this.WorkflowName = this.EntityPM.Name;
        this.WorkflowEntity = this.EntityPM.Entity;
        this.renderReactFlowModeler();
        this.loadObjectFields();
        this.SetButtonStates();
    }

    renderReactFlowModeler() {
        if (this.EntityPM) {
            let flowObject = this.getEntityFlowObject();
            let props = {
                flow: flowObject,
                flowChangedEvent: (event: any) => this.flowChangedEvent(event),
                flowObjectChangedEvent: (event: any) => this.flowObjectChangedEvent(event),
                openPropertiesEvent: (openPropertiesEventObject: any) => this.openPropertiesEvent(openPropertiesEventObject),
                confirmDeleteNodeEvent: (nodeToDelete: any) => this.confirmDeleteNodeEvent(nodeToDelete),
                returnPropertiesDataEventKey: this.ReturnPropertiesDataEventKey,
                returnDeleteNodeConfirmationEventKey: this.returnDeleteNodeConfirmationEventKey,
                setReactFlowInstance: (reactFlowInstance: any) => this.setReactFlowInstance(reactFlowInstance),
                nodeExternalDataKeys: {
                    nodeName: "name",
                    startNodeEntity: "entity",
                    startNodeTrigger: "trigger",
                    conditionNodeLabel: "conditionLabel"
                }
            };

            ReactDOM.render(React.createElement(ReactFlowModeler, props), this.containerRef.nativeElement);
        }
    }

    loadObjectFields() {
        if (ObjectFields.isLoaded()) {
            this.FlowObjectFields = ObjectFields.getAll();
            this.stopBusyIndicator();
        } else {
            let objectFieldListService = new ObjectFieldListService();
            let apiQueryFilters = ApiQueryFiltersBuilder.getObjectFieldsApiQueryFilters(null, null, null, true);
            objectFieldListService.getByFilters(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
                if (!serviceResponse.HasError) {
                    ObjectFields.set(serviceResponse.Result);
                    this.FlowObjectFields = ObjectFields.getAll();
                }
                this.stopBusyIndicator();
            });
        }
    }

    public SetButtonStates() {
        this.IsSaveDisabled = true
        this.IsActivateDisabled = true;
        this.IsSaveAsDisabled = true;
        if ((!this.isWorkflowHasVersion()) || (this.isDraftVersion() && this.isWorkflowHasChanges())) {
            this.IsSaveDisabled = false
        }
        if (!this.isDraftVersion() && ((!this.isWorkflowHasVersion()) || this.isActiveVersion || this.isInactiveVersion())) {
            this.IsSaveAsDisabled = false
        }
        if (this.isWorkflowHasVersion() && !this.isWorkflowHasChanges()) {
            if (this.isActiveVersion()) {
                this.IsActivateDisabled = false
                this.ActivateButtonTitle = "Deactivate";
            } else if (this.isDraftVersion() || this.isInactiveVersion()) {
                this.IsActivateDisabled = false
                this.ActivateButtonTitle = "Activate";
            }
        }
        if (this.IsSaveDisabled) {
            this.EntityPM.IsDirty = false
            this.HasChanges = false
        }
    }

    flowChangedEvent(event: any) {
        if (event && event.status === "success") {
            this.HasChanges = true;
        } else {
            let messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show(event.message ? event.message : "Error");
        }
    }

    flowObjectChangedEvent(flowObject: any) {
        if (flowObject) {
            this.setWorkflow(flowObject)
            this.SetButtonStates();
            this.entityArgs.SendMessage("workflowBuilderEdited");
        }
    }

    openPropertiesEvent(openPropertiesEventObject: any) {
        if (openPropertiesEventObject) {
            this.handleOpenPropertiesEvent(openPropertiesEventObject);
        }
    }

    confirmDeleteNodeEvent(nodeToDelete: any) {
        if (nodeToDelete) {
            let validateNodeDelete = this.validateNodeDelete(nodeToDelete);
            if (validateNodeDelete.isValid) {
                document.dispatchEvent(new CustomEvent(this.returnDeleteNodeConfirmationEventKey, { detail: true }));
            } else {
                let nodeNameToDelete = nodeToDelete.data["name"] || nodeToDelete.id;
                this.showDeleteNodeError(nodeNameToDelete, validateNodeDelete.usedInNodes);
            }
        }
    }

    showDeleteNodeError(nodeNameToDelete: string, usedInNodes: string[]) {
        let deleteNodeErrorWindow = new LogitudeWindow();
        let deleteNodeErrorWindowArgs: any = {
            NodeNameToDelete: nodeNameToDelete,
            UsedInNodes: usedInNodes
        };
        deleteNodeErrorWindow.Width = 460;
        deleteNodeErrorWindow.Height = 220;
        deleteNodeErrorWindow.RTL = false;
        deleteNodeErrorWindow.Title = "Can't delete " + nodeNameToDelete;
        deleteNodeErrorWindow.WindowArgs = deleteNodeErrorWindowArgs;

        deleteNodeErrorWindow.Show("./Workflow/Components/Messages/DeleteNodeWarningComponent");
        deleteNodeErrorWindow.WindowClosed.subscribe((_event: any) => { });
    }

    validateNodeDelete(nodeToDelete: any) {
        let flowObject = this.getCurrentFlowObject();
        if (flowObject) {
            let isValid = true;
            let usedInNodes: string[] = [];
            let getRecordNodes = FlowReader.getNodes(flowObject, "getRecordNode");
            let conditionNodes = FlowReader.getNodes(flowObject, "conditionNode");
            let setValueNodes = FlowReader.getNodes(flowObject, "setValueNode");
            let createRecordNodes = FlowReader.getNodes(flowObject, "createRecordNode");
            let loopNodes = FlowReader.getNodes(flowObject, "loopNode");

            let nodeUsedData = this.getNodeUsedData(nodeToDelete);
            if (nodeUsedData) {
                let getRecordStatus = this.validateNodesUsedData(getRecordNodes, "conditions", nodeUsedData);
                let conditionStatus = this.validateNodesUsedData(conditionNodes, "conditions", nodeUsedData);
                let setValueStatus = this.validateNodesUsedData(setValueNodes, "setValues", nodeUsedData);
                let createRecordStatus = this.validateNodesUsedData(createRecordNodes, "setValues", nodeUsedData);
                let loopStatus = this.validateLoopNodesUsedData(loopNodes, nodeUsedData);
                usedInNodes = usedInNodes
                    .concat(getRecordStatus.usedInNodes)
                    .concat(conditionStatus.usedInNodes)
                    .concat(setValueStatus.usedInNodes)
                    .concat(createRecordStatus.usedInNodes)
                    .concat(loopStatus.usedInNodes);

                usedInNodes = usedInNodes.filter((v, i, a) => a.indexOf(v) === i);
                isValid = getRecordStatus.isValid && conditionStatus.isValid && setValueStatus.isValid && createRecordStatus.isValid && loopStatus.isValid;
                return { isValid, usedInNodes };
            }

            return { isValid: true, usedInNodes: [] };
        }
        return { isValid: false, usedInNodes: [] };
    }

    getNodeUsedData(node: any) {
        if (node.type === "declareVariableNode") {
            let variableCode = node.data["variableCode"];
            return variableCode ? ("declaredvariables_" + variableCode) : null;
        } else if (node.type === "getRecordNode") {
            let name = node.data["name"];
            return name ? (Formatter.getCodeFromName(name) + "_") : null;
        } else if (node.type === "loopNode") {
            let name = node.data["name"];
            return name ? (Formatter.getCodeFromName(name) + "_") : null;
        }
        return null;
    }

    validateNodesUsedData(nodes: any, dataKey: string, nodeUsedData: string) {
        let isValid = true;
        let usedInNodes: string[] = []
        nodes.forEach((node: any) => {
            let nodeName = node.data["name"] || node.id;
            let nodeDataValues = node.data[dataKey] || [];
            nodeDataValues.forEach((dataValue: any) => {
                let value = dataValue["value"] || null;
                let field = dataValue["field"] || null;
                if (value && field && (value.startsWith(nodeUsedData) || field.startsWith(nodeUsedData))) {
                    isValid = false;
                    usedInNodes.push(nodeName);
                }
            });
        });
        return { isValid, usedInNodes };
    }

    validateLoopNodesUsedData(nodes: any, nodeUsedData: string) {
        let isValid = true;
        let usedInNodes: string[] = []
        nodes.forEach((node: any) => {
            let nodeName = node.data["name"] || node.id;
            let collectionVariable = node.data["collectionVariable"] || null;
            if (collectionVariable && collectionVariable === nodeUsedData) {
                isValid = false;
                usedInNodes.push(nodeName);
            }
        });
        return { isValid, usedInNodes };
    }

    setReactFlowInstance(reactFlowInstance: any) {
        this.ReactFlowInstance = reactFlowInstance;
    }

    handleOpenPropertiesEvent(openPropertiesEventObject: any) {
        let propertiesComponentPath = this.getPropertiesComponentPath(openPropertiesEventObject.nodeType);
        if (propertiesComponentPath) {
            let propertiesWindow = this.buildPropertiesWindow(openPropertiesEventObject);
            propertiesWindow.Show(propertiesComponentPath);
            propertiesWindow.WindowClosed.subscribe((data: any) => { this.handlePropertiesWindowClosed(data, openPropertiesEventObject.nodeType); });
        }
    }

    getPropertiesComponentPath(nodeType: string) {
        let propertiesComponentPath = "./Workflow/Components/Properties/";
        let propertiesComponentName = nodeType ? ((nodeType.charAt(0).toUpperCase() + nodeType.slice(1)).replace("Node", "") + "PropertiesComponent") : "";
        return (propertiesComponentPath + propertiesComponentName);
    }

    buildPropertiesWindow(openPropertiesEventObject: any) {
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            Data: JSON.parse(JSON.stringify(openPropertiesEventObject.nodeData)),
            WorkflowEntity: this.WorkflowEntity,
            FlowObject: this.getCurrentFlowObject(),
            CurrentNodeId: openPropertiesEventObject.nodeId,
            FlowObjectFields: this.FlowObjectFields
        };
        propertiesWindow.Height = openPropertiesEventObject.nodeType == "declareVariableNode" ? 320 : 850;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = this.getPropertiesWindowTitle(openPropertiesEventObject.nodeLabel);
        propertiesWindow.WindowArgs = propertiesWindowArgs;
        return propertiesWindow;
    }

    getPropertiesWindowTitle(nodeLabel: string) {
        let subTitle = "Configure";
        switch (nodeLabel) {
            case "Append Item":
                return subTitle + " Append to Collection";
            case "Delete Item":
                return subTitle + " Delete from Collection";
            default:
                return nodeLabel ? (subTitle + " " + nodeLabel) : (subTitle + " Element");
        }
    }

    buildEditWindow() {
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

    handlePropertiesWindowClosed(data: any, nodeType: string) {
        if (data) {
            document.dispatchEvent(new CustomEvent(this.ReturnPropertiesDataEventKey, { detail: data }));

            if (nodeType === "startNode") {
                let dataEntity = data["entity"];
                if (this.WorkflowEntity !== dataEntity) {
                    let flowObject = this.getCurrentFlowObject();
                    FlowReader.getNodes(flowObject, "conditionNode").forEach((conditionNode: any) => {
                        conditionNode.data["conditions"] = [];
                        conditionNode.data["conditionsOperation"] = null;
                    });
                }
                this.WorkflowEntity = dataEntity;
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

    //Actions
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

    setWorkflow(flowObject: any) {
        if (flowObject) {
            let startNode = FlowReader.getStartNode(flowObject);
            this.EntityPM.Entity = startNode ? (startNode.data["entity"] || null) : null;
            this.EntityPM.Trigger = startNode ? (startNode.data["trigger"] || null) : null;
            this.EntityPM.FlowJson = JSON.stringify(flowObject);
        }
    }

    saveAsWorkflow() {
        let propertiesComponentPath = "./Workflow/Components/WorkflowBuilder/CreateWorkflowVersionComponent";
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            WorkflowId: this.EntityPM.Id,
            FlowJson: this.EntityPM.FlowJson
        };
        propertiesWindow.Height = 340;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = "Save New Verison";
        propertiesWindow.WindowArgs = propertiesWindowArgs;

        propertiesWindow.Show(propertiesComponentPath);
        propertiesWindow.WindowClosed.subscribe((data: any) => { this.handleVersionPropertiesWindowClosed(data); });
    }

    ActivateVersion() {
        this.startBusyIndicator("Saving ...");
        this.WorkFlowVersionPMService.get(this.EntityPM.WorkFlowActiveVersionId).subscribe((response) => {
            if (!response.HasError) {
                let clickedVersion: WorkFlowVersionPM = response.Result;
                let isActivate: boolean = clickedVersion.StatusCode == "INVE" || clickedVersion.StatusCode == "DRFT"
                clickedVersion.StatusCode = isActivate ? "ACVE" : "INVE"
                this.WorkFlowVersionPMService.update(clickedVersion).subscribe((serviceResponse: ServiceResponse) => {
                    if (serviceResponse != null && !serviceResponse.HasError) {
                        this.handleActivateWorkflowResponse(serviceResponse.Result)
                        this.stopBusyIndicator();
                    } else {
                        this.stopBusyIndicator();
                    }
                });
            } else {
                this.stopBusyIndicator();
            }
        });
    }

    handleUpdateWorkflowResponse(serviceResponse: ServiceResponse, backAfterSave: boolean) {
        if (!serviceResponse.HasError) {
            this.handleSaveWorkflowResponse(serviceResponse.Result);
            this.stopBusyIndicator();
            // if (backAfterSave) {
            //     this.goBack();
            // }
        }
    }

    handleSaveWorkflowResponse(workflowPM: WorkFlowPM) {
        this.EntityPM = workflowPM;
        this.WorkflowId = workflowPM.Id;
        this.HasChanges = false;
        this.SetButtonStates()
        this.entityArgs.SendMessage("workflowBuilderEdited");
    }

    handleVersionPropertiesWindowClosed(data) {
        this.SetWorkflowDetails(data)
    }

    handleActivateWorkflowResponse(data) {
        this.SetWorkflowDetails(data)
    }

    SetWorkflowDetails(data) {
        let version: WorkFlowVersionPM = data;
        this.EntityPM.WorkFlowVersionStatusCode = version.StatusCode;
        this.EntityPM.WorkFlowVersionNumber = version.VersionNumber;
        this.EntityPM.WorkFlowActiveVersionId = version.Id;
        this.HasChanges = false;
        this.EntityPM.IsDirty = false;
        this.SetButtonStates()
        this.entityArgs.SendMessage("workflowBuilderEdited");
    }

    getCurrentFlowObject() {
        return this.ReactFlowInstance ? this.ReactFlowInstance.toObject() : null;
    }

    getEntityFlowObject() {
        return this.EntityPM.FlowJson && this.EntityPM.FlowJson !== "" ? JSON.parse(this.EntityPM.FlowJson) : null;
    }


    //flags
    isWorkflowHasVersion() {
        return this.EntityPM.WorkFlowActiveVersionId != null
    }

    isWorkflowHasChanges() {
        // return this.entityArgs.EntityPM.IsDirty
        return this.EntityPM.IsDirty
    }

    isDraftVersion() {
        return this.EntityPM.WorkFlowVersionStatusCode == "DRFT"
    }

    isActiveVersion() {
        return this.EntityPM.WorkFlowVersionStatusCode == "ACVE"
    }

    isInactiveVersion() {
        return this.EntityPM.WorkFlowVersionStatusCode == "INVE"
    }

    startBusyIndicator(message: string) {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
}
