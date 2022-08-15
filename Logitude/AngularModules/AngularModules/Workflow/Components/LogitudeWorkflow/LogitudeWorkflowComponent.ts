import { AfterViewInit, Component, ComponentRef, ElementRef, OnChanges, OnDestroy, ViewChild } from "@angular/core";
import { BaseComponent } from "../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import ReactFlowModeler from "logitude-workflow";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";

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

export class LogitudeWorkflowComponent extends BaseComponent implements OnChanges, AfterViewInit, OnDestroy {

    @ViewChild("logitudeWorkflowComponentContainer", { static: false }) containerRef: ElementRef;
    
    //@ViewChild("logitudeWorkflowComponentContainer", { read: ViewContainerRef, static: false }) containerRef: ViewContainerRef;


    public ComponentRef: ComponentRef<LogitudeWorkflowComponent>;

    returnPropertiesDataEventKey: string;

    constructor() {
        super();
    }

    ngOnChanges() {
        this.render();
    }

    ngAfterViewInit() {
        this.returnPropertiesDataEventKey = "returnPropertiesDataEventKey_" + (Date.now())?.toString();
        this.render();
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
    }

    render() {
        const props = {
            flow: null,
            openPropertiesEvent: this.openPropertiesEvent,
            exportFlowEvent: this.exportFlowEvent,
            returnPropertiesDataEventKey: this.returnPropertiesDataEventKey
        };
        ReactDOM.render(React.createElement(ReactFlowModeler, props), this.containerRef.nativeElement);
    }


    openPropertiesEvent = (openPropertiesEventObject: any) => {
        if (openPropertiesEventObject) {
            this.handleOpenPropertiesEvent(openPropertiesEventObject);
        }
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
            document.dispatchEvent(new CustomEvent(this.returnPropertiesDataEventKey, { detail: data }));
        }
    }

    exportFlowEvent = (flow: any) => {
        console.log(flow);
    }
}