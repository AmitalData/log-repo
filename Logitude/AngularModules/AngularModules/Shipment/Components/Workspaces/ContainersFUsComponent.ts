import { Component, OnInit, Output, EventEmitter } from '@angular/core';
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


@Component({

    templateUrl: './ContainersFUsComponent.html',
})

export class ContainersFUsComponent implements OnInit {

    onUserQueriesBackComplete(arg: any) { }

    private myDomainService: ContainersFUDomainService;
    @Output() ReloadUserQueries = new EventEmitter();
    public IsResourcesReady: boolean = false;
    public BackButtonTitle: string;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(private _entityResourceService: EntityResourceService) {
        this.myDomainService = new ContainersFUDomainService();
        this.BackButtonTitle = TextCodeTranslator.Translate("General.MH.ContainersFU");
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Container", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("ContainerFollowUp", 0).subscribe((response: any) => {
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
    }

    LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.ReloadUsersQuery();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    public IsContainersToggleFeatureUp: boolean = false;
    private SetContainersQueriesVisibility() {
        this.IsContainersToggleFeatureUp = false;
        var isOceanInsightsContainersFeatureToggleUp: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OIC")[0];
        if (isOceanInsightsContainersFeatureToggleUp && (this.IsQueryVisible_AllContainers || this.IsQueryVisible_ClosedContainers || this.IsQueryVisible_Containers || this.IsQueryVisible_CancelledContainers)) {
            this.IsContainersToggleFeatureUp = true;
        }
    }

    public IsQueryVisible_InTransit: boolean = false;
    public IsQueryVisible_ArrivedNotDelivered: boolean = false;
    public IsQueryVisible_DeliveredNotReturned: boolean = false;
    public IsQueryVisible_MyViewsGroup: boolean = false;
    public IsQueryVisible_AllContainers: boolean = false;
    public IsQueryVisible_ClosedContainers: boolean = false;
    public IsQueryVisible_Containers: boolean = false;
    public IsQueryVisible_CancelledContainers: boolean = false;
    private SetQueriesVisibility() {
        this.IsQueryVisible_InTransit = FeatureLocator.HasFeaturePermession("ContainerFollowUp", "InTransit") ? true : false;
        this.IsQueryVisible_ArrivedNotDelivered = FeatureLocator.HasFeaturePermession("ContainerFollowUp", "ArrivedNotDelivered") ? true : false;
        this.IsQueryVisible_DeliveredNotReturned = FeatureLocator.HasFeaturePermession("ContainerFollowUp", "DeliveredNotReturned") ? true : false;
        this.IsQueryVisible_MyViewsGroup = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
        this.IsQueryVisible_AllContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.AllContainers") ? true : false;
        this.IsQueryVisible_ClosedContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.ClosedContainers") ? true : false;
        this.IsQueryVisible_Containers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.Containers") ? true : false;
        this.IsQueryVisible_CancelledContainers = FeatureLocator.HasFeaturePermession("Container", "Container.Q.CancelledContainers") ? true : false;
    }

    public InTransit: string;
    public ArrivedNotDelivered: string;
    public DeliveredNotReturned: string;
    public ContainersCount: string;
    LoadQueriesCounts() {
        this.myDomainService.GetQueriesCounts().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult: ContainersFUSummary = myResponse.Result;

                    if (myResult != null) {
                        this.InTransit = myResult.InTransit > 1000 ? "1000+" : myResult.InTransit.toString();
                        this.ArrivedNotDelivered = myResult.ArrivedNotDelivered > 1000 ? "1000+" : myResult.ArrivedNotDelivered.toString();
                        this.DeliveredNotReturned = myResult.DeliveredNotReturned > 1000 ? "1000+" : myResult.DeliveredNotReturned.toString();
                        this.ContainersCount = myResult.ContainersCount > 10000 ? "10000+" : myResult.ContainersCount.toString();
                    }
                }
            }
        });
    }

    //filterAgrs: ApiQueryFilters;
    ViewQuery(myQueryCode: string) {
        if (myQueryCode != null) {
            var queryCode = myQueryCode;
            var objectTableName = "ContainerFollowUp";
            switch (myQueryCode) {
                case "ArrivedNotDelivered": {
                    ServiceLocator.SendTotangoUserActivity("Container F/U", "Arrived Not Delivered View");
                    break;
                }
                case "DeliveredNotReturned": {
                    ServiceLocator.SendTotangoUserActivity("Container F/U", "Delivered Not Returned View");
                    break;
                }
                case "InTransit": {
                    ServiceLocator.SendTotangoUserActivity("Container F/U", "In Transit View");
                    break;
                }
                case "All Containers": {
                    ServiceLocator.SendTotangoUserActivity("Container", "All Containers");
                    objectTableName = "Container";
                    break;
                }
                case "Containers": {
                    ServiceLocator.SendTotangoUserActivity("Container", "Containers");
                    objectTableName = "Container";
                    break;
                }
                case "Cancelled Containers": {
                    ServiceLocator.SendTotangoUserActivity("Container", "Cancelled Containers");
                    objectTableName = "Container";
                    break;
                }
                case "Closed Containers":
                    {
                        ServiceLocator.SendTotangoUserActivity("Container", "Closed Containers");
                        objectTableName = "Container";
                        break;
                    }
            }


            //var MethodName = null;
            //var displayTitle = null;
            //var backButtonTitle = TextCodeTranslator.Translate("General.MH.ContainersFU");
            //this.filterAgrs = new ApiQueryFilters();

            var listArgs = new ListComponentArgs();
            //listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName;
            //listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = this.BackButtonTitle;
            //listArgs.MethodName = MethodName;

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
