// import { Component, OnInit } from "@angular/core";
// import { BaseComponent } from "../../Infrastructure/Components/LogitudeComponents/BaseComponent";
// import { SessionLocator } from "../../Infrastructure/Utilities/SessionLocator";
// import { ShipmentPMService } from "../../Shipment/Services/StandardPMs/ShipmentPMService";
// @Component({
//     templateUrl: "./TasksAppComponent.html"
// })
// export class TasksAppComponent extends BaseComponent implements OnInit {
//     private CurrentSession = SessionLocator.SelectedSession;
//     public _ShipmentPMService: ShipmentPMService;
//     myUser: User;
//     constructor() {
//         super(); 
//         this._ShipmentPMService = new ShipmentPMService();
//     }
//     ngOnInit() {
//         var user = new User();
//         user.Email = SessionLocator.LoggedUserPM.Email;
//         user.Tenant = SessionLocator.Tenant;
//         this.myUser = user;
//     }

//     openShipmentEditScreen(event) {
//         var ShipmentNumber = event.detail;
//         this._ShipmentPMService.getSingleByShipmentNumber(ShipmentNumber).subscribe((myResult: any) => {
//             if (!myResult.HasError) {
//                 var backLabel = "Tasks";// + this.fatherComponent.EntityPM.QuoteNumber;
//                 var myEntityId = myResult.Result;
//                 SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef)
//                     .then(cmpRef => {
//                         cmpRef.instance.ComponentRef = cmpRef;
//                         cmpRef.instance.Run({ EntityId: myEntityId, ObjectTableName: "Shipment", BackButtonLabel: backLabel });
//                         let isEditComponentSaved = false;
//                         //cmpRef.instance.BackCompleted.subscribe(bk => {
//                         //    if (isEditComponentSaved) {
//                         //        //this.fatherComponent.entityArgs.EditComponent.ReloadEntityPM();
//                         //    }
//                         //});
//                         //cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
//                         //    if (isSaveSuccess) {
//                         //        isEditComponentSaved = true;
//                         //    }
//                         //});
//                     });
//             }
//         });
//     }
// }

// export class User {
//     Id: number;
//     Tenant: number;
//     FirstName: string;
//     LastName: string;
//     Email: string;
//     Password: string;
// }



import { BaseComponent } from "../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { Component, OnChanges, AfterViewInit } from "@angular/core";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { LoggedUser } from "collaboration-tool-core";
import TasksList from "collaboration-tool-tasks-list";

@Component({
    selector: "tasks-list",
    template: "<div [id]='rootId' class='prime-web-component'></div>"
})
export class TasksAppComponent extends BaseComponent implements OnChanges, AfterViewInit {

    public rootId = "tasks-list-root";
    private hasViewLoaded = false;

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
            loggedUserEmail: LoggedUser.getEmail()
        };

        ReactDOM.render(
            React.createElement(TasksList, props),
            document.getElementById(this.rootId)
        );
    }
}