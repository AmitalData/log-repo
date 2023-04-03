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
import { ApiQueryFiltersBuilder } from "Workflow/Utilities/ApiQueryFiltersBuilder";
import { FlowReader } from "Workflow/Utilities/FlowReader";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { Formatter } from "Workflow/Utilities/Formatter";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { AppTool } from "Infrastructure/Tools";
import { WorkFlowVersionPM } from "Workflow/EntityPMs/WorkFlowVersionPM";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";
import { ObjectTableListService } from "Infrastructure/Services/StandardLists/ObjectTableListService";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { StartTriggerTypes } from "Workflow/Constants/StartTriggerTypes";

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
    public WorkflowNumber: string;
    public WorkflowTriggerTypeCode: string;
    public WorkflowName: string;
    public WorkflowEntity: string = null;
    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;
    public ValidationErrorsList: string[] = [];
    public ReturnPropertiesDataEventKey: string = "returnPropertiesDataEventKey_" + (Date.now())?.toString();
    public returnDeleteNodeConfirmationEventKey: string = "returnDeleteNodeConfirmationEventKey_" + (Date.now())?.toString();
    public HasChanges = false;
    public IsFirstOpen: boolean = false;
    public IsViewMode: boolean = false;

    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();
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
        this.listen();
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    private listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode === "WFFB") {
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

            this.entityArgs.EntityArgEventEmitter.subscribe((event: any) => {
                if (event === "WorkflowVersionsUpdated") {
                    this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, HasChanges: false };
                    this.loadWorkflow(false);
                }
            });
        }
    }

    loadWorkflow(isNewWorkflow: boolean) {
        if (this.EntityId) {
            this.startLoading();
            this.WorkFlowPMService.get(this.EntityId).subscribe((serviceResponse: ServiceResponse) => { this.handleGetWorkflowResponse(serviceResponse, isNewWorkflow); });
        }
    }

    handleGetWorkflowResponse(serviceResponse: ServiceResponse, isNewWorkflow) {
        if (isNewWorkflow) {
            this.setNewWorkflow(serviceResponse);
        } else {
            this.setUpdatedWorkflow(serviceResponse);
        }
    }

    setNewWorkflow(serviceResponse: ServiceResponse) {
        this.EntityPM = serviceResponse.Result
        this.entityArgs.EntityPM = serviceResponse.Result
        this.entityArgs.EditComponent.EntityPM = serviceResponse.Result
        this.ValidVersion = this.getValidVersion();
        this.setCurrentDisplayedVersion(this.ValidVersion);
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
            this.setCurrentDisplayedVersion(version);
            ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
            this.handleRenderReactWorkflow();
        }
    }

    handleRenderReactWorkflow() {
        this.WorkflowTriggerTypeCode = this.EntityPM.WorkFlowTriggerTypeCode;
        this.WorkflowNumber = this.EntityPM.WorkFlowNumber;
        this.WorkflowName = this.EntityPM.Name;
        this.WorkflowEntity = this.ValidVersion.Entity;
        this.renderReactFlowModeler();
        this.loadObjectTablesAndFields();
    }

    setCurrentDisplayedVersion(workflowVersion: WorkFlowVersionPM) {
        if (workflowVersion) {
            this.CurrentVersionId = workflowVersion.Id;
            this.IsViewMode = workflowVersion.StatusCode !== "DRFT";
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, CurrentDisplayedVersionId: workflowVersion.Id };
        }
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
                    startNodeEntityLabel: "entityLabel",
                    startNodeApplicationLabel: "applicationLabel",
                    startNodeEventTypeLabel: "eventTypeLabel",
                    startNodeTrigger: "trigger",
                    conditionNodeMetLabel: "metLabel",
                    conditionNodeOtherwiseLabel: "otherwiseLabel"
                },
                isViewMode: this.IsViewMode,
                flowTriggerType: this.EntityPM.WorkFlowTriggerTypeCode
            };

            ReactDOM.render(React.createElement(ReactFlowModeler, props), this.containerRef.nativeElement);
        }
    }

    loadObjectTablesAndFields() {
        let isLoadedBefore = ObjectTables.isLoaded();
        let objectTableListService = new ObjectTableListService();
        let apiQueryFilters = ApiQueryFiltersBuilder.getObjectTableFilters(null, true, (isLoadedBefore ? true : null));
        objectTableListService.getByFilters(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                let objectTables = serviceResponse.Result.filter((o: any) => o.Tenant === 0 || o.Tenant === (SessionLocator.Tenant || 0));
                if (isLoadedBefore) {
                    ObjectTables.replace(objectTables);
                } else {
                    ObjectTables.set(objectTables);
                }
                ObjectTables.setLoaded();
            }
            this.loadObjectFields();
        });
    }

    loadObjectFields() {
        let isLoadedBefore = ObjectFields.isLoaded();
        let objectFieldListService = new ObjectFieldListService();
        let apiQueryFilters = ApiQueryFiltersBuilder.getObjectFieldFilters(null, null, null, true, (isLoadedBefore ? true : null));
        objectFieldListService.getByFilters(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                let objectFields = serviceResponse.Result.filter((o: any) => o.Tenant === 0 || o.Tenant === (SessionLocator.Tenant || 0));
                if (isLoadedBefore) {
                    ObjectFields.replace(objectFields);
                } else {
                    ObjectFields.set(objectFields);
                }
                ObjectFields.setLoaded();
            }
            this.stopLoading();
            this.handleWorkflowValidation();
            this.openStartConfiguration();
        });
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
        this.handleCopiedDeclareVariableNodes(flowObject);

        if (flowObject && this.HasChanges) {
            this.setWorkflowVersion(flowObject);
        }
    }

    handleCopiedDeclareVariableNodes(flowObject: any) {
        if (flowObject && flowObject.nodes) {
            flowObject.nodes.filter((n: any) => this.isNotHandledCopiedDeclareVariableNode(n)).forEach((node: any) => {
                let nodeName = node.data["name"];
                node.data["variableName"] = nodeName;
                node.data["variableCode"] = Formatter.getCodeFromName(nodeName);
            });
        }
    }

    isNotHandledCopiedDeclareVariableNode(node: any) {
        if (node) {
            return node.type === "declareVariableNode" &&
                node.data &&
                node.data["copiedFrom"] &&
                node.data["copyNumber"] &&
                node.data["name"] &&
                node.data["variableName"] &&
                node.data["name"].toString().startsWith("Copy " + node.data["copyNumber"].toString() + " of ") &&
                !node.data["variableName"].toString().startsWith("Copy " + node.data["copyNumber"].toString() + " of ");
        }
        return false;
    }

    setWorkflowVersion(flowObject: any) {
        if (flowObject) {
            var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == this.CurrentVersionId)
            if (version.StatusCode == "DRFT") {
                let startNode = FlowReader.getStartNode(flowObject);
                version.Entity = startNode ? (startNode.data["entity"] || null) : null;
                version.Trigger = startNode ? (startNode.data["trigger"] || null) : null;
                version.FlowJson = JSON.stringify(flowObject);

                this.handleWorkflowActionsValidation(flowObject, version.Entity, version.Trigger);
            }
        }
    }

    openPropertiesEvent(openPropertiesEventObject: any) {
        if (openPropertiesEventObject) {
            this.handleOpenPropertiesEvent(openPropertiesEventObject);
        }
    }

    confirmDeleteNodeEvent(deletedNode: any) {
        if (deletedNode) {
            let deletedNodeDisplayName = this.getNodeDisplayName(deletedNode);
            let usedInNodes = this.validateDeleteNode(deletedNode);
            if (usedInNodes && usedInNodes.length === 0) {
                this.handleDeleteNode(deletedNodeDisplayName);
            } else {
                this.showDeleteNodeWarning(deletedNodeDisplayName, usedInNodes);
            }
        }
    }

    handleDeleteNode(deletedNodeName: string) {
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.ShowNoButton = false;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.YesButtonText = "Ok";
        confirmWindow.Title = "Delete Element";
        confirmWindow.WindowClosed.subscribe(() => {
            if (confirmWindow.Yes) {
                document.dispatchEvent(new CustomEvent(this.returnDeleteNodeConfirmationEventKey, { detail: true }));
            }
        });
        confirmWindow.Show("Are you sure to delete element " + deletedNodeName + " ?");
    }

    showDeleteNodeWarning(deletedNodeName: string, usedInNodes: string[] | null) {
        let deleteNodeErrorWindow = new LogitudeWindow();
        let deleteNodeErrorWindowArgs: any = {
            NodeNameToDelete: deletedNodeName,
            UsedInNodes: usedInNodes
        };
        deleteNodeErrorWindow.Width = 500;
        deleteNodeErrorWindow.Height = 300;
        deleteNodeErrorWindow.RTL = false;
        deleteNodeErrorWindow.Title = "Cannot delete " + deletedNodeName;
        deleteNodeErrorWindow.WindowArgs = deleteNodeErrorWindowArgs;
        deleteNodeErrorWindow.Show("./Workflow/Components/Messages/DeleteNodeWarningComponent");
    }

    validateDeleteNode(deletedNode: any) {
        if (deletedNode) {
            let flowObject = this.getCurrentFlowObject();
            if (flowObject) {
                let usedInNodes = [];
                FlowReader.getNodes(flowObject).filter((n: any) => n.id !== deletedNode.id).forEach((node: any) => {
                    if (node && node.data) {
                        let clonedNodeData = JSON.parse(JSON.stringify(node.data));
                        let clonedNodeDataJson = JSON.stringify(clonedNodeData);
                        let usedFromDataValues = clonedNodeDataJson.match(/UsedFrom\":\"(.*?)\"/g);
                        if (usedFromDataValues && usedFromDataValues.length > 0) {
                            let usedFromElements = usedFromDataValues.map(v => v ? v.replace("UsedFrom\":\"", "").replace("\"", "") : null).filter(v => v !== null);
                            if (usedFromElements.some(elementId => elementId === deletedNode.id)) {
                                let usedNode = this.getNodeDisplayName(node);
                                usedInNodes.push(usedNode);
                            }
                        }
                    }
                });
                return usedInNodes;
            }
        }
        return null;
    }

    getNodeCode(node: any) {
        if (node) {
            switch (node.type) {
                case "getRecordNode":
                case "collectionFilterNode":
                case "loopNode":
                    return Formatter.getCodeFromName(node.data["name"]);
                case "declareVariableNode":
                    let variableCode = node.data["variableCode"] || null;
                    let variableType = node.data["variableType"] || null;
                    if (variableCode && variableType) {
                        let isRecordVariableType = variableType.toString().startsWith(FieldTypes.Record);
                        return (isRecordVariableType ? "declaredrecordvariables" : "declaredvariables") + "_" + variableCode;
                    }
                    return null;
                default:
                    return null;
            }
        }
        return null;
    }

    getNodeDisplayName(node: any) {
        return node && node.data ? (node.data["label"] || node.data["name"] || "Unknown") : "Unknown";
    }

    setReactFlowInstance(reactFlowInstance: any) {
        this.ReactFlowInstance = reactFlowInstance;
    }

    handleWorkflowActionsValidation(flowObject: any, entity: any, trigger: any) {
        let addedNode = flowObject.nodes.filter((node: any) => node.type !== "startNode" && node.type !== "connectorNode" && node.type !== "endNode");

        if (entity && trigger && addedNode.length > 0) {
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, IsActiveDisabled: false };
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, IsNewVersionDisabled: false };
        } else if (entity && trigger && addedNode.length == 0) {
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, IsActiveDisabled: true };
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, IsNewVersionDisabled: false };
        } else {
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, IsActiveDisabled: true };
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, IsNewVersionDisabled: true };
        }

        this.entityArgs.SendMessage("RefreshWorkflowButtons");
        this.entityArgs.SendMessage("RefreshWorkflowShortTitle");

        this.handleWorkflowWarnings();
    }

    handleWorkflowWarnings() {
        let warningMessages: string[] = [];
        let isActiveDisabled: boolean = this.entityArgs.EditComponentArgument?.IsActiveDisabled ? true : false;
        let isNewVersionDisabled: boolean = this.entityArgs.EditComponentArgument?.IsNewVersionDisabled ? true : false;

        if (this.ValidVersion.StatusCode !== "DRFT") {
            warningMessages.push("This version is currently active or was activated at least once. To make changes create a new version.");
        }
        else if (this.ValidVersion.StatusCode === "DRFT" && isActiveDisabled && isNewVersionDisabled) {
            warningMessages.push("The start element is not configured, you need to select the object whose records trigger the flow.");
        }
        else if (this.ValidVersion.StatusCode === "DRFT" && isActiveDisabled && !isNewVersionDisabled) {
            warningMessages.push("To activate the version, connect at least one element to the start element.");
        }

        this.setWarningMessages(warningMessages);
    }

    openStartConfiguration() {
        if (this.IsFirstOpen) {
            this.openStartPropertiesWindow();
        }
    }

    openStartPropertiesWindow() {
        let flowObject = this.getCurrentFlowObject();
        if (flowObject) {
            let startNode = flowObject.nodes.filter((n: any) => n.type === "startNode")[0];
            let openPropertiesEventObject = this.buildOpenPropertiesEventObject(startNode, true);
            if (openPropertiesEventObject) {
                this.handleOpenPropertiesEvent(openPropertiesEventObject);
                this.IsFirstOpen = false;
            }
        } else {
            setTimeout(() => {
                this.openStartPropertiesWindow();
            }, 100);
        }
    }

    handleWorkflowValidation() {
        let flowObject = this.getCurrentFlowObject();
        if (flowObject) {
            this.handleWorkflowActionsValidation(flowObject, this.ValidVersion.Entity, this.ValidVersion.Trigger);
        } else {
            setTimeout(() => {
                this.handleWorkflowValidation();
            }, 100);
        }
    }

    buildOpenPropertiesEventObject(node: any, isNewNode: boolean) {
        if (node) {
            let nodeObject = {
                isNewNode: isNewNode,
                nodeId: node.id,
                nodeType: node.type,
                nodeData: node.data || {},
                nodeLabel: node.data ? (node.data["label"] || node.data["name"]) : null
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
        }
    }

    getPropertiesComponentPath(nodeType: string) {
        let propertiesComponentPath = "./Workflow/Components/Properties/";
        if (nodeType === "startNode" && this.WorkflowTriggerTypeCode === StartTriggerTypes.EventTriggered) {
            return (propertiesComponentPath + "StartEventTriggeredPropertiesComponent");
        }
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
            WorkflowNumber: this.WorkflowNumber
        };
        propertiesWindow.Height = openPropertiesEventObject.nodeType == "declareVariableNode" ? 320 : 850;
        propertiesWindow.Width = 1000;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = this.getPropertiesWindowTitle(openPropertiesEventObject.nodeLabel);
        propertiesWindow.WindowArgs = propertiesWindowArgs;
        propertiesWindow.ShowFooterButtons = true;
        propertiesWindow.IsViewMode = this.IsViewMode;
        propertiesWindow.WindowClosed.subscribe((data: any) => { this.handlePropertiesWindowClosed(data, openPropertiesEventObject.nodeType); });
        return propertiesWindow;
    }

    getPropertiesWindowTitle(nodeLabel: string) {
        let subTitle = "Configure ";
        switch (nodeLabel) {
            case "Append Item":
                return subTitle + "Append to Collection";
            case "Delete Item":
                return subTitle + "Delete from Collection";
            default:
                return subTitle + (nodeLabel || "Element");
        }
    }

    handlePropertiesWindowClosed(data: any, nodeType: string) {

        if ((window as any)?.PrintData) {
            console.log(data);
        }

        if (data) {
            document.dispatchEvent(new CustomEvent(this.ReturnPropertiesDataEventKey, { detail: data }));

            if (nodeType === "startNode") {
                let dataEntity = data["entity"];
                // if (this.WorkflowEntity !== dataEntity) {
                //     let flowObject = this.getCurrentFlowObject();
                //     FlowReader.getNodes(flowObject, "conditionNode").forEach((conditionNode: any) => {
                //         conditionNode.data["conditions"] = [];
                //         conditionNode.data["conditionsOperation"] = null;
                //     });
                // }
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
            return activeVersion;
        } else {
            return newestVersion;
        }
    }

    setWarningMessages(messages: string[] | null = null) {
        this.entityArgs.SendMessage({ Code: "SetWorkflowWarningMessages", Messages: messages });
    }

    setErrorMessages(messages: string[] | null = null) {
        this.entityArgs.SendMessage({ Code: "SetWorkflowErrorMessages", Messages: messages });
    }

    startLoading(message: string = "Loading ...") {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopLoading() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
}