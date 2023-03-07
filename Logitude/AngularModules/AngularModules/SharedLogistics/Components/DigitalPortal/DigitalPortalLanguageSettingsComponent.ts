declare var window: any;
import { Component, OnInit, EventEmitter, QueryList, ViewChildren } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SharedLogisticsService } from '../../Services/Others/SharedLogisticsService';
import { DocumentTypePM } from '../../../Common/EntityPMs/DocumentTypePM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentPermissiosViewModel } from '../ViewModel/DocumentPermissiosViewModel';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'DigitalPortalLanguageSettings',
    templateUrl: './DigitalPortalLanguageSettingsComponent.html',
    inputs: ['OnCloseWindowEvent'],
})

export class DigitalPortalLanguageSettingsComponent implements OnInit {
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
    constructor() {
        // this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit() {
        this.IsCloud = ObjectsLocator.GlobalSetting.WorkEnvironment == "cloud";
        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();
        });
        this.Run();
    }

    Run() {
        this.SelectedTabCode = "ETV";
    }

    SelectedTabChange(selectedTabCode) {
        this.SelectedTabCode = selectedTabCode;
        // if (!this.IsCloud) this.LoadEventCreationResultComponent();
    }



    // SetWindowArgs(args: any) {
    //     this.FullComponentsVisibility = true;
    //     this.IsDigitalPortal = args.IsDigitalPortal;
    // }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        // this.CurrentSession.StartBusyIndicatorSaving();
    }

    // SharedDocumentsPermissionsComponentLoaded: boolean;
    // LoadEventCreationResultComponent() {
    //     if (!this.AllLocations) return;

    //     let myGeneratedComponentLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == 'AGV')[0];
    //     if (myGeneratedComponentLocation == null) {
    //         return;
    //     }
    //     if (this.SharedDocumentsPermissionsComponentLoaded) return;
    //     this.SharedDocumentsPermissionsComponentLoaded = true;
    //     SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent', myGeneratedComponentLocation.viewContainerRef)
    //         .then(cmpRef => {
    //             this.SharedDocumentPage = cmpRef.instance;
    //             this.SharedDocumentPage.FullComponentsVisibility = false;
    //             this.SharedDocumentPage.FromAgentView = true;
    //         });

    // }

}

