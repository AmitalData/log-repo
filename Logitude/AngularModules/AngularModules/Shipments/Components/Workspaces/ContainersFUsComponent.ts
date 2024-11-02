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
    public IsContainersFeatureActivated: boolean = false;
    private SetContainersQueriesVisibility() {
        this.IsContainersToggleFeatureUp = false;
        this.IsContainersFeatureActivated = false;

        

        if (FeatureLocator.HasFeaturePermession("Container", "ContainersActivated")) {
            this.IsContainersFeatureActivated = true;
        }
    }

    public IsQueryVisible_InTransit: boolean = false;
    public IsQueryVisible_ArrivedNotDelivered: boolean = false;
    public IsQueryVisible_DeliveredNotReturned: boolean = false;
   
    private SetQueriesVisibility() {
        this.IsQueryVisible_InTransit = FeatureLocator.HasFeaturePermession("ContainerFollowUp", "InTransit") ? true : false;
        this.IsQueryVisible_ArrivedNotDelivered = FeatureLocator.HasFeaturePermession("ContainerFollowUp", "ArrivedNotDelivered") ? true : false;
        this.IsQueryVisible_DeliveredNotReturned = FeatureLocator.HasFeaturePermession("ContainerFollowUp", "DeliveredNotReturned") ? true : false;
        

    }

    public InTransit: string;
    public ArrivedNotDelivered: string;
    public DeliveredNotReturned: string;
    LoadQueriesCounts() {
        this.myDomainService.GetQueriesCounts().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult: ContainersFUSummary = myResponse.Result;

                    if (myResult != null) {
                        this.InTransit = myResult.InTransit > 1000 ? "1000+" : myResult.InTransit.toString();
                        this.ArrivedNotDelivered = myResult.ArrivedNotDelivered > 1000 ? "1000+" : myResult.ArrivedNotDelivered.toString();
                        this.DeliveredNotReturned = myResult.DeliveredNotReturned > 1000 ? "1000+" : myResult.DeliveredNotReturned.toString();
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
