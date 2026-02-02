import { EventEmitter, Injectable, isDevMode, OnDestroy } from "@angular/core";
import { SearchIndexEditHistoryPM } from "Common/EntityPMs/SearchIndexEditHistoryPM";
import { SearchIndexEditHistoryPMService } from "Common/Services/StandardPMs/SearchIndexEditHistoryPMService";
import { AzureSearchWebService, FastSearchResult, FastSearchSettings } from "Customs/Services/WebServices/AzureSearchWebService";
import { ApiQueryFilters, FilterItem } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { SearchListDDLComponent } from "Infrastructure/Directives/SearchListDDL/SearchListDDLComponent";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { EntityListService } from "Infrastructure/Services/EntityListService";
import { FeatureLocator } from "Infrastructure/Utilities/FeatureLocator";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { BehaviorSubject, Observable } from "rxjs";
import { AppTool } from "Infrastructure/Tools";


@Injectable()
export class FastSearchService implements OnDestroy {
    orginalCurrentAdditionalFilters: FilterItem[] = null;
    private readonly azureSearchWebService: AzureSearchWebService = new AzureSearchWebService();
    private readonly searchIndexEditHistoryPMService: SearchIndexEditHistoryPMService = new SearchIndexEditHistoryPMService();
    private indexName: string = null;
    private objectTableName: string = null;
    private menuTableQuerySection: string = null;
    private subscriptions: any[] = [];
    public $fastSearchEnable: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    private $recentSearches: BehaviorSubject<FastSearchResult[]> = new BehaviorSubject<FastSearchResult[]>([]);
    private readonly azureSearchService = new AzureSearchWebService();

    public get $RecentSearches(): Observable<FastSearchResult[]> {
        return this.$recentSearches.asObservable();
    }
    private settings: FastSearchSettings | null = null;
    public get Settings() {
        return this.settings;
    }

    constructor(private _entityListService: EntityListService) { }

    ngOnDestroy(): void {
        this.$fastSearchEnable.complete();
        this.$recentSearches.complete();
        this.subscriptions.forEach(subscription => subscription.unsubscribe());
        this.subscriptions = [];
    }

    public async search(currentQueryFilters: ApiQueryFilters, searchText: string): Promise<FastSearchResult[]> {
        if (searchText?.length >= this.Settings.minimumSearchQueryLength) {
            this.orginalCurrentAdditionalFilters = [...currentQueryFilters.AdditionalFilters];
            return await this.azureSearchWebService.fastSearch(currentQueryFilters, searchText, this.indexName)

        } else if (this.orginalCurrentAdditionalFilters != null)
            currentQueryFilters.AdditionalFilters = this.orginalCurrentAdditionalFilters;
    }

    private resetData() {
        this.orginalCurrentAdditionalFilters = null;
        this.indexName = null;
        this.objectTableName = null;
        this.menuTableQuerySection = null;
        this.settings = null;
        this.$fastSearchEnable.next(false);
        this.$recentSearches.next([]);
    }

    public async initFastSearch(objectTable: ObjectTablePM, objectTableName: string, menuTableQuerySection: string,ShowFastSearch:boolean = false, index :string = null): Promise<void> {
        this.resetData();

        const havePermission = FeatureLocator.HasFeaturePermession("General", "FASTSEARCH") || isDevMode();
        const enabled = objectTable.ShowFastSearch || ShowFastSearch;
        if (!havePermission || !enabled) return;

        this.menuTableQuerySection = menuTableQuerySection;
        this.objectTableName = objectTableName;
        let indexName: string = !AppTool.IsNullOrEmpty(index)? index : objectTableName.replace('Customs.', '').toLowerCase();
        if (!indexName.endsWith('s'))
            indexName += 's';

        this.indexName = indexName;
        if (menuTableQuerySection == "Customs.Declaration")
            indexName = 'declarations';
        else if (menuTableQuerySection == "Customs.ExportDeclaration")
            indexName = 'exportDeclarations';
        
        const settings: FastSearchSettings = await this.azureSearchWebService.getSettings(indexName);
        this.settings = settings;
        this.$fastSearchEnable.next(true);
    }

    public subscribeMenuHeaderchangeevent(menuHeaderchangeevent: EventEmitter<unknown>) {
        this.subscriptions.push(menuHeaderchangeevent.subscribe(() => this.orginalCurrentAdditionalFilters = null));
    }

    public searchDropdownSelected(optionSelected: FastSearchResult | string, currentQueryFilters: ApiQueryFilters, selectedQueryCode: string, searchDropdownOptions: FastSearchResult[], methodName: string, onRowSelected: (rowData: any) => void, onQueryChangeEvent: any) {
        if (optionSelected === SearchListDDLComponent.showAll) {
            const ids: string[] = searchDropdownOptions.map((x: FastSearchResult) => x.id);
            ids.pop();
            currentQueryFilters.AdditionalFilters = [];
            currentQueryFilters.addAdditionalFilter("Id", ids.join(','), null, null, "InListExact", false, true, false, "String");
            onQueryChangeEvent.emit({ QueryCode: selectedQueryCode, Filters: currentQueryFilters, SearchFieldChanged: true, Reload: true });
        } else {
            this._entityListService.getSingle((optionSelected as FastSearchResult).id, this.objectTableName, methodName == undefined ? null : methodName).then((res: any) => {
                res.subscribe(myResponse => {
                    if (myResponse != null)
                        onRowSelected({ rowData: myResponse instanceof ServiceResponse ? myResponse.Result : myResponse });
                });
            });

            this.AddHistorySearch(optionSelected);
        }
    }

    public async AddHistorySearch(optionSelected: string | FastSearchResult, id: string = null) {
        if (id) 
            optionSelected = await this.SearchById(id);

        const searchIndexEditHistoryPM: SearchIndexEditHistoryPM = new SearchIndexEditHistoryPM();
        searchIndexEditHistoryPM.Entname = this.getEntname();
        searchIndexEditHistoryPM.Screen = this.indexName;
        searchIndexEditHistoryPM.KeyVal = JSON.stringify(optionSelected);
        searchIndexEditHistoryPM.Tenant = SessionLocator.Tenant;
        this.searchIndexEditHistoryPMService.insert(searchIndexEditHistoryPM).subscribe();
    }

    private async SearchById(id: string): Promise<FastSearchResult> {
        const filter = new ApiQueryFilters();
        filter.addAdditionalFilter("Id", id, null, null, "Equal", false, true, false, "string");
        if (this.menuTableQuerySection == "Customs.ExportDeclaration")
            filter.addAdditionalFilter("Direction", 'E', null, null, "Equal", false, true, false, "string");

        const searchRes = await this.azureSearchWebService.fastSearch(filter, '', this.indexName)
     
        if (searchRes.length != 1)
            throw new Error("Search result is not 1, id:" + id + ", searchRes: " + JSON.stringify(searchRes));
     
        return searchRes[0];
    }

    public async getRecentSearches(): Promise<FastSearchResult[]> {
        const Entname = this.getEntname();
        const recentSearches: FastSearchResult[] = await this.azureSearchService.getRecentSearches<FastSearchResult>(this.settings.recentEditScreen, Entname, this.settings.recentShowTopResults);
        this.$recentSearches.next(recentSearches);
        return recentSearches;
    }

    private getEntname(): string {
        return this.menuTableQuerySection == "Customs.ExportDeclaration" ? this.menuTableQuerySection : this.objectTableName;
    }

}