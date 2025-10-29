
declare var window: any;
import { Component, OnInit, EventEmitter, QueryList, ViewChildren } from '@angular/core';
import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { SharedLogisticsService } from '../Services/Others/SharedLogisticsService';
import { DocumentTypePMExtendedService } from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import { DocumentTypePM } from '../../Common/EntityPMs/DocumentTypePM';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentPermissiosViewModel } from './ViewModel/DocumentPermissiosViewModel';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'SharedLogisticsDocumentPermissios',
    templateUrl: './SharedLogisticsDocumentPermissiosComponent.html',
    inputs: ['OnCloseWindowEvent'],
    providers: [DocumentTypePMExtendedService],
})

export class SharedLogisticsDocumentPermissiosComponent implements OnInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private SharedDocumentPage: any = null;

    public DocumentPermissiosSelectedViewModel: any;
    myTenantZeroList: DocumentTypePM[];
    myTenantList: DocumentTypePM[];
    DocumentPermissiosLists: DocumentPermissiosViewModel[];
    DocumentUploadPermissiosLists: DocumentPermissiosViewModel[];
    OnCloseWindowEvent = new EventEmitter();
    ObjectTableId: string;
    FullComponentsVisibility: boolean = false;
    SelectedTabCode: string;
    mySearchText: string;
    MainMessage: string;
    HasAgentDocumentsPermission: boolean = false;
    IsCloud: boolean = false;
    IsDigitalPortal: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypePMExtendedService: DocumentTypePMExtendedService) {
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit() {
        this.IsCloud = ObjectsLocator.GlobalSetting?.WorkEnvironment == "cloud";
        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();
        });
        this.Run();
    }

    Run() {
        this.SelectedTabCode = "CUV";
        this.MainMessage = "";
        if (FeatureLocator.HasFeaturePermession("General", "AGENTDOCUMENTSPERMISSION")) {
            this.HasAgentDocumentsPermission = true;
            this.MainMessage += "In this screen you choose which documents can be viewed by the customer and which by the agent.";
        }
        else {
            this.MainMessage += "In this screen you choose which documents can be viewed by the customer.";
        }
        this.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Shipment")[0].Id;
        this.LoadTenantZeroDate();

        if (this.IsDigitalPortal)
            this.HasAgentDocumentsPermission = false;
    }

    SelectedTabChange(selectedTabCode) {
        this.SelectedTabCode = selectedTabCode;
        if (!this.IsCloud) this.LoadEventCreationResultComponent();
    }



    SetWindowArgs(args: any) {
        this.FullComponentsVisibility = true;
        this.IsDigitalPortal = args.IsDigitalPortal;
    }

    LoadTenantZeroDate() {
        this.myTenantZeroList = [];
        this._documentTypePMExtendedService.GetDocumentTypesByObjectTableAndTenant(this.ObjectTableId, 0).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantZeroList = pmResponse.Result;
                this.LoadTenantData();
            }
            else this.CurrentSession.StopBusyIndicator();
        });
    }

    LoadTenantData() {
        this.myTenantList = [];
        this.DocumentUploadPermissiosLists = [];

        this._documentTypePMExtendedService.GetDocumentTypesByObjectTableAndTenant(this.ObjectTableId, SessionLocator.Tenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantList = pmResponse.Result;
                
                this.BuildData();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    BuildData() {
        var myList: DocumentTypePM[] = [];
        this.DocumentPermissiosLists = [];
        this.DocumentUploadPermissiosLists = [];
        var documentUploadPermissiosLists = [];

        if (!this.mySearchText) {
            myList = this.myTenantList;
            documentUploadPermissiosLists = this.myTenantList.filter(a => a.IsDocIn && !a.InActive);
        }
        else {
            myList = this.myTenantList.filter(d => (d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.Name && d.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
            documentUploadPermissiosLists = this.myTenantList.filter(d => d.IsDocIn && !d.InActive &&
                                                                                    ((d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1)
                                                                                    ||
                                                                                    (d.Name && d.Name.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1)));

        }

        myList = this.SortItemSource(myList);
        documentUploadPermissiosLists = this.SortItemSource(documentUploadPermissiosLists);
        myList.forEach((item) => {
            var tenantZeroItem = this.myTenantZeroList.filter(t => t.Code == item.Code)[0];
            if (tenantZeroItem != null) {
                this.DocumentPermissiosLists.push(new DocumentPermissiosViewModel(tenantZeroItem, item));
            }
        });

        documentUploadPermissiosLists.forEach((item) => {
            var _tenantZeroItem = this.myTenantZeroList.filter(t => t.Code == item.Code)[0];
            if (_tenantZeroItem != null) {
                this.DocumentUploadPermissiosLists.push(new DocumentPermissiosViewModel(_tenantZeroItem, item));
            }
        });
    }

    SortItemSource(items: any) {
        items.sort((a, b) => {
            if (a.Name && a.Name.toLowerCase() < b.Name.toLowerCase()) {
                return -1;
            }
            else if (a.Name && a.Name.toLowerCase() > b.Name.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });

        return items;
    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myTenantList = [];
        this.DocumentPermissiosLists.forEach((item) => {
            if (item.entityPM.IsDirty) {
                this.myTenantList.push(item.entityPM);
            }
        });

        this.DocumentUploadPermissiosLists.forEach((item) => {
            if (item.entityPM.IsDirty) {
                this.myTenantList.push(item.entityPM);
            }
        });

        if (this.myTenantList.length > 0) {
            this._documentTypePMExtendedService.update(this.myTenantList).subscribe((res: any) => {
                this.SaveSharedDocumentPage();
            });
        }
        else {
            this.SaveSharedDocumentPage();
        }
    }

    private SaveSharedDocumentPage() {
        if (this.SharedDocumentPage) this.SharedDocumentPage.SaveButtonClicked(this.myTenantList);
        else this.CloseButtonClicked();
    }

    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
        if (this.SharedDocumentPage) this.SharedDocumentPage.onSearchTextChangeEvent(searchText);
    }

    SharedDocumentsPermissionsComponentLoaded: boolean;
    LoadEventCreationResultComponent() {
        if (!this.AllLocations) return;

        let myGeneratedComponentLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == 'AGV')[0];
        if (myGeneratedComponentLocation == null) {
            return;
        }
        if (this.SharedDocumentsPermissionsComponentLoaded) return;
        this.SharedDocumentsPermissionsComponentLoaded = true;
        SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent', myGeneratedComponentLocation.viewContainerRef)
            .then(cmpRef => {
                this.SharedDocumentPage = cmpRef.instance;
                this.SharedDocumentPage.FullComponentsVisibility = false;
                this.SharedDocumentPage.FromAgentView = true;
            });

    }

}
