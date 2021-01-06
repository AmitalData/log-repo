import { Component, OnDestroy, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationReferantDataWebService } from '../../../../Customs/Services/WebServices/DeclarationReferantDataWebService';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    templateUrl: './ReferantWorkspaceComponent.html',
    providers: [DeclarationReferantDataWebService],
})

export class ReferantWorkspaceComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();

    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public counters: any;

    // Queries Features
    public FilesInProcessVisibility: boolean = true;
    public TrackingCasesVisibility: boolean = true;
    public FilesInOCRVisibility: boolean = true;
    public FilesInSivugVisibility: boolean = true;
    public FilesInReviewVisibility: boolean = true;
    public FilesInCreditControlVisibility: boolean = true;
    public FilesAvailableFreeOfChargeVisibility: boolean = true;
    public AllCasesVisibility: boolean = true;
    public isRTL: boolean = false;

    ngAfterViewInit(): void {
    }
    constructor(public _declarationReferantDataWebService: DeclarationReferantDataWebService) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationReferantData").subscribe((response: any) => {
            _declarationReferantDataWebService.GetQueriesCounts().subscribe(
                (data: any) => {
                    this.counters = data.Result;
                    this.isScreenLoaded = true;
                    this.CurrentSession.StopBusyIndicator();
                });
        });
    }
    FilterChange($eevnt) {
    }
    ViewReferantQuery(myQueryCode: string) {
        if (myQueryCode != null) {
            var displayTitle = "";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "FilesInProcess":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInProcess");
                        break;
                    }
                case "TrackingCases":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.TrackingCases");
                        break;
                    }
                case "FilesInOCR":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInOCR");
                        break;
                    }
                case "FilesInSivug":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInSivug");
                        break;
                    }
                case "FilesInReview":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInReview");
                        break;
                    }
                case "FilesInCreditControl":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInCreditControl");
                        break;
                    }
                case "FilesAvailableFreeOfCharge":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesAvailableFreeOfCharge");
                        break;
                    }
                case "AllCases":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.AllCases");
                        break;
                    }
                default: { break;}
            }
            this.BuildFiltersForQuery(filters);
            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Customs.DeclarationReferantData";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.ReferantWorkspace");
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.LoadAllScreenData();
                            this._declarationReferantDataWebService.GetQueriesCounts().subscribe(
                                (data: any) => {
                                    this.counters = data.Result;
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                });
                        });
                    });
            });
        }
    }

    public LoadAllScreenData() {
        this.ReloadUsersQuery();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }
    BuildFiltersForQuery(filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
    }

}

