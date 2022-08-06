import { AfterViewInit, Component, ElementRef, OnChanges, OnDestroy, ViewChild, ViewEncapsulation } from "@angular/core";
import { BaseComponent } from "../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import ReactFlowModeler from "logitude-workflow";
import * as React from "react";
import * as ReactDOM from "react-dom";

const logitudeWorkflowComponentContainer = "LogitudeWorkflowComponentContainer";

@Component({
    selector: "logitude-workflow",
    template: `
    <style>
      .logitude-workflow-component-container {
        width: 100%;
        height: 100%;
      }
    </style>
    <div #${logitudeWorkflowComponentContainer} class="logitude-workflow-component-container"></div>
    `
})

export class LogitudeWorkflowComponent extends BaseComponent implements OnChanges, AfterViewInit, OnDestroy {
    @ViewChild(logitudeWorkflowComponentContainer, { static: false }) containerRef: ElementRef;

    private returnPropertiesDataEventKey: string;

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

    private render() {
        const props: any = {
            flow: null,
            openPropertiesEvent: this.openPropertiesEvent,
            exportFlowEvent: this.exportFlowEvent,
            returnPropertiesDataEventKey: this.returnPropertiesDataEventKey
        };

        ReactDOM.render(React.createElement(ReactFlowModeler, props), this.containerRef.nativeElement);
    }

    private openPropertiesEvent(eventObject: any) {
        if (eventObject) {
            let name = prompt((eventObject.isNewNode ? "New " : "Edit ") + eventObject.nodeLabel + " Element", (eventObject.nodeData?.name || ""));
            document.dispatchEvent(new CustomEvent(this.returnPropertiesDataEventKey, { detail: { name: (name || "") } }));
        }
    }

    private exportFlowEvent(flow: any) {
        console.log(flow);
    }
}