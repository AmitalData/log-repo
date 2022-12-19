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
    public ValidVersion: WorkFlowVersionPM = null;
    public CurrentVersionId: string = null;
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
    private TabSelectedEvent: any = null;

    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.setValidVersion(this.getValidVersion())
        this.Listen()
    }

    ngOnInit() {
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    private Listen() {
        //tab changed || clicked version row
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "WFFB") {
                    var clickedRowId = this.entityArgs.EditComponentArgument?.ClickedVersionRow!
                    if (clickedRowId && this.CurrentVersionId != clickedRowId) {
                        var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == clickedRowId);
                        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
                        this.setValidVersion(version);
                    }else{
                        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
                        this.setValidVersion(null)
                    }
                }
            });
        }

        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "WorkflowVersionsUpdated") {
                        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, ClickedVersionRow: null }
                        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, HasChanges: false }
                        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
                        this.setValidVersion(null)
                    }
                }
            );
        }
    }

    setValidVersion(version: WorkFlowVersionPM | null) {
        if (version) {
            this.ValidVersion = version;
            this.setCurrentDisplayedVersion(version.Id)
            this.loadWorkflow(false);
        } else {
            this.loadWorkflow(true);
        }
    }

    setCurrentDisplayedVersion(versionId: string) {
        this.CurrentVersionId = versionId
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, CurrentDisplayedVersionId: versionId }
    }

    loadWorkflow(setVersion: boolean) {
        if (this.EntityPM) {
            this.startBusyIndicator("Loading ...");
            this.WorkFlowPMService.get(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => { this.handleGetWorkflowResponse(serviceResponse, setVersion); });
        }
    }

    handleGetWorkflowResponse(serviceResponse: ServiceResponse, setVersion: boolean) {
        this.EntityPM.WorkFlowVersions = serviceResponse.Result.WorkFlowVersions
        if (setVersion) {
            this.ValidVersion = this.getValidVersion();
            this.setCurrentDisplayedVersion(this.ValidVersion.Id)
        }
        this.WorkflowName = this.EntityPM.Name;
        this.WorkflowEntity = this.ValidVersion.Entity;
        this.renderReactFlowModeler();
        this.loadObjectFields();
        this.entityArgs.SendMessage("RefreshWorkflowShortTitle");
        this.entityArgs.SendMessage("RefreshWorkflowButtons");
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

    flowChangedEvent(event: any) {
        if (event && event.status === "success") {
            var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == this.CurrentVersionId)
            if (version.StatusCode != "ACVE") {
                this.HasChanges = true;
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
            if (version.StatusCode != "ACVE") {
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
        propertiesWindow.Title = ("Configure " + openPropertiesEventObject.nodeLabel);
        propertiesWindow.WindowArgs = propertiesWindowArgs;
        return propertiesWindow;
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
