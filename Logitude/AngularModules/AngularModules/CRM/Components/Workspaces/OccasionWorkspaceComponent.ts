import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CRMDomainService, OccasionSummary } from '../../Services/CRMDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    moduleId: module.id,
    templateUrl: './OccasionWorkspaceComponent.html',
})

export class OccasionWorkspaceComponent {
    private myDomainService: CRMDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {

        this.myDomainService = new CRMDomainService();
        this.SetQueriesVisibility();
        this.LoadQueriesCounts();


    }

    public IsNewOccasionVisible = false;
    public AllOccasionsQueryVisibility: boolean = false;
    SetQueriesVisibility() {

        if (FeatureLocator.HasFeaturePermession("Occasion", "New")) {
            this.IsNewOccasionVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("Occasion", "Occasion.Q.AllOccasions")) {
            this.AllOccasionsQueryVisibility = true;
        }
    }

    NewOccasionClicked() {
        this._entityResourceService.getEntityResourceByTableName("OccasionType", 0).subscribe(response => {
            var windowTitle = "New Occasion";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 700;
            logWindow.Title = windowTitle;

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadQueriesCounts();
                }
            });

            logWindow.Show('./CRMModules/CRMOccasion/Components/NewEntity/NewOccasionComponent');
        });
    }


    public AllOccasionsCount: number = 0;
    LoadQueriesCounts() {
        this.myDomainService.GetOccasionsSummary().subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var myData: OccasionSummary = myResponse.Result;
                if (myData != null) {
                    this.AllOccasionsCount = myData.AllOccasionsCount;
                }
            }
        });
    }

    ViewQuery(code: string) {
        if (code) {
            var objectTableName = "Occasion";
            var queryCode = code;
            var displayTitle = "";
            var backButtonTitle = "CRM";

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();


            displayTitle = queryCode;

            var listArgs = new ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadQueriesCounts());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });;
        }
    }

}
