import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ContainersFUDomainService, ContainersFUSummary } from '../../Services/ContainersFUDomainService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { FeatureToggleList } from '../../../Infrastructure/EntityLists/FeatureToggleList';
import { ContainerViewsGraphComponent } from '../Templates/ContainerViewsGraphComponent';


@Component({

    templateUrl: './ContainerComponent.html',
})

export class ContainerComponent implements OnInit {
    @ViewChild(ContainerViewsGraphComponent) ContainerViewsGraphComponent: ContainerViewsGraphComponent;
    onUserQueriesBackComplete(arg: any) { }

    private myDomainService: ContainersFUDomainService;
    @Output() ReloadUserQueries = new EventEmitter();
    public IsResourcesReady: boolean = false;
    public BackButtonTitle: string;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(private _entityResourceService: EntityResourceService) {
        this.myDomainService = new ContainersFUDomainService();
        this.BackButtonTitle = TextCodeTranslator.Translate("General.MH.Containers");
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Container", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Container", 0).subscribe((response: any) => {
                this.IsResourcesReady = true;
                this.LoadAllScreenData();
                this.SetQueriesVisibility();
                this.SetContainersQueriesVisibility();
            });
        });
    }

    EditShipment(entity: any) {

    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
        this.ContainerViewsGraphComponent.GetData();
    }

    LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.ReloadUsersQuery();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    public IsContainersToggleFeatureUp: boolean = false;
    public IsContainersFeatureActivated: boolean = false;
    private SetContainersQueriesVisibility() {
        this.IsContainersToggleFeatureUp = false;
        this.IsContainersFeatureActivated = false;

        var isOceanInsightsContainersFeatureToggleUp: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OIC")[0];
        if (isOceanInsightsContainersFeatureToggleUp && (this.IsQueryVisible_AllContainers || this.IsQueryVisible_ClosedContainers || this.IsQueryVisible_Containers || this.IsQueryVisible_CancelledContainers)) {
            this.IsContainersToggleFeatureUp = true;
        }

        if (FeatureLocator.HasFeaturePermession("Container", "ContainersActivated")) {
            this.IsContainersFeatureActivated = true;
        }
    }

    public IsQueryVisible_MyViewsGroup: boolean = false;
    public IsQueryVisible_AllContainers: boolean = false;
    public IsQueryVisible_ClosedContainers: boolean = false;
    public IsQueryVisible_Containers: boolean = false;
    public IsQueryVisible_CancelledContainers: boolean = false;
    public IsQueryVisible_PendingPOLDepartureView: boolean = false;
    public IsQueryVisible_InTransitNewView: boolean = false;
    public IsQueryVisible_InTransitTransshipmentsView: boolean = false;
    public IsQueryVisible_PendingGateOutView: boolean = false;
    public IsQueryVisible_PendingEmptyReturnView: boolean = false;
    public IsQueryVisible_ExceptionsView: boolean = false;
    public IsQueryVisible_PendingArrivalView: boolean = false;
    public IsQueryVisible_PendingDischargeView: boolean = false;
    public IsQueryVisible_PendingDeliveryView: boolean = false;
    public IsQueryVisible_PreviousTrackedContainers: boolean = false;
    private SetQueriesVisibility() {
        this.IsQueryVisible_MyViewsGroup = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        this.IsQueryVisible_AllContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.AllContainers") ? true : false;
        this.IsQueryVisible_ClosedContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.ClosedContainers") ? true : false;
        this.IsQueryVisible_Containers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.Containers") ? true : false;
        this.IsQueryVisible_CancelledContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.CancelledContainers") ? true : false;
        this.IsQueryVisible_PendingPOLDepartureView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PendingPOLDeparture") ? true : false;
        this.IsQueryVisible_InTransitNewView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.InTransitNew") ? true : false;
        this.IsQueryVisible_InTransitTransshipmentsView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.InTransitTransshipments") ? true : false;
        this.IsQueryVisible_PendingGateOutView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PendingGateOut") ? true : false;
        this.IsQueryVisible_PendingEmptyReturnView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PendingEmptyReturn") ? true : false;
        this.IsQueryVisible_ExceptionsView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.Exceptions") ? true : false;
        this.IsQueryVisible_PendingArrivalView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PendingArrival") ? true : false;
        this.IsQueryVisible_PendingDischargeView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PendingDischarge") ? true : false;
        this.IsQueryVisible_PendingDeliveryView = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PendingDelivery") ? true : false;
        this.IsQueryVisible_PreviousTrackedContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.PreviousTrackedContainers") ? true : false;
    }
    public ContainersCount: string;
    LoadQueriesCounts() {
        this.myDomainService.GetContainerQueriesCounts().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult: ContainersFUSummary = myResponse.Result;

                    if (myResult != null) {
                        this.ContainersCount = myResult.ContainersCount > 10000 ? "10000+" : myResult.ContainersCount.toString();
                    }
                }
            }
        });
    }


    ViewQuery(myQueryCode: string) {
        if (myQueryCode != null) {
            var queryCode = myQueryCode;
            var objectTableName = "Container";
            var queryName = "";
            switch (myQueryCode) {
                case "All Containers": {
                    queryName =  "All Containers";
                    break;
                }
                case "Containers": {
                    queryName = "Containers";
                    break;
                }
                case "Cancelled Containers": {
                    queryName = "Cancelled Containers";
                    break;
                }
                case "Closed Containers": {
                    queryName = "Closed Containers";
                    break;
                }

                case "PendingPOLDeparture": {
                    queryName = "Pending POL Departure";
                    break;
                }

                case "InTransitNew": {
                    queryName = "In Transit";
                    break;
                }

                case "InTransitTransshipments": {
                    queryName = "In Transit Transshipments";
                    break;
                }

                case "PendingGateOut": {
                    queryName = "Pending Gate Out";
                    break;
                }

                case "PendingEmptyReturn": {
                    queryName = "Gated Out not Empty Return";
                    break;
                }

                case "Exceptions": {
                    queryName = "Exceptions";
                    break;
                }
                case "PendingArrival": {
                    queryName = "Pending Arrival";
                    break;
                }
                case "PendingDischarge": {
                    queryName = "Pending Discharge";
                    break;
                }
                case "PendingDelivery": {
                    queryName = "Pending Delivery";
                    break;
                }
                case "PreviousTrackedContainers": {
                    queryName = "Previous Tracked Containers";
                    break;
                }
               
            }
            ServiceLocator.SendTotangoUserActivity("Container Views", queryName);
            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = this.BackButtonTitle;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {

                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.LoadAllScreenData()
                    });

                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }
}
