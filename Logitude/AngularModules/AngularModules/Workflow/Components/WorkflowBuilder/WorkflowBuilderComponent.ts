import * as React from "react";
import * as ReactDOM from "react-dom";
import { Component, ComponentRef, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from "@angular/core";
import ReactFlowModeler from "logitude-workflow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { WorkFlowPM } from "Workflow/EntityPMs/WorkFlowPM";
import { WorkFlowPMService } from "Workflow/Services/StandardPMs/WorkFlowPMService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
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
    public ValidVersion: WorkFlowVersionPM = null;
    public CurrentVersionId: string = null;
    public WorkflowName: string;
    public WorkflowEntity: string = null;
    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;
    public ValidationErrorsList: string[] = [];
    public FlowObjectFields: ObjectFieldList[] = [];
    public ReturnPropertiesDataEventKey: string = "returnPropertiesDataEventKey_" + (Date.now())?.toString();
    public returnDeleteNodeConfirmationEventKey: string = "returnDeleteNodeConfirmationEventKey_" + (Date.now())?.toString();
    public HasChanges = false;
    public IsFirstOpen: boolean = false;

    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();;
    private TabSelectedEvent: any = null;

    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.EntityId = this.entityArgs.EditComponent.EntityId;
        this.IsFirstOpen = this.entityArgs.EditComponent.IsFirstOpen;
    }

    ngOnInit() {
        this.loadWorkflow(true);
        this.listen()
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    private listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "WFFB") {
                    var clickedRowId = this.entityArgs.EditComponentArgument?.ClickedVersionRow!
                    var updatedVersionId = this.entityArgs.EditComponentArgument?.UpdatedVersion!
                    if (updatedVersionId) {
                        var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == updatedVersionId);
                        this.displayGivenVersion(version);
                    }
                    else if (clickedRowId) {
                        var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == clickedRowId);
                        this.displayGivenVersion(version);
                    }
                }
            });
        }

        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "WorkflowVersionsUpdated") {
                        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, HasChanges: false }
                        this.loadWorkflow(false);
                    }
                }
            );
        }
    }

    loadWorkflow(isNewWorkflow: boolean) {
        if (this.EntityId) {
            this.startBusyIndicator("Loading ...");
            this.WorkFlowPMService.get(this.EntityId).subscribe((serviceResponse: ServiceResponse) => { this.handleGetWorkflowResponse(serviceResponse, isNewWorkflow); });
        }
    }

    handleGetWorkflowResponse(serviceResponse: ServiceResponse, isNewWorkflow) {
        if (isNewWorkflow) {
            this.setNewWorkflow(serviceResponse)
        } else {
            this.setUpdatedWorkflow(serviceResponse)
        }
    }

    setNewWorkflow(serviceResponse: ServiceResponse) {
        this.EntityPM = serviceResponse.Result
        this.entityArgs.EntityPM = serviceResponse.Result
        this.entityArgs.EditComponent.EntityPM = serviceResponse.Result
        this.ValidVersion = this.getValidVersion();
        this.setCurrentDisplayedVersion(this.ValidVersion.Id)
        this.handleRenderReactWorkflow();
    }

    setUpdatedWorkflow(serviceResponse: ServiceResponse) {
        this.EntityPM.WorkFlowVersions = serviceResponse.Result.WorkFlowVersions
        var updatedVersion = this.entityArgs.EditComponentArgument?.UpdatedVersion!
        var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == updatedVersion);
        this.displayGivenVersion(version);
    }


    displayGivenVersion(version: WorkFlowVersionPM) {
        if (version) {
            this.ValidVersion = version;
            this.setCurrentDisplayedVersion(version.Id)
            ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
            this.handleRenderReactWorkflow();
        }
    }

    handleRenderReactWorkflow() {
        this.WorkflowName = this.EntityPM.Name;
        this.WorkflowEntity = this.ValidVersion.Entity;
        this.renderReactFlowModeler();
        this.loadObjectFields();
        this.entityArgs.SendMessage("RefreshWorkflowShortTitle");
        this.entityArgs.SendMessage("RefreshWorkflowButtons");
    }

    setCurrentDisplayedVersion(versionId: string) {
        this.CurrentVersionId = versionId
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, CurrentDisplayedVersionId: versionId }
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
                externalDataKeys: {
                    nodeName: "name",
                    nodeLabel: "label",
                    startNodeEntity: "entity",
                    startNodeTrigger: "trigger",
                    conditionNodeMetLabel: "metLabel",
                    conditionNodeOtherwiseLabel: "otherwiseLabel"
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

    flowChangedEvent(event: any) {
        if (event && event.status === "success") {
            var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == this.CurrentVersionId)
            if (version.StatusCode == "DRFT") {
                this.HasChanges = true;
                this.EntityPM.IsDirty = true
                this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, HasChanges: true }
                this.entityArgs.SendMessage("RefreshWorkflowButtons");
            }
        } else {
            let messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show(event.message ? event.message : "Error");
        }
    }

    flowObjectChangedEvent(flowObject: any) {
        if (flowObject) {
            if (this.HasChanges) {
                this.setWorkflowVersion(flowObject)
            }
        }
    }

    setWorkflowVersion(flowObject: any) {
        if (flowObject) {
            var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == this.CurrentVersionId)
            if (version.StatusCode == "DRFT") {
                let startNode = FlowReader.getStartNode(flowObject);
                version.Entity = startNode ? (startNode.data["entity"] || null) : null;
                version.Trigger = startNode ? (startNode.data["trigger"] || null) : null;
                version.FlowJson = JSON.stringify(flowObject);
            }
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
                this.showConfirmationMessageForDeleteNode();
            } else {
                let nodeNameToDelete = nodeToDelete.data["label"] || nodeToDelete.id;
                this.showDeleteNodeError(nodeNameToDelete, validateNodeDelete.usedInNodes);
            }
        }
    }

    showConfirmationMessageForDeleteNode() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowNoButton = false
        confirmWindow.ShowCancelButton = true;
        confirmWindow.YesButtonText = "Ok";
        confirmWindow.Title = "Delete Element"
        confirmWindow.Show("Are you sure you want to delete this element?");

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                document.dispatchEvent(new CustomEvent(this.returnDeleteNodeConfirmationEventKey, { detail: true }));
            }
        });
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
            let name = node.data["name"];
            return name ? ("declaredvariables_" + name) : null;
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
            let nodeLabel = node.data["label"] || node.id;
            let nodeDataValues = node.data[dataKey] || [];
            nodeDataValues.forEach((dataValue: any) => {
                let value = dataValue["value"] || null;
                let field = dataValue["field"] || null;
                if (value && field && (value.startsWith(nodeUsedData) || field.startsWith(nodeUsedData))) {
                    isValid = false;
                    usedInNodes.push(nodeLabel);
                }
            });
        });
        return { isValid, usedInNodes };
    }

    validateLoopNodesUsedData(nodes: any, nodeUsedData: string) {
        let isValid = true;
        let usedInNodes: string[] = []
        nodes.forEach((node: any) => {
            let nodeLabel = node.data["label"] || node.id;
            let collectionVariable = node.data["collectionVariable"] || null;
            if (collectionVariable && collectionVariable === nodeUsedData) {
                isValid = false;
                usedInNodes.push(nodeLabel);
            }
        });
        return { isValid, usedInNodes };
    }

    setReactFlowInstance(reactFlowInstance: any) {
        this.ReactFlowInstance = reactFlowInstance;
        let flowObject = this.getCurrentFlowObject();
        if (this.IsFirstOpen && flowObject) {
            this.openConfigureStart(flowObject);
        }
    }

    openConfigureStart(flowObject: any) {
        let startNode = flowObject.nodes.filter(n => n.type === "startNode")[0];
        let nodeObject = this.buildPropertiesEventObjec(startNode);
        if (nodeObject) {
            this.IsFirstOpen = false;
            this.handleOpenPropertiesEvent(nodeObject);
        }
    }

    buildPropertiesEventObjec(startNode: any) {
        if (startNode) {
            let nodeObject = {
                isNewNode: true,
                nodeId: startNode.id,
                nodeType: startNode.type,
                nodeData: startNode.data || {},
                nodeLabel: startNode.data ? (startNode.data["label"] || startNode.data["name"]) : null
            };
            return nodeObject
        }
        return null;
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
        propertiesWindow.ShowFooterButtons = true;
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

    goBack(backCompleted: boolean = true) {
        if (this.ComponentRef) {
            this.BackCompleted.emit(backCompleted);
            this.ComponentRef.destroy();
        }
    }


    getCurrentFlowObject() {
        return this.ReactFlowInstance ? this.ReactFlowInstance.toObject() : null;
    }

    getEntityFlowObject() {
        return this.ValidVersion.FlowJson && this.ValidVersion.FlowJson !== "" ? JSON.parse(this.ValidVersion.FlowJson) : null;
    }

    getValidVersion() {
        var versions = this.EntityPM.WorkFlowVersions.sort((a, b) => a.VersionNumber < b.VersionNumber ? 1 : -1);
        var activeVersion = versions.find(e => e.StatusCode == "ACVE");
        var newestVersion = versions[0];
        if (activeVersion) {
            return activeVersion
        } else {
            return newestVersion
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
}