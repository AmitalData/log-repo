import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../EntityPMs/TicketPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {CustomerListService} from '../../../Common/Services/StandardLists/CustomerListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMTool} from '../../Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: "TicketShortTitleComponent.html",
})

export class TicketShortTitleComponent {
    public EntityPM: TicketPM;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildComponent();
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    }

    public RankCode: string;
    public RankName: string;
    public RankSource1: string;
    public RankSource2: string;
    public RankSource3: string;
    public IsRankVisible: boolean = false;
    private BuildComponent() {
        this.RankName = this.EntityPM.RankName;

        if (this.RankName != null) {

            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "gold": {
                    this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "platinum": {
                    this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }

            this.IsRankVisible = true;
        }
    }

    get TicketNumber() {
        return this.EntityPM.TicketNumber;
    }

    get CompanyName() {
        return this.EntityPM.CompanyName;
    }

    get IsCancelled() { return this.EntityPM.IsCancelled; }

    get IsClosed() { return this.EntityPM.IsClosed; }

    public ViewCustomerMethod() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CompanyId)) {
            var objectTable = this.EntityPM.CompanyTableName;

            if (objectTable.toLowerCase() == "shipping line") {
                objectTable = "ShippingLine";
            }
            else if (objectTable.toLowerCase() == "shipping agent") {
                objectTable = "ShippingAgent";
            }
            else if (objectTable.toLowerCase() == "custom agent") {
                objectTable = "CustomAgent";
            }
            else if (objectTable.toLocaleLowerCase() == "potential customer") {
                objectTable = "Customer";
            }

            //this._entityResourceService.getEntityResourceByTableName(objectTable, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityPM.CompanyId, ObjectTableName: objectTable, BackButtonLabel: "Ticket" });
                    });
            //});
        }
    }

    private EditBlockedCustomer(customerList: CustomerList) {
        var windowTitle = "View Customer";
        var windowArgs: any = {};
        windowArgs.CustomerList = customerList;
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
    }
}
