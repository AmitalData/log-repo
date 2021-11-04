import { BaseComponent } from "../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Component, OnChanges, AfterViewInit } from "@angular/core";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { SessionInfo } from "../../Infrastructure/Utilities/SessionInfo";
import { SessionLocator } from "../../Infrastructure/Utilities/SessionLocator";
import { ShipmentPMService } from "../../Shipment/Services/StandardPMs/ShipmentPMService";
import TasksList from "collaboration-tool-tasks-list";

@Component({
    selector: "tasks-list",
    template: "<div [id]='rootId' class='prime-web-component'></div>"
})

export class TasksAppComponent extends BaseComponent implements OnChanges, AfterViewInit {

    public rootId = "tasks-list-root";
    private hasViewLoaded = false;

    constructor() {
        super();
    }

    public ngOnChanges() {
        this.renderComponent();
    }

    public ngAfterViewInit() {
        this.hasViewLoaded = true;
        this.renderComponent();
    }

    private renderComponent() {
        if (!this.hasViewLoaded) {
            return;
        }

        const props: any = {
            logitudeAuthentication: { Tenant: SessionLocator.Tenant, Token: SessionInfo.Token },
            enableUncLink: true,
            uncLinkClickCallback: this.UNCNumberClicked
        };

        ReactDOM.render(
            React.createElement(TasksList, props),
            document.getElementById(this.rootId)
        );
    }

    private UNCNumberClicked(entityNumber: string) {
        var ShipmentNumber = entityNumber;
        var _ShipmentPMService = new ShipmentPMService();
        var CurrentSession = SessionLocator.SelectedSession;
        _ShipmentPMService.getSingleByShipmentNumber(ShipmentNumber).subscribe((myResult: any) => {
            if (!myResult.HasError) {
                var backLabel = "Tasks";
                var myEntityId = myResult.Result;
                SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: myEntityId, ObjectTableName: "Shipment", BackButtonLabel: backLabel });
                    });
            }
        });
    }
}