import { AfterViewInit, Component, ElementRef, OnChanges, OnDestroy, ViewChild } from "@angular/core";
import { BaseComponent } from "../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import TasksList from "collaboration-tool-tasks-list";
import { SessionInfo } from "../../Infrastructure/Utilities/SessionInfo";
import { SessionLocator } from "../../Infrastructure/Utilities/SessionLocator";
import { ShipmentPMService } from "../../Shipment/Services/StandardPMs/ShipmentPMService";
import * as React from "react";
import * as ReactDOM from "react-dom";

const tasksAppComponentContainer = "TasksAppComponentContainer";

@Component({
    selector: "tasks-list",
    template: `<div #${tasksAppComponentContainer} class="prime-web-component"></div>`,
})

export class TasksAppComponent extends BaseComponent implements OnChanges, AfterViewInit, OnDestroy {
    @ViewChild(tasksAppComponentContainer, { static: false }) containerRef: ElementRef;

    constructor() {
        super();
    }

    ngOnChanges() {
        this.render();
    }

    ngAfterViewInit() {
        this.render();
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.containerRef.nativeElement);
    }

    private render() {
        const props: any = {
            logitudeAuthentication: { Tenant: SessionLocator.Tenant, Token: SessionInfo.Token },
            enableUncLink: true,
            uncLinkClickCallback: this.uncNumberClicked
        };

        ReactDOM.render(React.createElement(TasksList, props), this.containerRef.nativeElement);
    }

    private uncNumberClicked(entityNumber: string) {
        let shipmentPMService = new ShipmentPMService();
        let CurrentSession = SessionLocator.SelectedSession;
        shipmentPMService.getSingleByShipmentNumber(entityNumber).subscribe((getByShipmentNumberResult: any) => {
            if (!getByShipmentNumberResult.HasError) {
                let backButtonLabel = "Tasks";
                let entityId = getByShipmentNumberResult.Result;
                SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", CurrentSession.SessionLocation.viewContainerRef)
                    .then((cmpRef: any) => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: "Shipment", BackButtonLabel: backButtonLabel });
                    });
            }
        });
    }
}